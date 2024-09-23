using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using DafnyCore.Verifier;
using Microsoft.Boogie;
using Bpl = Microsoft.Boogie;
using BplParser = Microsoft.Boogie.Parser;
using static Microsoft.Dafny.Util;
using Action = System.Action;
using PODesc = Microsoft.Dafny.ProofObligationDescription;

namespace Microsoft.Dafny;

public partial class BoogieGenerator {
  public const string FrameVariablePrefix = "$Frame$";

  private void TrStmt(Statement stmt, BoogieStmtListBuilder builder,
    List<Variable> locals, ExpressionTranslator etran) {

    stmt.ScopeDepth = builder.Context.ScopeDepth;

    Contract.Requires(stmt != null);
    Contract.Requires(builder != null);
    Contract.Requires(locals != null);
    Contract.Requires(etran != null);
    Contract.Requires(codeContext != null && predef != null);
    Contract.Ensures(fuelContext == Contract.OldValue(fuelContext));

    stmtContext = StmtType.NONE;
    adjustFuelForExists = true;  // fuel for exists might need to be adjusted based on whether it's in an assert or assume stmt.
    if (stmt is PredicateStmt predicateStmt) {
      TrPredicateStmt(predicateStmt, builder, locals, etran);

    } else if (stmt is PrintStmt) {
      AddComment(builder, stmt, "print statement");
      PrintStmt s = (PrintStmt)stmt;
      foreach (var arg in s.Args) {
        TrStmt_CheckWellformed(arg, builder, locals, etran, false);
      }
      if (options.TestGenOptions.Mode != TestGenerationOptions.Modes.None) {
        builder.AddCaptureState(s);
      }

    } else if (stmt is HideRevealStmt revealStmt) {
      TranslateRevealStmt(builder, locals, etran, revealStmt);
    } else if (stmt is BreakStmt) {
      var s = (BreakStmt)stmt;
      AddComment(builder, stmt, $"{s.Kind} statement");
      foreach (var _ in Enumerable.Range(0, builder.Context.ScopeDepth - s.TargetStmt.ScopeDepth)) {
        builder.Add(new ChangeScope(s.Tok, ChangeScope.Modes.Pop));
      }
      var lbl = (s.IsContinue ? "continue_" : "after_") + s.TargetStmt.Labels.Data.AssignUniqueId(CurrentIdGenerator);
      builder.Add(new GotoCmd(s.Tok, new List<string> { lbl }));
    } else if (stmt is ReturnStmt) {
      var s = (ReturnStmt)stmt;
      AddComment(builder, stmt, "return statement");
      if (s.ReverifyPost) {
        // $_reverifyPost := true;
        builder.Add(Bpl.Cmd.SimpleAssign(s.Tok, new Bpl.IdentifierExpr(s.Tok, "$_reverifyPost", Bpl.Type.Bool), Bpl.Expr.True));
      }
      if (s.HiddenUpdate != null) {
        TrStmt(s.HiddenUpdate, builder, locals, etran);
      }
      if (codeContext is IMethodCodeContext) {
        var method = (IMethodCodeContext)codeContext;
        method.Outs.ForEach(p => CheckDefiniteAssignmentReturn(stmt.Tok, p, builder));
      }

      if (codeContext is Method { FunctionFromWhichThisIsByMethodDecl: { ByMethodTok: { } } fun } method2) {
        AssumeCanCallForByMethodDecl(method2, builder);
      }

      foreach (var _ in Enumerable.Range(0, builder.Context.ScopeDepth)) {
        builder.Add(new ChangeScope(s.Tok, ChangeScope.Modes.Pop));
      }
      builder.Add(new Bpl.ReturnCmd(stmt.Tok));
    } else if (stmt is YieldStmt) {
      var s = (YieldStmt)stmt;
      AddComment(builder, s, "yield statement");
      Contract.Assert(codeContext is IteratorDecl);
      var iter = (IteratorDecl)codeContext;
      // if the yield statement has arguments, do them first
      if (s.HiddenUpdate != null) {
        TrStmt(s.HiddenUpdate, builder, locals, etran);
      }
      // this.ys := this.ys + [this.y];
      var th = new ThisExpr(iter);
      var dafnyOutExprs = new List<Expression>();
      Contract.Assert(iter.OutsFields.Count == iter.OutsHistoryFields.Count);
      for (int i = 0; i < iter.OutsFields.Count; i++) {
        var y = iter.OutsFields[i];
        var dafnyY = new MemberSelectExpr(s.Tok, th, y);
        dafnyOutExprs.Add(dafnyY);
        var ys = iter.OutsHistoryFields[i];
        var dafnyYs = new MemberSelectExpr(s.Tok, th, ys);
        var dafnySingletonY = new SeqDisplayExpr(s.Tok, new List<Expression>() { dafnyY });
        dafnySingletonY.Type = ys.Type;  // resolve here
        var rhs = new BinaryExpr(s.Tok, BinaryExpr.Opcode.Add, dafnyYs, dafnySingletonY);
        rhs.ResolvedOp = BinaryExpr.ResolvedOpcode.Concat;
        rhs.Type = ys.Type;  // resolve here
        var cmd = Bpl.Cmd.SimpleAssign(s.Tok, etran.HeapCastToIdentifierExpr,
          UpdateHeap(s.Tok, etran.HeapExpr, etran.TrExpr(th), new Bpl.IdentifierExpr(s.Tok, GetField(ys)), etran.TrExpr(rhs)));
        builder.Add(cmd);
      }
      // yieldCount := yieldCount + 1;  assume yieldCount == |ys|;
      var yc = new Bpl.IdentifierExpr(s.Tok, yieldCountVariable);
      var incYieldCount = Bpl.Cmd.SimpleAssign(s.Tok, yc, Bpl.Expr.Binary(s.Tok, Bpl.BinaryOperator.Opcode.Add, yc, Bpl.Expr.Literal(1)));
      builder.Add(incYieldCount);
      builder.Add(TrAssumeCmd(s.Tok, YieldCountAssumption(iter, etran)));
      // assume $IsGoodHeap($Heap);
      builder.Add(AssumeGoodHeap(s.Tok, etran));
      // assert YieldEnsures[subst];  // where 'subst' replaces "old(E)" with "E" being evaluated in $_OldIterHeap
      var yeEtran = new ExpressionTranslator(this, predef, etran.HeapExpr, new Bpl.IdentifierExpr(s.Tok, "$_OldIterHeap", predef.HeapType), iter);

      var rhss = s.Rhss == null
        ? dafnyOutExprs
        : s.Rhss.Select(rhs => rhs is ExprRhs e ? e.Expr : null).ToList();
      var fieldSubstMap = iter.OutsFields.Zip(rhss)
        .Where(outRhs => outRhs.Second != null)
        .ToDictionary(
          outRhs => outRhs.First.Name,
          outRhs => outRhs.Second
        );
      var fieldSub = new SpecialFieldSubstituter(fieldSubstMap);

      foreach (var p in iter.YieldEnsures) {
        var ss = TrSplitExpr(builder.Context, p.E, yeEtran, true, out var splitHappened);
        foreach (var split in ss) {
          if (RefinementToken.IsInherited(split.Tok, currentModule)) {
            // this postcondition was inherited into this module, so just ignore it
          } else if (split.IsChecked) {
            var yieldToken = new NestedToken(s.Tok, split.Tok);
            var desc = new PODesc.YieldEnsures(fieldSub.Substitute(p.E));
            builder.Add(AssertNS(yieldToken, split.E, desc, stmt.Tok, null));
          }
        }
        builder.Add(TrAssumeCmdWithDependencies(yeEtran, stmt.Tok, p.E, "yield ensures clause"));
      }
      YieldHavoc(iter.tok, iter, builder, etran);
      builder.AddCaptureState(s);

    } else if (stmt is AssignSuchThatStmt) {
      var s = (AssignSuchThatStmt)stmt;
      AddComment(builder, s, "assign-such-that statement");
      // Essentially, treat like an assert (that values for the LHS exist), a havoc (of the LHS), and an
      // assume (of the RHS).  However, we also need to check the well-formedness of the LHS and RHS.
      // The well-formedness of the LHS is done by the havoc. The well-formedness of the RHS is most
      // easily done after that havoc, but we need to be careful about two things:
      //   - We should not generate any errors for uses of LHSs. This is not an issue for fields or
      //     array elements, because they already have values before reaching the assign-such-that statement.
      //     (Note, "this.f" is not allowed as a LHS in the first division of a constructor.)
      //     For any local variable or out-parameter x that's a LHS, we create a new variable x' and
      //     substitute x' for x in the RHS before doing the well-formedness check.
      //   - The well-formedness checks need to be able to assume that each x' has a value of its
      //     type. However, this assumption must not carry over to the existence assertion, because
      //     then everything will be provable if x' is of a known-empty type. Instead, the well-formedness
      //     check is wrapped inside an "if" whose guard is the type antecedent. After the existence
      //     check, the type antecedent is assumed of the original x, the RHS is assumed, and x is
      //     marked off has having been definitely assigned.
      //
      // So, the Dafny statement
      //     E.f, x :| RHS(x);
      // is translated into the following Boogie code:
      //     var x';
      //     Tr[[ E.f := *; ]]  // this havoc is translated like a Dafny assignment statement, which means
      //                        // the well-formedness of E is checked here
      //     if (typeAntecedent(x')) {
      //       check well-formedness of RHS(x');
      //     }
      //     assert (exists x'' :: RHS(x''));  // for ":| assume", omit this line; for ":|", LHS is only allowed to contain simple variables
      //     defass#x := true;
      //     havoc x;
      //     assume RHS(x);

      var simpleLHSs = new List<IdentifierExpr>();
      Bpl.Expr typeAntecedent = Bpl.Expr.True;
      var havocLHSs = new List<Expression>();
      var havocRHSs = new List<AssignmentRhs>();
      var substMap = new Dictionary<IVariable, Expression>();
      foreach (var lhs in s.Lhss) {
        var lvalue = lhs.Resolved;
        if (lvalue is IdentifierExpr ide) {
          simpleLHSs.Add(ide);
          var wh = SetupVariableAsLocal(ide.Var, substMap, builder, locals, etran);
          typeAntecedent = BplAnd(typeAntecedent, wh);
        } else {
          havocLHSs.Add(lvalue);
          havocRHSs.Add(new HavocRhs(lhs.tok));  // note, a HavocRhs is constructed as already resolved
        }
      }
      ProcessLhss(havocLHSs, false, true, builder, locals, etran, stmt, out var lhsBuilder, out var bLhss, out _, out _, out _);
      ProcessRhss(lhsBuilder, bLhss, havocLHSs, havocRHSs, builder, locals, etran, stmt);

      // Here comes the well-formedness check
      var wellFormednessBuilder = new BoogieStmtListBuilder(this, options, builder.Context);
      var rhs = Substitute(s.Expr, null, substMap);
      TrStmt_CheckWellformed(rhs, wellFormednessBuilder, locals, etran, false);
      var ifCmd = new Bpl.IfCmd(s.Tok, typeAntecedent, wellFormednessBuilder.Collect(s.Tok), null, null);
      builder.Add(ifCmd);

      // Here comes the assert part
      if (s.AssumeToken == null) {
        substMap = new Dictionary<IVariable, Expression>();
        var bvars = new List<BoundVar>();
        foreach (var lhs in s.Lhss) {
          var l = lhs.Resolved;
          if (l is IdentifierExpr x) {
            CloneVariableAsBoundVar(x.tok, x.Var, "$as#" + x.Name, out var bv, out var ie);
            bvars.Add(bv);
            substMap.Add(x.Var, ie);
          } else {
            // other forms of LHSs have been ruled out by the resolver (it would be possible to
            // handle them, but it would involve heap-update expressions--the translation would take
            // effort, and it's not certain that the existential would be successful in verification).
            Contract.Assume(false);  // unexpected case
          }
        }

        GenerateAndCheckGuesses(s.Tok, bvars, s.Bounds, Substitute(s.Expr, null, substMap), TrTrigger(etran, s.Attributes, s.Tok, substMap), builder, etran);
      }

      // Mark off the simple variables as having definitely been assigned AND THEN havoc their values. By doing them
      // in this order, the type antecedents will in effect be assumed.
      var bHavocLHSs = new List<Bpl.IdentifierExpr>();
      foreach (var lhs in simpleLHSs) {
        MarkDefiniteAssignmentTracker(lhs, builder);
        bHavocLHSs.Add((Bpl.IdentifierExpr)etran.TrExpr(lhs));
      }
      builder.Add(new Bpl.HavocCmd(s.Tok, bHavocLHSs));

      // End by doing the assume
      builder.Add(TrAssumeCmdWithDependencies(etran, s.Tok, s.Expr, "assign-such-that constraint"));
      builder.AddCaptureState(s);  // just do one capture state--here, at the very end (that is, don't do one before the assume)

    } else if (stmt is AssignStatement statement) {
      TrUpdateStmt(builder, locals, etran, statement);
    } else if (stmt is AssignOrReturnStmt) {
      AddComment(builder, stmt, "assign-or-return statement (:-)");
      AssignOrReturnStmt s = (AssignOrReturnStmt)stmt;
      TrStmtList(s.ResolvedStatements, builder, locals, etran);

    } else if (stmt is SingleAssignStmt) {
      AddComment(builder, stmt, "assignment statement");
      SingleAssignStmt s = (SingleAssignStmt)stmt;
      TrAssignment(stmt, s.Lhs.Resolved, s.Rhs, builder, locals, etran);

    } else if (stmt is CallStmt) {
      AddComment(builder, stmt, "call statement");
      TrCallStmt((CallStmt)stmt, builder, locals, etran, null);

    } else if (stmt is DividedBlockStmt) {
      var s = (DividedBlockStmt)stmt;
      AddComment(builder, stmt, "divided block before new;");
      var prevDefiniteAssignmentTrackerCount = DefiniteAssignmentTrackers.Count;
      var tok = s.SeparatorTok ?? s.Tok;
      // a DividedBlockStmt occurs only inside a Constructor body of a class
      var cl = (ClassDecl)((Constructor)codeContext).EnclosingClass;
      var fields = Concat(cl.InheritedMembers, cl.Members).ConvertAll(member =>
        member is Field && !member.IsStatic && !member.IsInstanceIndependentConstant ? (Field)member : null);
      fields.RemoveAll(f => f == null);
      var localSurrogates = fields.ConvertAll(f => new Bpl.LocalVariable(f.tok, new TypedIdent(f.tok, SurrogateName(f), TrType(f.Type))));
      locals.AddRange(localSurrogates);
      fields.ForEach(f => AddDefiniteAssignmentTrackerSurrogate(f, cl, locals, codeContext is Constructor && codeContext.IsGhost));

      Contract.Assert(!inBodyInitContext);
      inBodyInitContext = true;
      TrStmtList(s.BodyInit, builder, locals, etran);
      Contract.Assert(inBodyInitContext);
      inBodyInitContext = false;

      // The "new;" translates into an allocation of "this"
      AddComment(builder, stmt, "new;");
      fields.ForEach(f => CheckDefiniteAssignmentSurrogate(s.SeparatorTok ?? s.RangeToken.EndToken, f, true, builder));
      fields.ForEach(RemoveDefiniteAssignmentTrackerSurrogate);
      var th = new ThisExpr(cl);
      var bplThis = (Bpl.IdentifierExpr)etran.TrExpr(th);
      SelectAllocateObject(tok, bplThis, th.Type, false, builder, etran);
      for (int i = 0; i < fields.Count; i++) {
        // assume $Heap[this, f] == this.f;
        var mse = new MemberSelectExpr(tok, th, fields[i]);
        Bpl.Expr surr = new Bpl.IdentifierExpr(tok, localSurrogates[i]);
        surr = CondApplyUnbox(tok, surr, fields[i].Type, mse.Type);
        builder.Add(new Bpl.AssumeCmd(tok, Bpl.Expr.Eq(etran.TrExpr(mse), surr)));
      }
      CommitAllocatedObject(tok, bplThis, null, builder, etran);

      AddComment(builder, stmt, "divided block after new;");
      TrStmtList(s.BodyProper, builder, locals, etran);
      RemoveDefiniteAssignmentTrackers(s.Body, prevDefiniteAssignmentTrackerCount);

    } else if (stmt is OpaqueBlock opaqueBlock) {
      OpaqueBlockVerifier.EmitBoogie(this, opaqueBlock, builder, locals, etran, (IMethodCodeContext)codeContext);
    } else if (stmt is BlockStmt blockStmt) {
      var prevDefiniteAssignmentTrackerCount = DefiniteAssignmentTrackers.Count;
      TrStmtList(blockStmt.Body, builder, locals, etran, blockStmt.RangeToken);
      RemoveDefiniteAssignmentTrackers(blockStmt.Body, prevDefiniteAssignmentTrackerCount);
    } else if (stmt is IfStmt ifStmt) {
      TrIfStmt(ifStmt, builder, locals, etran);

    } else if (stmt is AlternativeStmt) {
      AddComment(builder, stmt, "alternative statement");
      var s = (AlternativeStmt)stmt;
      var elseCase = Assert(s.Tok, Bpl.Expr.False, new PODesc.AlternativeIsComplete());
      TrAlternatives(s.Alternatives, s.Tok, b => b.Add(elseCase), builder, locals, etran, stmt.IsGhost);

    } else if (stmt is WhileStmt whileStmt) {
      TrWhileStmt(whileStmt, builder, locals, etran);

    } else if (stmt is AlternativeLoopStmt alternativeLoopStmt) {
      TrAlternativeLoopStmt(alternativeLoopStmt, builder, locals, etran);
    } else if (stmt is ForLoopStmt forLoopStmt) {
      TrForLoop(forLoopStmt, builder, locals, etran);

    } else if (stmt is ModifyStmt) {
      AddComment(builder, stmt, "modify statement");
      var s = (ModifyStmt)stmt;
      // check well-formedness of the modifies clauses
      var wfOptions = new WFOptions();
      CheckFrameWellFormed(wfOptions, s.Mod.Expressions, locals, builder, etran);
      // check that the modifies is a subset
      var desc = new PODesc.ModifyFrameSubset("modify statement", s.Mod.Expressions, GetContextModifiesFrames());
      CheckFrameSubset(s.Tok, s.Mod.Expressions, null, null, etran, etran.ModifiesFrame(s.Tok), builder, desc, null);
      // cause the change of the heap according to the given frame
      var suffix = CurrentIdGenerator.FreshId("modify#");
      string modifyFrameName = FrameVariablePrefix + suffix;
      var preModifyHeapVar = new Bpl.LocalVariable(s.Tok, new Bpl.TypedIdent(s.Tok, "$PreModifyHeap$" + suffix, predef.HeapType));
      locals.Add(preModifyHeapVar);
      DefineFrame(s.Tok, etran.ModifiesFrame(s.Tok), s.Mod.Expressions, builder, locals, modifyFrameName);
      if (s.Body == null) {
        var preModifyHeap = new Bpl.IdentifierExpr(s.Tok, preModifyHeapVar);
        // preModifyHeap := $Heap;
        builder.Add(Bpl.Cmd.SimpleAssign(s.Tok, preModifyHeap, etran.HeapExpr));
        // havoc $Heap;
        builder.Add(new Bpl.HavocCmd(s.Tok, new List<Bpl.IdentifierExpr> { etran.HeapCastToIdentifierExpr }));
        // assume $HeapSucc(preModifyHeap, $Heap);   OR $HeapSuccGhost
        builder.Add(TrAssumeCmd(s.Tok, HeapSucc(preModifyHeap, etran.HeapExpr, s.IsGhost)));
        // assume nothing outside the frame was changed
        var etranPreLoop = new ExpressionTranslator(this, predef, preModifyHeap, this.currentDeclaration is IFrameScope fs ? fs : null);
        var updatedFrameEtran = etran.WithModifiesFrame(modifyFrameName);
        builder.Add(TrAssumeCmd(s.Tok, FrameConditionUsingDefinedFrame(s.Tok, etranPreLoop, etran, updatedFrameEtran, updatedFrameEtran.ModifiesFrame(s.Tok))));
      } else {
        // do the body, but with preModifyHeapVar as the governing frame
        var updatedFrameEtran = etran.WithModifiesFrame(modifyFrameName);
        CurrentIdGenerator.Push();
        TrStmt(s.Body, builder, locals, updatedFrameEtran);
        CurrentIdGenerator.Pop();
      }
      builder.AddCaptureState(stmt);

    } else if (stmt is ForallStmt forallStmt) {
      TrForallStmt(builder, locals, etran, forallStmt);
    } else if (stmt is CalcStmt calcStmt) {
      TrCalcStmt(calcStmt, builder, locals, etran);
    } else if (stmt is NestedMatchStmt nestedMatchStmt) {
      TrStmt(nestedMatchStmt.Flattened, builder, locals, etran);
    } else if (stmt is MatchStmt matchStmt) {
      TrMatchStmt(matchStmt, builder, locals, etran);
    } else if (stmt is VarDeclStmt) {
      var s = (VarDeclStmt)stmt;
      TrVarDeclStmt(s, builder, locals, etran);
    } else if (stmt is VarDeclPattern varDeclPattern) {
      foreach (var dafnyLocal in varDeclPattern.LocalVars) {
        var boogieLocal = new Bpl.LocalVariable(dafnyLocal.Tok,
          new Bpl.TypedIdent(dafnyLocal.Tok, dafnyLocal.AssignUniqueName(currentDeclaration.IdGenerator),
            TrType(dafnyLocal.Type)));
        locals.Add(boogieLocal);
        var variableReference = new Bpl.IdentifierExpr(boogieLocal.tok, boogieLocal);
        builder.Add(new Bpl.HavocCmd(dafnyLocal.Tok, new List<Bpl.IdentifierExpr>
        {
          variableReference
        }));
        var wh = GetWhereClause(dafnyLocal.Tok, variableReference, dafnyLocal.Type, etran,
          isAllocContext.Var(varDeclPattern.IsGhost, dafnyLocal));
        if (wh != null) {
          builder.Add(TrAssumeCmd(dafnyLocal.Tok, wh));
        }
      }

      var varNameGen = CurrentIdGenerator.NestedFreshIdGenerator("let#");
      var pat = varDeclPattern.LHS;
      var rhs = varDeclPattern.RHS;
      var nm = varNameGen.FreshId("#0#");
      var boogieTupleLocal = new Bpl.LocalVariable(pat.tok, new TypedIdent(pat.tok, nm, TrType(rhs.Type)));
      locals.Add(boogieTupleLocal);
      var boogieTupleReference = new Bpl.IdentifierExpr(rhs.tok, boogieTupleLocal);

      void AddResultCommands(BoogieStmtListBuilder returnBuilder, Expression result) {
        Contract.Assert(pat.Expr.Type != null);
        var bResult = etran.TrExpr(result);
        CheckSubrange(result.tok, bResult, rhs.Type, pat.Expr.Type, rhs, returnBuilder);
        returnBuilder.Add(TrAssumeCmdWithDependenciesAndExtend(etran, rhs.tok, rhs,
          e => Bpl.Expr.Eq(boogieTupleReference, AdaptBoxing(rhs.tok, e, rhs.Type, pat.Expr.Type))));
      }

      CheckWellformedWithResult(rhs, new WFOptions(null, false, false), locals, builder, etran, AddResultCommands);
      builder.Add(TrAssumeCmd(rhs.tok, etran.CanCallAssumption(rhs)));
      builder.Add(new CommentCmd("CheckWellformedWithResult: any expression"));
      builder.Add(TrAssumeCmd(rhs.tok, MkIs(boogieTupleReference, pat.Expr.Type)));

      CheckCasePatternShape(pat, rhs, boogieTupleReference, rhs.tok, pat.Expr.Type, builder);
      builder.Add(TrAssumeCmdWithDependenciesAndExtend(etran, varDeclPattern.tok, pat.Expr,
        e => Expr.Eq(e, boogieTupleReference), "variable declaration"));
    } else if (stmt is TryRecoverStatement haltRecoveryStatement) {
      // try/recover statements are currently internal-only AST nodes that cannot be
      // directly used in user Dafny code. They are only generated by rewriters, and verifying
      // their use is low value.
      throw new NotSupportedException("Verification of try/recover statements is not supported");
    } else {
      Contract.Assert(false); throw new cce.UnreachableException();  // unexpected statement
    }
  }

