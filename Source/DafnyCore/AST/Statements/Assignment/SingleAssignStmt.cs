using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Microsoft.Dafny;

public class SingleAssignStmt : Statement, ICloneable<SingleAssignStmt> {
  public readonly Expression Lhs;
  public readonly AssignmentRhs Rhs;
  public override IEnumerable<INode> Children => new List<Node> { Lhs, Rhs }.Where(x => x != null);
  public override IEnumerable<INode> PreResolveChildren => Children;

  public override IEnumerable<IdentifierExpr> GetAssignedLocals() {
    return new[] { Lhs.Resolved }.OfType<IdentifierExpr>();
  }

  [ContractInvariantMethod]
  void ObjectInvariant() {
    Contract.Invariant(Lhs != null);
    Contract.Invariant(Rhs != null);
  }

  public override IOrigin Tok {
    get {
      if (Rhs.StartToken.Prev is not null) {
        var previous = Rhs.StartToken.Prev;
        // If there was a single assignment, report on the operator.
        var singleAssignment = previous.val == ":=";
        // If there was an implicit return assignment, report on the return.
        var implicitAssignment = previous.val == "return";
        if (singleAssignment || implicitAssignment) {
          return previous;
        }
      }
      return Rhs.StartToken;
    }
  }

  public SingleAssignStmt Clone(Cloner cloner) {
    return new SingleAssignStmt(cloner, this);
  }

  public SingleAssignStmt(Cloner cloner, SingleAssignStmt original) : base(cloner, original) {
    Lhs = cloner.CloneExpr(original.Lhs);
    Rhs = cloner.CloneRHS(original.Rhs);
  }

  public SingleAssignStmt(IOrigin rangeOrigin, Expression lhs, AssignmentRhs rhs)
    : base(rangeOrigin) {
    Contract.Requires(rangeOrigin != null);
    Contract.Requires(lhs != null);
    Contract.Requires(rhs != null);
    Lhs = lhs;
    Rhs = rhs;
  }

  public override IEnumerable<Statement> SubStatements {
    get {
      foreach (var s in Rhs.SubStatements) {
        yield return s;
      }
    }
  }

  public override IEnumerable<Statement> PreResolveSubStatements {
    get {
      foreach (var s in Rhs.PreResolveSubStatements) {
        yield return s;
      }
    }
  }

  public override IEnumerable<Expression> NonSpecificationSubExpressions {
    get {
      foreach (var e in base.NonSpecificationSubExpressions) { yield return e; }
      yield return Lhs;
      foreach (var ee in Rhs.NonSpecificationSubExpressions) {
        yield return ee;
      }
    }
  }

  public override IEnumerable<Expression> SpecificationSubExpressions {
    get {
      foreach (var e in base.SpecificationSubExpressions) { yield return e; }
      foreach (var ee in Rhs.SpecificationSubExpressions) {
        yield return ee;
      }
    }
  }

  /// <summary>
  /// This method assumes "lhs" has been successfully resolved.
  /// </summary>
  public static bool LhsIsToGhost(Expression lhs) {
    Contract.Requires(lhs != null);
    return LhsIsToGhost_Which(lhs) == NonGhostKind.IsGhost;
  }
  public static bool LhsIsToGhostOrAutoGhost(Expression lhs) {
    Contract.Requires(lhs != null);
    return LhsIsToGhost_Which(lhs) == NonGhostKind.IsGhost || lhs.Resolved is AutoGhostIdentifierExpr;
  }
  public enum NonGhostKind { IsGhost, Variable, Field, ArrayElement }
  public static string NonGhostKind_To_String(NonGhostKind gk) {
    Contract.Requires(gk != NonGhostKind.IsGhost);
    switch (gk) {
      case NonGhostKind.Variable: return "non-ghost variable";
      case NonGhostKind.Field: return "non-ghost field";
      case NonGhostKind.ArrayElement: return "array element";
      default:
        Contract.Assume(false);  // unexpected NonGhostKind
        throw new cce.UnreachableException();  // please compiler
    }
  }
  /// <summary>
  /// This method assumes "lhs" has been successfully resolved.
  /// </summary>
  public static NonGhostKind LhsIsToGhost_Which(Expression lhs) {
    Contract.Requires(lhs != null);
    lhs = lhs.Resolved;
    if (lhs is AutoGhostIdentifierExpr) {
      // TODO: Should we return something different for this case?
      var x = (IdentifierExpr)lhs;
      if (!x.Var.IsGhost) {
        return NonGhostKind.Variable;
      }
    } else if (lhs is IdentifierExpr) {
      var x = (IdentifierExpr)lhs;
      if (!x.Var.IsGhost) {
        return NonGhostKind.Variable;
      }
    } else if (lhs is MemberSelectExpr) {
      var x = (MemberSelectExpr)lhs;
      if (!x.Member.IsGhost) {
        return NonGhostKind.Field;
      }
    } else {
      // LHS denotes an array element, which is always non-ghost
      return NonGhostKind.ArrayElement;
    }
    return NonGhostKind.IsGhost;
  }

