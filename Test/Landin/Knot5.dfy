// RUN: %exits-with 2 %dafny /compile:0 "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

datatype Pack<T> = Pack(ghost c: T)

class Y {
  const f: Pack<Y -> nat>
  constructor(f: Pack<Y -> nat>)
    ensures this.f == f
  {
    this.f := f;
  }
}

method Main()
  ensures false
{
  var knot := new Y(Pack((x: Y) => 1 + x.f.c(x)));
  var a := knot.f.c(knot);
}