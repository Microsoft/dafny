//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
// Copyright by the contributors to the Dafny Project
// SPDX-License-Identifier: MIT
//
//-----------------------------------------------------------------------------
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DafnyCore;
using Microsoft.Boogie;

namespace Microsoft.Dafny {

  public class IllegalDafnyFile : Exception {
    public bool ProcessingError { get; }

    public IllegalDafnyFile(bool processingError = false) {
      this.ProcessingError = processingError;
    }
  }

  public class DafnyMain {

    public static void MaybePrintProgram(Program program, string filename, bool afterResolver) {
      if (filename == null) {
        return;
      }

      var tw = filename == "-" ? program.Options.OutputWriter : new StreamWriter(filename);
      var pr = new Printer(tw, program.Options, program.Options.PrintMode);
      pr.PrintProgramLargeStack(program, afterResolver);
    }

    /// <summary>
    /// Returns null on success, or an error string otherwise.
    /// </summary>
    public static string ParseCheck(TextReader stdIn, IList<DafnyFile /*!*/> /*!*/ files, string /*!*/ programName,
        DafnyOptions options, out Program program)
    //modifies Bpl.options.XmlSink.*;
    {
      string err = Parse(stdIn, files, programName, options, out program);
      if (err != null) {
        return err;
      }

      return Resolve(program);
    }

    public static string Parse(TextReader stdIn, IList<DafnyFile> files, string programName, DafnyOptions options,
      out Program program) {
      Contract.Requires(programName != null);
      Contract.Requires(files != null);
      program = null;

      var defaultModuleDefinition =
        new DefaultModuleDefinition(files.Where(f => !f.IsPreverified).Select(f => f.Uri).ToList());
      ErrorReporter reporter = options.DiagnosticsFormat switch {
        DafnyOptions.DiagnosticsFormats.PlainText => new ConsoleErrorReporter(options, defaultModuleDefinition),
        DafnyOptions.DiagnosticsFormats.JSON => new JsonConsoleErrorReporter(options, defaultModuleDefinition),
        _ => throw new ArgumentOutOfRangeException()
      };

      LiteralModuleDecl module = new LiteralModuleDecl(defaultModuleDefinition, null);
      BuiltIns builtIns = new BuiltIns(options);

      foreach (DafnyFile dafnyFile in files) {
        Contract.Assert(dafnyFile != null);
        if (options.XmlSink is { IsOpen: true } && !dafnyFile.UseStdin) {
          options.XmlSink.WriteFileFragment(dafnyFile.FilePath);
        }

        if (options.Trace) {
          options.OutputWriter.WriteLine("Parsing " + dafnyFile.FilePath);
        }

        // We model a precompiled file, a library, as an include
        var include = dafnyFile.IsPrecompiled ? new Include(new Token {
          Uri = dafnyFile.Uri,
          col = 1,
          line = 0
        }, new Uri("cli://"), dafnyFile.FilePath) : null;
        if (include != null) {
          // TODO this can be removed once the include error message in ErrorReporter.Error is removed.
          module.ModuleDef.Includes.Add(include);
        }
        var err = ParseFile(dafnyFile, null, module, builtIns, new Errors(reporter));
        if (err != null) {
          return err;
        }
      }

      if (!(options.DisallowIncludes || options.PrintIncludesMode == DafnyOptions.IncludesModes.Immediate)) {
        string errString = ParseIncludes(module, builtIns, files.Select(f => f.FilePath).ToHashSet(), new Errors(reporter));
        if (errString != null) {
          return errString;
        }
      }

      if (options.PrintIncludesMode == DafnyOptions.IncludesModes.Immediate) {
        DependencyMap dmap = new DependencyMap();
        dmap.AddIncludes(module.ModuleDef.Includes);
        dmap.PrintMap(options);
      }

      var verifiedRoots = files.Where(df => df.IsPreverified).Select(df => df.Uri).ToHashSet();
      var compiledRoots = files.Where(df => df.IsPrecompiled).Select(df => df.Uri).ToHashSet();
      program = new Program(programName, module, builtIns, reporter, verifiedRoots, compiledRoots);

      MaybePrintProgram(program, options.DafnyPrintFile, false);

      return null; // success
    }

