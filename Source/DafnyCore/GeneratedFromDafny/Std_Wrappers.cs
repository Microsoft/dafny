// Dafny program the_program compiled into C#
// To recompile, you will need the libraries
//     System.Runtime.Numerics.dll System.Collections.Immutable.dll
// but the 'dotnet' tool in net6.0 should pick those up automatically.
// Optionally, you may want to include compiler switches like
//     /debug /nowarn:162,164,168,183,219,436,1717,1718

using System;
using System.Numerics;
using System.Collections;

namespace Std.Wrappers {

  public partial class __default {
    public static Std.Wrappers._IOutcomeResult<__E> Need<__E>(bool condition, __E error)
    {
      if (condition) {
        return Std.Wrappers.OutcomeResult<__E>.create_Pass_k();
      } else {
        return Std.Wrappers.OutcomeResult<__E>.create_Fail_k(error);
      }
    }
  }

  public interface _IOption<out T> {
    bool is_None { get; }
    bool is_Some { get; }
    T dtor_value { get; }
    _IOption<__T> DowncastClone<__T>(Func<T, __T> converter0);
    bool IsFailure();
    Std.Wrappers._IOption<__U> PropagateFailure<__U>();
    T Extract();
    Std.Wrappers._IResult<T, __E> ToResult<__E>(__E error);
    Std.Wrappers._IOutcome<__E> ToOutcome<__E>(__E error);
  }
  public abstract class Option<T> : _IOption<T> {
    public Option() {
    }
    public static Std.Wrappers._IOption<T> Default() {
      return create_None();
    }
    public static Dafny.TypeDescriptor<Std.Wrappers._IOption<T>> _TypeDescriptor() {
      return new Dafny.TypeDescriptor<Std.Wrappers._IOption<T>>(Std.Wrappers.Option<T>.Default());
    }
    public static _IOption<T> create_None() {
      return new Option_None<T>();
    }
    public static _IOption<T> create_Some(T @value) {
      return new Option_Some<T>(@value);
    }
    public bool is_None { get { return this is Option_None<T>; } }
    public bool is_Some { get { return this is Option_Some<T>; } }
    public T dtor_value {
      get {
        var d = this;
        return ((Option_Some<T>)d)._value;
      }
    }
    public abstract _IOption<__T> DowncastClone<__T>(Func<T, __T> converter0);
    public bool IsFailure() {
      return (this).is_None;
    }
    public Std.Wrappers._IOption<__U> PropagateFailure<__U>() {
      return Std.Wrappers.Option<__U>.create_None();
    }
    public T Extract() {
      return (this).dtor_value;
    }
    public static T GetOr(Std.Wrappers._IOption<T> _this, T @default) {
      Std.Wrappers._IOption<T> _source0 = _this;
      if (_source0.is_None) {
        return @default;
      } else {
        T __mcc_h0 = _source0.dtor_value;
        T v = __mcc_h0;
        return v;
      }
    }
    public Std.Wrappers._IResult<T, __E> ToResult<__E>(__E error) {
      Std.Wrappers._IOption<T> _source1 = this;
      if (_source1.is_None) {
        return Std.Wrappers.Result<T, __E>.create_Failure(error);
      } else {
        T __mcc_h0 = _source1.dtor_value;
        T v = __mcc_h0;
        return Std.Wrappers.Result<T, __E>.create_Success(v);
      }
    }
    public Std.Wrappers._IOutcome<__E> ToOutcome<__E>(__E error) {
      Std.Wrappers._IOption<T> _source2 = this;
      if (_source2.is_None) {
        return Std.Wrappers.Outcome<__E>.create_Fail(error);
      } else {
        T __mcc_h0 = _source2.dtor_value;
        T v = __mcc_h0;
        return Std.Wrappers.Outcome<__E>.create_Pass();
      }
    }
    public static __FC Map<__FC>(Std.Wrappers._IOption<T> _this, Func<Std.Wrappers._IOption<T>, __FC> rewrap) {
      return Dafny.Helpers.Id<Func<Std.Wrappers._IOption<T>, __FC>>(rewrap)(_this);
    }
  }
  public class Option_None<T> : Option<T> {
    public Option_None() : base() {
    }
    public override _IOption<__T> DowncastClone<__T>(Func<T, __T> converter0) {
      if (this is _IOption<__T> dt) { return dt; }
      return new Option_None<__T>();
    }
    public override bool Equals(object other) {
      var oth = other as Std.Wrappers.Option_None<T>;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "Wrappers.Option.None";
      return s;
    }
  }
  public class Option_Some<T> : Option<T> {
    public readonly T _value;
    public Option_Some(T @value) : base() {
      this._value = @value;
    }
    public override _IOption<__T> DowncastClone<__T>(Func<T, __T> converter0) {
      if (this is _IOption<__T> dt) { return dt; }
      return new Option_Some<__T>(converter0(_value));
    }
    public override bool Equals(object other) {
      var oth = other as Std.Wrappers.Option_Some<T>;
      return oth != null && object.Equals(this._value, oth._value);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._value));
      return (int) hash;
    }
    public override string ToString() {
      string s = "Wrappers.Option.Some";
      s += "(";
      s += Dafny.Helpers.ToString(this._value);
      s += ")";
      return s;
    }
  }

  public interface _IResult<out R, out E> {
    bool is_Success { get; }
    bool is_Failure { get; }
    R dtor_value { get; }
    E dtor_error { get; }
    _IResult<__R, __E> DowncastClone<__R, __E>(Func<R, __R> converter0, Func<E, __E> converter1);
    bool IsFailure();
    Std.Wrappers._IResult<__U, E> PropagateFailure<__U>();
    R Extract();
    Std.Wrappers._IOption<R> ToOption();
    Std.Wrappers._IOutcome<E> ToOutcome();
  }
  public abstract class Result<R, E> : _IResult<R, E> {
    public Result() {
    }
    public static Std.Wrappers._IResult<R, E> Default(R _default_R) {
      return create_Success(_default_R);
    }
    public static Dafny.TypeDescriptor<Std.Wrappers._IResult<R, E>> _TypeDescriptor(Dafny.TypeDescriptor<R> _td_R) {
      return new Dafny.TypeDescriptor<Std.Wrappers._IResult<R, E>>(Std.Wrappers.Result<R, E>.Default(_td_R.Default()));
    }
    public static _IResult<R, E> create_Success(R @value) {
      return new Result_Success<R, E>(@value);
    }
    public static _IResult<R, E> create_Failure(E error) {
      return new Result_Failure<R, E>(error);
    }
    public bool is_Success { get { return this is Result_Success<R, E>; } }
    public bool is_Failure { get { return this is Result_Failure<R, E>; } }
    public R dtor_value {
      get {
        var d = this;
        return ((Result_Success<R, E>)d)._value;
      }
    }
    public E dtor_error {
      get {
        var d = this;
        return ((Result_Failure<R, E>)d)._error;
      }
    }
    public abstract _IResult<__R, __E> DowncastClone<__R, __E>(Func<R, __R> converter0, Func<E, __E> converter1);
    public bool IsFailure() {
      return (this).is_Failure;
    }
    public Std.Wrappers._IResult<__U, E> PropagateFailure<__U>() {
      return Std.Wrappers.Result<__U, E>.create_Failure((this).dtor_error);
    }
    public R Extract() {
      return (this).dtor_value;
    }
    public static R GetOr(Std.Wrappers._IResult<R, E> _this, R @default) {
      Std.Wrappers._IResult<R, E> _source3 = _this;
      if (_source3.is_Success) {
        R _10___mcc_h0 = _source3.dtor_value;
        R _11_s = _10___mcc_h0;
        return _11_s;
      } else {
        E _12___mcc_h1 = _source3.dtor_error;
        E _13_e = _12___mcc_h1;
        return @default;
      }
    }
    public Std.Wrappers._IOption<R> ToOption() {
      Std.Wrappers._IResult<R, E> _source4 = this;
      if (_source4.is_Success) {
        R _14___mcc_h0 = _source4.dtor_value;
        R _15_s = _14___mcc_h0;
        return Std.Wrappers.Option<R>.create_Some(_15_s);
      } else {
        E _16___mcc_h1 = _source4.dtor_error;
        E _17_e = _16___mcc_h1;
        return Std.Wrappers.Option<R>.create_None();
      }
    }
    public Std.Wrappers._IOutcome<E> ToOutcome() {
      Std.Wrappers._IResult<R, E> _source5 = this;
      if (_source5.is_Success) {
        R _18___mcc_h0 = _source5.dtor_value;
        R _19_s = _18___mcc_h0;
        return Std.Wrappers.Outcome<E>.create_Pass();
      } else {
        E _20___mcc_h1 = _source5.dtor_error;
        E _21_e = _20___mcc_h1;
        return Std.Wrappers.Outcome<E>.create_Fail(_21_e);
      }
    }
    public static __FC Map<__FC>(Std.Wrappers._IResult<R, E> _this, Func<Std.Wrappers._IResult<R, E>, __FC> rewrap) {
      return Dafny.Helpers.Id<Func<Std.Wrappers._IResult<R, E>, __FC>>(rewrap)(_this);
    }
    public static Std.Wrappers._IResult<R, __NewE> MapFailure<__NewE>(Std.Wrappers._IResult<R, E> _this, Func<E, __NewE> reWrap) {
      Std.Wrappers._IResult<R, E> _source6 = _this;
      if (_source6.is_Success) {
        R _22___mcc_h0 = _source6.dtor_value;
        R _23_s = _22___mcc_h0;
        return Std.Wrappers.Result<R, __NewE>.create_Success(_23_s);
      } else {
        E _24___mcc_h1 = _source6.dtor_error;
        E _25_e = _24___mcc_h1;
        return Std.Wrappers.Result<R, __NewE>.create_Failure(Dafny.Helpers.Id<Func<E, __NewE>>(reWrap)(_25_e));
      }
    }
  }
  public class Result_Success<R, E> : Result<R, E> {
    public readonly R _value;
    public Result_Success(R @value) : base() {
      this._value = @value;
    }
    public override _IResult<__R, __E> DowncastClone<__R, __E>(Func<R, __R> converter0, Func<E, __E> converter1) {
      if (this is _IResult<__R, __E> dt) { return dt; }
      return new Result_Success<__R, __E>(converter0(_value));
    }
    public override bool Equals(object other) {
      var oth = other as Std.Wrappers.Result_Success<R, E>;
      return oth != null && object.Equals(this._value, oth._value);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._value));
      return (int) hash;
    }
    public override string ToString() {
      string s = "Wrappers.Result.Success";
      s += "(";
      s += Dafny.Helpers.ToString(this._value);
      s += ")";
      return s;
    }
  }
  public class Result_Failure<R, E> : Result<R, E> {
    public readonly E _error;
    public Result_Failure(E error) : base() {
      this._error = error;
    }
    public override _IResult<__R, __E> DowncastClone<__R, __E>(Func<R, __R> converter0, Func<E, __E> converter1) {
      if (this is _IResult<__R, __E> dt) { return dt; }
      return new Result_Failure<__R, __E>(converter1(_error));
    }
    public override bool Equals(object other) {
      var oth = other as Std.Wrappers.Result_Failure<R, E>;
      return oth != null && object.Equals(this._error, oth._error);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._error));
      return (int) hash;
    }
    public override string ToString() {
      string s = "Wrappers.Result.Failure";
      s += "(";
      s += Dafny.Helpers.ToString(this._error);
      s += ")";
      return s;
    }
  }

  public interface _IOutcome<out E> {
    bool is_Pass { get; }
    bool is_Fail { get; }
    E dtor_error { get; }
    _IOutcome<__E> DowncastClone<__E>(Func<E, __E> converter0);
    bool IsFailure();
    Std.Wrappers._IOutcome<E> PropagateFailure();
    Std.Wrappers._IOption<__R> ToOption<__R>(__R r);
    Std.Wrappers._IResult<__R, E> ToResult<__R>(__R r);
  }
  public abstract class Outcome<E> : _IOutcome<E> {
    public Outcome() {
    }
    public static Std.Wrappers._IOutcome<E> Default() {
      return create_Pass();
    }
    public static Dafny.TypeDescriptor<Std.Wrappers._IOutcome<E>> _TypeDescriptor() {
      return new Dafny.TypeDescriptor<Std.Wrappers._IOutcome<E>>(Std.Wrappers.Outcome<E>.Default());
    }
    public static _IOutcome<E> create_Pass() {
      return new Outcome_Pass<E>();
    }
    public static _IOutcome<E> create_Fail(E error) {
      return new Outcome_Fail<E>(error);
    }
    public bool is_Pass { get { return this is Outcome_Pass<E>; } }
    public bool is_Fail { get { return this is Outcome_Fail<E>; } }
    public E dtor_error {
      get {
        var d = this;
        return ((Outcome_Fail<E>)d)._error;
      }
    }
    public abstract _IOutcome<__E> DowncastClone<__E>(Func<E, __E> converter0);
    public bool IsFailure() {
      return (this).is_Fail;
    }
    public Std.Wrappers._IOutcome<E> PropagateFailure() {
      return this;
    }
    public Std.Wrappers._IOption<__R> ToOption<__R>(__R r) {
      Std.Wrappers._IOutcome<E> _source7 = this;
      if (_source7.is_Pass) {
        return Std.Wrappers.Option<__R>.create_Some(r);
      } else {
        E _26___mcc_h0 = _source7.dtor_error;
        E _27_e = _26___mcc_h0;
        return Std.Wrappers.Option<__R>.create_None();
      }
    }
    public Std.Wrappers._IResult<__R, E> ToResult<__R>(__R r) {
      Std.Wrappers._IOutcome<E> _source8 = this;
      if (_source8.is_Pass) {
        return Std.Wrappers.Result<__R, E>.create_Success(r);
      } else {
        E _28___mcc_h0 = _source8.dtor_error;
        E _29_e = _28___mcc_h0;
        return Std.Wrappers.Result<__R, E>.create_Failure(_29_e);
      }
    }
    public static __FC Map<__FC>(Std.Wrappers._IOutcome<E> _this, Func<Std.Wrappers._IOutcome<E>, __FC> rewrap) {
      return Dafny.Helpers.Id<Func<Std.Wrappers._IOutcome<E>, __FC>>(rewrap)(_this);
    }
    public static Std.Wrappers._IResult<__T, __NewE> MapFailure<__T, __NewE>(Std.Wrappers._IOutcome<E> _this, Func<E, __NewE> rewrap, __T @default)
    {
      Std.Wrappers._IOutcome<E> _source9 = _this;
      if (_source9.is_Pass) {
        return Std.Wrappers.Result<__T, __NewE>.create_Success(@default);
      } else {
        E _30___mcc_h0 = _source9.dtor_error;
        E _31_e = _30___mcc_h0;
        return Std.Wrappers.Result<__T, __NewE>.create_Failure(Dafny.Helpers.Id<Func<E, __NewE>>(rewrap)(_31_e));
      }
    }
    public static Std.Wrappers._IOutcome<E> Need(bool condition, E error)
    {
      if (condition) {
        return Std.Wrappers.Outcome<E>.create_Pass();
      } else {
        return Std.Wrappers.Outcome<E>.create_Fail(error);
      }
    }
  }
  public class Outcome_Pass<E> : Outcome<E> {
    public Outcome_Pass() : base() {
    }
    public override _IOutcome<__E> DowncastClone<__E>(Func<E, __E> converter0) {
      if (this is _IOutcome<__E> dt) { return dt; }
      return new Outcome_Pass<__E>();
    }
    public override bool Equals(object other) {
      var oth = other as Std.Wrappers.Outcome_Pass<E>;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "Wrappers.Outcome.Pass";
      return s;
    }
  }
  public class Outcome_Fail<E> : Outcome<E> {
    public readonly E _error;
    public Outcome_Fail(E error) : base() {
      this._error = error;
    }
    public override _IOutcome<__E> DowncastClone<__E>(Func<E, __E> converter0) {
      if (this is _IOutcome<__E> dt) { return dt; }
      return new Outcome_Fail<__E>(converter0(_error));
    }
    public override bool Equals(object other) {
      var oth = other as Std.Wrappers.Outcome_Fail<E>;
      return oth != null && object.Equals(this._error, oth._error);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._error));
      return (int) hash;
    }
    public override string ToString() {
      string s = "Wrappers.Outcome.Fail";
      s += "(";
      s += Dafny.Helpers.ToString(this._error);
      s += ")";
      return s;
    }
  }

  public interface _IOutcomeResult<out E> {
    bool is_Pass_k { get; }
    bool is_Fail_k { get; }
    E dtor_error { get; }
    _IOutcomeResult<__E> DowncastClone<__E>(Func<E, __E> converter0);
    bool IsFailure();
    Std.Wrappers._IResult<__U, E> PropagateFailure<__U>();
  }
  public abstract class OutcomeResult<E> : _IOutcomeResult<E> {
    public OutcomeResult() {
    }
    public static Std.Wrappers._IOutcomeResult<E> Default() {
      return create_Pass_k();
    }
    public static Dafny.TypeDescriptor<Std.Wrappers._IOutcomeResult<E>> _TypeDescriptor() {
      return new Dafny.TypeDescriptor<Std.Wrappers._IOutcomeResult<E>>(Std.Wrappers.OutcomeResult<E>.Default());
    }
    public static _IOutcomeResult<E> create_Pass_k() {
      return new OutcomeResult_Pass_k<E>();
    }
    public static _IOutcomeResult<E> create_Fail_k(E error) {
      return new OutcomeResult_Fail_k<E>(error);
    }
    public bool is_Pass_k { get { return this is OutcomeResult_Pass_k<E>; } }
    public bool is_Fail_k { get { return this is OutcomeResult_Fail_k<E>; } }
    public E dtor_error {
      get {
        var d = this;
        return ((OutcomeResult_Fail_k<E>)d)._error;
      }
    }
    public abstract _IOutcomeResult<__E> DowncastClone<__E>(Func<E, __E> converter0);
    public bool IsFailure() {
      return (this).is_Fail_k;
    }
    public Std.Wrappers._IResult<__U, E> PropagateFailure<__U>() {
      return Std.Wrappers.Result<__U, E>.create_Failure((this).dtor_error);
    }
  }
  public class OutcomeResult_Pass_k<E> : OutcomeResult<E> {
    public OutcomeResult_Pass_k() : base() {
    }
    public override _IOutcomeResult<__E> DowncastClone<__E>(Func<E, __E> converter0) {
      if (this is _IOutcomeResult<__E> dt) { return dt; }
      return new OutcomeResult_Pass_k<__E>();
    }
    public override bool Equals(object other) {
      var oth = other as Std.Wrappers.OutcomeResult_Pass_k<E>;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "Wrappers.OutcomeResult.Pass'";
      return s;
    }
  }
  public class OutcomeResult_Fail_k<E> : OutcomeResult<E> {
    public readonly E _error;
    public OutcomeResult_Fail_k(E error) : base() {
      this._error = error;
    }
    public override _IOutcomeResult<__E> DowncastClone<__E>(Func<E, __E> converter0) {
      if (this is _IOutcomeResult<__E> dt) { return dt; }
      return new OutcomeResult_Fail_k<__E>(converter0(_error));
    }
    public override bool Equals(object other) {
      var oth = other as Std.Wrappers.OutcomeResult_Fail_k<E>;
      return oth != null && object.Equals(this._error, oth._error);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._error));
      return (int) hash;
    }
    public override string ToString() {
      string s = "Wrappers.OutcomeResult.Fail'";
      s += "(";
      s += Dafny.Helpers.ToString(this._error);
      s += ")";
      return s;
    }
  }
} // end of namespace Std.Wrappers