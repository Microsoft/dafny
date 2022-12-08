// RUN: %dafny /compile:0 "%s" > "%t"
// RUN: %exits-with 4 %dafny /noVerify /compile:4 /compileTarget:cs "%s" >> "%t"
// RUN: %exits-with 4 %dafny /noVerify /compile:4 /compileTarget:js "%s" >> "%t"
// RUN: %exits-with 4 %dafny /noVerify /compile:4 /compileTarget:go "%s" >> "%t"
// RUN: %exits-with 4 %dafny /noVerify /compile:4 /compileTarget:java "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"

module A {
  module AA {
    method {:main} Main() {}
  }
}

module B {
  class C {
    static method {:main} Main() {}
  }
}

method Main() {}
