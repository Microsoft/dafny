// RUN: %baredafny build -t:java --output Q.jar "%s" > "%t"
// RUN: java -jar Q.jar >>  "%t"
// RUN: %diff "%s.expect" "%t"

method Main() {
  print "Hello, World!\n";
}
