﻿using Microsoft.Boogie;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Dafny.LanguageServer.Workspace;

namespace Microsoft.Dafny.LanguageServer.Language {
  /// <summary>
  /// dafny-lang based implementation of the program verifier. Since it makes use of static members,
  /// any access is synchronized. Moreover, it ensures that exactly one instance exists over the whole
  /// application lifetime.
  /// </summary>
  /// <remarks>
  /// dafny-lang makes use of static members and assembly loading. Since thread-safety of this is not guaranteed,
  /// this verifier serializes all invocations.
  /// </remarks>
  public class DafnyProgramVerifier : IProgramVerifier {
    private static readonly object InitializationSyncObject = new();
    private static bool initialized;

    private readonly ILogger logger;
    private readonly VerifierOptions options;
    private readonly VerificationResultCache cache = new();

    DafnyOptions Options => DafnyOptions.O;

    private DafnyProgramVerifier(ILogger<IProgramVerifier> logger, VerifierOptions options) {
      this.logger = logger;
      this.options = options;
    }

    /// <summary>
    /// Factory method to safely create a new instance of the verifier. It ensures that global/static
    /// settings are set exactly ones.
    /// </summary>
    /// <param name="logger">A logger instance that may be used by this verifier instance.</param>
    /// <param name="options">Settings for the verifier.</param>
    /// <returns>A safely created dafny verifier instance.</returns>
    public static DafnyProgramVerifier Create(ILogger<IProgramVerifier> logger, IOptions<VerifierOptions> options) {
      lock (InitializationSyncObject) {
        if (!initialized) {
          // TODO This may be subject to change. See Microsoft.Boogie.Counterexample
          //      A dash means write to the textwriter instead of a file.
          // https://github.com/boogie-org/boogie/blob/b03dd2e4d5170757006eef94cbb07739ba50dddb/Source/VCGeneration/Couterexample.cs#L217
          DafnyOptions.O.ModelViewFile = "-";
          initialized = true;
          logger.LogTrace("Initialized the boogie verifier with " +
                          "VerifySnapshots={VerifySnapshots}.",
                          DafnyOptions.O.VerifySnapshots);
        }
        return new DafnyProgramVerifier(logger, options.Value);
      }
    }

    private static int GetConfiguredCoreCount(VerifierOptions options) {
      return options.VcsCores == 0
        ? Math.Max(1, Environment.ProcessorCount / 2)
        : Convert.ToInt32(options.VcsCores);
    }

    // 256MB
    private const int TranslatorMaxStackSize = 0x10000000; // 256MB
    static readonly ThreadTaskScheduler TranslatorScheduler = new(TranslatorMaxStackSize);

    public IReadOnlyList<IImplementationTask> Verify(Dafny.Program program, CancellationToken cancellationToken) {

      var errorReporter = (DiagnosticErrorReporter)program.Reporter;
      // Do not set these settings within the object's construction. It will break some tests within
      // VerificationNotificationTest and DiagnosticsTest that rely on updating these settings.
      DafnyOptions.O.TimeLimit = options.TimeLimit;
      DafnyOptions.O.VcsCores = GetConfiguredCoreCount(options);
      DafnyOptions.O.VerifySnapshots = (int)options.VerifySnapshots;
      
#pragma warning disable VSTHRD002
      var translated = Task.Factory.StartNew(() => Translator.Translate(program, errorReporter, new Translator.TranslatorFlags {
        InsertChecksums = true,
        ReportRanges = true
      }).ToList(), CancellationToken.None, TaskCreationOptions.None, TranslatorScheduler).Result;
#pragma warning restore VSTHRD002
      var executionEngine = new ExecutionEngine(DafnyOptions.O, cache);
      var result = translated.SelectMany(t => {
        var (_, boogieProgram) = t;
        var results = executionEngine.GetImplementationTasks(boogieProgram);
        return results;
      }).ToList();
      return result;
    }
  }
}
