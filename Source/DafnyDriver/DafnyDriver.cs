//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
// Copyright by the contributors to the Dafny Project
// SPDX-License-Identifier: MIT
//
//-----------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------
// DafnyDriver
//       - main program for taking a Dafny program and verifying it
//---------------------------------------------------------------------------------------------

using DafnyServer.CounterexampleGeneration;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Microsoft.Dafny {
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Diagnostics.Contracts;
  using System.IO;
  using System.Linq;

  using Microsoft.Boogie;
  using Bpl = Microsoft.Boogie;
  using System.Diagnostics;

  public class DafnyDriver {

    // TODO: Refactor so that non-errors (NOT_VERIFIED, DONT_PROCESS_FILES) don't result in non-zero exit codes
    public enum ExitValue { SUCCESS = 0, PREPROCESSING_ERROR, DAFNY_ERROR, COMPILE_ERROR, VERIFICATION_ERROR }

    public static int Main(string[] args) {
      int ret = 0;
      var thread = new System.Threading.Thread(
        new System.Threading.ThreadStart(() => { ret = ThreadMain(args); }),
          0x10000000); // 256MB stack size to prevent stack overflow
      thread.Start();
      thread.Join();
      return ret;
    }

    public static int ThreadMain(string[] args) {
      Contract.Requires(cce.NonNullElements(args));

      ErrorReporter reporter = new ConsoleErrorReporter();

      var dafnyOptions = new DafnyOptions();
      DafnyOptions.Install(dafnyOptions);

      CommandLineArgumentsResult cliArgumentsResult = ProcessCommandLineArguments(args, out var dafnyFiles, out var otherFiles);
      ExitValue exitValue;
      switch (cliArgumentsResult) {
        case CommandLineArgumentsResult.OK:
          exitValue = ProcessFiles(dafnyOptions, dafnyFiles, otherFiles.AsReadOnly(), reporter);
          break;
        case CommandLineArgumentsResult.PREPROCESSING_ERROR:
          exitValue = ExitValue.PREPROCESSING_ERROR;
          break;
        case CommandLineArgumentsResult.OK_EXIT_EARLY:
          exitValue = ExitValue.SUCCESS;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      if (DafnyOptions.O.XmlSink != null) {
        DafnyOptions.O.XmlSink.Close();
        if (DafnyOptions.O.VerificationLoggerConfigs.Any()) {
          BoogieXmlConvertor.RaiseTestLoggerEvents(DafnyOptions.O.BoogieXmlFilename, DafnyOptions.O.VerificationLoggerConfigs);
        }
      }
      if (DafnyOptions.O.Wait) {
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
      }
      if (!DafnyOptions.O.CountVerificationErrors) {
        return 0;
      }
      //Console.ReadKey();
      return (int)exitValue;
    }

    public enum CommandLineArgumentsResult {
      /// Indicates that arguments were parsed successfully.
      OK,
      /// Indicates that arguments were not parsed successfully.
      PREPROCESSING_ERROR,
      /// Indicates that arguments were parsed successfully, but the program should exit without processing files.
      OK_EXIT_EARLY
    }

    public static CommandLineArgumentsResult ProcessCommandLineArguments(string[] args, out List<DafnyFile> dafnyFiles, out List<string> otherFiles) {
      dafnyFiles = new List<DafnyFile>();
      otherFiles = new List<string>();

      DafnyOptions.O.RunningBoogieFromCommandLine = true;
      try {
        if (!DafnyOptions.O.Parse(args)) {
          return CommandLineArgumentsResult.PREPROCESSING_ERROR;
        }
      } catch (ProverException pe) {
        DafnyOptions.O.Printer.ErrorWriteLine(Console.Out, "*** ProverException: {0}", pe.Message);
        return CommandLineArgumentsResult.PREPROCESSING_ERROR;
      }

      // If requested, print version number, help, attribute help, etc. and exit.
      if (DafnyOptions.O.ProcessInfoFlags()) {
        return CommandLineArgumentsResult.OK_EXIT_EARLY;
      }

      if (DafnyOptions.O.UseStdin) {
        dafnyFiles.Add(new DafnyFile("<stdin>", true));
      } else if (DafnyOptions.O.Files.Count == 0) {
        DafnyOptions.O.Printer.ErrorWriteLine(Console.Out, "*** Error: No input files were specified.");
        return CommandLineArgumentsResult.PREPROCESSING_ERROR;
      }
      if (DafnyOptions.O.XmlSink != null) {
        string errMsg = DafnyOptions.O.XmlSink.Open();
        if (errMsg != null) {
          DafnyOptions.O.Printer.ErrorWriteLine(Console.Out, "*** Error: " + errMsg);
          return CommandLineArgumentsResult.PREPROCESSING_ERROR;
        }
      }
      if (DafnyOptions.O.ShowEnv == ExecutionEngineOptions.ShowEnvironment.Always) {
        Console.WriteLine("---Command arguments");
        foreach (string arg in args) {
          Contract.Assert(arg != null);
          Console.WriteLine(arg);
        }
        Console.WriteLine("--------------------");
      }

      ISet<String> filesSeen = new HashSet<string>();
      foreach (string file in DafnyOptions.O.Files) {
        Contract.Assert(file != null);
        string extension = Path.GetExtension(file);
        if (extension != null) { extension = extension.ToLower(); }

        bool isDafnyFile = false;
        try {
          var df = new DafnyFile(file);
          if (!filesSeen.Add(df.CanonicalPath)) {
            continue; // silently ignore duplicate
          }
          dafnyFiles.Add(df);
          isDafnyFile = true;
        } catch (IllegalDafnyFile) {
          // Fall through and try to handle the file as an "other file"
        }

        var supportedExtensions = DafnyOptions.O.Compiler.SupportedExtensions;
        if (supportedExtensions.Contains(extension)) {
          otherFiles.Add(file);
        } else if (!isDafnyFile) {
          if (string.IsNullOrEmpty(extension) && file.Length > 0 && (file[0] == '/' || file[0] == '-')) {
            DafnyOptions.O.Printer.ErrorWriteLine(Console.Out,
              "*** Error: Command-line argument '{0}' is neither a recognized option nor a filename with a supported extension ({1}).",
              file, string.Join(", ", Enumerable.Repeat(".dfy", 1).Concat(supportedExtensions)));
          } else {
            DafnyOptions.O.Printer.ErrorWriteLine(Console.Out,
              "*** Error: '{0}': Filename extension '{1}' is not supported. Input files must be Dafny programs (.dfy) or supported auxiliary files ({2})",
              file, extension, string.Join(", ", supportedExtensions));
          }
          return CommandLineArgumentsResult.PREPROCESSING_ERROR;
        }
      }

      if (dafnyFiles.Count == 0) {
        DafnyOptions.O.Printer.ErrorWriteLine(Console.Out, "*** Error: The command-line contains no .dfy files");
        return CommandLineArgumentsResult.PREPROCESSING_ERROR;
      }

      if (dafnyFiles.Count > 1 &&
          DafnyOptions.O.TestGenOptions.Mode != TestGenerationOptions.Modes.None) {
        DafnyOptions.O.Printer.ErrorWriteLine(Console.Out,
          "*** Error: Only one .dfy file can be specified for testing");
        return CommandLineArgumentsResult.PREPROCESSING_ERROR;
      }

      if (DafnyOptions.O.ExtractCounterexample && DafnyOptions.O.ModelViewFile == null) {
        DafnyOptions.O.Printer.ErrorWriteLine(Console.Out,
          "*** Error: ModelView file must be specified when attempting counterexample extraction");
        return CommandLineArgumentsResult.PREPROCESSING_ERROR;
      }
      return CommandLineArgumentsResult.OK;
    }

    static ExitValue ProcessFiles(ExecutionEngineOptions options, IList<DafnyFile/*!*/>/*!*/ dafnyFiles, ReadOnlyCollection<string> otherFileNames,
                                  ErrorReporter reporter, bool lookForSnapshots = true, string programId = null) {
      Contract.Requires(cce.NonNullElements(dafnyFiles));
      var dafnyFileNames = DafnyFile.fileNames(dafnyFiles);

      ExitValue exitValue = ExitValue.SUCCESS;
      if (DafnyOptions.O.TestGenOptions.WarnDeadCode) {
        foreach (var line in DafnyTestGeneration.Main
          .GetDeadCodeStatistics(dafnyFileNames[0])) {
          Console.WriteLine(line);
        }
        return exitValue;
      }
      if (DafnyOptions.O.TestGenOptions.Mode != TestGenerationOptions.Modes.None) {
        foreach (var line in DafnyTestGeneration.Main
          .GetTestClassForProgram(dafnyFileNames[0])) {
          Console.WriteLine(line);
        }
        return exitValue;
      }

      if (DafnyOptions.O.VerifySeparately && 1 < dafnyFiles.Count) {
        foreach (var f in dafnyFiles) {
          Console.WriteLine();
          Console.WriteLine("-------------------- {0} --------------------", f);
          var ev = ProcessFiles(options, new List<DafnyFile> { f }, new List<string>().AsReadOnly(), reporter, lookForSnapshots, f.FilePath);
          if (exitValue != ev && ev != ExitValue.SUCCESS) {
            exitValue = ev;
          }
        }
        return exitValue;
      }

      if (0 <= DafnyOptions.O.VerifySnapshots && lookForSnapshots) {
        var snapshotsByVersion = ExecutionEngine.LookForSnapshots(dafnyFileNames);
        foreach (var s in snapshotsByVersion) {
          var snapshots = new List<DafnyFile>();
          foreach (var f in s) {
            snapshots.Add(new DafnyFile(f));
          }
          var ev = ProcessFiles(options, snapshots, new List<string>().AsReadOnly(), reporter, false, programId);
          if (exitValue != ev && ev != ExitValue.SUCCESS) {
            exitValue = ev;
          }
        }
        return exitValue;
      }

      Dafny.Program dafnyProgram;
      string programName = dafnyFileNames.Count == 1 ? dafnyFileNames[0] : "the_program";
      string err = Dafny.Main.ParseCheck(dafnyFiles, programName, reporter, out dafnyProgram);
      if (err != null) {
        exitValue = ExitValue.DAFNY_ERROR;
        DafnyOptions.O.Printer.ErrorWriteLine(Console.Out, err);
      } else if (dafnyProgram != null && !DafnyOptions.O.NoResolve && !DafnyOptions.O.NoTypecheck
          && DafnyOptions.O.DafnyVerify) {

        var boogiePrograms = Translate(options, dafnyProgram);

        Dictionary<string, PipelineStatistics> statss;
        PipelineOutcome oc;
        string baseName = cce.NonNull(Path.GetFileName(dafnyFileNames[^1]));
        var verified = Boogie(options, baseName, boogiePrograms, programId, out statss, out oc);
        var compiled = Compile(dafnyFileNames[0], otherFileNames, dafnyProgram, oc, statss, verified);
        exitValue = verified && compiled ? ExitValue.SUCCESS : !verified ? ExitValue.VERIFICATION_ERROR : ExitValue.COMPILE_ERROR;
      }

      if (err == null && dafnyProgram != null && DafnyOptions.O.PrintStats) {
        Util.PrintStats(dafnyProgram);
      }
      if (err == null && dafnyProgram != null && DafnyOptions.O.PrintFunctionCallGraph) {
        Util.PrintFunctionCallGraph(dafnyProgram);
      }
      if (dafnyProgram != null && DafnyOptions.O.ExtractCounterexample && exitValue == ExitValue.VERIFICATION_ERROR) {
        PrintCounterexample(DafnyOptions.O.ModelViewFile);
      }
      return exitValue;
    }

    /// <summary>
    /// Extract the counterexample corresponding to the first failing
    /// assertion and print it to the console
    /// </summary>
    /// <param name="modelViewFile"> Name of the file from which to read
    /// the counterexample </param> 
    private static void PrintCounterexample(string modelViewFile) {
      var model = DafnyModel.ExtractModel(File.ReadAllText(modelViewFile));
      Console.WriteLine("Counterexample for first failing assertion: ");
      foreach (var state in model.States.Where(state => !state.IsInitialState)) {
        Console.WriteLine(state.FullStateName + ":");
        var vars = state.ExpandedVariableSet(-1);
        foreach (var variable in vars) {
          Console.WriteLine($"\t{variable.ShortName} : " +
                            $"{variable.Type.InDafnyFormat()} = " +
                            $"{variable.Value}");
        }
      }
    }

    public static IEnumerable<Tuple<string, Bpl.Program>> Translate(ExecutionEngineOptions options, Program dafnyProgram) {
      var nmodules = Translator.VerifiableModules(dafnyProgram).Count();


      foreach (var prog in Translator.Translate(dafnyProgram, dafnyProgram.reporter)) {

        if (DafnyOptions.O.PrintFile != null) {

          var nm = nmodules > 1 ? Dafny.Main.BoogieProgramSuffix(DafnyOptions.O.PrintFile, prog.Item1) : DafnyOptions.O.PrintFile;

          ExecutionEngine.PrintBplFile(options, nm, prog.Item2, false, false, DafnyOptions.O.PrettyPrint);
        }

        yield return prog;

      }
    }

    private static VerificationResultCache cache;
    public static bool Boogie(ExecutionEngineOptions options, string baseName,
      IEnumerable<Tuple<string, Bpl.Program>> boogiePrograms, string programId,
      out Dictionary<string, PipelineStatistics> statss, out PipelineOutcome outcome) {

      bool isVerified = true;
      outcome = PipelineOutcome.VerificationCompleted;
      statss = new Dictionary<string, PipelineStatistics>();

      Stopwatch watch = new Stopwatch();
      watch.Start();

      cache ??= new VerificationResultCache();
      var engine = new ExecutionEngine(options, cache);
      foreach (var prog in boogiePrograms) {
        if (DafnyOptions.O.SeparateModuleOutput) {
          DafnyOptions.O.Printer.AdvisoryWriteLine(Console.Out, "For module: {0}", prog.Item1);
        }

        isVerified = Dafny.Main.BoogieOnce(engine, baseName, prog.Item1, prog.Item2, programId, out var newstats, out var newOutcome) && isVerified;

        watch.Stop();

        if ((outcome == PipelineOutcome.VerificationCompleted || outcome == PipelineOutcome.Done) && newOutcome != PipelineOutcome.VerificationCompleted) {
          outcome = newOutcome;
        }

        if (DafnyOptions.O.SeparateModuleOutput) {
          TimeSpan ts = watch.Elapsed;
          string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
          DafnyOptions.O.Printer.AdvisoryWriteLine(Console.Out, "Elapsed time: {0}", elapsedTime);
          WriteTrailer(newstats);
        }

        statss.Add(prog.Item1, newstats);
        watch.Restart();
      }
      watch.Stop();

      return isVerified;
    }

    private static void WriteTrailer(PipelineStatistics stats) {
      if (!DafnyOptions.O.Verify && stats.ErrorCount == 0) {
        Console.WriteLine();
        Console.Write("{0} did not attempt verification", DafnyOptions.O.DescriptiveToolName);
        if (stats.InconclusiveCount != 0) {
          Console.Write(", {0} inconclusive{1}", stats.InconclusiveCount, Util.Plural(stats.InconclusiveCount));
        }
        if (stats.TimeoutCount != 0) {
          Console.Write(", {0} time out{1}", stats.TimeoutCount, Util.Plural(stats.TimeoutCount));
        }
        if (stats.OutOfMemoryCount != 0) {
          Console.Write(", {0} out of memory", stats.OutOfMemoryCount);
        }
        if (stats.OutOfResourceCount != 0) {
          Console.Write(", {0} out of resource", stats.OutOfResourceCount);
        }
        if (stats.SolverExceptionCount != 0) {
          Console.Write(", {0} solver exceptions", stats.SolverExceptionCount);
        }

        Console.WriteLine();
        Console.Out.Flush();
      } else {
        // This calls a routine within Boogie
        DafnyOptions.O.Printer.WriteTrailer(Console.Out, stats);
      }
    }

    private static void WriteStatss(Dictionary<string, PipelineStatistics> statss) {
      var statSum = new PipelineStatistics();
      foreach (var stats in statss) {
        statSum.VerifiedCount += stats.Value.VerifiedCount;
        statSum.ErrorCount += stats.Value.ErrorCount;
        statSum.TimeoutCount += stats.Value.TimeoutCount;
        statSum.OutOfResourceCount += stats.Value.OutOfResourceCount;
        statSum.OutOfMemoryCount += stats.Value.OutOfMemoryCount;
        statSum.SolverExceptionCount += stats.Value.SolverExceptionCount;
        statSum.CachedErrorCount += stats.Value.CachedErrorCount;
        statSum.CachedInconclusiveCount += stats.Value.CachedInconclusiveCount;
        statSum.CachedOutOfMemoryCount += stats.Value.CachedOutOfMemoryCount;
        statSum.CachedTimeoutCount += stats.Value.CachedTimeoutCount;
        statSum.CachedOutOfResourceCount += stats.Value.CachedOutOfResourceCount;
        statSum.CachedSolverExceptionCount += stats.Value.CachedSolverExceptionCount;
        statSum.CachedVerifiedCount += stats.Value.CachedVerifiedCount;
        statSum.InconclusiveCount += stats.Value.InconclusiveCount;
      }
      WriteTrailer(statSum);
    }


    public static bool Compile(string fileName, ReadOnlyCollection<string> otherFileNames, Program dafnyProgram,
                               PipelineOutcome oc, Dictionary<string, PipelineStatistics> statss, bool verified) {
      var resultFileName = DafnyOptions.O.DafnyPrintCompiledFile ?? fileName;
      bool compiled = true;
      switch (oc) {
        case PipelineOutcome.VerificationCompleted:
          WriteStatss(statss);
          if ((DafnyOptions.O.Compile && verified && !DafnyOptions.O.UserConstrainedProcsToCheck) || DafnyOptions.O.ForceCompile) {
            compiled = CompileDafnyProgram(dafnyProgram, resultFileName, otherFileNames, true);
          } else if ((2 <= DafnyOptions.O.SpillTargetCode && verified && !DafnyOptions.O.UserConstrainedProcsToCheck) || 3 <= DafnyOptions.O.SpillTargetCode) {
            compiled = CompileDafnyProgram(dafnyProgram, resultFileName, otherFileNames, false);
          }
          break;
        case PipelineOutcome.Done:
          WriteStatss(statss);
          if (DafnyOptions.O.ForceCompile || 3 <= DafnyOptions.O.SpillTargetCode) {
            compiled = CompileDafnyProgram(dafnyProgram, resultFileName, otherFileNames, DafnyOptions.O.ForceCompile);
          }
          break;
        default:
          // error has already been reported to user
          break;
      }
      return compiled;
    }

    #region Compilation

    private record TargetPaths(string Directory, string Filename) {
      private static Func<string, string> DeleteDot = p => p == "." ? "" : p;
      private static Func<string, string> AddDot = p => p == "" ? "." : p;
      private Func<string, string> RelativeToDirectory =
        path => DeleteDot(Path.GetRelativePath(AddDot(Directory), path));

      public string RelativeDirectory => RelativeToDirectory(AddDot(Path.GetDirectoryName(Filename)));
      public string RelativeFilename => RelativeToDirectory(Filename);
      public string SourceDirectory => Path.GetDirectoryName(Filename);
    }

    private static TargetPaths GenerateTargetPaths(string dafnyProgramName) {
      string targetBaseDir = DafnyOptions.O.Compiler.TargetBaseDir(dafnyProgramName);
      string targetExtension = DafnyOptions.O.Compiler.TargetExtension;

      // Note that using Path.ChangeExtension here does the wrong thing when dafnyProgramName has multiple periods (e.g., a.b.dfy)
      string targetBaseName = DafnyOptions.O.Compiler.TargetBasename(dafnyProgramName) + "." + targetExtension;
      string targetDir = Path.Combine(Path.GetDirectoryName(dafnyProgramName), targetBaseDir);

      string targetFilename = Path.Combine(targetDir, targetBaseName);

      return new TargetPaths(Directory: Path.GetDirectoryName(dafnyProgramName), Filename: targetFilename);
    }

    static void WriteDafnyProgramToFiles(TargetPaths paths, bool targetProgramHasErrors, string targetProgramText,
      string/*?*/ callToMain, Dictionary<string, string> otherFiles, TextWriter outputWriter) {
      WriteFile(paths.Filename, targetProgramText, callToMain);

      string NormalizeRelativeFilename(string fileName) {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
          ? fileName.Replace(@"\", "/")
          : fileName;
      }

      var relativeTarget = NormalizeRelativeFilename(paths.RelativeFilename);
      if (targetProgramHasErrors) {
        // Something went wrong during compilation (e.g., the compiler may have found an "assume" statement).
        // As a courtesy, we're still printing the text of the generated target program. We print a message regardless
        // of the CompileVerbose settings.
        outputWriter.WriteLine("Wrote textual form of partial target program to {0}", relativeTarget);
      } else if (DafnyOptions.O.CompileVerbose) {
        outputWriter.WriteLine("Wrote textual form of target program to {0}", relativeTarget);
      }

      foreach (var entry in otherFiles) {
        var filename = entry.Key;
        WriteFile(Path.Join(paths.SourceDirectory, filename), entry.Value);
        if (DafnyOptions.O.CompileVerbose) {
          outputWriter.WriteLine("Additional target code written to {0}", NormalizeRelativeFilename(Path.Join(paths.RelativeDirectory, filename)));
        }
      }
    }

    static void WriteFile(string filename, string text, string moreText = null) {
      var dir = Path.GetDirectoryName(filename);
      if (dir != "") {
        Directory.CreateDirectory(dir);
      }

      CheckFilenameIsLegal(filename);
      using (TextWriter target = new StreamWriter(new FileStream(filename, System.IO.FileMode.Create))) {
        target.Write(text);
        if (moreText != null) {
          target.Write(moreText);
        }
      }
    }

    private static void CheckFilenameIsLegal(string filename) {
      // We cannot get the full path correctly on Windows if the file name uses some reserved words
      // For example, Path.GetFullPath("con.txt") will return "//./con" which is incorrect.
      // https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file?redirectedfrom=MSDN
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
        var problematicNames =
          "CON, PRN, AUX, NUL, COM1, COM2, COM3, COM4, COM5, COM6, COM7, COM8, COM9, LPT1, LPT2, LPT3, LPT4, LPT5, LPT6, LPT7, LPT8, LPT9";
        var problematicRegex =
          new Regex(@"^(.*[/\\]|^)(" +
                    string.Join("|", problematicNames.Split(", ")) + @")(\.[^/\\]*)?$", RegexOptions.IgnoreCase);
        var match = problematicRegex.Match(filename);
        if (match.Success) {
          throw new Exception($"Cannot create a file with the name {filename}." +
                              $" Windows reserves the following file names: {problematicNames}");
        }
      }
    }

    /// <summary>
    /// Generate a C# program from the Dafny program and, if "invokeCompiler" is "true", invoke
    /// the C# compiler to compile it.
    /// </summary>
    public static bool CompileDafnyProgram(Dafny.Program dafnyProgram, string dafnyProgramName,
                                           ReadOnlyCollection<string> otherFileNames, bool invokeCompiler,
                                           TextWriter outputWriter = null) {
      Contract.Requires(dafnyProgram != null);
      Contract.Assert(dafnyProgramName != null);

      if (outputWriter == null) {
        outputWriter = Console.Out;
      }

      // Compile the Dafny program into a string that contains the target program
      var oldErrorCount = dafnyProgram.reporter.Count(ErrorLevel.Error);
      DafnyOptions.O.Compiler.OnPreCompile(dafnyProgram.reporter, otherFileNames);
      var compiler = DafnyOptions.O.Compiler;

      var hasMain = Compilers.SinglePassCompiler.HasMain(dafnyProgram, out var mainMethod);
      if (hasMain) {
        mainMethod.IsEntryPoint = true;
        dafnyProgram.MainMethod = mainMethod;
      }
      string targetProgramText;
      var otherFiles = new Dictionary<string, string>();
      {
        var output = new ConcreteSyntaxTree();
        compiler.Compile(dafnyProgram, output);
        var writerOptions = new WriterState();
        var targetProgramTextWriter = new StringWriter();
        var files = new Queue<FileSyntax>();
        output.Render(targetProgramTextWriter, 0, writerOptions, files);
        targetProgramText = targetProgramTextWriter.ToString();

        while (files.Count > 0) {
          var file = files.Dequeue();
          var otherFileWriter = new StringWriter();
          writerOptions.HasNewLine = false;
          file.Tree.Render(otherFileWriter, 0, writerOptions, files);
          otherFiles.Add(file.Filename, otherFileWriter.ToString());
        }
      }
      string callToMain = null;
      if (hasMain) {
        var callToMainTree = new ConcreteSyntaxTree();
        string baseName = Path.GetFileNameWithoutExtension(dafnyProgramName);
        compiler.EmitCallToMain(mainMethod, baseName, callToMainTree);
        callToMain = callToMainTree.ToString(); // assume there aren't multiple files just to call main
      }
      Contract.Assert(hasMain == (callToMain != null));
      bool targetProgramHasErrors = dafnyProgram.reporter.Count(ErrorLevel.Error) != oldErrorCount;

      compiler.OnPostCompile();

      // blurt out the code to a file, if requested, or if other target-language files were specified on the command line.
      var paths = GenerateTargetPaths(dafnyProgramName);
      if (DafnyOptions.O.SpillTargetCode > 0 || otherFileNames.Count > 0 || (invokeCompiler && !compiler.SupportsInMemoryCompilation) ||
          (invokeCompiler && compiler.TextualTargetIsExecutable && !DafnyOptions.O.RunAfterCompile)) {
        compiler.CleanSourceDirectory(paths.SourceDirectory);
        WriteDafnyProgramToFiles(paths, targetProgramHasErrors, targetProgramText, callToMain, otherFiles, outputWriter);
      }

      if (targetProgramHasErrors) {
        return false;
      }
      // If we got here, compilation succeeded
      if (!invokeCompiler) {
        return true; // If we're not asked to invoke the target compiler, we can report success
      }

      // compile the program into an assembly
      var compiledCorrectly = compiler.CompileTargetProgram(dafnyProgramName, targetProgramText, callToMain, paths.Filename, otherFileNames,
        hasMain && DafnyOptions.O.RunAfterCompile, outputWriter, out var compilationResult);
      if (compiledCorrectly && DafnyOptions.O.RunAfterCompile) {
        if (hasMain) {
          if (DafnyOptions.O.CompileVerbose) {
            outputWriter.WriteLine("Running...");
            outputWriter.WriteLine();
          }
          compiledCorrectly = compiler.RunTargetProgram(dafnyProgramName, targetProgramText, callToMain, paths.Filename, otherFileNames, compilationResult, outputWriter);
        } else {
          // make sure to give some feedback to the user
          if (DafnyOptions.O.CompileVerbose) {
            outputWriter.WriteLine("Program compiled successfully");
          }
        }
      }
      return compiledCorrectly;
    }

    #endregion

  }
}