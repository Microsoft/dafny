// RUN: %dafny_0 /compile:0 /unicodeChar:1 /print:"%t.print" /dprint:"%t.dprint" "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

class CharChar {
  var c: char;
  var d: char;

  method Order()
    modifies this;
    ensures c <= d;
  {
    if d < c {
      c, d := d, c;
    }
  }

  function Recurse(ch: char): bool
    reads this;
  {
    if c < ch then Recurse(c)
    else if d < ch then Recurse(d)
    else ch == ' '
  }

  function MinChar(s: string): char
    requires s != "";
  {
    var ch := s[0];
    if |s| == 1 then ch else
    var m := MinChar(s[1..]);
    if m < ch then m else ch
  }
  lemma MinCharLemma(s: string)
    requires |s| != 0;
    ensures forall i :: 0 <= i < |s| ==> MinChar(s) <= s[i];
  {
    if 2 <= |s| {
      var m := MinChar(s[1..]);
      assert forall i :: 1 <= i < |s| ==> m <= s[1..][i-1] == s[i];
    }
  }

  method CharEq(s: string) {
    if "hello Dafny" <= s {
      assert s[6] == 'D';
      assert s[7] == '\U{61}';
      if * {
        assert s[9] == '\n';  // error
      } else if * {
        assert s[1] < s[2] <= s[3];
      } else {
        assert s[0] <= s[5];  // error
      }
    }
  }
  method CharInbetween(ch: char)
    requires 'B' < ch;
  {
    if ch < 'D' {
      assert 'C' <= ch <= 'C';
      assert ch == 'C';
    } else {
      assert ch <= 'M';  // error
    }
  }
}

// arithmetic with char's

function toLower(ch: char): char
{
  if 'A' <= ch <= 'Z' then
    ch - 'A' + 'a'
  else
    ch
}

function BadToLower_Overflow(ch: char): char
{
  if 'A' <= ch then
    ch - 'A' + 'a'  // error: possible overvlow
  else
    ch
}

function BadToLower_Underflow(ch: char): char
{
  if ch <= 'Z' then
    ch - 'A' + 'a'  // error: possible overvlow
  else
    ch
}

lemma AboutChar(ch: char, dh: char)
  requires 'r' < dh < '\U{abcd}'
  ensures ch == ch + '\0' == ch - '\0'
  ensures dh - '\U{20}' < dh
  ensures dh + '\U{20}' > dh
  ensures '\n' + dh == dh + '\n'
{
}
