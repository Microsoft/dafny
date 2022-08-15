using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Boogie;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.Dafny; 

public class ExpectContracts : IRewriter {
  private readonly ClonerButDropMethodBodies cloner = new();
  private readonly Dictionary<MemberDecl, MemberDecl> wrappedDeclarations = new();
  private CallRedirector callRedirector;

  public ExpectContracts(ErrorReporter reporter) : base(reporter) {
  }

  /// <summary>
  /// Create an expect statement that checks the given contract clause
  /// expression and fails with a message that points to the original
  /// location of the contract clause if it is not true at runtime.
  /// </summary>
  /// <param name="expr">The contract clause expression to evaluate.</param>
  /// <param name="exprType">Either "requires" or "ensures", to use in the
  /// failure message.</param>
  /// <returns>The newly-created expect statement.</returns>
  private static Statement CreateContractExpectStatement(AttributedExpression expr, string exprType) {
    var tok = expr.E.tok;
    var msg = $"Runtime failure of {exprType} clause from {tok.filename}:{tok.line}:{tok.col}";
    var msgExpr = Expression.CreateStringLiteral(tok, msg);
    return new ExpectStmt(tok, expr.E.EndToken, expr.E, msgExpr, null);
  }

  /// <summary>
  /// Creates a block statement that includes an expect statement for every
  /// requires clause, followed by the provided call statement, followed by
  /// an expect statement for every ensures clause.
  /// </summary>
  /// <param name="requires">The list of requires clause expressions.</param>
  /// <param name="ensures">The list of ensures clause expressions.</param>
  /// <param name="callStmt">The call statement to include.</param>
  /// <returns>The newly-created block statement.</returns>
  private static BlockStmt MakeContractCheckingBody(List<AttributedExpression> requires, List<AttributedExpression> ensures, Statement callStmt) {
    var expectRequiresStmts = requires.Select(req =>
      CreateContractExpectStatement(req, "requires"));
    var expectEnsuresStmts = ensures.Select(ens =>
      CreateContractExpectStatement(ens, "ensures"));
    var callStmtList = new List<Statement>() { callStmt };
    var bodyStatements = expectRequiresStmts.Concat(callStmtList).Concat(expectEnsuresStmts);
    return new BlockStmt(callStmt.Tok, callStmt.EndTok, bodyStatements.ToList());
  }

  private bool ShouldGenerateWrapper(MemberDecl decl) {
    // TODO: make this more discriminating
    // TODO: could work for ghost statements eventually
    return !decl.IsGhost && decl is not Constructor;
  }

  private void AddWrapper(TopLevelDeclWithMembers parent, MemberDecl decl) {
    var tok = decl.tok; // TODO: do better

    var receiver = decl.IsStatic
      ? (Expression)new StaticReceiverExpr(tok, parent, false)
      : (Expression)new ThisExpr(parent);
    var newName = decl.Name + "_checked";
    MemberDecl newDecl = null;

    if (decl is Method origMethod) {
      var newMethod = cloner.CloneMethod(origMethod);
      newMethod.Name = newName;

      var args = newMethod.Ins.Select(Expression.CreateIdentExpr).ToList();
      var outs = newMethod.Outs.Select(Expression.CreateIdentExpr).ToList();
      var selector = new MemberSelectExpr(tok, receiver, origMethod.Name) {
        Member = origMethod,
        TypeApplication_JustMember = new List<Type>(), // TODO: fill in properly
        TypeApplication_AtEnclosingClass = new List<Type>() // TODO: fill in properly
      };
      var callStmt = new CallStmt(tok, tok, outs, selector, args);

      var body = MakeContractCheckingBody(origMethod.Req, origMethod.Ens, callStmt);
      newMethod.Body = body;
      newDecl = newMethod;
    } else if (decl is Function origFunc) {
      var newFunc = cloner.CloneFunction(origFunc);
      newFunc.Name = newName;

      var args = origFunc.Formals.Select(Expression.CreateIdentExpr).ToList();
      var callExpr = new FunctionCallExpr(tok, origFunc.Name, receiver, tok, tok, args);
      newFunc.Body = callExpr;

      var localName = origFunc.Result?.Name ?? "__result";
      var local = new LocalVariable(tok, tok, localName, origFunc.ResultType, false);
      var localExpr = new IdentifierExpr(tok, localName);
      var callRhs = new ExprRhs(callExpr);

      var lhss = new List<Expression> { localExpr };
      var locs = new List<LocalVariable> { local };
      var rhss = new List<AssignmentRhs> { callRhs };

      var callStmt = origFunc.Result?.Name is null
        ? (Statement)new VarDeclStmt(tok, tok, locs, new UpdateStmt(tok, tok, lhss, rhss))
        : (Statement)new AssignStmt(tok, tok, localExpr, callRhs);

      var body = MakeContractCheckingBody(origFunc.Req, origFunc.Ens, callStmt);
      body.AppendStmt(new ReturnStmt(tok, tok, new List<AssignmentRhs> { new ExprRhs(localExpr) }));
      newFunc.ByMethodBody = body;
      newDecl = newFunc;
    }

    if (newDecl is not null) {
      parent.Members.Add(newDecl);
      wrappedDeclarations.Add(decl, newDecl);
    }
  }

