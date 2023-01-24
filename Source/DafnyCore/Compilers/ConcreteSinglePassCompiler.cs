using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Numerics;
using JetBrains.Annotations;
using Microsoft.BaseTypes;

namespace Microsoft.Dafny.Compilers;

public abstract class ConcreteSinglePassCompiler : SinglePassCompiler<ICanRender> {
  public override void OnPreCompile(ErrorReporter reporter, ReadOnlyCollection<string> otherFileNames) {
    base.OnPreCompile(reporter, otherFileNames);
    Coverage = new CoverageInstrumenter(this);
  }

  // TODO Move TrExpr to SinglePassCompiler once all the methods it calls have been made to use TExpression instead of ConcreteSyntaxTree
  /// <summary>
  /// Before calling TrExpr(expr), the caller must have spilled the let variables declared in "expr".
  /// In order to give the compiler a way to put supporting statements above the current one, wStmts must be passed.
  /// </summary>
  protected internal override ICanRender TrExpr(Expression expr, bool inLetExprBody, ConcreteSyntaxTree wStmts) {
    Contract.Requires(expr != null);

    var wr = new ConcreteSyntaxTree();
    if (expr is LiteralExpr) {
      LiteralExpr e = (LiteralExpr)expr;
      return EmitLiteralExpr(e);
    } else if (expr is ThisExpr) {
      var thisWriter = wr;
      if (thisContext != null) {
        var instantiatedType = expr.Type.Subst(thisContext.ParentFormalTypeParametersToActuals);
        thisWriter = EmitCoercionIfNecessary(UserDefinedType.FromTopLevelDecl(expr.tok, thisContext), instantiatedType, expr.tok, wr);
      }

      thisWriter.Append(EmitThis());

      return wr;
    } else if (expr is IdentifierExpr) {
      var e = (IdentifierExpr)expr;
      if (inLetExprBody && !(e.Var is BoundVar)) {
        // copy variable to a temp since
        //   - C# doesn't allow out param in letExpr body, and
        //   - Java doesn't allow any non-final variable in letExpr body.
        var name = ProtectedFreshId("_pat_let_tv");
        wr.Write(name);
        DeclareLocalVar(name, null, null, false, IdName(e.Var), copyInstrWriters.Peek(), e.Type);
      } else {
        wr.Write(IdName(e.Var));
      }
    } else if (expr is SetDisplayExpr) {
      var e = (SetDisplayExpr)expr;
      EmitCollectionDisplay(e.Type.AsSetType, e.tok, e.Elements, inLetExprBody, wr, wStmts);

    } else if (expr is MultiSetDisplayExpr) {
      var e = (MultiSetDisplayExpr)expr;
      EmitCollectionDisplay(e.Type.AsMultiSetType, e.tok, e.Elements, inLetExprBody, wr, wStmts);

    } else if (expr is SeqDisplayExpr) {
      var e = (SeqDisplayExpr)expr;
      EmitCollectionDisplay(e.Type.AsSeqType, e.tok, e.Elements, inLetExprBody, wr, wStmts);

    } else if (expr is MapDisplayExpr) {
      var e = (MapDisplayExpr)expr;
      EmitMapDisplay(e.Type.AsMapType, e.tok, e.Elements, inLetExprBody, wr, wStmts);

    } else if (expr is MemberSelectExpr) {
      MemberSelectExpr e = (MemberSelectExpr)expr;
      SpecialField sf = e.Member as SpecialField;
      if (sf != null) {
        string compiledName, preStr, postStr;
        GetSpecialFieldInfo(sf.SpecialId, sf.IdParam, e.Obj.Type, out compiledName, out preStr, out postStr);
        wr.Write(preStr);

        if (sf.IsStatic && !SupportsStaticsInGenericClasses && sf.EnclosingClass.TypeArgs.Count != 0) {
          var typeArgs = e.TypeApplication_AtEnclosingClass;
          Contract.Assert(typeArgs.Count == sf.EnclosingClass.TypeArgs.Count);
          wr.Write("{0}.", TypeName_Companion(e.Obj.Type, wr, e.tok, sf));
          EmitNameAndActualTypeArgs(IdName(e.Member), typeArgs, e.tok, wr);
          var tas = TypeArgumentInstantiation.ListFromClass(sf.EnclosingClass, typeArgs);
          EmitTypeDescriptorsActuals(tas, e.tok, wr.ForkInParens());
        } else {
          void writeObj(ConcreteSyntaxTree w) {
            //Contract.Assert(!sf.IsStatic);
            w = EmitCoercionIfNecessary(e.Obj.Type, UserDefinedType.UpcastToMemberEnclosingType(e.Obj.Type, e.Member), e.tok, w);
            TrParenExpr(e.Obj, w, inLetExprBody, wStmts);
          }

          var typeArgs = CombineAllTypeArguments(e.Member, e.TypeApplication_AtEnclosingClass, e.TypeApplication_JustMember);
          EmitMemberSelect(writeObj, e.Obj.Type, e.Member, typeArgs, e.TypeArgumentSubstitutionsWithParents(), expr.Type).EmitRead(wr);
        }

        wr.Write(postStr);
      } else {
        var typeArgs = CombineAllTypeArguments(e.Member, e.TypeApplication_AtEnclosingClass, e.TypeApplication_JustMember);
        var typeMap = e.TypeArgumentSubstitutionsWithParents();
        var customReceiver = NeedsCustomReceiver(e.Member) && !(e.Member.EnclosingClass is TraitDecl);
        if (!customReceiver && !e.Member.IsStatic) {
          Action<ConcreteSyntaxTree> obj;
          // The eta conversion here is to avoid capture of the receiver, because the call to EmitMemberSelect below may generate
          // a lambda expression in the target language.
          if (e.Member is Function && typeArgs.Count != 0) {
            // need to eta-expand wrap the receiver
            var etaReceiver = ProtectedFreshId("_eta_this");
            wr = CreateIIFE_ExprBody(etaReceiver, e.Obj.Type, e.Obj.tok, e.Obj, inLetExprBody, e.Type.Subst(typeMap), e.tok, wr, ref wStmts);
            obj = w => w.Write(etaReceiver);
          } else {
            obj = w => w.Append(TrExpr(e.Obj, inLetExprBody, wStmts));
          }
          EmitMemberSelect(obj, e.Obj.Type, e.Member, typeArgs, typeMap, expr.Type).EmitRead(wr);
        } else {
          string customReceiverName = null;
          if (customReceiver && e.Member is Function) {
            // need to eta-expand wrap the receiver
            customReceiverName = ProtectedFreshId("_eta_this");
            wr = CreateIIFE_ExprBody(customReceiverName, e.Obj.Type, e.Obj.tok, e.Obj, inLetExprBody, e.Type.Subst(typeMap), e.tok, wr, ref wStmts);
          }
          Action<ConcreteSyntaxTree> obj = w => w.Write(TypeName_Companion(e.Obj.Type, wr, e.tok, e.Member));
          EmitMemberSelect(obj, e.Obj.Type, e.Member, typeArgs, typeMap, expr.Type, customReceiverName).EmitRead(wr);
        }
      }
    } else if (expr is SeqSelectExpr) {
      SeqSelectExpr e = (SeqSelectExpr)expr;
      Contract.Assert(e.Seq.Type != null);
      if (e.Seq.Type.IsArrayType) {
        if (e.SelectOne) {
          Contract.Assert(e.E0 != null && e.E1 == null);
          var w = EmitArraySelect(new List<Expression>() { e.E0 }, e.Type, inLetExprBody, wr, wStmts);
          TrParenExpr(e.Seq, w, inLetExprBody, wStmts);
        } else {
          EmitSeqSelectRange(e.Seq, e.E0, e.E1, true, inLetExprBody, wr, wStmts);
        }
      } else if (e.SelectOne) {
        Contract.Assert(e.E0 != null && e.E1 == null);
        EmitIndexCollectionSelect(e.Seq, e.E0, inLetExprBody, wr, wStmts);
      } else {
        EmitSeqSelectRange(e.Seq, e.E0, e.E1, false, inLetExprBody, wr, wStmts);
      }
    } else if (expr is SeqConstructionExpr) {
      var e = (SeqConstructionExpr)expr;
      EmitSeqConstructionExpr(e, inLetExprBody, wr, wStmts);
    } else if (expr is MultiSetFormingExpr) {
      var e = (MultiSetFormingExpr)expr;
      EmitMultiSetFormingExpr(e, inLetExprBody, wr, wStmts);
    } else if (expr is MultiSelectExpr) {
      MultiSelectExpr e = (MultiSelectExpr)expr;
      WriteCast(TypeName(e.Type.NormalizeExpand(), wr, e.tok), wr);
      var w = EmitArraySelect(e.Indices, e.Type, inLetExprBody, wr, wStmts);
      TrParenExpr(e.Array, w, inLetExprBody, wStmts);

    } else if (expr is SeqUpdateExpr) {
      SeqUpdateExpr e = (SeqUpdateExpr)expr;
      var collectionType = e.Type.AsCollectionType;
      Contract.Assert(collectionType != null);
      EmitIndexCollectionUpdate(e.Seq, e.Index, e.Value, collectionType, inLetExprBody, wr, wStmts);
    } else if (expr is DatatypeUpdateExpr) {
      var e = (DatatypeUpdateExpr)expr;
      if (e.Members.All(member => member.IsGhost)) {
        // all fields to be updated are ghost, which doesn't change the value
        wr.Append(TrExpr(e.Root, inLetExprBody, wStmts));
        return wr;
      }
      if (DatatypeWrapperEraser.IsErasableDatatypeWrapper(e.Root.Type.AsDatatype, out var dtor)) {
        var i = e.Members.IndexOf(dtor);
        if (0 <= i) {
          // the datatype is an erasable wrapper and its core destructor is part of the update (which implies everything else must be a ghost),
          // so proceed as with the rhs
          Contract.Assert(Enumerable.Range(0, e.Members.Count).All(j => j == i || e.Members[j].IsGhost));
          Contract.Assert(e.Members.Count == e.Updates.Count);
          var rhs = e.Updates[i].Item3;
          wr.Append(TrExpr(rhs, inLetExprBody, wStmts));
          return wr;
        }
      }
      // the optimized cases don't apply, so proceed according to the desugaring
      wr.Append(TrExpr(e.ResolvedExpression, inLetExprBody, wStmts));
    } else if (expr is FunctionCallExpr) {
      FunctionCallExpr e = (FunctionCallExpr)expr;
      if (e.Function is SpecialFunction) {
        CompileSpecialFunctionCallExpr(e, wr, inLetExprBody, wStmts, TrExpr);
      } else {
        CompileFunctionCallExpr(e, wr, inLetExprBody, wStmts, TrExpr);
      }

    } else if (expr is ApplyExpr) {
      var e = expr as ApplyExpr;
      EmitApplyExpr(e.Function.Type, e.tok, e.Function, e.Args, inLetExprBody, wr, wStmts);

    } else if (expr is DatatypeValue) {
      var dtv = (DatatypeValue)expr;
      Contract.Assert(dtv.Ctor != null);  // since dtv has been successfully resolved

      if (DatatypeWrapperEraser.IsErasableDatatypeWrapper(dtv.Ctor.EnclosingDatatype, out var dtor)) {
        var i = dtv.Ctor.Destructors.IndexOf(dtor);
        Contract.Assert(0 <= i);
        wr.Append(TrExpr(dtv.Arguments[i], inLetExprBody, wStmts));
        return wr;
      }

      var wrArgumentList = new ConcreteSyntaxTree();
      string sep = "";
      for (int i = 0; i < dtv.Arguments.Count; i++) {
        var formal = dtv.Ctor.Formals[i];
        if (!formal.IsGhost) {
          wrArgumentList.Write(sep);
          var w = EmitCoercionIfNecessary(from: dtv.Arguments[i].Type, to: dtv.Ctor.Formals[i].Type, tok: dtv.tok, wr: wrArgumentList);
          w.Append(TrExpr(dtv.Arguments[i], inLetExprBody, wStmts));
          sep = ", ";
        }
      }
      EmitDatatypeValue(dtv, wrArgumentList.ToString(), wr);

    } else if (expr is OldExpr) {
      Contract.Assert(false); throw new cce.UnreachableException();  // 'old' is always a ghost

    } else if (expr is UnaryOpExpr) {
      var e = (UnaryOpExpr)expr;
      if (e.ResolvedOp == UnaryOpExpr.ResolvedOpcode.BVNot) {
        wr = EmitBitvectorTruncation(e.Type.AsBitVectorType, false, wr);
      }
      EmitUnaryExpr(UnaryOpCodeMap[e.ResolvedOp], e.E, inLetExprBody, wr, wStmts);
    } else if (expr is ConversionExpr) {
      var e = (ConversionExpr)expr;
      Contract.Assert(e.ToType.IsRefType == e.E.Type.IsRefType);
      if (e.ToType.IsRefType) {
        var w = EmitCoercionIfNecessary(e.E.Type, e.ToType, e.tok, wr);
        w = EmitDowncastIfNecessary(e.E.Type, e.ToType, e.tok, w);
        w.Append(TrExpr(e.E, inLetExprBody, wStmts));
      } else {
        EmitConversionExpr(e, inLetExprBody, wr, wStmts);
      }

    } else if (expr is TypeTestExpr) {
      var e = (TypeTestExpr)expr;
      var fromType = e.E.Type;
      if (fromType.IsSubtypeOf(e.ToType, false, false)) {
        wr.Append(TrExpr(Expression.CreateBoolLiteral(e.tok, true), inLetExprBody, wStmts));
      } else {
        var name = $"_is_{GetUniqueAstNumber(e)}";
        wr = CreateIIFE_ExprBody(name, fromType, e.tok, e.E, inLetExprBody, Type.Bool, e.tok, wr, ref wStmts);
        EmitTypeTest(name, e.E.Type, e.ToType, e.tok, wr);
      }

    } else if (expr is BinaryExpr) {
      var e = (BinaryExpr)expr;

      if (IsComparisonToZero(e, out var arg, out var sign, out var negated) &&
          CompareZeroUsingSign(arg.Type)) {
        // Transform e.g. x < BigInteger.Zero into x.Sign == -1
        var w = EmitSign(arg.Type, wr);
        TrParenExpr(arg, w, inLetExprBody, wStmts);
        wr.Write(negated ? " != " : " == ");
        wr.Write(sign.ToString());
      } else {
        string opString, preOpString, postOpString, callString, staticCallString;
        bool reverseArguments, truncateResult, convertE1_to_int;
        CompileBinOp(e.ResolvedOp, e.E0, e.E1, e.tok, expr.Type,
          out opString,
          out preOpString,
          out postOpString,
          out callString,
          out staticCallString,
          out reverseArguments,
          out truncateResult,
          out convertE1_to_int,
          wr);

        if (truncateResult && e.Type.IsBitVectorType) {
          wr = EmitBitvectorTruncation(e.Type.AsBitVectorType, true, wr);
        }
        var e0 = reverseArguments ? e.E1 : e.E0;
        var e1 = reverseArguments ? e.E0 : e.E1;
        if (opString != null) {
          var nativeType = AsNativeType(e.Type);
          string nativeName = null, literalSuffix = null;
          bool needsCast = false;
          if (nativeType != null) {
            GetNativeInfo(nativeType.Sel, out nativeName, out literalSuffix, out needsCast);
          }

          var inner = wr;
          if (needsCast) {
            inner = wr.Write("(" + nativeName + ")").ForkInParens();
          }
          inner.Write(preOpString);
          TrParenExpr(e0, inner, inLetExprBody, wStmts);
          inner.Write(" {0} ", opString);
          if (convertE1_to_int) {
            EmitExprAsInt(e1, inLetExprBody, inner, wStmts);
          } else {
            TrParenExpr(e1, inner, inLetExprBody, wStmts);
          }
          wr.Write(postOpString);
        } else if (callString != null) {
          wr.Write(preOpString);
          TrParenExpr(e0, wr, inLetExprBody, wStmts);
          wr.Write(".{0}(", callString);
          if (convertE1_to_int) {
            EmitExprAsInt(e1, inLetExprBody, wr, wStmts);
          } else {
            TrParenExpr(e1, wr, inLetExprBody, wStmts);
          }
          wr.Write(")");
          wr.Write(postOpString);
        } else if (staticCallString != null) {
          wr.Write(preOpString);
          wr.Write("{0}(", staticCallString);
          wr.Append(TrExpr(e0, inLetExprBody, wStmts));
          wr.Write(", ");
          wr.Append(TrExpr(e1, inLetExprBody, wStmts));
          wr.Write(")");
          wr.Write(postOpString);
        }
      }
    } else if (expr is TernaryExpr) {
      Contract.Assume(false);  // currently, none of the ternary expressions is compilable

    } else if (expr is LetExpr) {
      var e = (LetExpr)expr;
      if (e.Exact) {
        // The Dafny "let" expression
        //    var Pattern(x,y) := G; E
        // is translated into C# as:
        //    LamLet(G, tmp =>
        //      LamLet(dtorX(tmp), x =>
        //      LamLet(dtorY(tmp), y => E)))
        Contract.Assert(e.LHSs.Count == e.RHSs.Count);  // checked by resolution
        var w = wr;
        for (int i = 0; i < e.LHSs.Count; i++) {
          var lhs = e.LHSs[i];
          if (Contract.Exists(lhs.Vars, bv => !bv.IsGhost)) {
            var rhsName = $"_pat_let{GetUniqueAstNumber(e)}_{i}";
            w = CreateIIFE_ExprBody(rhsName, e.RHSs[i].Type, e.RHSs[i].tok, e.RHSs[i], inLetExprBody, e.Body.Type, e.Body.tok, w, ref wStmts);
            w = TrCasePattern(lhs, rhsName, e.RHSs[i].Type, e.Body.Type, w, ref wStmts);
          }
        }
        w.Append(TrExpr(e.Body, true, wStmts));
      } else if (e.BoundVars.All(bv => bv.IsGhost)) {
        // The Dafny "let" expression
        //    ghost var x,y :| Constraint; E
        // is compiled just like E is, because the resolver has already checked that x,y (or other ghost variables, for that matter) don't
        // occur in E (moreover, the verifier has checked that values for x,y satisfying Constraint exist).
        wr.Append(TrExpr(e.Body, inLetExprBody, wStmts));
      } else {
        // The Dafny "let" expression
        //    var x,y :| Constraint; E
        // is translated into C# as:
        //    LamLet(0, dummy => {  // the only purpose of this construction here is to allow us to add some code inside an expression in C#
        //        var x,y;
        //        // Embark on computation that fills in x,y according to Constraint; the computation stops when the first
        //        // such value is found, but since the verifier checks that x,y follows uniquely from Constraint, this is
        //        // not a source of nondeterminancy.
        //        return E;
        //      })
        Contract.Assert(e.RHSs.Count == 1);  // checked by resolution
        var missingBounds = ComprehensionExpr.BoolBoundedPool.MissingBounds(e.BoundVars.ToList<BoundVar>(), e.Constraint_Bounds, ComprehensionExpr.BoundedPool.PoolVirtues.Enumerable);
        if (missingBounds.Count != 0) {
          foreach (var bv in missingBounds) {
            Error(e.tok, "this let-such-that expression is too advanced for the current compiler; Dafny's heuristics cannot find any bound for variable '{0}'", wr, bv.Name);
          }
        } else {
          var w = CreateIIFE1(0, e.Body.Type, e.Body.tok, "_let_dummy_" + GetUniqueAstNumber(e), wr, wStmts);
          foreach (var bv in e.BoundVars) {
            DeclareLocalVar(IdName(bv), bv.Type, bv.tok, false, ForcePlaceboValue(bv.Type, wr, bv.tok, true), w);
          }
          TrAssignSuchThat(new List<IVariable>(e.BoundVars).ConvertAll(bv => (IVariable)bv), e.RHSs[0], e.Constraint_Bounds, e.tok.line, w, inLetExprBody);
          EmitReturnExpr(e.Body, e.Body.Type, true, w);
        }
      }
    } else if (expr is NestedMatchExpr nestedMatchExpr) {
      wr.Append(TrExpr(nestedMatchExpr.Flattened, inLetExprBody, wStmts));
    } else if (expr is MatchExpr) {
      var e = (MatchExpr)expr;
      // ((System.Func<SourceType, TargetType>)((SourceType _source) => {
      //   if (source.is_Ctor0) {
      //     FormalType f0 = ((Dt_Ctor0)source._D).a0;
      //     ...
      //     return Body0;
      //   } else if (...) {
      //     ...
      //   } else if (true) {
      //     ...
      //   }
      // }))(src)

      string source = ProtectedFreshId("_source");
      ConcreteSyntaxTree w;
      w = CreateLambda(new List<Type>() { e.Source.Type }, e.tok, new List<string>() { source }, e.Type, wr, wStmts);

      if (e.Cases.Count == 0) {
        // the verifier would have proved we never get here; still, we need some code that will compile
        EmitAbsurd(null, w);
      } else {
        int i = 0;
        var sourceType = (UserDefinedType)e.Source.Type.NormalizeExpand();
        foreach (MatchCaseExpr mc in e.Cases) {
          var wCase = MatchCasePrelude(source, sourceType, mc.Ctor, mc.Arguments, i, e.Cases.Count, w);
          EmitReturnExpr(mc.Body, mc.Body.Type, inLetExprBody, wCase);
          i++;
        }
      }
      // We end with applying the source expression to the delegate we just built
      wr.Write(LambdaExecute);
      TrParenExpr(e.Source, wr, inLetExprBody, wStmts);

    } else if (expr is QuantifierExpr) {
      var e = (QuantifierExpr)expr;

      // Compilation does not check whether a quantifier was split.

      wr = CaptureFreeVariables(expr, true, out var su, inLetExprBody, wr, ref wStmts);
      var logicalBody = su.Substitute(e.LogicalBody(true));

      Contract.Assert(e.Bounds != null);  // for non-ghost quantifiers, the resolver would have insisted on finding bounds
      var n = e.BoundVars.Count;
      Contract.Assert(e.Bounds.Count == n);
      var wBody = wr;
      for (int i = 0; i < n; i++) {
        var bound = e.Bounds[i];
        var bv = e.BoundVars[i];

        var collectionElementType = CompileCollection(bound, bv, inLetExprBody, false, su, out var collection, wStmts, e.Bounds, e.BoundVars, i);
        wBody.Write("{0}(", GetQuantifierName(TypeName(collectionElementType, wBody, bv.tok)));
        wBody.Append(collection);
        wBody.Write(", {0}, ", expr is ForallExpr ? True : False);
        var native = AsNativeType(e.BoundVars[i].Type);
        var tmpVarName = ProtectedFreshId(e is ForallExpr ? "_forall_var_" : "_exists_var_");
        ConcreteSyntaxTree newWBody = CreateLambda(new List<Type> { collectionElementType }, e.tok, new List<string> { tmpVarName }, Type.Bool, wBody, wStmts, untyped: true);
        wStmts = newWBody.Fork();
        newWBody = MaybeInjectSubtypeConstraint(
          tmpVarName, collectionElementType, bv.Type,
          inLetExprBody, e.tok, newWBody, true, e is ForallExpr);
        EmitDowncastVariableAssignment(
          IdName(bv), bv.Type, tmpVarName, collectionElementType, true, e.tok, newWBody);
        newWBody = MaybeInjectSubsetConstraint(
          bv, bv.Type, collectionElementType, inLetExprBody, e.tok, newWBody, true, e is ForallExpr);
        wBody.Write(')');
        wBody = newWBody;
      }
      wBody.Append(TrExpr(logicalBody, inLetExprBody, wStmts));

    } else if (expr is SetComprehension) {
      var e = (SetComprehension)expr;
      // For "set i,j,k,l | R(i,j,k,l) :: Term(i,j,k,l)" where the term has type "G", emit something like:
      // ((System.Func<Set<G>>)(() => {
      //   var _coll = new List<G>();
      //   foreach (var tmp_l in sq.Elements) { L l = (L)tmp_l;
      //     foreach (var tmp_k in st.Elements) { K k = (K)tmp_k;
      //       for (BigInteger j = Lo; j < Hi; j++) {
      //         for (bool i in Helper.AllBooleans) {
      //           if (R(i,j,k,l)) {
      //             _coll.Add(Term(i,j,k,l));
      //           }
      //         }
      //       }
      //     }
      //   }
      //   return Dafny.Set<G>.FromCollection(_coll);
      // }))()
      wr = CaptureFreeVariables(e, true, out var su, inLetExprBody, wr, ref wStmts);
      e = (SetComprehension)su.Substitute(e);

      Contract.Assert(e.Bounds != null);  // the resolver would have insisted on finding bounds
      var collectionName = ProtectedFreshId("_coll");
      var bwr = CreateIIFE0(e.Type.AsSetType, e.tok, wr, wStmts);
      wr = bwr;
      EmitSetBuilder_New(wr, e, collectionName);
      var n = e.BoundVars.Count;
      Contract.Assert(e.Bounds.Count == n);
      for (var i = 0; i < n; i++) {
        var bound = e.Bounds[i];
        var bv = e.BoundVars[i];
        var tmpVar = ProtectedFreshId("_compr_");
        var wStmtsLoop = wr.Fork();
        var elementType = CompileCollection(bound, bv, inLetExprBody, true, null, out var collection, wStmtsLoop);
        wr = CreateGuardedForeachLoop(tmpVar, elementType, bv, true, inLetExprBody, e.tok, collection, wr);
      }
      ConcreteSyntaxTree guardWriter;
      var thn = EmitIf(out guardWriter, false, wr);
      guardWriter.Append(TrExpr(e.Range, inLetExprBody, wStmts));
      EmitSetBuilder_Add(e.Type.AsSetType, collectionName, e.Term, inLetExprBody, thn);
      var s = GetCollectionBuilder_Build(e.Type.AsSetType, e.tok, collectionName, wr);
      EmitReturnExpr(s, bwr);

    } else if (expr is MapComprehension) {
      var e = (MapComprehension)expr;
      // For "map i | R(i) :: Term(i)" where the term has type "V" and i has type "U", emit something like:
      // ((System.Func<Map<U, V>>)(() => {
      //   var _coll = new List<Pair<U,V>>();
      //   foreach (L l in sq.Elements) {
      //     foreach (K k in st.Elements) {
      //       for (BigInteger j = Lo; j < Hi; j++) {
      //         for (bool i in Helper.AllBooleans) {
      //           if (R(i,j,k,l)) {
      //             _coll.Add(new Pair(i, Term(i));
      //           }
      //         }
      //       }
      //     }
      //   }
      //   return Dafny.Map<U, V>.FromCollection(_coll);
      // }))()
      wr = CaptureFreeVariables(e, true, out var su, inLetExprBody, wr, ref wStmts);
      e = (MapComprehension)su.Substitute(e);

      Contract.Assert(e.Bounds != null);  // the resolver would have insisted on finding bounds
      var domtypeName = TypeName(e.Type.AsMapType.Domain, wr, e.tok);
      var rantypeName = TypeName(e.Type.AsMapType.Range, wr, e.tok);
      var collection_name = ProtectedFreshId("_coll");
      var bwr = CreateIIFE0(e.Type.AsMapType, e.tok, wr, wStmts);
      wr = bwr;
      EmitMapBuilder_New(wr, e, collection_name);
      var n = e.BoundVars.Count;
      Contract.Assert(e.Bounds.Count == n);
      for (var i = 0; i < n; i++) {
        var bound = e.Bounds[i];
        var bv = e.BoundVars[i];
        var tmpVar = ProtectedFreshId("_compr_");
        var wStmtsLoop = wr.Fork();
        var elementType = CompileCollection(bound, bv, inLetExprBody, true, null, out var collection, wStmtsLoop);
        wr = CreateGuardedForeachLoop(tmpVar, elementType, bv, true, false, bv.tok, collection, wr);
      }
      ConcreteSyntaxTree guardWriter;
      var thn = EmitIf(out guardWriter, false, wr);
      guardWriter.Append(TrExpr(e.Range, inLetExprBody, wStmts));
      var termLeftWriter = EmitMapBuilder_Add(e.Type.AsMapType, e.tok, collection_name, e.Term, inLetExprBody, thn);
      if (e.TermLeft == null) {
        Contract.Assert(e.BoundVars.Count == 1);
        termLeftWriter.Write(IdName(e.BoundVars[0]));
      } else {
        termLeftWriter.Append(TrExpr(e.TermLeft, inLetExprBody, wStmts));
      }

      var s = GetCollectionBuilder_Build(e.Type.AsMapType, e.tok, collection_name, wr);
      EmitReturnExpr(s, bwr);

    } else if (expr is LambdaExpr) {
      var e = (LambdaExpr)expr;

      wr = CaptureFreeVariables(e, false, out var su, inLetExprBody, wr, ref wStmts);
      wr = CreateLambda(e.BoundVars.ConvertAll(bv => bv.Type), Token.NoToken, e.BoundVars.ConvertAll(IdName), e.Body.Type, wr, wStmts);
      wStmts = wr.Fork();
      wr = EmitReturnExpr(wr);
      // May need an upcast or boxing conversion to coerce to the generic arrow result type
      wr = EmitCoercionIfNecessary(e.Body.Type, TypeForCoercion(e.Type.AsArrowType.Result), e.Body.tok, wr);
      wr.Append(TrExpr(su.Substitute(e.Body), inLetExprBody, wStmts));
    } else if (expr is StmtExpr) {
      var e = (StmtExpr)expr;
      wr.Append(TrExpr(e.E, inLetExprBody, wStmts));

    } else if (expr is ITEExpr) {
      var e = (ITEExpr)expr;
      EmitITE(e.Test, e.Thn, e.Els, e.Type, inLetExprBody, wr, wStmts);

    } else if (expr is ConcreteSyntaxExpression) {
      var e = (ConcreteSyntaxExpression)expr;
      return TrExpr(e.ResolvedExpression, inLetExprBody, wStmts);

    } else {
      Contract.Assert(false); throw new cce.UnreachableException();  // unexpected expression
    }

    return wr;
  }
  
    public static Plugin Plugin =
      new ConfiguredPlugin(InternalCompilersPluginConfiguration.Singleton);

    public static string DefaultNameMain = "Main";

    protected virtual string ModuleSeparator { get => "."; }
    protected virtual string ClassAccessor { get => "."; }

    protected Stack<ConcreteSyntaxTree> copyInstrWriters = new Stack<ConcreteSyntaxTree>(); // a buffer that stores copy instructions generated by letExpr that uses out param.
    protected TopLevelDeclWithMembers thisContext;  // non-null when type members are being translated
    protected Method enclosingMethod;  // non-null when a method body is being translated
    protected Function enclosingFunction;  // non-null when a function body is being translated

    protected internal readonly FreshIdGenerator idGenerator = new FreshIdGenerator();

    private protected string ProtectedFreshId(string prefix) => IdProtect(idGenerator.FreshId(prefix));
    private protected string ProtectedFreshNumericId(string prefix) => IdProtect(idGenerator.FreshNumericId(prefix));

    Dictionary<Expression, int> uniqueAstNumbers = new Dictionary<Expression, int>();

    protected int GetUniqueAstNumber(Expression expr) {
      Contract.Requires(expr != null);
      int n;
      if (!uniqueAstNumbers.TryGetValue(expr, out n)) {
        n = uniqueAstNumbers.Count;
        uniqueAstNumbers.Add(expr, n);
      }
      return n;
    }

    public CoverageInstrumenter Coverage;

    public ProcessStartInfo PrepareProcessStartInfo(string programName, IEnumerable<string> args = null) {
      var psi = new ProcessStartInfo(programName) {
        UseShellExecute = false,
        CreateNoWindow = false, // https://github.com/dotnet/runtime/issues/68259
      };
      foreach (var arg in args ?? Enumerable.Empty<string>()) {
        psi.ArgumentList.Add(arg);
      }
      return psi;
    }

