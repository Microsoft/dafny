using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.IO;
using System.Diagnostics.Contracts;
using DafnyCore;
using JetBrains.Annotations;
using Microsoft.BaseTypes;
using static Microsoft.Dafny.GeneratorErrors;

namespace Microsoft.Dafny.Compilers {
  public abstract partial class SinglePassCodeGenerator {


    protected void TrStmt(Statement stmt, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts = null) {
      Contract.Requires(stmt != null);
      Contract.Requires(wr != null);

      wStmts ??= wr.Fork();

      if (stmt.IsGhost) {
        return;
      }
      if (stmt is PrintStmt) {
        var s = (PrintStmt)stmt;
        foreach (var arg in s.Args) {
          EmitPrintStmt(wr, arg);
        }
      } else if (stmt is BreakStmt) {
        var s = (BreakStmt)stmt;
        var label = s.TargetStmt.Labels.Data.AssignUniqueId(idGenerator);
        if (s.IsContinue) {
          EmitContinue(label, wr);
        } else {
          EmitBreak(label, wr);
        }
      } else if (stmt is ProduceStmt) {
        var s = (ProduceStmt)stmt;
        var isTailRecursiveResult = false;
        if (s.HiddenUpdate != null) {
          TrStmt(s.HiddenUpdate, wr);
          var ss = s.HiddenUpdate.ResolvedStatements;
          if (ss.Count == 1 && ss[0] is AssignStmt assign && assign.Rhs is ExprRhs eRhs && eRhs.Expr.Resolved is FunctionCallExpr fce && IsTailRecursiveByMethodCall(fce)) {
            isTailRecursiveResult = true;
          }
        }
        if (s is YieldStmt) {
          EmitYield(wr);
        } else if (!isTailRecursiveResult) {
          EmitReturn(this.enclosingMethod.Outs, wr);
        }
      } else if (stmt is UpdateStmt) {
        var s = (UpdateStmt)stmt;
        var resolved = s.ResolvedStatements;
        if (resolved.Count == 1) {
          TrStmt(resolved[0], wr);
        } else {
          var assignStmts = resolved.Cast<AssignStmt>().Where(assignStmt => !assignStmt.IsGhost).ToList();
          var lhss = new List<Expression>();
          var rhss = new List<AssignmentRhs>();

          // multi-assignment
          Contract.Assert(s.Lhss.Count == resolved.Count);
          Contract.Assert(s.Rhss.Count == resolved.Count);
          var lhsTypes = new List<Type>();
          var rhsTypes = new List<Type>();
          foreach (var assignStmt in assignStmts) {
            var rhs = assignStmt.Rhs;
            if (rhs is HavocRhs) {
              if (Options.ForbidNondeterminism) {
                Error(ErrorId.c_nondeterminism_forbidden, rhs.Tok, "nondeterministic assignment forbidden by the --enforce-determinism option", wr);
              }
            } else {
              var lhs = assignStmt.Lhs;
              rhss.Add(rhs);
              lhss.Add(lhs);
              lhsTypes.Add(lhs.Type);
              rhsTypes.Add(TypeOfRhs(rhs));
            }
          }

          var wStmtsPre = wStmts.Fork();
          var lvalues = new List<ILvalue>();
          foreach (Expression lhs in lhss) {
            lvalues.Add(CreateLvalue(lhs, wStmts, wStmtsPre));
          }

          EmitMultiAssignment(lhss, lvalues, lhsTypes, out var wRhss, rhsTypes, wr);
          for (int i = 0; i < wRhss.Count; i++) {
            TrRhs(rhss[i], wRhss[i], wStmts);
          }
        }
      } else if (stmt is AssignStmt) {
        var s = (AssignStmt)stmt;
        Contract.Assert(s.Lhs is not SeqSelectExpr expr || expr.SelectOne);  // multi-element array assignments are not allowed
        if (s.Rhs is HavocRhs) {
          if (Options.ForbidNondeterminism) {
            Error(ErrorId.c_nondeterminism_forbidden, s.Rhs.Tok, "nondeterministic assignment forbidden by the --enforce-determinism option", wr);
          }
        } else if (s.Rhs is ExprRhs eRhs && eRhs.Expr.Resolved is FunctionCallExpr fce && IsTailRecursiveByMethodCall(fce)) {
          TrTailCallStmt(s.Tok, fce.Function.ByMethodDecl, fce.Receiver, fce.Args, null, wr);
        } else {
          var lvalue = CreateLvalue(s.Lhs, wr, wStmts);
          wStmts = wr.Fork();
          var wRhs = EmitAssignment(lvalue, TypeOfLhs(s.Lhs), TypeOfRhs(s.Rhs), wr, stmt.Tok);
          TrRhs(s.Rhs, wRhs, wStmts);
        }

      } else if (stmt is AssignSuchThatStmt) {
        var s = (AssignSuchThatStmt)stmt;
        if (Options.ForbidNondeterminism) {
          Error(ErrorId.c_assign_such_that_forbidden, s.Tok, "assign-such-that statement forbidden by the --enforce-determinism option", wr);
        }
        var lhss = s.Lhss.ConvertAll(lhs => ((IdentifierExpr)lhs.Resolved).Var);  // the resolver allows only IdentifierExpr left-hand sides
        var missingBounds = BoundedPool.MissingBounds(lhss, s.Bounds, BoundedPool.PoolVirtues.Enumerable);
        if (missingBounds.Count != 0) {
          foreach (var bv in missingBounds) {
            Error(ErrorId.c_assign_such_that_is_too_complex, s.Tok, "this assign-such-that statement is too advanced for the current compiler; Dafny's heuristics cannot find any bound for variable '{0}'", wr, bv.Name);
          }
        } else {
          Contract.Assert(s.Bounds != null);
          TrAssignSuchThat(lhss, s.Expr, s.Bounds, s.Tok.line, wr, false);
        }
      } else if (stmt is AssignOrReturnStmt) {
        var s = (AssignOrReturnStmt)stmt;
        // TODO there's potential here to use target-language specific features such as exceptions
        // to make it more target-language idiomatic and improve performance
        TrStmtList(s.ResolvedStatements, wr);

      } else if (stmt is ExpectStmt) {
        var s = (ExpectStmt)stmt;
        // TODO there's potential here to use target-language specific features such as exceptions
        // to make it more target-language idiomatic and improve performance
        ConcreteSyntaxTree bodyWriter = EmitIf(out var guardWriter, false, wr);
        var negated = new UnaryOpExpr(s.Tok, UnaryOpExpr.Opcode.Not, s.Expr);
        negated.Type = Type.Bool;
        EmitExpr(negated, false, guardWriter, wStmts);
        EmitHalt(s.Tok, s.Message, bodyWriter);

      } else if (stmt is CallStmt) {
        var s = (CallStmt)stmt;
        var wrBefore = wr.Fork();
        var wrCall = wr.Fork();
        var wrAfter = wr;
        TrCallStmt(s, null, wrCall, wrBefore, wrAfter);

      } else if (stmt is BlockStmt) {
        var w = EmitBlock(wr);
        TrStmtList(((BlockStmt)stmt).Body, w);

      } else if (stmt is IfStmt) {
        IfStmt s = (IfStmt)stmt;
        if (s.Guard == null) {
          if (Options.ForbidNondeterminism) {
            Error(ErrorId.c_nondeterministic_if_forbidden, s.Tok, "nondeterministic if statement forbidden by the --enforce-determinism option", wr);
          }
          // we can compile the branch of our choice
          ConcreteSyntaxTree guardWriter;
          if (s.Els == null) {
            // let's compile the "else" branch, since that involves no work
            // (still, let's leave a marker in the source code to indicate that this is what we did)
            Coverage.UnusedInstrumentationPoint(s.Thn.Tok, "then branch");
            var notFalse = (UnaryOpExpr)Expression.CreateNot(s.Thn.Tok, Expression.CreateBoolLiteral(s.Thn.Tok, false));
            var thenWriter = EmitIf(out guardWriter, false, wr);
            EmitUnaryExpr(ResolvedUnaryOp.BoolNot, notFalse.E, false, guardWriter, wStmts);
            Coverage.Instrument(s.Tok, "implicit else branch", wr);
            thenWriter = EmitIf(out guardWriter, false, thenWriter);
            EmitUnaryExpr(ResolvedUnaryOp.BoolNot, notFalse.E, false, guardWriter, wStmts);
            TrStmtList(new List<Statement>(), thenWriter);
          } else {
            // let's compile the "then" branch
            wr = EmitIf(out guardWriter, false, wr);
            EmitExpr(Expression.CreateBoolLiteral(s.Thn.tok, true), false, guardWriter, wStmts);
            Coverage.Instrument(s.Thn.Tok, "then branch", wr);
            TrStmtList(s.Thn.Body, wr);
            Coverage.UnusedInstrumentationPoint(s.Els.Tok, "else branch");
          }
        } else {
          if (s.IsBindingGuard && Options.ForbidNondeterminism) {
            Error(ErrorId.c_binding_if_forbidden, s.Tok, "binding if statement forbidden by the --enforce-determinism option", wr);
          }

          var coverageForElse = Coverage.IsRecording && !(s.Els is IfStmt);
          var thenWriter = EmitIf(out var guardWriter, s.Els != null || coverageForElse, wr);
          EmitExpr(s.IsBindingGuard ? ((ExistsExpr)s.Guard).AlphaRename("eg_d") : s.Guard, false, guardWriter, wStmts);
          // We'd like to do "TrStmt(s.Thn, indent)", except we want the scope of any existential variables to come inside the block
          if (s.IsBindingGuard) {
            IntroduceAndAssignBoundVars((ExistsExpr)s.Guard, thenWriter);
          }
          Coverage.Instrument(s.Thn.Tok, "then branch", thenWriter);
          TrStmtList(s.Thn.Body, thenWriter);

          if (coverageForElse) {
            wr = EmitBlock(wr);
            if (s.Els == null) {
              Coverage.Instrument(s.Tok, "implicit else branch", wr);
            } else {
              Coverage.Instrument(s.Els.Tok, "else branch", wr);
            }
          }
          if (s.Els != null) {
            TrStmtNonempty(s.Els, wr, wStmts);
          }
        }

      } else if (stmt is AlternativeStmt) {
        var s = (AlternativeStmt)stmt;
        if (Options.ForbidNondeterminism && 2 <= s.Alternatives.Count) {
          Error(ErrorId.c_case_based_if_forbidden, s.Tok, "case-based if statement forbidden by the --enforce-determinism option", wr);
        }
        foreach (var alternative in s.Alternatives) {
          var thn = EmitIf(out var guardWriter, true, wr);
          EmitExpr(alternative.IsBindingGuard ? ((ExistsExpr)alternative.Guard).AlphaRename("eg_d") : alternative.Guard, false, guardWriter, wStmts);
          if (alternative.IsBindingGuard) {
            IntroduceAndAssignBoundVars((ExistsExpr)alternative.Guard, thn);
          }
          Coverage.Instrument(alternative.Tok, "if-case branch", thn);
          TrStmtList(alternative.Body, thn);
        }
        var wElse = EmitBlock(wr);
        EmitAbsurd("unreachable alternative", wElse);

      } else if (stmt is WhileStmt) {
        WhileStmt s = (WhileStmt)stmt;
        if (s.Body == null) {
          return;
        }
        if (s.Guard == null) {
          if (Options.ForbidNondeterminism) {
            Error(ErrorId.c_non_deterministic_loop_forbidden, s.Tok, "nondeterministic loop forbidden by the --enforce-determinism option", wr);
          }
          // This loop is allowed to stop iterating at any time. We choose to never iterate, but we still
          // emit a loop structure. The structure "while (false) { }" comes to mind, but that results in
          // an "unreachable code" error from Java, so we instead use "while (true) { break; }".
          var wBody = CreateWhileLoop(out var guardWriter, wr);
          EmitExpr(Expression.CreateBoolLiteral(s.Body.tok, true), false, guardWriter, wStmts);
          EmitBreak(null, wBody);
          Coverage.UnusedInstrumentationPoint(s.Body.Tok, "while body");
        } else {
          var guardWriter = EmitWhile(s.Body.Tok, s.Body.Body, s.Labels, wr);
          EmitExpr(s.Guard, false, guardWriter, wStmts);
        }

      } else if (stmt is AlternativeLoopStmt loopStmt) {
        if (Options.ForbidNondeterminism) {
          Error(ErrorId.c_case_based_loop_forbidden, loopStmt.Tok, "case-based loop forbidden by the --enforce-determinism option", wr);
        }
        if (loopStmt.Alternatives.Count != 0) {
          var w = CreateWhileLoop(out var whileGuardWriter, wr);
          EmitExpr(Expression.CreateBoolLiteral(loopStmt.tok, true), false, whileGuardWriter, wStmts);
          w = EmitContinueLabel(loopStmt.Labels, w);
          foreach (var alternative in loopStmt.Alternatives) {
            var thn = EmitIf(out var guardWriter, true, w);
            EmitExpr(alternative.Guard, false, guardWriter, wStmts);
            Coverage.Instrument(alternative.Tok, "while-case branch", thn);
            TrStmtList(alternative.Body, thn);
          }
          var wElse = EmitBlock(w);
          {
            EmitBreak(null, wElse);
          }
        }

      } else if (stmt is ForLoopStmt) {
        var s = (ForLoopStmt)stmt;
        if (s.Body == null) {
          return;
        }
        string endVarName = null;
        if (s.End != null) {
          // introduce a variable to hold the value of the end-expression
          endVarName = ProtectedFreshId(s.GoingUp ? "_hi" : "_lo");
          wStmts = wr.Fork();
          EmitExpr(s.End, false, DeclareLocalVar(endVarName, s.End.Type, s.End.tok, wr), wStmts);
        }
        var startExprWriter = EmitForStmt(s.Tok, s.LoopIndex, s.GoingUp, endVarName, s.Body.Body, s.Labels, wr);
        EmitExpr(s.Start, false, startExprWriter, wStmts);

      } else if (stmt is ForallStmt) {
        var s = (ForallStmt)stmt;
        if (s.Kind != ForallStmt.BodyKind.Assign) {
          // Call and Proof have no side effects, so they can simply be optimized away.
          return;
        } else if (s.BoundVars.Count == 0) {
          // the bound variables just spell out a single point, so the forall statement is equivalent to one execution of the body
          TrStmt(s.Body, wr);
          return;
        }
        var s0 = (AssignStmt)s.S0;
        if (s0.Rhs is HavocRhs) {
          if (Options.ForbidNondeterminism) {
            Error(ErrorId.c_nondeterminism_forbidden, s0.Rhs.Tok, "nondeterministic assignment forbidden by --enforce-determinism", wr);
          }
          // The forall statement says to havoc a bunch of things.  This can be efficiently compiled
          // into doing nothing.
          return;
        }
        var rhs = ((ExprRhs)s0.Rhs).Expr;

        if (CanSequentializeForall(s.BoundVars, s.Bounds, s.Range, s0.Lhs, rhs)) {
          // Just put the statement inside the loops
          var wLoop = CompileGuardedLoops(s.BoundVars, s.Bounds, s.Range, wr);
          TrStmt(s0, wLoop);
        } else {
          // Compile:
          //   forall (w,x,y,z | Range(w,x,y,z)) {
          //     LHS(w,x,y,z) := RHS(w,x,y,z);
          //   }
          // where w,x,y,z have types seq<W>,set<X>,int,bool and LHS has L-1 top-level subexpressions
          // (that is, L denotes the number of top-level subexpressions of LHS plus 1),
          // into:
          //   var ingredients = new List< L-Tuple >();
          //   foreach (W w in sq.UniqueElements) {
          //     foreach (X x in st.Elements) {
          //       for (BigInteger y = Lo; j < Hi; j++) {
          //         for (bool z in Helper.AllBooleans) {
          //           if (Range(w,x,y,z)) {
          //             ingredients.Add(new L-Tuple( LHS0(w,x,y,z), LHS1(w,x,y,z), ..., RHS(w,x,y,z) ));
          //           }
          //         }
          //       }
          //     }
          //   }
          //   foreach (L-Tuple l in ingredients) {
          //     LHS[ l0, l1, l2, ..., l(L-2) ] = l(L-1);
          //   }
          //
          // Note, because the .NET Tuple class only supports up to 8 components, the compiler implementation
          // here supports arrays only up to 6 dimensions.  This does not seem like a serious practical limitation.
          // However, it may be more noticeable if the forall statement supported forall assignments in its
          // body.  To support cases where tuples would need more than 8 components, .NET Tuple's would have to
          // be nested.

          // Temporary names
          var c = ProtectedFreshNumericId("_ingredients+_tup");
          string ingredients = "_ingredients" + c;
          string tup = "_tup" + c;

          // Compute L
          int L;
          string tupleTypeArgs;
          List<Type> tupleTypeArgsList;
          if (s0.Lhs is MemberSelectExpr) {
            var lhs = (MemberSelectExpr)s0.Lhs;
            L = 2;
            tupleTypeArgs = TypeArgumentName(lhs.Obj.Type, wr, lhs.tok);
            tupleTypeArgsList = new List<Type> { lhs.Obj.Type };
          } else if (s0.Lhs is SeqSelectExpr) {
            var lhs = (SeqSelectExpr)s0.Lhs;
            L = 3;
            // note, we might as well do the BigInteger-to-int cast for array indices here, before putting things into the Tuple rather than when they are extracted from the Tuple
            tupleTypeArgs = TypeArgumentName(lhs.Seq.Type, wr, lhs.tok) + IntSelect;
            tupleTypeArgsList = new List<Type> { lhs.Seq.Type, null };
          } else {
            var lhs = (MultiSelectExpr)s0.Lhs;
            L = 2 + lhs.Indices.Count;
            if (8 < L) {
              Error(ErrorId.c_no_assignments_to_seven_d_arrays, lhs.tok, "compiler currently does not support assignments to more-than-6-dimensional arrays in forall statements", wr);
              return;
            }
            tupleTypeArgs = TypeArgumentName(lhs.Array.Type, wr, lhs.tok);
            tupleTypeArgsList = new List<Type> { lhs.Array.Type };
            for (int i = 0; i < lhs.Indices.Count; i++) {
              // note, we might as well do the BigInteger-to-int cast for array indices here, before putting things into the Tuple rather than when they are extracted from the Tuple
              tupleTypeArgs += IntSelect;
              tupleTypeArgsList.Add(null);
            }

          }
          tupleTypeArgs += "," + TypeArgumentName(rhs.Type, wr, rhs.tok);
          tupleTypeArgsList.Add(rhs.Type);

          // declare and construct "ingredients"
          var wrOuter = EmitIngredients(wr, ingredients, L, tupleTypeArgs, s, s0, rhs);

          //   foreach (L-Tuple l in ingredients) {
          //     LHS[ l0, l1, l2, ..., l(L-2) ] = l(L-1);
          //   }
          TargetTupleSize = L;
          wr = CreateForeachIngredientLoop(tup, L, tupleTypeArgs, out var collWriter, wrOuter);
          collWriter.Write(ingredients);
          {
            var wTup = new ConcreteSyntaxTree(wr.RelativeIndentLevel);
            var wCoerceTup = EmitCoercionToArbitraryTuple(wTup);
            wCoerceTup.Write(tup);
            tup = wTup.ToString();
          }
          if (s0.Lhs is MemberSelectExpr) {
            EmitMemberSelect(s0, tupleTypeArgsList, wr, tup);
          } else if (s0.Lhs is SeqSelectExpr) {
            EmitSeqSelect(s0, tupleTypeArgsList, wr, tup);
          } else {
            EmitMultiSelect(s0, tupleTypeArgsList, wr, tup, L);
          }
        }
      } else if (stmt is NestedMatchStmt nestedMatchStmt) {
        TrStmt(nestedMatchStmt.Flattened, wr, wStmts);
      } else if (stmt is MatchStmt matchStmt) {
        EmitMatchStmt(wr, matchStmt);
      } else if (stmt is VarDeclStmt) {
        var s = (VarDeclStmt)stmt;
        var i = 0;
        foreach (var local in s.Locals) {
          bool hasRhs = s.Update is AssignSuchThatStmt || s.Update is AssignOrReturnStmt;
          if (!hasRhs && s.Update is UpdateStmt u) {
            if (i < u.Rhss.Count && u.Rhss[i] is HavocRhs) {
              // there's no specific initial value
            } else {
              hasRhs = true;
            }
          }
          TrLocalVar(local, !hasRhs, wr);
          i++;
        }
        if (s.Update != null) {
          TrStmt(s.Update, wr);
        }

      } else if (stmt is VarDeclPattern) {
        var s = (VarDeclPattern)stmt;
        if (Contract.Exists(s.LHS.Vars, bv => !bv.IsGhost)) {
          TrCasePatternOpt(s.LHS, s.RHS, wr, false);
        }
      } else if (stmt is ModifyStmt) {
        var s = (ModifyStmt)stmt;
        if (s.Body != null) {
          TrStmt(s.Body, wr);
        } else if (Options.ForbidNondeterminism) {
          Error(ErrorId.c_bodyless_modify_statement_forbidden, s.Tok, "modify statement without a body forbidden by the --enforce-determinism option", wr);
        }
      } else if (stmt is TryRecoverStatement h) {
        EmitHaltRecoveryStmt(h.TryBody, IdName(h.HaltMessageVar), h.RecoverBody, wr);
      } else {
        Contract.Assert(false); throw new cce.UnreachableException();  // unexpected statement
      }
    }

    private void EmitMatchStmt(ConcreteSyntaxTree wr, MatchStmt s) {
      // Type source = e;
      // if (source.is_Ctor0) {
      //   FormalType f0 = ((Dt_Ctor0)source._D).a0;
      //   ...
      //   Body0;
      // } else if (...) {
      //   ...
      // } else if (true) {
      //   ...
      // }
      if (s.Cases.Count != 0) {
        string source = ProtectedFreshId("_source");
        DeclareLocalVar(source, s.Source.Type, s.Source.tok, s.Source, false, wr);

        int i = 0;
        var sourceType = (UserDefinedType)s.Source.Type.NormalizeExpand();
        foreach (MatchCaseStmt mc in s.Cases) {
          var w = MatchCasePrelude(source, sourceType, cce.NonNull(mc.Ctor), mc.Arguments, i, s.Cases.Count, wr);
          TrStmtList(mc.Body, w);
          i++;
        }
      }
    }
  }
}