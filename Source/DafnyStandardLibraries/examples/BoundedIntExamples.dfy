import opened DafnyStdLibs.BoundedInts

lemma BoundedIntUser(x: uint32, z: nat16)
  ensures TWO_TO_THE_15 * 2 == TWO_TO_THE_16
  ensures 0 <= (x as int) < TWO_TO_THE_32
{
  var y: uint64 := x as int as uint64;
  var int16 := z as int as int16;
  var uint16 := z as int as uint16;
}

method {:test} UseExterns() {
  var squareOf8 := Externs.NonDefault.SquareNativeInt(8);
  expect squareOf8 == 64;
}

module {:extern} {:member "NonDefault", true} Externs {
  import opened DafnyStdLibs.BoundedInts
  class {:extern} NonDefault {
    static method {:extern} SquareNativeInt(i: int32) returns (r: int32)
  }
}