    public int WaitForExit(Process process, TextWriter outputWriter, string errorMessage = null) {
      process.WaitForExit();
      if (process.ExitCode != 0 && errorMessage != null) {
        outputWriter.WriteLine("{0} Process exited with exit code {1}", errorMessage, process.ExitCode);
      }
      return process.ExitCode;
    }

    public Process StartProcess(ProcessStartInfo psi, TextWriter outputWriter) {
      string additionalInfo = "";

      try {
        if (Process.Start(psi) is { } process) {
          return process;
        }
      } catch (System.ComponentModel.Win32Exception e) {
        additionalInfo = $": {e.Message}";
      }

      outputWriter.WriteLine($"Error: Unable to start {psi.FileName}{additionalInfo}");
      return null;
    }

    public int RunProcess(ProcessStartInfo psi, TextWriter outputWriter, string errorMessage = null) {
      return StartProcess(psi, outputWriter) is { } process ?
         WaitForExit(process, outputWriter, errorMessage) : -1;
    }

    protected static void ReportError(ErrorReporter reporter, IToken tok, string msg, ConcreteSyntaxTree/*?*/ wr, params object[] args) {
      Contract.Requires(msg != null);
      Contract.Requires(args != null);

      reporter.Error(MessageSource.Compiler, tok, msg, args);
      wr?.WriteLine("/* {0} */", string.Format("Compilation error: " + msg, args));
    }

    public void Error(IToken tok, string msg, ConcreteSyntaxTree wr, params object[] args) {
      ReportError(Reporter, tok, msg, wr, args);
    }

    protected void UnsupportedFeatureError(IToken tok, Feature feature, string message = null, ConcreteSyntaxTree wr = null, params object[] args) {
      if (!UnsupportedFeatures.Contains(feature)) {
        throw new Exception($"'{feature}' is not an element of the {TargetId} compiler's UnsupportedFeatures set");
      }

      message ??= UnsupportedFeatureException.MessagePrefix + FeatureDescriptionAttribute.GetDescription(feature).Description;
      Error(tok, message, wr, args);
    }

    protected string IntSelect = ",int";
    protected string LambdaExecute = "";

    protected static bool UnicodeCharEnabled => DafnyOptions.O.Get(CommonOptionBag.UnicodeCharacters);

    protected static string CharMethodQualifier => UnicodeCharEnabled ? "Unicode" : "";

    protected virtual void EmitHeader(Program program, ConcreteSyntaxTree wr) { }
    protected virtual void EmitFooter(Program program, ConcreteSyntaxTree wr) { }
    protected virtual void EmitBuiltInDecls(BuiltIns builtIns, ConcreteSyntaxTree wr) { }

    public override void OnPostCompile() {
      base.OnPostCompile();
      Coverage.WriteLegendFile();
    }

    /// <summary>
    /// Creates a static Main method. The caller will fill the body of this static Main with a
    /// call to the instance Main method in the enclosing class.
    /// </summary>
    protected abstract ConcreteSyntaxTree CreateStaticMain(IClassWriter wr, string argsParameterName);
    protected abstract ConcreteSyntaxTree CreateModule(string moduleName, bool isDefault, bool isExtern, string/*?*/ libraryName, ConcreteSyntaxTree wr);
    protected abstract string GetHelperModuleName();
    protected interface IClassWriter {
      ConcreteSyntaxTree/*?*/ CreateMethod(Method m, List<TypeArgumentInstantiation> typeArgs, bool createBody, bool forBodyInheritance, bool lookasideBody);
      ConcreteSyntaxTree/*?*/ SynthesizeMethod(Method m, List<TypeArgumentInstantiation> typeArgs, bool createBody, bool forBodyInheritance, bool lookasideBody);
      ConcreteSyntaxTree/*?*/ CreateFunction(string name, List<TypeArgumentInstantiation> typeArgs, List<Formal> formals, Type resultType, IToken tok, bool isStatic, bool createBody,
        MemberDecl member, bool forBodyInheritance, bool lookasideBody);
      ConcreteSyntaxTree/*?*/ CreateGetter(string name, TopLevelDecl enclosingDecl, Type resultType, IToken tok, bool isStatic, bool isConst, bool createBody, MemberDecl/*?*/ member, bool forBodyInheritance);  // returns null iff !createBody
      ConcreteSyntaxTree/*?*/ CreateGetterSetter(string name, Type resultType, IToken tok, bool createBody, MemberDecl/*?*/ member, out ConcreteSyntaxTree setterWriter, bool forBodyInheritance);  // if createBody, then result and setterWriter are non-null, else both are null
      void DeclareField(string name, TopLevelDecl enclosingDecl, bool isStatic, bool isConst, Type type, IToken tok, string rhs, Field/*?*/ field);
      /// <summary>
      /// InitializeField is called for inherited fields. It is in lieu of calling DeclareField and is called only if
      /// ClassesRedeclareInheritedFields==false for the compiler.
      /// </summary>
      void InitializeField(Field field, Type instantiatedFieldType, TopLevelDeclWithMembers enclosingClass);
      ConcreteSyntaxTree/*?*/ ErrorWriter();
      void Finish();
    }
    protected virtual bool IncludeExternMembers { get => false; }
    protected virtual bool SupportsStaticsInGenericClasses => true;
    protected virtual bool TraitRepeatsInheritedDeclarations => false;
    protected IClassWriter CreateClass(string moduleName, string name, TopLevelDecl cls, ConcreteSyntaxTree wr) {
      return CreateClass(moduleName, name, false, null, cls.TypeArgs, cls, null, null, wr);
    }

    /// <summary>
    /// "tok" can be "null" if "superClasses" is.
    /// </summary>
    protected abstract IClassWriter CreateClass(string moduleName, string name, bool isExtern, string/*?*/ fullPrintName,
      List<TypeParameter> typeParameters, TopLevelDecl cls, List<Type>/*?*/ superClasses, IToken tok, ConcreteSyntaxTree wr);

    /// <summary>
    /// "tok" can be "null" if "superClasses" is.
    /// </summary>
    protected abstract IClassWriter CreateTrait(string name, bool isExtern, List<TypeParameter> typeParameters /*?*/,
      TopLevelDecl trait, List<Type> superClasses /*?*/, IToken tok, ConcreteSyntaxTree wr);
    protected virtual bool SupportsProperties => true;
    protected abstract ConcreteSyntaxTree CreateIterator(IteratorDecl iter, ConcreteSyntaxTree wr);
    /// <summary>
    /// Returns an IClassWriter that can be used to write additional members. If "dt" is already written
    /// in the DafnyRuntime.targetlanguage file, then returns "null".
    /// </summary>
    protected abstract IClassWriter/*?*/ DeclareDatatype(DatatypeDecl dt, ConcreteSyntaxTree wr);
    protected virtual bool DatatypeDeclarationAndMemberCompilationAreSeparate => true;
    /// <summary>
    /// Returns an IClassWriter that can be used to write additional members.
    /// </summary>
    protected abstract IClassWriter DeclareNewtype(NewtypeDecl nt, ConcreteSyntaxTree wr);
    protected abstract void DeclareSubsetType(SubsetTypeDecl sst, ConcreteSyntaxTree wr);
    protected string GetNativeTypeName(NativeType nt) {
      Contract.Requires(nt != null);
      string nativeName = null, literalSuffix = null;
      bool needsCastAfterArithmetic = false;
      GetNativeInfo(nt.Sel, out nativeName, out literalSuffix, out needsCastAfterArithmetic);
      return nativeName;
    }
    protected abstract void GetNativeInfo(NativeType.Selection sel, out string name, out string literalSuffix, out bool needsCastAfterArithmetic);

    protected List<T> SelectNonGhost<T>(TopLevelDecl cl, List<T> elements) {
      Contract.Requires(cl != null && elements != null);
      if (cl is TupleTypeDecl tupleDecl) {
        Contract.Assert(elements.Count == tupleDecl.Dims);
        return elements.Where((_, i) => !tupleDecl.ArgumentGhostness[i]).ToList();
      } else {
        return elements;
      }
    }

    protected virtual List<TypeParameter> UsedTypeParameters(DatatypeDecl dt) {
      Contract.Requires(dt != null);

      var idt = dt as IndDatatypeDecl;
      if (idt == null) {
        return dt.TypeArgs;
      } else {
        Contract.Assert(idt.TypeArgs.Count == idt.TypeParametersUsedInConstructionByGroundingCtor.Length);
        var tps = new List<TypeParameter>();
        for (int i = 0; i < idt.TypeArgs.Count; i++) {
          if (idt.TypeParametersUsedInConstructionByGroundingCtor[i]) {
            tps.Add(idt.TypeArgs[i]);
          }
        }
        return tps;
      }
    }

    protected List<TypeArgumentInstantiation> UsedTypeParameters(DatatypeDecl dt, List<Type> typeArgs) {
      Contract.Requires(dt != null);
      Contract.Requires(typeArgs != null);
      Contract.Requires(dt.TypeArgs.Count == typeArgs.Count);

      if (dt is not IndDatatypeDecl idt) {
        return TypeArgumentInstantiation.ListFromClass(dt, typeArgs);
      } else {
        Contract.Assert(typeArgs.Count == idt.TypeParametersUsedInConstructionByGroundingCtor.Length);
        var r = new List<TypeArgumentInstantiation>();
        for (int i = 0; i < typeArgs.Count; i++) {
          if (idt.TypeParametersUsedInConstructionByGroundingCtor[i]) {
            r.Add(new TypeArgumentInstantiation(dt.TypeArgs[i], typeArgs[i]));
          }
        }
        return r;
      }
    }

    protected bool NeedsTypeDescriptors(List<TypeArgumentInstantiation> typeArgs) {
      Contract.Requires(typeArgs != null);
      return typeArgs.Any(ta => NeedsTypeDescriptor(ta.Formal));
    }

    protected virtual bool NeedsTypeDescriptor(TypeParameter tp) {
      Contract.Requires(tp != null);
      return tp.Characteristics.HasCompiledValue;
    }

    protected abstract string TypeDescriptor(Type type, ConcreteSyntaxTree wr, IToken tok);

    protected void EmitTypeDescriptorsActuals(List<TypeArgumentInstantiation> typeArgs, IToken tok, ConcreteSyntaxTree wr, bool useAllTypeArgs = false) {
      var prefix = "";
      EmitTypeDescriptorsActuals(typeArgs, tok, wr, ref prefix, useAllTypeArgs);
    }

    protected void EmitTypeDescriptorsActuals(List<TypeArgumentInstantiation> typeArgs, IToken tok, ConcreteSyntaxTree wr, ref string prefix, bool useAllTypeArgs = false) {
      Contract.Requires(typeArgs != null);
      Contract.Requires(tok != null);
      Contract.Requires(wr != null);
      Contract.Requires(prefix != null);

      foreach (var ta in typeArgs) {
        if (useAllTypeArgs || NeedsTypeDescriptor(ta.Formal)) {
          wr.Write("{0}{1}", prefix, TypeDescriptor(ta.Actual, wr, tok));
          prefix = ", ";
        }
      }
    }

    /// <summary>
    /// EmitTailCallStructure evolves "wr" into a structure that can be used as the jump target
    /// for tail calls (see EmitJumpToTailCallStart).
    /// The precondition of the method is:
    ///     (member is Method m0 && m0.IsTailRecursive) || (member is Function f0 && f0.IsTailRecursive)
    /// </summary>
    protected abstract ConcreteSyntaxTree EmitTailCallStructure(MemberDecl member, ConcreteSyntaxTree wr);
    protected abstract void EmitJumpToTailCallStart(ConcreteSyntaxTree wr);

    internal abstract string TypeName(Type type, ConcreteSyntaxTree wr, IToken tok, MemberDecl/*?*/ member = null);
    // For cases where a type looks different when it's an argument, such as (*sigh*) Java primitives
    protected virtual string TypeArgumentName(Type type, ConcreteSyntaxTree wr, IToken tok) {
      return TypeName(type, wr, tok);
    }
    /// <summary>
    /// This method returns the target representation of one possible value of the type.
    /// Requires: usePlaceboValue || type.HasCompilableValue
    ///
    ///   usePlaceboValue - If "true", the default value produced is one that the target language accepts as a value
    ///                  of the type, but which may not correspond to a Dafny value. This option is used when it is known
    ///                  that the Dafny program will not use the value (for example, when a field is automatically initialized
    ///                  but the Dafny program will soon assign a new value).
    /// </summary>
    protected abstract string TypeInitializationValue(Type type, ConcreteSyntaxTree wr, IToken tok, bool usePlaceboValue, bool constructTypeParameterDefaultsFromTypeDescriptors);

    protected string TypeName_UDT(string fullCompileName, UserDefinedType udt, ConcreteSyntaxTree wr, IToken tok, bool omitTypeArguments = false) {
      Contract.Requires(fullCompileName != null);
      Contract.Requires(udt != null);
      Contract.Requires(wr != null);
      Contract.Requires(tok != null);
      Contract.Requires(udt.TypeArgs.Count == (udt.ResolvedClass == null ? 0 : udt.ResolvedClass.TypeArgs.Count));
      var cl = udt.ResolvedClass;
      var typeParams = SelectNonGhost(cl, cl.TypeArgs);
      var typeArgs = SelectNonGhost(cl, udt.TypeArgs);
      return TypeName_UDT(fullCompileName, typeParams.ConvertAll(tp => tp.Variance), typeArgs, wr, tok, omitTypeArguments);
    }
    protected abstract string TypeName_UDT(string fullCompileName, List<TypeParameter.TPVariance> variance, List<Type> typeArgs,
      ConcreteSyntaxTree wr, IToken tok, bool omitTypeArguments);
    protected abstract string/*?*/ TypeName_Companion(Type type, ConcreteSyntaxTree wr, IToken tok, MemberDecl/*?*/ member);
    protected string TypeName_Companion(TopLevelDecl cls, ConcreteSyntaxTree wr, IToken tok) {
      Contract.Requires(cls != null);
      Contract.Requires(wr != null);
      Contract.Requires(tok != null);
      return TypeName_Companion(UserDefinedType.FromTopLevelDecl(tok, cls), wr, tok, null);
    }
    /// Return the "native form" of a type, to which EmitCoercionToNativeForm coerces it.
    protected virtual Type NativeForm(Type type) {
      return type;
    }

    protected abstract bool DeclareFormal(string prefix, string name, Type type, IToken tok, bool isInParam, ConcreteSyntaxTree wr);
    /// <summary>
    /// If "leaveRoomForRhs" is false and "rhs" is null, then generates:
    ///     type name;
    /// If "leaveRoomForRhs" is false and "rhs" is non-null, then generates:
    ///     type name = rhs;
    /// If "leaveRoomForRhs" is true, in which case "rhs" must be null, then generates:
    ///     type name
    /// which is intended to be followed up by a call to EmitAssignmentRhs.
    /// In the above, if "type" is null, then it is replaced by "var" or "let".
    /// "tok" is allowed to be null if "type" is.
    /// </summary>
    protected abstract void DeclareLocalVar(string name, Type/*?*/ type, IToken /*?*/ tok, bool leaveRoomForRhs, string/*?*/ rhs, ConcreteSyntaxTree wr);

    protected virtual void DeclareLocalVar(string name, Type /*?*/ type, IToken /*?*/ tok, bool leaveRoomForRhs, string /*?*/ rhs, ConcreteSyntaxTree wr, Type t) {
      DeclareLocalVar(name, type, tok, leaveRoomForRhs, rhs, wr);
    }
    /// <summary>
    /// Generates:
    ///     type name = rhs;
    /// In the above, if "type" is null, then it is replaced by "var" or "let".
    /// "tok" is allowed to be null if "type" is.
    /// </summary>
    protected virtual void DeclareLocalVar(string name, Type/*?*/ type, IToken/*?*/ tok, Expression rhs, bool inLetExprBody, ConcreteSyntaxTree wr) {
      var wStmts = wr.Fork();
      var w = DeclareLocalVar(name, type, tok, wr);
      wr.Append(TrExpr(rhs, inLetExprBody, wStmts));
    }

    /// <summary>
    /// Generates
    ///     type name = <<writer returned>>;
    /// In the above, if "type" is null, then it is replaced by "var" or "let".
    /// "tok" is allowed to be null if "type" is.
    /// </summary>
    protected abstract ConcreteSyntaxTree DeclareLocalVar(string name, Type/*?*/ type, IToken/*?*/ tok, ConcreteSyntaxTree wr);
    protected virtual void DeclareOutCollector(string collectorVarName, ConcreteSyntaxTree wr) { }  // called only for return-style calls
    protected virtual void DeclareSpecificOutCollector(string collectorVarName, ConcreteSyntaxTree wr, List<Type> formalTypes, List<Type> lhsTypes) { DeclareOutCollector(collectorVarName, wr); } // for languages that don't allow "let" or "var" expressions
    protected virtual bool UseReturnStyleOuts(Method m, int nonGhostOutCount) => false;
    protected virtual ConcreteSyntaxTree EmitMethodReturns(Method m, ConcreteSyntaxTree wr) { return wr; } // for languages that need explicit return statements not provided by Dafny
    protected virtual bool SupportsMultipleReturns { get => false; }
    protected virtual bool SupportsAmbiguousTypeDecl { get => true; }
    protected virtual bool ClassesRedeclareInheritedFields => true;
    protected virtual void AddTupleToSet(int i) { }
    public int TargetTupleSize = 0;
    /// The punctuation that comes at the end of a statement.  Note that
    /// statements are followed by newlines regardless.
    protected virtual string StmtTerminator { get => ";"; }
    protected virtual string True { get => "true"; }
    protected virtual string False { get => "false"; }
    protected virtual string Conj { get => "&&"; }
    public void EndStmt(ConcreteSyntaxTree wr) { wr.WriteLine(StmtTerminator); }
    protected abstract void DeclareLocalOutVar(string name, Type type, IToken tok, string rhs, bool useReturnStyleOuts, ConcreteSyntaxTree wr);
    protected virtual void EmitActualOutArg(string actualOutParamName, ConcreteSyntaxTree wr) { }  // actualOutParamName is always the name of a local variable; called only for non-return-style outs
    protected virtual void EmitOutParameterSplits(string outCollector, List<string> actualOutParamNames, ConcreteSyntaxTree wr) { }  // called only for return-style calls
    protected virtual void EmitCastOutParameterSplits(string outCollector, List<string> actualOutParamNames, ConcreteSyntaxTree wr, List<Type> formalOutParamTypes, List<Type> lhsTypes, IToken tok) {
      EmitOutParameterSplits(outCollector, actualOutParamNames, wr);
    }

    protected abstract void EmitActualTypeArgs(List<Type> typeArgs, IToken tok, ConcreteSyntaxTree wr);

    protected virtual void EmitNameAndActualTypeArgs(string protectedName, List<Type> typeArgs, IToken tok, ConcreteSyntaxTree wr) {
      wr.Write(protectedName);
      EmitActualTypeArgs(typeArgs, tok, wr);
    }
    protected abstract string GenerateLhsDecl(string target, Type/*?*/ type, ConcreteSyntaxTree wr, IToken tok);

    protected virtual ConcreteSyntaxTree EmitAssignment(ILvalue wLhs, Type lhsType /*?*/, Type rhsType /*?*/,
      ConcreteSyntaxTree wr, IToken tok) {
      var w = wLhs.EmitWrite(wr);

      w = EmitCoercionIfNecessary(rhsType, lhsType, tok, w);
      w = EmitDowncastIfNecessary(rhsType, lhsType, tok, w);
      return w;
    }

    protected virtual void EmitAssignment(out ConcreteSyntaxTree wLhs, Type/*?*/ lhsType, out ConcreteSyntaxTree wRhs, Type/*?*/ rhsType, ConcreteSyntaxTree wr) {
      wLhs = wr.Fork();
      wr.Write(" = ");
      var w = wr;
      w = EmitCoercionIfNecessary(rhsType, lhsType, Token.NoToken, w);
      w = EmitDowncastIfNecessary(rhsType, lhsType, Token.NoToken, w);
      wRhs = w.Fork();
      EndStmt(wr);
    }
    protected void EmitAssignment(string lhs, Type/*?*/ lhsType, string rhs, Type/*?*/ rhsType, ConcreteSyntaxTree wr) {
      EmitAssignment(out var wLhs, lhsType, out var wRhs, rhsType, wr);
      wLhs.Write(lhs);
      wRhs.Write(rhs);
    }
    protected void EmitAssignmentRhs(string rhs, ConcreteSyntaxTree wr) {
      var w = EmitAssignmentRhs(wr);
      w.Write(rhs);
    }
    protected void EmitAssignmentRhs(Expression rhs, bool inLetExprBody, ConcreteSyntaxTree wr) {
      var wStmts = wr.Fork();
      var w = EmitAssignmentRhs(wr);
      w.Append(TrExpr(rhs, inLetExprBody, wStmts));
    }

    protected virtual ConcreteSyntaxTree EmitAssignmentRhs(ConcreteSyntaxTree wr) {
      wr.Write(" = ");
      var w = wr.Fork();
      EndStmt(wr);
      return w;
    }

    protected virtual string EmitAssignmentLhs(Expression e, ConcreteSyntaxTree wr) {
      var wStmts = wr.Fork();
      var target = ProtectedFreshId("_lhs");
      wr.Write(GenerateLhsDecl(target, e.Type, wr, e.tok));
      wr.Write(" = ");
      wr.Append(TrExpr(e, false, wStmts));
      EndStmt(wr);
      return target;
    }

    protected virtual void EmitMultiAssignment(List<Expression> lhsExprs, List<ILvalue> lhss, List<Type> lhsTypes, out List<ConcreteSyntaxTree> wRhss,
      List<Type> rhsTypes, ConcreteSyntaxTree wr) {
      Contract.Assert(lhss.Count == lhsTypes.Count);
      Contract.Assert(lhsTypes.Count == rhsTypes.Count);

      wRhss = new List<ConcreteSyntaxTree>();
      var rhsVars = new List<string>();
      foreach (var rhsType in rhsTypes) {
        string target = ProtectedFreshId("_rhs");
        rhsVars.Add(target);
        wr.Write(GenerateLhsDecl(target, rhsType, wr, Token.NoToken));
        wRhss.Add(EmitAssignmentRhs(wr));
      }

      List<ILvalue> lhssn;
      if (lhss.Count > 1) {
        lhssn = new List<ILvalue>();
        for (int i = 0; i < lhss.Count; ++i) {
          Expression lexpr = lhsExprs[i].Resolved;
          ILvalue lhs = lhss[i];
          if (lexpr is IdentifierExpr) {
            lhssn.Add(lhs);
          } else if (lexpr is MemberSelectExpr) {
            var resolved = (MemberSelectExpr)lexpr;
            string target = EmitAssignmentLhs(resolved.Obj, wr);
            var typeArgs = TypeArgumentInstantiation.ListFromMember(resolved.Member, null, resolved.TypeApplication_JustMember);
            ILvalue newLhs = EmitMemberSelect(w => w.Write(target), resolved.Obj.Type, resolved.Member, typeArgs, resolved.TypeArgumentSubstitutionsWithParents(), resolved.Type, internalAccess: enclosingMethod is Constructor);
            lhssn.Add(newLhs);
          } else if (lexpr is SeqSelectExpr) {
            var seqExpr = (SeqSelectExpr)lexpr;
            string targetArray = EmitAssignmentLhs(seqExpr.Seq, wr);
            string targetIndex = EmitAssignmentLhs(seqExpr.E0, wr);
            if (seqExpr.Seq.Type.IsArrayType || seqExpr.Seq.Type.AsSeqType != null) {
              targetIndex = ArrayIndexToNativeInt(targetIndex, seqExpr.E0.Type);
            }
            ILvalue newLhs = EmitArraySelectAsLvalue(targetArray,
              new List<string>() { targetIndex }, lhsTypes[i]);
            lhssn.Add(newLhs);
          } else if (lexpr is MultiSelectExpr) {
            var seqExpr = (MultiSelectExpr)lexpr;
            Expression array = seqExpr.Array;
            List<Expression> indices = seqExpr.Indices;
            string targetArray = EmitAssignmentLhs(array, wr);
            var targetIndices = new List<string>();
            foreach (var index in indices) {
              string targetIndex = EmitAssignmentLhs(index, wr);
              targetIndices.Add(targetIndex);
            }
            ILvalue newLhs = EmitArraySelectAsLvalue(targetArray, targetIndices, lhsTypes[i]);
            lhssn.Add(newLhs);
          } else {
            Contract.Assert(false); // Unknown kind of expression
            lhssn.Add(lhs);
          }
        }
      } else {
        lhssn = lhss;
      }

      Contract.Assert(rhsVars.Count == lhsTypes.Count);
      for (int i = 0; i < rhsVars.Count; i++) {
        ConcreteSyntaxTree wRhsVar = EmitAssignment(lhssn[i], lhsTypes[i], rhsTypes[i], wr, Token.NoToken);
        wRhsVar.Write(rhsVars[i]);
      }
    }

    protected virtual void EmitSetterParameter(ConcreteSyntaxTree wr) {
      wr.Write("value");
    }
    protected abstract void EmitPrintStmt(ConcreteSyntaxTree wr, Expression arg);
    protected abstract void EmitReturn(List<Formal> outParams, ConcreteSyntaxTree wr);
    protected virtual void EmitReturnExpr(Expression expr, Type resultType, bool inLetExprBody, ConcreteSyntaxTree wr) {  // emits "return <expr>;" for function bodies
      var wStmts = wr.Fork();
      var w = EmitReturnExpr(wr);
      w.Append(TrExpr(expr, inLetExprBody, wStmts));
    }
    protected virtual void EmitReturnExpr(string returnExpr, ConcreteSyntaxTree wr) {  // emits "return <returnExpr>;" for function bodies
      var w = EmitReturnExpr(wr);
      w.Write(returnExpr);
    }
    protected virtual ConcreteSyntaxTree EmitReturnExpr(ConcreteSyntaxTree wr) {
      // emits "return <returnExpr>;" for function bodies
      wr.Write("return ");
      var w = wr.Fork();
      EndStmt(wr);
      return w;
    }
    /// <summary>
    /// Labels the code written to the TargetWriter returned, in such that way that any
    /// emitted break to the label inside that code will abruptly end the execution of the code.
    /// </summary>
    protected abstract ConcreteSyntaxTree CreateLabeledCode(string label, bool createContinueLabel, ConcreteSyntaxTree wr);
    protected abstract void EmitBreak(string/*?*/ label, ConcreteSyntaxTree wr);
    protected abstract void EmitContinue(string label, ConcreteSyntaxTree wr);
    protected abstract void EmitYield(ConcreteSyntaxTree wr);
    protected abstract void EmitAbsurd(string/*?*/ message, ConcreteSyntaxTree wr);
    protected virtual void EmitAbsurd(string message, ConcreteSyntaxTree wr, bool needIterLimit) {
      EmitAbsurd(message, wr);
    }

    protected abstract void EmitHalt(IToken tok, Expression /*?*/ messageExpr, ConcreteSyntaxTree wr);

    protected ConcreteSyntaxTree EmitIf(string guard, bool hasElse, ConcreteSyntaxTree wr) {
      ConcreteSyntaxTree guardWriter;
      var thn = EmitIf(out guardWriter, hasElse, wr);
      guardWriter.Write(guard);
      return thn;
    }
    protected virtual ConcreteSyntaxTree EmitIf(out ConcreteSyntaxTree guardWriter, bool hasElse, ConcreteSyntaxTree wr) {
      wr.Write("if (");
      guardWriter = wr.Fork();
      if (hasElse) {
        var thn = wr.NewBlock(")", " else", BlockStyle.SpaceBrace, BlockStyle.SpaceBrace);
        return thn;
      } else {
        var thn = wr.NewBlock(")");
        return thn;
      }
    }

    protected virtual ConcreteSyntaxTree EmitBlock(ConcreteSyntaxTree wr) {
      return wr.NewBlock("", open: BlockStyle.Brace);
    }

    protected virtual ConcreteSyntaxTree EmitWhile(IToken tok, List<Statement> body, LList<Label> labels, ConcreteSyntaxTree wr) {  // returns the guard writer
      ConcreteSyntaxTree guardWriter;
      var wBody = CreateWhileLoop(out guardWriter, wr);
      wBody = EmitContinueLabel(labels, wBody);
      Coverage.Instrument(tok, "while body", wBody);
      TrStmtList(body, wBody);
      return guardWriter;
    }

    protected abstract ConcreteSyntaxTree EmitForStmt(IToken tok, IVariable loopIndex, bool goingUp, string /*?*/ endVarName,
      List<Statement> body, LList<Label> labels, ConcreteSyntaxTree wr);

    protected virtual ConcreteSyntaxTree CreateWhileLoop(out ConcreteSyntaxTree guardWriter, ConcreteSyntaxTree wr) {
      wr.Write("while (");
      guardWriter = wr.Fork();
      var wBody = wr.NewBlock(")");
      return wBody;
    }
    protected abstract ConcreteSyntaxTree CreateForLoop(string indexVar, string bound, ConcreteSyntaxTree wr);
    protected abstract ConcreteSyntaxTree CreateDoublingForLoop(string indexVar, int start, ConcreteSyntaxTree wr);
    protected abstract void EmitIncrementVar(string varName, ConcreteSyntaxTree wr);  // increments a BigInteger by 1
    protected abstract void EmitDecrementVar(string varName, ConcreteSyntaxTree wr);  // decrements a BigInteger by 1

    protected abstract string GetQuantifierName(string bvType);

    /// <summary>
    /// Emit a loop like this:
    ///     foreach (tmpVarName:collectionElementType in [[collectionWriter]]) {
    ///       [[bodyWriter]]
    ///     }
    /// where
    ///   * "[[collectionWriter]]" is the writer returned as "collectionWriter"
    ///   * "[[bodyWriter]]" is the block writer returned
    /// </summary>
    protected abstract ConcreteSyntaxTree CreateForeachLoop(
      string tmpVarName, Type collectionElementType,
      IToken tok, out ConcreteSyntaxTree collectionWriter, ConcreteSyntaxTree wr);

    /// <summary>
    /// Creates a guarded foreach loop that iterates over a collection, and apply required subtype
    /// and compiled subset types filters. Will not emit intermediate ifs if there is no need.
    ///
    ///     foreach(collectionElementType tmpVarName in collectionWriter) {
    ///       if(tmpVarName is [boundVar.type]) {
    ///         var [IDName(boundVar)] = ([boundVar.type])(tmpvarName);
    ///         if(constraints_of_boundvar.Type([IDName(boundVar)])) {
    ///           ...
    ///         }
    ///       }
    ///     }
    /// </summary>
    /// <returns>A writer to write inside the deepest if-then</returns>
    protected ConcreteSyntaxTree CreateGuardedForeachLoop(
      string tmpVarName, Type collectionElementType,
      IVariable boundVar,
      bool introduceBoundVar, bool inLetExprBody,
      IToken tok, ConcreteSyntaxTree collection, ConcreteSyntaxTree wr
      ) {
      wr = CreateForeachLoop(tmpVarName, collectionElementType, tok, out var collectionWriter, wr);
      collectionWriter.Append(collection);
      wr = MaybeInjectSubtypeConstraint(tmpVarName, collectionElementType, boundVar.Type, inLetExprBody, tok, wr);
      EmitDowncastVariableAssignment(IdName(boundVar), boundVar.Type, tmpVarName, collectionElementType,
          introduceBoundVar, tok, wr);
      wr = MaybeInjectSubsetConstraint(boundVar, boundVar.Type, collectionElementType, inLetExprBody, tok, wr);
      return wr;
    }

