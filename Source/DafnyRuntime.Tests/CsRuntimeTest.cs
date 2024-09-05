﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Threading;
using Xunit;
using Dafny;

namespace DafnyRuntime.Tests;

public class RuntimeTest {
  [Fact]
  public void TestConcatSequence() {
    var c = new ConcatSequence<int>(Sequence<int>.FromElements(1, 2), Sequence<int>.FromElements(3, 4));
    var x = c.ImmutableElements;
    c.ComputeElements(); // For coverage
    var left = new ConcatSequence<int>(Sequence<int>.FromElements(1), Sequence<int>.FromElements(2));
    var tree = new ConcatSequence<int>(left, Sequence<int>.FromElements(3, 4));
    left.left = null; // For coverage, we simulate multithreading
    Thread t = new Thread(delegate () {
      left.right = null;
      Thread.Sleep(100); // Ensure ComputeElements() does not work immediately
      left.elmts = new List<int>() { 1, 2 }.ToImmutableArray();
    });
    t.Start();
    var result = tree.ComputeElements(); // Without the thread, will loop forever
    Assert.Equal(Sequence<int>.FromArray(result.ToArray()), Sequence<int>.FromArray(x.ToArray()));
  }

  class Dummy { }

  [Fact]
  public void TestSetAndCoverage() {
    var x = Set<Dummy?>.FromCollectionPlusOne(new List<Dummy?>() { new Dummy(), null }, new Dummy());
    var y = Set<Dummy?>.FromCollectionPlusOne(new List<Dummy?>() { new Dummy(), new Dummy() }, new Dummy());
    var z = Set<Dummy?>.FromCollectionPlusOne(new List<Dummy?>() { new Dummy(), new Dummy() }, null);
    Assert.Equal(3, x.Count);
    Assert.Equal(3, y.Count);
    Assert.Equal(3, z.Count);
    Assert.Equal(3, x.LongCount);
    Assert.Equal(3, y.LongCount);
    Assert.Equal(3, z.LongCount);
    Assert.Equal(8, x.AllSubsets.ToList().Count);
    Assert.Equal(8, y.AllSubsets.ToList().Count);
    Assert.Equal(8, z.AllSubsets.ToList().Count);
    Dafny.ISet<Dummy?>? n = null;
    Assert.False(x.Equals(n));
    object? nObject = null;
    Assert.False(x.Equals(nObject));
    Assert.False(x.EqualsAux(n));
    Assert.NotEqual(0, x.GetHashCode());
  }

  [Fact]
  public void TestMapAndCoverage() {
    Map<Dummy?, Dummy?>? n = null;
    var x = Map<Dummy?, Dummy?>.FromElements(
      new Pair<Dummy, Dummy>(new Dummy(), new Dummy()),
      new Pair<Dummy?, Dummy>(null, new Dummy()),
      new Pair<Dummy, Dummy?>(new Dummy(), null));

    var nn = Map<Dummy?, Dummy?>.FromElements(
      new Pair<Dummy, Dummy>(new Dummy(), new Dummy()));
    Assert.Equal(3, x.Count);
    Assert.Equal(3, x.LongCount);
    Assert.Equal(1, nn.Count);
    Assert.Equal(1, nn.LongCount);
    Assert.False(nn.Equals(n));

    var oo = Map<object?, object?>.FromElements(
      new Pair<Dummy, Dummy>(new Dummy(), new Dummy()),
      new Pair<Dummy?, Dummy>(null, new Dummy()),
      new Pair<Dummy, Dummy?>(new Dummy(), null));
    Assert.True(oo.EqualsObjObj(oo));
    // TBC
  }

  [Fact]
  public void TestHaltException() {
    var x = new HaltException("Test failed");
    Assert.Equal("Test failed", x.Message);
  }

  [Fact]
  public void RuneTest() {
    Assert.Throws<ArgumentException>(() => new Rune(0xD801));
    Assert.Throws<ArgumentException>(() => new Rune(0x11_0000));
    Assert.False(new Rune(65).Equals(null));
    Assert.True(new Rune(65).Equals(new Rune(65)));
    Assert.Equal(-1, new Rune(65).CompareTo(new Rune(66)));
    Assert.Equal(0, new Rune(65).CompareTo(new Rune(65)));
    Assert.Equal(1, new Rune(65).CompareTo(new Rune(64)));
    IComparable c = new Rune(65);
    Assert.Equal(1, c.CompareTo(null));
    Assert.Equal(0, c.CompareTo(new Rune(65)));
    Assert.Throws<ArgumentException>(() => c.CompareTo("abc"));
    var input = "\uD800"; // High surrogate but no low surrogate afterward
    Assert.Throws<IndexOutOfRangeException>(() => Rune.Enumerate(input).ToList());
    input = "\uDC00"; // Low surrogate
    Assert.Throws<ArgumentException>(() => Rune.Enumerate(input).ToList());
    input = "\uD800\uD800"; // High surrogate twice
    Assert.Throws<ArgumentException>(() => Rune.Enumerate(input).ToList());
  }

  [Fact]
  public void DowncastClone0() {
    Func<int> originalFunc = () => 42;
    var clonedFunc = originalFunc.DowncastClone(i => i.ToString());
    Assert.Equal("42", clonedFunc());
  }

  [Fact]
  public void DowncastClone1() {
    Func<int, double> originalFunc = x => x * 2.5;
    var clonedFunc = originalFunc.DowncastClone<int, double, string, string>(int.Parse, d => d.ToString("F2"));
    Assert.Equal("5.00", clonedFunc("2"));
  }

