using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Microsoft.Dafny;

/// <summary>
/// Automatically accumulate requires for function calls within a function body,
/// if requested via {:autoreq}
/// </summary>
public class AutoReqFunctionRewriter : IRewriter {
  Function parentFunction;
  bool containsMatch; // TODO: Track this per-requirement, rather than per-function
  private BuiltIns builtIns;

  public AutoReqFunctionRewriter(ErrorReporter reporter, BuiltIns builtIns)
    : base(reporter) {
    Contract.Requires(reporter != null);
    this.builtIns = builtIns;
  }

  internal override void PostResolveIntermediate(ModuleDefinition m) {
    var components = m.CallGraph.TopologicallySortedComponents();

    foreach (var scComponent in components) {  // Visit the call graph bottom up, so anything we call already has its prequisites calculated
      if (scComponent is Function) {
        Function fn = (Function)scComponent;
        if (Attributes.ContainsBoolAtAnyLevel(fn, "autoReq")) {
          parentFunction = fn;  // Remember where the recursion started
          containsMatch = false;  // Assume no match statements are involved

          List<AttributedExpression> auto_reqs = new List<AttributedExpression>();

          // First handle all of the requirements' preconditions
          foreach (AttributedExpression req in fn.Req) {
            foreach (Expression e in GenerateAutoReqs(req.E)) {
              auto_reqs.Add(CreateAutoAttributedExpression(e, req.Attributes));
            }
          }
          fn.Req.InsertRange(0, auto_reqs); // Need to come before the actual requires
          addAutoReqToolTipInfoToFunction("pre", fn, auto_reqs);

          // Then the body itself, if any
          if (fn.Body != null) {
            auto_reqs = new List<AttributedExpression>();
            foreach (Expression e in GenerateAutoReqs(fn.Body)) {
              auto_reqs.Add(CreateAutoAttributedExpression(e));
            }
            fn.Req.AddRange(auto_reqs);
            addAutoReqToolTipInfoToFunction("post", fn, auto_reqs);
          }
        }
      } else if (scComponent is Method) {
        Method method = (Method)scComponent;
        if (Attributes.ContainsBoolAtAnyLevel(method, "autoReq")) {
          parentFunction = null;
          containsMatch = false; // Assume no match statements are involved

          List<AttributedExpression> auto_reqs = new List<AttributedExpression>();
          foreach (AttributedExpression req in method.Req) {
            List<Expression> local_auto_reqs = GenerateAutoReqs(req.E);
            foreach (Expression local_auto_req in local_auto_reqs) {
              auto_reqs.Add(CreateAutoAttributedExpression(local_auto_req));
            }
          }
          method.Req.InsertRange(0, auto_reqs); // Need to come before the actual requires
          addAutoReqToolTipInfoToMethod("pre", method, auto_reqs);
        }
      }
    }
  }

  public void addAutoReqToolTipInfoToFunction(string label, Function f, List<AttributedExpression> reqs) {
    string prefix = "auto requires " + label + " ";
    string tip = "";

    string sep = "";
    foreach (var req in reqs) {
      if (containsMatch) {  // Pretty print the requirements
        tip += sep + prefix + Printer.ExtendedExprToString(Reporter.Options, req.E) + ";";
      } else {
        tip += sep + prefix + Printer.ExprToString(Reporter.Options, req.E) + ";";
      }
      sep = "\n";
    }

    if (!tip.Equals("")) {
      Reporter.Info(MessageSource.Rewriter, f.tok, tip);
      if (Reporter.Options.AutoReqPrintFile != null) {
        using (System.IO.TextWriter writer = new System.IO.StreamWriter(Reporter.Options.AutoReqPrintFile, true)) {
          writer.WriteLine(f.Name);
          writer.WriteLine("\t" + tip);
        }
      }
    }
  }

  public void addAutoReqToolTipInfoToMethod(string label, Method method, List<AttributedExpression> reqs) {
    string tip = "";

    foreach (var req in reqs) {
      string prefix = "auto ";
      prefix += " requires " + label + " ";
      if (containsMatch) {  // Pretty print the requirements
        tip += prefix + Printer.ExtendedExprToString(Reporter.Options, req.E) + ";\n";
      } else {
        tip += prefix + Printer.ExprToString(Reporter.Options, req.E) + ";\n";
      }
    }

    if (!tip.Equals("")) {
      Reporter.Info(MessageSource.Rewriter, method.tok, tip);
      if (Reporter.Options.AutoReqPrintFile != null) {
        using System.IO.TextWriter writer = new System.IO.StreamWriter(Reporter.Options.AutoReqPrintFile, true);
        writer.WriteLine(method.Name);
        writer.WriteLine("\t" + tip);
      }
    }
  }

