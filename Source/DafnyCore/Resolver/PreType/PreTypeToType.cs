//-----------------------------------------------------------------------------
//
// Copyright by the contributors to the Dafny Project
// SPDX-License-Identifier: MIT
//
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Boogie;

namespace Microsoft.Dafny;


class PreTypeToTypeVisitor : ASTVisitor<IASTVisitorContext> {
  public override IASTVisitorContext GetContext(IASTVisitorContext astVisitorContext, bool inFunctionPostcondition) {
    return astVisitorContext;
  }

  public PreTypeToTypeVisitor() {
  }

  public static Type PreType2Type(PreType preType) {
    var pt = (DPreType)preType.Normalize(); // all pre-types should have been filled in and resolved to a non-proxy
    if (pt.PrintablePreType != null) {
      pt = pt.PrintablePreType;
    }
    switch (pt.Decl.Name) {
      case "bool":
        return Type.Bool;
      case "char":
        return Type.Char;
      case "int":
        return Type.Int;
      case "real":
        return Type.Real;
      case "ORDINAL":
        return Type.BigOrdinal;
      case "set":
        return new SetType(true, PreType2Type(pt.Arguments[0]));
      case "iset":
        return new SetType(false, PreType2Type(pt.Arguments[0]));
      case "multiset":
        return new MultiSetType(PreType2Type(pt.Arguments[0]));
      case "seq":
        return new SeqType(PreType2Type(pt.Arguments[0]));
      case "map":
        return new MapType(true, PreType2Type(pt.Arguments[0]), PreType2Type(pt.Arguments[1]));
      case "imap":
        return new MapType(false, PreType2Type(pt.Arguments[0]), PreType2Type(pt.Arguments[1]));
      default:
        break;
    }
    var arguments = pt.Arguments.ConvertAll(PreType2Type);
    if (pt.Decl is ValuetypeDecl valuetypeDecl) {
      return valuetypeDecl.CreateType(arguments);
    } else if (pt.Decl is ArrowTypeDecl arrowTypeDecl) {
      return new ArrowType(pt.Decl.tok, arrowTypeDecl, arguments);
    } else {
      return new UserDefinedType(pt.Decl.tok, pt.Decl.Name, pt.Decl, arguments);
    }
  }

  protected override void VisitOneDeclaration(TopLevelDecl decl) {
    if (decl is NewtypeDecl newtypeDecl) {
      UpdateIfOmitted(newtypeDecl.BaseType, newtypeDecl.BasePreType);
    } else if (decl is SubsetTypeDecl subsetTypeDecl) {
      UpdateIfOmitted(subsetTypeDecl.Var.Type, subsetTypeDecl.Var.PreType);
    }
    base.VisitOneDeclaration(decl);
  }

  private void UpdateIfOmitted(Type type, PreType preType) {
    var preTypeConverted = PreType2Type(preType);
    UpdateIfOmitted(type, preTypeConverted);
  }

  private void UpdateIfOmitted(Type type, Type preTypeConverted) {
    if (type is TypeProxy { T: null } typeProxy) {
      typeProxy.T = preTypeConverted;
    } else {
      type = type.NormalizeExpand();
      // TODO: "type" should also be moved up to the parent type that corresponds to "preType.Decl"
      Contract.Assert((type as UserDefinedType)?.ResolvedClass == (preTypeConverted as UserDefinedType)?.ResolvedClass);
      Contract.Assert(type.TypeArgs.Count == preTypeConverted.TypeArgs.Count);
      for (var i = 0; i < type.TypeArgs.Count; i++) {
        UpdateIfOmitted(type.TypeArgs[i], preTypeConverted.TypeArgs[i]);
      }
    }
  }

  private void UpdateTypeOfVariables(IEnumerable<IVariable> variables) {
    foreach (var v in variables) {
      UpdateIfOmitted(v.Type, v.PreType);
    }
  }

  public override void VisitField(Field field) {
    // The type of the const might have been omitted in the program text and then inferred
    UpdateIfOmitted(field.Type, field.PreType);
    base.VisitField(field);
  }

