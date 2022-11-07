// RUN: %dafny /compile:0 "%s" > "%t"
// RUN: %dafny /noVerify /compile:4 /spillTargetCode:2 /compileTarget:cs "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /spillTargetCode:2 /compileTarget:js "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /spillTargetCode:2 /compileTarget:go "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /spillTargetCode:2 /compileTarget:java "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /spillTargetCode:2 /compileTarget:py "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"

datatype SingletonRecord = SingletonRecord(u: int)
datatype GhostOrNot = ghost Ghost(a: int, b: int) | Compiled(x: int)
//datatype GenericGhostOrNot<X> = ghost Ghost(a: int, b: int) | GenericCompiled(x: X)

method Main() {
  TestTargetTypesAndConstructors();
  TestSelect();
  TestUpdate();
  TestDiscriminators();
  TestMatchStmt();
  TestMatchExpr();
}

method TestTargetTypesAndConstructors() {
  var r := SingletonRecord(62); // type of r should turn into int
  var g := Compiled(63); // type of g should turn into int
  var rst := (2, 5);
  var xyz := (2, ghost 3, 5); // type of xyz should turn into Tuple2
  var abc := (ghost 2, 3, ghost 5); // type of abc should turn into int

  print r, " ", g, " ", rst, " ", xyz, " ", abc, "\n"; // 62 63 (2, 5) (2, 5) 3
}

method TestSelect() {
  var r := SingletonRecord(62);
  var g := Compiled(63);
  var rst := (2, 5);
  var xyz := (2, ghost 3, 5);
  var abc := (ghost 2, 3, ghost 5);

  print r.u, " "; // 62
  print g.x, " "; // 63
  print rst.1, " "; // 5
  print xyz.2, " "; // 5
  print abc.1, "\n"; // 3
}

method TestUpdate() {
  var r := SingletonRecord(62);
  var g := Compiled(63);
  var rst := (2, 5);
  var xyz := (2, ghost 3, 5);
  var abc := (ghost 2, 3, ghost 5);

  rst := rst.(0 := 888);
  xyz := xyz.(0 := 888);
  abc := abc.(0 := 888); // no-op

  print rst.1, " "; // 5
  print xyz.2, " "; // 5
  print abc.1, "\n"; // 3

  r := r.(u := 1062); // rhs optimized to just 1062
  g := g.(x := 1063); // rhs optimized to just 1063
  rst := rst.(1 := 1005);
  xyz := xyz.(2 := 1005);
  abc := abc.(1 := 1003); // rhs optimized to just 1003

  print r.u, " "; // 1062
  print g.x, " "; // 1063
  print rst.1, " "; // 1005
  print xyz.2, " "; // 1005
  print abc.1, "\n"; // 1003
}

method TestDiscriminators() {
  var r := SingletonRecord(62);
  var g := Compiled(63);
  print r.SingletonRecord?, " ", g.Compiled?, "\n"; // true true
}

method TestMatchStmt() {
  var a := SingletonRecord(20);
  var b := Compiled(21);
  var c0 := (ghost 100, 101, a, ghost 103, 104);
  var c1 := (c0, ghost 200);
  var c := (ghost 300, c1);

  match a {
    case SingletonRecord(u0) => print u0, " "; // 20
  }
  match a {
    case SingletonRecord(19) =>
    case SingletonRecord(u1) => print u1, " "; // 20
  }
  match a {
    case SingletonRecord(19) =>
    case SingletonRecord(20) => print "*20 "; // *20
    case SingletonRecord(_) =>
  }

  match b {
    case Compiled(v) => print v, " "; // 21
  }

  match c {
    case (g300, ((g100, h101, SingletonRecord(w), g103, h104), g200)) => print w, "\n"; // 20
  }
}

method TestMatchExpr() {
  var a := SingletonRecord(20);
  var b := Compiled(21);
  var c0 := (ghost 100, 101, a, ghost 103, 104);
  var c1 := (c0, ghost 200);
  var c := (ghost 300, c1);

  print match a {
    case SingletonRecord(u0) => u0
  }, " "; // 20
  print match a {
    case SingletonRecord(19) => -1
    case SingletonRecord(u1) => u1
  }, " "; // 20
  print "*", match a {
    case SingletonRecord(19) => -1
    case SingletonRecord(20) => 20
    case SingletonRecord(_) => -1
  }, " "; // *20

  print match b {
    case Compiled(v) => v
  }, " "; // 21

  print match c {
    case (g300, ((g100, h101, SingletonRecord(w), g103, h104), g200)) => w
  }, "\n"; // 20
}