  internal override void PreResolve(ModuleDefinition moduleDefinition) {
    // Keep a list of members to wrap so that we don't modify the collection we're iterating over.
    List<(TopLevelDeclWithMembers, MemberDecl)> membersToWrap = new();

    foreach (var topLevelDecl in moduleDefinition.TopLevelDecls.OfType<TopLevelDeclWithMembers>()) {
      foreach (var decl in topLevelDecl.Members) {
        if (ShouldGenerateWrapper(decl)) {
          membersToWrap.Add((topLevelDecl, decl));
        }
      }
    }

    foreach (var (topLevelDecl, decl) in membersToWrap) {
      AddWrapper(topLevelDecl, decl);
    }

    callRedirector = new CallRedirector(wrappedDeclarations);
  }

  class CallRedirector : TopDownVisitor<MemberDecl> {
    private readonly Dictionary<MemberDecl, MemberDecl> wrappedDeclarations;

    public CallRedirector(Dictionary<MemberDecl, MemberDecl> wrappedDeclarations) {
      this.wrappedDeclarations = wrappedDeclarations;
    }

    private bool HasTestAttribute(MemberDecl decl) {
      return Attributes.Contains(decl.Attributes, "test");
    }

    private bool HasExternAttribute(MemberDecl decl) {
      return Attributes.Contains(decl.Attributes, "extern");
    }

    private bool ShouldCallWrapper(MemberDecl caller, MemberDecl callee) {
      // TODO: make this configurable
      return (HasTestAttribute(caller) || HasExternAttribute(callee)) &&
             wrappedDeclarations.ContainsKey(caller);
    }

    protected override bool VisitOneExpr(Expression expr, ref MemberDecl decl) {
      if (expr is FunctionCallExpr fce) {
        if (ShouldCallWrapper(decl, fce.Function)) {
          var target = fce.Function;
          var newTarget = wrappedDeclarations[target];
          Console.WriteLine($"Call (expression) to {target.Name} redirecting to {newTarget.Name}");
          // TODO: apparently the following isn't enough
          fce.Function = (Function)newTarget;
          fce.Name = newTarget.Name;
        }
      }

      return true;
    }

    protected override bool VisitOneStmt(Statement stmt, ref MemberDecl decl) {
      if (stmt is CallStmt cs) {
        if (ShouldCallWrapper(decl, cs.Method)) {
          var target = cs.MethodSelect.Member;
          var newTarget = wrappedDeclarations[target];
          Console.WriteLine($"Call (statement) to {target.Name} redirecting to {newTarget.Name}");
          // TODO: apparently the following isn't enough
          cs.MethodSelect.Member = newTarget;
          cs.MethodSelect.MemberName = newTarget.Name;
        }
      }

      return true;
    }
  }

  internal override void PostResolve(ModuleDefinition moduleDefinition) {
    foreach (var topLevelDecl in moduleDefinition.TopLevelDecls.OfType<TopLevelDeclWithMembers>()) {
      foreach (var decl in topLevelDecl.Members) {
        if (decl is ICallable callable) {
          Console.WriteLine($"Visiting {decl.Name}");
          callRedirector.Visit(callable, decl);
        }
      }
    }
  }
}
