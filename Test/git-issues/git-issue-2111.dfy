// RUN: %exits-with 2 %baredafny verify %args "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

datatype DT = C(s: string) {
  predicate F() {
    C.XYZ()
  }
}
