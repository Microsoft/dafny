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

using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Microsoft.Boogie;
using Bpl = Microsoft.Boogie;
using System.Diagnostics;
using Microsoft.Dafny.Compilers;
using Microsoft.Dafny.LanguageServer.CounterExampleGeneration;
using Microsoft.Dafny.Plugins;

namespace Microsoft.Dafny {

  /// <summary>
  /// Calls into different phases of Dafny's compilation pipeline,
  /// such as parsing, resolution, verification and code generation
  /// 
  /// Will be replaced by CompilationManager
  /// </summary>
  public class CompilerDriver : IDisposable {
    private readonly ExecutionEngine engine;

    public CompilerDriver(DafnyOptions dafnyOptions) {
      engine = ExecutionEngine.CreateWithoutSharedCache(dafnyOptions);
    }

    public static async Task<int> Run(DafnyOptions options) {
      options.RunningBoogieFromCommandLine = true;

      var backend = GetBackend(options);
      if (backend == null) {
        return (int)ExitValue.PREPROCESSING_ERROR;
      }
      options.Backend = backend;

      var getFilesExitCode = GetDafnyFiles(options, out var dafnyFiles, out var otherFiles);
      if (getFilesExitCode != ExitValue.SUCCESS) {
        return (int)getFilesExitCode;
      }

      if (options.ExtractCounterexample && options.ModelViewFile == null) {
        options.Printer.ErrorWriteLine(options.OutputWriter,
          "*** Error: ModelView file must be specified when attempting counterexample extraction");
        return (int)ExitValue.PREPROCESSING_ERROR;
      }

      using var driver = new CompilerDriver(options);
      ProofDependencyManager depManager = new();
      var exitValue = await driver.ProcessFilesAsync(dafnyFiles, otherFiles.AsReadOnly(), options, depManager);

      options.XmlSink?.Close();

      if (options.VerificationLoggerConfigs.Any()) {
        try {
          VerificationResultLogger.RaiseTestLoggerEvents(options, depManager);
        } catch (ArgumentException ae) {
          options.Printer.ErrorWriteLine(options.OutputWriter, $"*** Error: {ae.Message}");
          exitValue = ExitValue.PREPROCESSING_ERROR;
        }
      }

      if (options.Wait) {
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
      }

      return (int)exitValue;
    }

