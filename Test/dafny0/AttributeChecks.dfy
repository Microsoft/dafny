// RUN: %dafny_0 /print:"%t.print" /dprint:- /compile:0 /env:0 "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

module JustAboutEverything {
  method If(n: int) returns (k: int)
  {
    var i := 0;
    if {:split true} i < n {
      if {:split 1 + 0} true {}
    }
    else if {:split false} i > n {}
    else {:split true} {}
  }

  method IfAlt(n: int) returns (k: int)
  {
    var i := 0;
    if {:split true}
    case {:split 1 + true} i < n => // error: 1 + true is ill-typed
      if {:split 1 + k} true {}
    case {:split false} i > n  => {}
    return 2;
  }

  method While(n: int) returns (k: int)
  {
    var f: int -> int := x => x;
    var i := 0;
    while {:split true} {:split true + k} i < n // error: true + k is ill-typed
      invariant forall u :: f(u) == u + i
    {
    }
    return 2;
  }

  method WhileAlt(n: int) returns (k: int)
  {
    var i := 0;
    var f: int -> int := x => x;
    while {:split}
      invariant forall u :: f(u) == u + i
    {
      case {:split 1 + true} i < n  => {} // error:  1 + true is ill-typed
      case {:split false} i > n => return 2;
    }
  }

  datatype List<T> = Nil | Cons (hd:T, tl: List<T>)
  method Match(xs: List<int>) returns (r: int)
  {
    match {:split 1} xs {
      case {:split false} Cons(y, z) => return y;
      case {:split 1 + false} Nil => return 0; // error:  1 + false is ill-typed
    }
  }

  function method CaseExpr(r: List<int>): List<int>
  {
    match r {
      case Nil => Nil
      case {:ignore 3 + true} Cons(h, Nil) => Nil // error:  3 + true is ill-typed
      case {:ignore false} Cons(h, t) => CaseExpr(t)
    }
  }

  method Calc(x: int, y: int)
  {
    calc {:split 1} {:split 1 + false} { // error:  1 + false is ill-typed
      x + y;
      { assume x == 0; }
      y;
    }
    assert x == 0; // OK: follows from x + y == y;
  }

  method ForAll(i: int, j: int, arr: array2<int>)
  {
    forall i , j {:split 1 + false} {:split i + j}  | i in {-3, 4} && j in {1, 2}  { // error:  1 + false is ill-typed
      arr[i, j] := 0;
    }
  }

  method AssertBy(x: int, y: int)
  {
    assert {:split false + x} {:split} x == 6 by {  // error:  false + x is ill-typed
      assume x == 6;
      assume y == 8;
    }
    assert {:split} y == 8;
  }

  method For(lo: int, hi: int) returns (k: int)
    requires lo <= hi
  {
    var f: int -> int := x => x;
    for {:split i} {:split true + k} i := lo to hi // error: true + k is ill-typed
      invariant forall u :: f(u) == u + i
    {
    }
    return 2;
  }

  datatype {:dt 0} {:dt false + 3} Datatype = // error: false + 3 is ill-typed
    {:dt k} Blue | {:dt 50} Green // error: k is unknown

  datatype {:dt 0} {:dt false + 3} AnotherDatatype = // error: false + 3 is ill-typed
    | {:dt 50} Blue | {:dt k} Green // error: k is unknown

  function FAttr(x: int): int
    requires {:myAttr false + 3} true // error: false + 3 is ill-typed
    ensures {:myAttr false + 3} true // error: false + 3 is ill-typed
    decreases {:myAttr false + 3} x // error: false + 3 is ill-typed
  {
    10
  }

  function GAttr(x: int): int
    requires {:myAttr old(3)} true // error: old is not allowed here
    ensures {:myAttr old(3)} true // error: old is not allowed here
    decreases {:myAttr old(3)} x // error: old is not allowed here
  {
    10
  }

  function HAttr(x: int): (r: int)
    requires {:myAttr x, r} true // error: r is not in scope here
    ensures {:myAttr x, r} true
    decreases {:myAttr x, r} x // error: r is not in scope here
  {
    10
  }

  twostate function F2Attr(x: int): int
    requires {:myAttr false + 3} true // error: false + 3 is ill-typed
    ensures {:myAttr false + 3} true // error: false + 3 is ill-typed
    decreases {:myAttr false + 3} x // error: false + 3 is ill-typed
  {
    10
  }