  private void TrUpdateStmt(BoogieStmtListBuilder builder, List<Variable> locals, ExpressionTranslator etran, AssignStatement statement) {
    // This UpdateStmt can be single-target assignment, a multi-assignment, a call statement, or
    // an array-range update.  Handle the multi-assignment here and handle the others as for .ResolvedStatements.
    var resolved = statement.ResolvedStatements;
    if (resolved.Count == 1) {
      TrStmt(resolved[0], builder, locals, etran);
    } else {
      AddComment(builder, statement, "update statement");
      var assignStmts = resolved.Cast<SingleAssignStmt>().ToList();
      var lhss = assignStmts.Select(a => a.Lhs).ToList();
      var rhss = assignStmts.Select(a => a.Rhs).ToList();
      // note: because we have more than one expression, we always must assign to Boogie locals in a two
      // phase operation. Thus rhssCanAffectPreviouslyKnownExpressions is just true.
      Contract.Assert(1 < lhss.Count);

      ProcessLhss(lhss, true, false, builder, locals, etran, statement, out var lhsBuilder, out var bLhss, out var lhsObjs, out var lhsFields, out var lhsNames);
      // We know that, because the translation saves to a local variable, that the RHS always need to
      // generate a new local, i.e. bLhss is just all nulls.
      Contract.Assert(Contract.ForAll(bLhss, lhs => lhs == null));
      // This generates the assignments, and gives them to us as finalRhss.
      var finalRhss = ProcessUpdateAssignRhss(lhss, rhss, builder, locals, etran, statement);
      // ProcessLhss has laid down framing conditions and the ProcessUpdateAssignRhss will check subranges (nats),
      // but we need to generate the distinctness condition (two LHS are equal only when the RHS is also
      // equal). We need both the LHS and the RHS to do this, which is why we need to do it here.
      CheckLhssDistinctness(finalRhss, statement.Rhss, lhss, builder, etran, lhsObjs, lhsFields, lhsNames, statement.OriginalInitialLhs);
      // Now actually perform the assignments to the LHS.
      for (int i = 0; i < lhss.Count; i++) {
        lhsBuilder[i](finalRhss[i], statement.Rhss[i] is HavocRhs, builder, etran);
      }
      builder.AddCaptureState(statement);
    }
  }