    public static ExitValue GetDafnyFiles(DafnyOptions options,
      out List<DafnyFile> dafnyFiles,
      out List<string> otherFiles) {

      if (options.DafnyProject != null) {
        foreach (var uri in options.DafnyProject.GetRootSourceUris(OnDiskFileSystem.Instance)) {
          options.CliRootSourceUris.Add(uri);
        }
      }

      dafnyFiles = new List<DafnyFile>();
      otherFiles = new List<string>();
      var outputWriter = options.OutputWriter;

      if (options.UseStdin) {
        var uri = new Uri("stdin:///");
        options.CliRootSourceUris.Add(uri);
        dafnyFiles.Add(DafnyFile.CreateAndValidate(new ConsoleErrorReporter(options), OnDiskFileSystem.Instance, options, uri, Token.NoToken));
      } else if (options.CliRootSourceUris.Count == 0) {
        options.Printer.ErrorWriteLine(options.ErrorWriter, "*** Error: No input files were specified in command-line. " + options.Environment);
        return ExitValue.PREPROCESSING_ERROR;
      }
      if (options.XmlSink != null) {
        string errMsg = options.XmlSink.Open();
        if (errMsg != null) {
          options.Printer.ErrorWriteLine(options.ErrorWriter, "*** Error: " + errMsg);
          return ExitValue.PREPROCESSING_ERROR;
        }
      }
      if (options.ShowEnv == ExecutionEngineOptions.ShowEnvironment.Always) {
        outputWriter.WriteLine(options.Environment);
      }

      ISet<String> filesSeen = new HashSet<string>();
      foreach (var file in options.CliRootSourceUris.Where(u => u.IsFile).Select(u => u.LocalPath).
                 Concat(SplitOptionValueIntoFiles(options.LibraryFiles))) {
        Contract.Assert(file != null);
        var extension = Path.GetExtension(file);
        if (extension != null) { extension = extension.ToLower(); }

        bool isDafnyFile = false;
        var relative = Path.GetFileName(file);
        bool useRelative = options.UseBaseNameForFileName || relative.StartsWith("-");
        var nameToShow = useRelative ? relative
          : Path.GetRelativePath(Directory.GetCurrentDirectory(), file);
        try {
          var consoleErrorReporter = new ConsoleErrorReporter(options);
          var df = DafnyFile.CreateAndValidate(consoleErrorReporter, OnDiskFileSystem.Instance, options, new Uri(Path.GetFullPath(file)), Token.Cli);
          if (df == null) {
            if (consoleErrorReporter.FailCompilation) {
              return ExitValue.PREPROCESSING_ERROR;
            }
          } else {
            if (options.LibraryFiles.Contains(file)) {
              df.IsPreverified = true;
              df.IsPrecompiled = true;
            }
            if (!filesSeen.Add(df.CanonicalPath)) {
              continue; // silently ignore duplicate
            }
            dafnyFiles.Add(df);
            isDafnyFile = true;
          }
        } catch (ArgumentException e) {
          options.Printer.ErrorWriteLine(options.ErrorWriter, "*** Error: {0}: ", nameToShow, e.Message);
          return ExitValue.PREPROCESSING_ERROR;
        } catch (Exception e) {
          options.Printer.ErrorWriteLine(options.ErrorWriter, "*** Error: {0}: {1}", nameToShow, e.Message);
          return ExitValue.PREPROCESSING_ERROR;
        }

        var supportedExtensions = options.Backend.SupportedExtensions;
        if (supportedExtensions.Contains(extension)) {
          // .h files are not part of the build, they are just emitted as includes
          // TODO: This should be delegated to the backend instead (i.e. the CppCompilerBackend)
          if (File.Exists(file) || extension == ".h") {
            otherFiles.Add(file);
          } else {
            options.Printer.ErrorWriteLine(options.OutputWriter, $"*** Error: file {nameToShow} not found");
            return ExitValue.PREPROCESSING_ERROR;
          }
        } else if (options.AllowSourceFolders && Directory.Exists(file)) {
          options.SourceFolders.Add(file);
        } else if (!isDafnyFile) {
          if (options.UsingNewCli && string.IsNullOrEmpty(extension) && file.Length > 0) {
            options.Printer.ErrorWriteLine(options.OutputWriter,
              "*** Error: Command-line argument '{0}' is neither a recognized option nor a filename with a supported extension ({1}).",
              nameToShow,
              string.Join(", ", Enumerable.Repeat(".dfy", 1).Concat(supportedExtensions)));
          } else if (string.IsNullOrEmpty(extension) && file.Length > 0 && (file[0] == '/' || file[0] == '-')) {
            options.Printer.ErrorWriteLine(options.OutputWriter,
              "*** Error: Command-line argument '{0}' is neither a recognized option nor a filename with a supported extension ({1}).",
              nameToShow, string.Join(", ", Enumerable.Repeat(".dfy", 1).Concat(supportedExtensions)));
          } else {
            options.Printer.ErrorWriteLine(options.OutputWriter,
              "*** Error: '{0}': Filename extension '{1}' is not supported. Input files must be Dafny programs (.dfy) or supported auxiliary files ({2})",
              nameToShow, extension, string.Join(", ", supportedExtensions));
          }
          return ExitValue.PREPROCESSING_ERROR;
        }
      }

      if (dafnyFiles.Count == 0 && options.SourceFolders.Count == 0) {
        if (!options.AllowSourceFolders) {
          options.Printer.ErrorWriteLine(Console.Out, "*** Error: The command-line contains no .dfy files");
          // TODO: With the test on CliRootUris.Count above, this code is no longer reachable
          options.Printer.ErrorWriteLine(options.OutputWriter, "*** Error: The command-line contains no .dfy files");
          return ExitValue.PREPROCESSING_ERROR;
        }

        options.Printer.ErrorWriteLine(Console.Out, "*** Error: The command-line contains no .dfy files or folders");
        //options.Printer.ErrorWriteLine(Console.Out,
        //  "Usage:\ndafny format [--check] [--print] <file/folder> <file/folder>...\nYou can use '.' for the current directory");
        return ExitValue.PREPROCESSING_ERROR;
      }

      // Add standard library .doo files after explicitly provided source files,
      // only because if they are added first, one might be used as the program name,
      // which is not handled well.
      if (options.Get(CommonOptionBag.UseStandardLibraries)) {
        var reporter = new ConsoleErrorReporter(options);
        if (options.CompilerName is null or "cs" or "java" or "go" or "py" or "js") {
          var targetName = options.CompilerName ?? "notarget";
          var stdlibDooUri = DafnyMain.StandardLibrariesDooUriTarget[targetName];
          options.CliRootSourceUris.Add(stdlibDooUri);
          dafnyFiles.Add(DafnyFile.CreateAndValidate(reporter, OnDiskFileSystem.Instance, options, stdlibDooUri, Token.Cli));
        }

        options.CliRootSourceUris.Add(DafnyMain.StandardLibrariesDooUri);
        dafnyFiles.Add(DafnyFile.CreateAndValidate(reporter, OnDiskFileSystem.Instance, options, DafnyMain.StandardLibrariesDooUri, Token.Cli));
      }

      return ExitValue.SUCCESS;
    }

