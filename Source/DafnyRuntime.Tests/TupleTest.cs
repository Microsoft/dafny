using System;
using Xunit;
using Dafny;

namespace DafnyRuntime.Tests {
  public class TupleTests {

    public Func<int, string> C = (int i) => i.ToString();
    public Func<int, int> I = (int i) => i;
    public Dafny.TypeDescriptor<int> D = new Dafny.TypeDescriptor<int>(0);

    [Fact]
    public void TestTuple0() {
      var t = new _System.Tuple0();
      Assert.False(t.Equals(null));
      Assert.True(t.Equals(new _System.Tuple0()));
      Assert.Equal(t, t.DowncastClone());
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("()", t.ToString());
      Assert.Equal(t, _System.Tuple0.Default());
      Assert.Equal(t, _System.Tuple0.create____hMake0());
      Assert.Equal(_System.Tuple0.create(), _System.Tuple0._TypeDescriptor().Default());
    }

    [Fact]
    public void TestTuple1() {
      var t = new _System.Tuple1<int>(0);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 1; i++) {
        Assert.Equal(i == 1, t.Equals(
          new _System.Tuple1<int>(
            i <= 0 ? 1 : 0)));
      }

      Assert.Equal(new _System.Tuple1<string>("0"), t.DowncastClone(C));
      Assert.Equal(t, t.DowncastClone(I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0)", t.ToString());
      Assert.Equal(t, _System.Tuple1<int>.Default(0));
      Assert.Equal(t, _System.Tuple1<int>.create____hMake1(0));
      Assert.Equal(_System.Tuple1<int>.create(0), _System.Tuple1<int>._TypeDescriptor(D).Default());
      Assert.Equal(0, t.dtor__0);
    }

    [Fact]
    public void TestTuple2() {
      var t = new _System.Tuple2<int, int>(0, 1);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 2; i++) {
        Assert.Equal(i == 2, t.Equals(
          new _System.Tuple2<int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1)));
      }