  void TrVarDeclStmt(VarDeclStmt varDeclStmt, BoogieStmtListBuilder builder, List<Variable> locals, ExpressionTranslator etran) {

    var newLocalIds = new List<Bpl.IdentifierExpr>();
    int i = 0;
    foreach (var local in varDeclStmt.Locals) {
      Bpl.Type varType = TrType(local.Type);
      Bpl.Expr wh = GetWhereClause(local.Tok,
        new Bpl.IdentifierExpr(local.Tok, local.AssignUniqueName(currentDeclaration.IdGenerator), varType),
        local.Type, etran, isAllocContext.Var(varDeclStmt.IsGhost, local));
      // if needed, register definite-assignment tracking for this local
      var needDefiniteAssignmentTracking = varDeclStmt.Assign == null || varDeclStmt.Assign is AssignSuchThatStmt;
      if (varDeclStmt.Assign is AssignStatement) {
        // there is an initial assignment, but we need to look out for "*" being that assignment
        var us = (AssignStatement)varDeclStmt.Assign;
        if (i < us.Rhss.Count && us.Rhss[i] is HavocRhs) {
          needDefiniteAssignmentTracking = true;
        }
      }
      if (!local.Type.IsNonempty) {
        // This prevents generating an unsatisfiable where clause for possibly empty types
        needDefiniteAssignmentTracking = true;
      }
      if (needDefiniteAssignmentTracking) {
        var defassExpr = AddDefiniteAssignmentTracker(local, locals);
        if (wh != null && defassExpr != null) {
          // make the "where" expression be "defass ==> wh", because we don't want to assume anything
          // before the variable has been assigned (for a variable that needs definite-assignment tracking
          // in the first place)
          wh = BplImp(defassExpr, wh);
        }
      }
      // create the variable itself (now that "wh" may mention the definite-assignment tracker)
      Bpl.LocalVariable var = new Bpl.LocalVariable(local.Tok, new Bpl.TypedIdent(local.Tok, local.AssignUniqueName(currentDeclaration.IdGenerator), varType, wh));
      var.Attributes = etran.TrAttributes(local.Attributes, null);
      newLocalIds.Add(new Bpl.IdentifierExpr(local.Tok, var));
      locals.Add(var);
      i++;
    }
    if (varDeclStmt.Assign == null) {
      // it is necessary to do a havoc here in order to give the variable the correct allocation state
      builder.Add(new HavocCmd(varDeclStmt.Tok, newLocalIds));
    }
    // processing of "assumption" variables happens after the variable is possibly havocked above
    foreach (var local in varDeclStmt.Locals) {
      if (Attributes.Contains(local.Attributes, "assumption")) {
        Bpl.Type varType = TrType(local.Type);
        builder.Add(new AssumeCmd(local.Tok, new Bpl.IdentifierExpr(local.Tok, local.AssignUniqueName(currentDeclaration.IdGenerator), varType), new QKeyValue(local.Tok, "assumption_variable_initialization", new List<object>(), null)));
      }
    }
    if (varDeclStmt.Assign != null) {
      TrStmt(varDeclStmt.Assign, builder, locals, etran);
    }
  }

