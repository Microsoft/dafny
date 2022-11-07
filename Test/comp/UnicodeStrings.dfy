// RUN: %testDafnyForEachCompiler "%s" -- /unicodeChar:1

// TODO:
// * Parse errors with character literals

newtype uint8 = x: int | 0 <= x < 0x100 
newtype uint16 = x: int | 0 <= x < 0x1_0000 
newtype uint32 = x: int | 0 <= x < 0x1_0000_0000 
newtype uint64 = x: int | 0 <= x < 0x1_0000_0000_0000_0000 
newtype int8 = x: int | -0x80 <= x < 0x80
newtype int16 = x: int | -0x8000 <= x < 0x8000
newtype int32 = x: int | -0x8000_0000 <= x < 0x8000_0000
newtype int64 = x: int | -0x8000_0000_0000_0000 <= x < 8000_0000_0000_0000


// WARNING: Do not do this in real code!
// It's a great example of what NOT to do when working with Unicode,
// since the concept of upper/lower case is culture-specific.
// TODO: more thorough testing of comparators, especially
// edge cases where lexicographic comparision of UTF-8/16 would get the wrong answer.
function method ToLower(ch: char): char {
  if 'A' <= ch <= 'Z' then
    ch - 'A' + 'a'
  else
    ch
}

function method MapToLower(s: string): string {
  if 0 == |s| then
    []
  else
    [ToLower(s[0])] + MapToLower(s[1..])
}

function method MapToInt32(s: string): seq<int32> {
  if 0 == |s| then
    []
  else
    [s[0] as int32] + MapToInt32(s[1..])
}

datatype Option<T> = Some(value: T) | None

datatype StringOption = SomeString(value: string) | NoString

method Main(args: seq<string>) {
  var trickyString := "Dafny is just so \U{1F60E}";
  print trickyString, "\n";

  var trickyString2 := "Dafny is just so " + [0x1F60E as char];
  print trickyString2, "\n";

  // Testing that runtimes don't confuse a seq<uint32> for a string
  // (which would be a problem if we used Int32 in C# instead of Rune, for example)
  var s := "Ceci n'est pas une string";
  var notAString := MapToInt32(s);
  print notAString, "\n";

  // Ensuring character arithmetic can be compiled
  var sarcastic := "Oh UNicOdE, tHaT's a REaL usEFuL FEaTuRe!";
  var sincere := MapToLower(sarcastic);
  print sincere, "\n";

  print 'D', "\n";
  // TODO:
  // print '\'', "\n";
  // print '\n', "\n";

  var mightBeString := Some(trickyString);
  print mightBeString, "\n";
  print mightBeString.value, "\n";

  var definitelyString := SomeString(trickyString);
  print definitelyString, "\n";
  print definitelyString.value, "\n";

  var tupleOfString := (trickyString, 42);
  print tupleOfString, "\n";

  Print("D");
  Print('D');
  Print(0x1F60E as char);

  CharCasting();
  CharComparisons();
  AllCharsTest();
}

method Print<T>(t: T) {
  print t, "\n";
}

method AssertAndExpect(p: bool) 
  requires p
{
  expect p;
}

method CharCasting() {
  var chars := "\0azAZ\U{10FFFF}";
  for i := 0 to |chars| {
    CastChar(chars[i]);
  }
}

method CastChar(c: char) {
  if (c as int < 0x80) {
    var asInt8 := c as int8;
    AssertAndExpect(asInt8 as char == c);
  }
  if (c as int < 0x100) {
    var asUint8 := c as uint8;
    AssertAndExpect(asUint8 as char == c);
  }
  if (c as int < 0x8000) {
    var asInt16 := c as int16;
    AssertAndExpect(asInt16 as char == c);
  }
  if (c as int < 0x1_0000) {
    var asUint16 := c as uint16;
    AssertAndExpect(asUint16 as char == c);
  }
  var asInt32 := c as int32;
  AssertAndExpect(asInt32 as char == c);
  var asInt64 := c as int64;
  AssertAndExpect(asInt64 as char == c);
}

method CharComparisons() {
  AssertAndExpect('a' < 'b');
  AssertAndExpect('a' <= 'a');
  AssertAndExpect('b' > 'a');
  AssertAndExpect('b' >= 'a');

  // Some evil edge cases to catch comparing
  // encoded bytes lexicographically by accident 😈.
  // UTF-8 seems to have the property that this comparison
  // is the same, but UTF-16 doesn't.

  // '￮' == [0xFFEE] in UTF-16
  // '𝄞' == [0xD834, 0xDD1E] in UTF-16
  AssertAndExpect('￮' < '\U{1D11E}');
}

method AllCharsTest() {
  // var allChars := set c: char | true;
  // var allCodePoints := (set cp: int | 0 <= cp < 0xD800 :: cp as char)
  //                    + (set cp: int | 0xE000 <= cp < 0x11_0000 :: cp as char);
  // assert forall c: char :: 0 <= c as int < 0xD800 || 0xE000 <= c as int < 0x11_0000;
  // assert allChars == allCodePoints;
  // expect allChars == allCodePoints;

}
