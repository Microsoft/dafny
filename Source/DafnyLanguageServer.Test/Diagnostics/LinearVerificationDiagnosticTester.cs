using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Dafny.LanguageServer.IntegrationTest.Extensions;
using Microsoft.Dafny.LanguageServer.IntegrationTest.Util;
using Microsoft.Dafny.LanguageServer.Workspace.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Client;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace Microsoft.Dafny.LanguageServer.IntegrationTest.Diagnostics;

public abstract class LinearVerificationDiagnosticTester : ClientBasedLanguageServerTest {
  protected TestNotificationReceiver<VerificationStatusGutter> VerificationDiagnosticReceiver;
  [TestInitialize]
  public override async Task SetUp() {
    diagnosticReceiver = new();
    VerificationDiagnosticReceiver = new();
    client = await InitializeClient(options =>
      options
        .OnPublishDiagnostics(diagnosticReceiver.NotificationReceived)
        .AddHandler(DafnyRequestNames.VerificationStatusGutter,
          NotificationHandler.For<VerificationStatusGutter>(VerificationDiagnosticReceiver.NotificationReceived))
      );
  }

  public static Dictionary<LineVerificationStatus, string> LineVerificationStatusToString = new() {
    { LineVerificationStatus.Nothing, "   " },
    { LineVerificationStatus.Scheduled, " . " },
    { LineVerificationStatus.Verifying, " S " },
    { LineVerificationStatus.VerifiedObsolete, " I " },
    { LineVerificationStatus.VerifiedVerifying, " $ " },
    { LineVerificationStatus.Verified, " | " },
    { LineVerificationStatus.ErrorContextObsolete, "[I]" },
    { LineVerificationStatus.ErrorContextVerifying, "[S]" },
    { LineVerificationStatus.ErrorContext, "[ ]" },
    { LineVerificationStatus.AssertionFailedObsolete, "[-]" },
    { LineVerificationStatus.AssertionFailedVerifying, "[~]" },
    { LineVerificationStatus.AssertionFailed, "[=]" },
    { LineVerificationStatus.AssertionVerifiedInErrorContextObsolete, "[o]" },
    { LineVerificationStatus.AssertionVerifiedInErrorContextVerifying, "[Q]" },
    { LineVerificationStatus.AssertionVerifiedInErrorContext, "[O]" },
    { LineVerificationStatus.ResolutionError, @"/!\" }
  };

  private static bool IsNotIndicatingProgress(LineVerificationStatus status) {
    return status != LineVerificationStatus.Scheduled &&
           status != LineVerificationStatus.Verifying &&
           status != LineVerificationStatus.AssertionFailedObsolete &&
           status != LineVerificationStatus.AssertionFailedVerifying &&
           status != LineVerificationStatus.VerifiedObsolete &&
           status != LineVerificationStatus.VerifiedVerifying &&
           status != LineVerificationStatus.ErrorContextObsolete &&
           status != LineVerificationStatus.ErrorContextVerifying &&
           status != LineVerificationStatus.AssertionVerifiedInErrorContextObsolete &&
           status != LineVerificationStatus.AssertionVerifiedInErrorContextVerifying;
  }
  public static string RenderTrace(List<LineVerificationStatus[]> statusesTrace, string code) {
    var codeLines = new Regex("\r?\n").Split(code);
    var renderedCode = "";
    for (var line = 0; line < codeLines.Length; line++) {
      if (line != 0) {
        renderedCode += "\n";
      }
      foreach (var statusTrace in statusesTrace) {
        renderedCode += LineVerificationStatusToString[statusTrace[line]];
      }

      renderedCode += ":";
      renderedCode += codeLines[line];
    }

    return renderedCode;
  }

  /// <summary>
  /// Extracts the code from a trace
  /// </summary>
  /// <param name="tracesAndCode"></param>
  /// <returns></returns>
  public static string ExtractCode(string tracesAndCode) {
    var i = 0;
    while (i < tracesAndCode.Length &&
           tracesAndCode[i] != ':' &&
           tracesAndCode[i] != '\n' &&
           (tracesAndCode[i] != '/' || (i + 1 < tracesAndCode.Length && tracesAndCode[i + 1] == '!')) &&
           tracesAndCode[i] != '(') {
      i++;
    }

    // For the first time without trace
    if (i >= tracesAndCode.Length || tracesAndCode[i] != ':') {
      return tracesAndCode;
    }
    var pattern = $"(?<newline>^|\r?\n).{{{i}}}:(?<line>.*)";
    var regexRemoveTrace = new Regex(pattern);
    var codeWithoutTrace = regexRemoveTrace.Replace(tracesAndCode, match =>
      (match.Groups["newline"].Value == "" ? "" : "\n") + match.Groups["line"].Value
    );
    return codeWithoutTrace;
  }

  protected List<LineVerificationStatus[]> previousTraces = null;

  protected async Task<List<LineVerificationStatus[]>> GetAllLineVerificationDiagnostics(TextDocumentItem documentItem) {
    var traces = new List<LineVerificationStatus[]>();
    var maximumNumberOfTraces = 50;
    var previousPerLineDiagnostics
      = previousTraces == null || previousTraces.Count == 0 ? null :
        previousTraces[^1].ToList();
    var nextDiagnostic = await diagnosticReceiver.AwaitNextNotificationAsync(CancellationToken);
    for (; maximumNumberOfTraces > 0; maximumNumberOfTraces--) {
      var verificationDiagnosticReport = await VerificationDiagnosticReceiver.AwaitNextNotificationAsync(CancellationToken);
      if (documentItem.Uri != verificationDiagnosticReport.Uri || documentItem.Version != verificationDiagnosticReport.Version) {
        continue;
      }
      var newPerLineDiagnostics = verificationDiagnosticReport.PerLineStatus.ToList();
      if ((previousPerLineDiagnostics != null
          && previousPerLineDiagnostics.SequenceEqual(newPerLineDiagnostics)) ||
          newPerLineDiagnostics.All(status => status == LineVerificationStatus.Nothing)) {
        continue;
      }

      traces.Add(verificationDiagnosticReport.PerLineStatus);
      if (NoMoreNotificationsToAwaitFrom(verificationDiagnosticReport)) {
        break;
      }

      previousPerLineDiagnostics = newPerLineDiagnostics;
    }

    previousTraces = traces;
    return traces;
  }

  private static bool NoMoreNotificationsToAwaitFrom(VerificationStatusGutter verificationDiagnosticReport) {
    return verificationDiagnosticReport.PerLineStatus.Contains(LineVerificationStatus.ResolutionError) ||
           verificationDiagnosticReport.PerLineStatus.All(IsNotIndicatingProgress) ||
           verificationDiagnosticReport.PerLineStatus.All(status => status == LineVerificationStatus.Nothing);
  }

  /// <summary>
  /// Given some code, will emit the edit like this:
  /// ```
  /// sentence //Next1:sentence2 //Next2:sentence3
  /// ^^^^^^^^^^^^^^^^^ remove
  /// ```
  /// ```
  /// sentence //Next1:\nsentence2 //Next2:sentence3
  /// ^^^^^^^^^^^^^^^^^^^ replace with newline
  /// ```
  /// ```
  /// sentence //Remove1:sentence2 //Next2:sentence3
  /// ^^^^^^^^^^^^^^^^^^^ remove, including the newline before sentence if any
  /// ```
  /// </summary>
  /// <param name="code">The original code with the //Next: comments or //NextN:</param>
  /// <returns></returns>
  public Tuple<string, List<Tuple<Range, string>>> ExtractCodeAndChanges(string code) {
    var lineMatcher = new Regex(@"\r?\n");
    var matcher = new Regex(@"(?<previousNewline>^|\r?\n)(?<toRemove>.*?//(?<newtOrRemove>Next|Remove)(?<id>\d*):(?<newline>\\n)?)");
    var originalCode = code;
    var matches = matcher.Matches(code);
    var changes = new List<Tuple<Range, string>>();
    while (matches.Count > 0) {
      var firstChange = 0;
      Match firstChangeMatch = null;
      for (var i = 0; i < matches.Count; i++) {
        if (matches[i].Groups["id"].Value != "") {
          var intValue = Int32.Parse(matches[i].Groups["id"].Value);
          if (firstChange == 0 || intValue < firstChange) {
            firstChange = intValue;
            firstChangeMatch = matches[i];
          }
        } else {
          firstChangeMatch = matches[i];
          break;
        }
      }

      if (firstChangeMatch == null) {
        break;
      }

      var startRemove =
        firstChangeMatch.Groups["newtOrRemove"].Value == "Next" ?
        firstChangeMatch.Groups["toRemove"].Index :
        firstChangeMatch.Groups["previousNewline"].Index;
      var endRemove = firstChangeMatch.Groups["toRemove"].Index + firstChangeMatch.Groups["toRemove"].Value.Length;

      Position IndexToPosition(int index) {
        var before = code.Substring(0, index);
        var newlines = lineMatcher.Matches(before);
        var line = newlines.Count;
        var character = index;
        if (newlines.Count > 0) {
          var lastNewline = newlines[newlines.Count - 1];
          character = index - (lastNewline.Index + lastNewline.Value.Length);
        }

        return new Position(line, character);
      }

      // For now, simple: Remove the line
      changes.Add(new Tuple<Range, string>(
        new Range(IndexToPosition(startRemove), IndexToPosition(endRemove)), firstChangeMatch.Groups["newline"].Success ? "\n" : ""));
      code = code.Substring(0, startRemove) + code.Substring(endRemove);
      matches = matcher.Matches(code);
    }

    return new Tuple<string, List<Tuple<Range, string>>>(originalCode, changes);
  }

  // If testTrace is false, codeAndTree should not contain a trace to test.
  public async Task VerifyTrace(string codeAndTrace, bool testTrace = true) {
    codeAndTrace = codeAndTrace[0] == '\n' ? codeAndTrace.Substring(1) :
      codeAndTrace.Substring(0, 2) == "\r\n" ? codeAndTrace.Substring(2) :
      codeAndTrace;
    var codeAndChanges = testTrace ? ExtractCode(codeAndTrace) : codeAndTrace;
    var (code, changes) = ExtractCodeAndChanges(codeAndChanges);
    var documentItem = CreateTestDocument(code);
    await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
    var traces = new List<LineVerificationStatus[]>();
    traces.AddRange(await GetAllLineVerificationDiagnostics(documentItem));
    foreach (var (range, inserted) in changes) {
      ApplyChange(ref documentItem, range, inserted);
      traces.AddRange(await GetAllLineVerificationDiagnostics(documentItem));
    }

    if (testTrace) {
      var traceObtained = RenderTrace(traces, code);
      var ignoreQuestionMarks = AcceptQuestionMarks(traceObtained, codeAndTrace);
      var expected = "\n" + codeAndTrace + "\n";
      var actual = "\n" + ignoreQuestionMarks + "\n";
      AssertWithDiff.Equal(expected, actual);
    }
  }

  // Finds all the "?" at the beginning in expected and replace the characters at the same position in traceObtained
  // by a question mark, so that we don't care what is verified first.
  // Do this only if lengths are the same
  public string AcceptQuestionMarks(string traceObtained, string expected) {
    if (traceObtained.Length != expected.Length) {
      return traceObtained;
    }

    var toFindRegex = new Regex(@"(?<=(?:^|\n)[^:]*)\?");
    var matches = toFindRegex.Matches(expected);
    var pattern = "";
    for (var matchIndex = 0; matchIndex < matches.Count; matchIndex++) {
      pattern += (pattern == "" ? "" : "|")
                 + (@"(?<=^[\S\s]{" + matches[matchIndex].Index + @"}).");
    }

    if (pattern == "") {
      return traceObtained;
    }

    var toReplaceRegex = new Regex(pattern);
    return toReplaceRegex.Replace(traceObtained, "?");
  }
}