    static IEnumerable<string> SplitOptionValueIntoFiles(HashSet<string> inputs) {
      var result = new HashSet<string>();
      foreach (var input in inputs) {
        var values = input.Split(',');
        foreach (var slice in values) {
          var name = slice.Trim();
          if (Directory.Exists(name)) {
            var files = Directory.GetFiles(name, "*.dfy", SearchOption.AllDirectories);
            foreach (var file in files) { result.Add(file); }
          } else {
            result.Add(name);
          }
        }
      }
      return result;
    }

    private static IExecutableBackend GetBackend(DafnyOptions options) {
      var backends = options.Plugins.SelectMany(p => p.GetCompilers(options)).ToList();
      var backend = backends.LastOrDefault(c => c.TargetId == options.CompilerName);
      if (backend == null) {
        if (options.CompilerName != null) {
          var known = String.Join(", ", backends.Select(c => $"'{c.TargetId}' ({c.TargetName})"));
          options.Printer.ErrorWriteLine(options.ErrorWriter,
            $"*** Error: No compiler found for target \"{options.CompilerName}\"{(options.CompilerName.StartsWith("-t") || options.CompilerName.StartsWith("--") ? " (use just a target name, not a -t or --target option)" : "")}; expecting one of {known}");
        } else {
          backend = new NoExecutableBackend(options);
        }
      }

      return backend;
    }