      Assert.Equal(new _System.Tuple2<string, string>("0", "1"), t.DowncastClone(C, C));
      Assert.Equal(t, t.DowncastClone(I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1)", t.ToString());
      Assert.Equal(t, _System.Tuple2<int, int>.Default(0, 1));
      Assert.Equal(t, _System.Tuple2<int, int>.create____hMake2(0, 1));
      Assert.Equal(_System.Tuple2<int, int>.create(0, 0), _System.Tuple2<int, int>._TypeDescriptor(D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
    }

    // I am grateful for LLMs to generate all these tests.
    [Fact]
    public void TestTuple3() {
      var t = new _System.Tuple3<int, int, int>(0, 1, 2);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 3; i++) {
        Assert.Equal(i == 3, t.Equals(
          new _System.Tuple3<int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2)));
      }

      Assert.Equal(new _System.Tuple3<string, string, string>("0", "1", "2"), t.DowncastClone(C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2)", t.ToString());
      Assert.Equal(t, _System.Tuple3<int, int, int>.Default(0, 1, 2));
      Assert.Equal(t, _System.Tuple3<int, int, int>.create____hMake3(0, 1, 2));
      Assert.Equal(_System.Tuple3<int, int, int>.create(0, 0, 0),
        _System.Tuple3<int, int, int>._TypeDescriptor(D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
    }

    [Fact]
    public void TestTuple4() {
      var t = new _System.Tuple4<int, int, int, int>(0, 1, 2, 3);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 4; i++) {
        Assert.Equal(i == 4, t.Equals(
          new _System.Tuple4<int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3)));
      }

      Assert.Equal(new _System.Tuple4<string, string, string, string>("0", "1", "2", "3"), t.DowncastClone(C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3)", t.ToString());
      Assert.Equal(t, _System.Tuple4<int, int, int, int>.Default(0, 1, 2, 3));
      Assert.Equal(t, _System.Tuple4<int, int, int, int>.create____hMake4(0, 1, 2, 3));
      Assert.Equal(_System.Tuple4<int, int, int, int>.create(0, 0, 0, 0),
        _System.Tuple4<int, int, int, int>._TypeDescriptor(D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
    }

    [Fact]
    public void TestTuple5() {
      var t = new _System.Tuple5<int, int, int, int, int>(0, 1, 2, 3, 4);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 5; i++) {
        Assert.Equal(i == 5, t.Equals(
          new _System.Tuple5<int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4)));
      }

      Assert.Equal(new _System.Tuple5<string, string, string, string, string>("0", "1", "2", "3", "4"), t.DowncastClone(C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4)", t.ToString());
      Assert.Equal(t, _System.Tuple5<int, int, int, int, int>.Default(0, 1, 2, 3, 4));
      Assert.Equal(t, _System.Tuple5<int, int, int, int, int>.create____hMake5(0, 1, 2, 3, 4));
      Assert.Equal(_System.Tuple5<int, int, int, int, int>.create(0, 0, 0, 0, 0),
        _System.Tuple5<int, int, int, int, int>._TypeDescriptor(D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
    }

    [Fact]
    public void TestTuple6() {
      var t = new _System.Tuple6<int, int, int, int, int, int>(0, 1, 2, 3, 4, 5);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 6; i++) {
        Assert.Equal(i == 6, t.Equals(
          new _System.Tuple6<int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5)));
      }

      Assert.Equal(new _System.Tuple6<string, string, string, string, string, string>("0", "1", "2", "3", "4", "5"), t.DowncastClone(C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5)", t.ToString());
      Assert.Equal(t, _System.Tuple6<int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5));
      Assert.Equal(t, _System.Tuple6<int, int, int, int, int, int>.create____hMake6(0, 1, 2, 3, 4, 5));
      Assert.Equal(_System.Tuple6<int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0),
        _System.Tuple6<int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
    }

    [Fact]
    public void TestTuple7() {
      var t = new _System.Tuple7<int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 7; i++) {
        Assert.Equal(i == 7, t.Equals(
          new _System.Tuple7<int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6)));
      }

      Assert.Equal(new _System.Tuple7<string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6"), t.DowncastClone(C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6)", t.ToString());
      Assert.Equal(t, _System.Tuple7<int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6));
      Assert.Equal(t, _System.Tuple7<int, int, int, int, int, int, int>.create____hMake7(0, 1, 2, 3, 4, 5, 6));
      Assert.Equal(_System.Tuple7<int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0),
        _System.Tuple7<int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
    }

