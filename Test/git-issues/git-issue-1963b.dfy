// RUN: %dafny /compile:0 "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

/// This file checks that refinement is forbidden for datatypes

module A {
  datatype D = D
  lemma P(d: D) ensures d == D {}
}

module B refines A {
  datatype D = ... D | D'
}

method M() ensures false {
  assert B.D == B.D' by { B.P(B.D'); }
}
