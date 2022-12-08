// RUN: %exits-with 4 %dafny /compile:3 /compileTarget:java "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

datatype Wrapper<T> = Wrapper(s: seq<T>)
{
  static function method empty(): Wrapper<T>
  { Wrapper([]) }
}

method Main() {
  var xs: Wrapper<nat> := Wrapper<nat>.empty();
  print xs, "\n";
}