  private void TranslateRevealStmt(BoogieStmtListBuilder builder, List<Variable> locals, ExpressionTranslator etran,
    HideRevealStmt revealStmt) {
    AddComment(builder, revealStmt, "hide/reveal statement");
    foreach (var la in revealStmt.LabeledAsserts) {
      Contract.Assert(la.E != null);  // this should have been filled in by now
      builder.Add(new Bpl.AssumeCmd(revealStmt.Tok, la.E));
    }

    if (builder.Context.ContainsHide) {
      if (revealStmt.Wildcard) {
        builder.Add(new HideRevealCmd(revealStmt.Tok, revealStmt.Mode));
      } else {
        foreach (var member in revealStmt.OffsetMembers) {
          builder.Add(new HideRevealCmd(new Bpl.IdentifierExpr(revealStmt.Tok, member.FullSanitizedName), revealStmt.Mode));
        }
      }
    }

    TrStmtList(revealStmt.ResolvedStatements, builder, locals, etran);
  }

  private void TrCalcStmt(CalcStmt stmt, BoogieStmtListBuilder builder, List<Variable> locals, ExpressionTranslator etran) {
    Contract.Requires(stmt != null);
    Contract.Requires(builder != null);
    Contract.Requires(locals != null);
    Contract.Requires(etran != null);

    /* Translate into:
      if (*) {
          assert wf(line0);
      } else if (*) {
          assume wf(line0);
          // if op is ==>: assume line0;
          hint0;
          assert wf(line1);
          assert line0 op line1;
          assume false;
      } else if (*) { ...
      } else if (*) {
          assume wf(line<n-1>);
          // if op is ==>: assume line<n-1>;
          hint<n-1>;
          assert wf(line<n>);
          assert line<n-1> op line<n>;
          assume false;
      }
      assume line<0> op line<n>;
      */
    Contract.Assert(stmt.Steps.Count == stmt.Hints.Count); // established by the resolver
    AddComment(builder, stmt, "calc statement");
    this.fuelContext = FuelSetting.ExpandFuelContext(stmt.Attributes, stmt.Tok, this.fuelContext, this.reporter);
    DefineFuelConstant(stmt.Tok, stmt.Attributes, builder, etran);
    CurrentIdGenerator.Push(); // put the entire calc statement within its own sub-branch
    if (stmt.Lines.Count > 0) {
      Bpl.IfCmd ifCmd = null;
      BoogieStmtListBuilder b;
      // if the dangling hint is empty, do not generate anything for the dummy step
      var stepCount = stmt.Hints.Last().Body.Count == 0 ? stmt.Steps.Count - 1 : stmt.Steps.Count;
      // check steps:
      for (int i = stepCount; 0 <= --i;) {
        b = new BoogieStmtListBuilder(this, options, builder.Context);
        // assume wf[line<i>]:
        AddComment(b, stmt, "assume wf[lhs]");
        CurrentIdGenerator.Push();
        assertAsAssume = true;
        TrStmt_CheckWellformed(CalcStmt.Lhs(stmt.Steps[i]), b, locals, etran, false);
        assertAsAssume = false;
        if (stmt.Steps[i] is BinaryExpr && (((BinaryExpr)stmt.Steps[i]).ResolvedOp == BinaryExpr.ResolvedOpcode.Imp)) {
          // assume line<i>:
          AddComment(b, stmt, "assume lhs");
          b.Add(TrAssumeCmdWithDependencies(etran, stmt.Tok, CalcStmt.Lhs(stmt.Steps[i]), "calc statement step"));
        }
        // hint:
        AddComment(b, stmt, "Hint" + i.ToString());

        TrStmt(stmt.Hints[i], b, locals, etran);
        if (i < stmt.Steps.Count - 1) {
          // non-dummy step
          // check well formedness of the goal line:
          AddComment(b, stmt, "assert wf[rhs]");
          if (stmt.Steps[i] is TernaryExpr) {
            // check the prefix-equality limit
            var index = ((TernaryExpr)stmt.Steps[i]).E0;
            TrStmt_CheckWellformed(index, b, locals, etran, false);
            if (index.Type.IsNumericBased(Type.NumericPersuasion.Int)) {
              var desc = new PODesc.PrefixEqualityLimit(index);
              b.Add(AssertNS(index.tok, Bpl.Expr.Le(Bpl.Expr.Literal(0), etran.TrExpr(index)), desc));
            }
          }
          TrStmt_CheckWellformed(CalcStmt.Rhs(stmt.Steps[i]), b, locals, etran, false);
          var ss = TrSplitExpr(builder.Context, stmt.Steps[i], etran, true, out var splitHappened);
          // assert step:
          AddComment(b, stmt, "assert line" + i.ToString() + " " + (stmt.StepOps[i] ?? stmt.Op).ToString() + " line" + (i + 1).ToString());
          if (!splitHappened) {
            b.Add(AssertNS(stmt.Lines[i + 1].tok, etran.TrExpr(stmt.Steps[i]), new PODesc.CalculationStep(stmt.Steps[i], stmt.Hints[i])));
          } else {
            foreach (var split in ss) {
              if (split.IsChecked) {
                b.Add(AssertNS(stmt.Lines[i + 1].tok, split.E, new PODesc.CalculationStep(stmt.Steps[i], stmt.Hints[i])));
              }
            }
          }
        }
        b.Add(TrAssumeCmd(stmt.Tok, Bpl.Expr.False));
        ifCmd = new Bpl.IfCmd(stmt.Tok, null, b.Collect(stmt.Tok), ifCmd, null);
        CurrentIdGenerator.Pop();
      }
      // check well formedness of the first line:
      b = new BoogieStmtListBuilder(this, options, builder.Context);
      AddComment(b, stmt, "assert wf[initial]");
      Contract.Assert(stmt.Result != null); // established by the resolver
      TrStmt_CheckWellformed(CalcStmt.Lhs(stmt.Result), b, locals, etran, false);
      b.Add(TrAssumeCmd(stmt.Tok, Bpl.Expr.False));
      ifCmd = new Bpl.IfCmd(stmt.Tok, null, b.Collect(stmt.Tok), ifCmd, null);
      builder.Add(ifCmd);
      // assume result:
      if (stmt.Steps.Count > 1) {
        builder.Add(TrAssumeCmdWithDependencies(etran, stmt.Tok, stmt.Result, "calc statement result"));
      }
    }
    CurrentIdGenerator.Pop();
    this.fuelContext = FuelSetting.PopFuelContext();
  }

