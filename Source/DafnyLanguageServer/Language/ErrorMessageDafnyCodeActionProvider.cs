using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Dafny.LanguageServer.Plugins;
using Microsoft.Dafny.LanguageServer.Workspace;
using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace Microsoft.Dafny.LanguageServer;
public class ErrorMessageDafnyCodeActionProvider : DiagnosticDafnyCodeActionProvider {
  private Range InterpretDataAsRangeOrDefault(JToken? data, Range def) {
    if (data is null) {
      return def;
    }
    try {
      String s = data.ToString();
      var nums = s.Split(" ");
      var line = Int32.Parse(nums[0]);
      var column = Int32.Parse(nums[1]);
      var length = Int32.Parse(nums[2]);
      return new Range(line, column, line, column + length);
    } catch (Exception) {
      // Just return the default
    }
    return def;
  }

  protected override IEnumerable<DafnyCodeAction>? GetDafnyCodeActions(IDafnyCodeActionInput input,
    DafnyDiagnostic diagnostic, Range selection) {
    var actionSigs = ErrorRegistry.GetAction(diagnostic.ErrorId);
    var actions = new List<DafnyCodeAction>();
    if (actionSigs != null) {
      var range = diagnostic.Token.ToRange();
      foreach (var sig in actionSigs) {
        var dafnyActions = sig(range);
        actions.AddRange(dafnyActions.Select(dafnyAction => new InstantDafnyCodeAction(dafnyAction.Title, new[] { diagnostic.ToLspDiagnostic() }, dafnyAction.Edits)));
      }
    }
    return actions;
  }
}