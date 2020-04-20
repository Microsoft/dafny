// RUN: %dafny /compile:0 /rprint:- /dafnyVerify:0 "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

module M0 {
  trait Tr<X> {
    function F(x: X): int { 15 }
  }

  class Cl<Y> extends Tr<Y> {
    lemma M() {
      var v := this;  // Cl<Y>
      var u: Tr := this;  // Tr<Y>

      var f := v.F;  // Y -> int
      var g := this.F;  // Y -> int
    }
  }
}


module M1 {
  trait Tr<X(0)> {
    var w: X
  }

  class Cl<Y> extends Tr<(Y,Y)> {
  }

  lemma M(c: Cl<int>) {
    var x := c.w;  // (int, int)
  }
}

module M2 {
  trait Tr<X, W> {
    function method F(x: X, w: W): bv10 { 15 }
  }

  class Cl<Y> extends Tr<(Y,Y), real> {
  }

  lemma M(c: Cl<int>) {
    var aa;  // (int, int)
    var bb;  // real
    var u := c.F(aa, bb);  // bv10
  }
}

module M3 {
  trait Tr<X, W> {
    function method F(x: X, w: W): bv10 { 15 }
  }

  class Cl<Y> extends Tr<(Y,Y), real> {
    function method H(y: Y): bv10 {
      F((y, y), 5.0)
    }
  }
}
