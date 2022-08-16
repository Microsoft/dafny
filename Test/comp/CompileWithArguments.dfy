// RUN: %dafny /compile:0 "%s" > "%t"
// RUN: %dafny /noVerify /compile:4 /compileTarget:cs "/mainArgs:csharp 1" "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /compileTarget:js "/mainArgs:javascript 2" "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /compileTarget:go "/mainArgs:\"go go\" 1" "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /compileTarget:java "/mainArgs:java heya" "%s" >> "%t"
// RUN: %dafny /noVerify /compile:4 /compileTarget:py "/mainArgs:python 1" "%s" >> "%t"
// RUN: %dafny /noVerify /compile:2 /compileTarget:cs "%s" /out:CompileWithArgument.dll
// RUN: %dotnet CompileWithArgument.dll "ellel" 2 >> "%t"
// RUN: %dotnet CompileWithArgument.dll "on the go" 1 >> "%t"
// RUN: %dotnet CompileWithArgument.dll "dll" "Aloha from" >> "%t"
// RUN: %diff "%s.expect" "%t"

method Main(args: array<string>) {
  if args.Length != 2 {
    print "Expected 2 arguments, got ", args.Length;
  } else {
    if args[1] == "1" {
      print "Hello ",args[0], "\n";
    } else if args[1] == "2" {
      print "Howdy ", args[0], "\n";
    } else {
      print args[1], " ", args[0], "\n";
    }
  }
}