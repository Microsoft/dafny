// RUN: %verify %s &> "%t"
// RUN: %diff "%s.expect" "%t"

method ByClause(b: bool) {
  var r: int :| false by {
    assume {:axiom} false;
  }
}