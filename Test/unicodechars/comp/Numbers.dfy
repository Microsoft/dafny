// RUN: %dafny /compile:0 /verifyAllModules /unicodeChar:1 "%s" > "%t"
// RUN: %dafny /noVerify /compile:4 /unicodeChar:1 /compileTarget:cs "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /unicodeChar:1 /compileTarget:js "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /unicodeChar:1 /compileTarget:go "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /unicodeChar:1 /compileTarget:java "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /unicodeChar:1 /compileTarget:py "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"
include "../../comp/Numbers.dfy"