  // Stitch a list of expressions together with logical ands
  Expression Andify(IToken tok, List<Expression> exprs) {
    Expression ret = Expression.CreateBoolLiteral(new AutoGeneratedToken(tok), true);

    foreach (var expr in exprs) {
      ret = Expression.CreateAnd(ret, expr);
    }

    return ret;
  }

  static AttributedExpression CreateAutoAttributedExpression(Expression e, Attributes attrs = null) {
    return new AttributedExpression(AutoGeneratedToken.WrapExpression(e), attrs);
  }

  List<Expression> gatherReqs(Function f, List<Expression> args, List<Type> typeArguments, Expression f_this) {
    List<Expression> translated_f_reqs = new List<Expression>();

    if (f.Req.Count > 0) {
      Dictionary<IVariable, Expression/*!*/> substMap = new Dictionary<IVariable, Expression>();
      Dictionary<TypeParameter, Type> typeMap = TypeParameter.SubstitutionMap(f.TypeArgs, typeArguments);

      for (int i = 0; i < f.Formals.Count; i++) {
        substMap.Add(f.Formals[i], args[i]);
      }

      foreach (var req in f.Req) {
        var sub = new AutoReqSubstituter(f_this, substMap, typeMap, builtIns);
        translated_f_reqs.Add(sub.Substitute(req.E));
      }
    }

    return translated_f_reqs;
  }

  class AutoReqSubstituter : Substituter {
    public AutoReqSubstituter(Expression receiverReplacement, Dictionary<IVariable, Expression> substMap, Dictionary<TypeParameter, Type> typeMap,
      BuiltIns builtIns)
      : base(receiverReplacement, substMap, typeMap, null, builtIns) {
    }

    public override Expression Substitute(Expression expr) {
      var r = base.Substitute(expr);
      if (r is MemberSelectExpr memberSelectExpr) {
        return Expression.WrapResolvedMemberSelect(memberSelectExpr);
      } else if (r is FunctionCallExpr functionCallExpr) {
        return Expression.WrapResolvedCall(functionCallExpr, builtIns);
      }
      return r;
    }
  }

