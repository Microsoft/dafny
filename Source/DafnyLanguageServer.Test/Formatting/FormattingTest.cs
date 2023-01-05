
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Dafny.LanguageServer.Handlers;
using Microsoft.Dafny.LanguageServer.IntegrationTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dafny.LanguageServer.IntegrationTest.Util;
using Microsoft.Dafny.LanguageServer.Workspace;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace Microsoft.Dafny.LanguageServer.Formatting;
[TestClass]
public class FormattingTest : ClientBasedLanguageServerTest {
  [TestInitialize]
  public override async Task SetUp(Action<DafnyOptions> modifyOptions) {
    await base.SetUp(o => {
      o.ProverOptions.Add("-proverOpt:SOLVER=noop");
      modifyOptions(o);
    });
  }

  [TestMethod]
  public async Task TestFormatting1() {
    var source = @"
function test
       (
 a: int
        ): int {
       if   a - 1
  == 0 then
         1
         else
a + 1
    }
";
    var target = @"
function test
  (
    a: int
  ): int {
  if   a - 1
    == 0 then
    1
  else
    a + 1
}
";
    await FormattingWorksFor(source, target);
    await FormattingWorksFor(target, target);
  }

  [TestMethod]
  public async Task TestWhenDocIsEmpty() {
    var source = @"
";
    await FormattingWorksFor(source);
  }

  [TestMethod]
  public async Task TestWhenParsingFails() {
    var source = @"
function test() {
  var x := 1:
  var y = 2;
  z
}

module A {
  class B {
    method z() {
      
    }
  }
}";
    await FormattingWorksFor(source);
  }

  private async Task<List<TextEdit>> RequestFormattingAsync(TextDocumentItem documentItem) {
    var editList = await client.RequestDocumentFormatting(
      new DocumentFormattingParams {
        TextDocument = documentItem.Uri.GetFileSystemPath(),
      },
      CancellationToken
    );
    if (editList != null) {
      return editList.ToList();
    } else {
      return new List<TextEdit>();
    }
  }

  private async Task FormattingWorksFor(string source, string target = null) {
    if (target == null) {
      target = source;
    }
    var documentItem = CreateTestDocument(source);
    await client.OpenDocumentAndWaitAsync(documentItem, CancellationToken);
    var verificationDiagnostics = await diagnosticsReceiver.AwaitNextDiagnosticsAsync(CancellationToken);
    DocumentAfterParsing document = await Documents.GetLastDocumentAsync(documentItem);
    var edits = await RequestFormattingAsync(documentItem);
    edits.Reverse();
    var finalText = source;
    Assert.IsNotNull(document, "document != null");
    var codeActionInput = new DafnyCodeActionInput(document);

    foreach (var edit in edits) {
      finalText = codeActionInput.Extract(new Range((0, 0), edit.Range.Start)) +
                  edit.NewText +
                  codeActionInput.Extract(new Range(edit.Range.End, document.TextDocumentItem.Range.End));
    }
    Assert.AreEqual(target, finalText);
  }
}
