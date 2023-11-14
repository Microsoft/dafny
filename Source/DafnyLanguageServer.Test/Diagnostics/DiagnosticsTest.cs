﻿using System;
using Microsoft.Dafny.LanguageServer.IntegrationTest.Extensions;
using Microsoft.Dafny.LanguageServer.IntegrationTest.Util;
using Microsoft.Dafny.LanguageServer.Workspace;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Dafny.LanguageServer.Workspace.ChangeProcessors;
using Newtonsoft.Json;
using NuGet.Frameworks;
using Xunit.Abstractions;
using Xunit;
using Xunit.Sdk;
using XunitAssertMessages;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace Microsoft.Dafny.LanguageServer.IntegrationTest.Synchronization {
  public class DiagnosticsTest : ClientBasedLanguageServerTest {
    private readonly string testFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Synchronization/TestFiles");

    [Fact]
    public async Task RedundantAssumptionsGetWarnings() {
      var path = Path.Combine(testFilesDirectory, "ProofDependencies/LSPProofDependencyTest.dfy");
      var documentItem = CreateTestDocument(await File.ReadAllTextAsync(path), path);
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);

      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Equal(3, diagnostics.Length);
      Assert.Contains(diagnostics, diagnostic =>
        diagnostic.Severity == DiagnosticSeverity.Warning &&
        diagnostic.Range == new Range(3, 11, 3, 16) &&
        diagnostic.Message == "unnecessary assumption"
        );
      Assert.Contains(diagnostics, diagnostic =>
        diagnostic.Severity == DiagnosticSeverity.Warning &&
        diagnostic.Range == new Range(13, 11, 13, 17) &&
        diagnostic.Message == "proved using contradictory assumptions: assertion always holds"
      );
      Assert.Contains(diagnostics, diagnostic =>
        diagnostic.Severity == DiagnosticSeverity.Warning &&
        diagnostic.Range == new Range(12, 11, 12, 17) &&
        diagnostic.Message == "unnecessary assumption"
      );
    }

    [Fact]
    public async Task LeastLemmaIsVerified() {
      var source = @"
least lemma Foo()
  ensures false
{}";
      var document = await CreateOpenAndWaitForResolve(source);
      var diagnostics = await GetLastDiagnostics(document);
      Assert.NotEmpty(diagnostics);
    }

    [Fact(Skip = "Not implemented. Requires separating diagnostics from different sources")]
    public async Task FixedParseErrorUpdatesBeforeResolution() {
      var source = @"
mfunction HasParseAndResolutionError(): int {
  true
}".TrimStart();

      var document = await CreateOpenAndWaitForResolve(source);
      var parseDiagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Equal(MessageSource.Parser.ToString(), parseDiagnostics[0].Source);
      ApplyChange(ref document, new Range(0, 0, 0, 1), " ");
      var parseDiagnostics2 = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Empty(parseDiagnostics2);
      var resolutionDiagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Single(resolutionDiagnostics);
    }

    [Fact]
    public async Task SelectedTriggersDiagnosticsDoesNotDisappear() {
      var producerSource = @"
module Producer {
  function Zoo(): set<(int,int)> {
    set x: int | 0 <= x < 5, y | 0 <= y < 6 :: (x,y)
  }

  const used := 3
}
".TrimStart();

      var consumerSource = @"
module Consumer {
  import Producer
  const user := Producer.used + 4
}
".TrimStart();

      var directory = Path.GetRandomFileName();
      var project = await CreateOpenAndWaitForResolve("", Path.Combine(directory, DafnyProject.FileName));
      var producer = await CreateOpenAndWaitForResolve(producerSource, Path.Combine(directory, "producer.dfy"));
      var consumer = await CreateOpenAndWaitForResolve(consumerSource, Path.Combine(directory, "consumer.dfy"));
      var diag1 = await GetLastDiagnostics(producer, DiagnosticSeverity.Hint, allowStale: true);
      Assert.Equal(DiagnosticSeverity.Hint, diag1[0].Severity);
      ApplyChange(ref consumer, new Range(0, 0, 0, 0), "//trigger change\n");
      await AssertNoDiagnosticsAreComing(CancellationToken, producer);
    }


    [Fact]
    public async Task CorrectParseDiagnosticsDoNotOverridePreviousResolutionOnes() {
      var source = @"
function HasResolutionError(): int {
  true
}".TrimStart();

      var document = await CreateOpenAndWaitForResolve(source);
      var resolutionDiagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Equal(MessageSource.Resolver.ToString(), resolutionDiagnostics[0].Source);
      ApplyChange(ref document, new Range(3, 0, 3, 0), "// comment to trigger update\n");
      await AssertNoDiagnosticsAreComing(CancellationToken);
      ApplyChange(ref document, new Range(1, 0, 1, 0), "disturbFunctionKeyword");
      var parseDiagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Equal(MessageSource.Parser.ToString(), parseDiagnostics[0].Source);
    }

    [Fact]
    public async Task DiagnosticsForVerificationTimeoutHasNameAsRange() {
      var documentItem = CreateTestDocument(SlowToVerify, "DiagnosticsForVerificationTimeout.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Contains("Verification out of resource", diagnostics[0].Message);
      Assert.Equal(new Range(0, 20, 0, 42), diagnostics[0].Range);
    }

    [Fact]
    public async Task NoFlickeringWhenMixingCorrectAndErrorBatches() {
      var source = @"
method {:vcs_split_on_every_assert} Foo(x: int) {
  if (x == 0) {
    assert true;
  } else if (x == 1) {
    assert true;
  } else {
    assert false;
  }
}";
      var document = await CreateOpenAndWaitForResolve(source);
      var status1 = await verificationStatusReceiver.AwaitNextNotificationAsync(CancellationToken);
      Assert.Equal(PublishedVerificationStatus.Stale, status1.NamedVerifiables[0].Status);
      var status12 = await verificationStatusReceiver.AwaitNextNotificationAsync(CancellationToken);
      Assert.Equal(PublishedVerificationStatus.Queued, status12.NamedVerifiables[0].Status);
      var status2 = await verificationStatusReceiver.AwaitNextNotificationAsync(CancellationToken);
      Assert.Equal(PublishedVerificationStatus.Running, status2.NamedVerifiables[0].Status);
      var status3 = await verificationStatusReceiver.AwaitNextNotificationAsync(CancellationToken);
      Assert.Equal(PublishedVerificationStatus.Error, status3.NamedVerifiables[0].Status);
      await AssertNoVerificationStatusIsComing(document, CancellationToken);
    }

    [Fact]
    public async Task IncrementalBatchDiagnostics() {
      var source = @"
method {:vcs_split_on_every_assert} Foo(x: int) {
  if (x == 0) {
    assert false;
  } else {
    assert false;
  }
}";
      await CreateOpenAndWaitForResolve(source);
      var diagnostics1 = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Single(diagnostics1);
      var diagnostics2 = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Equal(2, diagnostics2.Length);
    }

    [Fact]
    public async Task ResolutionErrorInDifferentFileBlocksVerification() {
      var source = @"
include ""./semanticError.dfy""
method Foo() ensures false { 
  var x := SemanticError.untypedExport; 
}
";

      var documentItem = CreateTestDocument(source, Path.Combine(testFilesDirectory, "test.dfy"));
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);

      var diagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Single(diagnostics);
      Assert.Contains("semanticError.dfy", diagnostics[0].Message);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }
    [Fact]
    public async Task GitIssue3155ItemWithSameKeyAlreadyBeenAdded() {
      var source = @"
datatype Test =
    | A(field: int)
    | B(field: int)


predicate updateTest(test: Test, test': Test)
{
    test' == test.(field := 1)
}
".TrimStart();
      var documentItem = CreateTestDocument(source, "GitIssue3155ItemWithSameKeyAlreadyBeenAdded.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task GitIssue3062CrashOfLanguageServer() {
      var source = @"
function bullspec(s:seq<nat>, u:seq<nat>): (r: nat)
  requires |s| > 0
  requires |u| > 0
  requires |s| == |u|
  ensures forall i | i < r :: s[i] != u[i]
  ensures r == |s| || s[r] == u[r]
{
  if |s| == 0 then 0 else
  if |s| == 1 then
    if s[0]==u[0] 
    then 1 else 0
  else
    if s[0] != u[0] 
    then bullspec(s[1..],u[1..])
    else 1+bullspec(s[1..],u[1..])
}".TrimStart();
      var documentItem = CreateTestDocument(source, "GitIssue3062CrashOfLanguageServer.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      Assert.Equal(PublishedVerificationStatus.Stale, await PopNextStatus());
      Assert.Equal(PublishedVerificationStatus.Queued, await PopNextStatus());
      Assert.Equal(PublishedVerificationStatus.Running, await PopNextStatus());
      Assert.Equal(PublishedVerificationStatus.Error, await PopNextStatus());
      var diagnostics1 = diagnosticsReceiver.GetLast(documentItem);
      Assert.Equal(4, diagnostics1.Length);
      ApplyChange(ref documentItem, ((7, 25), (10, 17)), "");
      var diagnostics2 = await GetNextDiagnostics(documentItem);
      Assert.Equal(5, diagnostics2.Length);
      Assert.Equal("Parser", diagnostics2[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics2[0].Severity);
      ApplyChange(ref documentItem, ((7, 20), (7, 25)), "");
      Assert.Equal(PublishedVerificationStatus.Stale, await PopNextStatus());
      Assert.Equal(PublishedVerificationStatus.Queued, await PopNextStatus());
      Assert.Equal(PublishedVerificationStatus.Running, await PopNextStatus());
      Assert.Equal(PublishedVerificationStatus.Error, await PopNextStatus());
      var diagnostics3 = diagnosticsReceiver.GetLast(documentItem);
      Assert.Equal(6, diagnostics3.Length);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task EmptyFileNoCodeWarning() {
      var source = "";
      var documentItem = CreateTestDocument(source, "EmptyFileNoCodeWarning.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Equal(new Range(0, 0, 0, 0), diagnostics[0].Range);
    }

    [Fact]
    public async Task OpeningFailingFunctionPreconditionHasRelatedDiagnostics() {
      var source = @"
predicate P(i: int) {
  i <= 0
}

function Call(i: int): int
  requires P(i)

method Test(i: int) returns (j: int)
  requires i > 0
{
  return Call(2/i);
}".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningFailingFunctionPreconditionHasRelatedDiagnostics.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.NotNull(diagnostics[0].RelatedInformation);
      var relatedInformation =
        diagnostics[0].RelatedInformation.ToArray();
      Assert.Equal(2, relatedInformation.Length);
      Assert.Equal("Could not prove: P(i)", relatedInformation[0].Message);
      Assert.Equal("Could not prove: i <= 0", relatedInformation[1].Message);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningFlawlessDocumentReportsNoDiagnostics() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires y >= 0 && x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningFlawlessDocumentReportsNoDiagnostics.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task RefinementTokensCorrectlyReported() {
      // git-issue-2402
      var source = @"
abstract module M 
{ 
    type T(00)
    const t : T
    lemma Randomlemma()
    {
        forall i:int {}
    }
}

module N refines M 
{
}".TrimStart();
      var documentItem = CreateTestDocument(source, "RefinementTokensCorrectlyReported.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Equal(2, diagnostics.Length);
      Assert.Contains(diagnostics, d => d.Message.Contains(
        "static non-ghost const field 't' of type 'T' (which does not have a default compiled value) must give a defining value"));
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task NoCrashWhenPressingEnterAfterSelectingAllTextAndInputtingText() {
      var source = @"
predicate {:opaque} m() {
  true
}
".TrimStart();
      var documentItem = CreateTestDocument(source, "NoCrashWhenPressingEnterAfterSelectingAllTextAndInputtingText.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      await AssertNoDiagnosticsAreComing(CancellationToken, documentItem);
      ApplyChange(ref documentItem, ((0, 0), (3, 0)), "\n");

      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      ApplyChange(ref documentItem, ((1, 0), (1, 0)), "const x := 1");
      var diagnostics2 = await GetLastDiagnostics(documentItem);
      Assert.Empty(diagnostics2);
      await AssertNoDiagnosticsAreComing(CancellationToken, documentItem);
    }

    [Fact]
    public async Task OpeningOpaqueFunctionWorks() {
      var source = @"
predicate {:opaque} m() {
  true
}".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningOpaqueFunctionWorks.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithSyntaxErrorReportsDiagnosticsWithParserErrors() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int
  requires y >= 0 && x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithSyntaxErrorReportsDiagnosticsWithParserErrors.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Single(diagnostics);
      Assert.Equal("Parser", diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithSemanticErrorReportsDiagnosticsWithSemanticErrors() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires y >= 0 && x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - ""1"");
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithSemanticErrorReportsDiagnosticsWithSemanticErrors.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Single(diagnostics);
      Assert.Equal("Resolver", diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithMultipleSemanticErrorsReportsDiagnosticsWithAllSemanticErrors() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires y >= 0 && x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == ""0"" {
    product := 0;
  } else {
    var step := Multiply(x, y - ""1"");
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithMultipleSemanticErrorsReportsDiagnosticsWithAllSemanticErrors.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Equal(2, diagnostics.Length);
      Assert.Equal("Resolver", diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      Assert.Equal("Resolver", diagnostics[1].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[1].Severity);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithVerificationErrorReportsDiagnosticsWithVerificationErrors() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithVerificationErrorReportsDiagnosticsWithVerificationErrors.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.Equal(MessageSource.Verifier.ToString(), diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithDefaultParamErrorHighlightsCallSite() {
      var source = @"
function test(x: int := 99): bool
  requires x >= 100
{
  false
}

method A()
{
  var b := test();
} ".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithDefaultParamErrorHighlightsCallSite.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.Equal(MessageSource.Verifier.ToString(), diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      Assert.Equal(8, diagnostics[0].Range.Start.Line);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithVerificationErrorDoesNotReportDiagnosticsWithVerificationErrorsIfNotVerifyOnChange() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      await SetUp(options => options.Set(ProjectManager.Verification, VerifyOnMode.Never));
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithVerificationErrorDoesNotReportDiagnosticsWithVerificationErrorsIfNotVerifyOnChange.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithMultipleVerificationErrorsReportsDiagnosticsWithAllVerificationErrorsAndRelatedInformation() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithMultipleVerificationErrorsReportsDiagnosticsWithAllVerificationErrorsAndRelatedInformation.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Equal(2, diagnostics.Length);
      Assert.Equal(MessageSource.Verifier.ToString(), diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      Assert.Equal(MessageSource.Verifier.ToString(), diagnostics[1].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[1].Severity);
      Assert.Single(diagnostics[0].RelatedInformation);
      var relatedInformation = diagnostics[0].RelatedInformation.First();
      Assert.Equal("This postcondition might not hold: product >= 0", relatedInformation.Message);
      Assert.Equal(new Range(new Position(2, 30), new Position(2, 42)), relatedInformation.Location.Range);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task ChangingCorrectDocumentToOneWithSyntaxErrorsReportsTheSyntaxErrors() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires y >= 0 && x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "ChangingCorrectDocumentToOneWithSyntaxErrorsReportsTheSyntaxErrors.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);

      await AssertNoDiagnosticsAreComing(CancellationToken);
      ApplyChange(ref documentItem, new Range((0, 53), (0, 54)), "");

      var diagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Single(diagnostics);
      Assert.Equal("Parser", diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task ChangingCorrectDocumentToOneWithSyntaxErrorsReportsTheSyntaxErrorsIfNotVerifyOnChange() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires y >= 0 && x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      await SetUp(options => options.Set(ProjectManager.Verification, VerifyOnMode.Never));
      var documentItem = CreateTestDocument(source, "ChangingCorrectDocumentToOneWithSyntaxErrorsReportsTheSyntaxErrorsIfNotVerifyOnChange.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      await AssertNoDiagnosticsAreComing(CancellationToken);

      ApplyChange(ref documentItem, new Range(0, 53, 0, 54), "");

      var diagnostics = await GetNextDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.Equal("Parser", diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task ChangingCorrectDocumentToOneWithVerificationErrorsReportsTheVerificationErrors() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires y >= 0 && x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "ChangingCorrectDocumentToOneWithVerificationErrorsReportsTheVerificationErrors.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);

      ApplyChange(ref documentItem, new Range((8, 30), (8, 31)), "+");

      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.Equal(MessageSource.Verifier.ToString(), diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task ChangingCorrectDocumentToOneWithVerificationErrorsDoesNotReportVerificationErrorsIfNotVerifyOnChange() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires y >= 0 && x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      await SetUp(options => options.Set(ProjectManager.Verification, VerifyOnMode.Never));
      var documentItem = CreateTestDocument(source, "ChangingCorrectDocumentToOneWithVerificationErrorsDoesNotReportVerificationErrorsIfNotVerifyOnChange.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);

      await AssertNoDiagnosticsAreComing(CancellationToken);
      ApplyChange(ref documentItem, new Range((8, 30), (8, 31)), "+");
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task ApplyingMultipleChangesInDocumentOnlySendsOneReport() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires y >= 0 && x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "ApplyingMultipleChangesInDocumentOnlySendsOneReport.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);

      await AssertNoDiagnosticsAreComing(CancellationToken);
      var newVersion = documentItem with { Version = documentItem.Version + 1 };
      client.DidChangeTextDocument(new DidChangeTextDocumentParams {
        TextDocument = new OptionalVersionedTextDocumentIdentifier {
          Uri = newVersion.Uri,
          Version = newVersion.Version
        },
        ContentChanges = new[] {
          new TextDocumentContentChangeEvent {
            Range = new Range((0, 53), (0, 54)),
            Text = ""
          },
          new TextDocumentContentChangeEvent {
            Range = new Range((0, 53), (0, 53)),
            Text = ")"
          }
        }
      });

      // The test applies a change that introduces a syntax error and fixes it thereafter.
      // Therefore, we know that the erroneous state was never reported when we now receive
      // a report without any diagnostics/errors.
      // Otherwise, we'd have to wait for a signal/diagnostic that should never be sent, e.g.
      // with a timeout.
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task ClosingDocumentWithSyntaxErrorHidesDiagnosticsBySendingEmptyDiagnostics() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int
  requires y >= 0 && x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "ClosingDocumentWithSyntaxErrorHidesDiagnosticsBySendingEmptyDiagnostics.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var errorDiagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Single(errorDiagnostics);
      client.DidCloseTextDocument(new DidCloseTextDocumentParams { TextDocument = documentItem });
      var diagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Empty(diagnostics);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentThatIncludesNonExistentDocumentReportsParserErrorAtInclude() {
      var source = "include \"doesNotExist.dfy\"";
      var documentItem = CreateTestDocument(source, Path.Combine(Directory.GetCurrentDirectory(), "Synchronization/TestFiles/test.dfy"));
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Single(diagnostics);
      Assert.Equal("Parser", diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      Assert.Equal(new Range((0, 8), (0, 26)), diagnostics[0].Range);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentThatIncludesDocumentWithSyntaxErrorsReportsParserErrorAtInclude() {
      var source = "include \"syntaxError.dfy\"";
      var documentItem = CreateTestDocument(source, Path.Combine(Directory.GetCurrentDirectory(), "Synchronization/TestFiles/test.dfy"));
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Single(diagnostics);
      Assert.Equal("Parser", diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      Assert.Equal(new Range((0, 0), (0, 1)), diagnostics[0].Range);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task DoubleIncludesGitHubIssue3599() {
      var source = @"
include ""./A.dfy""
include ""./B.dfy""
module ModC {
  lemma Lem() ensures false {}
}
";
      var documentItem = CreateTestDocument(source, Path.Combine(Directory.GetCurrentDirectory(), "Synchronization/TestFiles/test.dfy"));
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.Contains("a postcondition", diagnostics[0].Message);
    }

    [Fact]
    public async Task OpeningDocumentWithSemanticErrorsInIncludeReportsResolverErrorAtIncludeStatement() {
      var source = "include \"semanticError.dfy\"";
      var documentItem = CreateTestDocument(source, Path.Combine(testFilesDirectory, "test.dfy"));
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.Single(diagnostics);
      Assert.Equal("Parser", diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      Assert.Equal(new Range((0, 0), (0, 1)), diagnostics[0].Range);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task SavingDocumentWithVerificationErrorDoesNotDiscardDiagnosticsWithVerificationErrorsIfVerifyOnChange() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "SavingDocumentWithVerificationErrorDoesNotDiscardDiagnosticsWithVerificationErrorsIfVerifyOnChange.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var changeDiagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(changeDiagnostics);
      Assert.Equal(MessageSource.Verifier.ToString(), changeDiagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, changeDiagnostics[0].Severity);
      client.SaveDocument(documentItem);

      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task SavingDocumentWithVerificationErrorReportsDiagnosticsWithVerificationErrorsIfVerifyOnSave() {
      var source = @"
method Multiply(x: int, y: int) returns (product: int)
  requires x >= 0
  decreases y
  ensures product == x * y && product >= 0
{
  if y == 0 {
    product := 0;
  } else {
    var step := Multiply(x, y - 1);
    product := x + step;
  }
}".TrimStart();
      await SetUp(options => options.Set(ProjectManager.Verification, VerifyOnMode.Save));
      var documentItem = CreateTestDocument(source, "SavingDocumentWithVerificationErrorReportsDiagnosticsWithVerificationErrorsIfVerifyOnSave.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      await AssertNoDiagnosticsAreComing(CancellationToken);
      client.SaveDocument(documentItem);
      var saveDiagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(saveDiagnostics);
      Assert.Equal(MessageSource.Verifier.ToString(), saveDiagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, saveDiagnostics[0].Severity);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithVerificationErrorReportsDiagnosticsWithVerificationErrorsAndNestedRelatedLocations() {
      var source = @"
class Test {
    var a: nat
    var b: nat
    var c: nat

    predicate Valid()
        reads this
    {
        && a < b
        && b < c
    }

    method Foo()
        requires Valid()
        ensures Valid()
        modifies this
    {
        c := 10;
    }
}".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithVerificationErrorReportsDiagnosticsWithVerificationErrorsAndNestedRelatedLocations.dfy");
      await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.Equal(MessageSource.Verifier.ToString(), diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      var relatedInformation = diagnostics[0].RelatedInformation.ToArray();
      Assert.Equal(2, relatedInformation.Length);
      Assert.Equal("This postcondition might not hold: Valid()", relatedInformation[0].Message);
      Assert.Equal(new Range((14, 16), (14, 23)), relatedInformation[0].Location.Range);
      Assert.Equal("Could not prove: b < c", relatedInformation[1].Message);
      Assert.Equal(new Range((9, 11), (9, 16)), relatedInformation[1].Location.Range);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithMultipleVerificationCoresReturnsStableDiagnostics() {
      var sourceWithHighTimeout = new CancellationTokenSource();
      sourceWithHighTimeout.CancelAfter(TimeSpan.FromSeconds(240));
      var cancellationToken = sourceWithHighTimeout.Token;

      var source = @"
method t0() { assert true; }
method t1() { assert true; }
method t2() { assert true; }
method t3() { assert true; }
method t4() { assert true; }
method t5() { assert true; }
method t6() { assert false; }
method t7() { assert false; }
method t8() { assert false; }
method t9() { assert false; }
method t10() { assert false; }".TrimStart();
      await SetUp(options => options.Set(BoogieOptionBag.Cores, 4U));
      for (int i = 0; i < 10; i++) {
        diagnosticsReceiver.ClearHistory();
        var documentItem = CreateTestDocument(source, $"test_{i}.dfy");
        client.OpenDocument(documentItem);
        var diagnostics = await GetLastDiagnostics(documentItem);
        try {
          AssertM.Equal(5, diagnostics.Length, $"Iteration is {i}");
        } catch (EqualException) {
          WriteVerificationHistory();
        }
        Assert.Equal(MessageSource.Verifier.ToString(), diagnostics[0].Source);
        Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
        await AssertNoDiagnosticsAreComing(cancellationToken);
      }
    }

    [Fact]
    public async Task OpeningDocumentWithElephantOperatorDoesNotThrowException() {
      var source = @"
module {:options ""/functionSyntax:4""} Library {
  // Library
  datatype Option<T> = Some(value: T) | None
  datatype Result<T> = Success(value: T) | Failure(s: string, pos: int) {
    predicate IsFailure() {
      Failure?
    }
    function PropagateFailure<U>(): Result<U>
      requires IsFailure()
    {
      Failure(s, pos)
    }
    function Extract(): T
      requires !IsFailure()
    {
      value
    }
  }
}
module Parser {
  import opened Library

  datatype Parser = Parser(expected: string) | Log(p: Parser, message: string)
  {
    method {:termination false} Parse(s: string, i: nat)
      returns (result: Result<(string, nat)>)
    {
      if Log? {
        var v1 :- p.Parse(s, i); // Removing this line makes the language server not crash anymore.
      }
      
      result := Failure(""bam"", i);
    }
  }
}".TrimStart();
      await SetUp(options => options.Set(BoogieOptionBag.Cores, 1U));
      diagnosticsReceiver.ClearHistory();
      var documentItem = CreateTestDocument(source, $"test1.dfy");
      client.OpenDocument(documentItem);
      var diagnostics = await GetLastDiagnostics(documentItem);
      AssertM.Equal(0, diagnostics.Count(diagnostic =>
        diagnostic.Severity != DiagnosticSeverity.Information &&
        diagnostic.Severity != DiagnosticSeverity.Hint), $"Expected no issue");
    }

    [Fact]
    public async Task OpeningDocumentWithTimeoutReportsTimeoutDiagnostic() {
      var source = @"
function {:unroll 100} Ack(m: nat, n: nat): nat
  decreases m, n
{
  if m == 0 then
    n + 1
  else if n == 0 then
    Ack(m - 1, 1)
  else
    Ack(m - 1, Ack(m, n - 1))
}

method test() {
  assert Ack(5, 5) == 0;
}".TrimStart();
      await SetUp(options => options.Set(BoogieOptionBag.VerificationTimeLimit, 1U));
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithTimeoutReportsTimeoutDiagnostic.dfy");
      client.OpenDocument(documentItem);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.Contains("timed out", diagnostics[0].Message);
    }

    [Fact]
    public async Task OpeningDocumentWithComplexExpressionUnderlinesAllOfIt() {
      var source = @"
method test(i: int, j: int) {
  assert i > j || i < j; 
//       ^^^^^^^^^^^^^^
}
".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithComplexExpressionUnderlinesAllOfIt.dfy");
      client.OpenDocument(documentItem);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.Equal(MessageSource.Verifier.ToString(), diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      Assert.Equal(new Range((1, 9), (1, 23)), diagnostics[0].Range);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithFailedCallUnderlinesAllOfIt() {
      var source = @"
method test() {
  other(2, 1);
//^^^^^^^^^^^^
}

method other(i: int, j: int)
  requires i < j {
}
".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithFailedCallUnderlinesAllOfIt.dfy");
      client.OpenDocument(documentItem);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.Equal(MessageSource.Verifier.ToString(), diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      Assert.Equal(new Range((1, 2), (1, 14)), diagnostics[0].Range);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task OpeningDocumentWithFailedCallExpressionUnderlinesAllOfIt() {
      var source = @"
method test() {
  var x := 1 + other(2, 1);
//             ^^^^^^^^^^^
}

function other(i: int, j: int): int
  requires i < j {
  2
}
".TrimStart();
      var documentItem = CreateTestDocument(source, "OpeningDocumentWithFailedCallExpressionUnderlinesAllOfIt.dfy");
      client.OpenDocument(documentItem);
      var diagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics);
      Assert.Equal(MessageSource.Verifier.ToString(), diagnostics[0].Source);
      Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].Severity);
      Assert.Equal(new Range((1, 15), (1, 26)), diagnostics[0].Range);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task IncrementalVerificationDiagnosticsBetweenMethods() {
      var source = SlowToVerify + @"
method test() {
  assert false;
}
".TrimStart();
      var documentItem = CreateTestDocument(source, "IncrementalVerificationDiagnosticsBetweenMethods.dfy");
      client.OpenDocument(documentItem);
      var secondVerificationDiagnostics = await GetLastDiagnostics(documentItem);
      var firstVerificationDiagnostics = diagnosticsReceiver.History[^2].Diagnostics.Where(d => d.Severity <= DiagnosticSeverity.Warning).ToList();
      try {

        Assert.Single(firstVerificationDiagnostics);
        // Second diagnostic is a timeout exception from SlowToVerify
        Assert.Equal(2, secondVerificationDiagnostics.Length);
      } catch (OperationCanceledException) {
        await output.WriteLineAsync($"firstVerificationDiagnostics: {firstVerificationDiagnostics.Stringify()}");
        WriteVerificationHistory();
      }
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task IncrementalVerificationDiagnosticsBetweenAssertionsAndWellFormedness() {
      var source = @"
method test() 
  ensures 3 / 0 == 1 {
  assert false;
}
".TrimStart();
      await SetUp(options => options.Set(BoogieOptionBag.Cores, 1U));
      var documentItem = CreateTestDocument(source, "IncrementalVerificationDiagnosticsBetweenAssertionsAndWellFormedness.dfy");
      client.OpenDocument(documentItem);

      var secondVerificationDiagnostics = await GetLastDiagnostics(documentItem);
      AssertM.Equal(2, secondVerificationDiagnostics.Length, secondVerificationDiagnostics.Stringify());
      var firstVerificationDiagnostics = diagnosticsReceiver.History[^2].Diagnostics.
        Where(d => d.Severity <= DiagnosticSeverity.Warning).ToList();

      Assert.Single(firstVerificationDiagnostics);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task NoDiagnosticFlickeringWhenIncremental() {
      var source = @"
method test() {
  assert false;
}
method test2() {
  assert false;
}
".TrimStart();
      await SetUp(options => options.Set(BoogieOptionBag.Cores, 1U));
      var documentItem = CreateTestDocument(source, "NoDiagnosticFlickeringWhenIncremental.dfy");
      client.OpenDocument(documentItem);
      var firstVerificationDiagnostics = await GetNextDiagnostics(documentItem);
      var secondVerificationDiagnostics = await GetNextDiagnostics(documentItem);
      Assert.Single(firstVerificationDiagnostics);
      Assert.Equal(2, secondVerificationDiagnostics.Length);

      ApplyChange(ref documentItem, new Range((1, 9), (1, 14)), "true"); ;

      // Next line should not be needed after resolving https://github.com/dafny-lang/dafny/issues/4377
      var parseDiagnostics2 = await GetNextDiagnostics(documentItem);
      var resolutionDiagnostics2 = await GetNextDiagnostics(documentItem);
      AssertDiagnosticListsAreEqualBesidesMigration(secondVerificationDiagnostics, resolutionDiagnostics2);
      var firstVerificationDiagnostics2 = await GetLastDiagnostics(documentItem);
      Assert.Single(firstVerificationDiagnostics2); // Still contains second failing method

      ApplyChange(ref documentItem, new Range((4, 9), (4, 14)), "true");

      // Next line should not be needed after resolving https://github.com/dafny-lang/dafny/issues/4377
      var parseDiagnostics3 = await GetNextDiagnostics(documentItem);
      var resolutionDiagnostics3 = await GetNextDiagnostics(documentItem);
      AssertDiagnosticListsAreEqualBesidesMigration(firstVerificationDiagnostics2, resolutionDiagnostics3);
      var secondVerificationDiagnostics3 = await GetLastDiagnostics(documentItem);
      Assert.Empty(secondVerificationDiagnostics3);

      await AssertNoDiagnosticsAreComing(CancellationToken);
    }


    [Fact]
    public async Task ApplyChangeBeforeVerificationFinishes() {
      var source = @"
method test() {
  assert false;
}
".TrimStart() + SlowToVerifyNoLimit;
      await SetUp(options => options.Set(BoogieOptionBag.Cores, 1U));
      var documentItem = CreateTestDocument(source, "ApplyChangeBeforeVerificationFinishes.dfy");
      client.OpenDocument(documentItem);
      var status = await WaitForStatus(new Range(0, 7, 0, 11), PublishedVerificationStatus.Error);
      var firstVerificationDiagnostics = diagnosticsReceiver.GetLast(documentItem);
      Assert.Single(firstVerificationDiagnostics);

      // Second verification diagnostics get cancelled.
      ApplyChange(ref documentItem, new Range((1, 9), (1, 14)), "true");

      // Next line should not be needed after resolving https://github.com/dafny-lang/dafny/issues/4377
      var parseDiagnostics2 = await GetNextDiagnostics(documentItem);
      // https://github.com/dafny-lang/dafny/issues/4377
      var resolutionDiagnostics2 = await GetNextDiagnostics(documentItem);
      AssertDiagnosticListsAreEqualBesidesMigration(firstVerificationDiagnostics, resolutionDiagnostics2);
      var secondVerificationDiagnostics2 = await GetLastDiagnostics(documentItem);
      var firstVerificationDiagnostics2 = diagnosticsReceiver.History[^2].Diagnostics.Where(d => d.Severity <= DiagnosticSeverity.Warning).ToArray();
      Assert.Empty(firstVerificationDiagnostics2); // Still contains second failing method
      Assert.Single(secondVerificationDiagnostics2);
    }

    [Fact]
    public async Task DoNotMigrateDiagnosticsOfRemovedMethod() {
      var source = @"
method test() {
  assert false;
}
method test2() {
  assert false;
}
".TrimStart();
      await SetUp(options => options.Set(BoogieOptionBag.Cores, 1U));
      var documentItem = CreateTestDocument(source, "DoNotMigrateDiagnosticsOfRemovedMethod.dfy");
      client.OpenDocument(documentItem);
      var firstVerificationDiagnostics = await GetNextDiagnostics(documentItem);
      var secondVerificationDiagnostics = await GetNextDiagnostics(documentItem);
      Assert.Single(firstVerificationDiagnostics);
      Assert.Equal(2, secondVerificationDiagnostics.Length);

      /*
       * New source becomes
       * method test() {
           assert false;
           assert false;
         }
       */
      ApplyChange(ref documentItem, new Range((2, 0), (4, 0)), "");

      var resolutionDiagnosticsAfter = await GetNextDiagnostics(documentItem);
      Assert.Single(resolutionDiagnosticsAfter);
    }

    private static void AssertDiagnosticListsAreEqualBesidesMigration(Diagnostic[] expected, Diagnostic[] actual) {
      AssertM.Equal(expected.Length, actual.Length, $"expected: {expected.Stringify()}, but was: {actual.Stringify()}");
      foreach (var t in expected.Zip(actual)) {
        AssertM.Equal(IdeState.OutdatedPrefix + t.First.Message, t.Second.Message, t.Second.ToString());
      }
    }

    [Fact]
    public async Task DiagnosticsInDifferentImplementationUnderOneNamedVerificationTask() {
      var source = @"
method test() 
  ensures 3 / 0 == 2 {
  assert false;
}
".TrimStart();
      await SetUp(options => options.Set(BoogieOptionBag.Cores, 1U));
      var documentItem = CreateTestDocument(source, "DiagnosticsInDifferentImplementationUnderOneNamedVerificationTask.dfy");
      client.OpenDocument(documentItem);
      var diagnostics = await GetLastDiagnostics(documentItem);
      AssertM.Equal(2, diagnostics.Length, diagnostics.Stringify());
    }

    [Fact]
    public async Task MethodRenameDoesNotAffectMigration() {
      var source = @"
method Foo() {
  assert false;
}
".TrimStart();
      var documentItem = CreateTestDocument(source, "MethodRenameDoesNotAffectMigration.dfy");
      client.OpenDocument(documentItem);
      var preChangeDiagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(preChangeDiagnostics);
      ApplyChange(ref documentItem, new Range(0, 7, 0, 10), "Bar");
      var resolutionDiagnostics = await GetNextDiagnostics(documentItem);
      AssertDiagnosticListsAreEqualBesidesMigration(preChangeDiagnostics, resolutionDiagnostics);
    }

    [Fact]
    public async Task ModuleRenameDoesNotAffectMigration() {
      var source = @"
module Foo {
  method Bar() {
    assert false;
  }
}
".TrimStart();
      var documentItem = CreateTestDocument(source, "ModuleRenameDoesNotAffectMigration.dfy");
      client.OpenDocument(documentItem);
      var preChangeDiagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(preChangeDiagnostics);
      await AssertNoDiagnosticsAreComing(CancellationToken);
      ApplyChange(ref documentItem, new Range(0, 7, 0, 10), "Zap");
      var resolutionDiagnostics = await GetNextDiagnostics(documentItem);
      AssertDiagnosticListsAreEqualBesidesMigration(preChangeDiagnostics, resolutionDiagnostics);
    }

    /**
     * This test is an indirect way to test performance. It tests that the diagnostics of
     * resolution, verification task determination, and verification itself, are returned separately.
     */
    [Fact]
    public async Task ResolutionDiagnosticsAreReturnedBeforeComputingVerificationTasks() {
      var source = @"
method Foo() { 
  assert false; 
}".TrimStart();
      var documentItem = CreateTestDocument(source, "ResolutionDiagnosticsAreReturnedBeforeComputingVerificationTasks.dfy");
      client.OpenDocument(documentItem);
      var verificationDiagnostics = await GetLastDiagnostics(documentItem);
      Assert.Single(verificationDiagnostics);
      ApplyChange(ref documentItem, new Range(0, 0, 0, 1), "");
      var brokenSyntaxDiagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      Assert.True(brokenSyntaxDiagnostics.Length > 1);
      documentItem = documentItem with { Version = documentItem.Version + 1 };
      // Fix syntax error and replace method header so verification diagnostics are not migrated.
      ApplyChange(ref documentItem, new Range(0, 0, 1, 0), "method Bar() {\n");
      // Next line is made obsolete by resolving https://github.com/dafny-lang/dafny/issues/4377
      var unnecessaryDiagnostics = await diagnosticsReceiver.AwaitNextNotificationAsync(CancellationToken);
      var resolutionDiagnostics = await diagnosticsReceiver.AwaitNextNotificationAsync(CancellationToken);
      Assert.Empty(resolutionDiagnostics.Diagnostics);
      var translationDiagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
      // Verification diagnostics were removed since task no longer exists.
      Assert.Single(translationDiagnostics);
      await AssertNoDiagnosticsAreComing(CancellationToken);
    }

    [Fact]
    public async Task DiagnosticsAfterSavingWithVerifyOnChange() {
      var source = @"
method Foo() { 
  assert false; 
}".TrimStart();
      var documentItem = CreateTestDocument(source, "DiagnosticsAfterSavingWithVerifyOnChange.dfy");
      client.OpenDocument(documentItem);
      await client.SaveDocumentAndWaitAsync(documentItem, CancellationToken);
      var diagnostics1 = await GetLastDiagnostics(documentItem);
      Assert.Single(diagnostics1);
      ApplyChange(ref documentItem, new Range(1, 0, 2, 0), "");
      var diagnostics2 = await GetLastDiagnostics(documentItem);
      Assert.Empty(diagnostics2);
    }

    public DiagnosticsTest(ITestOutputHelper output) : base(output) {
    }
  }
}