    [Fact]
    public void TestTuple8() {
      var t = new _System.Tuple8<int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 8; i++) {
        Assert.Equal(i == 8, t.Equals(
          new _System.Tuple8<int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7)));
      }

      Assert.Equal(new _System.Tuple8<string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7"), t.DowncastClone(C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7)", t.ToString());
      Assert.Equal(t, _System.Tuple8<int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7));
      Assert.Equal(t, _System.Tuple8<int, int, int, int, int, int, int, int>.create____hMake8(0, 1, 2, 3, 4, 5, 6, 7));
      Assert.Equal(_System.Tuple8<int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple8<int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
    }

    [Fact]
    public void TestTuple9() {
      var t = new _System.Tuple9<int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 9; i++) {
        Assert.Equal(i == 9, t.Equals(
          new _System.Tuple9<int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8)));
      }

      Assert.Equal(new _System.Tuple9<string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8"), t.DowncastClone(C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8)", t.ToString());
      Assert.Equal(t, _System.Tuple9<int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8));
      Assert.Equal(t, _System.Tuple9<int, int, int, int, int, int, int, int, int>.create____hMake9(0, 1, 2, 3, 4, 5, 6, 7, 8));
      Assert.Equal(_System.Tuple9<int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple9<int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
    }

    [Fact]
    public void TestTuple10() {
      var t = new _System.Tuple10<int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 10; i++) {
        Assert.Equal(i == 10, t.Equals(
          new _System.Tuple10<int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9)));
      }

      Assert.Equal(new _System.Tuple10<string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9)", t.ToString());
      Assert.Equal(t, _System.Tuple10<int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9));
      Assert.Equal(t, _System.Tuple10<int, int, int, int, int, int, int, int, int, int>.create____hMake10(0, 1, 2, 3, 4, 5, 6, 7, 8, 9));
      Assert.Equal(_System.Tuple10<int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple10<int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
    }

    [Fact]
    public void TestTuple11() {
      var t = new _System.Tuple11<int, int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 11; i++) {
        Assert.Equal(i == 11, t.Equals(
          new _System.Tuple11<int, int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9,
            i <= 10 ? 11 : 10)));
      }

      Assert.Equal(new _System.Tuple11<string, string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10)", t.ToString());
      Assert.Equal(t, _System.Tuple11<int, int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
      Assert.Equal(t, _System.Tuple11<int, int, int, int, int, int, int, int, int, int, int>.create____hMake11(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
      Assert.Equal(_System.Tuple11<int, int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple11<int, int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
      Assert.Equal(10, t.dtor__10);
    }

    [Fact]
    public void TestTuple12() {
      var t = new _System.Tuple12<int, int, int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 12; i++) {
        Assert.Equal(i == 12, t.Equals(
          new _System.Tuple12<int, int, int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9,
            i <= 10 ? 11 : 10,
            i <= 11 ? 12 : 11)));
      }

      Assert.Equal(new _System.Tuple12<string, string, string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)", t.ToString());
      Assert.Equal(t, _System.Tuple12<int, int, int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
      Assert.Equal(t, _System.Tuple12<int, int, int, int, int, int, int, int, int, int, int, int>.create____hMake12(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
      Assert.Equal(_System.Tuple12<int, int, int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple12<int, int, int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
      Assert.Equal(10, t.dtor__10);
      Assert.Equal(11, t.dtor__11);
    }

    [Fact]
    public void TestTuple13() {
      var t = new _System.Tuple13<int, int, int, int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 13; i++) {
        Assert.Equal(i == 13, t.Equals(
          new _System.Tuple13<int, int, int, int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9,
            i <= 10 ? 11 : 10,
            i <= 11 ? 12 : 11,
            i <= 12 ? 13 : 12)));
      }
      Assert.Equal(new _System.Tuple13<string, string, string, string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)", t.ToString());
      Assert.Equal(t, _System.Tuple13<int, int, int, int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
      Assert.Equal(t, _System.Tuple13<int, int, int, int, int, int, int, int, int, int, int, int, int>.create____hMake13(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
      Assert.Equal(_System.Tuple13<int, int, int, int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple13<int, int, int, int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
      Assert.Equal(10, t.dtor__10);
      Assert.Equal(11, t.dtor__11);
      Assert.Equal(12, t.dtor__12);
    }

    [Fact]
    public void TestTuple14() {
      var t = new _System.Tuple14<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 14; i++) {
        Assert.Equal(i == 14, t.Equals(
          new _System.Tuple14<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9,
            i <= 10 ? 11 : 10,
            i <= 11 ? 12 : 11,
            i <= 12 ? 13 : 12,
            i <= 13 ? 14 : 13)));
      }

      Assert.Equal(new _System.Tuple14<string, string, string, string, string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)", t.ToString());
      Assert.Equal(t, _System.Tuple14<int, int, int, int, int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
      Assert.Equal(t, _System.Tuple14<int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create____hMake14(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
      Assert.Equal(_System.Tuple14<int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple14<int, int, int, int, int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
      Assert.Equal(10, t.dtor__10);
      Assert.Equal(11, t.dtor__11);
      Assert.Equal(12, t.dtor__12);
      Assert.Equal(13, t.dtor__13);
    }

    [Fact]
    public void TestTuple15() {
      var t = new _System.Tuple15<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 15; i++) {
        Assert.Equal(i == 15, t.Equals(
          new _System.Tuple15<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9,
            i <= 10 ? 11 : 10,
            i <= 11 ? 12 : 11,
            i <= 12 ? 13 : 12,
            i <= 13 ? 14 : 13,
            i <= 14 ? 15 : 14)));
      }

      Assert.Equal(new _System.Tuple15<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)", t.ToString());
      Assert.Equal(t, _System.Tuple15<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
      Assert.Equal(t, _System.Tuple15<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create____hMake15(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
      Assert.Equal(_System.Tuple15<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple15<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
      Assert.Equal(10, t.dtor__10);
      Assert.Equal(11, t.dtor__11);
      Assert.Equal(12, t.dtor__12);
      Assert.Equal(13, t.dtor__13);
      Assert.Equal(14, t.dtor__14);
    }
    [Fact]
    public void TestTuple16() {
      var t = new _System.Tuple16<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 16; i++) {
        Assert.Equal(i == 16, t.Equals(
          new _System.Tuple16<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9,
            i <= 10 ? 11 : 10,
            i <= 11 ? 12 : 11,
            i <= 12 ? 13 : 12,
            i <= 13 ? 14 : 13,
            i <= 14 ? 15 : 14,
            i <= 15 ? 16 : 15)));
      }

      Assert.Equal(new _System.Tuple16<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)", t.ToString());
      Assert.Equal(t, _System.Tuple16<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
      Assert.Equal(t, _System.Tuple16<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create____hMake16(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
      Assert.Equal(_System.Tuple16<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple16<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
      Assert.Equal(10, t.dtor__10);
      Assert.Equal(11, t.dtor__11);
      Assert.Equal(12, t.dtor__12);
      Assert.Equal(13, t.dtor__13);
      Assert.Equal(14, t.dtor__14);
      Assert.Equal(15, t.dtor__15);
    }

    [Fact]
    public void TestTuple17() {
      var t = new _System.Tuple17<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 17; i++) {
        Assert.Equal(i == 17, t.Equals(
          new _System.Tuple17<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9,
            i <= 10 ? 11 : 10,
            i <= 11 ? 12 : 11,
            i <= 12 ? 13 : 12,
            i <= 13 ? 14 : 13,
            i <= 14 ? 15 : 14,
            i <= 15 ? 16 : 15,
            i <= 16 ? 17 : 16)));
      }

      Assert.Equal(new _System.Tuple17<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)", t.ToString());
      Assert.Equal(t, _System.Tuple17<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
      Assert.Equal(t, _System.Tuple17<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create____hMake17(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
      Assert.Equal(_System.Tuple17<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple17<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
      Assert.Equal(10, t.dtor__10);
      Assert.Equal(11, t.dtor__11);
      Assert.Equal(12, t.dtor__12);
      Assert.Equal(13, t.dtor__13);
      Assert.Equal(14, t.dtor__14);
      Assert.Equal(15, t.dtor__15);
      Assert.Equal(16, t.dtor__16);
    }

    [Fact]
    public void TestTuple18() {
      var t = new _System.Tuple18<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 18; i++) {
        Assert.Equal(i == 18, t.Equals(
          new _System.Tuple18<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9,
            i <= 10 ? 11 : 10,
            i <= 11 ? 12 : 11,
            i <= 12 ? 13 : 12,
            i <= 13 ? 14 : 13,
            i <= 14 ? 15 : 14,
            i <= 15 ? 16 : 15,
            i <= 16 ? 17 : 16,
            i <= 17 ? 18 : 17)));
      }

      Assert.Equal(new _System.Tuple18<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17)", t.ToString());
      Assert.Equal(t, _System.Tuple18<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17));
      Assert.Equal(t, _System.Tuple18<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create____hMake18(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17));
      Assert.Equal(_System.Tuple18<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple18<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
      Assert.Equal(10, t.dtor__10);
      Assert.Equal(11, t.dtor__11);
      Assert.Equal(12, t.dtor__12);
      Assert.Equal(13, t.dtor__13);
      Assert.Equal(14, t.dtor__14);
      Assert.Equal(15, t.dtor__15);
      Assert.Equal(16, t.dtor__16);
      Assert.Equal(17, t.dtor__17);
    }

    [Fact]
    public void TestTuple19() {
      var t = new _System.Tuple19<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 19; i++) {
        Assert.Equal(i == 19, t.Equals(
          new _System.Tuple19<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9,
            i <= 10 ? 11 : 10,
            i <= 11 ? 12 : 11,
            i <= 12 ? 13 : 12,
            i <= 13 ? 14 : 13,
            i <= 14 ? 15 : 14,
            i <= 15 ? 16 : 15,
            i <= 16 ? 17 : 16,
            i <= 17 ? 18 : 17,
            i <= 18 ? 19 : 18)));
      }

      Assert.Equal(new _System.Tuple19<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18)", t.ToString());
      Assert.Equal(t, _System.Tuple19<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18));
      Assert.Equal(t, _System.Tuple19<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create____hMake19(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18));
      Assert.Equal(_System.Tuple19<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple19<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
      Assert.Equal(10, t.dtor__10);
      Assert.Equal(11, t.dtor__11);
      Assert.Equal(12, t.dtor__12);
      Assert.Equal(13, t.dtor__13);
      Assert.Equal(14, t.dtor__14);
      Assert.Equal(15, t.dtor__15);
      Assert.Equal(16, t.dtor__16);
      Assert.Equal(17, t.dtor__17);
      Assert.Equal(18, t.dtor__18);
    }

    [Fact]
    public void TestTuple20() {
      var t = new _System.Tuple20<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19);
      Assert.False(t.Equals(null));
      for (var i = 0; i <= 20; i++) {
        Assert.Equal(i == 20, t.Equals(
          new _System.Tuple20<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
            i <= 0 ? 1 : 0,
            i <= 1 ? 2 : 1,
            i <= 2 ? 3 : 2,
            i <= 3 ? 4 : 3,
            i <= 4 ? 5 : 4,
            i <= 5 ? 6 : 5,
            i <= 6 ? 7 : 6,
            i <= 7 ? 8 : 7,
            i <= 8 ? 9 : 8,
            i <= 9 ? 10 : 9,
            i <= 10 ? 11 : 10,
            i <= 11 ? 12 : 11,
            i <= 12 ? 13 : 12,
            i <= 13 ? 14 : 13,
            i <= 14 ? 15 : 14,
            i <= 15 ? 16 : 15,
            i <= 16 ? 17 : 16,
            i <= 17 ? 18 : 17,
            i <= 18 ? 19 : 18,
            i <= 19 ? 20 : 19)));
      }

      Assert.Equal(new _System.Tuple20<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19"), t.DowncastClone(C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C, C));
      Assert.Equal(t, t.DowncastClone(I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I, I));
      Assert.NotEqual(0, t.GetHashCode());
      Assert.Equal("(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19)", t.ToString());
      Assert.Equal(t, _System.Tuple20<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.Default(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19));
      Assert.Equal(t, _System.Tuple20<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create____hMake20(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19));
      Assert.Equal(_System.Tuple20<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.create(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
          _System.Tuple20<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>._TypeDescriptor(D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D, D).Default());
      Assert.Equal(0, t.dtor__0);
      Assert.Equal(1, t.dtor__1);
      Assert.Equal(2, t.dtor__2);
      Assert.Equal(3, t.dtor__3);
      Assert.Equal(4, t.dtor__4);
      Assert.Equal(5, t.dtor__5);
      Assert.Equal(6, t.dtor__6);
      Assert.Equal(7, t.dtor__7);
      Assert.Equal(8, t.dtor__8);
      Assert.Equal(9, t.dtor__9);
      Assert.Equal(10, t.dtor__10);
      Assert.Equal(11, t.dtor__11);
      Assert.Equal(12, t.dtor__12);
      Assert.Equal(13, t.dtor__13);
      Assert.Equal(14, t.dtor__14);
      Assert.Equal(15, t.dtor__15);
      Assert.Equal(16, t.dtor__16);
      Assert.Equal(17, t.dtor__17);
      Assert.Equal(18, t.dtor__18);
      Assert.Equal(19, t.dtor__19);
    }

  }
}