    /// <summary>
    /// Returns a subtype condition like:
    ///     tmpVarName is member of type boundVarType
    /// Returns null if no condition is necessary
    /// </summary>
    [CanBeNull]
    protected abstract string GetSubtypeCondition(
      string tmpVarName, Type boundVarType, IToken tok, ConcreteSyntaxTree wPreconditions);

    /// <summary>
    /// Emit an (already verified) downcast assignment like:
    /// 
    ///     var boundVarName:boundVarType := tmpVarName as boundVarType;
    ///     [[bodyWriter]]
    /// 
    /// where
    ///   * "[[bodyWriter]]" is where the writer wr's position will be next
    /// </summary>
    /// <param name="boundVarName">Name of the variable after casting</param>
    /// <param name="boundVarType">Expected variable type</param>
    /// <param name="tmpVarName">The collection's variable name</param>
    /// <param name="collectionElementType">type this variable is casted from, in case it is useful</param>
    /// <param name="introduceBoundVar">Whether or not to declare the variable, in languages requiring declarations</param>
    /// <param name="tok">A position in the AST</param>
    /// <param name="wr">The concrete syntax tree writer</param>
    protected abstract void EmitDowncastVariableAssignment(string boundVarName, Type boundVarType, string tmpVarName,
      Type collectionElementType, bool introduceBoundVar, IToken tok, ConcreteSyntaxTree wr);

    /// <summary>
    /// Emit a simple foreach loop over the elements (which are known as "ingredients") of a collection assembled for
    /// the purpose of compiling a "forall" statement.
    ///
    ///     foreach (boundVarName:boundVarType in [[coll]]) {
    ///       [[body]]
    ///     }
    ///
    /// where "boundVarType" is an L-tuple whose components are "tupleTypeArgs" (see EmitIngredients). If "boundVarType" can
    /// be inferred from the ingredients emitted by EmitIngredients, then "L" and "tupleTypeArgs" can be ignored and
    /// "boundVarType" be replaced by some target-language way of saying "please infer the type" (like "var" in C#).
    /// </summary>
    protected abstract ConcreteSyntaxTree CreateForeachIngredientLoop(string boundVarName, int L, string tupleTypeArgs, out ConcreteSyntaxTree collectionWriter, ConcreteSyntaxTree wr);

    /// <summary>
    /// If "initCall" is non-null, then "initCall.Method is Constructor".
    /// </summary>
    protected abstract void EmitNew(Type type, IToken tok, CallStmt initCall /*?*/, ConcreteSyntaxTree wr,
      ConcreteSyntaxTree wStmts);

    // To support target language constructors without an additional initCall in {:extern} code, we ignore the initCall
    // and call the constructor with all arguments.
    protected string ConstructorArguments(CallStmt initCall, ConcreteSyntaxTree wStmts, Constructor ctor, string sep = "") {
      var arguments = Enumerable.Empty<string>();
      if (ctor != null && ctor.IsExtern(out _, out _)) {
        // the arguments of any external constructor are placed here
        arguments = ctor.Ins.Select((f, i) => (f, i))
          .Where(tp => !tp.f.IsGhost)
          .Select(tp => Expr(initCall.Args[tp.i], false, wStmts).ToString());
      }
      return (arguments.Any() ? sep : "") + arguments.Comma();
    }
    protected abstract void EmitNewArray(Type elmtType, IToken tok, List<Expression> dimensions,
      bool mustInitialize, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);
    protected abstract void EmitStringLiteral(string str, bool isVerbatim, ConcreteSyntaxTree wr);
    protected abstract ConcreteSyntaxTree EmitBitvectorTruncation(BitvectorType bvType, bool surroundByUnchecked, ConcreteSyntaxTree wr);

    protected abstract void EmitRotate(Expression e0, Expression e1, bool isRotateLeft, ConcreteSyntaxTree wr,
      bool inLetExprBody, ConcreteSyntaxTree wStmts, FCE_Arg_Translator tr);
    /// <summary>
    /// Return true if x < 0 should be rendered as sign(x) < 0 when x has the
    /// given type.  Typically, this is only a win at non-native types, since
    /// BigIntegers benefit from not having to access the number zero.
    /// </summary>
    protected virtual bool CompareZeroUsingSign(Type type) {
      return false;
    }
    protected virtual ConcreteSyntaxTree EmitSign(Type type, ConcreteSyntaxTree wr) {
      // Currently, this should only be called when CompareZeroUsingSign is true
      Contract.Assert(false);
      throw new cce.UnreachableException();
    }
    protected abstract void EmitEmptyTupleList(string tupleTypeArgs, ConcreteSyntaxTree wr);
    protected abstract ConcreteSyntaxTree EmitAddTupleToList(string ingredients, string tupleTypeArgs, ConcreteSyntaxTree wr);
    protected abstract void EmitTupleSelect(string prefix, int i, ConcreteSyntaxTree wr);

    protected virtual bool NeedsCastFromTypeParameter => false;

    protected virtual bool TargetSubtypingRequiresEqualTypeArguments(Type type) => false;

    protected virtual bool IsCoercionNecessary(Type /*?*/ from, Type /*?*/ to) {
      return NeedsCastFromTypeParameter;
    }

    protected virtual Type TypeForCoercion(Type type) {
      return type;
    }

    /// <summary>
    /// If "from" and "to" are both given, and if a "from" needs an explicit coercion in order to become a "to", emit that coercion.
    /// Needed in languages where either
    ///   (a) we need to represent upcasts as explicit operations (like Go, or array types in Java), or
    ///   (b) there's static typing but no parametric polymorphism (like Go) so that lots of things need to be boxed and unboxed.
    /// </summary>
    protected virtual ConcreteSyntaxTree EmitCoercionIfNecessary(Type/*?*/ from, Type/*?*/ to, IToken tok, ConcreteSyntaxTree wr) {
      return wr;
    }

    protected ConcreteSyntaxTree EmitDowncastIfNecessary(Type /*?*/ from, Type /*?*/ to, IToken tok, ConcreteSyntaxTree wr) {
      Contract.Requires(tok != null);
      Contract.Requires(wr != null);
      if (from != null && to != null) {
        from = DatatypeWrapperEraser.SimplifyType(from);
        to = DatatypeWrapperEraser.SimplifyType(to);
        if (!IsTargetSupertype(to, from)) {
          // By the way, it is tempting to think that IsTargetSupertype(from, to)) would hold here, but that's not true.
          // For one, in a language with NeedsCastFromTypeParameter, "to" and "from" may contain uninstantiated formal type parameters.
          // Also, it is possible (subject to a check enforced by the verifier) to assign Datatype<X> to Datatype<Y>,
          // where Datatype is co-variant in its argument type and X and Y are two incomparable types with a common supertype.

          wr = EmitDowncast(from, to, tok, wr);
        }
      }
      return wr;
    }

    /// <summary>
    /// Determine if "to" is a supertype of "from" in the target language, if "!typeEqualityOnly".
    /// Determine if "to" is equal to "from" in the target language, if "typeEqualityOnly".
    /// This to similar to Type.IsSupertype and Type.Equals, respectively, but ignores subset types (that
    /// is, always uses the base type of any subset type).
    /// </summary>
    public bool IsTargetSupertype(Type to, Type from, bool typeEqualityOnly = false) {
      Contract.Requires(from != null);
      Contract.Requires(to != null);
      to = to.NormalizeExpand();
      from = from.NormalizeExpand();
      if (Type.SameHead(to, from)) {
        Contract.Assert(to.TypeArgs.Count == from.TypeArgs.Count);
        var formalTypeParameters = (to as UserDefinedType)?.ResolvedClass?.TypeArgs;
        Contract.Assert(formalTypeParameters == null || formalTypeParameters.Count == to.TypeArgs.Count);
        Contract.Assert(formalTypeParameters != null || to.TypeArgs.Count == 0 || to is CollectionType);
        for (var i = 0; i < to.TypeArgs.Count; i++) {
          bool okay;
          if (typeEqualityOnly || TargetSubtypingRequiresEqualTypeArguments(to)) {
            okay = IsTargetSupertype(to.TypeArgs[i], from.TypeArgs[i], true);
          } else if (formalTypeParameters == null || formalTypeParameters[i].Variance == TypeParameter.TPVariance.Co) {
            okay = IsTargetSupertype(to.TypeArgs[i], from.TypeArgs[i]);
          } else if (formalTypeParameters[i].Variance == TypeParameter.TPVariance.Contra) {
            okay = IsTargetSupertype(from.TypeArgs[i], to.TypeArgs[i]);
          } else {
            okay = IsTargetSupertype(to.TypeArgs[i], from.TypeArgs[i], true);
          }
          if (!okay) {
            return false;
          }
        }
        return true;
      } else if (typeEqualityOnly) {
        return false;
      } else if (to.IsObjectQ) {
        return true;
      } else {
        return from.ParentTypes().Any(fromParentType => IsTargetSupertype(to, fromParentType));
      }
    }

    protected ConcreteSyntaxTree Downcast(Type from, Type to, IToken tok, ICanRender expression) {
      var result = new ConcreteSyntaxTree();
      EmitDowncast(from, to, tok, result).Append(expression);
      return result;
    }

    protected virtual ConcreteSyntaxTree EmitDowncast(Type from, Type to, IToken tok, ConcreteSyntaxTree wr) {
      Contract.Requires(from != null);
      Contract.Requires(to != null);
      Contract.Requires(tok != null);
      Contract.Requires(wr != null);
      Contract.Requires(!IsTargetSupertype(to, from));
      return wr;
    }

    protected virtual ConcreteSyntaxTree EmitCoercionToNativeForm(Type/*?*/ from, IToken tok, ConcreteSyntaxTree wr) {
      return wr;
    }
    protected virtual ConcreteSyntaxTree EmitCoercionFromNativeForm(Type/*?*/ to, IToken tok, ConcreteSyntaxTree wr) {
      return wr;
    }
    protected virtual ConcreteSyntaxTree EmitCoercionToNativeInt(ConcreteSyntaxTree wr) {
      return wr;
    }
    /// <summary>
    /// Emit a coercion of a value to any tuple, returning the writer for the value to coerce.  Needed in translating ForallStmt because some of the tuple components are native ints for which we have no Type object, but Go needs to coerce the value that comes out of the iterator.  Safe to leave this alone in subclasses that don't have the same problem.
    /// </summary>
    protected virtual ConcreteSyntaxTree EmitCoercionToArbitraryTuple(ConcreteSyntaxTree wr) {
      return wr;
    }
    protected virtual string IdName(TopLevelDecl d) {
      Contract.Requires(d != null);
      return IdProtect(d.CompileName);
    }
    protected virtual string IdName(MemberDecl member) {
      Contract.Requires(member != null);
      return IdProtect(member.CompileName);
    }
    protected virtual string IdName(TypeParameter tp) {
      Contract.Requires(tp != null);
      return IdProtect(tp.CompileName);
    }
    protected virtual string IdName(IVariable v) {
      Contract.Requires(v != null);
      return IdProtect(v.CompileName);
    }
    protected virtual string IdMemberName(MemberSelectExpr mse) {
      Contract.Requires(mse != null);
      return IdProtect(mse.MemberName);
    }
    protected virtual string IdProtect(string name) {
      Contract.Requires(name != null);
      return name;
    }
    protected abstract string FullTypeName(UserDefinedType udt, MemberDecl/*?*/ member = null);
    protected virtual void EmitNull(Type type, ConcreteSyntaxTree wr) {
      wr.Write("null");
    }
    protected virtual void EmitITE(Expression guard, Expression thn, Expression els, Type resultType, bool inLetExprBody,
        ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts) {
      Contract.Requires(guard != null);
      Contract.Requires(thn != null);
      Contract.Requires(thn.Type != null);
      Contract.Requires(els != null);
      Contract.Requires(resultType != null);
      Contract.Requires(wr != null);

      resultType = resultType.NormalizeExpand();
      var thenExpr = Expr(thn, inLetExprBody, wStmts);
      var castedThenExpr = resultType.Equals(thn.Type.NormalizeExpand()) ? thenExpr : Cast(resultType, thenExpr);
      var elseExpr = Expr(els, inLetExprBody, wStmts);
      var castedElseExpr = resultType.Equals(els.Type.NormalizeExpand()) ? elseExpr : Cast(resultType, elseExpr);
      wr.Format($"(({Expr(guard, inLetExprBody, wStmts)}) ? ({castedThenExpr}) : ({castedElseExpr}))");
    }

    public ConcreteSyntaxTree Cast(Type toType, ConcreteSyntaxTree expr) {
      var result = new ConcreteSyntaxTree();
      EmitCast(toType, result).Append(expr);
      return result;
    }

    protected virtual ConcreteSyntaxTree EmitCast(Type toType, ConcreteSyntaxTree wr) {
      wr.Write("({0})", TypeName(toType, wr, Token.NoToken));
      return wr.ForkInParens();
    }
    protected abstract void EmitDatatypeValue(DatatypeValue dtv, string arguments, ConcreteSyntaxTree wr);
    protected abstract void GetSpecialFieldInfo(SpecialField.ID id, object idParam, Type receiverType, out string compiledName, out string preString, out string postString);

    /// <summary>
    /// A "TypeArgumentInstantiation" is essentially a pair consisting of a formal type parameter and an actual type for that parameter.
    /// </summary>
    public class TypeArgumentInstantiation {
      public readonly TypeParameter Formal;
      public readonly Type Actual;

      public TypeArgumentInstantiation(TypeParameter formal, Type actual) {
        Contract.Requires(formal != null);
        Contract.Requires(actual != null);
        Formal = formal;
        Actual = actual;
      }

      /// <summary>
      /// Uses "formal" for both formal and actual.
      /// </summary>
      public TypeArgumentInstantiation(TypeParameter formal) {
        Contract.Requires(formal != null);
        Formal = formal;
        Actual = new UserDefinedType(formal);
      }

      public static List<TypeArgumentInstantiation> ListFromMember(MemberDecl member, List<Type> /*?*/ classActuals, List<Type> /*?*/ memberActuals) {
        Contract.Requires(classActuals == null || classActuals.Count == (member.EnclosingClass == null ? 0 : member.EnclosingClass.TypeArgs.Count));
        Contract.Requires(memberActuals == null || memberActuals.Count == (member is ICallable ic ? ic.TypeArgs.Count : 0));

        var r = new List<TypeArgumentInstantiation>();
        void add(List<TypeParameter> formals, List<Type> actuals) {
          Contract.Assert(formals.Count == actuals.Count);
          for (var i = 0; i < formals.Count; i++) {
            r.Add(new TypeArgumentInstantiation(formals[i], actuals[i]));
          }
        };

        if (classActuals != null && classActuals.Count != 0) {
          Contract.Assert(member.EnclosingClass.TypeArgs.TrueForAll(ta => ta.Parent is TopLevelDecl));
          add(member.EnclosingClass.TypeArgs, classActuals);
        }
        if (memberActuals != null && member is ICallable icallable) {
          Contract.Assert(icallable.TypeArgs.TrueForAll(ta => !(ta.Parent is TopLevelDecl)));
          add(icallable.TypeArgs, memberActuals);
        }
        return r;
      }

      public static List<TypeArgumentInstantiation> ListFromClass(TopLevelDecl cl, List<Type> actuals) {
        Contract.Requires(cl != null);
        Contract.Requires(actuals != null);
        Contract.Requires(cl.TypeArgs.Count == actuals.Count);

        var r = new List<TypeArgumentInstantiation>();
        for (var i = 0; i < cl.TypeArgs.Count; i++) {
          r.Add(new TypeArgumentInstantiation(cl.TypeArgs[i], actuals[i]));
        }
        return r;
      }

      public static List<TypeArgumentInstantiation> ListFromFormals(List<TypeParameter> formals) {
        Contract.Requires(formals != null);
        return formals.ConvertAll(tp => new TypeArgumentInstantiation(tp, new UserDefinedType(tp)));
      }

      public static List<TypeParameter> ToFormals(List<TypeArgumentInstantiation> typeArgs) {
        Contract.Requires(typeArgs != null);
        return typeArgs.ConvertAll(ta => ta.Formal);
      }

      public static List<Type> ToActuals(List<TypeArgumentInstantiation> typeArgs) {
        Contract.Requires(typeArgs != null);
        return typeArgs.ConvertAll(ta => ta.Actual);
      }
    }

    /// <summary>
    /// Answers two questions whose answers are used to filter type parameters.
    /// For a member c, F, or M:
    ///     (co-)datatype/class/trait <<cl>> {
    ///       <<isStatic>> const c ...
    ///       <<isStatic>> function F ...
    ///       <<isStatic>> method M ...
    ///     }
    /// does a type parameter of "cl"
    ///  - get compiled as a type parameter of the member (needsTypeParameter)
    ///  - get compiled as a type descriptor of the member (needsTypeDescriptor)
    /// For a member of a trait with a rhs/body, if "lookasideBody" is "true", the questions are to
    /// be answered for the member emitted into the companion class, not the signature that goes into
    /// the target type.
    /// </summary>
    protected virtual void TypeArgDescriptorUse(bool isStatic, bool lookasideBody, TopLevelDeclWithMembers cl, out bool needsTypeParameter, out bool needsTypeDescriptor) {
      Contract.Requires(cl is DatatypeDecl || cl is ClassDecl);
      // TODO: Decide whether to express this as a Feature
      throw new NotImplementedException();
    }

    protected internal List<TypeArgumentInstantiation> ForTypeParameters(List<TypeArgumentInstantiation> typeArgs, MemberDecl member, bool lookasideBody) {
      Contract.Requires(member is ConstantField || member is Function || member is Method);
      Contract.Requires(typeArgs != null);
      var memberHasBody =
        (member is ConstantField cf && cf.Rhs != null) ||
        (member is Function f && f.Body != null) ||
        (member is Method m && m.Body != null);
      var r = new List<TypeArgumentInstantiation>();
      foreach (var ta in typeArgs) {
        var tp = ta.Formal;
        if (tp.Parent is TopLevelDeclWithMembers) {
          TypeArgDescriptorUse(member.IsStatic, lookasideBody, (TopLevelDeclWithMembers)member.EnclosingClass, out var needsTypeParameter, out var _);
          if (!needsTypeParameter) {
            continue;
          }
        }
        r.Add(ta);
      }
      return r;
    }

    protected List<TypeArgumentInstantiation> ForTypeDescriptors(List<TypeArgumentInstantiation> typeArgs, TopLevelDecl enclosingClass, MemberDecl member, bool lookasideBody) {
      Contract.Requires(member is ConstantField || member is Function || member is Method);
      Contract.Requires(typeArgs != null);
      var memberHasBody =
        (member is ConstantField cf && cf.Rhs != null) ||
        (member is Function f && f.Body != null) ||
        (member is Method m && m.Body != null);
      var r = new List<TypeArgumentInstantiation>();
      foreach (var ta in typeArgs) {
        var tp = ta.Formal;
        if (tp.Parent is TopLevelDeclWithMembers) {
          TypeArgDescriptorUse(member == null || member.IsStatic, lookasideBody, (TopLevelDeclWithMembers)enclosingClass, out var _, out var needsTypeDescriptor);
          if (!needsTypeDescriptor) {
            continue;
          }
        }
        r.Add(ta);
      }
      return r;
    }

    /// <summary>
    /// The "additionalCustomParameter" is used when the member is an instance function that requires customer-receiver support.
    /// This parameter is then to be added between any run-time type descriptors and the "normal" arguments. The caller will
    /// arrange for "additionalCustomParameter" to be properly bound.
    /// </summary>
    protected abstract ILvalue EmitMemberSelect(Action<ConcreteSyntaxTree> obj, Type objType, MemberDecl member, List<TypeArgumentInstantiation> typeArgs, Dictionary<TypeParameter, Type> typeMap,
      Type expectedType, string/*?*/ additionalCustomParameter = null, bool internalAccess = false);

    protected abstract ConcreteSyntaxTree EmitArraySelect(List<string> indices, Type elmtType, ConcreteSyntaxTree wr);
    protected abstract ConcreteSyntaxTree EmitArraySelect(List<Expression> indices, Type elmtType, bool inLetExprBody,
      ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);
    protected virtual ILvalue EmitArraySelectAsLvalue(string array, List<string> indices, Type elmtType) {
      return SimpleLvalue(wr => {
        wr.Write(array);
        EmitArraySelect(indices, elmtType, wr);
      });
    }
    protected virtual ConcreteSyntaxTree EmitArrayUpdate(List<string> indices, string rhs, Type elmtType, ConcreteSyntaxTree wr) {
      var w = EmitArraySelect(indices, elmtType, wr);
      wr.Write(" = {0}", rhs);
      return w;
    }
    protected ConcreteSyntaxTree EmitArrayUpdate(List<string> indices, Expression rhs, ConcreteSyntaxTree wr) {
      var w = new ConcreteSyntaxTree(wr.RelativeIndentLevel);
      w.Append(TrExpr(rhs, false, wr));
      return EmitArrayUpdate(indices, w.ToString(), rhs.Type, wr);
    }
    protected virtual string ArrayIndexToInt(string arrayIndex, Type fromType) {
      Contract.Requires(arrayIndex != null);
      Contract.Requires(fromType != null);
      return arrayIndex;
    }
    protected virtual string ArrayIndexToNativeInt(string arrayIndex, Type fromType) {
      Contract.Requires(arrayIndex != null);
      Contract.Requires(fromType != null);
      return arrayIndex;
    }
    protected abstract void EmitExprAsInt(Expression expr, bool inLetExprBody, ConcreteSyntaxTree wr,
      ConcreteSyntaxTree wStmts);
    protected abstract void EmitIndexCollectionSelect(Expression source, Expression index, bool inLetExprBody,
      ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);
    protected abstract void EmitIndexCollectionUpdate(Expression source, Expression index, Expression value,
      CollectionType resultCollectionType, bool inLetExprBody, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);
    protected virtual void EmitIndexCollectionUpdate(out ConcreteSyntaxTree wSource, out ConcreteSyntaxTree wIndex, out ConcreteSyntaxTree wValue, ConcreteSyntaxTree wr, bool nativeIndex) {
      wSource = wr.Fork();
      wr.Write('[');
      wIndex = wr.Fork();
      wr.Write("] = ");
      wValue = wr.Fork();
    }
    /// <summary>
    /// If "fromArray" is false, then "source" is a sequence.
    /// If "fromArray" is true, then "source" is an array.
    /// </summary>
    protected abstract void EmitSeqSelectRange(Expression source, Expression lo /*?*/, Expression hi /*?*/,
      bool fromArray, bool inLetExprBody, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);
    protected abstract void EmitSeqConstructionExpr(SeqConstructionExpr expr, bool inLetExprBody, ConcreteSyntaxTree wr,
      ConcreteSyntaxTree wStmts);
    protected abstract void EmitMultiSetFormingExpr(MultiSetFormingExpr expr, bool inLetExprBody, ConcreteSyntaxTree wr,
      ConcreteSyntaxTree wStmts);
    protected abstract void EmitApplyExpr(Type functionType, IToken tok, Expression function, List<Expression> arguments, bool inLetExprBody, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);
    protected virtual bool TargetLambdaCanUseEnclosingLocals => true;
    protected abstract ConcreteSyntaxTree EmitBetaRedex(List<string> boundVars, List<Expression> arguments, List<Type> boundTypes,
      Type resultType, IToken resultTok, bool inLetExprBody, ConcreteSyntaxTree wr, ref ConcreteSyntaxTree wStmts);
    protected virtual void EmitConstructorCheck(string source, DatatypeCtor ctor, ConcreteSyntaxTree wr) {
      wr.Write("{0}.is_{1}", source, ctor.CompileName);
    }
    /// <summary>
    /// EmitDestructor is somewhat similar to following "source" with a call to EmitMemberSelect.
    /// However, EmitDestructor may also need to perform a cast on "source".
    /// Furthermore, EmitDestructor also needs to work for anonymous destructors.
    /// </summary>
    protected abstract void EmitDestructor(string source, Formal dtor, int formalNonGhostIndex, DatatypeCtor ctor, List<Type> typeArgs, Type bvType, ConcreteSyntaxTree wr);
    protected virtual bool TargetLambdasRestrictedToExpressions => false;
    protected abstract ConcreteSyntaxTree CreateLambda(List<Type> inTypes, IToken tok, List<string> inNames,
      Type resultType, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts, bool untyped = false);

    /// <summary>
    /// Emit an "Immediately Invoked Function Expression" with the semantics of
    ///     var bvName: bvType := <<wrRhs>>; <<wrBody>>
    /// where <<wrBody>> will have type "bodyType". In many languages, this IIFE will not be a "let" expression but a "lambda" expression like this:
    ///     ((bvName: bvType) => <<wrBody>>)(<<wrRhs>>)
    /// </summary>
    protected abstract void CreateIIFE(string bvName, Type bvType, IToken bvTok, Type bodyType, IToken bodyTok,
      ConcreteSyntaxTree wr, ref ConcreteSyntaxTree wStmts, out ConcreteSyntaxTree wrRhs, out ConcreteSyntaxTree wrBody);
    protected ConcreteSyntaxTree CreateIIFE_ExprBody(string bvName, Type bvType, IToken bvTok, Expression rhs,
      bool inLetExprBody, Type bodyType, IToken bodyTok, ConcreteSyntaxTree wr, ref ConcreteSyntaxTree wStmts) {
      var innerScope = wStmts.Fork();
      CreateIIFE(bvName, bvType, bvTok, bodyType, bodyTok, wr, ref wStmts, out var wrRhs, out var wrBody);
      wrRhs.Append(TrExpr(rhs, inLetExprBody, innerScope));
      return wrBody;
    }

    protected abstract ConcreteSyntaxTree CreateIIFE0(Type resultType, IToken resultTok, ConcreteSyntaxTree wr,
      ConcreteSyntaxTree wStmts);  // Immediately Invoked Function Expression
    protected abstract ConcreteSyntaxTree CreateIIFE1(int source, Type resultType, IToken resultTok, string bvName,
      ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);  // Immediately Invoked Function Expression
    public enum ResolvedUnaryOp { BoolNot, BitwiseNot, Cardinality }

    protected static readonly Dictionary<UnaryOpExpr.ResolvedOpcode, ResolvedUnaryOp> UnaryOpCodeMap = new() {
      [UnaryOpExpr.ResolvedOpcode.BVNot] = ResolvedUnaryOp.BitwiseNot,
      [UnaryOpExpr.ResolvedOpcode.BoolNot] = ResolvedUnaryOp.BoolNot,
      [UnaryOpExpr.ResolvedOpcode.SeqLength] = ResolvedUnaryOp.Cardinality,
      [UnaryOpExpr.ResolvedOpcode.SetCard] = ResolvedUnaryOp.Cardinality,
      [UnaryOpExpr.ResolvedOpcode.MultiSetCard] = ResolvedUnaryOp.Cardinality,
      [UnaryOpExpr.ResolvedOpcode.MapCard] = ResolvedUnaryOp.Cardinality
    };

    protected abstract void EmitUnaryExpr(ResolvedUnaryOp op, Expression expr, bool inLetExprBody,
      ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);

    protected virtual void CompileBinOp(BinaryExpr.ResolvedOpcode op,
      Expression e0, Expression e1, IToken tok, Type resultType,
      out string opString,
      out string preOpString,
      out string postOpString,
      out string callString,
      out string staticCallString,
      out bool reverseArguments,
      out bool truncateResult,
      out bool convertE1_to_int,
      ConcreteSyntaxTree errorWr) {

      // This default implementation does not handle all cases. It handles some cases that look the same
      // in C-like languages. It also handles cases that can be solved by another operator, but reversing
      // the arguments or following the operation with a negation.
      opString = null;
      preOpString = "";
      postOpString = "";
      callString = null;
      staticCallString = null;
      reverseArguments = false;
      truncateResult = false;
      convertE1_to_int = false;

      BinaryExpr.ResolvedOpcode dualOp = BinaryExpr.ResolvedOpcode.Add;  // NOTE! "Add" is used to say "there is no dual op"
      BinaryExpr.ResolvedOpcode negatedOp = BinaryExpr.ResolvedOpcode.Add;  // NOTE! "Add" is used to say "there is no negated op"

      switch (op) {
        case BinaryExpr.ResolvedOpcode.Iff:
          opString = "=="; break;
        case BinaryExpr.ResolvedOpcode.Imp:
          preOpString = "!"; opString = "||"; break;
        case BinaryExpr.ResolvedOpcode.Or:
          opString = "||"; break;
        case BinaryExpr.ResolvedOpcode.And:
          opString = "&&"; break;
        case BinaryExpr.ResolvedOpcode.BitwiseAnd:
          opString = "&"; break;
        case BinaryExpr.ResolvedOpcode.BitwiseOr:
          opString = "|"; break;
        case BinaryExpr.ResolvedOpcode.BitwiseXor:
          opString = "^"; break;

        case BinaryExpr.ResolvedOpcode.Lt:
        case BinaryExpr.ResolvedOpcode.LtChar:
          opString = "<"; break;
        case BinaryExpr.ResolvedOpcode.Le:
        case BinaryExpr.ResolvedOpcode.LeChar:
          opString = "<="; break;
        case BinaryExpr.ResolvedOpcode.Ge:
        case BinaryExpr.ResolvedOpcode.GeChar:
          opString = ">="; break;
        case BinaryExpr.ResolvedOpcode.Gt:
        case BinaryExpr.ResolvedOpcode.GtChar:
          opString = ">"; break;

        case BinaryExpr.ResolvedOpcode.SetNeq:
          negatedOp = BinaryExpr.ResolvedOpcode.SetEq; break;
        case BinaryExpr.ResolvedOpcode.MultiSetNeq:
          negatedOp = BinaryExpr.ResolvedOpcode.MultiSetEq; break;
        case BinaryExpr.ResolvedOpcode.SeqNeq:
          negatedOp = BinaryExpr.ResolvedOpcode.SeqEq; break;
        case BinaryExpr.ResolvedOpcode.MapNeq:
          negatedOp = BinaryExpr.ResolvedOpcode.MapEq; break;

        case BinaryExpr.ResolvedOpcode.Superset:
          dualOp = BinaryExpr.ResolvedOpcode.Subset; break;
        case BinaryExpr.ResolvedOpcode.MultiSuperset:
          dualOp = BinaryExpr.ResolvedOpcode.MultiSubset; break;

        case BinaryExpr.ResolvedOpcode.ProperSuperset:
          dualOp = BinaryExpr.ResolvedOpcode.ProperSubset; break;
        case BinaryExpr.ResolvedOpcode.ProperMultiSuperset:
          dualOp = BinaryExpr.ResolvedOpcode.ProperMultiSubset; break;

        case BinaryExpr.ResolvedOpcode.NotInSet:
          negatedOp = BinaryExpr.ResolvedOpcode.InSet; break;
        case BinaryExpr.ResolvedOpcode.NotInMultiSet:
          negatedOp = BinaryExpr.ResolvedOpcode.InMultiSet; break;
        case BinaryExpr.ResolvedOpcode.NotInSeq:
          negatedOp = BinaryExpr.ResolvedOpcode.InSeq; break;
        case BinaryExpr.ResolvedOpcode.NotInMap:
          negatedOp = BinaryExpr.ResolvedOpcode.InMap; break;

        default:
          // The operator is one that needs to be handled in the specific compilers.
          Contract.Assert(false); throw new cce.UnreachableException();  // unexpected binary expression
      }

      if (dualOp != BinaryExpr.ResolvedOpcode.Add) {  // remember from above that Add stands for "there is no dual"
        Contract.Assert(negatedOp == BinaryExpr.ResolvedOpcode.Add);
        CompileBinOp(dualOp,
          e1, e0, tok, resultType,
          out opString, out preOpString, out postOpString, out callString, out staticCallString, out reverseArguments, out truncateResult, out convertE1_to_int,
          errorWr);
        reverseArguments = !reverseArguments;
      } else if (negatedOp != BinaryExpr.ResolvedOpcode.Add) {  // remember from above that Add stands for "there is no negated op"
        CompileBinOp(negatedOp,
          e0, e1, tok, resultType,
          out opString, out preOpString, out postOpString, out callString, out staticCallString, out reverseArguments, out truncateResult, out convertE1_to_int,
          errorWr);
        preOpString = "!" + preOpString;
      }
    }

