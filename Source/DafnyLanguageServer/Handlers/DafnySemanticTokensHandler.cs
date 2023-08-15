﻿using Microsoft.Dafny.LanguageServer.Workspace;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Dafny.LanguageServer.Language.SemanticTokens;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Boogie;
using Microsoft.Dafny.LanguageServer.Language;
using OmniSharp.Extensions.LanguageServer.Protocol;

namespace Microsoft.Dafny.LanguageServer.Handlers {
  public class DafnySemanticTokensHandler : SemanticTokensHandlerBase {
    private readonly ILogger logger;
    private readonly IDocumentDatabase documents;

    public DafnySemanticTokensHandler(ILogger<DafnySemanticTokensHandler> logger, IDocumentDatabase documents) {
      this.logger = logger;
      this.documents = documents;
    }

    protected override SemanticTokensRegistrationOptions CreateRegistrationOptions(SemanticTokensCapability capability,
      ClientCapabilities clientCapabilities) {
      return new SemanticTokensRegistrationOptions {
        DocumentSelector = DocumentSelector.ForLanguage("dafny"),
        Legend = new SemanticTokensLegend() { // FIXME adjust depending on client (capability.TokenModifiers / capability.TokenTypes)
          TokenModifiers = new Container<SemanticTokenModifier>(SemanticTokenModifier.Defaults),
          TokenTypes = new Container<SemanticTokenType>(SemanticTokenType.Defaults),
        },
        Full = new SemanticTokensCapabilityRequestFull {
          Delta = true
        },
        Range = true
      };
    }

    private void CollectScannerTokens(DafnySemanticTokensBuilder builder, Dafny.Program program) {
      var tok = program.GetFirstTopLevelToken();
      while (tok != null) {
        foreach (var leadingComment in tok.LeadingComments) {
          builder.Push("parser", leadingComment);
        }
        builder.Push("parser", tok);
        foreach (var trailingComment in tok.TrailingComments) {
          builder.Push("parser", trailingComment);
        }
        tok = tok.Next;
      }
    }

    private void CollectResolutionBasedTokens(DafnySemanticTokensBuilder builder, Dafny.Program program) {
      //new LspSemanticTokensGeneratingVisitor(builder).Visit(program);
      program.Visit((Node n) => {
        switch (n) {
          case DatatypeDecl decl: {
              builder.Push("resolution", decl.NameToken, SemanticTokenType.Type);
              break;
            }
          case Type type: {
              builder.Push("resolution", type.tok, SemanticTokenType.Type);
              break;
            }
        }

        return true;
      });
    }

    protected override async Task Tokenize(SemanticTokensBuilder builder, ITextDocumentIdentifierParams identifier,
      CancellationToken cancellationToken) {
      var documentManager = documents.GetDocumentManager(identifier.TextDocument);
      if (documentManager == null) {
        logger.LogWarning("Tokens requested for unloaded document {DocumentUri}", identifier.TextDocument.Uri);
        return;
      }
      var document = await documentManager.GetLastDocumentAsync();
      var dafnyBuilder = new DafnySemanticTokensBuilder(builder, logger);
      CollectScannerTokens(dafnyBuilder, document.Program);
      CollectResolutionBasedTokens(dafnyBuilder, document.Program);
    }

    protected override Task<SemanticTokensDocument> GetSemanticTokensDocument(ITextDocumentIdentifierParams @params, CancellationToken cancellationToken) {
      return Task.FromResult(new SemanticTokensDocument(RegistrationOptions.Legend));
    }
  }
}
