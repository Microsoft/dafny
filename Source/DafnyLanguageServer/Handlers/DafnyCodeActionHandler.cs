﻿using System;
using System.Collections.Concurrent;
using Microsoft.Dafny.LanguageServer.Language;
using Microsoft.Dafny.LanguageServer.Workspace;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Dafny.LanguageServer.Plugins;
using Newtonsoft.Json.Linq;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace Microsoft.Dafny.LanguageServer.Handlers;

public class DafnyCodeActionHandler : CodeActionHandlerBase {
  private readonly ILogger<DafnyCodeActionHandler> logger;
  private readonly IDocumentDatabase documents;

  public DafnyCodeActionHandler(ILogger<DafnyCodeActionHandler> logger, IDocumentDatabase documents) {
    this.logger = logger;
    this.documents = documents;
  }

  public record DafnyCodeActionWithId(DafnyCodeAction DafnyCodeAction, int Id);

  protected override CodeActionRegistrationOptions CreateRegistrationOptions(CodeActionCapability capability,
    ClientCapabilities clientCapabilities) {
    return new CodeActionRegistrationOptions {
      DocumentSelector = DocumentSelector.ForLanguage("dafny"),
      ResolveProvider = true,
      CodeActionKinds = Container<CodeActionKind>.From(
        CodeActionKind.QuickFix
      ),
      WorkDoneProgress = false
    };
  }


  /// <summary>
  /// Returns the fixes along with a unique identifier
  /// </summary>
  private IEnumerable<DafnyCodeActionWithId> GetFixesWithIds(IEnumerable<DafnyCodeActionProvider> fixers, DocumentAfterParsing document, CodeActionParams request) {
    var id = 0;
    return fixers.SelectMany(fixer => {
      var fixerInput = new DafnyCodeActionInput(document);
      var quickFixes = fixer.GetDafnyCodeActions(fixerInput, request.Range);
      var fixerCodeActions = quickFixes.Select(quickFix =>
        new DafnyCodeActionWithId(quickFix, id++));
      return fixerCodeActions;
    });
  }

  private readonly ConcurrentDictionary<string, IReadOnlyList<DafnyCodeActionWithId>> documentUriToDafnyCodeActiones = new();

  public override async Task<CommandOrCodeActionContainer> Handle(CodeActionParams request, CancellationToken cancellationToken) {
    var document = await documents.GetLastDocumentAsync(request.TextDocument);
    if (document == null) {
      logger.LogWarning("dafny code actions requested for unloaded document {DocumentUri}", request.TextDocument.Uri);
      return new CommandOrCodeActionContainer();
    }
    var quickFixers = GetDafnyCodeActionProviders();
    var fixesWithId = GetFixesWithIds(quickFixers, document, request).ToArray();

    var documentUri = document.Uri.ToString();
    documentUriToDafnyCodeActiones.AddOrUpdate(documentUri, _ => fixesWithId, (_, _) => fixesWithId);
    var codeActions = fixesWithId.Select(fixWithId => {
      CommandOrCodeAction t = new CodeAction {
        Title = fixWithId.DafnyCodeAction.Title,
        Data = new JArray(documentUri, fixWithId.Id),
        Diagnostics = fixWithId.DafnyCodeAction.Diagnostics
      };
      return t;
    }
    ).ToArray();
    return new CommandOrCodeActionContainer(codeActions);
  }


  public class ErrorMessageCodeActionProvider : DiagnosticDafnyCodeActionProvider {
    private Range InterpretDataAsRangeOrDefault(JToken? data, Range def) {
      if (data is null) return def;
      try {
        var sl = data.First?.First?.First?.ToString();
        var sc = data.First?.First?.Last?.ToString();
        var el = data.Last?.First?.First?.ToString();
        var ec = data.Last?.First?.Last?.ToString();
        if (sl == null || sc == null || el == null || ec == null) return def;
        int sline = Int32.Parse(sl.Substring(sl.IndexOf(":") + 1));
        int schar = Int32.Parse(sc.Substring(sc.IndexOf(":") + 1));
        int eline = Int32.Parse(el.Substring(el.IndexOf(":") + 1));
        int echar = Int32.Parse(ec.Substring(ec.IndexOf(":") + 1));
        return new Range(sline, schar, eline, echar);
      } catch (Exception) {
      }
      return def;
    }

