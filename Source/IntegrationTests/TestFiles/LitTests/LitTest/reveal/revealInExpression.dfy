// echo ''
// NORUN: ! %verify --type-system-refresh --allow-axioms --bprint:%t.bpl --isolate-assertions --boogie "/printPruned:%S/pruned" %s > %t
// NORUN: %diff "%s.expect" "%t"

function P(x: int): bool {
  true
}

function Q(): int 
  requires P() {
  3
}

method RevealExpressionScope()
{
  hide *;
  var a := (reveal P; assert P(0); true);
  assert P(1); // error
  var b := (var x := (reveal P; assert P(2); true); 
            assert P(3); x); // error
  var c := (var x: bool :| (reveal P; assert P(4); x); 
            assert P(5); x); // error
  var d := (forall x: bool :: reveal P; assert P(6); x) || 
           (assert P(7); true); // error
  var e := ((x: bool) => reveal P; assert P(7); x)(true) || 
           (assert P(8); true); // error 
}

method MatchExpressionScope(x: int) {
  hide *;
  var x := match x {
    case 0 =>
      reveal P; assert P(0); 1
    case _ =>
      assert P(0); 2 // error
  }
  assert P(1); // error
}