    public async Task<ExitValue> ProcessFilesAsync(IReadOnlyList<DafnyFile/*!*/>/*!*/ dafnyFiles,
      ReadOnlyCollection<string> otherFileNames,
      DafnyOptions options, ProofDependencyManager depManager,
      bool lookForSnapshots = true, string programId = null) {
      Contract.Requires(cce.NonNullElements(dafnyFiles));
      var dafnyFileNames = DafnyFile.FileNames(dafnyFiles);

      ExitValue exitValue = ExitValue.SUCCESS;

      if (options.VerifySeparately && 1 < dafnyFiles.Count) {
        foreach (var f in dafnyFiles) {
          await options.OutputWriter.WriteLineAsync();
          await options.OutputWriter.WriteLineAsync($"-------------------- {f} --------------------");
          var ev = await ProcessFilesAsync(new List<DafnyFile> { f }, new List<string>().AsReadOnly(), options, depManager, lookForSnapshots, f.FilePath);
          if (exitValue != ev && ev != ExitValue.SUCCESS) {
            exitValue = ev;
          }
        }
        return exitValue;
      }

      if (0 < options.VerifySnapshots && lookForSnapshots) {
        var snapshotsByVersion = ExecutionEngine.LookForSnapshots(dafnyFileNames);
        foreach (var s in snapshotsByVersion) {
          var snapshots = new List<DafnyFile>();
          foreach (var f in s) {
            var uri = new Uri(Path.GetFullPath(f));
            snapshots.Add(DafnyFile.CreateAndValidate(new ConsoleErrorReporter(options), OnDiskFileSystem.Instance, options, uri, Token.Cli));
            options.CliRootSourceUris.Add(uri);
          }
          var ev = await ProcessFilesAsync(snapshots, new List<string>().AsReadOnly(), options, depManager, false, programId);
          if (exitValue != ev && ev != ExitValue.SUCCESS) {
            exitValue = ev;
          }
        }
        return exitValue;
      }

      string programName = dafnyFileNames.Count == 1 ? dafnyFileNames[0] : "the_program";
      var err = DafnyMain.ParseCheck(options.Input, dafnyFiles, programName, options, out var dafnyProgram);
      if (err != null) {
        exitValue = ExitValue.DAFNY_ERROR;
        options.Printer.ErrorWriteLine(options.OutputWriter, err);
      } else if (dafnyProgram != null && !options.NoResolve && !options.NoTypecheck
          && options.DafnyVerify) {

        dafnyProgram.ProofDependencyManager = depManager;
        var boogiePrograms =
          await DafnyMain.LargeStackFactory.StartNew(() => Translate(engine.Options, dafnyProgram).ToList());

        string baseName = cce.NonNull(Path.GetFileName(dafnyFileNames[^1]));
        var (verified, outcome, moduleStats) = await BoogieAsync(options, baseName, boogiePrograms, programId);

        if (options.TrackVerificationCoverage) {
          ProofDependencyWarnings.WarnAboutSuspiciousDependencies(options, dafnyProgram.Reporter, depManager);
          var coverageReportDir = options.Get(CommonOptionBag.VerificationCoverageReport);
          if (coverageReportDir != null) {
            new CoverageReporter(options).SerializeVerificationCoverageReport(
              depManager, dafnyProgram,
              boogiePrograms.SelectMany(tp => tp.Item2.AllCoveredElements),
              coverageReportDir);
          }
        }

        bool compiled;
        try {
          compiled = await Compile(dafnyFileNames[0], otherFileNames, dafnyProgram, outcome, moduleStats, verified);
        } catch (UnsupportedFeatureException e) {
          if (!options.Backend.UnsupportedFeatures.Contains(e.Feature)) {
            throw new Exception($"'{e.Feature}' is not an element of the {options.Backend.TargetId} compiler's UnsupportedFeatures set");
          }
          dafnyProgram.Reporter.Error(MessageSource.Compiler, CompilerErrors.ErrorId.f_unsupported_feature, e.Token, e.Message);
          compiled = false;
        }

        var failBecauseOfWarnings = dafnyProgram.Reporter.WarningCount > 0 && options.FailOnWarnings;
        if (failBecauseOfWarnings) {
          exitValue = ExitValue.DAFNY_ERROR;
        } else if (!verified) {
          exitValue = ExitValue.VERIFICATION_ERROR;
        } else if (!compiled) {
          exitValue = ExitValue.COMPILE_ERROR;
        } else {
          exitValue = ExitValue.SUCCESS;
        }
      }

      if (err == null && dafnyProgram != null && options.PrintStats) {
        Util.PrintStats(dafnyProgram);
      }
      if (err == null && dafnyProgram != null && options.PrintFunctionCallGraph) {
        Util.PrintFunctionCallGraph(dafnyProgram);
      }
      if (dafnyProgram != null && options.ExtractCounterexample && exitValue == ExitValue.VERIFICATION_ERROR) {
        PrintCounterexample(options, options.ModelViewFile);
      }
      return exitValue;
    }