    protected override IEnumerable<DafnyCodeAction>? GetDafnyCodeActions(IDafnyCodeActionInput input, Diagnostic diagnostic, Range selection) {
      //if (diagnostic.Code == "") return new List<DafnyCodeAction> { };
      var action = DafnyCodeActions.GetAction(diagnostic.Code);
      if (action == null) return new List<DafnyCodeAction> { };
      Range range = InterpretDataAsRangeOrDefault(diagnostic.Data, diagnostic.Range);
      return action(diagnostic, range);
    }
  }

  private DafnyCodeActionProvider[] GetDafnyCodeActionProviders() {
    return new List<DafnyCodeActionProvider>() {
      new VerificationDafnyCodeActionProvider()
    }
    .Concat(new List<DafnyCodeActionProvider> { new ErrorMessageCodeActionProvider() })
    .Concat(
      DafnyOptions.O.Plugins.SelectMany(plugin =>
        plugin is ConfiguredPlugin { Configuration: PluginConfiguration configuration } ?
            configuration.GetDafnyCodeActionProviders() : new DafnyCodeActionProvider[] { })).ToArray();
  }

  public override Task<CodeAction> Handle(CodeAction request, CancellationToken cancellationToken) {
    var command = request.Data;
    if (command == null || command.Count() < 2 || command[1] == null) {
      return Task.FromResult(request);
    }

    string? documentUri = command[0]?.Value<string>();
    if (documentUri == null || !documentUriToDafnyCodeActiones.TryGetValue(documentUri, out var quickFixes)) {
      return Task.FromResult(request);
    }

    int? id = command[1]?.Value<int>();
    if (id == null) {
      return Task.FromResult(request);
    }

    DafnyCodeActionWithId? selectedDafnyCodeAction = quickFixes.Where(pluginDafnyCodeAction => pluginDafnyCodeAction.Id == id)
      .FirstOrDefault((DafnyCodeActionWithId?)null);
    if (selectedDafnyCodeAction == null) {
      return Task.FromResult(request);
    }

    return Task.FromResult(new CodeAction {
      Edit = new WorkspaceEdit() {
        DocumentChanges = new Container<WorkspaceEditDocumentChange>(
          new WorkspaceEditDocumentChange(new TextDocumentEdit() {
            TextDocument = new OptionalVersionedTextDocumentIdentifier() {
              Uri = documentUri
            },
            Edits = new TextEditContainer(GetTextEdits(selectedDafnyCodeAction.DafnyCodeAction.GetEdits()))
          }))
      }
    });
  }

  private IEnumerable<TextEdit> GetTextEdits(IEnumerable<DafnyCodeActionEdit> quickFixEdits) {
    var edits = new List<TextEdit>();
    foreach (var (range, toReplace) in quickFixEdits) {
      edits.Add(new TextEdit() {
        NewText = toReplace,
        Range = range
      });
    }
    return edits;
  }
}

public class DafnyCodeActionInput : IDafnyCodeActionInput {
  public DafnyCodeActionInput(DocumentAfterParsing document) {
    Document = document;
  }

  public string Uri => Document.Uri.ToString();
  public int Version => Document.Version;
  public string Code => Document.TextDocumentItem.Text;
  public Dafny.Program Program => Document.Program;
  public DocumentAfterParsing Document { get; }

  public Diagnostic[] Diagnostics {
    get {
      var result = Document.Diagnostics.ToArray();
      return result;
    }
  }

  public string Extract(Range range) {
    var buffer = Document.TextDocumentItem;
    try {
      return buffer.Extract(range);
    } catch (ArgumentException) {
      return "";
    }
  }
}
