// RUN: %dafny /compile:0 /printTooltips "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

// This test ensures that the trigger  collector (the routine that picks trigger
// candidates) does not  actually consider all subsets of terms;  if it did, the
// following would take horribly long

ghost predicate P0(x: bool)
ghost predicate P1(x: bool)
ghost predicate P2(x: bool)
ghost predicate P3(x: bool)
ghost predicate P4(x: bool)
ghost predicate P5(x: bool)
ghost predicate P6(x: bool)
ghost predicate P7(x: bool)
ghost predicate P8(x: bool)
ghost predicate P9(x: bool)
ghost predicate P10(x: bool)
ghost predicate P11(x: bool)
ghost predicate P12(x: bool)
ghost predicate P13(x: bool)
ghost predicate P14(x: bool)
ghost predicate P15(x: bool)
ghost predicate P16(x: bool)
ghost predicate P17(x: bool)
ghost predicate P18(x: bool)
ghost predicate P19(x: bool)
ghost predicate P20(x: bool)
ghost predicate P21(x: bool)
ghost predicate P22(x: bool)
ghost predicate P23(x: bool)
ghost predicate P24(x: bool)
ghost predicate P25(x: bool)
ghost predicate P26(x: bool)
ghost predicate P27(x: bool)
ghost predicate P28(x: bool)
ghost predicate P29(x: bool)
ghost predicate P30(x: bool)
ghost predicate P31(x: bool)
ghost predicate P32(x: bool)
ghost predicate P33(x: bool)
ghost predicate P34(x: bool)
ghost predicate P35(x: bool)
ghost predicate P36(x: bool)
ghost predicate P37(x: bool)
ghost predicate P38(x: bool)
ghost predicate P39(x: bool)
ghost predicate P40(x: bool)
ghost predicate P41(x: bool)
ghost predicate P42(x: bool)
ghost predicate P43(x: bool)
ghost predicate P44(x: bool)
ghost predicate P45(x: bool)
ghost predicate P46(x: bool)
ghost predicate P47(x: bool)
ghost predicate P48(x: bool)
ghost predicate P49(x: bool)

method M() {
  assert forall x :: true || P0(x) || P1(x) || P2(x) || P3(x) || P4(x) || P5(x) || P6(x) || P7(x) || P8(x) || P9(x) || P10(x) || P11(x) || P12(x) || P13(x) || P14(x) || P15(x) || P16(x) || P17(x) || P18(x) || P19(x) || P20(x) || P21(x) || P22(x) || P23(x) || P24(x) || P25(x) || P26(x) || P27(x) || P28(x) || P29(x) || P30(x) || P31(x) || P32(x) || P33(x) || P34(x) || P35(x) || P36(x) || P37(x) || P38(x) || P39(x) || P40(x) || P41(x) || P42(x) || P43(x) || P44(x) || P45(x) || P46(x) || P47(x) || P48(x) || P49(x);
}
