using System;
using System.Collections.Generic;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;
using Microsoft.Dafny.LanguageServer.Plugins;
using Newtonsoft.Json.Linq;
using static Microsoft.Dafny.ErrorRegistry;

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

  protected override IEnumerable<DafnyCodeAction>? GetDafnyCodeActions(IDafnyCodeActionInput input, Diagnostic diagnostic, Range selection) {
    var actionSigs = GetAction(diagnostic.Code);
    var actions = new List<DafnyCodeAction>();
    if (actionSigs != null) {
      var range = InterpretDataAsRangeOrDefault(diagnostic.Data, diagnostic.Range);
      foreach (var sig in actionSigs) {
        actions.AddRange(sig(input, diagnostic, range));
      }
    }
    return actions;
  }
}