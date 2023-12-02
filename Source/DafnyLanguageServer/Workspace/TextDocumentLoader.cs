﻿using Microsoft.Dafny.LanguageServer.Language;
using Microsoft.Dafny.LanguageServer.Language.Symbols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Dafny.Compilers;
using Microsoft.Extensions.Logging;

namespace Microsoft.Dafny.LanguageServer.Workspace {
  /// <summary>
  /// Text document loader implementation that offloads the whole load procedure on one dedicated
  /// thread with a stack size of 256MB. Since only one thread is used, document loading is implicitely synchronized.
  /// The verification runs on the calling thread.
  /// </summary>
  /// <remarks>
  /// The increased stack size is necessary to solve the issue https://github.com/dafny-lang/dafny/issues/1447.
  /// </remarks>
  public class TextDocumentLoader : ITextDocumentLoader {
    private readonly ILogger<ITextDocumentLoader> logger;
    private readonly IDafnyParser parser;
    private readonly ISymbolResolver symbolResolver;
    private readonly ISymbolTableFactory symbolTableFactory;
    private readonly IGhostStateDiagnosticCollector ghostStateDiagnosticCollector;

    protected TextDocumentLoader(
      ILogger<ITextDocumentLoader> documentLoader,
      IDafnyParser parser,
      ISymbolResolver symbolResolver,
      ISymbolTableFactory symbolTableFactory,
      IGhostStateDiagnosticCollector ghostStateDiagnosticCollector) {
      this.logger = documentLoader;
      this.parser = parser;
      this.symbolResolver = symbolResolver;
      this.symbolTableFactory = symbolTableFactory;
      this.ghostStateDiagnosticCollector = ghostStateDiagnosticCollector;
    }

    public static TextDocumentLoader Create(
      IDafnyParser parser,
      ISymbolResolver symbolResolver,
      ISymbolTableFactory symbolTableFactory,
      IGhostStateDiagnosticCollector ghostStateDiagnosticCollector,
      ILogger<ITextDocumentLoader> logger
      ) {
      return new TextDocumentLoader(logger, parser, symbolResolver, symbolTableFactory, ghostStateDiagnosticCollector);
    }

    public async Task<Program> ParseAsync(ErrorReporter errorReporter, CompilationInput compilation, CancellationToken cancellationToken) {
#pragma warning disable CS1998
      return await await DafnyMain.LargeStackFactory.StartNew(
        async () => ParseInternal(errorReporter, compilation, cancellationToken), cancellationToken
#pragma warning restore CS1998
      );
    }

    private Program ParseInternal(ErrorReporter errorReporter, CompilationInput compilation,
      CancellationToken cancellationToken) {
      var program = parser.Parse(compilation, errorReporter, cancellationToken);
      compilation.Project.Errors.CopyDiagnostics(program.Reporter);
      var projectPath = compilation.Project.Uri.LocalPath;
      if (projectPath.EndsWith(DafnyProject.FileName)) {
        var projectDirectory = Path.GetDirectoryName(projectPath)!;
        var filesMessage = string.Join("\n", compilation.RootUris.Select(uri => Path.GetRelativePath(projectDirectory, uri.LocalPath)));
        if (filesMessage.Any()) {
          program.Reporter.Info(MessageSource.Parser, compilation.Project.StartingToken, "Files referenced by project are:" + Environment.NewLine + filesMessage);
        } else {
          program.Reporter.Warning(MessageSource.Parser, CompilerErrors.ErrorId.None, compilation.Project.StartingToken, "Project references no files");
        }
      }

      return program;
    }

    public async Task<ResolutionResult> ResolveAsync(CompilationInput input,
      Program program,
      CancellationToken cancellationToken) {
#pragma warning disable CS1998
      return await await DafnyMain.LargeStackFactory.StartNew(
        async () => ResolveInternal(input, program, cancellationToken), cancellationToken);
#pragma warning restore CS1998
    }

    private ResolutionResult ResolveInternal(CompilationInput input, Program program, CancellationToken cancellationToken) {

      var errorReporter = (ObservableErrorReporter)program.Reporter;
      if (errorReporter.HasErrors) {
        throw new TaskCanceledException();
      }

      var compilationUnit = symbolResolver.ResolveSymbols(input.Project, program, cancellationToken);
      var legacySymbolTable = symbolTableFactory.CreateFrom(compilationUnit, cancellationToken);

      var newSymbolTable = errorReporter.HasErrors
        ? null
        : symbolTableFactory.CreateFrom(program, cancellationToken);

      var ghostDiagnostics = ghostStateDiagnosticCollector.GetGhostStateDiagnostics(legacySymbolTable, cancellationToken);

      List<ICanVerify>? verifiables;
      if (errorReporter.HasErrorsUntilResolver) {
        verifiables = null;
      } else {
        var symbols = SymbolExtensions.GetSymbolDescendants(program.DefaultModule);
        verifiables = symbols.OfType<ICanVerify>().Where(v => !AutoGeneratedToken.Is(v.RangeToken) &&
                                                              v.ContainingModule.ShouldVerify(program.Compilation) &&
                                                              v.ShouldVerify(program.Compilation) &&
                                                              v.ShouldVerify).ToList();
      }

      return new ResolutionResult(
        program,
        newSymbolTable,
        legacySymbolTable,
        ghostDiagnostics,
        verifiables
      );
    }
  }
}