  List<Expression> GenerateAutoReqs(Expression expr) {
    List<Expression> reqs = new List<Expression>();

    if (expr is LiteralExpr) {
    } else if (expr is ThisExpr) {
    } else if (expr is IdentifierExpr) {
    } else if (expr is SetDisplayExpr) {
      SetDisplayExpr e = (SetDisplayExpr)expr;

      foreach (var elt in e.Elements) {
        reqs.AddRange(GenerateAutoReqs(elt));
      }
    } else if (expr is MultiSetDisplayExpr) {
      MultiSetDisplayExpr e = (MultiSetDisplayExpr)expr;
      foreach (var elt in e.Elements) {
        reqs.AddRange(GenerateAutoReqs(elt));
      }
    } else if (expr is SeqDisplayExpr) {
      SeqDisplayExpr e = (SeqDisplayExpr)expr;
      foreach (var elt in e.Elements) {
        reqs.AddRange(GenerateAutoReqs(elt));
      }
    } else if (expr is MapDisplayExpr) {
      MapDisplayExpr e = (MapDisplayExpr)expr;

      foreach (ExpressionPair p in e.Elements) {
        reqs.AddRange(GenerateAutoReqs(p.A));
        reqs.AddRange(GenerateAutoReqs(p.B));
      }
    } else if (expr is MemberSelectExpr) {
      MemberSelectExpr e = (MemberSelectExpr)expr;
      Contract.Assert(e.Member != null && e.Member is Field);

      reqs.AddRange(GenerateAutoReqs(e.Obj));
    } else if (expr is SeqSelectExpr) {
      SeqSelectExpr e = (SeqSelectExpr)expr;

      reqs.AddRange(GenerateAutoReqs(e.Seq));
      if (e.E0 != null) {
        reqs.AddRange(GenerateAutoReqs(e.E0));
      }

      if (e.E1 != null) {
        reqs.AddRange(GenerateAutoReqs(e.E1));
      }
    } else if (expr is SeqUpdateExpr) {
      SeqUpdateExpr e = (SeqUpdateExpr)expr;
      reqs.AddRange(GenerateAutoReqs(e.Seq));
      reqs.AddRange(GenerateAutoReqs(e.Index));
      reqs.AddRange(GenerateAutoReqs(e.Value));
    } else if (expr is DatatypeUpdateExpr) {
      foreach (var ee in expr.SubExpressions) {
        reqs.AddRange(GenerateAutoReqs(ee));
      }
    } else if (expr is FunctionCallExpr) {
      FunctionCallExpr e = (FunctionCallExpr)expr;

      // All of the arguments need to be satisfied
      foreach (var arg in e.Args) {
        reqs.AddRange(GenerateAutoReqs(arg));
      }

      if (parentFunction != null && ModuleDefinition.InSameSCC(e.Function, parentFunction)) {
        // We're making a call within the same SCC, so don't descend into this function
      } else {
        reqs.AddRange(gatherReqs(e.Function, e.Args, e.TypeApplication_JustFunction, e.Receiver));
      }
    } else if (expr is DatatypeValue) {
      DatatypeValue dtv = (DatatypeValue)expr;
      Contract.Assert(dtv.Ctor != null);  // since dtv has been successfully resolved
      for (int i = 0; i < dtv.Arguments.Count; i++) {
        Expression arg = dtv.Arguments[i];
        reqs.AddRange(GenerateAutoReqs(arg));
      }
    } else if (expr is OldExpr) {
    } else if (expr is MatchExpr) {
      MatchExpr e = (MatchExpr)expr;
      containsMatch = true;
      reqs.AddRange(GenerateAutoReqs(e.Source));

      List<MatchCaseExpr> newMatches = new List<MatchCaseExpr>();
      foreach (MatchCaseExpr caseExpr in e.Cases) {
        //MatchCaseExpr c = new MatchCaseExpr(caseExpr.tok, caseExpr.Id, caseExpr.Arguments, andify(caseExpr.tok, generateAutoReqs(caseExpr.Body)));
        //c.Ctor = caseExpr.Ctor; // resolve here
        MatchCaseExpr c = Expression.CreateMatchCase(caseExpr, Andify(caseExpr.tok, GenerateAutoReqs(caseExpr.Body)));
        newMatches.Add(c);
      }

      reqs.Add(Expression.CreateMatch(e.tok, e.Source, newMatches, e.Type));
    } else if (expr is SeqConstructionExpr) {
      var e = (SeqConstructionExpr)expr;
      reqs.AddRange(GenerateAutoReqs(e.N));
      reqs.AddRange(GenerateAutoReqs(e.Initializer));
    } else if (expr is MultiSetFormingExpr) {
      MultiSetFormingExpr e = (MultiSetFormingExpr)expr;
      reqs.AddRange(GenerateAutoReqs(e.E));
    } else if (expr is UnaryExpr) {
      UnaryExpr e = (UnaryExpr)expr;
      Expression arg = e.E;
      reqs.AddRange(GenerateAutoReqs(arg));
    } else if (expr is BinaryExpr) {
      BinaryExpr e = (BinaryExpr)expr;

      switch (e.ResolvedOp) {
        case BinaryExpr.ResolvedOpcode.Imp:
        case BinaryExpr.ResolvedOpcode.And:
          reqs.AddRange(GenerateAutoReqs(e.E0));
          foreach (var req in GenerateAutoReqs(e.E1)) {
            // We only care about this req if E0 is true, since And short-circuits
            var cloner = new Cloner(true);
            var e0 = cloner.CloneExpr(e.E0);
            reqs.Add(Expression.CreateImplies(e0, req));
          }
          break;

        case BinaryExpr.ResolvedOpcode.Or:
          reqs.AddRange(GenerateAutoReqs(e.E0));
          foreach (var req in GenerateAutoReqs(e.E1)) {
            // We only care about this req if E0 is false, since Or short-circuits
            var cloner = new Cloner(true);
            var e0 = cloner.CloneExpr(e.E0);
            reqs.Add(Expression.CreateImplies(Expression.CreateNot(e.E1.tok, e0), req));
          }
          break;

        default:
          reqs.AddRange(GenerateAutoReqs(e.E0));
          reqs.AddRange(GenerateAutoReqs(e.E1));
          break;
      }
    } else if (expr is TernaryExpr) {
      var e = (TernaryExpr)expr;

      reqs.AddRange(GenerateAutoReqs(e.E0));
      reqs.AddRange(GenerateAutoReqs(e.E1));
      reqs.AddRange(GenerateAutoReqs(e.E2));
    } else if (expr is LetExpr) {
      var e = (LetExpr)expr;

      if (e.Exact) {
        foreach (var rhs in e.RHSs) {
          reqs.AddRange(GenerateAutoReqs(rhs));
        }
        var new_reqs = GenerateAutoReqs(e.Body);
        if (new_reqs.Count > 0) {
          reqs.Add(Expression.CreateLet(e.tok, e.LHSs, e.RHSs, Andify(e.tok, new_reqs), e.Exact));
        }
      } else {
        // TODO: Still need to figure out what the right choice is here:
        // Given: var x :| g(x); f(x, y) do we:
        //    1) Update the original statement to be: var x :| g(x) && WP(f(x,y)); f(x, y)
        //    2) Add forall x :: g(x) ==> WP(f(x, y)) to the function's requirements
        //    3) Current option -- do nothing.  Up to the spec writer to fix
      }
    } else if (expr is QuantifierExpr) {
      QuantifierExpr e = (QuantifierExpr)expr;

      // See LetExpr for issues with the e.Range

      var auto_reqs = GenerateAutoReqs(e.Term);
      if (auto_reqs.Count > 0) {
        Expression allReqsSatisfied = Andify(e.Term.tok, auto_reqs);
        Expression allReqsSatisfiedAndTerm = Expression.CreateAnd(allReqsSatisfied, e.Term);
        e.Term = allReqsSatisfiedAndTerm;
        Reporter.Info(MessageSource.Rewriter, e.tok, "autoreq added (" + Printer.ExtendedExprToString(Reporter.Options, allReqsSatisfied) + ") &&");
      }
    } else if (expr is SetComprehension) {
      var e = (SetComprehension)expr;
      // Translate "set xs | R :: T"

      // See LetExpr for issues with the e.Range
      //reqs.AddRange(generateAutoReqs(e.Range));
      var auto_reqs = GenerateAutoReqs(e.Term);
      if (auto_reqs.Count > 0) {
        reqs.Add(Expression.CreateQuantifier(new ForallExpr(e.tok, e.RangeToken, e.BoundVars, e.Range, Andify(e.Term.tok, auto_reqs), e.Attributes), true));
      }
    } else if (expr is MapComprehension) {
      var e = (MapComprehension)expr;
      // Translate "map x | R :: T" into
      // See LetExpr for issues with the e.Range
      //reqs.AddRange(generateAutoReqs(e.Range));
      var auto_reqs = new List<Expression>();
      if (e.TermLeft != null) {
        auto_reqs.AddRange(GenerateAutoReqs(e.TermLeft));
      }
      auto_reqs.AddRange(GenerateAutoReqs(e.Term));
      if (auto_reqs.Count > 0) {
        reqs.Add(Expression.CreateQuantifier(new ForallExpr(e.tok, e.RangeToken, e.BoundVars, e.Range, Andify(e.Term.tok, auto_reqs), e.Attributes), true));
      }
    } else if (expr is StmtExpr) {
      var e = (StmtExpr)expr;
      reqs.AddRange(GenerateAutoReqs(e.E));
    } else if (expr is ITEExpr) {
      ITEExpr e = (ITEExpr)expr;
      reqs.AddRange(GenerateAutoReqs(e.Test));
      reqs.Add(Expression.CreateITE(e.Test, Andify(e.Thn.tok, GenerateAutoReqs(e.Thn)), Andify(e.Els.tok, GenerateAutoReqs(e.Els))));
    } else if (expr is NestedMatchExpr) {
      containsMatch = true;

      var e = (NestedMatchExpr)expr;
      reqs.AddRange(GenerateAutoReqs(e.Source));

      var newCases = e.Cases.Select(cas =>
        new NestedMatchCaseExpr(cas.Tok, cas.Pat, Andify(cas.Body.tok, GenerateAutoReqs(cas.Body)), cas.Attributes)).ToList();
      var nestedMatchExpr = new NestedMatchExpr(e.tok, e.Source, newCases, e.UsesOptionalBraces);
      nestedMatchExpr.Type = Type.Bool;
      reqs.Add(nestedMatchExpr);
    } else if (expr is ConcreteSyntaxExpression) {
      var e = (ConcreteSyntaxExpression)expr;
      reqs.AddRange(GenerateAutoReqs(e.ResolvedExpression));
    } else {
      //Contract.Assert(false); throw new cce.UnreachableException();  // unexpected expression
    }

    return reqs;
  }
}