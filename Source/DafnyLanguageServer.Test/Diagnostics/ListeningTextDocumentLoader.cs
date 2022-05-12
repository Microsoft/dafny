﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Boogie;
using Microsoft.Dafny.LanguageServer.Language;
using Microsoft.Dafny.LanguageServer.Language.Symbols;
using Microsoft.Dafny.LanguageServer.Workspace;
using Microsoft.Extensions.Logging;

namespace Microsoft.Dafny.LanguageServer.IntegrationTest.Diagnostics;

public class ListeningTextDocumentLoader : TextDocumentLoader {
  public List<List<int>> LinearPriorities = new List<List<int>>();

  public ListeningTextDocumentLoader(
    [NotNull] ILoggerFactory loggerFactory, [NotNull] IDafnyParser parser,
    [NotNull] ISymbolResolver symbolResolver, [NotNull] IProgramVerifier verifier,
    [NotNull] ISymbolTableFactory symbolTableFactory,
    [NotNull] IGhostStateDiagnosticCollector ghostStateDiagnosticCollector,
    [NotNull] ICompilationStatusNotificationPublisher notificationPublisher,
    [NotNull] IDiagnosticPublisher diagnosticPublisher) : base(loggerFactory, parser, symbolResolver, verifier,
    symbolTableFactory, ghostStateDiagnosticCollector, notificationPublisher, diagnosticPublisher) {
  }



  public override VerificationProgressReporter CreateVerificationProgressReporter(DafnyDocument document) {
    return new ListeningVerificationProgressReporter(
      loggerFactory.CreateLogger<ListeningVerificationProgressReporter>(),
      document, notificationPublisher, diagnosticPublisher, this);
  }

  public void RecordImplementationsPriority(List<int> priorityListPerImplementation) {
    LinearPriorities.Add(priorityListPerImplementation);
  }
}

public class ListeningVerificationProgressReporter : VerificationProgressReporter {
  public ListeningTextDocumentLoader TextDocumentLoader { get; }

  public ListeningVerificationProgressReporter(
    [NotNull] ILogger<VerificationProgressReporter> logger,
    [NotNull] DafnyDocument document,
    [NotNull] ICompilationStatusNotificationPublisher publisher,
    [NotNull] IDiagnosticPublisher diagnosticPublisher,
    ListeningTextDocumentLoader textDocumentLoader
    )
    : base(logger, document, publisher, diagnosticPublisher) {
    TextDocumentLoader = textDocumentLoader;
  }

  public override void ReportImplementationsBeforeVerification(Implementation[] implementations) {
    base.ReportImplementationsBeforeVerification(implementations);
    TextDocumentLoader.RecordImplementationsPriority(implementations.Select(implementation => implementation.Priority)
      .ToList());
  }
}