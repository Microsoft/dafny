using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Microsoft.Dafny;

class CollectFriendlyCallsInSpec_Visitor : FindFriendlyCalls_Visitor {
  readonly ISet<Expression> friendlyCalls;
  readonly ExtremeLemma Context;
  public CollectFriendlyCallsInSpec_Visitor(ErrorReporter reporter, ISet<Expression> friendlyCalls, bool co, ExtremeLemma context)
    : base(reporter, co, context.KNat) {
    Contract.Requires(reporter != null);
    Contract.Requires(friendlyCalls != null);
    Contract.Requires(context != null);
    this.friendlyCalls = friendlyCalls;
    this.Context = context;
  }
  protected override bool VisitOneExpr(Expression expr, ref Resolver.CallingPosition cp) {
    if (cp == Resolver.CallingPosition.Neither) {
      // no friendly calls in "expr"
      return false;  // don't recurse into subexpressions
    }
    if (expr is FunctionCallExpr) {
      if (cp == Resolver.CallingPosition.Positive) {
        var fexp = (FunctionCallExpr)expr;
        if (IsCoContext ? fexp.Function is GreatestPredicate : fexp.Function is LeastPredicate) {
          if (Context.KNat != ((ExtremePredicate)fexp.Function).KNat) {
            Resolver.KNatMismatchError(reporter, expr.tok, Context.Name, Context.TypeOfK, ((ExtremePredicate)fexp.Function).TypeOfK);
          } else {
            friendlyCalls.Add(fexp);
          }
        }
      }
      return false;  // don't explore subexpressions any further
    } else if (expr is BinaryExpr && IsCoContext) {
      var bin = (BinaryExpr)expr;
      if (cp == Resolver.CallingPosition.Positive && bin.ResolvedOp == BinaryExpr.ResolvedOpcode.EqCommon && bin.E0.Type.IsCoDatatype) {
        friendlyCalls.Add(bin);
        return false;  // don't explore subexpressions any further
      } else if (cp == Resolver.CallingPosition.Negative && bin.ResolvedOp == BinaryExpr.ResolvedOpcode.NeqCommon && bin.E0.Type.IsCoDatatype) {
        friendlyCalls.Add(bin);
        return false;  // don't explore subexpressions any further
      }
    }
    return base.VisitOneExpr(expr, ref cp);
  }
}