  twostate function G2Attr(x: int): int
    requires {:myAttr old(3)} true
    ensures {:myAttr old(3)} true
    decreases {:myAttr old(3)} x
  {
    10
  }

  twostate function H2Attr(x: int): (r: int)
    requires {:myAttr x, r} true // error: r is not in scope here
    ensures {:myAttr x, r} true
    decreases {:myAttr x, r} x // error: r is not in scope here
  {
    10
  }

  method MAttr(x: int) returns (y: int)
    requires {:myAttr false + 3} true // error: false + 3 is ill-typed
    modifies {:myAttr false + 3} {} // error: false + 3 is ill-typed
    ensures {:myAttr false + 3} true // error: false + 3 is ill-typed
    decreases {:myAttr false + 3} x // error: false + 3 is ill-typed
  {
  }

  method NAttr(x: int) returns (y: int)
    requires {:myAttr old(3)} true // error: old is not allowed here
    modifies {:myAttr old(3)} {} // error: old is not allowed here
    ensures {:myAttr old(3)} true
    decreases {:myAttr old(3)} x // error: old is not allowed here
  {
  }

  method OAttr(x: int) returns (y: int)
    requires {:myAttr x, y} true // error: y is not in scope here
    modifies {:myAttr x, y} {} // error: y is not in scope here
    ensures {:myAttr x, y} true
    decreases {:myAttr x, y} x // error: y is not in scope here
  {
  }

  twostate lemma M2Attr(x: int) returns (y: int)
    requires {:myAttr false + 3} true // error: false + 3 is ill-typed
    modifies {:myAttr false + 3} {} // error (x2): false + 3 is ill-typed, and twostate lemma cannot have modifies clause
    ensures {:myAttr false + 3} true // error: false + 3 is ill-typed
    decreases {:myAttr false + 3} x // error: false + 3 is ill-typed
  {
  }

  twostate lemma N2Attr(x: int) returns (y: int)
    requires {:myAttr old(3)} true
    modifies {:myAttr old(3)} {} // error (x2): old is not allowed here, and twostate lemma cannot have modifies clause
    ensures {:myAttr old(3)} true
    decreases {:myAttr old(3)} x // error: old is not allowed here
  {
  }

  twostate lemma O2Attr(x: int) returns (y: int)
    requires {:myAttr x, y} true // error: y is not in scope here
    modifies {:myAttr x, y} {} // error (x2): y is not in scope here, and twostate lemma cannot have modifies clause
    ensures {:myAttr x, y} true
    decreases {:myAttr x, y} x // error: y is not in scope here
  {
  }

  iterator Iter(x: int) yields (y: int)
    requires {:myAttr false + 3} true // error: false + 3 is ill-typed
    yield requires {:myAttr false + 3} true // error: false + 3 is ill-typed
    modifies {:myAttr false + 3} {} // error: false + 3 is ill-typed
    yield ensures {:myAttr false + 3} true // error: false + 3 is ill-typed
    ensures {:myAttr false + 3} true // error: false + 3 is ill-typed
    decreases {:myAttr false + 3} x // error: false + 3 is ill-typed
  {
  }

  iterator Jter(x: int) yields (y: int)
    requires {:myAttr old(3)} true // error: old is not allowed here
    yield requires {:myAttr old(3)} true // error: old is not allowed here
    modifies {:myAttr old(3)} {} // error: old is not allowed here
    yield ensures {:myAttr old(3)} true
    ensures {:myAttr old(3)} true
    decreases {:myAttr old(3)} x // error: old is not allowed here
  {
  }

  iterator Kter(x: int) yields (y: int)
    requires {:myAttr x, y, ys} true // error (x2): y and ys are not in scope here
    yield requires {:myAttr x, y, ys} true // these are allowed (but it would be weird for anyone to use y and ys here)
    modifies {:myAttr x, y, ys} {} // error (x2): y and ys are not in scope here
    yield ensures {:myAttr x, y, ys} true
    ensures {:myAttr x, y, ys} true
    decreases {:myAttr x, y, ys} x // error (x2): y and ys are not in scope here
  {
  }

  method ModifyStatement(s: set<object>) {
    modify {:myAttr false + 3} s; // error: false + 3 is ill-typed
  }