    protected abstract void EmitIsZero(string varName, ConcreteSyntaxTree wr);
    protected abstract void EmitConversionExpr(ConversionExpr e, bool inLetExprBody, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);
    /// <summary>
    /// "fromType" is assignable to "toType", "fromType" is not a subtype of "toType", and both "fromType" and "toType" refer to
    /// reference types or subset types thereof.
    /// </summary>
    protected abstract void EmitTypeTest(string localName, Type fromType, Type toType, IToken tok, ConcreteSyntaxTree wr);
    protected abstract void EmitCollectionDisplay(CollectionType ct, IToken tok, List<Expression> elements,
      bool inLetExprBody, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);  // used for sets, multisets, and sequences
    protected abstract void EmitMapDisplay(MapType mt, IToken tok, List<ExpressionPair> elements,
      bool inLetExprBody, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);

    protected abstract void EmitSetBuilder_New(ConcreteSyntaxTree wr, SetComprehension e, string collectionName);
    protected abstract void EmitMapBuilder_New(ConcreteSyntaxTree wr, MapComprehension e, string collectionName);

    protected abstract void EmitSetBuilder_Add(CollectionType ct, string collName, Expression elmt, bool inLetExprBody, ConcreteSyntaxTree wr);
    protected abstract ConcreteSyntaxTree EmitMapBuilder_Add(MapType mt, IToken tok, string collName, Expression term, bool inLetExprBody, ConcreteSyntaxTree wr);

    /// <summary>
    /// The "ct" type is either a SetType or a MapType.
    /// </summary>
    protected abstract string GetCollectionBuilder_Build(CollectionType ct, IToken tok, string collName, ConcreteSyntaxTree wr);

    protected virtual Type EmitIntegerRange(Type type, out ConcreteSyntaxTree wLo, out ConcreteSyntaxTree wHi, ConcreteSyntaxTree wr) {
      Type result;
      if (AsNativeType(type) != null) {
        wr.Write("{0}.IntegerRange(", IdProtect(type.AsNewtype.FullCompileName));
        result = type;
      } else {
        wr.Write("{0}.IntegerRange(", GetHelperModuleName());
        result = new IntType();
      }
      wLo = wr.Fork();
      wr.Write(", ");
      wHi = wr.Fork();
      wr.Write(')');
      return result;
    }
    protected abstract void EmitSingleValueGenerator(Expression e, bool inLetExprBody, string type,
      ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts);
    protected virtual void FinishModule() { }

    protected virtual void DeclareExternType(OpaqueTypeDecl d, Expression compileTypeHint, ConcreteSyntaxTree wr) { }

    protected virtual void OrganizeModules(Program program, out List<ModuleDefinition> modules) {
      modules = program.CompileModules;
    }

    public override void Compile(Program program, ConcreteSyntaxTree wrx) {
      Contract.Requires(program != null);

      EmitHeader(program, wrx);
      EmitBuiltInDecls(program.BuiltIns, wrx);
      var temp = new List<ModuleDefinition>();
      OrganizeModules(program, out temp);
      program.CompileModules = temp;
      foreach (ModuleDefinition m in program.CompileModules) {
        if (m.IsAbstract) {
          // the purpose of an abstract module is to skip compilation
          continue;
        }
        if (!m.IsToBeCompiled) {
          continue;
        }
        var moduleIsExtern = false;
        string libraryName = null;
        if (!DafnyOptions.O.DisallowExterns) {
          var args = Attributes.FindExpressions(m.Attributes, "extern");
          if (args != null) {
            if (args.Count == 2) {
              libraryName = (string)(args[1] as StringLiteralExpr)?.Value;
            }
            moduleIsExtern = true;
          }
        }
        var wr = CreateModule(m.CompileName, m.IsDefaultModule, moduleIsExtern, libraryName, wrx);
        var v = new CheckHasNoAssumesVisitor(this, wr);
        foreach (TopLevelDecl d in m.TopLevelDecls) {
          bool compileIt = true;
          if (Attributes.ContainsBool(d.Attributes, "compile", ref compileIt) && !compileIt) {
            continue;
          }
          var newLineWriter = wr.Fork();
          if (d is OpaqueTypeDecl) {
            var at = (OpaqueTypeDecl)d;
            bool externP = Attributes.Contains(at.Attributes, "extern");
            if (externP) {
              var exprs = Attributes.FindExpressions(at.Attributes, "extern");
              Contract.Assert(exprs != null);  // because externP is true
              if (exprs.Count == 1) {
                DeclareExternType(at, exprs[0], wr);
              } else {
                Error(d.tok, "Opaque type ('{0}') with extern attribute requires a compile hint.  Expected {{:extern compile_type_hint}} ", wr, at.FullName);
              }
              v.Visit(exprs);
            } else {
              Error(d.tok, "Opaque type ('{0}') cannot be compiled; perhaps make it a type synonym or use :extern.", wr, at.FullName);
            }
          } else if (d is TypeSynonymDecl) {
            var sst = d as SubsetTypeDecl;
            if (sst != null) {
              DeclareSubsetType(sst, wr);
              v.Visit(sst);
            } else {
              continue;
            }
          } else if (d is NewtypeDecl) {
            var nt = (NewtypeDecl)d;
            var w = DeclareNewtype(nt, wr);
            v.Visit(nt);
            CompileClassMembers(program, nt, w);
          } else if ((d as TupleTypeDecl)?.NonGhostDims == 1 && SupportsDatatypeWrapperErasure && DafnyOptions.O.Get(CommonOptionBag.OptimizeErasableDatatypeWrapper)) {
            // ignore this type declaration
          } else if (d is DatatypeDecl) {
            var dt = (DatatypeDecl)d;
            CheckForCapitalizationConflicts(dt.Ctors);
            foreach (var ctor in dt.Ctors) {
              CheckForCapitalizationConflicts(ctor.Destructors);
            }

            if (!DeclaredDatatypes.Add((m, dt.CompileName))) {
              continue;
            }
            var w = DeclareDatatype(dt, wr);
            if (w != null) {
              CompileClassMembers(program, dt, w);
            } else if (DatatypeDeclarationAndMemberCompilationAreSeparate) {
              continue;
            }
          } else if (d is IteratorDecl) {
            var iter = (IteratorDecl)d;
            if (DafnyOptions.O.ForbidNondeterminism && iter.Outs.Count > 0) {
              Error(iter.tok, "since yield parameters are initialized arbitrarily, iterators are forbidden by the --enforce-determinism option", wr);
            }

            var wIter = CreateIterator(iter, wr);
            if (iter.Body == null) {
              Error(iter.tok, "iterator {0} has no body", wIter, iter.FullName);
            } else {
              TrStmtList(iter.Body.Body, wIter);
            }

          } else if (d is TraitDecl trait) {
            // writing the trait
            var w = CreateTrait(trait.CompileName, trait.IsExtern(out _, out _), trait.TypeArgs, trait, trait.ParentTypeInformation.UniqueParentTraits(), trait.tok, wr);
            CompileClassMembers(program, trait, w);
          } else if (d is ClassDecl cl) {
            var include = true;
            if (cl.IsDefaultClass) {
              Predicate<MemberDecl> compilationMaterial = x =>
                !x.IsGhost && (DafnyOptions.O.DisallowExterns || !Attributes.Contains(x.Attributes, "extern"));
              include = cl.Members.Exists(compilationMaterial) || cl.InheritedMembers.Exists(compilationMaterial);
            }
            var classIsExtern = false;
            if (include) {
              classIsExtern = (!DafnyOptions.O.DisallowExterns && Attributes.Contains(cl.Attributes, "extern")) || (cl.IsDefaultClass && Attributes.Contains(cl.EnclosingModuleDefinition.Attributes, "extern"));
              if (classIsExtern && cl.Members.TrueForAll(member => member.IsGhost || Attributes.Contains(member.Attributes, "extern"))) {
                include = false;
              }
            }
            if (include) {
              var cw = CreateClass(IdProtect(d.EnclosingModuleDefinition.CompileName), IdName(cl), classIsExtern, cl.FullName,
                cl.TypeArgs, cl, cl.ParentTypeInformation.UniqueParentTraits(), cl.tok, wr);
              CompileClassMembers(program, cl, cw);
              cw.Finish();
            } else {
              // still check that given members satisfy compilation rules
              var abyss = new NullClassWriter();
              CompileClassMembers(program, cl, abyss);
            }
          } else if (d is ValuetypeDecl) {
            // nop
            continue;
          } else if (d is ModuleDecl) {
            // nop
            continue;
          } else { Contract.Assert(false); }

          newLineWriter.WriteLine();
        }

        FinishModule();
      }
      EmitFooter(program, wrx);
    }

    public ISet<(ModuleDefinition, string)> DeclaredDatatypes { get; } = new HashSet<(ModuleDefinition, string)>();

    protected class NullClassWriter : IClassWriter {
      private readonly ConcreteSyntaxTree abyss = new ConcreteSyntaxTree();
      private readonly ConcreteSyntaxTree block;

      public NullClassWriter() {
        block = abyss.NewBlock("");
      }

      public ConcreteSyntaxTree/*?*/ CreateMethod(Method m, List<TypeArgumentInstantiation> typeArgs, bool createBody, bool forBodyInheritance, bool lookasideBody) {
        return createBody ? block : null;
      }

      public ConcreteSyntaxTree SynthesizeMethod(Method m, List<TypeArgumentInstantiation> typeArgs, bool createBody, bool forBodyInheritance, bool lookasideBody) {
        throw new UnsupportedFeatureException(m.tok, Feature.MethodSynthesis);
      }

      public ConcreteSyntaxTree/*?*/ CreateFunction(string name, List<TypeArgumentInstantiation> typeArgs, List<Formal> formals, Type resultType, IToken tok, bool isStatic, bool createBody, MemberDecl member, bool forBodyInheritance, bool lookasideBody) {
        return createBody ? block : null;
      }
      public ConcreteSyntaxTree/*?*/ CreateGetter(string name, TopLevelDecl enclosingDecl, Type resultType, IToken tok, bool isStatic, bool isConst, bool createBody, MemberDecl/*?*/ member, bool forBodyInheritance) {
        return createBody ? block : null;
      }
      public ConcreteSyntaxTree/*?*/ CreateGetterSetter(string name, Type resultType, IToken tok, bool createBody, MemberDecl/*?*/ member, out ConcreteSyntaxTree setterWriter, bool forBodyInheritance) {
        if (createBody) {
          setterWriter = block;
          return block;
        } else {
          setterWriter = null;
          return null;
        }
      }
      public void DeclareField(string name, TopLevelDecl enclosingDecl, bool isStatic, bool isConst, Type type, IToken tok, string rhs, Field field) { }

      public void InitializeField(Field field, Type instantiatedFieldType, TopLevelDeclWithMembers enclosingClass) { }

      public ConcreteSyntaxTree/*?*/ ErrorWriter() {
        return null; // match the old behavior of Compile() where this is used
      }

      public void Finish() { }
    }

    protected void ReadRuntimeSystem(Program program, string filename, ConcreteSyntaxTree wr) {
      Contract.Requires(filename != null);
      Contract.Requires(wr != null);

      if (DafnyOptions.O.UseRuntimeLib) {
        return;
      }


      var assembly = System.Reflection.Assembly.Load("DafnyPipeline");
      var stream = assembly.GetManifestResourceStream(filename);
      if (stream is null) {
        throw new Exception($"Cannot find embedded resource: {filename}");
      }

      var rd = new StreamReader(stream);
      WriteFromStream(rd, wr.Append((new Verbatim())));
    }

    protected void WriteFromFile(string inputFilename, TextWriter outputWriter) {
      var rd = new StreamReader(new FileStream(inputFilename, FileMode.Open, FileAccess.Read));
      WriteFromStream(rd, outputWriter);
    }

    protected void WriteFromStream(StreamReader rd, TextWriter outputWriter) {
      while (true) {
        string s = rd.ReadLine();
        if (s == null) {
          return;
        }
        outputWriter.WriteLine(s);
      }
    }

    // create a varName that is not a duplicate of formals' name
    protected string GenVarName(string root, List<Formal> formals) {
      bool finished = false;
      while (!finished) {
        finished = true;
        int i = 0;
        foreach (var arg in formals) {
          if (!arg.IsGhost) {
            // FormalName returns a protected name, so we compare a protected version of "root" to it
            if (IdProtect(root).Equals(FormalName(arg, i))) {
              root += root;
              finished = false;
            }
            i++;
          }
        }
      }
      return root;
    }

    protected int WriteFormals(string sep, List<Formal> formals, ConcreteSyntaxTree wr, List<Formal>/*?*/ useTheseNamesForFormals = null) {
      Contract.Requires(sep != null);
      Contract.Requires(formals != null);
      Contract.Requires(wr != null);
      Contract.Requires(useTheseNamesForFormals == null || useTheseNamesForFormals.Count == formals.Count);

      int n = 0;
      for (var i = 0; i < formals.Count; i++) {
        var arg = formals[i];
        if (!arg.IsGhost) {
          string name = FormalName(useTheseNamesForFormals == null ? arg : useTheseNamesForFormals[i], n);
          if (DeclareFormal(sep, name, arg.Type, arg.tok, arg.InParam, wr)) {
            sep = ", ";
          }
          n++;
        }
      }
      return n;  // the number of formals written
    }

    protected string FormalName(Formal formal, int i) {
      Contract.Requires(formal != null);
      Contract.Ensures(Contract.Result<string>() != null);

      return IdProtect(formal.HasName ? formal.CompileName : "_a" + i);
    }

    public static bool HasMain(Program program, out Method mainMethod) {
      Contract.Ensures(Contract.Result<bool>() == (Contract.ValueAtReturn(out mainMethod) != null));
      mainMethod = null;
      bool hasMain = false;
      string name = DafnyOptions.O.MainMethod;
      if (name != null && name == "-") {
        return false;
      }

      if (name != null && name != "") {
        foreach (var module in program.CompileModules) {
          if (module.IsAbstract) {
            // the purpose of an abstract module is to skip compilation
            continue;
          }
          foreach (var decl in module.TopLevelDecls) {
            if (decl is TopLevelDeclWithMembers c) {
              foreach (MemberDecl member in c.Members) {
                if (member is Method m && member.FullDafnyName == name) {
                  mainMethod = m;
                  if (!IsPermittedAsMain(program, mainMethod, out string reason)) {
                    ReportError(program.Reporter, mainMethod.tok, "The method '{0}' is not permitted as a main method ({1}).", null, name, reason);
                    mainMethod = null;
                    return false;
                  } else {
                    return true;
                  }
                }
              }
            }
          }
        }
        ReportError(program.Reporter, program.DefaultModule.tok, "Could not find the method named by the -Main option: {0}", null, name);
      }
      foreach (var module in program.CompileModules) {
        if (module.IsAbstract) {
          // the purpose of an abstract module is to skip compilation
          continue;
        }
        foreach (var decl in module.TopLevelDecls) {
          var c = decl as TopLevelDeclWithMembers;
          if (c != null) {
            foreach (var member in c.Members) {
              var m = member as Method;
              if (m != null && Attributes.Contains(m.Attributes, "main")) {
                if (mainMethod == null) {
                  mainMethod = m;
                  hasMain = true;
                } else {
                  // more than one main in the program
                  ReportError(program.Reporter, m.tok, "More than one method is marked '{{:main}}'. First declaration appeared at {0}.", null,
                    ErrorReporter.TokenToString(mainMethod.tok));
                  hasMain = false;
                }
              }
            }
          }
        }
      }
      if (hasMain) {
        if (!IsPermittedAsMain(program, mainMethod, out string reason)) {
          ReportError(program.Reporter, mainMethod.tok, "This method marked '{{:main}}' is not permitted as a main method ({0}).", null, reason);
          mainMethod = null;
          return false;
        } else {
          return true;
        }
      }
      if (mainMethod != null) {
        mainMethod = null;
        return false;
      }

      mainMethod = null;
      foreach (var module in program.CompileModules) {
        if (module.IsAbstract) {
          // the purpose of an abstract module is to skip compilation
          continue;
        }
        foreach (var decl in module.TopLevelDecls) {
          var c = decl as TopLevelDeclWithMembers;
          if (c != null) {
            foreach (var member in c.Members) {
              var m = member as Method;
              if (m != null && m.Name == DefaultNameMain) {
                if (mainMethod == null) {
                  mainMethod = m;
                  hasMain = true;
                } else {
                  // more than one main in the program
                  ReportError(program.Reporter, m.tok, "More than one method is declared as '{0}'. First declaration appeared at {1}.", null,
                    DefaultNameMain, ErrorReporter.TokenToString(mainMethod.tok));
                  hasMain = false;
                }
              }
            }
          }
        }
      }

      if (hasMain) {
        if (!IsPermittedAsMain(program, mainMethod, out string reason)) {
          ReportError(program.Reporter, mainMethod.tok, "This method 'Main' is not permitted as a main method ({0}).", null, reason);
          return false;
        } else {
          return true;
        }
      } else {
        // make sure "mainMethod" returns as null
        mainMethod = null;
        return false;
      }
    }

    public static bool IsPermittedAsMain(Program program, Method m, out String reason) {
      Contract.Requires(m.EnclosingClass is TopLevelDeclWithMembers);
      // In order to be a legal Main() method, the following must be true:
      //    The method is not a ghost method
      //    The method takes no non-ghost parameters and no type parameters
      //      except at most one array of type "array<string>"
      //    The enclosing type does not take any type parameters
      //    If the method is an instance (that is, non-static) method in a class, then the enclosing class must not declare any constructor
      // In addition, either:
      //    The method is called "Main"
      //    The method has no requires clause
      //    The method has no modifies clause
      // or:
      //    The method is annotated with {:main}
      // Note, in the case where the method is annotated with {:main}, the method is allowed to have preconditions and modifies clauses.
      // This lets the programmer add some explicit assumptions about the outside world, modeled, for example, via ghost parameters.
      var cl = (TopLevelDeclWithMembers)m.EnclosingClass;
      if (m.IsGhost) {
        reason = "the method is ghost";
        return false;
      }
      if (m.TypeArgs.Count != 0) {
        reason = "the method has type parameters";
        return false;
      }
      if (cl is OpaqueTypeDecl) {
        reason = "the enclosing type is an opaque type";
        return false;
      }
      if (!m.IsStatic) {
        if (cl is TraitDecl) {
          reason = "the method is not static and the enclosing type does not support auto-initialization";
          return false;
        } else if (cl is ClassDecl) {
          if (cl.Members.Exists(f => f is Constructor)) {
            reason = "the method is not static and the enclosing class has constructors";
            return false;
          }
        } else {
          var ty = UserDefinedType.FromTopLevelDeclWithAllBooleanTypeParameters(cl);
          if (!ty.HasCompilableValue) {
            reason = "the method is not static and the enclosing type does not support auto-initialization";
            return false;
          }
        }
      }
      if (!m.Ins.TrueForAll(f => f.IsGhost)) {
        var nonGhostFormals = m.Ins.Where(f => !f.IsGhost).ToList();
        if (nonGhostFormals.Count > 1) {
          reason = "the method has two or more non-ghost parameters";
          return false;
        }
        var typeOfUniqueFormal = nonGhostFormals[0].Type.NormalizeExpandKeepConstraints();
        if (typeOfUniqueFormal.AsSeqType is not { } seqType ||
            seqType.Arg.AsSeqType is not { } subSeqType ||
            !subSeqType.Arg.IsCharType) {
          reason = "the method's non-ghost argument type should be an seq<string>, got " + typeOfUniqueFormal;
          return false;
        }
      } else {
        // Need to manually insert the args.
        var argsType = new SeqType(new SeqType(new CharType()));
        m.Ins.Add(new ImplicitFormal(m.tok, "_noArgsParameter", argsType, true, false));
      }
      if (!m.Outs.TrueForAll(f => f.IsGhost)) {
        reason = "the method has non-ghost out parameters";
        return false;
      }
      if (Attributes.Contains(m.Attributes, "main")) {
        reason = "";
        return true;
      }
      if (m.Req.Count != 0) {
        reason = "the method has requires clauses";
        return false;
      }
      if (m.Mod.Expressions.Count != 0) {
        reason = "the method has modifies clauses";
        return false;
      }
      reason = "";
      return true;
    }

    void OrderedBySCC(List<MemberDecl> decls, TopLevelDeclWithMembers c) {
      List<ConstantField> consts = new List<ConstantField>();
      foreach (var decl in decls) {
        if (decl is ConstantField) {
          consts.Add((ConstantField)decl);
        }
      }
      consts.Sort((a, b) => c.EnclosingModuleDefinition.CallGraph.GetSCCRepresentativeId(a) - c.EnclosingModuleDefinition.CallGraph.GetSCCRepresentativeId(b));
      foreach (var con in consts) {
        decls.Remove(con);
      }
      decls.AddRange(consts);
    }

    public virtual bool NeedsCustomReceiver(MemberDecl member) {
      Contract.Requires(member != null);
      // One of the limitations in many target language encodings are restrictions to instance members. If an
      // instance member can't be directly expressed in the target language, we make it a static member with an
      // additional first argument specifying the `this`, giving it a `CustomReceiver`.
      if (member.IsStatic) {
        return false;
      } else if (member.EnclosingClass is NewtypeDecl) {
        return true;
      } else if (member.EnclosingClass is TraitDecl) {
        return member is ConstantField { Rhs: { } } or Function { Body: { } } or Method { Body: { } };
      } else if (member.EnclosingClass is DatatypeDecl datatypeDecl) {
        // An undefined value "o" cannot use this o.F(...) form in most languages.
        // Also, an erasable wrapper type has a receiver that's not part of the enclosing target class.
        return datatypeDecl.Ctors.Any(ctor => ctor.IsGhost) || DatatypeWrapperEraser.IsErasableDatatypeWrapper(datatypeDecl, out _);
      } else {
        return false;
      }
    }

    void CompileClassMembers(Program program, TopLevelDeclWithMembers c, IClassWriter classWriter) {
      Contract.Requires(c != null);
      Contract.Requires(classWriter != null);
      Contract.Requires(thisContext == null);
      Contract.Ensures(thisContext == null);

      var errorWr = classWriter.ErrorWriter();
      var v = new CheckHasNoAssumesVisitor(this, errorWr);

      if (c is ClassDecl) {
        CheckHandleWellformed((ClassDecl)c, errorWr);
      }

      var inheritedMembers = c.InheritedMembers;
      CheckForCapitalizationConflicts(c.Members, inheritedMembers);
      OrderedBySCC(inheritedMembers, c);
      OrderedBySCC(c.Members, c);

      if (c is not TraitDecl || TraitRepeatsInheritedDeclarations) {
        thisContext = c;
        foreach (var member in inheritedMembers.Select(memberx => (memberx as Function)?.ByMethodDecl ?? memberx)) {
          Contract.Assert(!member.IsStatic);  // only instance members should ever be added to .InheritedMembers
          if (member.IsGhost) {
            // skip
          } else if (c is TraitDecl) {
            RedeclareInheritedMember(member, classWriter);
          } else if (member is ConstantField) {
            var cf = (ConstantField)member;
            var cfType = cf.Type.Subst(c.ParentFormalTypeParametersToActuals);
            if (cf.Rhs == null) {
              Contract.Assert(!cf.IsStatic); // as checked above, only instance members can be inherited
              classWriter.DeclareField("_" + cf.CompileName, c, false, false, cfType, cf.tok, PlaceboValue(cfType, errorWr, cf.tok, true), cf);
            }
            var w = CreateFunctionOrGetter(cf, IdName(cf), c, false, true, true, classWriter);
            Contract.Assert(w != null);  // since the previous line asked for a body
            if (cf.Rhs == null) {
              var sw = EmitReturnExpr(w);
              sw = EmitCoercionIfNecessary(cfType, cf.Type, cf.tok, sw);
              // get { return this._{0}; }
              sw.Append(EmitThis());
              sw.Write("._{0}", cf.CompileName);
            } else {
              EmitCallToInheritedConstRHS(cf, w);
            }
          } else if (member is Field f) {
            var fType = f.Type.Subst(c.ParentFormalTypeParametersToActuals);
            // every field is inherited
            classWriter.DeclareField("_" + f.CompileName, c, false, false, fType, f.tok, PlaceboValue(fType, errorWr, f.tok, true), f);
            ConcreteSyntaxTree wSet;
            var wGet = classWriter.CreateGetterSetter(IdName(f), f.Type, f.tok, true, member, out wSet, true);
            {
              var sw = EmitReturnExpr(wGet);
              sw = EmitCoercionIfNecessary(fType, f.Type, f.tok, sw);
              // get { return this._{0}; }
              sw.Append(EmitThis());
              sw.Write("._{0}", f.CompileName);
            }
            {
              // set { this._{0} = value; }
              wSet.Append(EmitThis());
              wSet.Write("._{0}", f.CompileName);
              var sw = EmitAssignmentRhs(wSet);
              sw = EmitCoercionIfNecessary(f.Type, fType, f.tok, sw);
              EmitSetterParameter(sw);
            }
          } else if (member is Function fn) {
            if (!Attributes.Contains(fn.Attributes, "extern")) {
              Contract.Assert(fn.Body != null);
              var w = classWriter.CreateFunction(IdName(fn), CombineAllTypeArguments(fn), fn.Formals, fn.ResultType, fn.tok, fn.IsStatic, true, fn, true, false);
              EmitCallToInheritedFunction(fn, w);
            }
          } else if (member is Method method) {
            if (!Attributes.Contains(method.Attributes, "extern")) {
              Contract.Assert(method.Body != null);
              var w = classWriter.CreateMethod(method, CombineAllTypeArguments(member), true, true, false);
              EmitCallToInheritedMethod(method, w);
            }
          } else {
            Contract.Assert(false);  // unexpected member
          }
        }
        thisContext = null;
      }

      foreach (MemberDecl memberx in c.Members) {
        var member = (memberx as Function)?.ByMethodDecl ?? memberx;
        if (!member.IsStatic) {
          thisContext = c;
        }
        if (c is TraitDecl && member.OverriddenMember != null && !member.IsOverrideThatAddsBody) {
          if (TraitRepeatsInheritedDeclarations) {
            RedeclareInheritedMember(member, classWriter);
          } else {
            // emit nothing in the trait; this member will be emitted in the classes that extend this trait
          }
        } else if (member is Field) {
          var f = (Field)member;
          if (f.IsGhost) {
            // emit nothing
          } else if (!DafnyOptions.O.DisallowExterns && Attributes.Contains(f.Attributes, "extern")) {
            // emit nothing
          } else if (f is ConstantField) {
            var cf = (ConstantField)f;
            if (cf.IsStatic && !SupportsStaticsInGenericClasses && cf.EnclosingClass.TypeArgs.Count != 0) {
              var wBody = classWriter.CreateFunction(IdName(cf), CombineAllTypeArguments(cf), new List<Formal>(), cf.Type, cf.tok, true, true, member, false, false);
              Contract.Assert(wBody != null);  // since the previous line asked for a body
              if (cf.Rhs != null) {
                CompileReturnBody(cf.Rhs, f.Type, wBody, null);
              } else {
                EmitReturnExpr(PlaceboValue(cf.Type, wBody, cf.tok, true), wBody);
              }
            } else {
              ConcreteSyntaxTree wBody;
              if (cf.IsStatic) {
                wBody = CreateFunctionOrGetter(cf, IdName(cf), c, true, true, false, classWriter);
                Contract.Assert(wBody != null);  // since the previous line asked for a body
              } else if (NeedsCustomReceiver(cf)) {
                // An instance field in a newtype needs to be modeled as a static function that takes a parameter,
                // because a newtype value is always represented as some existing type.
                // Likewise, an instance const with a RHS in a trait needs to be modeled as a static function (in the companion class)
                // that takes a parameter, because trait-equivalents in target languages don't allow implementations.
                wBody = classWriter.CreateFunction(IdName(cf), CombineAllTypeArguments(cf), new List<Formal>(), cf.Type, cf.tok, true, true, cf, false, true);
                Contract.Assert(wBody != null);  // since the previous line asked for a body
                if (c is TraitDecl) {
                  // also declare a function for the field in the interface
                  var wBodyInterface = CreateFunctionOrGetter(cf, IdName(cf), c, false, false, false, classWriter);
                  Contract.Assert(wBodyInterface == null);  // since the previous line said not to create a body
                }
              } else if (c is TraitDecl) {
                wBody = CreateFunctionOrGetter(cf, IdName(cf), c, false, false, false, classWriter);
                Contract.Assert(wBody == null);  // since the previous line said not to create a body
              } else if (cf.Rhs == null && c is ClassDecl) {
                // create a backing field, since this constant field may be assigned in constructors
                classWriter.DeclareField("_" + f.CompileName, c, false, false, f.Type, f.tok, PlaceboValue(f.Type, errorWr, f.tok, true), f);
                wBody = CreateFunctionOrGetter(cf, IdName(cf), c, false, true, false, classWriter);
                Contract.Assert(wBody != null);  // since the previous line asked for a body
              } else {
                wBody = CreateFunctionOrGetter(cf, IdName(cf), c, false, true, false, classWriter);
                Contract.Assert(wBody != null);  // since the previous line asked for a body
              }
              if (wBody != null) {
                if (cf.Rhs != null) {
                  CompileReturnBody(cf.Rhs, cf.Type, wBody, null);
                } else if (!cf.IsStatic && c is ClassDecl) {
                  var sw = EmitReturnExpr(wBody);
                  var typeSubst = new Dictionary<TypeParameter, Type>();
                  cf.EnclosingClass.TypeArgs.ForEach(tp => typeSubst.Add(tp, (Type)new UserDefinedType(tp)));
                  var typeArgs = CombineAllTypeArguments(cf);
                  EmitMemberSelect(wr => wr.Append(EmitThis()), UserDefinedType.FromTopLevelDecl(c.tok, c), cf,
                    typeArgs, typeSubst, f.Type, internalAccess: true).EmitRead(sw);
                } else {
                  EmitReturnExpr(PlaceboValue(cf.Type, wBody, cf.tok, true), wBody);
                }
              }
            }
          } else if (c is TraitDecl) {
            ConcreteSyntaxTree wSet;
            var wGet = classWriter.CreateGetterSetter(IdName(f), f.Type, f.tok, false, member, out wSet, false);
            Contract.Assert(wSet == null && wGet == null);  // since the previous line specified no body
          } else {
            // A trait field is just declared, not initialized. Any other field gets a default value if field's type is an auto-init type and
            // gets a placebo value if the field's type is not an auto-init type.
            var rhs = c is TraitDecl ? null : PlaceboValue(f.Type, errorWr, f.tok, true);
            classWriter.DeclareField(IdName(f), c, f.IsStatic, false, f.Type, f.tok, rhs, f);
          }
          if (f is ConstantField && ((ConstantField)f).Rhs != null) {
            v.Visit(((ConstantField)f).Rhs);
          }
        } else if (member is Function) {
          var f = (Function)member;
          if (f.Body == null && !(c is TraitDecl && !f.IsStatic) &&
              !(!DafnyOptions.O.DisallowExterns && (Attributes.Contains(f.Attributes, "dllimport") || (IncludeExternMembers && Attributes.Contains(f.Attributes, "extern"))))) {
            // A (ghost or non-ghost) function must always have a body, except if it's an instance function in a trait.
            if (Attributes.Contains(f.Attributes, "axiom") || (!DafnyOptions.O.DisallowExterns && Attributes.Contains(f.Attributes, "extern"))) {
              // suppress error message
            } else {
              Error(f.tok, "Function {0} has no body", errorWr, f.FullName);
            }
          } else if (f.IsGhost) {
            // nothing to compile, but we do check for assumes
            if (f.Body == null) {
              Contract.Assert((c is TraitDecl && !f.IsStatic) || Attributes.Contains(f.Attributes, "extern"));
            }

            if (Attributes.Contains(f.Attributes, "test")) {
              Error(f.tok, "Function {0} must be compiled to use the {{:test}} attribute", errorWr, f.FullName);
            }
          } else if (c is TraitDecl && !f.IsStatic) {
            if (f.OverriddenMember == null) {
              var w = classWriter.CreateFunction(IdName(f), CombineAllTypeArguments(f), f.Formals, f.ResultType, f.tok, false, false, f, false, false);
              Contract.Assert(w == null); // since we requested no body
            } else if (TraitRepeatsInheritedDeclarations) {
              RedeclareInheritedMember(f, classWriter);
            }
            if (f.Body != null) {
              CompileFunction(f, classWriter, true);
            }
          } else {
            CompileFunction(f, classWriter, false);
          }
          v.Visit(f);
        } else if (member is Method m) {
          if (Attributes.Contains(m.Attributes, "synthesize")) {
            if (m.IsStatic && m.Outs.Count > 0 && m.Body == null) {
              classWriter.SynthesizeMethod(m, CombineAllTypeArguments(m), true, true, false);
            } else {
              Error(m.tok, "Method {0} is annotated with :synthesize but " +
                           "is not static, has a body, or does not return " +
                           "anything",
                errorWr, m.FullName);
            }
          } else if (m.Body == null && !(c is TraitDecl && !m.IsStatic) &&
                     !(!DafnyOptions.O.DisallowExterns && (Attributes.Contains(m.Attributes, "dllimport") || (IncludeExternMembers && Attributes.Contains(m.Attributes, "extern"))))) {
            // A (ghost or non-ghost) method must always have a body, except if it's an instance method in a trait.
            if (Attributes.Contains(m.Attributes, "axiom") || (!DafnyOptions.O.DisallowExterns && Attributes.Contains(m.Attributes, "extern"))) {
              // suppress error message
            } else {
              Error(m.tok, "Method {0} has no body", errorWr, m.FullName);
            }
          } else if (m.IsGhost) {
            if (m.Body == null) {
              Contract.Assert(c is TraitDecl && !m.IsStatic);
            }
          } else if (c is TraitDecl && !m.IsStatic) {
            if (m.OverriddenMember == null) {
              var w = classWriter.CreateMethod(m, CombineAllTypeArguments(m), false, false, false);
              Contract.Assert(w == null);  // since we requested no body
            } else if (TraitRepeatsInheritedDeclarations) {
              RedeclareInheritedMember(m, classWriter);
            }
            if (m.Body != null) {
              CompileMethod(program, m, classWriter, true);
            }
          } else {
            CompileMethod(program, m, classWriter, false);
          }
          v.Visit(m);
        } else {
          Contract.Assert(false); throw new cce.UnreachableException();  // unexpected member
        }

        thisContext = null;
      }
    }

