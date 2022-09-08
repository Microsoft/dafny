//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
// Copyright by the contributors to the Dafny Project
// SPDX-License-Identifier: MIT
//
//-----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

namespace Microsoft.Dafny {

  public class UselessOldLinter : IRewriter {
    internal override void PostResolve(Program program) {
      base.PostResolve(program);
      foreach (var moduleDefinition in program.Modules()) {
        foreach (var topLevelDecl in moduleDefinition.TopLevelDecls.OfType<TopLevelDeclWithMembers>()) {
          foreach (var callable in topLevelDecl.Members.OfType<ICallable>()) {
            var visitor = new UselessOldLinterVisitor(this.Reporter);
            visitor.Visit(callable, Unit.Default);
          }
        }
      }
    }

    public UselessOldLinter(ErrorReporter reporter) : base(reporter) {
    }
  }

  class UselessOldLinterVisitor : TopDownVisitor<Unit> {
    private readonly ErrorReporter reporter;

    public UselessOldLinterVisitor(ErrorReporter reporter) {
      this.reporter = reporter;
    }

    protected override bool VisitOneExpr(Expression expr, ref Unit st) {
      if (expr is OldExpr oldExpr && !AutoGeneratedToken.Is(oldExpr.tok)) {
        var fvs = new HashSet<IVariable>();
        var usesHeap = false;
        FreeVariablesUtil.ComputeFreeVariables(oldExpr.E, fvs, ref usesHeap, true);
        if (!usesHeap) {
          this.reporter.Warning(MessageSource.Rewriter, oldExpr.tok,
            $"Argument to 'old' does not dereference the mutable heap, so this use of 'old' has no effect");
        }
      }
      return base.VisitOneExpr(expr, ref st);
    }
  }
}
