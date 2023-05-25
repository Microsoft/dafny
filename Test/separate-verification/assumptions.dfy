// Passing --no-verify because TestAuditor.dfy intentionally has a failing assertion,
// but that would otherwise mean we don't get far enough to exercise the LibraryBackend.
// RUN: ! %baredafny build -t:lib --verify-scope=RootSourcesAndIncludes %args %s > %t
// RUN: %diff "%s.expect" %t
include "../auditor/IgnoredAssumptions.dfy"
// Lemma with no body
lemma MinusBv8NoBody(i: nat, j: nat)
  requires j <= i < 256
  ensures (i - j) as bv8 == i as bv8 - j as bv8

// Lemma with an {:axiom} attribute
lemma {:axiom} LeftShiftBV128(v: bv128, idx: nat)
  requires idx <= 128
  ensures v << idx == v << idx as bv8

// Function or method with an {:axiom} attribute or {:verify false} attribute

// Lemma with an assume statement in its body
lemma MinusBv8Assume(i: nat, j: nat)
  requires j <= i < 256
  ensures (i - j) as bv8 == i as bv8 - j as bv8
{
  assume (i - j) as bv8 == i as bv8 - j as bv8;
}

// Method declared {:extern} with an ensures clause and no body
newtype int32 = x | -0x8000_0000 <= x < 0x8000_0000
newtype uint8 = x | 0 <= x < 0x100

method {:extern} GenerateBytes(i: int32) returns (res: seq<uint8>)
    requires i >= 0
    ensures |res| == i as int

// Method declared {:extern} with an ensures clause and a body
method {:extern} GenerateBytesWithModel(i: int32) returns (res: seq<uint8>)
    requires i >= 0
    ensures |res| == i as int
{
  return seq(i, _ => 0 as uint8);
}

// Compiled method with an assume statement with an {:axiom} attribute in its body
method GenerateBytesWrapper(i: int32) returns (res: seq<uint8>)
{
  assume {:axiom} i >= 0;
  res := GenerateBytes(i);
}

// Function or method with no body
ghost function WhoKnows(x: int): int

// Method declared {:extern} with no body and no ensures clauses
method {:extern} GenerateBytesNoGuarantee(i: int32) returns (res: seq<uint8>)
    requires i >= 0

// Extern function with postcondition. Should result in two findings.
function {:extern} ExternFunction(i: int32): (res: int32)
  ensures res != i

// Successful proof of a function, method, or lemma (shown only in reports from later versions of the tool)
method GoodMethod(i: nat) returns (res: nat)
  requires i < 64
  ensures res == 64 + i
{
  return 64 + i;
}

method DoesNotTerminate()
  decreases *
{
  DoesNotTerminate();
}

trait {:termination false} T {
}

lemma ForallWithoutBody()
  ensures forall y : int :: y != y
{
    forall y : int
      ensures y != y
}

method LoopWithoutBody(n: int)
{
    var i := 0;
    while i < n
      decreases n - i
    assert true;
}

abstract module M {
  method AbstractMethod(x: int) returns (y: int)
    ensures y > x
}

opaque function f(): int {
  0
}

// A method that's safe for concurrent use because it doesn't touch the
// heap.
method {:concurrent} ConcurrentMethod(x: int) returns (r: int) {
  return x;
}

method {:axiom} AxiomWithStuffInIt(x: int) returns (r: int) {
  assume x > 0;
  assume {:axiom} x > 10;

  forall y : int
    ensures y != y

  var i := 0;
  while i < x
    decreases x - i

  return x;
}

method AssertOnly() {
  assert {:only} true;
  assert false;
}