    protected ConcreteSyntaxTree /*?*/ CreateFunctionOrGetter(ConstantField cf, string name, TopLevelDecl enclosingDecl, bool isStatic,
      bool createBody, bool forBodyInheritance, IClassWriter classWriter) {
      var typeArgs = CombineAllTypeArguments(cf);
      var typeDescriptors = ForTypeDescriptors(typeArgs, cf.EnclosingClass, cf, false);
      if (NeedsTypeDescriptors(typeDescriptors)) {
        return classWriter.CreateFunction(name, typeArgs, new List<Formal>(), cf.Type, cf.tok, isStatic, createBody, cf, forBodyInheritance, false);
      } else {
        return classWriter.CreateGetter(name, enclosingDecl, cf.Type, cf.tok, isStatic, true, createBody, cf, forBodyInheritance);
      }
    }

    private void RedeclareInheritedMember(MemberDecl member, IClassWriter classWriter) {
      Contract.Requires(member != null);
      Contract.Requires(classWriter != null);

      if (member is ConstantField cf) {
        var wBody = CreateFunctionOrGetter(cf, IdName(cf), member.EnclosingClass, false, false, false, classWriter);
        Contract.Assert(wBody == null); // since the previous line said not to create a body
      } else if (member is Field field) {
        ConcreteSyntaxTree wSet;
        var wGet = classWriter.CreateGetterSetter(IdName(field), field.Type, field.tok, false, member, out wSet, false);
        Contract.Assert(wGet == null && wSet == null); // since the previous line said not to create a body
      } else if (member is Function) {
        var fn = ((Function)member).Original;
        var wBody = classWriter.CreateFunction(IdName(fn), CombineAllTypeArguments(fn), fn.Formals, fn.ResultType, fn.tok, fn.IsStatic, false, fn, false, false);
        Contract.Assert(wBody == null); // since the previous line said not to create a body
      } else if (member is Method) {
        var method = ((Method)member).Original;
        var wBody = classWriter.CreateMethod(method, CombineAllTypeArguments(method), false, false, false);
        Contract.Assert(wBody == null); // since the previous line said not to create a body
      } else {
        Contract.Assert(false); // unexpected member
      }
    }

    protected void EmitCallToInheritedConstRHS(ConstantField f, ConcreteSyntaxTree wr) {
      Contract.Requires(f != null);
      Contract.Requires(!f.IsStatic);
      Contract.Requires(f.EnclosingClass is TraitDecl);
      Contract.Requires(f.Rhs != null);
      Contract.Requires(wr != null);
      Contract.Requires(thisContext != null);

      var fOriginal = f;

      // In a target language that requires type coercions, the function declared in "thisContext" has
      // the same signature as in "fOriginal.EnclosingClass".
      wr = EmitReturnExpr(wr);
      wr = EmitCoercionIfNecessary(f.Type, fOriginal.Type, f.tok, wr);

      var calleeReceiverType = UserDefinedType.FromTopLevelDecl(f.tok, f.EnclosingClass).Subst(thisContext.ParentFormalTypeParametersToActuals);
      wr.Write("{0}{1}", TypeName_Companion(calleeReceiverType, wr, f.tok, f), ModuleSeparator);
      var typeArgs = CombineAllTypeArguments(f, thisContext);
      EmitNameAndActualTypeArgs(IdName(f), TypeArgumentInstantiation.ToActuals(ForTypeParameters(typeArgs, f, true)), f.tok, wr);
      wr.Write("(");
      var sep = "";
      EmitTypeDescriptorsActuals(ForTypeDescriptors(typeArgs, f.EnclosingClass, f, true), f.tok, wr, ref sep);

      wr.Write(sep);
      var w = EmitCoercionIfNecessary(UserDefinedType.FromTopLevelDecl(f.tok, thisContext), calleeReceiverType, f.tok, wr);
      w.Append(EmitThis());
      wr.Write(")");
    }

    protected void EmitCallToInheritedFunction(Function f, ConcreteSyntaxTree wr) {
      Contract.Requires(f != null);
      Contract.Requires(!f.IsStatic);
      Contract.Requires(f.EnclosingClass is TraitDecl);
      Contract.Requires(f.Body != null);
      Contract.Requires(wr != null);
      Contract.Requires(thisContext != null);

      // There are three types involved.
      // First, "f.Original.EnclosingClass" is the trait where the function was first declared.
      // In descendant traits from there on, the function may occur several times, each time with
      // a strengthening of the specification. Those traits do no play a role here.
      // Second, there is "f.EnclosingClass", which is the trait where the function is given a body.
      // Often, "f.EnclosingClass" and "f.Original.EnclosingClass" will be the same.
      // Third and finally, there is "thisContext", which is the class that inherits "f" and its
      // implementation, and for which we're about to generate a call to body compiled for "f".

      // In a target language that requires type coercions, the function declared in "thisContext" has
      // the same signature as in "f.Original.EnclosingClass".
      wr = EmitReturnExpr(wr);
      wr = EmitCoercionIfNecessary(f.ResultType, f.Original.ResultType, f.tok, wr);

      var calleeReceiverType = UserDefinedType.FromTopLevelDecl(f.tok, f.EnclosingClass).Subst(thisContext.ParentFormalTypeParametersToActuals);
      wr.Write("{0}{1}", TypeName_Companion(calleeReceiverType, wr, f.tok, f), ModuleSeparator);
      var typeArgs = CombineAllTypeArguments(f, thisContext);
      EmitNameAndActualTypeArgs(IdName(f), TypeArgumentInstantiation.ToActuals(ForTypeParameters(typeArgs, f, true)), f.tok, wr);
      wr.Write("(");
      var sep = "";
      EmitTypeDescriptorsActuals(ForTypeDescriptors(typeArgs, f.EnclosingClass, f, true), f.tok, wr, ref sep);

      wr.Write(sep);
      var w = EmitCoercionIfNecessary(UserDefinedType.FromTopLevelDecl(f.tok, thisContext), calleeReceiverType, f.tok, wr);
      w.Append(EmitThis());
      sep = ", ";

      for (int j = 0, l = 0; j < f.Formals.Count; j++) {
        var p = f.Formals[j];
        if (!p.IsGhost) {
          wr.Write(sep);
          w = EmitCoercionIfNecessary(f.Original.Formals[j].Type, f.Formals[j].Type, f.tok, wr);
          w.Write(IdName(p));
          sep = ", ";
          l++;
        }
      }
      wr.Write(")");
    }

    protected void EmitCallToInheritedMethod(Method method, ConcreteSyntaxTree wr) {
      Contract.Requires(method != null);
      Contract.Requires(!method.IsStatic);
      Contract.Requires(method.EnclosingClass is TraitDecl);
      Contract.Requires(method.Body != null);
      Contract.Requires(wr != null);
      Contract.Requires(thisContext != null);

      // There are three types involved. See comment in EmitCallToInheritedFunction.

      var nonGhostOutParameterCount = method.Outs.Count(p => !p.IsGhost);
      var returnStyleOuts = UseReturnStyleOuts(method, nonGhostOutParameterCount);
      var returnStyleOutCollector = nonGhostOutParameterCount > 1 && returnStyleOuts && !SupportsMultipleReturns ? ProtectedFreshId("_outcollector") : null;

      var outTmps = new List<string>();  // contains a name for each non-ghost formal out-parameter
      var outTypes = new List<Type>();  // contains a type for each non-ghost formal out-parameter
      for (int i = 0; i < method.Outs.Count; i++) {
        Formal p = method.Outs[i];
        if (!p.IsGhost) {
          var target = returnStyleOutCollector != null ? IdName(p) : ProtectedFreshId("_out");
          outTmps.Add(target);
          outTypes.Add(p.Type);
          DeclareLocalVar(target, p.Type, p.tok, false, null, wr);
        }
      }
      Contract.Assert(outTmps.Count == nonGhostOutParameterCount && outTypes.Count == nonGhostOutParameterCount);

      if (returnStyleOutCollector != null) {
        DeclareSpecificOutCollector(returnStyleOutCollector, wr, outTypes, outTypes);
      } else if (nonGhostOutParameterCount > 0 && returnStyleOuts) {
        wr.Write("{0} = ", Util.Comma(outTmps));
      }

      var protectedName = IdName(method);
      var calleeReceiverType = UserDefinedType.FromTopLevelDecl(method.tok, method.EnclosingClass).Subst(thisContext.ParentFormalTypeParametersToActuals);
      wr.Write(TypeName_Companion(calleeReceiverType, wr, method.tok, method));
      wr.Write(ClassAccessor);

      var typeArgs = CombineAllTypeArguments(method, thisContext);
      EmitNameAndActualTypeArgs(protectedName, TypeArgumentInstantiation.ToActuals(ForTypeParameters(typeArgs, method, true)), method.tok, wr);
      wr.Write("(");
      var sep = "";
      EmitTypeDescriptorsActuals(ForTypeDescriptors(typeArgs, method.EnclosingClass, method, true), method.tok, wr, ref sep);

      wr.Write(sep);
      var w = EmitCoercionIfNecessary(UserDefinedType.FromTopLevelDecl(method.tok, thisContext), calleeReceiverType, method.tok, wr);
      w.Append(EmitThis());
      sep = ", ";

      for (int j = 0, l = 0; j < method.Ins.Count; j++) {
        var p = method.Ins[j];
        if (!p.IsGhost) {
          wr.Write(sep);
          w = EmitCoercionIfNecessary(method.Original.Ins[j].Type, method.Ins[j].Type, method.tok, wr);
          w.Write(IdName(p));
          sep = ", ";
          l++;
        }
      }

      if (!returnStyleOuts) {
        foreach (var outTmp in outTmps) {
          wr.Write(sep);
          EmitActualOutArg(outTmp, wr);
          sep = ", ";
        }
      }
      wr.Write(')');
      EndStmt(wr);

      if (returnStyleOutCollector != null) {
        EmitCastOutParameterSplits(returnStyleOutCollector, outTmps, wr, outTypes, outTypes, method.tok);
        EmitReturn(method.Outs, wr);
      } else if (!returnStyleOuts) {
        for (int j = 0, l = 0; j < method.Outs.Count; j++) {
          var p = method.Outs[j];
          if (!p.IsGhost) {
            EmitAssignment(IdName(p), method.Outs[j].Type, outTmps[l], outTypes[l], wr);
            l++;
          }
        }
      } else {
        var wrReturn = EmitReturnExpr(wr);
        sep = "";
        for (int j = 0, l = 0; j < method.Outs.Count; j++) {
          var p = method.Outs[j];
          if (!p.IsGhost) {
            wrReturn.Write(sep);
            w = EmitCoercionIfNecessary(method.Outs[j].Type, outTypes[l], method.tok, wrReturn);
            w.Write(outTmps[l]);
            sep = ", ";
            l++;
          }
        }
      }
    }

    protected List<TypeArgumentInstantiation> CombineAllTypeArguments(MemberDecl member) {
      Contract.Requires(member != null);
      var classActuals = member.EnclosingClass.TypeArgs.ConvertAll(tp => (Type)new UserDefinedType(tp));
      var memberActuals = member is ICallable ic ? ic.TypeArgs.ConvertAll(tp => (Type)new UserDefinedType(tp)) : null;
      return CombineAllTypeArguments(member, classActuals, memberActuals);
    }

    protected List<TypeArgumentInstantiation> CombineAllTypeArguments(MemberDecl member, TopLevelDeclWithMembers receiverContext) {
      Contract.Requires(member is ICallable);
      Contract.Requires(receiverContext != null);
      var classActuals = member.EnclosingClass.TypeArgs.ConvertAll(tp => receiverContext.ParentFormalTypeParametersToActuals[tp]);
      var memberActuals = ((ICallable)member).TypeArgs.ConvertAll(tp => (Type)new UserDefinedType(tp));
      return CombineAllTypeArguments(member, classActuals, memberActuals);
    }

    protected List<TypeArgumentInstantiation> CombineAllTypeArguments(MemberDecl member, List<Type> typeArgsEnclosingClass, List<Type> typeArgsMember) {
      Contract.Requires(member != null);
      Contract.Requires(typeArgsEnclosingClass != null);
      Contract.Requires(typeArgsMember != null);

      return TypeArgumentInstantiation.ListFromMember(member, typeArgsEnclosingClass, typeArgsMember);
    }

    protected int WriteRuntimeTypeDescriptorsFormals(List<TypeArgumentInstantiation> typeParams,
      ConcreteSyntaxTree wr, ref string prefix, Func<TypeParameter, string> formatter) {
      Contract.Requires(typeParams != null);
      Contract.Requires(prefix != null);
      Contract.Requires(wr != null);
      Contract.Ensures(Contract.ValueAtReturn(out prefix) != null);

      var c = 0;
      foreach (var ta in typeParams) {
        var tp = ta.Formal;
        if (NeedsTypeDescriptor(tp)) {
          var formatted = formatter(tp);
          wr.Write($"{prefix}{formatted}");
          prefix = ", ";

          c++;
        }
      }
      return c;
    }

    void CheckHandleWellformed(ClassDecl cl, ConcreteSyntaxTree/*?*/ errorWr) {
      Contract.Requires(cl != null);
      var isHandle = true;
      if (Attributes.ContainsBool(cl.Attributes, "handle", ref isHandle) && isHandle) {
        foreach (var trait in cl.ParentTraitHeads) {
          isHandle = true;
          if (Attributes.ContainsBool(trait.Attributes, "handle", ref isHandle) && isHandle) {
            // all is good
          } else {
            Error(cl.tok, "{0} '{1}' is marked as :handle, so all the traits it extends must be be marked as :handle as well: {2}", errorWr, cl.WhatKind, cl.Name, trait.Name);
          }
        }
        foreach (var member in cl.InheritedMembers.Concat(cl.Members)) {
          if (!member.IsGhost && !member.IsStatic) {
            Error(member.tok, "{0} '{1}' is marked as :handle, so all its non-static members must be ghost: {2}", errorWr, cl.WhatKind, cl.Name, member.Name);
          }
        }
      }
    }

    /// <summary>
    /// Check whether two declarations have the same name if capitalized.
    /// </summary>
    /// <param name="canChange">The declarations to check.</param>
    /// <param name="cantChange">Additional declarations which may conflict, but which can't be given different names.  For example, these may be the inherited members of a class.</param>
    /// <remarks>
    /// If two elements of <paramref name="canChange"/> have the same
    /// capitalization, the lowercase one will get a
    /// <c>{:_capitalizationConflict}</c> attribute.  If
    /// <paramref name="cantChange"/> is given and one of its elements conflicts
    /// with one from <paramref name="canChange"/>, the element from
    /// <paramref name="canChange"/> gets the attribute whether it is lowercase
    /// or not.
    /// </remarks>
    /// <seealso cref="HasCapitalizationConflict"/>
    private void CheckForCapitalizationConflicts<T>(IEnumerable<T> canChange, IEnumerable<T> cantChange = null) where T : Declaration {
      if (cantChange == null) {
        cantChange = Enumerable.Empty<T>();
      }
      IDictionary<string, T> declsByCapName = new Dictionary<string, T>();
      ISet<string> fixedNames = new HashSet<string>(from decl in cantChange select Capitalize(decl.CompileName));

      foreach (var decl in canChange) {
        var name = decl.CompileName;
        var capName = Capitalize(name);
        if (name == capName) {
          if (fixedNames.Contains(name)) {
            // Normally we mark the lowercase one, but in this case we can't change that one
            MarkCapitalizationConflict(decl);
          } else {
            T other;
            if (declsByCapName.TryGetValue(name, out other)) {
              // Presume that the other is the lowercase one
              MarkCapitalizationConflict(other);
            } else {
              declsByCapName.Add(name, decl);
            }
          }
        } else {
          if (declsByCapName.ContainsKey(capName)) {
            MarkCapitalizationConflict(decl);
          } else {
            declsByCapName.Add(capName, decl);
          }
        }
      }
    }

    protected string Capitalize(string str) {
      if (!str.Any(c => c != '_')) {
        return PrefixForForcedCapitalization + str;
      }
      var origStr = str;
      while (str.StartsWith("_")) {
        str = str.Substring(1) + "_";
      }
      if (!char.IsLetter(str[0])) {
        return PrefixForForcedCapitalization + origStr;
      } else {
        return char.ToUpper(str[0]) + str.Substring(1);
      }
    }

    protected virtual string PrefixForForcedCapitalization { get => "Cap_"; }

    private static void MarkCapitalizationConflict(Declaration decl) {
      decl.Attributes = new Attributes(CapitalizationConflictAttribute, new List<Expression>(), decl.Attributes);
    }

    protected static bool HasCapitalizationConflict(Declaration decl) {
      return Attributes.Contains(decl.Attributes, CapitalizationConflictAttribute);
    }

    private static string CapitalizationConflictAttribute = "_capitalizationConflict";

    private void CompileFunction(Function f, IClassWriter cw, bool lookasideBody) {
      Contract.Requires(f != null);
      Contract.Requires(cw != null);
      Contract.Requires(f.Body != null || Attributes.Contains(f.Attributes, "dllimport") || (IncludeExternMembers && Attributes.Contains(f.Attributes, "extern")));

      var w = cw.CreateFunction(IdName(f), CombineAllTypeArguments(f), f.Formals, f.ResultType, f.tok, f.IsStatic, !f.IsExtern(out _, out _), f, false, lookasideBody);
      if (w != null) {
        IVariable accVar = null;
        if (f.IsTailRecursive) {
          if (f.IsAccumulatorTailRecursive) {
            accVar = new LocalVariable(f.tok, f.tok, "_accumulator", f.ResultType, false) {
              type = f.ResultType
            };
            Expression unit;
            if (f.ResultType.IsNumericBased(Type.NumericPersuasion.Int) || f.ResultType.IsBigOrdinalType) {
              unit = new LiteralExpr(f.tok, f.TailRecursion == Function.TailStatus.Accumulate_Mul ? 1 : 0);
              unit.Type = f.ResultType;
            } else if (f.ResultType.IsNumericBased(Type.NumericPersuasion.Real)) {
              unit = new LiteralExpr(f.tok, f.TailRecursion == Function.TailStatus.Accumulate_Mul ? BigDec.FromInt(1) : BigDec.ZERO);
              unit.Type = f.ResultType;
            } else if (f.ResultType.IsBitVectorType) {
              var n = f.TailRecursion == Function.TailStatus.Accumulate_Mul ? 1 : 0;
              unit = new LiteralExpr(f.tok, n);
              unit.Type = f.ResultType;
            } else if (f.ResultType.AsSetType != null) {
              unit = new SetDisplayExpr(f.tok, !f.ResultType.IsISetType, new List<Expression>());
              unit.Type = f.ResultType;
            } else if (f.ResultType.AsMultiSetType != null) {
              unit = new MultiSetDisplayExpr(f.tok, new List<Expression>());
              unit.Type = f.ResultType;
            } else if (f.ResultType.AsSeqType != null) {
              unit = new SeqDisplayExpr(f.tok, new List<Expression>());
              unit.Type = f.ResultType;
            } else {
              Contract.Assert(false);  // unexpected type
              throw new cce.UnreachableException();
            }
            DeclareLocalVar(IdName(accVar), accVar.Type, f.tok, unit, false, w);
          }
          w = EmitTailCallStructure(f, w);
        }
        Coverage.Instrument(f.Body.tok, $"entry to function {f.FullName}", w);
        Contract.Assert(enclosingFunction == null);
        enclosingFunction = f;
        CompileReturnBody(f.Body, f.Original.ResultType, w, accVar);
        Contract.Assert(enclosingFunction == f);
        enclosingFunction = null;
      }
    }

    public const string STATIC_ARGS_NAME = "args";

    private void CompileMethod(Program program, Method m, IClassWriter cw, bool lookasideBody) {
      Contract.Requires(cw != null);
      Contract.Requires(m != null);
      Contract.Requires(m.Body != null || Attributes.Contains(m.Attributes, "dllimport") || (IncludeExternMembers && Attributes.Contains(m.Attributes, "extern")));

      var w = cw.CreateMethod(m, CombineAllTypeArguments(m), !m.IsExtern(out _, out _), false, lookasideBody);
      if (w != null) {
        if (m.IsTailRecursive) {
          w = EmitTailCallStructure(m, w);
        }

        Coverage.Instrument(m.Body.Tok, $"entry to method {m.FullName}", w);

        var nonGhostOutsCount = m.Outs.Count(p => !p.IsGhost);

        var useReturnStyleOuts = UseReturnStyleOuts(m, nonGhostOutsCount);
        foreach (var p in m.Outs) {
          if (!p.IsGhost) {
            DeclareLocalOutVar(IdName(p), p.Type, p.tok, PlaceboValue(p.Type, w, p.tok, true), useReturnStyleOuts, w);
          }
        }

        w = EmitMethodReturns(m, w);

        if (m.Body == null) {
          Error(m.tok, "Method {0} has no body", w, m.FullName);
        } else {
          Contract.Assert(enclosingMethod == null);
          enclosingMethod = m;
          TrStmtList(m.Body.Body, w);
          Contract.Assert(enclosingMethod == m);
          enclosingMethod = null;
        }
      }

      if (m == program.MainMethod && IssueCreateStaticMain(m)) {
        w = CreateStaticMain(cw, STATIC_ARGS_NAME);
        var ty = UserDefinedType.FromTopLevelDeclWithAllBooleanTypeParameters(m.EnclosingClass);
        LocalVariable receiver = null;
        if (!m.IsStatic) {
          receiver = new LocalVariable(m.tok, m.tok, "b", ty, false) {
            type = ty
          };
          if (m.EnclosingClass is ClassDecl) {
            var wStmts = w.Fork();
            var wRhs = DeclareLocalVar(IdName(receiver), ty, m.tok, w);
            EmitNew(ty, m.tok, null, wRhs, wStmts);
          } else {
            TrLocalVar(receiver, true, w);
          }
        }
        var typeArgs = CombineAllTypeArguments(m, ty.TypeArgs, m.TypeArgs.ConvertAll(tp => (Type)Type.Bool));
        bool customReceiver = !(m.EnclosingClass is TraitDecl) && NeedsCustomReceiver(m);

        if (receiver != null && !customReceiver) {
          w.Write("{0}.", IdName(receiver));
        } else {
          var companion = TypeName_Companion(UserDefinedType.FromTopLevelDeclWithAllBooleanTypeParameters(m.EnclosingClass), w, m.tok, m);
          w.Write("{0}.", companion);
        }
        EmitNameAndActualTypeArgs(IdName(m), TypeArgumentInstantiation.ToActuals(ForTypeParameters(typeArgs, m, false)), m.tok, w);
        w.Write("(");
        var sep = "";
        if (receiver != null && customReceiver) {
          w.Write("{0}", IdName(receiver));
          sep = ", ";
        }
        EmitTypeDescriptorsActuals(ForTypeDescriptors(typeArgs, m.EnclosingClass, m, false), m.tok, w, ref sep);
        w.Write(sep + STATIC_ARGS_NAME);
        w.Write(")");
        EndStmt(w);
      }
    }

    protected virtual bool IssueCreateStaticMain(Method m) {
      return !m.IsStatic || m.EnclosingClass.TypeArgs.Count != 0;
    }

    void TrCasePatternOpt<VT>(CasePattern<VT> pat, Expression rhs, ConcreteSyntaxTree wr, bool inLetExprBody)
      where VT : class, IVariable {
      TrCasePatternOpt(pat, rhs, null, rhs.Type, rhs.tok, wr, inLetExprBody);
    }

    void TrCasePatternOpt<VT>(CasePattern<VT> pat, Expression rhs, string rhs_string, Type rhsType, IToken rhsTok, ConcreteSyntaxTree wr, bool inLetExprBody)
      where VT : class, IVariable {
      Contract.Requires(pat != null);
      Contract.Requires(pat.Var != null || rhs != null || rhs_string != null);
      Contract.Requires(rhs != null || rhs_string != null);
      Contract.Requires(rhsType != null && rhsTok != null);

      if (pat.Var != null) {
        // The trivial Dafny "pattern" expression
        //    var x := G
        // is translated into C# as:
        // var x := G;
        var bv = pat.Var;
        if (!bv.IsGhost) {
          var wStmts = wr.Fork();
          var w = DeclareLocalVar(IdProtect(bv.CompileName), bv.Type, rhsTok, wr);
          if (rhs != null) {
            w = EmitCoercionIfNecessary(from: rhs.Type, to: bv.Type, tok: rhsTok, wr: w);
            w.Append(TrExpr(rhs, inLetExprBody, wStmts));
          } else {
            w.Write(rhs_string);
          }
        }
      } else if (pat.Arguments != null) {
        // The Dafny "pattern" expression
        //    var Pattern(x,y) := G
        // is translated into C# as:
        // var tmp := G;
        // var x := dtorX(tmp);
        // var y := dtorY(tmp);
        var ctor = pat.Ctor;
        Contract.Assert(ctor != null);  // follows from successful resolution
        Contract.Assert(pat.Arguments.Count == ctor.Formals.Count);  // follows from successful resolution

        // Create the temporary variable to hold G
        var tmp_name = ProtectedFreshId("_let_tmp_rhs");
        if (rhs != null) {
          DeclareLocalVar(tmp_name, rhs.Type, rhs.tok, rhs, inLetExprBody, wr);
        } else {
          DeclareLocalVar(tmp_name, rhsType, rhsTok, false, rhs_string, wr);
        }

        var dtv = (DatatypeValue)pat.Expr;
        var substMap = TypeParameter.SubstitutionMap(ctor.EnclosingDatatype.TypeArgs, dtv.InferredTypeArgs);
        var k = 0;  // number of non-ghost formals processed
        for (int i = 0; i < pat.Arguments.Count; i++) {
          var arg = pat.Arguments[i];
          var formal = ctor.Formals[i];
          if (formal.IsGhost) {
            // nothing to compile, but do a sanity check
            Contract.Assert(Contract.ForAll(arg.Vars, bv => bv.IsGhost));
          } else {
            var sw = new ConcreteSyntaxTree(wr.RelativeIndentLevel);
            EmitDestructor(tmp_name, formal, k, ctor, dtv.InferredTypeArgs, arg.Expr.Type, sw);
            Type targetType = formal.Type.Subst(substMap);
            TrCasePatternOpt(arg, null, sw.ToString(), targetType, pat.Expr.tok, wr, inLetExprBody);
            k++;
          }
        }
      }
    }

