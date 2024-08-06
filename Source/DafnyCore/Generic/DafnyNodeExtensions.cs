using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Dafny;

namespace Microsoft.Dafny;

public static class DafnyNodeExtensions {
  
  public static Expression WrapExpression(Expression expr) {
    return Expression.CreateParensExpression(new AutoGeneratedToken(expr.tok), expr);
  }
  
  /// <summary>
  /// // Applies plugin-defined docstring filters
  /// </summary>
  public static string GetDocstring(this IHasDocstring node, DafnyOptions options) {
    var plugins = options.Plugins;
    string trivia = node.GetTriviaContainingDocstring();
    if (string.IsNullOrEmpty(trivia)) {
      return null;
    }

    var rawDocstring = ExtractDocstring(trivia);
    foreach (var plugin in plugins) {
      foreach (var docstringRewriter in plugin.GetDocstringRewriters(options)) {
        rawDocstring = docstringRewriter.RewriteDocstring(rawDocstring) ?? rawDocstring;
      }
    }

    return rawDocstring;
  }

  private static string ExtractDocstring(string trivia) {
    var extraction = new Regex(
      $@"(?<multiline>(?<indentation>[ \t]*)/\*(?<multilinecontent>{TriviaFormatterHelper.MultilineCommentContent})\*/)" +
      $@"|(?<singleline>// ?(?<singlelinecontent>[^\r\n]*?)[ \t]*(?:{TriviaFormatterHelper.AnyNewline}|$))");
    var rawDocstring = new List<string>() { };
    var matches = extraction.Matches(trivia);
    for (var i = 0; i < matches.Count; i++) {
      var match = matches[i];
      if (match.Groups["multiline"].Success) {
        // For each line except the first,
        // we need to remove the indentation on every line.
        // The length of removed indentation is maximum the space before the first "/*" + 3 characters
        // Additionally, if there is a "* " or a " *" or a "  * ", we remove it as well
        // provided it always started with a star.
        var indentation = match.Groups["indentation"].Value;
        var multilineContent = match.Groups["multilinecontent"].Value;
        var newlineRegex = new Regex(TriviaFormatterHelper.AnyNewline);
        var contentLines = newlineRegex.Split(multilineContent);
        var starRegex = new Regex(@"^[ \t]*\*\ ?(?<remaining>.*)$");
        var wasNeverInterrupted = true;
        var localDocstring = "";
        for (var j = 0; j < contentLines.Length; j++) {
          if (j != 0) {
            localDocstring += "\n";
          }
          var contentLine = contentLines[j];
          var lineMatch = starRegex.Match(contentLine);
          if (lineMatch.Success && wasNeverInterrupted) {
            localDocstring += lineMatch.Groups["remaining"].Value;
          } else {
            if (j == 0) {
              localDocstring += contentLine.TrimStart();
            } else {
              wasNeverInterrupted = false;
              if (contentLine.StartsWith(indentation)) {
                var trimmedIndentation =
                  contentLine.Substring(0, Math.Min(indentation.Length + 3, contentLine.Length)).TrimStart();
                var remaining = (contentLine.Length >= indentation.Length + 3
                  ? contentLine.Substring(indentation.Length + 3)
                  : "");
                localDocstring += trimmedIndentation + remaining;
              } else {
                localDocstring += contentLine.Trim();
              }
            }
          }
        }

        localDocstring = localDocstring.Trim();
        rawDocstring.Add(localDocstring);
      } else if (match.Groups["singleline"].Success) {
        rawDocstring.Add(match.Groups["singlelinecontent"].Value);
      }
    }

    return string.Join("\n", rawDocstring);
  }
}