    /// <summary>
    /// Extract the counterexample corresponding to the first failing
    /// assertion and print it to the console
    /// </summary>
    private static void PrintCounterexample(DafnyOptions options, string modelViewFile) {
      var model = DafnyModel.ExtractModel(options, File.ReadAllText(modelViewFile));
      options.OutputWriter.WriteLine("Counterexample for first failing assertion: ");
      foreach (var state in model.States.Where(state => !state.IsInitialState)) {
        options.OutputWriter.WriteLine(state.FullStateName + ":");
        var vars = state.ExpandedVariableSet(-1);
        foreach (var variable in vars) {
          options.OutputWriter.WriteLine($"\t{variable.ShortName} : " +
                                   $"{DafnyModelTypeUtils.GetInDafnyFormat(variable.Type)} = " +
                                   $"{variable.Value}");
        }
      }
    }

    private static string BoogieProgramSuffix(string printFile, string suffix) {
      var baseName = Path.GetFileNameWithoutExtension(printFile);
      var dirName = Path.GetDirectoryName(printFile);

      return Path.Combine(dirName, baseName + "_" + suffix + Path.GetExtension(printFile));
    }

    public static IEnumerable<Tuple<string, Bpl.Program>> Translate(ExecutionEngineOptions options, Program dafnyProgram) {
      var modulesCount = BoogieGenerator.VerifiableModules(dafnyProgram).Count();


      foreach (var prog in BoogieGenerator.Translate(dafnyProgram, dafnyProgram.Reporter)) {

        if (options.PrintFile != null) {

          var fileName = modulesCount > 1 ? Dafny.DafnyMain.BoogieProgramSuffix(options.PrintFile, prog.Item1) : options.PrintFile;

          ExecutionEngine.PrintBplFile(options, fileName, prog.Item2, false, false, options.PrettyPrint);
        }

        yield return prog;

      }
    }

    public async Task<(bool IsVerified, PipelineOutcome Outcome, IDictionary<string, PipelineStatistics> ModuleStats)>
      BoogieAsync(DafnyOptions options,
        string baseName,
        IEnumerable<Tuple<string, Bpl.Program>> boogiePrograms, string programId) {

      var concurrentModuleStats = new ConcurrentDictionary<string, PipelineStatistics>();
      var writerManager = new ConcurrentToSequentialWriteManager(options.OutputWriter);

      var moduleTasks = boogiePrograms.Select(async program => {
        await using var moduleWriter = writerManager.AppendWriter();
        // ReSharper disable once AccessToDisposedClosure
        var result = await Task.Run(() =>
          BoogieOnceWithTimerAsync(moduleWriter, options, baseName, programId, program.Item1, program.Item2));
        concurrentModuleStats.TryAdd(program.Item1, result.Stats);
        return result;
      }).ToList();

      await Task.WhenAll(moduleTasks);
      await options.OutputWriter.FlushAsync();
      var outcome = moduleTasks.Select(t => t.Result.Outcome)
        .Aggregate(PipelineOutcome.VerificationCompleted, MergeOutcomes);

      var isVerified = moduleTasks.Select(t =>
        DafnyMain.IsBoogieVerified(t.Result.Outcome, t.Result.Stats)).All(x => x);
      return (isVerified, outcome, concurrentModuleStats);
    }

    private async Task<(PipelineOutcome Outcome, PipelineStatistics Stats)> BoogieOnceWithTimerAsync(
      TextWriter output,
      DafnyOptions options,
      string baseName, string programId,
      string moduleName,
      Bpl.Program program) {
      Stopwatch watch = new Stopwatch();
      watch.Start();
      if (options.SeparateModuleOutput) {
        options.Printer.AdvisoryWriteLine(output, "For module: {0}", moduleName);
      }

      var result =
        await Dafny.DafnyMain.BoogieOnce(options, output, engine, baseName, moduleName, program, programId);

      watch.Stop();

      if (options.SeparateModuleOutput) {
        TimeSpan ts = watch.Elapsed;
        string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
        options.Printer.AdvisoryWriteLine(output, "Elapsed time: {0}", elapsedTime);
        WriteTrailer(options, output, result.Statistics);
      }

      return result;
    }