    void TrExprOpt(Expression expr, Type resultType, ConcreteSyntaxTree wr, IVariable/*?*/ accumulatorVar) {
      Contract.Requires(expr != null);
      Contract.Requires(wr != null);
      Contract.Requires(resultType != null);
      Contract.Requires(accumulatorVar == null || (enclosingFunction != null && enclosingFunction.IsAccumulatorTailRecursive));

      expr = expr.Resolved;
      if (expr is LetExpr) {
        var e = (LetExpr)expr;
        if (e.Exact) {
          for (int i = 0; i < e.LHSs.Count; i++) {
            var lhs = e.LHSs[i];
            if (Contract.Exists(lhs.Vars, bv => !bv.IsGhost)) {
              TrCasePatternOpt(lhs, e.RHSs[i], wr, false);
            }
          }
          TrExprOpt(e.Body, resultType, wr, accumulatorVar);
        } else {
          // We haven't optimized the other cases, so fallback to normal compilation
          EmitReturnExpr(e, resultType, false, wr);
        }

      } else if (expr is ITEExpr) {
        var e = (ITEExpr)expr;
        ConcreteSyntaxTree guardWriter;
        var wStmts = wr.Fork();
        var thn = EmitIf(out guardWriter, true, wr);
        guardWriter.Append(TrExpr(e.Test, false, wStmts));
        Coverage.Instrument(e.Thn.tok, "then branch", thn);
        TrExprOpt(e.Thn, resultType, thn, accumulatorVar);
        ConcreteSyntaxTree els = wr;
        if (!(e.Els is ITEExpr)) {
          els = EmitBlock(wr);
          Coverage.Instrument(e.Thn.tok, "else branch", els);
        }
        TrExprOpt(e.Els, resultType, els, accumulatorVar);

      } else if (expr is NestedMatchExpr nestedMatchExpr) {
        TrExprOpt(nestedMatchExpr.Flattened, resultType, wr, accumulatorVar);
      } else if (expr is MatchExpr) {
        var e = (MatchExpr)expr;
        //   var _source = E;
        //   if (source.is_Ctor0) {
        //     FormalType f0 = ((Dt_Ctor0)source._D).a0;
        //     ...
        //     return Body0;
        //   } else if (...) {
        //     ...
        //   } else if (true) {
        //     ...
        //   }
        string source = ProtectedFreshId("_source");
        DeclareLocalVar(source, e.Source.Type, e.Source.tok, e.Source, false, wr);

        if (e.Cases.Count == 0) {
          // the verifier would have proved we never get here; still, we need some code that will compile
          EmitAbsurd(null, wr);
        } else {
          int i = 0;
          var sourceType = (UserDefinedType)e.Source.Type.NormalizeExpand();
          foreach (MatchCaseExpr mc in e.Cases) {
            var w = MatchCasePrelude(source, sourceType, mc.Ctor, mc.Arguments, i, e.Cases.Count, wr);
            TrExprOpt(mc.Body, resultType, w, accumulatorVar);
            i++;
          }
        }

      } else if (expr is StmtExpr) {
        var e = (StmtExpr)expr;
        TrExprOpt(e.E, resultType, wr, accumulatorVar);

      } else if (expr is FunctionCallExpr fce && fce.Function == enclosingFunction && enclosingFunction.IsTailRecursive) {
        var e = fce;
        // compile call as tail-recursive

        // assign the actual in-parameters to temporary variables
        var inTmps = new List<string>();
        var inTypes = new List<Type/*?*/>();
        if (!e.Function.IsStatic) {
          string inTmp = ProtectedFreshId("_in");
          inTmps.Add(inTmp);
          inTypes.Add(null);
          DeclareLocalVar(inTmp, null, null, e.Receiver, false, wr);
        }
        for (int i = 0; i < e.Function.Formals.Count; i++) {
          Formal p = e.Function.Formals[i];
          if (!p.IsGhost) {
            string inTmp = ProtectedFreshId("_in");
            inTmps.Add(inTmp);
            inTypes.Add(e.Args[i].Type);
            DeclareLocalVar(inTmp, e.Args[i].Type, p.tok, e.Args[i], false, wr);
          }
        }
        // Now, assign to the formals
        int n = 0;
        if (!e.Function.IsStatic) {
          wr.Write("_this = ");
          ConcreteSyntaxTree wRHS;
          if (thisContext == null) {
            wRHS = wr;
          } else {
            var instantiatedType = e.Receiver.Type.Subst(thisContext.ParentFormalTypeParametersToActuals);
            wRHS = EmitCoercionIfNecessary(instantiatedType, UserDefinedType.FromTopLevelDecl(e.tok, thisContext), e.tok, wr);
          }
          wRHS.Write(inTmps[n]);
          EndStmt(wr);
          n++;
        }
        foreach (var p in e.Function.Formals) {
          if (!p.IsGhost) {
            EmitAssignment(IdName(p), p.Type, inTmps[n], inTypes[n], wr);
            n++;
          }
        }
        Contract.Assert(n == inTmps.Count);
        // finally, the jump back to the head of the function
        EmitJumpToTailCallStart(wr);

      } else if (expr is BinaryExpr bin
                 && bin.AccumulatesForTailRecursion != BinaryExpr.AccumulationOperand.None
                 && enclosingFunction is { IsAccumulatorTailRecursive: true }) {
        Contract.Assert(accumulatorVar != null);
        Expression tailTerm;
        Expression rhs;
        var acc = new IdentifierExpr(expr.tok, accumulatorVar);
        if (bin.AccumulatesForTailRecursion == BinaryExpr.AccumulationOperand.Left) {
          rhs = new BinaryExpr(bin.tok, bin.ResolvedOp, acc, bin.E0);
          tailTerm = bin.E1;
        } else {
          switch (bin.ResolvedOp) {
            case BinaryExpr.ResolvedOpcode.Sub:
              rhs = new BinaryExpr(bin.tok, BinaryExpr.ResolvedOpcode.Add, bin.E1, acc);
              break;
            case BinaryExpr.ResolvedOpcode.SetDifference:
              rhs = new BinaryExpr(bin.tok, BinaryExpr.ResolvedOpcode.Union, bin.E1, acc);
              break;
            case BinaryExpr.ResolvedOpcode.MultiSetDifference:
              rhs = new BinaryExpr(bin.tok, BinaryExpr.ResolvedOpcode.MultiSetUnion, bin.E1, acc);
              break;
            default:
              rhs = new BinaryExpr(bin.tok, bin.ResolvedOp, bin.E1, acc);
              break;
          }
          tailTerm = bin.E0;
        }
        ConcreteSyntaxTree wLhs, wRhs;
        var wStmts = wr.Fork();
        EmitAssignment(out wLhs, enclosingFunction.ResultType, out wRhs, enclosingFunction.ResultType, wr);
        wLhs.Append(TrExpr(acc, false, wStmts));
        wRhs.Append(TrExpr(rhs, false, wStmts));
        TrExprOpt(tailTerm, resultType, wr, accumulatorVar);

      } else {
        // We haven't optimized any other cases, so fallback to normal compilation
        if (enclosingFunction != null && enclosingFunction.IsAccumulatorTailRecursive) {
          // Remember to include the accumulator
          Contract.Assert(accumulatorVar != null);
          var acc = new IdentifierExpr(expr.tok, accumulatorVar);
          switch (enclosingFunction.TailRecursion) {
            case Function.TailStatus.Accumulate_Add:
              expr = new BinaryExpr(expr.tok, BinaryExpr.ResolvedOpcode.Add, expr, acc);
              break;
            case Function.TailStatus.AccumulateRight_Sub:
              expr = new BinaryExpr(expr.tok, BinaryExpr.ResolvedOpcode.Sub, expr, acc);
              break;
            case Function.TailStatus.Accumulate_Mul:
              expr = new BinaryExpr(expr.tok, BinaryExpr.ResolvedOpcode.Mul, expr, acc);
              break;
            case Function.TailStatus.Accumulate_SetUnion:
              expr = new BinaryExpr(expr.tok, BinaryExpr.ResolvedOpcode.Union, expr, acc);
              break;
            case Function.TailStatus.AccumulateRight_SetDifference:
              expr = new BinaryExpr(expr.tok, BinaryExpr.ResolvedOpcode.SetDifference, expr, acc);
              break;
            case Function.TailStatus.Accumulate_MultiSetUnion:
              expr = new BinaryExpr(expr.tok, BinaryExpr.ResolvedOpcode.MultiSetUnion, expr, acc);
              break;
            case Function.TailStatus.AccumulateRight_MultiSetDifference:
              expr = new BinaryExpr(expr.tok, BinaryExpr.ResolvedOpcode.MultiSetDifference, expr, acc);
              break;
            case Function.TailStatus.AccumulateLeft_Concat:
              expr = new BinaryExpr(expr.tok, BinaryExpr.ResolvedOpcode.Concat, acc, expr); // note order of operands
              break;
            case Function.TailStatus.AccumulateRight_Concat:
              expr = new BinaryExpr(expr.tok, BinaryExpr.ResolvedOpcode.Concat, expr, acc);
              break;
            default:
              Contract.Assert(false); // unexpected TailStatus
              throw new cce.UnreachableException();
          }
        } else {
          Contract.Assert(accumulatorVar == null);
        }
        EmitReturnExpr(expr, resultType, false, wr);
      }
    }

    void CompileReturnBody(Expression body, Type originalResultType, ConcreteSyntaxTree wr, IVariable/*?*/ accumulatorVar) {
      Contract.Requires(body != null);
      Contract.Requires(originalResultType != null);
      Contract.Requires(wr != null);
      Contract.Requires(accumulatorVar == null || (enclosingFunction != null && enclosingFunction.IsAccumulatorTailRecursive));
      copyInstrWriters.Push(wr.Fork());
      TrExprOpt(body.Resolved, originalResultType, wr, accumulatorVar);
      copyInstrWriters.Pop();
    }

    // ----- Type ---------------------------------------------------------------------------------

    protected NativeType AsNativeType(Type typ) {
      Contract.Requires(typ != null);
      if (typ.AsNewtype != null) {
        return typ.AsNewtype.NativeType;
      } else if (typ.IsBitVectorType) {
        return typ.AsBitVectorType.NativeType;
      }
      return null;
    }

    protected bool NeedsEuclideanDivision(Type typ) {
      if (AsNativeType(typ) is { LowerBound: var lb }) {
        // Dafny's division differs from '/' only on negative numbers
        return lb < BigInteger.Zero;
      }
      // IsNumericBased drills past newtypes, unlike IsIntegerType
      return typ.IsNumericBased(Type.NumericPersuasion.Int);
    }

    /// <summary>
    /// Note, C# and Java reverse the order of brackets in array type names.
    /// </summary>
    protected void TypeName_SplitArrayName(Type type, ConcreteSyntaxTree wr, IToken tok, out string typeNameSansBrackets, out string brackets) {
      Contract.Requires(type != null);

      TypeName_SplitArrayName(type, out var innermostElementType, out brackets);
      typeNameSansBrackets = TypeName(innermostElementType, wr, tok);
    }

    protected virtual void TypeName_SplitArrayName(Type type, out Type innermostElementType, out string brackets) {
      Contract.Requires(type != null);

      type = DatatypeWrapperEraser.SimplifyType(type);
      var at = type.AsArrayType;
      if (at != null) {
        var elementType = type.TypeArgs[0];
        TypeName_SplitArrayName(elementType, out innermostElementType, out brackets);
        brackets = TypeNameArrayBrackets(at.Dims) + brackets;
      } else {
        innermostElementType = type;
        brackets = "";
      }
    }

    protected virtual string TypeNameArrayBrackets(int dims) {
      Contract.Requires(0 <= dims);
      return $"[{Util.Repeat(dims - 1, ",")}]";
    }

    protected bool ComplicatedTypeParameterForCompilation(TypeParameter.TPVariance v, Type t) {
      Contract.Requires(t != null);
      return v != TypeParameter.TPVariance.Non && t.IsTraitType;
    }

    protected string/*!*/ TypeNames(List<Type/*!*/>/*!*/ types, ConcreteSyntaxTree wr, IToken tok) {
      Contract.Requires(cce.NonNullElements(types));
      Contract.Ensures(Contract.Result<string>() != null);
      return Util.Comma(types, ty => TypeName(ty, wr, tok));
    }

    /// <summary>
    /// If "type" is an auto-init type, then return a default value, else return a placebo value.
    /// </summary>
    protected string PlaceboValue(Type type, ConcreteSyntaxTree wr, IToken tok, bool constructTypeParameterDefaultsFromTypeDescriptors = false) {
      if (type.HasCompilableValue) {
        return DefaultValue(type, wr, tok, constructTypeParameterDefaultsFromTypeDescriptors);
      } else {
        return ForcePlaceboValue(type, wr, tok, constructTypeParameterDefaultsFromTypeDescriptors);
      }
    }

    protected string ForcePlaceboValue(Type type, ConcreteSyntaxTree wr, IToken tok, bool constructTypeParameterDefaultsFromTypeDescriptors = false) {
      Contract.Requires(type != null);
      Contract.Requires(wr != null);
      Contract.Requires(tok != null);
      Contract.Ensures(Contract.Result<string>() != null);

      type = DatatypeWrapperEraser.SimplifyType(type, true);
      return TypeInitializationValue(type, wr, tok, true, constructTypeParameterDefaultsFromTypeDescriptors);
    }

    protected string DefaultValue(Type type, ConcreteSyntaxTree wr, IToken tok, bool constructTypeParameterDefaultsFromTypeDescriptors = false) {
      Contract.Requires(type != null);
      Contract.Requires(wr != null);
      Contract.Requires(tok != null);
      Contract.Ensures(Contract.Result<string>() != null);

      // If "type" is a datatype with a ghost grounding constructor, then compile as a placebo for DatatypeWrapperEraser.SimplifyType(type, true).
      // Otherwise, get default value for DatatypeWrapperEraser.SimplifyType(type, true), which may itself have a ghost grounding constructor, in
      // which case the value we produce is a placebo.
      bool HasGhostGroundingCtor(Type ty) {
        return (ty.NormalizeExpandKeepConstraints() as UserDefinedType)?.ResolvedClass is DatatypeDecl dt && dt.GetGroundingCtor().IsGhost;
      }

      var simplifiedType = DatatypeWrapperEraser.SimplifyType(type, true);
      var usePlaceboValue = HasGhostGroundingCtor(type) || HasGhostGroundingCtor(simplifiedType);
      return TypeInitializationValue(simplifiedType, wr, tok, usePlaceboValue, constructTypeParameterDefaultsFromTypeDescriptors);
    }

    // ----- Stmt ---------------------------------------------------------------------------------

    void TrStmtNonempty(Statement stmt, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts = null) {
      Contract.Requires(stmt != null);
      Contract.Requires(wr != null);
      TrStmt(stmt, wr, wStmts);
      if (stmt.IsGhost) {
        TrStmtList(new List<Statement>(), EmitBlock(wr));
      }
    }