  private void TrMatchStmt(MatchStmt stmt, BoogieStmtListBuilder builder, List<Variable> locals, ExpressionTranslator etran) {
    Contract.Requires(stmt != null);
    Contract.Requires(builder != null);
    Contract.Requires(locals != null);
    Contract.Requires(etran != null);

    FillMissingCases(stmt);

    TrStmt_CheckWellformed(stmt.Source, builder, locals, etran, true);
    Bpl.Expr source = etran.TrExpr(stmt.Source);
    var b = new BoogieStmtListBuilder(this, options, builder.Context);
    b.Add(TrAssumeCmd(stmt.Tok, Bpl.Expr.False));
    Bpl.StmtList els = b.Collect(stmt.Tok);
    Bpl.IfCmd ifCmd = null;
    foreach (var missingCtor in stmt.MissingCases) {
      // havoc all bound variables
      b = new BoogieStmtListBuilder(this, options, builder.Context);
      List<Variable> newLocals = new List<Variable>();
      Bpl.Expr r = CtorInvocation(stmt.Tok, missingCtor, etran, newLocals, b);
      locals.AddRange(newLocals);

      if (newLocals.Count != 0) {
        List<Bpl.IdentifierExpr> havocIds = new List<Bpl.IdentifierExpr>();
        foreach (Variable local in newLocals) {
          havocIds.Add(new Bpl.IdentifierExpr(local.tok, local));
        }
        builder.Add(new Bpl.HavocCmd(stmt.Tok, havocIds));
      }
      String missingStr = stmt.Context.FillHole(new IdCtx(missingCtor)).AbstractAllHoles()
        .ToString();
      var desc = new PODesc.MatchIsComplete("statement", missingStr);
      b.Add(Assert(stmt.Tok, Bpl.Expr.False, desc));

      Bpl.Expr guard = Bpl.Expr.Eq(source, r);
      ifCmd = new Bpl.IfCmd(stmt.Tok, guard, b.Collect(stmt.Tok), ifCmd, els);
      els = null;
    }
    for (int i = stmt.Cases.Count; 0 <= --i;) {
      var mc = (MatchCaseStmt)stmt.Cases[i];
      CurrentIdGenerator.Push();
      // havoc all bound variables
      b = new BoogieStmtListBuilder(this, options, builder.Context);
      List<Variable> newLocals = new List<Variable>();
      Bpl.Expr r = CtorInvocation(mc, stmt.Source.Type, etran, newLocals, b, stmt.IsGhost ? NOALLOC : ISALLOC);
      locals.AddRange(newLocals);

      if (newLocals.Count != 0) {
        List<Bpl.IdentifierExpr> havocIds = new List<Bpl.IdentifierExpr>();
        foreach (Variable local in newLocals) {
          havocIds.Add(new Bpl.IdentifierExpr(local.tok, local));
        }
        builder.Add(new Bpl.HavocCmd(mc.tok, havocIds));
      }

      // translate the body into b
      var prevDefiniteAssignmentTrackerCount = DefiniteAssignmentTrackers.Count;
      TrStmtList(mc.Body, b, locals, etran);
      RemoveDefiniteAssignmentTrackers(mc.Body, prevDefiniteAssignmentTrackerCount);

      Bpl.Expr guard = Bpl.Expr.Eq(source, r);
      ifCmd = new Bpl.IfCmd(mc.tok, guard, b.Collect(mc.tok), ifCmd, els);
      els = null;
      CurrentIdGenerator.Pop();
    }
    if (ifCmd != null) {
      builder.Add(ifCmd);
    }
  }

