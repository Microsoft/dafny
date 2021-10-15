// RUN: %dafny /compileVerbose:1 /compileTarget:cs "%s" > "%t"
// RUN: dotnet CompileAndThenRun.dll >> "%t"

// RUN: %dafny /compileVerbose:1 /compileTarget:js "%s" >> "%t"
// RUN: node %S/CompileAndThenRun.js >> "%t"

// RUN: %dafny /compileVerbose:1 /compileTarget:go "%s" >> "%t"
// RUN: %S/CompileAndThenRun >> "%t"

// RUN: %dafny /compileVerbose:1 /compileTarget:java "%s" >> "%t"
// RUN: java -cp %binaries/DafnyRuntime.jar:%S/CompileAndThenRun-java CompileAndThenRun >> "%t"

// RUN: %dafny /compileVerbose:1 /compileTarget:cpp "%s" >> "%t"
// RUN: CompileAndThenRun.exe >> "%t"

// RUN: %diff "%s.expect" "%t"

method Main() {
  print "hello, Dafny\n";
}
