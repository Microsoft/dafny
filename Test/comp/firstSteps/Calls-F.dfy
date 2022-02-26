// This is a partial test from comp/Calls.dfy to be used as a milestone for compiler builds. 
// RUN: %dafny /compile:0 "%s" > "%t"
// RUN: %dafny /noVerify /compile:4 /compileTarget:py "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"

function method F(x: int, y: bool): int {
  x + if y then 2 else 3
}

method Main() {
  var w := F(2, false);
  print w, "\n";
}