  void FillMissingCases(IMatch match) {
    Contract.Requires(match != null);
    if (match.MissingCases.Any()) {
      return;
    }

    var dtd = match.Source.Type.AsDatatype;
    var constructors = dtd?.ConstructorsByName;

    ISet<string> memberNamesUsed = new HashSet<string>();

    foreach (var matchCase in match.Cases) {
      if (constructors != null) {
        Contract.Assert(dtd != null);
        var ctorId = matchCase.Ctor.Name;
        if (match.Source.Type.AsDatatype is TupleTypeDecl) {
          var tuple = (TupleTypeDecl)match.Source.Type.AsDatatype;
          ctorId = SystemModuleManager.TupleTypeCtorName(tuple.Dims);
        }

        if (constructors.ContainsKey(ctorId)) {
          memberNamesUsed.Add(ctorId); // add mc.Id to the set of names used
        }
      }
    }
    if (dtd != null && memberNamesUsed.Count != dtd.Ctors.Count) {
      // We could complain about the syntactic omission of constructors:
      //   Reporter.Error(MessageSource.Resolver, stmt, "match statement does not cover all constructors");
      // but instead we let the verifier do a semantic check.
      // So, for now, record the missing constructors:
      foreach (var ctr in dtd.Ctors) {
        if (!memberNamesUsed.Contains(ctr.Name)) {
          match.MissingCases.Add(ctr);
        }
      }
      Contract.Assert(memberNamesUsed.Count + match.MissingCases.Count == dtd.Ctors.Count);
    }
  }
  private static SubrangeCheckContext MakeNumericBoundsSubrangeCheckContext(BoundVar bvar, Expression lo, Expression hi) {
    var source = new IdentifierExpr(Token.NoToken, bvar);
    var loBound = lo == null ? null : new BinaryExpr(Token.NoToken, BinaryExpr.Opcode.Le, lo, source);
    var hiBound = hi == null ? null : new BinaryExpr(Token.NoToken, BinaryExpr.Opcode.Lt, source, hi);
    var bounds = (loBound, hiBound) switch {
      (not null, not null) => new BinaryExpr(Token.NoToken, BinaryExpr.Opcode.And, loBound, hiBound),
      (not null, null) => loBound,
      (null, not null) => hiBound,
    };
    Expression CheckContext(Expression check) => new ForallExpr(
      Token.NoToken,
      RangeToken.NoToken,
      new() { bvar },
      bounds,
      check,
      null
    );

    return CheckContext;
  }

  private void TrIfStmt(IfStmt stmt, BoogieStmtListBuilder builder, List<Variable> locals, ExpressionTranslator etran) {
    Contract.Requires(stmt != null);
    Contract.Requires(builder != null);
    Contract.Requires(locals != null);
    Contract.Requires(etran != null);

    AddComment(builder, stmt, "if statement");
    Expression guard;
    if (stmt.Guard == null) {
      guard = null;
    } else {
      guard = stmt.IsBindingGuard ? ((ExistsExpr)stmt.Guard).AlphaRename("eg$") : stmt.Guard;
      TrStmt_CheckWellformed(guard, builder, locals, etran, true);
    }
    BoogieStmtListBuilder b = new BoogieStmtListBuilder(this, options, builder.Context);
    if (stmt.IsBindingGuard) {
      CurrentIdGenerator.Push();
      var exists = (ExistsExpr)stmt.Guard; // the original (that is, not alpha-renamed) guard
      IntroduceAndAssignExistentialVars(exists, b, builder, locals, etran, stmt.IsGhost);
      CurrentIdGenerator.Pop();
    }
    CurrentIdGenerator.Push();
    Bpl.StmtList thn = TrStmt2StmtList(b, stmt.Thn, locals, etran, stmt.Thn is not BlockStmt);
    CurrentIdGenerator.Pop();
    Bpl.StmtList els;
    Bpl.IfCmd elsIf = null;
    b = new BoogieStmtListBuilder(this, options, builder.Context);
    if (stmt.IsBindingGuard) {
      b.Add(TrAssumeCmdWithDependenciesAndExtend(etran, guard.tok, guard, Expr.Not, "if statement binding guard"));
    }
    if (stmt.Els == null) {
      els = b.Collect(stmt.Tok);
    } else {
      CurrentIdGenerator.Push();
      els = TrStmt2StmtList(b, stmt.Els, locals, etran, stmt.Els is not BlockStmt);
      CurrentIdGenerator.Pop();
      if (els.BigBlocks.Count == 1) {
        Bpl.BigBlock bb = els.BigBlocks[0];
        if (bb.LabelName == null && bb.simpleCmds.Count == 0 && bb.ec is Bpl.IfCmd) {
          elsIf = (Bpl.IfCmd)bb.ec;
          els = null;
        }
      }
    }
    builder.Add(new Bpl.IfCmd(stmt.Tok, guard == null || stmt.IsBindingGuard ? null : etran.TrExpr(guard), thn, elsIf, els));
  }

