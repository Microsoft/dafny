﻿using Microsoft.Dafny.LanguageServer.Util;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Threading;

namespace Microsoft.Dafny.LanguageServer.Workspace.ChangeProcessors {
  public class TextChangeProcessor : ITextChangeProcessor {
    public DocumentTextBuffer ApplyChange(DocumentTextBuffer originalTextDocument, DidChangeTextDocumentParams documentChange, CancellationToken cancellationToken) {
      var newText = ApplyTextChanges(originalTextDocument.Text, originalTextDocument.NumberOfLines, documentChange.ContentChanges, out var newNumberOfLines, cancellationToken);
      return originalTextDocument with {
        Version = documentChange.TextDocument.Version,
        Text = newText,
        NumberOfLines = newNumberOfLines
      };
    }

    private static string ApplyTextChanges(string originalText, int originalLines, Container<TextDocumentContentChangeEvent> changes, out int numberOfLines, CancellationToken cancellationToken) {
      var mergedText = originalText;
      var mergedNumberOfLines = originalLines;
      foreach (var change in changes) {
        cancellationToken.ThrowIfCancellationRequested();
        mergedText = ApplyTextChange(mergedText, change, ref mergedNumberOfLines, cancellationToken);
      }
      numberOfLines = mergedNumberOfLines;
      return mergedText;
    }

    private static string ApplyTextChange(string text, TextDocumentContentChangeEvent change, ref int numberOfLines,
      CancellationToken cancellationToken) {
      if (change.Range == null) {
        numberOfLines = DocumentTextBuffer.ComputeNumberOfLines(change.Text);
        // https://microsoft.github.io/language-server-protocol/specifications/specification-3-17/#textDocumentContentChangeEvent
        return change.Text;
      }
      int absoluteStart = change.Range.Start.ToAbsolutePosition(text, cancellationToken);
      int absoluteEnd = change.Range.End.ToAbsolutePosition(text, cancellationToken);
      numberOfLines -=
        DocumentTextBuffer.ComputeNumberOfNewlines(text.Substring(absoluteStart, absoluteEnd - absoluteStart));
      numberOfLines += DocumentTextBuffer.ComputeNumberOfNewlines(change.Text);
      return text[..absoluteStart] + change.Text + text[absoluteEnd..];
    }
  }
}