  public override void ResolveGhostness(ModuleResolver resolver, ErrorReporter reporter, bool mustBeErasable,
    ICodeContext codeContext,
    string proofContext,
    bool allowAssumptionVariables, bool inConstructorInitializationPhase) {
    var lhs = Lhs.Resolved;

    // Make an auto-ghost variable a ghost if the RHS is a ghost
    if (lhs.Resolved is AutoGhostIdentifierExpr autoGhostIdExpr) {
      if (Rhs is ExprRhs eRhs && ExpressionTester.UsesSpecFeatures(eRhs.Expr)) {
        autoGhostIdExpr.Var.MakeGhost();
      } else if (Rhs is TypeRhs tRhs) {
        if (tRhs.InitCall != null && tRhs.InitCall.Method.IsGhost) {
          autoGhostIdExpr.Var.MakeGhost();
        } else if (tRhs.ArrayDimensions != null && tRhs.ArrayDimensions.Exists(ExpressionTester.UsesSpecFeatures)) {
          autoGhostIdExpr.Var.MakeGhost();
        } else if (tRhs.ElementInit != null && ExpressionTester.UsesSpecFeatures(tRhs.ElementInit)) {
          autoGhostIdExpr.Var.MakeGhost();
        } else if (tRhs.InitDisplay != null && tRhs.InitDisplay.Any(ExpressionTester.UsesSpecFeatures)) {
          autoGhostIdExpr.Var.MakeGhost();
        }
      }
    }

    if (proofContext != null && Rhs is TypeRhs) {
      reporter.Error(MessageSource.Resolver, ResolutionErrors.ErrorId.r_new_forbidden_in_proof, Rhs.Tok, $"{proofContext} is not allowed to use 'new'");
    }

    var gk = LhsIsToGhost_Which(lhs);
    if (gk == NonGhostKind.IsGhost) {
      IsGhost = true;
      if (proofContext != null && !(lhs is IdentifierExpr)) {
        reporter.Error(MessageSource.Resolver, ResolutionErrors.ErrorId.r_no_heap_update_in_proof, lhs.tok, $"{proofContext} is not allowed to make heap updates");
      }
      if (Rhs is TypeRhs tRhs && tRhs.InitCall != null) {
        tRhs.InitCall.ResolveGhostness(resolver, reporter, true, codeContext, proofContext, allowAssumptionVariables, inConstructorInitializationPhase);
      }
    } else if (gk == NonGhostKind.Variable && codeContext.IsGhost) {
      // cool
    } else if (mustBeErasable) {
      if (inConstructorInitializationPhase && codeContext is Constructor && codeContext.IsGhost && lhs is MemberSelectExpr mse &&
          mse.Obj.Resolved is ThisExpr) {
        // in this first division (before "new;") of a ghost constructor, allow assignment to non-ghost field of the object being constructed
      } else {
        string reason;
        if (codeContext.IsGhost) {
          reason = string.Format("this is a ghost {0}", codeContext is MemberDecl member ? member.WhatKind : "context");
        } else {
          reason = "the statement is in a ghost context; e.g., it may be guarded by a specification-only expression";
        }
        reporter.Error(MessageSource.Resolver, ResolutionErrors.ErrorId.r_assignment_forbidden_in_context, this, $"assignment to {NonGhostKind_To_String(gk)} is not allowed in this context, because {reason}");
      }
    } else {
      if (gk == NonGhostKind.Field) {
        var mse = (MemberSelectExpr)lhs;
        ExpressionTester.CheckIsCompilable(resolver, reporter, mse.Obj, codeContext);
      } else if (gk == NonGhostKind.ArrayElement) {
        ExpressionTester.CheckIsCompilable(resolver, reporter, lhs, codeContext);
      }

      if (Rhs is ExprRhs) {
        var rhs = (ExprRhs)Rhs;
        if (!LhsIsToGhost(lhs)) {
          ExpressionTester.CheckIsCompilable(resolver, reporter, rhs.Expr, codeContext);
        }
      } else if (Rhs is HavocRhs) {
        // cool
      } else {
        var rhs = (TypeRhs)Rhs;
        if (rhs.ArrayDimensions != null) {
          rhs.ArrayDimensions.ForEach(ee => ExpressionTester.CheckIsCompilable(resolver, reporter, ee, codeContext));
          if (rhs.ElementInit != null) {
            ExpressionTester.CheckIsCompilable(resolver, reporter, rhs.ElementInit, codeContext);
          }
          if (rhs.InitDisplay != null) {
            rhs.InitDisplay.ForEach(ee => ExpressionTester.CheckIsCompilable(resolver, reporter, ee, codeContext));
          }
        }
        if (rhs.InitCall != null) {
          var callee = rhs.InitCall.Method;
          if (callee.IsGhost) {
            reporter.Error(MessageSource.Resolver, ResolutionErrors.ErrorId.r_assignment_to_ghost_constructor_only_in_ghost,
              rhs.InitCall, "the result of a ghost constructor can only be assigned to a ghost variable");
          }
          for (var i = 0; i < rhs.InitCall.Args.Count; i++) {
            if (!callee.Ins[i].IsGhost) {
              ExpressionTester.CheckIsCompilable(resolver, reporter, rhs.InitCall.Args[i], codeContext);
            }
          }
        }
      }
    }
  }
}