  void TrAlternatives(List<GuardedAlternative> alternatives, IToken elseToken, Action<BoogieStmtListBuilder> buildElseCase,
    BoogieStmtListBuilder builder, List<Variable> locals, ExpressionTranslator etran, bool isGhost) {
    Contract.Requires(alternatives != null);
    Contract.Requires(builder != null);
    Contract.Requires(locals != null);
    Contract.Requires(etran != null);

    if (alternatives.Count == 0) {
      buildElseCase(builder);
      return;
    }

    // alpha-rename any binding guards
    var guards = alternatives.ConvertAll(alt => alt.IsBindingGuard ? ((ExistsExpr)alt.Guard).AlphaRename("eg$") : alt.Guard);

    // build the negation of the disjunction of all guards (that is, the conjunction of their negations)
    Bpl.Expr noGuard = Bpl.Expr.True;
    var b = new BoogieStmtListBuilder(this, options, builder.Context);
    foreach (var g in guards) {
      b.Add(TrAssumeCmd(g.tok, etran.CanCallAssumption(g)));
      noGuard = BplAnd(noGuard, Bpl.Expr.Not(etran.TrExpr(g)));
    }

    b.Add(TrAssumeCmd(elseToken, noGuard));
    buildElseCase(b);
    Bpl.StmtList els = b.Collect(elseToken);

    Bpl.IfCmd elsIf = null;
    for (int i = alternatives.Count; 0 <= --i;) {
      Contract.Assert(elsIf == null || els == null);  // loop invariant
      CurrentIdGenerator.Push();
      var alternative = alternatives[i];
      b = new BoogieStmtListBuilder(this, options, builder.Context);
      TrStmt_CheckWellformed(guards[i], b, locals, etran, true);
      if (alternative.IsBindingGuard) {
        var exists = (ExistsExpr)alternative.Guard;  // the original (that is, not alpha-renamed) guard
        IntroduceAndAssignExistentialVars(exists, b, builder, locals, etran, isGhost);
      } else {
        b.Add(TrAssumeCmdWithDependencies(etran, alternative.Guard.tok, alternative.Guard, "alternative guard"));
      }
      var prevDefiniteAssignmentTrackerCount = DefiniteAssignmentTrackers.Count;
      TrStmtList(alternative.Body, b, locals, etran, alternative.RangeToken);
      RemoveDefiniteAssignmentTrackers(alternative.Body, prevDefiniteAssignmentTrackerCount);
      Bpl.StmtList thn = b.Collect(alternative.Tok);
      elsIf = new Bpl.IfCmd(alternative.Tok, null, thn, elsIf, els);
      els = null;
      CurrentIdGenerator.Pop();
    }
    Contract.Assert(elsIf != null && els == null); // follows from loop invariant and the fact that there's more than one alternative
    builder.Add(elsIf);
  }


  void RecordNewObjectsIn_New(IToken tok, IteratorDecl iter, Bpl.Expr initHeap, Bpl.IdentifierExpr currentHeap,
    BoogieStmtListBuilder builder, List<Variable> locals, ExpressionTranslator etran) {
    Contract.Requires(tok != null);
    Contract.Requires(iter != null);
    Contract.Requires(initHeap != null);
    Contract.Requires(currentHeap != null);
    Contract.Requires(builder != null);
    Contract.Requires(locals != null);
    Contract.Requires(etran != null);
    // Add all newly allocated objects to the set this._new
    var updatedSet = new Bpl.LocalVariable(iter.tok, new Bpl.TypedIdent(iter.tok, CurrentIdGenerator.FreshId("$iter_newUpdate"), predef.SetType));
    locals.Add(updatedSet);
    var updatedSetIE = new Bpl.IdentifierExpr(iter.tok, updatedSet);
    // call $iter_newUpdate := $IterCollectNewObjects(initHeap, $Heap, this, _new);
    var th = new Bpl.IdentifierExpr(iter.tok, etran.This, predef.RefType);
    var nwField = new Bpl.IdentifierExpr(tok, GetField(iter.Member_New));
    Bpl.Cmd cmd = new CallCmd(iter.tok, "$IterCollectNewObjects",
      new List<Bpl.Expr>() { initHeap, etran.HeapExpr, th, nwField },
      new List<Bpl.IdentifierExpr>() { updatedSetIE });
    builder.Add(cmd);
    // $Heap[this, _new] := $iter_newUpdate;
    cmd = Bpl.Cmd.SimpleAssign(iter.tok, currentHeap, UpdateHeap(iter.tok, currentHeap, th, nwField, updatedSetIE));
    builder.Add(cmd);
    // assume $IsGoodHeap($Heap)
    builder.Add(AssumeGoodHeap(tok, etran));
  }

  private string GetObjFieldDetails(Expression lhs, ExpressionTranslator etran, out Bpl.Expr obj, out Bpl.Expr F) {
    string description;
    if (lhs is MemberSelectExpr) {
      var fse = (MemberSelectExpr)lhs;
      obj = etran.TrExpr(fse.Obj);
      F = GetField(fse);
      description = "an object field";
    } else if (lhs is SeqSelectExpr) {
      var sel = (SeqSelectExpr)lhs;
      obj = etran.TrExpr(sel.Seq);
      var idx = etran.TrExpr(sel.E0);
      idx = ConvertExpression(sel.E0.tok, idx, sel.E0.Type, Type.Int);
      F = FunctionCall(sel.tok, BuiltinFunction.IndexField, null, idx);
      description = "an array element";
    } else {
      MultiSelectExpr mse = (MultiSelectExpr)lhs;
      obj = etran.TrExpr(mse.Array);
      F = etran.GetArrayIndexFieldName(mse.tok, mse.Indices);
      description = "an array element";
    }
    return description;
  }

  private void SelectAllocateObject(IToken tok, Bpl.IdentifierExpr nw, Type type, bool includeHavoc, BoogieStmtListBuilder builder, ExpressionTranslator etran) {
    Contract.Requires(tok != null);
    Contract.Requires(nw != null);
    Contract.Requires(type != null);
    Contract.Requires(builder != null);
    Contract.Requires(etran != null);
    var udt = type as UserDefinedType;
    if (udt != null && udt.ResolvedClass is NonNullTypeDecl) {
      var nnt = (NonNullTypeDecl)udt.ResolvedClass;
      type = nnt.RhsWithArgument(type.TypeArgs);
    }
    if (includeHavoc) {
      // havoc $nw;
      builder.Add(new Bpl.HavocCmd(tok, new List<Bpl.IdentifierExpr> { nw }));
      // assume $nw != null && $Is($nw, type);
      var nwNotNull = Bpl.Expr.Neq(nw, predef.Null);
      // drop the $Is conjunct if the type is "object", because "new object" allocates an object of an arbitrary type
      var rightType = type.IsObjectQ ? Bpl.Expr.True : MkIs(nw, type);
      builder.Add(TrAssumeCmd(tok, BplAnd(nwNotNull, rightType)));
    }
    // assume !$Heap[$nw, alloc];
    var notAlloc = Bpl.Expr.Not(etran.IsAlloced(tok, nw));
    builder.Add(TrAssumeCmd(tok, notAlloc));
  }