    private static readonly TaskScheduler largeThreadScheduler =
      CustomStackSizePoolTaskScheduler.Create(0x10000000, Environment.ProcessorCount);

    public static readonly TaskFactory LargeStackFactory = new(CancellationToken.None,
      TaskCreationOptions.DenyChildAttach, TaskContinuationOptions.None, largeThreadScheduler);

    public static string Resolve(Program program) {
      if (program.Options.NoResolve || program.Options.NoTypecheck) {
        return null;
      }

      var r = new Resolver(program);
      LargeStackFactory.StartNew(() => r.ResolveProgram(program)).Wait();
      MaybePrintProgram(program, program.Options.DafnyPrintResolvedFile, true);

      if (program.Reporter.ErrorCountUntilResolver != 0) {
        return string.Format("{0} resolution/type errors detected in {1}", program.Reporter.Count(ErrorLevel.Error),
          program.Name);
      }

      return null; // success
    }

    // Lower-case file names before comparing them, since Windows uses case-insensitive file names
    private class IncludeComparer : IComparer<Include> {
      public int Compare(Include x, Include y) {
        return x.CompareTo(y);
      }
    }

    public static string ParseIncludes(ModuleDecl module, BuiltIns builtIns, ISet<string> excludeFiles, Errors errs) {
      var includesFound = new SortedSet<Include>(new IncludeComparer());
      var allIncludes = ((LiteralModuleDecl)module).ModuleDef.Includes;

      var notCompiledResult = TraverseIncludesFrom(0);
      if (notCompiledResult != null) {
        return notCompiledResult;
      }

      if (builtIns.Options.PrintIncludesMode != DafnyOptions.IncludesModes.None) {
        var dependencyMap = new DependencyMap();
        dependencyMap.AddIncludes(allIncludes);
        dependencyMap.PrintMap(builtIns.Options);
      }

      return null; // Success

      string TraverseIncludesFrom(int startingIndex) {
        var includeIndex = startingIndex;
        var stack = new Stack<Include>();

        while (true) {
          var addedItems = allIncludes.Skip(includeIndex);
          foreach (var addedItem in addedItems.Reverse()) {
            stack.Push(addedItem);
          }

          includeIndex = allIncludes.Count;

          if (stack.Count == 0) {
            break;
          }

          var include = stack.Pop();
          if (!includesFound.Add(include) || excludeFiles.Contains(include.CanonicalPath)) {
            continue;
          }

          DafnyFile file;
          try {
            file = new DafnyFile(builtIns.Options, include.IncludedFilename);
          } catch (IllegalDafnyFile) {
            return ($"Include of file \"{include.IncludedFilename}\" failed.");
          }

          string result = ParseFile(file, include, module, builtIns, errs);
          if (result != null) {
            return result;
          }
        }

        return null;
      }
    }

    private static string ParseFile(DafnyFile dafnyFile, Include include, ModuleDecl module, BuiltIns builtIns, Errors errs) {
      var fn = builtIns.Options.UseBaseNameForFileName ? Path.GetFileName(dafnyFile.FilePath) : dafnyFile.FilePath;
      try {
        int errorCount = Parser.Parse(dafnyFile.Content, dafnyFile.Uri, module, builtIns, errs);
        if (errorCount != 0) {
          return $"{errorCount} parse errors detected in {fn}";
        }
      } catch (IOException e) {
        IToken tok = include == null ? Token.NoToken : include.tok;
        errs.SemErr(tok, "Unable to open included file");
        return $"Error opening file \"{fn}\": {e.Message}";
      }

      return null; // Success
    }