    protected internal void TrStmt(Statement stmt, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts = null) {
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
          // multi-assignment
          Contract.Assert(s.Lhss.Count == resolved.Count);
          Contract.Assert(s.Rhss.Count == resolved.Count);
          var lhsTypes = new List<Type>();
          var rhsTypes = new List<Type>();
          var lhss = new List<Expression>();
          var rhss = new List<AssignmentRhs>();
          for (int i = 0; i < resolved.Count; i++) {
            if (!resolved[i].IsGhost) {
              var lhs = s.Lhss[i];
              var rhs = s.Rhss[i];
              if (rhs is HavocRhs) {
                if (DafnyOptions.O.ForbidNondeterminism) {
                  Error(rhs.Tok, "nondeterministic assignment forbidden by the --enforce-determinism option", wr);
                }
              } else {
                lhss.Add(lhs);
                lhsTypes.Add(lhs.Type);
                rhss.Add(rhs);
                rhsTypes.Add(TypeOfRhs(rhs));
              }
            }
          }

          var wStmtsPre = wStmts.Fork();
          var lvalues = new List<ILvalue>();
          foreach (Expression lhs in lhss) {
            lvalues.Add(CreateLvalue(lhs, wStmts, wStmtsPre));
          }
          List<ConcreteSyntaxTree> wRhss;
          EmitMultiAssignment(lhss, lvalues, lhsTypes, out wRhss, rhsTypes, wr);
          for (int i = 0; i < wRhss.Count; i++) {
            TrRhs(rhss[i], wRhss[i], wStmts);
          }
        }
      } else if (stmt is AssignStmt) {
        var s = (AssignStmt)stmt;
        Contract.Assert(s.Lhs is not SeqSelectExpr expr || expr.SelectOne);  // multi-element array assignments are not allowed
        if (s.Rhs is HavocRhs) {
          if (DafnyOptions.O.ForbidNondeterminism) {
            Error(s.Rhs.Tok, "nondeterministic assignment forbidden by the --enforce-determinism option", wr);
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
        if (DafnyOptions.O.ForbidNondeterminism) {
          Error(s.Tok, "assign-such-that statement forbidden by the --enforce-determinism option", wr);
        }
        var lhss = s.Lhss.ConvertAll(lhs => ((IdentifierExpr)lhs.Resolved).Var);  // the resolver allows only IdentifierExpr left-hand sides
        var missingBounds = ComprehensionExpr.BoundedPool.MissingBounds(lhss, s.Bounds, ComprehensionExpr.BoundedPool.PoolVirtues.Enumerable);
        if (missingBounds.Count != 0) {
          foreach (var bv in missingBounds) {
            Error(s.Tok, "this assign-such-that statement is too advanced for the current compiler; Dafny's heuristics cannot find any bound for variable '{0}'", wr, bv.Name);
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
        ConcreteSyntaxTree guardWriter;
        ConcreteSyntaxTree bodyWriter = EmitIf(out guardWriter, false, wr);
        var negated = new UnaryOpExpr(s.Tok, UnaryOpExpr.Opcode.Not, s.Expr);
        negated.Type = Type.Bool;
        guardWriter.Append(TrExpr(negated, false, wStmts));
        EmitHalt(s.Tok, s.Message, bodyWriter);

      } else if (stmt is CallStmt) {
        var s = (CallStmt)stmt;
        TrCallStmt(s, null, wr);

      } else if (stmt is BlockStmt) {
        var w = EmitBlock(wr);
        TrStmtList(((BlockStmt)stmt).Body, w);

      } else if (stmt is IfStmt) {
        IfStmt s = (IfStmt)stmt;
        if (s.Guard == null) {
          if (DafnyOptions.O.ForbidNondeterminism) {
            Error(s.Tok, "nondeterministic if statement forbidden by the --enforce-determinism option", wr);
          }
          // we can compile the branch of our choice
          ConcreteSyntaxTree guardWriter;
          if (s.Els == null) {
            // let's compile the "else" branch, since that involves no work
            // (still, let's leave a marker in the source code to indicate that this is what we did)
            Coverage.UnusedInstrumentationPoint(s.Thn.Tok, "then branch");
            var notFalse = (UnaryOpExpr)Expression.CreateNot(s.Thn.Tok, new LiteralExpr(s.Thn.Tok, false));
            var thenWriter = EmitIf(out guardWriter, false, wr);
            EmitUnaryExpr(ResolvedUnaryOp.BoolNot, notFalse.E, false, guardWriter, wStmts);
            Coverage.Instrument(s.Tok, "implicit else branch", wr);
            thenWriter = EmitIf(out guardWriter, false, thenWriter);
            EmitUnaryExpr(ResolvedUnaryOp.BoolNot, notFalse.E, false, guardWriter, wStmts);
            TrStmtList(new List<Statement>(), thenWriter);
          } else {
            // let's compile the "then" branch
            wr = EmitIf(out guardWriter, false, wr);
            guardWriter.Write(True);
            Coverage.Instrument(s.Thn.Tok, "then branch", wr);
            TrStmtList(s.Thn.Body, wr);
            Coverage.UnusedInstrumentationPoint(s.Els.Tok, "else branch");
          }
        } else {
          if (s.IsBindingGuard && DafnyOptions.O.ForbidNondeterminism) {
            Error(s.Tok, "binding if statement forbidden by the --enforce-determinism option", wr);
          }
          ConcreteSyntaxTree guardWriter;
          var coverageForElse = Coverage.IsRecording && !(s.Els is IfStmt);
          var thenWriter = EmitIf(out guardWriter, s.Els != null || coverageForElse, wr);
          guardWriter.Append(TrExpr(s.IsBindingGuard ? Translator.AlphaRename((ExistsExpr)s.Guard, "eg_d") : s.Guard, false, wStmts));
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
        if (DafnyOptions.O.ForbidNondeterminism && 2 <= s.Alternatives.Count) {
          Error(s.Tok, "case-based if statement forbidden by the --enforce-determinism option", wr);
        }
        foreach (var alternative in s.Alternatives) {
          ConcreteSyntaxTree guardWriter;
          var thn = EmitIf(out guardWriter, true, wr);
          guardWriter.Append(TrExpr(alternative.IsBindingGuard ? Translator.AlphaRename((ExistsExpr)alternative.Guard, "eg_d") : alternative.Guard, false, wStmts));
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
          if (DafnyOptions.O.ForbidNondeterminism) {
            Error(s.Tok, "nondeterministic loop forbidden by the --enforce-determinism option", wr);
          }
          // This loop is allowed to stop iterating at any time. We choose to never iterate, but we still
          // emit a loop structure. The structure "while (false) { }" comes to mind, but that results in
          // an "unreachable code" error from Java, so we instead use "while (true) { break; }".
          ConcreteSyntaxTree guardWriter;
          var wBody = CreateWhileLoop(out guardWriter, wr);
          guardWriter.Write(True);
          EmitBreak(null, wBody);
          Coverage.UnusedInstrumentationPoint(s.Body.Tok, "while body");
        } else {
          var guardWriter = EmitWhile(s.Body.Tok, s.Body.Body, s.Labels, wr);
          guardWriter.Append(TrExpr(s.Guard, false, wStmts));
        }

      } else if (stmt is AlternativeLoopStmt loopStmt) {
        if (DafnyOptions.O.ForbidNondeterminism) {
          Error(loopStmt.Tok, "case-based loop forbidden by the --enforce-determinism option", wr);
        }
        if (loopStmt.Alternatives.Count != 0) {
          ConcreteSyntaxTree whileGuardWriter;
          var w = CreateWhileLoop(out whileGuardWriter, wr);
          whileGuardWriter.Write(True);
          w = EmitContinueLabel(loopStmt.Labels, w);
          foreach (var alternative in loopStmt.Alternatives) {
            ConcreteSyntaxTree guardWriter;
            var thn = EmitIf(out guardWriter, true, w);
            guardWriter.Append(TrExpr(alternative.Guard, false, wStmts));
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
          wr.Write(GenerateLhsDecl(endVarName, s.End.Type, wr, s.End.tok));
          EmitAssignmentRhs(s.End, false, wr);
        }
        var startExprWriter = EmitForStmt(s.Tok, s.LoopIndex, s.GoingUp, endVarName, s.Body.Body, s.Labels, wr);
        startExprWriter.Append(TrExpr(s.Start, false, wStmts));

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
          if (DafnyOptions.O.ForbidNondeterminism) {
            Error(s0.Rhs.Tok, "nondeterministic assignment forbidden by --enforce-determinism", wr);
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
              Error(lhs.tok, "compiler currently does not support assignments to more-than-6-dimensional arrays in forall statements", wr);
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
          ConcreteSyntaxTree collWriter;
          TargetTupleSize = L;
          wr = CreateForeachIngredientLoop(tup, L, tupleTypeArgs, out collWriter, wrOuter);
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
      } else if (stmt is MatchStmt) {
        MatchStmt s = (MatchStmt)stmt;
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
        } else if (DafnyOptions.O.ForbidNondeterminism) {
          Error(s.Tok, "modify statement without a body forbidden by the --enforce-determinism option", wr);
        }
      } else if (stmt is TryRecoverStatement h) {
        EmitHaltRecoveryStmt(h.TryBody, h.HaltMessageVar.CompileName, h.RecoverBody, wr);
      } else {
        Contract.Assert(false); throw new cce.UnreachableException();  // unexpected statement
      }
    }

    protected virtual ConcreteSyntaxTree EmitIngredients(ConcreteSyntaxTree wr, string ingredients, int L, string tupleTypeArgs, ForallStmt s, AssignStmt s0, Expression rhs) {
      var wStmts = wr.Fork();
      var wrVarInit = DeclareLocalVar(ingredients, null, null, wr);
      {
        EmitEmptyTupleList(tupleTypeArgs, wrVarInit);
      }

      var wrOuter = wr;
      wr = CompileGuardedLoops(s.BoundVars, s.Bounds, s.Range, wr);

      var wrTuple = EmitAddTupleToList(ingredients, tupleTypeArgs, wr);
      {
        if (s0.Lhs is MemberSelectExpr) {
          var lhs = (MemberSelectExpr)s0.Lhs;
          wrTuple.Append(TrExpr(lhs.Obj, false, wStmts));
        } else if (s0.Lhs is SeqSelectExpr) {
          var lhs = (SeqSelectExpr)s0.Lhs;
          wrTuple.Append(TrExpr(lhs.Seq, false, wStmts));
          wrTuple.Write(", ");
          EmitExprAsInt(lhs.E0, false, wrTuple, wStmts);
        } else {
          var lhs = (MultiSelectExpr)s0.Lhs;
          wrTuple.Append(TrExpr(lhs.Array, false, wStmts));
          for (int i = 0; i < lhs.Indices.Count; i++) {
            wrTuple.Write(", ");
            EmitExprAsInt(lhs.Indices[i], false, wrTuple, wStmts);
          }
        }

        wrTuple.Write(", ");
        wrTuple.Append(TrExpr(rhs, false, wStmts));
      }

      return wrOuter;
    }

    bool IsTailRecursiveByMethodCall(FunctionCallExpr fce) {
      Contract.Requires(fce != null);
      return fce.IsByMethodCall && fce.Function.ByMethodDecl == enclosingMethod && fce.Function.ByMethodDecl.IsTailRecursive;
    }

    protected virtual void EmitMemberSelect(AssignStmt s0, List<Type> tupleTypeArgsList, ConcreteSyntaxTree wr, string tup) {
      var lhs = (MemberSelectExpr)s0.Lhs;

      var typeArgs = TypeArgumentInstantiation.ListFromMember(lhs.Member, null, lhs.TypeApplication_JustMember);
      var lvalue = EmitMemberSelect(w => {
        var wObj = EmitCoercionIfNecessary(from: null, to: tupleTypeArgsList[0], s0.Tok, w);
        EmitTupleSelect(tup, 0, wObj);
      }, lhs.Obj.Type, lhs.Member, typeArgs, lhs.TypeArgumentSubstitutionsWithParents(), lhs.Type);

      var wRhs = EmitAssignment(lvalue, lhs.Type, tupleTypeArgsList[1], wr, s0.Tok);
      var wCoerced = EmitCoercionIfNecessary(from: null, to: tupleTypeArgsList[1], tok: s0.Tok, wr: wRhs);
      EmitTupleSelect(tup, 1, wCoerced);
    }

    protected virtual void EmitSeqSelect(AssignStmt s0, List<Type> tupleTypeArgsList, ConcreteSyntaxTree wr, string tup) {
      var lhs = (SeqSelectExpr)s0.Lhs;
      ConcreteSyntaxTree wColl, wIndex, wValue;
      EmitIndexCollectionUpdate(out wColl, out wIndex, out wValue, wr, nativeIndex: true);
      var wCoerce = EmitCoercionIfNecessary(from: null, to: lhs.Seq.Type, tok: s0.Tok, wr: wColl);
      EmitTupleSelect(tup, 0, wCoerce);
      var wCast = EmitCoercionToNativeInt(wIndex);
      EmitTupleSelect(tup, 1, wCast);
      EmitTupleSelect(tup, 2, wValue);
      EndStmt(wr);
    }

    protected virtual void EmitMultiSelect(AssignStmt s0, List<Type> tupleTypeArgsList, ConcreteSyntaxTree wr, string tup, int L) {
      var lhs = (MultiSelectExpr)s0.Lhs;
      var wArray = new ConcreteSyntaxTree(wr.RelativeIndentLevel);
      var wCoerced = EmitCoercionIfNecessary(from: null, to: tupleTypeArgsList[0], tok: s0.Tok, wr: wArray);
      EmitTupleSelect(tup, 0, wCoerced);
      var array = wArray.ToString();
      var indices = new List<string>();
      for (int i = 0; i < lhs.Indices.Count; i++) {
        var wIndex = new ConcreteSyntaxTree();
        EmitTupleSelect(tup, i + 1, wIndex);
        indices.Add(wIndex.ToString());
      }
      var lvalue = EmitArraySelectAsLvalue(array, indices, tupleTypeArgsList[L - 1]);
      var wRhs = lvalue.EmitWrite(wr);
      EmitTupleSelect(tup, L - 1, wRhs);
      EndStmt(wr);
    }

    protected ConcreteSyntaxTree CompileGuardedLoops(List<BoundVar> bvs, List<ComprehensionExpr.BoundedPool> bounds, Expression range, ConcreteSyntaxTree wr) {
      var n = bvs.Count;
      Contract.Assert(bounds.Count == n);
      for (int i = 0; i < n; i++) {
        var bound = bounds[i];
        var bv = bvs[i];
        var tmpVar = ProtectedFreshId("_guard_loop_");
        var wStmtsLoop = wr.Fork();
        var elementType = CompileCollection(bound, bv, false, false, null, out var collection, wStmtsLoop, bounds, bvs, i);
        wr = CreateGuardedForeachLoop(tmpVar, elementType, bv, true, false, range.tok, collection, wr);
      }

      // if (range) {
      //   ingredients.Add(new L-Tuple( LHS0(w,x,y,z), LHS1(w,x,y,z), ..., RHS(w,x,y,z) ));
      // }
      ConcreteSyntaxTree guardWriter = new ConcreteSyntaxTree();
      var wStmts = guardWriter.Fork();
      wr = EmitIf(out guardWriter, false, wr);
      foreach (var bvConstraints in bvs.Select(bv => Resolver.GetImpliedTypeConstraint(bv, bv.Type))) {
        TrParenExpr(bvConstraints, guardWriter, false, wStmts);
        guardWriter.Write($" {Conj} ");
      }
      TrParenExpr(range, guardWriter, false, wStmts);

      return wr;
    }

    protected Type CompileCollection(ComprehensionExpr.BoundedPool bound, IVariable bv, bool inLetExprBody, bool includeDuplicates,
        Substituter/*?*/ su, out ConcreteSyntaxTree collectionWriter, ConcreteSyntaxTree wStmts,
        List<ComprehensionExpr.BoundedPool>/*?*/ bounds = null, List<BoundVar>/*?*/ boundVars = null, int boundIndex = 0) {
      Contract.Requires(bound != null);
      Contract.Requires(bounds == null || (boundVars != null && bounds.Count == boundVars.Count && 0 <= boundIndex && boundIndex < bounds.Count));

      collectionWriter = new ConcreteSyntaxTree();

      var propertySuffix = SupportsProperties ? "" : "()";
      su = su ?? new Substituter(null, new Dictionary<IVariable, Expression>(), new Dictionary<TypeParameter, Type>());

      if (bound is ComprehensionExpr.BoolBoundedPool) {
        collectionWriter.Write("{0}.AllBooleans()", GetHelperModuleName());
        return new BoolType();
      } else if (bound is ComprehensionExpr.CharBoundedPool) {
        collectionWriter.Write($"{GetHelperModuleName()}.All{CharMethodQualifier}Chars()");
        return new CharType();
      } else if (bound is ComprehensionExpr.IntBoundedPool) {
        var b = (ComprehensionExpr.IntBoundedPool)bound;
        var type = EmitIntegerRange(bv.Type, out var wLo, out var wHi, collectionWriter);
        if (b.LowerBound == null) {
          EmitNull(bv.Type, wLo);
        } else if (bounds != null) {
          var low = SubstituteBound(b, bounds, boundVars, boundIndex, true);
          wLo.Append(TrExpr(su.Substitute(low), inLetExprBody, wStmts));
        } else {
          wLo.Append(TrExpr(su.Substitute(b.LowerBound), inLetExprBody, wStmts));
        }
        if (b.UpperBound == null) {
          EmitNull(bv.Type, wHi);
        } else if (bounds != null) {
          var high = SubstituteBound(b, bounds, boundVars, boundIndex, false);
          wHi.Append(TrExpr(su.Substitute(high), inLetExprBody, wStmts));
        } else {
          wHi.Append(TrExpr(su.Substitute(b.UpperBound), inLetExprBody, wStmts));
        }
        return type;
      } else if (bound is AssignSuchThatStmt.WiggleWaggleBound) {
        collectionWriter.Write("{0}.AllIntegers()", GetHelperModuleName());
        return bv.Type;
      } else if (bound is ComprehensionExpr.ExactBoundedPool) {
        var b = (ComprehensionExpr.ExactBoundedPool)bound;
        EmitSingleValueGenerator(su.Substitute(b.E), inLetExprBody, TypeName(b.E.Type, collectionWriter, b.E.tok), collectionWriter, wStmts);
        return b.E.Type;
      } else if (bound is ComprehensionExpr.SetBoundedPool setBoundedPool) {
        TrParenExpr(su.Substitute(setBoundedPool.Set), collectionWriter, inLetExprBody, wStmts);
        collectionWriter.Write(".Elements" + propertySuffix);
        return setBoundedPool.CollectionElementType;
      } else if (bound is ComprehensionExpr.MultiSetBoundedPool) {
        var b = (ComprehensionExpr.MultiSetBoundedPool)bound;
        TrParenExpr(su.Substitute(b.MultiSet), collectionWriter, inLetExprBody, wStmts);
        collectionWriter.Write((includeDuplicates ? ".Elements" : ".UniqueElements") + propertySuffix);
        return b.CollectionElementType;
      } else if (bound is ComprehensionExpr.SubSetBoundedPool) {
        var b = (ComprehensionExpr.SubSetBoundedPool)bound;
        TrParenExpr(su.Substitute(b.UpperBound), collectionWriter, inLetExprBody, wStmts);
        collectionWriter.Write(".AllSubsets" + propertySuffix);
        return b.UpperBound.Type;
      } else if (bound is ComprehensionExpr.MapBoundedPool) {
        var b = (ComprehensionExpr.MapBoundedPool)bound;
        TrParenExpr(su.Substitute(b.Map), collectionWriter, inLetExprBody, wStmts);
        GetSpecialFieldInfo(SpecialField.ID.Keys, null, null, out var keyName, out _, out _);
        collectionWriter.Write($".{keyName}.Elements{propertySuffix}");
        return b.CollectionElementType;
      } else if (bound is ComprehensionExpr.SeqBoundedPool) {
        var b = (ComprehensionExpr.SeqBoundedPool)bound;
        TrParenExpr(su.Substitute(b.Seq), collectionWriter, inLetExprBody, wStmts);
        collectionWriter.Write((includeDuplicates ? ".Elements" : ".UniqueElements") + propertySuffix);
        return b.CollectionElementType;
      } else if (bound is ComprehensionExpr.DatatypeBoundedPool) {
        var b = (ComprehensionExpr.DatatypeBoundedPool)bound;
        collectionWriter.Write("{0}.AllSingletonConstructors{1}", TypeName_Companion(bv.Type, collectionWriter, bv.Tok, null), propertySuffix);
        return new UserDefinedType(bv.Tok, new NameSegment(bv.Tok, b.Decl.Name, new())) {
          ResolvedClass = b.Decl
        };
      } else {
        Contract.Assert(false); throw new cce.UnreachableException();  // unexpected BoundedPool type
      }
    }

    private Expression SubstituteBound(ComprehensionExpr.IntBoundedPool b, List<ComprehensionExpr.BoundedPool> bounds, List<BoundVar> boundVars, int index, bool lowBound) {
      Contract.Requires(b != null);
      Contract.Requires((lowBound ? b.LowerBound : b.UpperBound) != null);
      Contract.Requires(bounds != null);
      Contract.Requires(boundVars != null);
      Contract.Requires(bounds.Count == boundVars.Count);
      Contract.Requires(0 <= index && index < boundVars.Count);
      // if the outer bound is dependent on the inner boundvar, we need to
      // substitute the inner boundvar with its bound.
      var bnd = lowBound ? b.LowerBound : b.UpperBound;
      var sm = new Dictionary<IVariable, Expression>();
      for (int i = index + 1; i < boundVars.Count; i++) {
        var bound = bounds[i];
        if (bound is ComprehensionExpr.IntBoundedPool) {
          var ib = (ComprehensionExpr.IntBoundedPool)bound;
          var bv = boundVars[i];
          sm[bv] = lowBound ? ib.LowerBound : ib.UpperBound;
        }
      }
      var su = new Substituter(null, sm, new Dictionary<TypeParameter, Type>());
      return su.Substitute(bnd);
    }

    private void IntroduceAndAssignBoundVars(ExistsExpr exists, ConcreteSyntaxTree wr) {
      Contract.Requires(exists != null);
      Contract.Assume(exists.Bounds != null);  // follows from successful resolution
      Contract.Assert(exists.Range == null);  // follows from invariant of class IfStmt
      foreach (var bv in exists.BoundVars) {
        TrLocalVar(bv, false, wr);
      }
      var ivars = exists.BoundVars.ConvertAll(bv => (IVariable)bv);
      TrAssignSuchThat(ivars, exists.Term, exists.Bounds, exists.tok.line, wr, false);
    }

    private bool CanSequentializeForall(List<BoundVar> bvs, List<ComprehensionExpr.BoundedPool> bounds, Expression range, Expression lhs, Expression rhs) {
      // Given a statement
      //
      //   forall i, ... | R {
      //     L := E;
      //   }
      //
      // we sequentialize if all of these conditions hold:
      //
      //   1. There are no calls to functions which may have read effects in R,
      //      L, or E
      //   2. Each index value will be produced only once (note that this is
      //      currently always true thanks to the use of UniqueElements())
      //   3. If L has the form A[I] for some A and I, then one of the following
      //      is true:
      //      a. There are no array dereferences or array-to-sequence
      //         conversions in R, A, I, or E
      //      b. All of the following are true:
      //         i.   There is only one bound variable; call it i
      //         ii.  I is the variable i
      //         iii. Each array dereference in R, A, or E has the form B[i] for
      //              some B
      //         iv.  There are no array-to-sequence conversions in R, A, or E
      //   4. If L has the form A[I, J, ...] for some A, I, J, ... then there
      //      are no multi-D array dereferences in R, A, E, or any of the
      //      indices I, J, ...
      //   5. If L has the form O.f for some field f, then one of the following
      //      is true:
      //      a. There are no accesses of f in R, O, or E
      //      b. All of the following are true:
      //         i.   There is only one bound variable; call it i
      //         ii.  O is the variable i
      //         iii. Each access of f in R or E has the form i.f
      //
      // TODO It may be possible to weaken rule 4 by adding an alternative
      // similar to rules 3b and 5b.
      Contract.Assert(bvs.Count == bounds.Count);

      if (!noImpureFunctionCalls(lhs, rhs, range)) {
        return false;
      } else if (lhs is SeqSelectExpr sse) {
        return
          no1DArrayAccesses(sse.Seq, sse.E0, range, rhs) ||

          (bvs.Count == 1 &&
          isVar(bvs[0], sse.E0) &&
          indexIsAlwaysVar(bvs[0], range, sse.Seq, rhs)); // also covers sequence conversions
      } else if (lhs is MultiSelectExpr mse) {
        return
          noMultiDArrayAccesses(mse.Array, range, rhs) &&
          noMultiDArrayAccesses(mse.Indices.ToArray());
      } else {
        // !@#$#@$% scope rules won't let me call this mse ...
        var mse2 = (MemberSelectExpr)lhs;

        return
          noFieldAccesses(mse2.Member, mse2.Obj, range, rhs) ||

          (bvs.Count == 1 &&
          isVar(bvs[0], mse2.Obj) &&
          accessedObjectIsAlwaysVar(mse2.Member, bvs[0], range, rhs));
      }

      bool noImpureFunctionCalls(params Expression[] exprs) {
        return exprs.All(e => Check<ApplyExpr>(e, ae => {
          var ty = (UserDefinedType)ae.Function.Type.NormalizeExpandKeepConstraints();
          return ArrowType.IsPartialArrowTypeName(ty.Name) || ArrowType.IsTotalArrowTypeName(ty.Name);
        }));
      }

      bool no1DArrayAccesses(params Expression[] exprs) {
        return exprs.All(e => Check<SeqSelectExpr>(e, sse => !sse.Seq.Type.IsArrayType)); // allow sequence accesses
      }

      bool noMultiDArrayAccesses(params Expression[] exprs) {
        return exprs.All(e => Check<MultiSelectExpr>(e, _ => false));
      }

      bool noFieldAccesses(MemberDecl member, params Expression[] exprs) {
        return exprs.All(e => Check<MemberSelectExpr>(e, mse => mse.Member != member));
      }

      bool isVar(BoundVar var, Expression expr) {
        return expr.Resolved is IdentifierExpr ie && ie.Var == var;
      }

      bool indexIsAlwaysVar(BoundVar var, params Expression[] exprs) {
        return exprs.All(e => Check<SeqSelectExpr>(e, sse2 => sse2.SelectOne && isVar(var, sse2.E0)));
      }

      bool accessedObjectIsAlwaysVar(MemberDecl member, BoundVar var, params Expression[] exprs) {
        return exprs.All(e => Check<MemberSelectExpr>(e, mse => mse.Member != member || isVar(var, mse.Obj)));
      }
    }

    /// <summary>
    /// Check all of the given expression's subexpressions of a given type
    /// using a predicate.  Returns true only if the predicate returns true for
    /// all subexpressions of type <typeparamref name="E"/>.
    /// </summary>
    private static bool Check<E>(Expression e, Predicate<E> pred) where E : Expression {
      var checker = new Checker<E>(pred);
      checker.Visit(e, null);
      return checker.Passing;
    }

    private class Checker<E> : TopDownVisitor<object> where E : Expression {
      private readonly Predicate<E> Pred;
      public bool Passing = true;

      public Checker(Predicate<E> pred) {
        Pred = pred;
      }

      protected override bool VisitOneExpr(Expression expr, ref object st) {
        if (expr is E e && !Pred(e)) {
          Passing = false;
          return false;
        } else {
          return true;
        }
      }
    }

    protected void TrAssignSuchThat(List<IVariable> lhss, Expression constraint, List<ComprehensionExpr.BoundedPool> bounds, int debuginfoLine, ConcreteSyntaxTree wr, bool inLetExprBody) {
      Contract.Requires(lhss != null);
      Contract.Requires(constraint != null);
      Contract.Requires(bounds != null);
      // For "i,j,k,l :| R(i,j,k,l);", emit something like:
      //
      // for (BigInteger iterLimit = 5; ; iterLimit *= 2) {
      //   var il$0 = iterLimit;
      //   foreach (L l' in sq.Elements) { l = l';
      //     if (il$0 == 0) { break; }  il$0--;
      //     var il$1 = iterLimit;
      //     foreach (K k' in st.Elements) { k = k';
      //       if (il$1 == 0) { break; }  il$1--;
      //       var il$2 = iterLimit;
      //       j = Lo;
      //       for (;; j++) {
      //         if (il$2 == 0) { break; }  il$2--;
      //         foreach (bool i' in Helper.AllBooleans) { i = i';
      //           if (R(i,j,k,l)) {
      //             goto ASSIGN_SUCH_THAT_<id>;
      //           }
      //         }
      //       }
      //     }
      //   }
      // }
      // throw new Exception("assign-such-that search produced no value"); // a verified program never gets here; however, we need this "throw" to please the C# compiler
      // ASSIGN_SUCH_THAT_<id>: ;
      //
      // where the iterLimit loop can be omitted if lhss.Count == 1 or if all bounds are finite.  Further optimizations could be done, but
      // are omitted for now.
      //
      var n = lhss.Count;
      Contract.Assert(bounds.Count == n);
      var c = ProtectedFreshNumericId("_ASSIGN_SUCH_THAT_+_iterLimit_");
      var doneLabel = "_ASSIGN_SUCH_THAT_" + c;
      var iterLimit = "_iterLimit_" + c;

      bool needIterLimit = lhss.Count != 1 && bounds.Exists(bnd => (bnd.Virtues & ComprehensionExpr.BoundedPool.PoolVirtues.Finite) == 0);
      wr = CreateLabeledCode(doneLabel, false, wr);
      var wrOuter = wr;
      if (needIterLimit) {
        wr = CreateDoublingForLoop(iterLimit, 5, wr);
      }

      for (int i = 0; i < n; i++) {
        var bound = bounds[i];
        Contract.Assert((bound.Virtues & ComprehensionExpr.BoundedPool.PoolVirtues.Enumerable) != 0);  // if we have got this far, it must be an enumerable bound
        var bv = lhss[i];
        if (needIterLimit) {
          DeclareLocalVar(string.Format("{0}_{1}", iterLimit, i), null, null, false, iterLimit, wr, Type.Int);
        }
        var tmpVar = ProtectedFreshId("_assign_such_that_");
        var wStmts = wr.Fork();
        var elementType = CompileCollection(bound, bv, inLetExprBody, true, null, out var collection, wStmts);
        wr = CreateGuardedForeachLoop(tmpVar, elementType, bv, false, inLetExprBody, bv.Tok, collection, wr);
        if (needIterLimit) {
          var varName = $"{iterLimit}_{i}";
          ConcreteSyntaxTree isZeroWriter;
          var thn = EmitIf(out isZeroWriter, false, wr);
          EmitIsZero(varName, isZeroWriter);
          EmitBreak(null, thn);
          EmitDecrementVar(varName, wr);
        }
      }

      copyInstrWriters.Push(wr.Fork());
      ConcreteSyntaxTree guardWriter;
      var wStmtsIf = wr.Fork();
      var wBody = EmitIf(out guardWriter, false, wr);
      guardWriter.Append(TrExpr(constraint, inLetExprBody, wStmtsIf));
      EmitBreak(doneLabel, wBody);
      copyInstrWriters.Pop();

      // Java compiler throws unreachable error when absurd statement is written after unbounded for-loop, so we don't write it then.
      EmitAbsurd(string.Format("assign-such-that search produced no value (line {0})", debuginfoLine), wrOuter, needIterLimit);
    }

    protected interface ILvalue {
      void EmitRead(ConcreteSyntaxTree wr);

      /// Write an assignment expression (or equivalent) for the lvalue,
      /// returning a TargetWriter for the RHS.  IMPORTANT: Whoever calls
      /// EmitWrite is responsible for making the types match up (as by
      /// EmitCoercionIfNecessary), for example by going through the overload
      /// of EmitAssignment that takes an ILvalue.
      ConcreteSyntaxTree EmitWrite(ConcreteSyntaxTree wr);
    }

    protected ILvalue SimpleLvalue(Action<ConcreteSyntaxTree> action) {
      return new SimpleLvalueImpl(this, action);
    }

    protected ILvalue SimpleLvalue(Action<ConcreteSyntaxTree> lvalueAction, Action<ConcreteSyntaxTree> rvalueAction) {
      return new SimpleLvalueImpl(this, lvalueAction, rvalueAction);
    }

    protected ILvalue StringLvalue(string str) {
      return new SimpleLvalueImpl(this, wr => wr.Write(str));
    }

    protected ILvalue SuffixLvalue(Action<ConcreteSyntaxTree> action, string str, params object[] args) {
      return new SimpleLvalueImpl(this, wr => { action(wr); wr.Write(str, args); });
    }

    protected ILvalue EnclosedLvalue(string prefix, Action<ConcreteSyntaxTree> action, string suffixStr, params object[] suffixArgs) {
      return new SimpleLvalueImpl(this, wr => { wr.Write(prefix); action(wr); wr.Write(suffixStr, suffixArgs); });
    }

    protected ILvalue CoercedLvalue(ILvalue lvalue, Type/*?*/ from, Type/*?*/ to) {
      return new CoercedLvalueImpl(this, lvalue, from, to);
    }

    protected ILvalue GetterSetterLvalue(Action<ConcreteSyntaxTree> obj, string getterName, string setterName) {
      Contract.Requires(obj != null);
      Contract.Requires(getterName != null);
      Contract.Requires(setterName != null);
      return new GetterSetterLvalueImpl(obj, getterName, setterName);
    }

    private class SimpleLvalueImpl : ILvalue {
      private readonly ConcreteSinglePassCompiler Compiler;
      private readonly Action<ConcreteSyntaxTree> LvalueAction, RvalueAction;

      public SimpleLvalueImpl(ConcreteSinglePassCompiler compiler, Action<ConcreteSyntaxTree> action) {
        Compiler = compiler;
        LvalueAction = action;
        RvalueAction = action;
      }

      public SimpleLvalueImpl(ConcreteSinglePassCompiler compiler, Action<ConcreteSyntaxTree> lvalueAction, Action<ConcreteSyntaxTree> rvalueAction) {
        Compiler = compiler;
        LvalueAction = lvalueAction;
        RvalueAction = rvalueAction;
      }

      public void EmitRead(ConcreteSyntaxTree wr) {
        RvalueAction(wr);
      }

      public ConcreteSyntaxTree EmitWrite(ConcreteSyntaxTree wr) {
        Compiler.EmitAssignment(out var wLhs, null, out var wRhs, null, wr);
        LvalueAction(wLhs);
        return wRhs;
      }
    }

    private class CoercedLvalueImpl : ILvalue {
      private readonly ConcreteSinglePassCompiler Compiler;
      private readonly ILvalue lvalue;
      private readonly Type /*?*/ from;
      private readonly Type /*?*/ to;

      public CoercedLvalueImpl(ConcreteSinglePassCompiler compiler, ILvalue lvalue, Type/*?*/ from, Type/*?*/ to) {
        Compiler = compiler;
        this.lvalue = lvalue;
        this.from = from;
        this.to = to;
      }

      public void EmitRead(ConcreteSyntaxTree wr) {
        wr = Compiler.EmitCoercionIfNecessary(from, to, Token.NoToken, wr);
        lvalue.EmitRead(wr);
      }

      public ConcreteSyntaxTree EmitWrite(ConcreteSyntaxTree wr) {
        return lvalue.EmitWrite(wr);
      }
    }

    private class GetterSetterLvalueImpl : ILvalue {
      private readonly Action<ConcreteSyntaxTree> obj;
      private readonly string getterName;
      private readonly string setterName;

      public GetterSetterLvalueImpl(Action<ConcreteSyntaxTree> obj, string getterName, string setterName) {
        this.obj = obj;
        this.getterName = getterName;
        this.setterName = setterName;
      }

      public void EmitRead(ConcreteSyntaxTree wr) {
        obj(wr);
        wr.Write($".{getterName}()");
      }

      public ConcreteSyntaxTree EmitWrite(ConcreteSyntaxTree wr) {
        obj(wr);
        wr.Write($".{setterName}(");
        var w = wr.Fork();
        wr.WriteLine(");");
        return w;
      }
    }

    ILvalue CreateLvalue(Expression lhs, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts) {
      Contract.Requires(lhs != null);
      Contract.Requires(wr != null);

      lhs = lhs.Resolved;
      if (lhs is IdentifierExpr) {
        var ll = (IdentifierExpr)lhs;
        return StringLvalue(IdName(ll.Var));
      } else if (lhs is MemberSelectExpr) {
        var ll = (MemberSelectExpr)lhs;
        Contract.Assert(!ll.Member.IsInstanceIndependentConstant);  // instance-independent const's don't have assignment statements
        var obj = StabilizeExpr(ll.Obj, "_obj", wr, wStmts);
        var typeArgs = TypeArgumentInstantiation.ListFromMember(ll.Member, null, ll.TypeApplication_JustMember);
        return EmitMemberSelect(w => w.Write(obj), ll.Obj.Type, ll.Member, typeArgs, ll.TypeArgumentSubstitutionsWithParents(), lhs.Type,
          internalAccess: enclosingMethod is Constructor);
      } else if (lhs is SeqSelectExpr) {
        var ll = (SeqSelectExpr)lhs;
        var arr = StabilizeExpr(ll.Seq, "_arr", wr, wStmts);
        var index = StabilizeExpr(ll.E0, "_index", wr, wStmts);
        if (ll.Seq.Type.IsArrayType || ll.Seq.Type.AsSeqType != null) {
          index = ArrayIndexToNativeInt(index, ll.E0.Type);
        }
        return EmitArraySelectAsLvalue(arr, new List<string>() { index }, ll.Type);
      } else {
        var ll = (MultiSelectExpr)lhs;
        string arr = StabilizeExpr(ll.Array, "_arr", wr, wStmts);
        var indices = new List<string>();
        int i = 0;
        foreach (var idx in ll.Indices) {
          var index = StabilizeExpr(idx, "_index" + i + "_", wr, wStmts);
          index = ArrayIndexToNativeInt(index, idx.Type);
          indices.Add(index);
          i++;
        }
        return EmitArraySelectAsLvalue(arr, indices, ll.Type);
      }
    }

    /// <summary>
    /// If the given expression's value is stable, translate it and return the
    /// string form.  Otherwise, output code to evaluate the expression, then
    /// return a fresh variable bound to its value.
    /// </summary>
    /// <param name="e">An expression to evaluate</param>
    /// <param name="prefix">The prefix to give the fresh variable, if
    /// needed.</param>
    /// <param name="wr">A writer in a position to write statements
    /// evaluating the expression</param>
    /// <returns>A string giving the translated value as a stable
    /// expression</returns>
    /// <seealso cref="IsStableExpr"/>
    private string StabilizeExpr(Expression e, string prefix, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts) {
      if (IsStableExpr(e)) {
        var sw = new ConcreteSyntaxTree();
        TrParenExpr(e, sw, false, wStmts);
        return sw.ToString();
      } else {
        var v = ProtectedFreshId(prefix);
        DeclareLocalVar(v, null, null, e, false, wr);
        return v;
      }
    }

    /// <summary>
    /// Returns whether the given expression is <em>stable</em>, that is,
    /// whether its value is fixed over the course of the evaluation of an
    /// expression.  Note that anything that could be altered by a function call
    /// (say, the value of a non-constant field) is unstable.
    /// </summary>
    private bool IsStableExpr(Expression e) {
      if (e is IdentifierExpr || e is ThisExpr || e is LiteralExpr) {
        return true;
      } else if (e is MemberSelectExpr mse) {
        if (!IsStableExpr(mse.Obj)) {
          return false;
        }
        var member = mse.Member;
        if (member is ConstantField) {
          return true;
        } else if (member is SpecialField sf) {
          switch (sf.SpecialId) {
            case SpecialField.ID.ArrayLength:
            case SpecialField.ID.ArrayLengthInt:
            case SpecialField.ID.Floor:
            case SpecialField.ID.IsLimit:
            case SpecialField.ID.IsSucc:
            case SpecialField.ID.Offset:
            case SpecialField.ID.IsNat:
            case SpecialField.ID.Keys:
            case SpecialField.ID.Values:
            case SpecialField.ID.Items:
              return true;
            default:
              return false;
          }
        } else {
          return false;
        }
      } else if (e is ConcreteSyntaxExpression cse) {
        return IsStableExpr(cse.ResolvedExpression);
      } else {
        return false;
      }
    }

    /// <summary>
    /// Translate the right-hand side of an assignment.
    /// </summary>
    /// <param name="rhs">The RHS to translate</param>
    /// <param name="wr">The writer at the position for the translated RHS</param>
    /// <param name="wStmts">A writer at an earlier position where extra statements may be written</param>
    void TrRhs(AssignmentRhs rhs, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts) {
      Contract.Requires(!(rhs is HavocRhs));
      Contract.Requires(wr != null);

      var tRhs = rhs as TypeRhs;

      if (tRhs == null) {
        var eRhs = (ExprRhs)rhs;  // it's not HavocRhs (by the precondition) or TypeRhs (by the "if" test), so it's gotta be ExprRhs
        wr.Append(TrExpr(eRhs.Expr, false, wStmts));
      } else {
        var nw = ProtectedFreshId("_nw");
        var pwStmts = wStmts.Fork();
        var wRhs = DeclareLocalVar(nw, tRhs.Type, rhs.Tok, wStmts);
        TrTypeRhs(tRhs, wRhs, pwStmts);

        // Proceed with initialization
        if (tRhs.InitCall != null) {
          string q, n;
          if (tRhs.InitCall.Method is Constructor && tRhs.InitCall.Method.IsExtern(out q, out n)) {
            // initialization was done at the time of allocation
          } else {
            TrCallStmt(tRhs.InitCall, nw, wStmts);
          }
        } else if (tRhs.ElementInit != null) {
          // Compute the array-initializing function once and for all (as required by the language definition)
          var f = ProtectedFreshId("_arrayinit");
          DeclareLocalVar(f, tRhs.ElementInit.Type, tRhs.ElementInit.tok, tRhs.ElementInit, false, wStmts);
          // Build a loop nest that will call the initializer for all indices
          var indices = Util.Map(Enumerable.Range(0, tRhs.ArrayDimensions.Count), ii => ProtectedFreshId("_arrayinit_" + ii));
          var w = wStmts;
          for (var d = 0; d < tRhs.ArrayDimensions.Count; d++) {
            string len, pre, post;
            GetSpecialFieldInfo(SpecialField.ID.ArrayLength, tRhs.ArrayDimensions.Count == 1 ? null : d, tRhs.Type, out len, out pre, out post);
            var bound = $"{pre}{nw}{(len == "" ? "" : "." + len)}{post}";
            w = CreateForLoop(indices[d], bound, w);
          }
          var eltRhs = string.Format("{0}{2}({1})", f, indices.Comma(idx => ArrayIndexToInt(idx, Type.Int)), LambdaExecute);
          var wArray = EmitArrayUpdate(indices, eltRhs, tRhs.EType, w);
          wArray.Write(nw);
          EndStmt(w);
        } else if (tRhs.InitDisplay != null) {
          var ii = 0;
          foreach (var v in tRhs.InitDisplay) {
            var wArray = EmitArrayUpdate(new List<string> { ii.ToString() }, v, wStmts);
            wArray.Write(nw);
            EndStmt(wStmts);
            ii++;
          }
        }

        // Assign to the final LHS
        wr.Write(nw);
      }
    }

    private static Type TypeOfLhs(Expression lhs) {
      Contract.Requires(lhs != null);
      if (lhs is IdentifierExpr) {
        var e = (IdentifierExpr)lhs;
        return e.Var.Type;
      } else if (lhs is MemberSelectExpr) {
        var e = (MemberSelectExpr)lhs;
        return ((Field)e.Member).Type.Subst(e.TypeArgumentSubstitutionsWithParents());
      } else if (lhs is SeqSelectExpr) {
        var e = (SeqSelectExpr)lhs;
        return e.Seq.Type.NormalizeExpand().TypeArgs[0];
      } else {
        var e = (MultiSelectExpr)lhs;
        return e.Array.Type.NormalizeExpand().TypeArgs[0];
      }
    }

    // to do: Make Type an abstract property of AssignmentRhs.  Unfortunately, this would first require convincing Microsoft that it makes sense for a base class to have a property that's only settable in some subclasses.  Until then, let it be known that Java's "properties" (i.e. getter/setter pairs) are more powerful >:-)
    private static Type TypeOfRhs(AssignmentRhs rhs) {
      if (rhs is TypeRhs tRhs) {
        return tRhs.Type;
      } else if (rhs is ExprRhs eRhs) {
        return eRhs.Expr.Type;
      } else {
        return null;
      }
    }

    void TrCallStmt(CallStmt s, string receiverReplacement, ConcreteSyntaxTree wr) {
      Contract.Requires(s != null);
      Contract.Assert(s.Method != null);  // follows from the fact that stmt has been successfully resolved

      if (s.Method == enclosingMethod && enclosingMethod.IsTailRecursive) {
        // compile call as tail-recursive
        TrTailCallStmt(s.Tok, s.Method, s.Receiver, s.Args, receiverReplacement, wr);
      } else {
        // compile call as a regular call
        var wStmts = wr.Fork();

        var lvalues = new List<ILvalue>();  // contains an entry for each non-ghost formal out-parameter, but the entry is null if the actual out-parameter is ghost
        Contract.Assert(s.Lhs.Count == s.Method.Outs.Count);
        for (int i = 0; i < s.Method.Outs.Count; i++) {
          Formal p = s.Method.Outs[i];
          if (!p.IsGhost) {
            var lhs = s.Lhs[i].Resolved;
            if (lhs is IdentifierExpr lhsIE && lhsIE.Var.IsGhost) {
              lvalues.Add(null);
            } else if (lhs is MemberSelectExpr lhsMSE && lhsMSE.Member.IsGhost) {
              lvalues.Add(null);
            } else {
              lvalues.Add(CreateLvalue(s.Lhs[i], wr, wStmts));
            }
          }
        }
        var outTmps = new List<string>();  // contains a name for each non-ghost formal out-parameter
        var outTypes = new List<Type>();  // contains a type for each non-ghost formal out-parameter
        var outFormalTypes = new List<Type>(); // contains the type as it appears in the method type (possibly includes type parameters)
        var outLhsTypes = new List<Type>(); // contains the type as it appears on the LHS (may give types for those parameters)
        for (int i = 0; i < s.Method.Outs.Count; i++) {
          Formal p = s.Method.Original.Outs[i];
          if (!p.IsGhost) {
            string target = ProtectedFreshId("_out");
            outTmps.Add(target);
            var instantiatedType = p.Type.Subst(s.MethodSelect.TypeArgumentSubstitutionsWithParents());
            Type type;
            if (NeedsCastFromTypeParameter && IsCoercionNecessary(p.Type, instantiatedType)) {
              //
              // The type of the parameter will differ from the LHS type in a
              // situation like this:
              //
              //   method Gimmie<R(0)>() returns (r: R) { }
              //
              //   var a : int := Gimmie();
              //
              // The method Gimme will be compiled down to Go (or JavaScript)
              // as a function which returns any value (some details omitted):
              //
              //   func Gimmie(ty _dafny.Type) interface{} {
              //     return ty.Default()
              //   }
              //
              // If we naively translate, we get a type error:
              //
              //   var lhs int = Gimmie(_dafny.IntType) // error!
              //
              // Fortunately, we have the information at hand to do better.  The
              // LHS does have the actual final type (int in this case), and the
              // out parameter on the method tells us the compiled type
              // returned.  Therefore what we want to do is this:
              //
              //   var lhs dafny.Int
              //   var _out interface{}
              //
              //   _out = Gimmie(dafny.IntType)
              //
              //   lhs = _out.(int) // type cast
              //
              // All we have to do now is declare the out variable with the type
              // from the parameter; later we'll do the type cast.
              //
              // None of this is necessary if the language supports parametric
              // functions to begin with (C#) or has dynamic typing so none of
              // this comes up (JavaScript), so we only do this if
              // NeedsCastFromTypeParameter is on.
              //
              // This used to just assign p.Type to type, but that was something of a hack
              // that didn't work in all cases: if p.Type is indeed a type parameter,
              // it won't be in scope on the caller side. That means you can't generally
              // declare a local variable with that type; it happened to work for Go
              // because it would just use interface{}, but Java would try to use the type
              // parameter directly. The TypeForCoercion() hook was added as a place to
              // explicitly swap in a target-language type such as java.lang.Object.
              type = TypeForCoercion(p.Type);
            } else {
              type = instantiatedType;
            }
            if (s.Method.IsExtern(out _, out _)) {
              type = NativeForm(type);
            }
            outTypes.Add(type);
            outFormalTypes.Add(p.Type);
            outLhsTypes.Add(s.Lhs[i].Type);
            DeclareLocalVar(target, type, s.Lhs[i].tok, false, null, wr);
          }
        }
        Contract.Assert(lvalues.Count == outTmps.Count);

        bool customReceiver = !(s.Method.EnclosingClass is TraitDecl) && NeedsCustomReceiver(s.Method);
        Contract.Assert(receiverReplacement == null || !customReceiver);  // What would be done in this case? It doesn't ever happen, right?

        var returnStyleOuts = UseReturnStyleOuts(s.Method, outTmps.Count);
        var returnStyleOutCollector = outTmps.Count > 1 && returnStyleOuts && !SupportsMultipleReturns ? ProtectedFreshId("_outcollector") : null;
        if (returnStyleOutCollector != null) {
          DeclareSpecificOutCollector(returnStyleOutCollector, wr, outFormalTypes, outLhsTypes);
        } else if (outTmps.Count > 0 && returnStyleOuts) {
          wr.Write("{0} = ", Util.Comma(outTmps));
        }
        var wrOrig = wr;
        if (returnStyleOutCollector == null && outTmps.Count == 1 && returnStyleOuts) {
          var instantiatedFromType = outFormalTypes[0].Subst(s.MethodSelect.TypeArgumentSubstitutionsWithParents());
          var toType = outTypes[0];
          wr = EmitDowncastIfNecessary(instantiatedFromType, toType, s.Tok, wr);
        }
        var protectedName = IdName(s.Method);
        if (receiverReplacement != null) {
          wr.Write(IdProtect(receiverReplacement));
          wr.Write(ClassAccessor);
        } else if (customReceiver) {
          wr.Write(TypeName_Companion(s.Receiver.Type, wr, s.Tok, s.Method));
          wr.Write(ClassAccessor);
        } else if (!s.Method.IsStatic) {
          TrParenExpr(s.Receiver, wr, false, wStmts);
          wr.Write(ClassAccessor);
        } else if (s.Method.IsExtern(out var qual, out var compileName) && qual != null) {
          wr.Write("{0}{1}", qual, ModuleSeparator);
          protectedName = compileName;
        } else {
          wr.Write(TypeName_Companion(s.Receiver.Type, wr, s.Tok, s.Method));
          wr.Write(ModuleSeparator);
        }
        var typeArgs = CombineAllTypeArguments(s.Method, s.MethodSelect.TypeApplication_AtEnclosingClass, s.MethodSelect.TypeApplication_JustMember);
        EmitNameAndActualTypeArgs(protectedName, TypeArgumentInstantiation.ToActuals(ForTypeParameters(typeArgs, s.Method, false)), s.Tok, wr);
        wr.Write("(");
        var sep = "";
        EmitTypeDescriptorsActuals(ForTypeDescriptors(typeArgs, s.Method.EnclosingClass, s.Method, false), s.Tok, wr, ref sep);
        if (customReceiver) {
          wr.Write(sep);
          var w = EmitCoercionIfNecessary(s.Receiver.Type, UserDefinedType.UpcastToMemberEnclosingType(s.Receiver.Type, s.Method), s.Tok, wr);
          w.Append(TrExpr(s.Receiver, false, wStmts));
          sep = ", ";
        }
        for (int i = 0; i < s.Method.Ins.Count; i++) {
          Formal p = s.Method.Ins[i];
          if (!p.IsGhost) {
            wr.Write(sep);
            var fromType = s.Args[i].Type;
            var toType = s.Method.Ins[i].Type;
            var instantiatedToType = toType.Subst(s.MethodSelect.TypeArgumentSubstitutionsWithParents());
            // Order of coercions is important here: EmitCoercionToNativeForm may coerce into a type we're unaware of, so it *has* to be last
            var w = EmitCoercionIfNecessary(fromType, toType, s.Tok, wr);
            w = EmitDowncastIfNecessary(fromType, instantiatedToType, s.Tok, w);
            if (s.Method.IsExtern(out _, out _)) {
              w = EmitCoercionToNativeForm(toType, s.Tok, w);
            }
            w.Append(TrExpr(s.Args[i], false, wStmts));
            sep = ", ";
          }
        }

        if (!returnStyleOuts) {
          foreach (var outTmp in outTmps) {
            wr.Write(sep);
            EmitActualOutArg(outTmp, wr);
            sep = ", ";
          }
        }
        wr.Write(')');
        wr = wrOrig;
        EndStmt(wr);
        if (returnStyleOutCollector != null) {
          EmitCastOutParameterSplits(returnStyleOutCollector, outTmps, wr, outFormalTypes, outTypes, s.Tok);
        }

        // assign to the actual LHSs
        for (int j = 0, l = 0; j < s.Method.Outs.Count; j++) {
          var p = s.Method.Outs[j];
          if (!p.IsGhost) {
            var lvalue = lvalues[l];
            if (lvalue != null) {
              // The type information here takes care both of implicit upcasts and
              // implicit downcasts from type parameters (see above).
              ConcreteSyntaxTree wRhs = EmitAssignment(lvalue, s.Lhs[j].Type, outTypes[l], wr, s.Tok);
              if (s.Method.IsExtern(out _, out _)) {
                wRhs = EmitCoercionFromNativeForm(p.Type, s.Tok, wRhs);
              }
              wRhs.Write(outTmps[l]);
              // Coercion from the out type to the LHS type is the responsibility
              // of the EmitAssignment above
            }
            l++;
          }
        }
      }
    }

    void TrTailCallStmt(IToken tok, Method method, Expression receiver, List<Expression> args, string receiverReplacement, ConcreteSyntaxTree wr) {
      Contract.Requires(tok != null);
      Contract.Requires(method != null);
      Contract.Requires(receiver != null);
      Contract.Requires(args != null);
      Contract.Requires(method.IsTailRecursive);
      Contract.Requires(wr != null);

      // assign the actual in-parameters to temporary variables
      var inTmps = new List<string>();
      var inTypes = new List<Type/*?*/>();
      if (receiverReplacement != null) {
        // TODO:  What to do here?  When does this happen, what does it mean?
      } else if (!method.IsStatic) {
        string inTmp = ProtectedFreshId("_in");
        inTmps.Add(inTmp);
        inTypes.Add(null);
        DeclareLocalVar(inTmp, null, null, receiver, false, wr);
      }
      for (int i = 0; i < method.Ins.Count; i++) {
        Formal p = method.Ins[i];
        if (!p.IsGhost) {
          string inTmp = ProtectedFreshId("_in");
          inTmps.Add(inTmp);
          inTypes.Add(args[i].Type);
          DeclareLocalVar(inTmp, args[i].Type, p.tok, args[i], false, wr);
        }
      }
      // Now, assign to the formals
      int n = 0;
      if (!method.IsStatic) {
        wr.Write("_this = ");
        ConcreteSyntaxTree wRHS;
        if (thisContext == null) {
          wRHS = wr;
        } else {
          var instantiatedType = receiver.Type.Subst(thisContext.ParentFormalTypeParametersToActuals);
          wRHS = EmitCoercionIfNecessary(instantiatedType, UserDefinedType.FromTopLevelDecl(tok, thisContext), tok, wr);
        }
        wRHS.Write(inTmps[n]);
        EndStmt(wr);
        n++;
      }
      foreach (var p in method.Ins) {
        if (!p.IsGhost) {
          EmitAssignment(IdName(p), p.Type, inTmps[n], inTypes[n], wr);
          n++;
        }
      }
      Contract.Assert(n == inTmps.Count);
      // finally, the jump back to the head of the method
      EmitJumpToTailCallStart(wr);
    }

    /// <summary>
    /// Before calling TrAssignmentRhs(rhs), the caller must have spilled the let variables declared in "tp".
    /// </summary>
    void TrTypeRhs(TypeRhs tp, ConcreteSyntaxTree wr, ConcreteSyntaxTree wStmts) {
      Contract.Requires(tp != null);
      Contract.Requires(wr != null);

      if (tp.ArrayDimensions == null) {
        var initCall = tp.InitCall != null && tp.InitCall.Method is Constructor ? tp.InitCall : null;
        EmitNew(tp.EType, tp.Tok, initCall, wr, wStmts);
      } else if (tp.ElementInit != null || tp.InitDisplay != null || DatatypeWrapperEraser.CanBeLeftUninitialized(tp.EType)) {
        EmitNewArray(tp.EType, tp.Tok, tp.ArrayDimensions, false, wr, wStmts);
      } else {
        // If an initializer is not known, the only way the verifier would have allowed this allocation
        // is if the requested size is 0.
        EmitNewArray(tp.EType, tp.Tok, tp.ArrayDimensions, tp.EType.HasCompilableValue, wr, wStmts);
      }
    }

    protected virtual void TrStmtList(List<Statement> stmts, ConcreteSyntaxTree writer) {
      Contract.Requires(cce.NonNullElements(stmts));
      Contract.Requires(writer != null);
      foreach (Statement ss in stmts) {
        // label:        // if any
        //   <prelude>   // filled via copyInstrWriters -- copies out-parameters used in letexpr to local variables
        //   ss          // translation of ss has side effect of filling the top copyInstrWriters
        var w = writer;
        if (ss.Labels != null && !(ss is VarDeclPattern or VarDeclStmt)) {
          // We are not breaking out of VarDeclPattern or VarDeclStmt, so the labels there are useless
          // They were useful for verification
          w = CreateLabeledCode(ss.Labels.Data.AssignUniqueId(idGenerator), false, w);
        }
        var prelude = w.Fork();
        copyInstrWriters.Push(prelude);
        TrStmt(ss, w);
        copyInstrWriters.Pop();
      }
    }

    protected ConcreteSyntaxTree EmitContinueLabel(LList<Label> loopLabels, ConcreteSyntaxTree writer) {
      Contract.Requires(writer != null);
      if (loopLabels != null) {
        writer = CreateLabeledCode(loopLabels.Data.AssignUniqueId(idGenerator), true, writer);
      }
      return writer;
    }

    void TrLocalVar(IVariable v, bool alwaysInitialize, ConcreteSyntaxTree wr) {
      Contract.Requires(v != null);
      if (v.IsGhost) {
        // only emit non-ghosts (we get here only for local variables introduced implicitly by call statements)
        return;
      }
      DeclareLocalVar(IdName(v), v.Type, v.Tok, false, alwaysInitialize ? PlaceboValue(v.Type, wr, v.Tok, true) : null, wr);
    }

    protected ConcreteSyntaxTree MatchCasePrelude(string source, UserDefinedType sourceType, DatatypeCtor ctor, List<BoundVar> arguments, int caseIndex, int caseCount, ConcreteSyntaxTree wr) {
      Contract.Requires(source != null);
      Contract.Requires(sourceType != null);
      Contract.Requires(ctor != null);
      Contract.Requires(cce.NonNullElements(arguments));
      Contract.Requires(0 <= caseIndex && caseIndex < caseCount);
      // if (source.is_Ctor0) {
      //   FormalType f0 = ((Dt_Ctor0)source._D).a0;
      //   ...
      var lastCase = caseIndex == caseCount - 1;
      ConcreteSyntaxTree w;
      if (lastCase) {
        // Need to avoid if (true) because some languages (Go, someday Java)
        // pretend that an if (true) isn't a certainty, leading to a complaint
        // about a missing return statement
        w = EmitBlock(wr);
      } else {
        ConcreteSyntaxTree guardWriter;
        w = EmitIf(out guardWriter, !lastCase, wr);
        EmitConstructorCheck(source, ctor, guardWriter);
      }

      int k = 0;  // number of processed non-ghost arguments
      for (int m = 0; m < ctor.Formals.Count; m++) {
        Formal arg = ctor.Formals[m];
        if (!arg.IsGhost) {
          BoundVar bv = arguments[m];
          // FormalType f0 = ((Dt_Ctor0)source._D).a0;
          var sw = DeclareLocalVar(IdName(bv), bv.Type, bv.Tok, w);
          EmitDestructor(source, arg, k, ctor, SelectNonGhost(sourceType.ResolvedClass, sourceType.TypeArgs), bv.Type, sw);
          k++;
        }
      }
      return w;
    }

    // ----- Expression ---------------------------------------------------------------------------

    /// <summary>
    /// Before calling TrParenExpr(expr), the caller must have spilled the let variables declared in "expr".
    /// </summary>
    protected void TrParenExpr(string prefix, Expression expr, ConcreteSyntaxTree wr, bool inLetExprBody, ConcreteSyntaxTree wStmts) {
      Contract.Requires(prefix != null);
      Contract.Requires(expr != null);
      Contract.Requires(wr != null);
      wr.Write(prefix);
      TrParenExpr(expr, wr, inLetExprBody, wStmts);
    }

    /// <summary>
    /// Before calling TrParenExpr(expr), the caller must have spilled the let variables declared in "expr".
    /// </summary>
    protected void TrParenExpr(Expression expr, ConcreteSyntaxTree wr, bool inLetExprBody, ConcreteSyntaxTree wStmts) {
      Contract.Requires(expr != null);
      Contract.Requires(wr != null);
      Contract.Requires(wStmts != null);
      wr.ForkInParens().Append(TrExpr(expr, inLetExprBody, wStmts));
    }

    /// <summary>
    /// Before calling TrExprList(exprs), the caller must have spilled the let variables declared in expressions in "exprs".
    /// </summary>
    protected void TrExprList(List<Expression> exprs, ConcreteSyntaxTree wr, bool inLetExprBody, ConcreteSyntaxTree wStmts,
        Type/*?*/ type = null, bool parens = true) {
      Contract.Requires(cce.NonNullElements(exprs));
      if (parens) { wr = wr.ForkInParens(); }
      string sep = "";
      foreach (Expression e in exprs) {
        wr.Write(sep);
        ConcreteSyntaxTree w;
        if (type != null) {
          w = wr.Fork();
          w = EmitCoercionIfNecessary(e.Type, type, e.tok, w);
        } else {
          w = wr;
        }
        w.Append(TrExpr(e, inLetExprBody, wStmts));
        sep = ", ";
      }
    }

    protected virtual void WriteCast(string s, ConcreteSyntaxTree wr) { }

    protected ConcreteSyntaxTree Expr(Expression expr, bool inLetExprBody, ConcreteSyntaxTree wStmts) {
      var result = new ConcreteSyntaxTree();
      result.Append(TrExpr(expr, inLetExprBody, wStmts));
      return result;
    }

    /// <summary>
    /// When inside an enumeration like this:
    /// 
    ///     foreach(var [tmpVarName]: [collectionElementType] in ...) {
    ///        ...
    ///     }
    /// 
    /// MaybeInjectSubtypeConstraint emits a subtype constraint that tmpVarName should be of type boundVarType, typically of the form
    /// 
    ///       if([tmpVarName] is [boundVarType]) {
    ///         // This is where 'wr' will write
    ///       }
    ///
    /// If isReturning is true, then it will also insert the "return" statements,
    /// to use in the lambdas used by forall and exists statements:
    ///
    ///       if([tmpVarName] is [boundVarType]) {
    ///         return // This is where 'wr' will write
    ///       } else {
    ///         return [elseReturnValue];
    ///       }
    ///
    /// </summary>
    /// <returns></returns>
    protected ConcreteSyntaxTree MaybeInjectSubtypeConstraint(string tmpVarName,
      Type collectionElementType, Type boundVarType, bool inLetExprBody,
      IToken tok, ConcreteSyntaxTree wr, bool isReturning = false, bool elseReturnValue = false
      ) {
      var iterationValuesNeedToBeChecked = IsTargetSupertype(collectionElementType, boundVarType);
      if (iterationValuesNeedToBeChecked) {
        var preconditions = wr.Fork();
        var conditions = GetSubtypeCondition(tmpVarName, boundVarType, tok, preconditions);
        if (conditions == null) {
          preconditions.Clear();
        } else {
          var thenWriter = EmitIf(out var guardWriter, isReturning, wr);
          guardWriter.Write(conditions);
          if (isReturning) {
            wr = EmitBlock(wr);
            var wStmts = wr.Fork();
            wr = EmitReturnExpr(wr);
            wr.Append(TrExpr(new LiteralExpr(tok, elseReturnValue), inLetExprBody, wStmts));
          }
          wr = thenWriter;
        }
      }
      return wr;
    }

    protected ConcreteSyntaxTree MaybeInjectSubsetConstraint(IVariable boundVar, Type boundVarType,
      Type collectionElementType, bool inLetExprBody,
      IToken tok, ConcreteSyntaxTree wr, bool isReturning = false, bool elseReturnValue = false,
      bool isSubfiltering = false) {
      if (!boundVarType.Equals(collectionElementType, true) &&
          boundVarType.NormalizeExpand(true) is UserDefinedType
          {
            TypeArgs: var typeArgs,
            ResolvedClass:
            SubsetTypeDecl
            {
              TypeArgs: var typeParametersArgs,
              Var: var variable,
              Constraint: var constraint
            }
          }) {
        if (variable.Type.NormalizeExpandKeepConstraints() is UserDefinedType
          {
            ResolvedClass: SubsetTypeDecl
          } normalizedVariableType) {
          wr = MaybeInjectSubsetConstraint(boundVar, normalizedVariableType, collectionElementType,
              inLetExprBody, tok, wr, isReturning, elseReturnValue, true);
        }

        var bvIdentifier = new IdentifierExpr(tok, boundVar);
        var typeParameters = TypeParameter.SubstitutionMap(typeParametersArgs, typeArgs);
        var subContract = new Substituter(null,
          new Dictionary<IVariable, Expression>()
          {
            {variable, bvIdentifier}
          },
          new Dictionary<TypeParameter, Type>(
            typeParameters
          )
        );
        var constraintInContext = subContract.Substitute(constraint);
        var wStmts = wr.Fork();
        var thenWriter = EmitIf(out var guardWriter, hasElse: isReturning, wr);
        guardWriter.Append(TrExpr(constraintInContext, inLetExprBody, wStmts));
        if (isReturning) {
          var elseBranch = wr;
          elseBranch = EmitBlock(elseBranch);
          elseBranch = EmitReturnExpr(elseBranch);
          wStmts = elseBranch.Fork();
          elseBranch.Append(TrExpr(new LiteralExpr(tok, elseReturnValue), inLetExprBody, wStmts));
        }
        wr = thenWriter;
      }

      if (isReturning && !isSubfiltering) {
        wr = EmitReturnExpr(wr);
      }
      return wr;
    }

    protected ConcreteSyntaxTree CaptureFreeVariables(Expression expr, bool captureOnlyAsRequiredByTargetLanguage,
      out Substituter su, bool inLetExprBody, ConcreteSyntaxTree wr, ref ConcreteSyntaxTree wStmts) {
      if (captureOnlyAsRequiredByTargetLanguage && TargetLambdaCanUseEnclosingLocals) {
        // nothing to do
      } else {
        CreateFreeVarSubstitution(expr, out var bvars, out var fexprs, out su);
        if (bvars.Count != 0) {
          return EmitBetaRedex(bvars.ConvertAll(IdName), fexprs, bvars.ConvertAll(bv => bv.Type), expr.Type, expr.tok, inLetExprBody, wr, ref wStmts);
        }
      }
      su = Substituter.EMPTY;
      return wr;
    }

    void CreateFreeVarSubstitution(Expression expr, out List<BoundVar> bvars, out List<Expression> fexprs, out Substituter su) {
      Contract.Requires(expr != null);

      var fvs = FreeVariablesUtil.ComputeFreeVariables(expr);
      var sm = new Dictionary<IVariable, Expression>();

      bvars = new List<BoundVar>();
      fexprs = new List<Expression>();
      foreach (var fv in fvs) {
        if (fv.IsGhost) {
          continue;
        }
        fexprs.Add(new IdentifierExpr(fv.Tok, fv.Name) {
          Var = fv, // resolved here!
          Type = fv.Type
        });
        var bv = new BoundVar(fv.Tok, fv.Name, fv.Type);
        bvars.Add(bv);
        sm[fv] = new IdentifierExpr(bv.Tok, bv.Name) {
          Var = bv, // resolved here!
          Type = bv.Type
        };
      }

      su = new Substituter(null, sm, new Dictionary<TypeParameter, Type>());
    }

    protected bool IsHandleComparison(IToken tok, Expression e0, Expression e1, ConcreteSyntaxTree errorWr) {
      Contract.Requires(tok != null);
      Contract.Requires(e0 != null);
      Contract.Requires(e1 != null);
      TopLevelDecl cl;
      var isHandle0 = true;
      cl = (e0.Type.NormalizeExpand() as UserDefinedType)?.ResolvedClass;
      if (cl == null || !Attributes.ContainsBool(cl.Attributes, "handle", ref isHandle0)) {
        isHandle0 = false;
      }
      var isHandle1 = true;
      cl = (e1.Type.NormalizeExpand() as UserDefinedType)?.ResolvedClass;
      if (cl == null || !Attributes.ContainsBool(cl.Attributes, "handle", ref isHandle1)) {
        isHandle1 = false;
      }
      if (isHandle0 && isHandle1) {
        return true;
      } else if (isHandle0 || isHandle1) {
        Error(tok, "Comparison of a handle can only be with another handle", errorWr);
      }
      return false;
    }

    protected ConcreteSyntaxTree StringLiteral(StringLiteralExpr str) {
      var result = new ConcreteSyntaxTree();
      TrStringLiteral(str, result);
      return result;
    }

    protected virtual void TrStringLiteral(StringLiteralExpr str, ConcreteSyntaxTree wr) {
      Contract.Requires(str != null);
      Contract.Requires(wr != null);
      EmitStringLiteral((string)str.Value, str.IsVerbatim, wr);
    }

    /// <summary>
    /// Try to evaluate "expr" into one BigInteger.  On success, return it; otherwise, return "null".
    /// </summary>
    /// <param name="expr"></param>
    /// <returns></returns>
    public static Nullable<BigInteger> PartiallyEvaluate(Expression expr) {
      Contract.Requires(expr != null);
      expr = expr.Resolved;
      if (expr is LiteralExpr) {
        var e = (LiteralExpr)expr;
        if (e.Value is BigInteger) {
          return (BigInteger)e.Value;
        }
      } else if (expr is BinaryExpr) {
        var e = (BinaryExpr)expr;
        switch (e.ResolvedOp) {
          case BinaryExpr.ResolvedOpcode.Add:
          case BinaryExpr.ResolvedOpcode.Sub:
          case BinaryExpr.ResolvedOpcode.Mul:
            // possibly the most important case is Sub, since that's how NegationExpression's end up
            var arg0 = PartiallyEvaluate(e.E0);
            var arg1 = arg0 == null ? null : PartiallyEvaluate(e.E1);
            if (arg1 != null) {
              switch (e.ResolvedOp) {
                case BinaryExpr.ResolvedOpcode.Add:
                  return arg0 + arg1;
                case BinaryExpr.ResolvedOpcode.Sub:
                  return arg0 - arg1;
                case BinaryExpr.ResolvedOpcode.Mul:
                  return arg0 * arg1;
                default:
                  Contract.Assert(false);
                  break;  // please compiler
              }
            }
            break;
          default:
            break;
        }
      }
      return null;
    }

    protected ConcreteSyntaxTree TrCasePattern(CasePattern<BoundVar> pat, string rhsString, Type rhsType, Type bodyType,
      ConcreteSyntaxTree wr, ref ConcreteSyntaxTree wStmts) {
      Contract.Requires(pat != null);
      Contract.Requires(rhsString != null);
      Contract.Requires(rhsType != null);
      Contract.Requires(bodyType != null);
      Contract.Requires(wr != null);

      if (pat.Var != null) {
        var bv = pat.Var;
        if (!bv.IsGhost) {
          CreateIIFE(IdProtect(bv.CompileName), bv.Type, bv.Tok, bodyType, pat.tok, wr, ref wStmts, out var wrRhs, out var wrBody);
          wrRhs = EmitDowncastIfNecessary(rhsType, bv.Type, bv.tok, wrRhs);
          wrRhs.Write(rhsString);
          return wrBody;
        }
      } else if (pat.Arguments != null) {
        var ctor = pat.Ctor;
        Contract.Assert(ctor != null);  // follows from successful resolution
        Contract.Assert(pat.Arguments.Count == ctor.Formals.Count);  // follows from successful resolution
        Contract.Assert(ctor.EnclosingDatatype.TypeArgs.Count == rhsType.NormalizeExpand().TypeArgs.Count);
        var typeSubst = TypeParameter.SubstitutionMap(ctor.EnclosingDatatype.TypeArgs, rhsType.NormalizeExpand().TypeArgs);
        var k = 0;  // number of non-ghost formals processed
        for (int i = 0; i < pat.Arguments.Count; i++) {
          var arg = pat.Arguments[i];
          var formal = ctor.Formals[i];
          if (formal.IsGhost) {
            // nothing to compile, but do a sanity check
            Contract.Assert(!Contract.Exists(arg.Vars, bv => !bv.IsGhost));
          } else {
            var sw = new ConcreteSyntaxTree(wr.RelativeIndentLevel);
            EmitDestructor(rhsString, formal, k, ctor, ((DatatypeValue)pat.Expr).InferredTypeArgs, arg.Expr.Type, sw);
            wr = TrCasePattern(arg, sw.ToString(), formal.Type.Subst(typeSubst), bodyType, wr, ref wStmts);
            k++;
          }
        }
      }
      return wr;
    }

    protected void CompileSpecialFunctionCallExpr(FunctionCallExpr e, ConcreteSyntaxTree wr, bool inLetExprBody,
        ConcreteSyntaxTree wStmts, FCE_Arg_Translator tr) {
      string name = e.Function.Name;

      if (name == "RotateLeft") {
        EmitRotate(e.Receiver, e.Args[0], true, wr, inLetExprBody, wStmts, tr);
      } else if (name == "RotateRight") {
        EmitRotate(e.Receiver, e.Args[0], false, wr, inLetExprBody, wStmts, tr);
      } else {
        CompileFunctionCallExpr(e, wr, inLetExprBody, wStmts, tr);
      }
    }

    protected void CompileFunctionCallExpr(FunctionCallExpr e, ConcreteSyntaxTree wr, bool inLetExprBody,
        ConcreteSyntaxTree wStmts, FCE_Arg_Translator tr) {
      Contract.Requires(e != null && e.Function != null);
      Contract.Requires(wr != null);
      Contract.Requires(tr != null);
      Function f = e.Function;

      var toType = thisContext == null ? e.Type : e.Type.Subst(thisContext.ParentFormalTypeParametersToActuals);
      wr = EmitCoercionIfNecessary(f.Original.ResultType, toType, e.tok, wr);

      var customReceiver = !(f.EnclosingClass is TraitDecl) && NeedsCustomReceiver(f);
      string qual = "";
      string compileName = "";
      if (f.IsExtern(out qual, out compileName) && qual != null) {
        wr.Write("{0}{1}", qual, ModuleSeparator);
      } else if (f.IsStatic || customReceiver) {
        wr.Write("{0}{1}", TypeName_Companion(e.Receiver.Type, wr, e.tok, f), ModuleSeparator);
        compileName = IdName(f);
      } else {
        wr.Write("(");
        wr.Append(tr(e.Receiver, inLetExprBody, wStmts));
        wr.Write("){0}", ClassAccessor);
        compileName = IdName(f);
      }
      var typeArgs = CombineAllTypeArguments(f, e.TypeApplication_AtEnclosingClass, e.TypeApplication_JustFunction);
      EmitNameAndActualTypeArgs(compileName, TypeArgumentInstantiation.ToActuals(ForTypeParameters(typeArgs, f, false)), f.tok, wr);
      wr.Write("(");
      var sep = "";
      EmitTypeDescriptorsActuals(ForTypeDescriptors(typeArgs, f.EnclosingClass, f, false), e.tok, wr, ref sep);
      if (customReceiver) {
        wr.Write(sep);
        var w = EmitCoercionIfNecessary(e.Receiver.Type, UserDefinedType.UpcastToMemberEnclosingType(e.Receiver.Type, e.Function), e.tok, wr);
        w.Append(TrExpr(e.Receiver, inLetExprBody, wStmts));
        sep = ", ";
      }
      for (int i = 0; i < e.Args.Count; i++) {
        if (!e.Function.Formals[i].IsGhost) {
          wr.Write(sep);
          var fromType = e.Args[i].Type;
          var w = EmitCoercionIfNecessary(fromType, e.Function.Formals[i].Type, tok: e.tok, wr: wr);
          var instantiatedToType = e.Function.Formals[i].Type.Subst(e.TypeArgumentSubstitutionsWithParents());
          w = EmitDowncastIfNecessary(fromType, instantiatedToType, e.tok, w);
          w.Append(tr(e.Args[i], inLetExprBody, wStmts));
          sep = ", ";
        }
      }
      wr.Write(")");
    }

    protected bool IsComparisonToZero(BinaryExpr expr, out Expression/*?*/ arg, out int sign, out bool negated) {
      int s;
      if (IsComparisonWithZeroOnRight(expr.Op, expr.E1, out s, out negated)) {
        // e.g. x < 0
        arg = expr.E0;
        sign = s;
        return true;
      } else if (IsComparisonWithZeroOnRight(expr.Op, expr.E0, out s, out negated)) {
        // e.g. 0 < x, equivalent to x < 0
        arg = expr.E1;
        sign = -s;
        return true;
      } else {
        arg = null;
        sign = 0;
        return false;
      }
    }

    private bool IsComparisonWithZeroOnRight(
      BinaryExpr.Opcode op, Expression right,
      out int sign, out bool negated) {

      var rightVal = PartiallyEvaluate(right);
      if (rightVal == null || rightVal != BigInteger.Zero) {
        sign = 0; // need to assign something
        negated = true; // need to assign something
        return false;
      } else {
        switch (op) {
          case BinaryExpr.Opcode.Lt:
            // x < 0 <==> sign(x) == -1
            sign = -1;
            negated = false;
            return true;
          case BinaryExpr.Opcode.Le:
            // x <= 0 <==> sign(x) != 1
            sign = 1;
            negated = true;
            return true;
          case BinaryExpr.Opcode.Eq:
            // x == 0 <==> sign(x) == 0
            sign = 0;
            negated = false;
            return true;
          case BinaryExpr.Opcode.Neq:
            // x != 0 <==> sign(x) != 0
            sign = 0;
            negated = true;
            return true;
          case BinaryExpr.Opcode.Gt:
            // x > 0 <==> sign(x) == 1
            sign = 1;
            negated = false;
            return true;
          case BinaryExpr.Opcode.Ge:
            // x >= 0 <==> sign(x) != -1
            sign = -1;
            negated = true;
            return true;
          default:
            sign = 0; // need to assign something
            negated = false; // ditto
            return false;
        }
      }
    }

    protected abstract void EmitHaltRecoveryStmt(Statement body, string haltMessageVarName, Statement recoveryBody, ConcreteSyntaxTree wr);
}

public class CoverageInstrumenter {
  private readonly ConcreteSinglePassCompiler compiler;
  private List<(IToken, string)>/*?*/ legend;  // non-null implies DafnyOptions.O.CoverageLegendFile is non-null

  public CoverageInstrumenter(ConcreteSinglePassCompiler compiler) {
    this.compiler = compiler;
    if (DafnyOptions.O.CoverageLegendFile != null) {
      legend = new List<(IToken, string)>();
    }
  }

  public bool IsRecording {
    get => legend != null;
  }

  public void Instrument(IToken tok, string description, ConcreteSyntaxTree wr) {
    Contract.Requires(tok != null);
    Contract.Requires(description != null);
    Contract.Requires(wr != null || !IsRecording);
    if (legend != null) {
      wr.Write("DafnyProfiling.CodeCoverage.Record({0})", legend.Count);
      compiler.EndStmt(wr);
      legend.Add((tok, description));
    }
  }

  public void UnusedInstrumentationPoint(IToken tok, string description) {
    Contract.Requires(tok != null);
    Contract.Requires(description != null);
    if (legend != null) {
      legend.Add((tok, description));
    }
  }

  public void InstrumentExpr(IToken tok, string description, bool resultValue, ConcreteSyntaxTree wr) {
    Contract.Requires(tok != null);
    Contract.Requires(description != null);
    Contract.Requires(wr != null || !IsRecording);
    if (legend != null) {
      // The "Record" call always returns "true", so we negate it to get the value "false"
      wr.Write("{1}DafnyProfiling.CodeCoverage.Record({0})", legend.Count, resultValue ? "" : "!");
      legend.Add((tok, description));
    }
  }

  /// <summary>
  /// Should be called once "n" has reached its final value
  /// </summary>
  public void EmitSetup(ConcreteSyntaxTree wr) {
    Contract.Requires(wr != null);
    if (legend != null) {
      wr.Write("DafnyProfiling.CodeCoverage.Setup({0})", legend.Count);
      compiler.EndStmt(wr);
    }
  }

  public void EmitTearDown(ConcreteSyntaxTree wr) {
    Contract.Requires(wr != null);
    if (legend != null) {
      wr.Write("DafnyProfiling.CodeCoverage.TearDown()");
      compiler.EndStmt(wr);
    }
  }

  public void WriteLegendFile() {
    if (legend != null) {
      var filename = DafnyOptions.O.CoverageLegendFile;
      Contract.Assert(filename != null);
      TextWriter wr = filename == "-" ? System.Console.Out : new StreamWriter(new FileStream(Path.GetFullPath(filename), System.IO.FileMode.Create));
      {
        for (var i = 0; i < legend.Count; i++) {
          var e = legend[i];
          wr.WriteLine("{0}: {1}({2},{3}): {4}", i, e.Item1.Filename, e.Item1.line, e.Item1.col, e.Item2);
        }
      }
      legend = null;
    }
  }
}