  [Fact]
  public void DowncastClone2() {
    Func<int, int, int> originalFunc = (x, y) => x + y;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, string, string, string>(int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("7", clonedFunc("3", "4"));
  }

  [Fact]
  public void DowncastClone3() {
    Func<int, int, int, int> originalFunc = (x, y, z) => x + y + z;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, string, string, string, string>(int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("10", clonedFunc("2", "3", "5"));
  }

  [Fact]
  public void DowncastClone4() {
    Func<int, int, int, int, int> originalFunc = (a, b, c, d) => a + b + c + d;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("10", clonedFunc("1", "2", "3", "4"));
  }

  [Fact]
  public void DowncastClone5() {
    Func<int, int, int, int, int, int> originalFunc = (a, b, c, d, e) => a + b + c + d + e;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("15", clonedFunc("1", "2", "3", "4", "5"));
  }

  [Fact]
  public void DowncastClone6() {
    Func<int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f) => a + b + c + d + e + f;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, string, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("21", clonedFunc("1", "2", "3", "4", "5", "6"));
  }

  [Fact]
  public void DowncastClone7() {
    Func<int, int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f, g) => a + b + c + d + e + f + g;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, int, string, string, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("28", clonedFunc("1", "2", "3", "4", "5", "6", "7"));
  }

  [Fact]
  public void DowncastClone8() {
    Func<int, int, int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, int, int, string, string, string, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("36", clonedFunc("1", "2", "3", "4", "5", "6", "7", "8"));
  }

  [Fact]
  public void DowncastClone9() {
    Func<int, int, int, int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, int, int, int, string, string, string, string, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("45", clonedFunc("1", "2", "3", "4", "5", "6", "7", "8", "9"));
  }

  [Fact]
  public void DowncastClone10() {
    Func<int, int, int, int, int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, int, int, int, int, string, string, string, string, string, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("55", clonedFunc("1", "2", "3", "4", "5", "6", "7", "8", "9", "10"));
  }

  [Fact]
  public void DowncastClone11() {
    Func<int, int, int, int, int, int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, int, int, int, int, int, string, string, string, string, string, string, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("66", clonedFunc("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11"));
  }

  [Fact]
  public void DowncastClone12() {
    Func<int, int, int, int, int, int, int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, int, int, int, int, int, int, string, string, string, string, string, string, string, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("78", clonedFunc("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"));
  }

  [Fact]
  public void DowncastClone13() {
    Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("91", clonedFunc("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13"));
  }

  [Fact]
  public void DowncastClone14() {
    Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("105", clonedFunc("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"));
  }

  [Fact]
  public void DowncastClone15() {
    Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, sum => sum.ToString());
    Assert.Equal("120", clonedFunc("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"));
  }

  [Fact]
  public void DowncastClone16() {
    Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> originalFunc = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p;
    var clonedFunc = originalFunc.DowncastClone<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
        int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse,
        int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse, int.Parse,
        sum => sum.ToString()
    );
    Assert.Equal("136", clonedFunc("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16"));
  }


  [Fact]
  public void InitNewArray1Test() {
    var a = ArrayHelpers.InitNewArray1('a',
      new BigInteger(2));
    Assert.Equal(2, a.Length);
    Assert.Equal('a', a[0]);
    Assert.Equal('a', a[1]);
  }

  [Fact]
  public void InitNewArray2Test() {
    var a = ArrayHelpers.InitNewArray2('a',
      new BigInteger(1),
      new BigInteger(2));
    for (var i = 0; i < 2; i++) {
      Assert.Equal(i == 1 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0]);
    Assert.Equal('a', a[0, 1]);
  }

  [Fact]
  public void InitNewArray3Test() {
    var a = ArrayHelpers.InitNewArray3('a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2));
    for (var i = 0; i < 3; i++) {
      Assert.Equal(i == 2 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0]);
    Assert.Equal('a', a[0, 0, 1]);
  }

  [Fact]
  public void InitNewArray4Test() {
    var a = ArrayHelpers.InitNewArray4(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 4; i++) {
      Assert.Equal(i == 3 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray5Test() {
    var a = ArrayHelpers.InitNewArray5(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 5; i++) {
      Assert.Equal(i == 4 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray6Test() {
    var a = ArrayHelpers.InitNewArray6(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 6; i++) {
      Assert.Equal(i == 5 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray7Test() {
    var a = ArrayHelpers.InitNewArray7(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 7; i++) {
      Assert.Equal(i == 6 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray8Test() {
    var a = ArrayHelpers.InitNewArray8(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 8; i++) {
      Assert.Equal(i == 7 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray9Test() {
    var a = ArrayHelpers.InitNewArray9(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 9; i++) {
      Assert.Equal(i == 8 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray10Test() {
    var a = ArrayHelpers.InitNewArray10(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 10; i++) {
      Assert.Equal(i == 9 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray11Test() {
    var a = ArrayHelpers.InitNewArray11(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 11; i++) {
      Assert.Equal(i == 10 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray12Test() {
    var a = ArrayHelpers.InitNewArray12(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 12; i++) {
      Assert.Equal(i == 11 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray13Test() {
    var a = ArrayHelpers.InitNewArray13(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 13; i++) {
      Assert.Equal(i == 12 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray14Test() {
    var a = ArrayHelpers.InitNewArray14(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 14; i++) {
      Assert.Equal(i == 13 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray15Test() {
    var a = ArrayHelpers.InitNewArray15(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 15; i++) {
      Assert.Equal(i == 14 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1]);
  }

  [Fact]
  public void InitNewArray16Test() {
    var a = ArrayHelpers.InitNewArray16(
      'a',
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(1),
      new BigInteger(2)
    );
    for (var i = 0; i < 16; i++) {
      Assert.Equal(i == 15 ? 2 : 1, a.GetLength(i));
    }
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    Assert.Equal('a', a[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1]);
  }
}