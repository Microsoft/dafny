using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Boogie;
using Microsoft.Dafny.Auditor;

namespace Microsoft.Dafny;

public interface IINode { // TODO rename to INode   
  RangeToken RangeToken { get; }
}

public abstract class INode : IINode { // TODO Rename to remove I from the rename.

  public IToken tok = Token.NoToken;
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public IToken Tok {
    get => tok;
    set => tok = value;
  }

  /// <summary>
  /// These children should be such that they contain information produced by resolution such as inferred types
  /// and resolved references. However, they should not be so transformed that source location from the initial
  /// program is lost. As an example, the pattern matching compilation may deduplicate nodes from the original AST,
  /// losing source location information, so those transformed nodes should not be returned by this property.
  /// </summary>
  public abstract IEnumerable<INode> Children { get; }

  public IEnumerable<INode> Descendants() {
    return Children.Concat(Children.SelectMany(n => n.Descendants()));
  }

  public virtual IEnumerable<AssumptionDescription> Assumptions() {
    return Enumerable.Empty<AssumptionDescription>();
  }

  public ISet<INode> Visit(Func<INode, bool> beforeChildren = null, Action<INode> afterChildren = null) {
    beforeChildren ??= node => true;
    afterChildren ??= node => { };

    var visited = new HashSet<INode>();
    var toVisit = new LinkedList<INode>();
    toVisit.AddFirst(this);
    while (toVisit.Any()) {
      var current = toVisit.First();
      toVisit.RemoveFirst();
      if (!visited.Add(current)) {
        continue;
      }

      if (!beforeChildren(current)) {
        continue;
      }

      var nodeAfterChildren = toVisit.First;
      foreach (var child in current.Children) {
        if (child == null) {
          throw new InvalidOperationException($"Object of type {current.GetType()} has null child");
        }

        if (nodeAfterChildren == null) {
          toVisit.AddLast(child);
        } else {
          toVisit.AddBefore(nodeAfterChildren, child);
        }
      }

      afterChildren(current);
    }

    return visited;
  }

  protected RangeToken rangeToken = null;

  // Contains tokens that did not make it in the AST but are part of the expression,
  // Enables ranges to be correct.
  // TODO: Re-add format tokens where needed until we put all the formatting to replace the tok of every expression
  internal IToken[] FormatTokens = null;

  public virtual RangeToken RangeToken {
    get {
      if (rangeToken == null) {

        var startTok = tok;
        var endTok = tok;

        void UpdateStartEndToken(IToken token1) {
          if (token1.Filename != tok.Filename) {
            return;
          }

          if (token1.pos < startTok.pos) {
            startTok = token1;
          }

          if (token1.pos + token1.val.Length > endTok.pos + endTok.val.Length) {
            endTok = token1;
          }
        }

        void UpdateStartEndTokRecursive(INode node) {
          if (node is null) {
            return;
          }

          if (node.tok.Filename != tok.Filename || node is Expression { IsImplicit: true } ||
              node is DefaultValueExpression) {
            // Ignore any auto-generated expressions.
          } else if (node != this && node.RangeToken != null) {
            UpdateStartEndToken(node.StartToken);
            UpdateStartEndToken(node.EndToken);
          } else {
            UpdateStartEndToken(node.tok);
            node.Children.Iter(UpdateStartEndTokRecursive);
          }
        }

        UpdateStartEndTokRecursive(this);

        if (FormatTokens != null) {
          foreach (var token in FormatTokens) {
            UpdateStartEndToken(token);
          }
        }

        rangeToken = new RangeToken(startTok, endTok);
      }

      return rangeToken;
    }
    set => rangeToken = value;
  }

  public IToken StartToken => RangeToken?.StartToken;

  public IToken EndToken => RangeToken?.EndToken;

  protected IReadOnlyList<IToken> OwnedTokensCache;

  /// <summary>
  /// A token is owned by a node if it was used to parse this node,
  /// but is not owned by any of this Node's children
  /// </summary>
  public IEnumerable<IToken> OwnedTokens {
    get {
      if (OwnedTokensCache != null) {
        return OwnedTokensCache;
      }

      var startToEndTokenNotOwned =
        Children.Where(child => child.StartToken != null && child.EndToken != null)
          .ToDictionary(child => child.StartToken!, child => child.EndToken!);

      var result = new List<IToken>();
      if (StartToken == null) {
        Contract.Assume(EndToken == null);
      } else {
        Contract.Assume(EndToken != null);
        var tmpToken = StartToken;
        while (tmpToken != null && tmpToken != EndToken.Next) {
          if (startToEndTokenNotOwned.TryGetValue(tmpToken, out var endNotOwnedToken)) {
            tmpToken = endNotOwnedToken;
          } else if (tmpToken.filename != null) {
            result.Add(tmpToken);
          }

          tmpToken = tmpToken.Next;
        }
      }


      OwnedTokensCache = result;

      return OwnedTokensCache;
    }
  }
}