  protected override void PostVisitOneExpression(Expression expr, IASTVisitorContext context) {
    if (expr is FunctionCallExpr functionCallExpr) {
      functionCallExpr.TypeApplication_AtEnclosingClass = functionCallExpr.PreTypeApplication_AtEnclosingClass.ConvertAll(PreType2Type);
      functionCallExpr.TypeApplication_JustFunction = functionCallExpr.PreTypeApplication_JustFunction.ConvertAll(PreType2Type);
    } else if (expr is MemberSelectExpr memberSelectExpr) {
      memberSelectExpr.TypeApplication_AtEnclosingClass = memberSelectExpr.PreTypeApplication_AtEnclosingClass.ConvertAll(PreType2Type);
      memberSelectExpr.TypeApplication_JustMember = memberSelectExpr.PreTypeApplication_JustMember.ConvertAll(PreType2Type);
    } else if (expr is ComprehensionExpr comprehensionExpr) {
      UpdateTypeOfVariables(comprehensionExpr.BoundVars);
    } else if (expr is LetExpr letExpr) {
      UpdateTypeOfVariables(letExpr.BoundVars);
      foreach (var lhs in letExpr.LHSs) {
        VisitPattern(lhs, context);
      }
    } else if (expr is DatatypeValue datatypeValue) {
      Contract.Assert(datatypeValue.InferredTypeArgs.Count == 0 || datatypeValue.InferredTypeArgs.Count == datatypeValue.InferredPreTypeArgs.Count);
      if (datatypeValue.InferredTypeArgs.Count == 0) {
        foreach (var preTypeArgument in datatypeValue.InferredPreTypeArgs) {
          datatypeValue.InferredTypeArgs.Add(PreType2Type(preTypeArgument));
        }
      }
    }

    if (expr.PreType is UnusedPreType) {
      expr.Type = new InferredTypeProxy();
    } else {
      expr.Type = PreType2Type(expr.PreType);
    }
    base.PostVisitOneExpression(expr, context);

    if (expr is ConcreteSyntaxExpression { ResolvedExpression: { } resolvedExpression }) {
      VisitExpression(resolvedExpression, context);
    }
  }

  private void VisitPattern<VT>(CasePattern<VT> casePattern, IASTVisitorContext context) where VT : class, IVariable {
    if (casePattern.Var != null) {
      UpdateIfOmitted(casePattern.Var.Type, casePattern.Var.PreType);
    }
    VisitExpression(casePattern.Expr, context);

    if (casePattern.Arguments != null) {
      casePattern.Arguments.ForEach(v => VisitPattern(v, context));
    }
  }

  protected override void PostVisitOneStatement(Statement stmt, IASTVisitorContext context) {
    if (stmt is VarDeclStmt varDeclStmt) {
      UpdateTypeOfVariables(varDeclStmt.Locals);
    } else if (stmt is VarDeclPattern varDeclPattern) {
      UpdateTypeOfVariables(varDeclPattern.LocalVars);
      VisitPattern(varDeclPattern.LHS, context);
    } else if (stmt is AssignStmt { Rhs: TypeRhs tRhs }) {
      tRhs.Type = PreType2Type(tRhs.PreType);
      if (tRhs.ArrayDimensions != null) {
        // In this case, we expect tRhs.PreType to be an array type
        var arrayPreType = (DPreType)tRhs.PreType.Normalize();
        Contract.Assert(arrayPreType.Decl is ArrayClassDecl);
        Contract.Assert(arrayPreType.Arguments.Count == 1);
        UpdateIfOmitted(tRhs.EType, arrayPreType.Arguments[0]);
      } else {
        UpdateIfOmitted(tRhs.EType, tRhs.PreType);
      }
    } else if (stmt is AssignSuchThatStmt assignSuchThatStmt) {
      foreach (var lhs in assignSuchThatStmt.Lhss) {
        VisitExpression(lhs, context);
      }
    } else if (stmt is ProduceStmt produceStmt) {
      if (produceStmt.HiddenUpdate != null) {
        VisitStatement(produceStmt.HiddenUpdate, context);
      }
    } else if (stmt is CalcStmt calcStmt) {
      // The expression in each line has been visited, but pairs of those lines are then put together to
      // form steps. These steps (are always boolean, and) need to be visited, too. Their subexpressions
      // have already been visited, so it suffices to call PostVisitOneExpression (instead of VisitExpression)
      // here.
      foreach (var step in calcStmt.Steps) {
        PostVisitOneExpression(step, context);
      }
      PostVisitOneExpression(calcStmt.Result, context);
    } else if (stmt is ForallStmt forallStmt) {
      UpdateTypeOfVariables(forallStmt.BoundVars);
    }

    base.PostVisitOneStatement(stmt, context);

    if (stmt is UpdateStmt updateStmt) {
      foreach (var ss in updateStmt.ResolvedStatements) {
        VisitStatement(ss, context);
      }
    }
  }
}