    private static PipelineOutcome MergeOutcomes(PipelineOutcome first, PipelineOutcome second) {

      if ((first == PipelineOutcome.VerificationCompleted || first == PipelineOutcome.Done) &&
          second != PipelineOutcome.VerificationCompleted) {
        return second;
      }

      return first;
    }

    public static void WriteTrailer(DafnyOptions options, TextWriter output, PipelineStatistics stats) {
      if (!options.Verify && stats.ErrorCount == 0) {
        output.WriteLine();
        output.Write("{0} did not attempt verification", options.DescriptiveToolName);
        if (stats.InconclusiveCount != 0) {
          output.Write(", {0} inconclusive{1}", stats.InconclusiveCount, Util.Plural(stats.InconclusiveCount));
        }
        if (stats.TimeoutCount != 0) {
          output.Write(", {0} time out{1}", stats.TimeoutCount, Util.Plural(stats.TimeoutCount));
        }
        if (stats.OutOfMemoryCount != 0) {
          output.Write(", {0} out of memory", stats.OutOfMemoryCount);
        }
        if (stats.OutOfResourceCount != 0) {
          output.Write(", {0} out of resource", stats.OutOfResourceCount);
        }
        if (stats.SolverExceptionCount != 0) {
          output.Write(", {0} solver exceptions", stats.SolverExceptionCount);
        }

        output.WriteLine();
        output.Flush();
      } else {
        // This calls a routine within Boogie
        options.Printer.WriteTrailer(output, stats);
      }
    }