  private void CommitAllocatedObject(IToken tok, Bpl.IdentifierExpr nw, Bpl.Cmd extraCmd, BoogieStmtListBuilder builder, ExpressionTranslator etran) {
    Contract.Requires(tok != null);
    Contract.Requires(nw != null);
    Contract.Requires(builder != null);
    Contract.Requires(etran != null);

    // $Heap[$nw, alloc] := true;
    Bpl.Expr alloc = predef.Alloc(tok);
    Bpl.IdentifierExpr heap = etran.HeapCastToIdentifierExpr;
    Bpl.Cmd cmd = Bpl.Cmd.SimpleAssign(tok, heap, UpdateHeap(tok, heap, nw, alloc, Bpl.Expr.True));
    builder.Add(cmd);
    if (extraCmd != null) {
      builder.Add(extraCmd);
    }
    // assume $IsGoodHeap($Heap);
    builder.Add(AssumeGoodHeap(tok, etran));
    // assume $IsHeapAnchor($Heap);
    builder.Add(new Bpl.AssumeCmd(tok, FunctionCall(tok, BuiltinFunction.IsHeapAnchor, null, etran.HeapExpr)));
  }


  private void IntroduceAndAssignExistentialVars(ExistsExpr exists, BoogieStmtListBuilder builder, BoogieStmtListBuilder builderOutsideIfConstruct, List<Variable> locals, ExpressionTranslator etran, bool isGhost) {
    Contract.Requires(exists != null);
    Contract.Requires(exists.Range == null);
    Contract.Requires(builder != null);
    Contract.Requires(builderOutsideIfConstruct != null);
    Contract.Requires(locals != null);
    Contract.Requires(etran != null);
    // declare and havoc the bound variables of 'exists' as local variables
    var iesForHavoc = new List<Bpl.IdentifierExpr>();
    foreach (var bv in exists.BoundVars) {
      Bpl.Type varType = TrType(bv.Type);
      Bpl.Expr wh = GetWhereClause(bv.Tok,
        new Bpl.IdentifierExpr(bv.Tok, bv.AssignUniqueName(currentDeclaration.IdGenerator), varType),
        bv.Type, etran, isAllocContext.Var(isGhost, bv));
      Bpl.Variable local = new Bpl.LocalVariable(bv.Tok, new Bpl.TypedIdent(bv.Tok, bv.AssignUniqueName(currentDeclaration.IdGenerator), varType, wh));
      locals.Add(local);
      iesForHavoc.Add(new Bpl.IdentifierExpr(local.tok, local));
    }
    builderOutsideIfConstruct.Add(new Bpl.HavocCmd(exists.tok, iesForHavoc));
    builder.Add(TrAssumeCmd(exists.tok, etran.TrExpr(exists.Term)));
  }

  public void TrStmtList(List<Statement> stmts, BoogieStmtListBuilder builder, List<Variable> locals, ExpressionTranslator etran,
    RangeToken scopeRange = null, bool processLabels = true) {
    Contract.Requires(stmts != null);
    Contract.Requires(builder != null);
    Contract.Requires(locals != null);
    Contract.Requires(etran != null);

    BoogieStmtListBuilder innerBuilder = builder;
    if (scopeRange != null && !builder.Context.ReturnPosition) {
      /*
       * Boogie's reports at which return location
       * the postconditions of a procedure could not be satisfied.
       *
       * The return locations are the sinks of the Boogie control flow graph.
       * However, adding a command to the end of a Boogie block, can cause previous sinks in the CFG to then lead
       * to that new command, which changes the reported error location.
       *
       * Because of this reason, we need to make sure not to pop at points right before the implementation returns,
       * which is why we check builder.Context.ReturnPosition
       *
       * A more reliable way of Boogie error reporting would be not to rely on sinks in the CFG,
       * By for example reporting all points at which a control flow decision was made.
       */
      builder.Add(new ChangeScope(scopeRange.StartToken, ChangeScope.Modes.Push));
      innerBuilder = builder.WithContext(builder.Context with {
        ScopeDepth = builder.Context.ScopeDepth + 1
      });
    }

    for (var index = 0; index < stmts.Count; index++) {
      var ss = stmts[index];
      var last = index == stmts.Count - 1;
      var indexContext = innerBuilder.Context with {
        ReturnPosition = innerBuilder.Context.ReturnPosition && last
      };
      var indexBuilder = innerBuilder.WithContext(indexContext);
      if (processLabels) {
        for (var l = ss.Labels; l != null; l = l.Next) {
          var heapAt = new Bpl.LocalVariable(ss.Tok,
            new Bpl.TypedIdent(ss.Tok, "$Heap_at_" + l.Data.AssignUniqueId(CurrentIdGenerator), predef.HeapType));
          locals.Add(heapAt);
          builder.Add(Bpl.Cmd.SimpleAssign(ss.Tok, new Bpl.IdentifierExpr(ss.Tok, heapAt), etran.HeapExpr));
        }
      }

      TrStmt(ss, indexBuilder, locals, etran);
      if (processLabels && ss.Labels != null) {
        builder.AddLabelCmd("after_" + ss.Labels.Data.AssignUniqueId(CurrentIdGenerator));
      }
    }

    if (scopeRange != null && !builder.Context.ReturnPosition) {
      builder.Add(new ChangeScope(scopeRange.EndToken, ChangeScope.Modes.Pop));
    }
  }

  void TrStmt_CheckWellformed(Expression expr, BoogieStmtListBuilder builder, List<Variable> locals,
    ExpressionTranslator etran, bool subsumption, bool lValueContext = false, AddResultCommands addResultCommands = null) {
    Contract.Requires(expr != null);
    Contract.Requires(builder != null);
    Contract.Requires(locals != null);
    Contract.Requires(etran != null);
    Contract.Requires(predef != null);

    Bpl.QKeyValue kv;
    if (subsumption) {
      kv = null;  // this is the default behavior of Boogie's assert
    } else {
      List<object> args = new List<object>();
      // {:subsumption 0}
      args.Add(Bpl.Expr.Literal(0));
      kv = new Bpl.QKeyValue(expr.tok, "subsumption", args, null);
    }
    var options = new WFOptions(kv);
    // Only do reads checks if reads clauses on methods are enabled and the reads clause is not *.
    // The latter is important to avoid any extra verification cost for backwards compatibility.
    if (etran.readsFrame != null) {
      options = options.WithReadsChecks(true);
    }
    if (lValueContext) {
      options = options.WithLValueContext(true);
    }
    CheckWellformedWithResult(expr, options, locals, builder, etran, addResultCommands);
    builder.Add(TrAssumeCmd(expr.tok, etran.CanCallAssumption(expr)));
  }

  List<FrameExpression> GetContextReadsFrames() {
    return (codeContext as MethodOrFunction)?.Reads?.Expressions ?? new();
  }

  List<FrameExpression> GetContextModifiesFrames() {
    return (codeContext as IMethodCodeContext)?.Modifies?.Expressions ?? new();
  }
}