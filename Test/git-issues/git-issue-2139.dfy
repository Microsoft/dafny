// RUN: %dafny_0 /compile:0 "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

datatype T = T(T, T) {
  predicate P() {
    match this
      case T(0, 1) => false
  }
}
method a() 
{
    var tok := (0,0);
    match tok {
      case "B" => 
      case _ => 
    }
}