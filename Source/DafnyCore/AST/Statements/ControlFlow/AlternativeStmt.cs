using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Microsoft.Dafny;

public class AlternativeStmt : Statement, ICloneable<AlternativeStmt>, ICanFormat {
  public readonly bool UsesOptionalBraces;
  public readonly List<GuardedAlternative> Alternatives;
  [ContractInvariantMethod]
  void ObjectInvariant() {
    Contract.Invariant(Alternatives != null);
  }

  public AlternativeStmt Clone(Cloner cloner) {
    return new AlternativeStmt(cloner, this);
  }

  public AlternativeStmt(Cloner cloner, AlternativeStmt original) : base(cloner, original) {
    Alternatives = original.Alternatives.ConvertAll(cloner.CloneGuardedAlternative);
    UsesOptionalBraces = original.UsesOptionalBraces;
  }

  public AlternativeStmt(RangeToken rangeToken, List<GuardedAlternative> alternatives, bool usesOptionalBraces)
    : base(rangeToken) {
    Contract.Requires(alternatives != null);
    this.Alternatives = alternatives;
    this.UsesOptionalBraces = usesOptionalBraces;
  }
  public AlternativeStmt(RangeToken rangeToken, List<GuardedAlternative> alternatives, bool usesOptionalBraces, Attributes attrs)
    : base(rangeToken, attrs) {
    Contract.Requires(alternatives != null);
    this.Alternatives = alternatives;
    this.UsesOptionalBraces = usesOptionalBraces;
  }
  public override IEnumerable<Statement> SubStatements {
    get {
      foreach (var alt in Alternatives) {
        foreach (var s in alt.Body) {
          yield return s;
        }
      }
    }
  }
  public override IEnumerable<Expression> NonSpecificationSubExpressions {
    get {
      foreach (var e in base.NonSpecificationSubExpressions) { yield return e; }
      foreach (var alt in Alternatives) {
        yield return alt.Guard;
      }
    }
  }

  public override IEnumerable<INode> Children => Alternatives;
  public bool SetIndent(int indentBefore, TokenNewIndentCollector formatter) {
    return formatter.SetIndentCases(indentBefore, OwnedTokens.Concat(Alternatives.SelectMany(alternative => alternative.OwnedTokens)).OrderBy(token => token.pos), () => {
      formatter.VisitAlternatives(Alternatives, indentBefore);
    });
  }

  public void Resolve(INewOrOldResolver resolver, ResolutionContext resolutionContext) {
    if (resolver.Options.ForbidNondeterminism && 2 <= Alternatives.Count) {
      resolver.Reporter.Error(MessageSource.Resolver, GeneratorErrors.ErrorId.c_case_based_if_forbidden, Tok,
        "case-based if statement forbidden by the --enforce-determinism option");
    }
    ResolveAlternatives(resolver, Alternatives, null, resolutionContext);
  }


  public static void ResolveAlternatives(INewOrOldResolver resolver, List<GuardedAlternative> alternatives,
    AlternativeLoopStmt loopToCatchBreaks, ResolutionContext resolutionContext) {
    Contract.Requires(alternatives != null);
    Contract.Requires(resolutionContext != null);

    // first, resolve the guards
    foreach (var alternative in alternatives) {
      int prevErrorCount = resolver.Reporter.Count(ErrorLevel.Error);
      resolver.ResolveExpression(alternative.Guard, resolutionContext);
      Contract.Assert(alternative.Guard.Type != null);  // follows from postcondition of ResolveExpression
      bool successfullyResolved = resolver.Reporter.Count(ErrorLevel.Error) == prevErrorCount;
      resolver.ConstrainTypeExprBool(alternative.Guard, "condition is expected to be of type bool, but is {0}");
    }

    if (loopToCatchBreaks != null) {
      resolver.LoopStack.Add(loopToCatchBreaks);  // push
    }
    foreach (var alternative in alternatives) {
      resolver.Scope.PushMarker();
      resolver.DominatingStatementLabels.PushMarker();
      if (alternative.IsBindingGuard) {
        var exists = (ExistsExpr)alternative.Guard;
        foreach (var v in exists.BoundVars) {
          resolver.ScopePushAndReport(resolver.Scope, v, "bound-variable");
        }
      }
      resolver.ResolveAttributes(alternative, resolutionContext);
      foreach (Statement ss in alternative.Body) {
        resolver.ResolveStatementWithLabels(ss, resolutionContext);
      }
      resolver.DominatingStatementLabels.PopMarker();
      resolver.Scope.PopMarker();
    }
    if (loopToCatchBreaks != null) {
      resolver.LoopStack.RemoveAt(resolver.LoopStack.Count - 1);  // pop
    }
  }
}