  method LocalVariablesAndAssignments(opt: Option<int>) returns (r: Option<int>) {
    // variables declared with attributes
    var
      {:boolAttr false + 3} a: int, // error: false + 3 is ill-typed
      {:boolAttr false + 3} b: int; // error: false + 3 is ill-typed
      
    // simple assignments, where each RHS has an attribute
    var x, y :=
      10 {:boolAttr false + 3}, // error: false + 3 is ill-typed
      20 {:boolAttr false + 3}; // error: false + 3 is ill-typed
    x, y :=
      10 {:boolAttr false + 3}, // error: false + 3 is ill-typed
      20 {:boolAttr false + 3}; // error: false + 3 is ill-typed

    // method call, where either the variable or the RHS has an attribute
    var {:boolAttr false + 3} u0 := If(13); // error: false + 3 is ill-typed
    var u1 := If(13) {:boolAttr false + 3}; // error: false + 3 is ill-typed
    u1 := If(13) {:boolAttr false + 3}; // error: false + 3 is ill-typed
    
    // arbitrary assignment, where either the variable or the RHS has an attribute
    var {:boolAttr false + 3} k0: int := *; // error: false + 3 is ill-typed
    var k1: int := * {:boolAttr false + 3}; // error: false + 3 is ill-typed
    k1 := * {:boolAttr false + 3}; // error: false + 3 is ill-typed
    
    // allocation, where either the variable or the RHS has an attribute
    var {:boolAttr false + 3} c0 := new CClass; // error: false + 3 is ill-typed
    var c1 := new CClass {:boolAttr false + 3}; // error: false + 3 is ill-typed
    c1 := new CClass {:boolAttr false + 3}; // error: false + 3 is ill-typed
    var {:boolAttr false + 3} d0 := new DClass(); // error: false + 3 is ill-typed
    var d1 := new DClass() {:boolAttr false + 3}; // error: false + 3 is ill-typed
    d1 := new DClass() {:boolAttr false + 3}; // error: false + 3 is ill-typed

    // assign-such-that, where variable has an attribute
    var s := {101};
    var {:boolAttr false + 3} w0 :| w0 in s; // error: false + 3 is ill-typed
    var
      {:boolAttr false + 3} w1, // error: false + 3 is ill-typed
      {:boolAttr false + 3} w2 // error: false + 3 is ill-typed
      :| w1 in s && w2 in s;

    // :- with expression RHS, where variable declarations have attributes
    var {:boolAttr false + 3} f0 :- opt;
    var
      {:boolAttr false + 3} f1,
      {:boolAttr false + 3} f2
      :- opt, true;
    // :- with call RHS, where variable declarations have attributes
    var {:boolAttr false + 3} f3 :- GiveOption();
    var
      {:boolAttr false + 3} f4,
      {:boolAttr false + 3} f5
      :- GiveOptions();

    // :- with expression RHS, where RHSs have attributes
//    var g0 :- opt {:boolAttr false + 3}; // NOT ALLOWED BY PARSER
//    var g1, g2 :-
//      opt {:boolAttr false + 3}, // NOT ALLOWED BY PARSER
//      true {:boolAttr false + 3};
    var g3, g4, g5 :-
      opt, 
      true {:boolAttr false + 3}, // FAILS TO PRINT THIS RHS!
      true {:boolAttr false + 3}; // FAILS TO PRINT THIS RHS!
    // :- with call RHS, where variable declarations have attributes
//    var g6 :- GiveOption() {:boolAttr false + 3}; // NOT ALLOWED BY PARSER
//    var g7, g8 :- GiveOptions() {:boolAttr false + 3}; // NOT ALLOWED BY PARSER
  }

  class CClass {
  }
  class DClass {
    constructor () { }
  }
  
  method GiveOption() returns (r: Option<int>)
  method GiveOptions() returns (r: Option<int>, s: int)

  datatype Option<+T> = None | Some(value: T) {
    predicate method IsFailure() {
      None?
    }
    function method PropagateFailure<U>(): Option<U>
      requires None?
    {
      None
    }
    function method Extract(): T
      requires Some?
    {
      value
    }
  }
}

module
  {:myAttr false + 3} // error: false + 3 is ill-typed
  {:myAttr old(3)} // error: old is not allowed here
  {:myAttr k} // error: k is not in scope here
  Modu
{
}
