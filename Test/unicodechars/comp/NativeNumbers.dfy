// Skip JavaScript because JavaScript doesn't have the same native types

// RUN: %dafny /compile:0 /verifyAllModules /unicodeChar:1 "%s" > "%t"
// RUN: %dafny /noVerify /compile:4 /unicodeChar:1 /spillTargetCode:2 /compileTarget:cs "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /unicodeChar:1 /spillTargetCode:2 /compileTarget:go "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /unicodeChar:1 /spillTargetCode:2 /compileTarget:java "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /unicodeChar:1 /spillTargetCode:2 /compileTarget:py "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"
include "../../comp/NativeNumbers.dfy"