    public static async Task<(PipelineOutcome Outcome, PipelineStatistics Statistics)> BoogieOnce(
      DafnyOptions options,
      TextWriter output,
      ExecutionEngine engine,
      string baseFile,
      string moduleName,
      Microsoft.Boogie.Program boogieProgram, string programId) {
      var moduleId = (programId ?? "main_program_id") + "_" + moduleName;
      var z3NotFoundMessage = $@"
Z3 not found. Please either provide a path to the `z3` executable using
the `--solver-path <path>` option, manually place the `z3` directory
next to the `dafny` executable you are using (this directory should
contain `bin/z3-{DafnyOptions.DefaultZ3Version}` or `bin/z3-{DafnyOptions.DefaultZ3Version}.exe`), or set the PATH environment variable
to also include a directory containing the `z3` executable.
";

      var proverPath = options.ProverOptions.Find(o => o.StartsWith("PROVER_PATH="));
      if (proverPath is null && options.Verify) {
        options.OutputWriter.WriteLine(z3NotFoundMessage);
        return (PipelineOutcome.FatalError, new PipelineStatistics());
      }

      string bplFilename;
      if (options.PrintFile != null) {
        bplFilename = options.PrintFile;
      } else {
        string baseName = cce.NonNull(Path.GetFileName(baseFile));
        baseName = cce.NonNull(Path.ChangeExtension(baseName, "bpl"));
        bplFilename = Path.Combine(Path.GetTempPath(), baseName);
      }

      bplFilename = BoogieProgramSuffix(bplFilename, moduleName);
      var (outcome, stats) = await BoogiePipelineWithRerun(options,
        output, engine, boogieProgram, bplFilename,
        1 < options.VerifySnapshots ? moduleId : null);
      return (outcome, stats);
    }

    public static string BoogieProgramSuffix(string printFile, string suffix) {
      var baseName = Path.GetFileNameWithoutExtension(printFile);
      var dirName = Path.GetDirectoryName(printFile);

      return Path.Combine(dirName, baseName + "_" + suffix + Path.GetExtension(printFile));
    }

    public static bool IsBoogieVerified(PipelineOutcome outcome, PipelineStatistics statistics) {
      return (outcome == PipelineOutcome.Done || outcome == PipelineOutcome.VerificationCompleted)
             && statistics.ErrorCount == 0
             && statistics.InconclusiveCount == 0
             && statistics.TimeoutCount == 0
             && statistics.OutOfResourceCount == 0
             && statistics.OutOfMemoryCount == 0;
    }

    /// <summary>
    /// Resolve, type check, infer invariants for, and verify the given Boogie program.
    /// The intention is that this Boogie program has been produced by translation from something
    /// else.  Hence, any resolution errors and type checking errors are due to errors in
    /// the translation.
    /// The method prints errors for resolution and type checking errors, but still returns
    /// their error code.
    /// </summary>
    private static async Task<(PipelineOutcome Outcome, PipelineStatistics Statistics)> BoogiePipelineWithRerun(
      DafnyOptions options,
      TextWriter output, ExecutionEngine engine, Microsoft.Boogie.Program /*!*/ program, string /*!*/ bplFileName,
      string programId) {
      Contract.Requires(program != null);
      Contract.Requires(bplFileName != null);

      var stats = new PipelineStatistics();
      var outcome = engine.ResolveAndTypecheck(program, bplFileName, out _);
      switch (outcome) {
        case PipelineOutcome.Done:
          return (outcome, stats);

        case PipelineOutcome.ResolutionError:
        case PipelineOutcome.TypeCheckingError:
          engine.PrintBplFile(bplFileName, program, false, false, options.PrettyPrint);
          await options.OutputWriter.WriteLineAsync();
          await options.OutputWriter.WriteLineAsync(
            "*** Encountered internal translation error - re-running Boogie to get better debug information");
          await options.OutputWriter.WriteLineAsync();

          var /*!*/
            fileNames = new List<string /*!*/> { bplFileName };
          var reparsedProgram = engine.ParseBoogieProgram(fileNames, true);
          if (reparsedProgram != null) {
            engine.ResolveAndTypecheck(reparsedProgram, bplFileName, out _);
          }

          return (outcome, stats);

        case PipelineOutcome.ResolvedAndTypeChecked:
          engine.EliminateDeadVariables(program);
          engine.CollectModSets(program);
          engine.CoalesceBlocks(program);
          engine.Inline(program);
          var inferAndVerifyOutcome = await engine.InferAndVerify(output, program, stats, programId);
          return (inferAndVerifyOutcome, stats);

        default:
          Contract.Assert(false);
          throw new cce.UnreachableException(); // unexpected outcome
      }
    }

  }
}
