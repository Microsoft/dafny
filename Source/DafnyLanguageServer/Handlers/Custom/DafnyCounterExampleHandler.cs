﻿using Microsoft.Boogie;
using Microsoft.Boogie.ModelViewer;
using Microsoft.Boogie.ModelViewer.Dafny;
using Microsoft.Dafny.LanguageServer.Language;
using Microsoft.Dafny.LanguageServer.Workspace;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Dafny.LanguageServer.Handlers.Custom {
  public class DafnyCounterExampleHandler : ICounterExampleHandler {
    private readonly ILogger _logger;
    private readonly IDocumentDatabase _documents;

    public DafnyCounterExampleHandler(ILogger<DafnyCounterExampleHandler> logger, IDocumentDatabase documents) {
      _logger = logger;
      _documents = documents;
    }

    public Task<CounterExampleList> Handle(CounterExampleParams request, CancellationToken cancellationToken) {
      DafnyDocument? document;
      if(!_documents.TryGetDocument(request.TextDocument, out document)) {
        _logger.LogWarning("counter-examples requested for unloaded document {DocumentUri}", request.TextDocument.Uri);
        return Task.FromResult(new CounterExampleList());
      }
      return Task.FromResult(new CounterExampleLoader(_logger, document, cancellationToken).GetCounterExamples());
    }

    private class CounterExampleLoader {
      private const string InitialStateName = "<initial>";
      private static readonly Regex StatePositionRegex = new(
        @".*\.dfy\((?<line>\d+),(?<character>\d+)\)",
        RegexOptions.IgnoreCase | RegexOptions.Singleline
      );

      private readonly ILogger _logger;
      private readonly DafnyDocument _document;
      private readonly CancellationToken _cancellationToken;

      public CounterExampleLoader(ILogger logger, DafnyDocument document, CancellationToken cancellationToken) {
        _logger = logger;
        _document = document;
        _cancellationToken = cancellationToken;
      }

      public CounterExampleList GetCounterExamples() {
        if(_document.SerializedCounterExamples == null) {
          _logger.LogDebug("got no counter-examples for document {DocumentUri}", _document.Uri);
          return new CounterExampleList();
        }
        var counterExamples = GetLanguageSpecificModels(_document.SerializedCounterExamples)
          .SelectMany(GetCounterExamples)
          .ToArray();
        return new CounterExampleList(counterExamples);
      }

      private IEnumerable<ILanguageSpecificModel> GetLanguageSpecificModels(string serializedCounterExamples) {
        using var counterExampleReader = new StringReader(serializedCounterExamples);
        return Model.ParseModels(counterExampleReader)
          .WithCancellation(_cancellationToken)
          .Select(GetLanguagSpecificModel);
      }

      private ILanguageSpecificModel GetLanguagSpecificModel(Model model) {
        // TODO Make view options configurable?
        return Provider.Instance.GetLanguageSpecificModel(model, new ViewOptions { DebugMode = true, ViewLevel = 3 });
      }

      private IEnumerable<CounterExampleItem> GetCounterExamples(ILanguageSpecificModel model) {
        return model.States
          .WithCancellation(_cancellationToken)
          .OfType<StateNode>()
          .Where(state => !IsInitialState(state))
          .Select(GetCounterExample);
      }

      private static bool IsInitialState(StateNode state) {
        return state.Name.Equals(InitialStateName);
      }

      private CounterExampleItem GetCounterExample(StateNode state) {
        return new CounterExampleItem(
          GetPositionFromInitialState(state),
          GetVariablesFromState(state)
        );
      }

      private static Position GetPositionFromInitialState(IState state) {
        var match = StatePositionRegex.Match(state.Name);
        if(!match.Success) {
          throw new ArgumentException($"state does not contain position: {state.Name}");
        }
        // Note: lines in a model start with 1, characters/columns with 0.
        return new Position(
          int.Parse(match.Groups["line"].Value) - 1,
          int.Parse(match.Groups["character"].Value)
        );
      }

      private IDictionary<string, string> GetVariablesFromState(StateNode state) {
        return state.Vars
          .WithCancellation(_cancellationToken)
          .ToDictionary(
            variable => variable.ShortName,
            variable => variable.Value
          );
      }
    }
  }
}