    public static void WriteProgramVerificationSummary(DafnyOptions options, TextWriter output, IDictionary<string, PipelineStatistics> moduleStats) {
      var statSum = new PipelineStatistics();
      foreach (var stats in moduleStats) {
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
      WriteTrailer(options, output, statSum);
    }


    public static async Task<bool> Compile(string fileName, ReadOnlyCollection<string> otherFileNames, Program dafnyProgram,
                               PipelineOutcome oc, IDictionary<string, PipelineStatistics> moduleStats, bool verified) {
      var options = dafnyProgram.Options;
      var resultFileName = options.DafnyPrintCompiledFile ?? fileName;
      bool compiled = true;
      switch (oc) {
        case PipelineOutcome.VerificationCompleted:
          WriteProgramVerificationSummary(options, options.OutputWriter, moduleStats);
          if ((options.Compile && verified && !options.UserConstrainedProcsToCheck) || options.ForceCompile) {
            compiled = await CompileDafnyProgram(dafnyProgram, resultFileName, otherFileNames, true);
          } else if ((2 <= options.SpillTargetCode && verified && !options.UserConstrainedProcsToCheck) || 3 <= options.SpillTargetCode) {
            compiled = await CompileDafnyProgram(dafnyProgram, resultFileName, otherFileNames, false);
          }
          break;
        case PipelineOutcome.Done:
          WriteProgramVerificationSummary(options, options.OutputWriter, moduleStats);
          if (options.ForceCompile || 3 <= options.SpillTargetCode) {
            compiled = await CompileDafnyProgram(dafnyProgram, resultFileName, otherFileNames, options.ForceCompile);
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

    private static TargetPaths GenerateTargetPaths(DafnyOptions options, string dafnyProgramName) {
      string targetBaseDir = options.Backend.TargetBaseDir(dafnyProgramName);
      string targetExtension = options.Backend.TargetExtension;

      // Note that using Path.ChangeExtension here does the wrong thing when dafnyProgramName has multiple periods (e.g., a.b.dfy)
      string targetBaseName = options.Backend.TargetBasename(dafnyProgramName) + "." + targetExtension;
      string targetDir = Path.Combine(Path.GetDirectoryName(dafnyProgramName), targetBaseDir);

      string targetFilename = Path.Combine(targetDir, targetBaseName);

      return new TargetPaths(Directory: Path.GetDirectoryName(dafnyProgramName), Filename: targetFilename);
    }

    static void WriteDafnyProgramToFiles(DafnyOptions options, TargetPaths paths, bool targetProgramHasErrors, string targetProgramText,
      string/*?*/ callToMain, Dictionary<string, string> otherFiles, TextWriter outputWriter) {
      if (targetProgramText.Length != 0) {
        WriteFile(paths.Filename, targetProgramText, callToMain);
      }

      string NormalizeRelativeFilename(string fileName) {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
          ? fileName.Replace(@"\", "/")
          : fileName;
      }

      var relativeTarget = NormalizeRelativeFilename(paths.RelativeFilename);
      if (targetProgramHasErrors) {
        // Something went wrong during compilation (e.g., the compiler may have found an "assume" statement).
        // As a courtesy, we're still printing the text of the generated target program. We print a message regardless
        // of the Verbose settings.
        outputWriter.WriteLine("Wrote textual form of partial target program to {0}", relativeTarget);
      } else if (options.Verbose) {
        outputWriter.WriteLine("Wrote textual form of target program to {0}", relativeTarget);
      }

      foreach (var entry in otherFiles) {
        var filename = entry.Key;
        WriteFile(Path.Join(paths.SourceDirectory, filename), entry.Value);
        if (options.Verbose) {
          outputWriter.WriteLine("Additional target code written to {0}", NormalizeRelativeFilename(Path.Join(paths.RelativeDirectory, filename)));
        }
      }
    }

    public static void WriteFile(string filename, string text, string moreText = null) {
      var dir = Path.GetDirectoryName(filename);
      if (dir != "") {
        Directory.CreateDirectory(dir);
      }

      CheckFilenameIsLegal(filename);
      using TextWriter target = new StreamWriter(new FileStream(filename, FileMode.Create));
      target.Write(text);
      if (moreText != null) {
        target.Write(moreText);
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
    public static async Task<bool> CompileDafnyProgram(Program dafnyProgram, string dafnyProgramName,
                                           ReadOnlyCollection<string> otherFileNames, bool invokeCompiler) {

      var rewriters = RewriterCollection.GetRewriters(dafnyProgram.Reporter, dafnyProgram);
      foreach (var rewriter in rewriters) {
        rewriter.PostVerification(dafnyProgram);
      }

      Contract.Requires(dafnyProgram != null);
      Contract.Assert(dafnyProgramName != null);

      var outputWriter = dafnyProgram.Options.OutputWriter;
      var errorWriter = dafnyProgram.Options.ErrorWriter;

      // Compile the Dafny program into a string that contains the target program
      var oldErrorCount = dafnyProgram.Reporter.Count(ErrorLevel.Error);
      var options = dafnyProgram.Options;
      options.Backend.OnPreCompile(dafnyProgram.Reporter, otherFileNames);

      // Now that an internal compiler is instantiated, apply any plugin instrumentation.
      foreach (var compilerInstrumenter in options.Plugins.SelectMany(p => p.GetCompilerInstrumenters(dafnyProgram.Reporter))) {
        options.Backend.InstrumentCompiler(compilerInstrumenter, dafnyProgram);
      }

      if (options.Get(CommonOptionBag.ExecutionCoverageReport) != null
          && options.Backend.UnsupportedFeatures.Contains(Feature.RuntimeCoverageReport)) {
        throw new UnsupportedFeatureException(dafnyProgram.GetStartOfFirstFileToken(), Feature.RuntimeCoverageReport);
      }

      var compiler = options.Backend;

      var hasMain = SinglePassCompiler.HasMain(dafnyProgram, out var mainMethod);
      if (hasMain) {
        mainMethod.IsEntryPoint = true;
        dafnyProgram.MainMethod = mainMethod;
      }
      string targetProgramText;
      var otherFiles = new Dictionary<string, string>();
      {
        var output = new ConcreteSyntaxTree();
        await DafnyMain.LargeStackFactory.StartNew(() => compiler.Compile(dafnyProgram, output));
        var writerOptions = new WriterState();
        var targetProgramTextWriter = new StringWriter();
        var files = new Queue<FileSyntax>();
        output.Render(targetProgramTextWriter, 0, writerOptions, files, compiler.TargetIndentSize);
        targetProgramText = targetProgramTextWriter.ToString();

        while (files.Count > 0) {
          var file = files.Dequeue();
          var otherFileWriter = new StringWriter();
          writerOptions.HasNewLine = false;
          file.Tree.Render(otherFileWriter, 0, writerOptions, files, compiler.TargetIndentSize);
          otherFiles.Add(file.Filename, otherFileWriter.ToString());
        }
      }
      string callToMain = null;
      if (hasMain) {
        var callToMainTree = new ConcreteSyntaxTree();
        string baseName = Path.GetFileNameWithoutExtension(dafnyProgramName);
        compiler.EmitCallToMain(mainMethod, baseName, callToMainTree);
        callToMain = callToMainTree.MakeString(compiler.TargetIndentSize); // assume there aren't multiple files just to call main
      }
      Contract.Assert(hasMain == (callToMain != null));
      bool targetProgramHasErrors = dafnyProgram.Reporter.Count(ErrorLevel.Error) != oldErrorCount;

      compiler.OnPostCompile();

      // blurt out the code to a file, if requested, or if other target-language files were specified on the command line.
      var targetPaths = GenerateTargetPaths(options, dafnyProgramName);
      if (options.SpillTargetCode > 0 || otherFileNames.Count > 0 || (invokeCompiler && !compiler.SupportsInMemoryCompilation) ||
          (invokeCompiler && compiler.TextualTargetIsExecutable && !options.RunAfterCompile)) {
        compiler.CleanSourceDirectory(targetPaths.SourceDirectory);
        WriteDafnyProgramToFiles(options, targetPaths, targetProgramHasErrors, targetProgramText, callToMain, otherFiles, outputWriter);
      }

      if (dafnyProgram.Reporter.FailCompilation) {
        return false;
      }
      // If we got here, compilation succeeded
      if (!invokeCompiler) {
        return true; // If we're not asked to invoke the target compiler, we can report success
      }

      // compile the program into an assembly
      var compiledCorrectly = compiler.CompileTargetProgram(dafnyProgramName, targetProgramText, callToMain, targetPaths.Filename, otherFileNames,
        hasMain && options.RunAfterCompile, outputWriter, out var compilationResult);
      if (compiledCorrectly && options.RunAfterCompile) {
        if (hasMain) {
          if (options.Verbose) {
            await outputWriter.WriteLineAsync("Running...");
            await outputWriter.WriteLineAsync();
          }

          compiledCorrectly = compiler.RunTargetProgram(dafnyProgramName, targetProgramText, callToMain,
            targetPaths.Filename, otherFileNames, compilationResult, outputWriter, errorWriter);

          if (compiledCorrectly) {
            var coverageReportDir = options.Get(CommonOptionBag.ExecutionCoverageReport);
            if (coverageReportDir != null) {
              var coverageReport = new CoverageReport("Execution Coverage", "Branches", "_tests_actual", dafnyProgram);
              compiler.PopulateCoverageReport(coverageReport);
              new CoverageReporter(options).SerializeCoverageReports(coverageReport, coverageReportDir);
            }
          }
        } else {
          // make sure to give some feedback to the user
          if (options.Verbose) {
            await outputWriter.WriteLineAsync("Program compiled successfully");
          }
        }
      }
      return compiledCorrectly;
    }

    #endregion

    public void Dispose() {
      engine.Dispose();
    }
  }
}
