// RUN: %testDafnyForEachCompiler --refresh-exit-code=0 "%s" 

module State {

  datatype State = Whatever

}

module Foo {

   import opened State

   const bar: State

   method Main() {
     print "Hello!\n";
  }
}