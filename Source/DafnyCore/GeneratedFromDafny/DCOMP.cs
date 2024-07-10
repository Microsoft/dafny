// Dafny program the_program compiled into C#
// To recompile, you will need the libraries
//     System.Runtime.Numerics.dll System.Collections.Immutable.dll
// but the 'dotnet' tool in net6.0 should pick those up automatically.
// Optionally, you may want to include compiler switches like
//     /debug /nowarn:162,164,168,183,219,436,1717,1718

using System;
using System.Numerics;
using System.Collections;

namespace DCOMP {

  public partial class __default {
    public static bool is__tuple__numeric(Dafny.ISequence<Dafny.Rune> i) {
      return ((((new BigInteger((i).Count)) >= (new BigInteger(2))) && (((i).Select(BigInteger.Zero)) == (new Dafny.Rune('_')))) && ((Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0123456789")).Contains((i).Select(BigInteger.One)))) && (((new BigInteger((i).Count)) == (new BigInteger(2))) || (((new BigInteger((i).Count)) == (new BigInteger(3))) && ((Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0123456789")).Contains((i).Select(new BigInteger(2))))));
    }
    public static bool has__special(Dafny.ISequence<Dafny.Rune> i) {
    TAIL_CALL_START: ;
      if ((new BigInteger((i).Count)).Sign == 0) {
        return false;
      } else if (((i).Select(BigInteger.Zero)) == (new Dafny.Rune('.'))) {
        return true;
      } else if (((i).Select(BigInteger.Zero)) == (new Dafny.Rune('#'))) {
        return true;
      } else if (((i).Select(BigInteger.Zero)) == (new Dafny.Rune('_'))) {
        if ((new BigInteger(2)) <= (new BigInteger((i).Count))) {
          if (((i).Select(BigInteger.One)) != (new Dafny.Rune('_'))) {
            return true;
          } else {
            Dafny.ISequence<Dafny.Rune> _in135 = (i).Drop(new BigInteger(2));
            i = _in135;
            goto TAIL_CALL_START;
          }
        } else {
          return true;
        }
      } else {
        Dafny.ISequence<Dafny.Rune> _in136 = (i).Drop(BigInteger.One);
        i = _in136;
        goto TAIL_CALL_START;
      }
    }
    public static Dafny.ISequence<Dafny.Rune> idiomatic__rust(Dafny.ISequence<Dafny.Rune> i) {
      Dafny.ISequence<Dafny.Rune> _1299___accumulator = Dafny.Sequence<Dafny.Rune>.FromElements();
    TAIL_CALL_START: ;
      if ((new BigInteger((i).Count)).Sign == 0) {
        return Dafny.Sequence<Dafny.Rune>.Concat(_1299___accumulator, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
      } else if (((i).Select(BigInteger.Zero)) == (new Dafny.Rune('_'))) {
        _1299___accumulator = Dafny.Sequence<Dafny.Rune>.Concat(_1299___accumulator, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"));
        Dafny.ISequence<Dafny.Rune> _in137 = (i).Drop(new BigInteger(2));
        i = _in137;
        goto TAIL_CALL_START;
      } else {
        _1299___accumulator = Dafny.Sequence<Dafny.Rune>.Concat(_1299___accumulator, Dafny.Sequence<Dafny.Rune>.FromElements((i).Select(BigInteger.Zero)));
        Dafny.ISequence<Dafny.Rune> _in138 = (i).Drop(BigInteger.One);
        i = _in138;
        goto TAIL_CALL_START;
      }
    }
    public static Dafny.ISequence<Dafny.Rune> replaceDots(Dafny.ISequence<Dafny.Rune> i) {
      Dafny.ISequence<Dafny.Rune> _1300___accumulator = Dafny.Sequence<Dafny.Rune>.FromElements();
    TAIL_CALL_START: ;
      if ((new BigInteger((i).Count)).Sign == 0) {
        return Dafny.Sequence<Dafny.Rune>.Concat(_1300___accumulator, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
      } else if (((i).Select(BigInteger.Zero)) == (new Dafny.Rune('.'))) {
        _1300___accumulator = Dafny.Sequence<Dafny.Rune>.Concat(_1300___accumulator, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_d"));
        Dafny.ISequence<Dafny.Rune> _in139 = (i).Drop(BigInteger.One);
        i = _in139;
        goto TAIL_CALL_START;
      } else {
        _1300___accumulator = Dafny.Sequence<Dafny.Rune>.Concat(_1300___accumulator, Dafny.Sequence<Dafny.Rune>.FromElements((i).Select(BigInteger.Zero)));
        Dafny.ISequence<Dafny.Rune> _in140 = (i).Drop(BigInteger.One);
        i = _in140;
        goto TAIL_CALL_START;
      }
    }
    public static bool is__tuple__builder(Dafny.ISequence<Dafny.Rune> i) {
      return ((((new BigInteger((i).Count)) >= (new BigInteger(9))) && (((i).Take(new BigInteger(8))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("___hMake")))) && ((Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0123456789")).Contains((i).Select(new BigInteger(8))))) && (((new BigInteger((i).Count)) == (new BigInteger(9))) || (((new BigInteger((i).Count)) == (new BigInteger(10))) && ((Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0123456789")).Contains((i).Select(new BigInteger(9))))));
    }
    public static Dafny.ISequence<Dafny.Rune> better__tuple__builder__name(Dafny.ISequence<Dafny.Rune> i) {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_T"), (i).Drop(new BigInteger(8)));
    }
    public static bool is__dafny__generated__id(Dafny.ISequence<Dafny.Rune> i) {
      return ((((new BigInteger((i).Count)).Sign == 1) && (((i).Select(BigInteger.Zero)) == (new Dafny.Rune('_')))) && (!(DCOMP.__default.has__special((i).Drop(BigInteger.One))))) && (!((new BigInteger((i).Count)) >= (new BigInteger(2))) || (((i).Select(BigInteger.One)) != (new Dafny.Rune('T'))));
    }
    public static bool is__idiomatic__rust__id(Dafny.ISequence<Dafny.Rune> i) {
      return ((((new BigInteger((i).Count)).Sign == 1) && (!(DCOMP.__default.has__special(i)))) && (!(DCOMP.__default.reserved__rust).Contains(i))) && (!(DCOMP.__default.reserved__rust__need__prefix).Contains(i));
    }
    public static Dafny.ISequence<Dafny.Rune> escapeName(Dafny.ISequence<Dafny.Rune> n) {
      return DCOMP.__default.escapeIdent((n));
    }
    public static Dafny.ISequence<Dafny.Rune> escapeIdent(Dafny.ISequence<Dafny.Rune> i) {
      if (DCOMP.__default.is__tuple__numeric(i)) {
        return i;
      } else if (DCOMP.__default.is__tuple__builder(i)) {
        return DCOMP.__default.better__tuple__builder__name(i);
      } else if (((i).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))) || ((i).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Self")))) {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("r#_"), i);
      } else if ((DCOMP.__default.reserved__rust).Contains(i)) {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("r#"), i);
      } else if (DCOMP.__default.is__idiomatic__rust__id(i)) {
        return DCOMP.__default.idiomatic__rust(i);
      } else if (DCOMP.__default.is__dafny__generated__id(i)) {
        return i;
      } else {
        Dafny.ISequence<Dafny.Rune> _1301_r = DCOMP.__default.replaceDots(i);
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("r#_"), _1301_r);
      }
    }
    public static Dafny.ISequence<Dafny.Rune> escapeVar(Dafny.ISequence<Dafny.Rune> f) {
      Dafny.ISequence<Dafny.Rune> _1302_r = (f);
      if ((DCOMP.__default.reserved__vars).Contains(_1302_r)) {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"), _1302_r);
      } else {
        return DCOMP.__default.escapeIdent((f));
      }
    }
    public static Dafny.ISequence<Dafny.Rune> AddAssignedPrefix(Dafny.ISequence<Dafny.Rune> rustName) {
      if (((new BigInteger((rustName).Count)) >= (new BigInteger(2))) && (((rustName).Subsequence(BigInteger.Zero, new BigInteger(2))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("r#")))) {
        return Dafny.Sequence<Dafny.Rune>.Concat(DCOMP.__default.ASSIGNED__PREFIX, (rustName).Drop(new BigInteger(2)));
      } else {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(DCOMP.__default.ASSIGNED__PREFIX, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_")), rustName);
      }
    }
    public static Std.Wrappers._IOption<DAST._IResolvedType> TraitTypeContainingMethodAux(Dafny.ISequence<DAST._IType> rs, Dafny.ISequence<Dafny.Rune> dafnyName)
    {
      if ((new BigInteger((rs).Count)).Sign == 0) {
        return Std.Wrappers.Option<DAST._IResolvedType>.create_None();
      } else {
        Std.Wrappers._IOption<DAST._IResolvedType> _1303_res = ((System.Func<Std.Wrappers._IOption<DAST._IResolvedType>>)(() => {
          DAST._IType _source66 = (rs).Select(BigInteger.Zero);
          {
            if (_source66.is_UserDefined) {
              DAST._IResolvedType _1304_resolvedType = _source66.dtor_resolved;
              return DCOMP.__default.TraitTypeContainingMethod(_1304_resolvedType, dafnyName);
            }
          }
          {
            return Std.Wrappers.Option<DAST._IResolvedType>.create_None();
          }
        }))();
        Std.Wrappers._IOption<DAST._IResolvedType> _source67 = _1303_res;
        {
          if (_source67.is_Some) {
            return _1303_res;
          }
        }
        {
          return DCOMP.__default.TraitTypeContainingMethodAux((rs).Drop(BigInteger.One), dafnyName);
        }
      }
    }
    public static Std.Wrappers._IOption<DAST._IResolvedType> TraitTypeContainingMethod(DAST._IResolvedType r, Dafny.ISequence<Dafny.Rune> dafnyName)
    {
      DAST._IResolvedType _let_tmp_rhs53 = r;
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1305_path = _let_tmp_rhs53.dtor_path;
      Dafny.ISequence<DAST._IType> _1306_typeArgs = _let_tmp_rhs53.dtor_typeArgs;
      DAST._IResolvedTypeBase _1307_kind = _let_tmp_rhs53.dtor_kind;
      Dafny.ISequence<DAST._IAttribute> _1308_attributes = _let_tmp_rhs53.dtor_attributes;
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1309_properMethods = _let_tmp_rhs53.dtor_properMethods;
      Dafny.ISequence<DAST._IType> _1310_extendedTypes = _let_tmp_rhs53.dtor_extendedTypes;
      if ((_1309_properMethods).Contains(dafnyName)) {
        return Std.Wrappers.Option<DAST._IResolvedType>.create_Some(r);
      } else {
        return DCOMP.__default.TraitTypeContainingMethodAux(_1310_extendedTypes, dafnyName);
      }
    }
    public static Std.Wrappers._IOption<DCOMP._IExternAttribute> OptExtern(DAST._IAttribute attr, Dafny.ISequence<Dafny.Rune> dafnyName)
    {
      if (((attr).dtor_name).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("extern"))) {
        return Std.Wrappers.Option<DCOMP._IExternAttribute>.create_Some((((new BigInteger(((attr).dtor_args).Count)).Sign == 0) ? (DCOMP.ExternAttribute.create_SimpleExtern(DCOMP.__default.escapeName(dafnyName))) : ((((new BigInteger(((attr).dtor_args).Count)) == (BigInteger.One)) ? (DCOMP.ExternAttribute.create_SimpleExtern(((attr).dtor_args).Select(BigInteger.Zero))) : ((((new BigInteger(((attr).dtor_args).Count)) == (new BigInteger(2))) ? (DCOMP.ExternAttribute.create_AdvancedExtern(DCOMP.__default.ReplaceDotByDoubleColon(((attr).dtor_args).Select(BigInteger.Zero)), ((attr).dtor_args).Select(BigInteger.One))) : (DCOMP.ExternAttribute.create_UnsupportedExtern(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{:extern} supports only 0, 1 or 2 attributes, got "), Std.Strings.__default.OfNat(new BigInteger(((attr).dtor_args).Count)))))))))));
      } else {
        return Std.Wrappers.Option<DCOMP._IExternAttribute>.create_None();
      }
    }
    public static Dafny.ISequence<Dafny.Rune> ReplaceDotByDoubleColon(Dafny.ISequence<Dafny.Rune> s) {
      Dafny.ISequence<Dafny.Rune> _1311___accumulator = Dafny.Sequence<Dafny.Rune>.FromElements();
    TAIL_CALL_START: ;
      if ((new BigInteger((s).Count)).Sign == 0) {
        return Dafny.Sequence<Dafny.Rune>.Concat(_1311___accumulator, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
      } else if (((s).Select(BigInteger.Zero)) == (new Dafny.Rune(' '))) {
        return Dafny.Sequence<Dafny.Rune>.Concat(_1311___accumulator, s);
      } else {
        _1311___accumulator = Dafny.Sequence<Dafny.Rune>.Concat(_1311___accumulator, ((((s).Select(BigInteger.Zero)) == (new Dafny.Rune('.'))) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")) : (Dafny.Sequence<Dafny.Rune>.FromElements((s).Select(BigInteger.Zero)))));
        Dafny.ISequence<Dafny.Rune> _in141 = (s).Drop(BigInteger.One);
        s = _in141;
        goto TAIL_CALL_START;
      }
    }
    public static DCOMP._IExternAttribute ExtractExtern(Dafny.ISequence<DAST._IAttribute> attributes, Dafny.ISequence<Dafny.Rune> dafnyName)
    {
      DCOMP._IExternAttribute res = DCOMP.ExternAttribute.Default();
      BigInteger _hi5 = new BigInteger((attributes).Count);
      for (BigInteger _1312_i = BigInteger.Zero; _1312_i < _hi5; _1312_i++) {
        Std.Wrappers._IOption<DCOMP._IExternAttribute> _1313_attr;
        _1313_attr = DCOMP.__default.OptExtern((attributes).Select(_1312_i), dafnyName);
        Std.Wrappers._IOption<DCOMP._IExternAttribute> _source68 = _1313_attr;
        {
          if (_source68.is_Some) {
            DCOMP._IExternAttribute _1314_n = _source68.dtor_value;
            res = _1314_n;
            return res;
            goto after_match1;
          }
        }
        {
        }
      after_match1: ;
      }
      res = DCOMP.ExternAttribute.create_NoExtern();
      return res;
    }
    public static DCOMP._IExternAttribute ExtractExternMod(DAST._IModule mod) {
      return DCOMP.__default.ExtractExtern((mod).dtor_attributes, (mod).dtor_name);
    }
    public static Dafny.ISet<Dafny.ISequence<Dafny.Rune>> reserved__rust { get {
      return Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("async"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("await"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("break"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("const"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("continue"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("crate"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dyn"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("else"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("enum"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("extern"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("false"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fn"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("for"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("if"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("impl"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("in"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("let"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("loop"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("match"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("mod"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("move"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("mut"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("pub"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("ref"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("return"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("static"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("struct"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("super"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("trait"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("true"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("type"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("union"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unsafe"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("use"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("where"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("while"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Keywords"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("The"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abstract"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("become"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("box"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("do"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("final"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("macro"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("override"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("priv"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("try"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("typeof"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unsized"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("virtual"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("yield"));
    } }
    public static Dafny.ISet<Dafny.ISequence<Dafny.Rune>> reserved__rust__need__prefix { get {
      return Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u8"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u16"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u32"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u64"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u128"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i8"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i16"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i32"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i64"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i128"));
    } }
    public static Dafny.ISet<Dafny.ISequence<Dafny.Rune>> reserved__vars { get {
      return Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("None"));
    } }
    public static Dafny.ISequence<Dafny.Rune> ASSIGNED__PREFIX { get {
      return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_set");
    } }
    public static Dafny.ISequence<Dafny.Rune> IND { get {
      return RAST.__default.IND;
    } }
    public static DAST._IAttribute AttributeOwned { get {
      return DAST.Attribute.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("owned"), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements());
    } }
  }

  public interface _IOwnership {
    bool is_OwnershipOwned { get; }
    bool is_OwnershipOwnedBox { get; }
    bool is_OwnershipBorrowed { get; }
    bool is_OwnershipBorrowedMut { get; }
    bool is_OwnershipAutoBorrowed { get; }
    _IOwnership DowncastClone();
  }
  public abstract class Ownership : _IOwnership {
    public Ownership() {
    }
    private static readonly DCOMP._IOwnership theDefault = create_OwnershipOwned();
    public static DCOMP._IOwnership Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<DCOMP._IOwnership> _TYPE = new Dafny.TypeDescriptor<DCOMP._IOwnership>(DCOMP.Ownership.Default());
    public static Dafny.TypeDescriptor<DCOMP._IOwnership> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IOwnership create_OwnershipOwned() {
      return new Ownership_OwnershipOwned();
    }
    public static _IOwnership create_OwnershipOwnedBox() {
      return new Ownership_OwnershipOwnedBox();
    }
    public static _IOwnership create_OwnershipBorrowed() {
      return new Ownership_OwnershipBorrowed();
    }
    public static _IOwnership create_OwnershipBorrowedMut() {
      return new Ownership_OwnershipBorrowedMut();
    }
    public static _IOwnership create_OwnershipAutoBorrowed() {
      return new Ownership_OwnershipAutoBorrowed();
    }
    public bool is_OwnershipOwned { get { return this is Ownership_OwnershipOwned; } }
    public bool is_OwnershipOwnedBox { get { return this is Ownership_OwnershipOwnedBox; } }
    public bool is_OwnershipBorrowed { get { return this is Ownership_OwnershipBorrowed; } }
    public bool is_OwnershipBorrowedMut { get { return this is Ownership_OwnershipBorrowedMut; } }
    public bool is_OwnershipAutoBorrowed { get { return this is Ownership_OwnershipAutoBorrowed; } }
    public static System.Collections.Generic.IEnumerable<_IOwnership> AllSingletonConstructors {
      get {
        yield return Ownership.create_OwnershipOwned();
        yield return Ownership.create_OwnershipOwnedBox();
        yield return Ownership.create_OwnershipBorrowed();
        yield return Ownership.create_OwnershipBorrowedMut();
        yield return Ownership.create_OwnershipAutoBorrowed();
      }
    }
    public abstract _IOwnership DowncastClone();
  }
  public class Ownership_OwnershipOwned : Ownership {
    public Ownership_OwnershipOwned() : base() {
    }
    public override _IOwnership DowncastClone() {
      if (this is _IOwnership dt) { return dt; }
      return new Ownership_OwnershipOwned();
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.Ownership_OwnershipOwned;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.Ownership.OwnershipOwned";
      return s;
    }
  }
  public class Ownership_OwnershipOwnedBox : Ownership {
    public Ownership_OwnershipOwnedBox() : base() {
    }
    public override _IOwnership DowncastClone() {
      if (this is _IOwnership dt) { return dt; }
      return new Ownership_OwnershipOwnedBox();
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.Ownership_OwnershipOwnedBox;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.Ownership.OwnershipOwnedBox";
      return s;
    }
  }
  public class Ownership_OwnershipBorrowed : Ownership {
    public Ownership_OwnershipBorrowed() : base() {
    }
    public override _IOwnership DowncastClone() {
      if (this is _IOwnership dt) { return dt; }
      return new Ownership_OwnershipBorrowed();
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.Ownership_OwnershipBorrowed;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 2;
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.Ownership.OwnershipBorrowed";
      return s;
    }
  }
  public class Ownership_OwnershipBorrowedMut : Ownership {
    public Ownership_OwnershipBorrowedMut() : base() {
    }
    public override _IOwnership DowncastClone() {
      if (this is _IOwnership dt) { return dt; }
      return new Ownership_OwnershipBorrowedMut();
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.Ownership_OwnershipBorrowedMut;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 3;
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.Ownership.OwnershipBorrowedMut";
      return s;
    }
  }
  public class Ownership_OwnershipAutoBorrowed : Ownership {
    public Ownership_OwnershipAutoBorrowed() : base() {
    }
    public override _IOwnership DowncastClone() {
      if (this is _IOwnership dt) { return dt; }
      return new Ownership_OwnershipAutoBorrowed();
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.Ownership_OwnershipAutoBorrowed;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 4;
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.Ownership.OwnershipAutoBorrowed";
      return s;
    }
  }

  public interface _IEnvironment {
    bool is_Environment { get; }
    Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_names { get; }
    Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> dtor_types { get; }
    _IEnvironment DowncastClone();
    DCOMP._IEnvironment ToOwned();
    bool CanReadWithoutClone(Dafny.ISequence<Dafny.Rune> name);
    bool HasCloneSemantics(Dafny.ISequence<Dafny.Rune> name);
    Std.Wrappers._IOption<RAST._IType> GetType(Dafny.ISequence<Dafny.Rune> name);
    bool IsBorrowed(Dafny.ISequence<Dafny.Rune> name);
    bool IsBorrowedMut(Dafny.ISequence<Dafny.Rune> name);
    DCOMP._IEnvironment AddAssigned(Dafny.ISequence<Dafny.Rune> name, RAST._IType tpe);
    DCOMP._IEnvironment merge(DCOMP._IEnvironment other);
    DCOMP._IEnvironment RemoveAssigned(Dafny.ISequence<Dafny.Rune> name);
  }
  public class Environment : _IEnvironment {
    public readonly Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _names;
    public readonly Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> _types;
    public Environment(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> names, Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> types) {
      this._names = names;
      this._types = types;
    }
    public _IEnvironment DowncastClone() {
      if (this is _IEnvironment dt) { return dt; }
      return new Environment(_names, _types);
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.Environment;
      return oth != null && object.Equals(this._names, oth._names) && object.Equals(this._types, oth._types);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._names));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._types));
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.Environment.Environment";
      s += "(";
      s += Dafny.Helpers.ToString(this._names);
      s += ", ";
      s += Dafny.Helpers.ToString(this._types);
      s += ")";
      return s;
    }
    private static readonly DCOMP._IEnvironment theDefault = create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Empty, Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Empty);
    public static DCOMP._IEnvironment Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<DCOMP._IEnvironment> _TYPE = new Dafny.TypeDescriptor<DCOMP._IEnvironment>(DCOMP.Environment.Default());
    public static Dafny.TypeDescriptor<DCOMP._IEnvironment> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IEnvironment create(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> names, Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> types) {
      return new Environment(names, types);
    }
    public static _IEnvironment create_Environment(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> names, Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> types) {
      return create(names, types);
    }
    public bool is_Environment { get { return true; } }
    public Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_names {
      get {
        return this._names;
      }
    }
    public Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> dtor_types {
      get {
        return this._types;
      }
    }
    public DCOMP._IEnvironment ToOwned() {
      DCOMP._IEnvironment _1315_dt__update__tmp_h0 = this;
      Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> _1316_dt__update_htypes_h0 = ((System.Func<Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType>>)(() => {
        var _coll8 = new System.Collections.Generic.List<Dafny.Pair<Dafny.ISequence<Dafny.Rune>,RAST._IType>>();
        foreach (Dafny.ISequence<Dafny.Rune> _compr_9 in ((this).dtor_types).Keys.Elements) {
          Dafny.ISequence<Dafny.Rune> _1317_k = (Dafny.ISequence<Dafny.Rune>)_compr_9;
          if (((this).dtor_types).Contains(_1317_k)) {
            _coll8.Add(new Dafny.Pair<Dafny.ISequence<Dafny.Rune>,RAST._IType>(_1317_k, (Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Select((this).dtor_types,_1317_k)).ToOwned()));
          }
        }
        return Dafny.Map<Dafny.ISequence<Dafny.Rune>,RAST._IType>.FromCollection(_coll8);
      }))();
      return DCOMP.Environment.create((_1315_dt__update__tmp_h0).dtor_names, _1316_dt__update_htypes_h0);
    }
    public static DCOMP._IEnvironment Empty() {
      return DCOMP.Environment.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.FromElements());
    }
    public bool CanReadWithoutClone(Dafny.ISequence<Dafny.Rune> name) {
      return (((this).dtor_types).Contains(name)) && ((Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Select((this).dtor_types,name)).CanReadWithoutClone());
    }
    public bool HasCloneSemantics(Dafny.ISequence<Dafny.Rune> name) {
      return !((this).CanReadWithoutClone(name));
    }
    public Std.Wrappers._IOption<RAST._IType> GetType(Dafny.ISequence<Dafny.Rune> name) {
      if (((this).dtor_types).Contains(name)) {
        return Std.Wrappers.Option<RAST._IType>.create_Some(Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Select((this).dtor_types,name));
      } else {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      }
    }
    public bool IsBorrowed(Dafny.ISequence<Dafny.Rune> name) {
      return (((this).dtor_types).Contains(name)) && ((Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Select((this).dtor_types,name)).is_Borrowed);
    }
    public bool IsBorrowedMut(Dafny.ISequence<Dafny.Rune> name) {
      return (((this).dtor_types).Contains(name)) && ((Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Select((this).dtor_types,name)).is_BorrowedMut);
    }
    public DCOMP._IEnvironment AddAssigned(Dafny.ISequence<Dafny.Rune> name, RAST._IType tpe)
    {
      return DCOMP.Environment.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat((this).dtor_names, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(name)), Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update((this).dtor_types, name, tpe));
    }
    public DCOMP._IEnvironment merge(DCOMP._IEnvironment other) {
      return DCOMP.Environment.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat((this).dtor_names, (other).dtor_names), Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Merge((this).dtor_types, (other).dtor_types));
    }
    public DCOMP._IEnvironment RemoveAssigned(Dafny.ISequence<Dafny.Rune> name) {
      BigInteger _1318_indexInEnv = Std.Collections.Seq.__default.IndexOf<Dafny.ISequence<Dafny.Rune>>((this).dtor_names, name);
      return DCOMP.Environment.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(((this).dtor_names).Subsequence(BigInteger.Zero, _1318_indexInEnv), ((this).dtor_names).Drop((_1318_indexInEnv) + (BigInteger.One))), Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Subtract((this).dtor_types, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(name)));
    }
  }

  public interface _IObjectType {
    bool is_RawPointers { get; }
    bool is_RcMut { get; }
    _IObjectType DowncastClone();
  }
  public abstract class ObjectType : _IObjectType {
    public ObjectType() {
    }
    private static readonly DCOMP._IObjectType theDefault = create_RawPointers();
    public static DCOMP._IObjectType Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<DCOMP._IObjectType> _TYPE = new Dafny.TypeDescriptor<DCOMP._IObjectType>(DCOMP.ObjectType.Default());
    public static Dafny.TypeDescriptor<DCOMP._IObjectType> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IObjectType create_RawPointers() {
      return new ObjectType_RawPointers();
    }
    public static _IObjectType create_RcMut() {
      return new ObjectType_RcMut();
    }
    public bool is_RawPointers { get { return this is ObjectType_RawPointers; } }
    public bool is_RcMut { get { return this is ObjectType_RcMut; } }
    public static System.Collections.Generic.IEnumerable<_IObjectType> AllSingletonConstructors {
      get {
        yield return ObjectType.create_RawPointers();
        yield return ObjectType.create_RcMut();
      }
    }
    public abstract _IObjectType DowncastClone();
  }
  public class ObjectType_RawPointers : ObjectType {
    public ObjectType_RawPointers() : base() {
    }
    public override _IObjectType DowncastClone() {
      if (this is _IObjectType dt) { return dt; }
      return new ObjectType_RawPointers();
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.ObjectType_RawPointers;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.ObjectType.RawPointers";
      return s;
    }
  }
  public class ObjectType_RcMut : ObjectType {
    public ObjectType_RcMut() : base() {
    }
    public override _IObjectType DowncastClone() {
      if (this is _IObjectType dt) { return dt; }
      return new ObjectType_RcMut();
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.ObjectType_RcMut;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.ObjectType.RcMut";
      return s;
    }
  }

  public interface _IGenTypeContext {
    bool is_GenTypeContext { get; }
    bool dtor_forTraitParents { get; }
  }
  public class GenTypeContext : _IGenTypeContext {
    public readonly bool _forTraitParents;
    public GenTypeContext(bool forTraitParents) {
      this._forTraitParents = forTraitParents;
    }
    public static bool DowncastClone(bool _this) {
      return _this;
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.GenTypeContext;
      return oth != null && this._forTraitParents == oth._forTraitParents;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._forTraitParents));
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.GenTypeContext.GenTypeContext";
      s += "(";
      s += Dafny.Helpers.ToString(this._forTraitParents);
      s += ")";
      return s;
    }
    private static readonly bool theDefault = false;
    public static bool Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<bool> _TYPE = new Dafny.TypeDescriptor<bool>(false);
    public static Dafny.TypeDescriptor<bool> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IGenTypeContext create(bool forTraitParents) {
      return new GenTypeContext(forTraitParents);
    }
    public static _IGenTypeContext create_GenTypeContext(bool forTraitParents) {
      return create(forTraitParents);
    }
    public bool is_GenTypeContext { get { return true; } }
    public bool dtor_forTraitParents {
      get {
        return this._forTraitParents;
      }
    }
    public static bool ForTraitParents() {
      return true;
    }
    public static bool @default() {
      return false;
    }
  }

  public interface _ISelfInfo {
    bool is_NoSelf { get; }
    bool is_ThisTyped { get; }
    Dafny.ISequence<Dafny.Rune> dtor_rSelfName { get; }
    DAST._IType dtor_dafnyType { get; }
    _ISelfInfo DowncastClone();
    bool IsSelf();
  }
  public abstract class SelfInfo : _ISelfInfo {
    public SelfInfo() {
    }
    private static readonly DCOMP._ISelfInfo theDefault = create_NoSelf();
    public static DCOMP._ISelfInfo Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<DCOMP._ISelfInfo> _TYPE = new Dafny.TypeDescriptor<DCOMP._ISelfInfo>(DCOMP.SelfInfo.Default());
    public static Dafny.TypeDescriptor<DCOMP._ISelfInfo> _TypeDescriptor() {
      return _TYPE;
    }
    public static _ISelfInfo create_NoSelf() {
      return new SelfInfo_NoSelf();
    }
    public static _ISelfInfo create_ThisTyped(Dafny.ISequence<Dafny.Rune> rSelfName, DAST._IType dafnyType) {
      return new SelfInfo_ThisTyped(rSelfName, dafnyType);
    }
    public bool is_NoSelf { get { return this is SelfInfo_NoSelf; } }
    public bool is_ThisTyped { get { return this is SelfInfo_ThisTyped; } }
    public Dafny.ISequence<Dafny.Rune> dtor_rSelfName {
      get {
        var d = this;
        return ((SelfInfo_ThisTyped)d)._rSelfName;
      }
    }
    public DAST._IType dtor_dafnyType {
      get {
        var d = this;
        return ((SelfInfo_ThisTyped)d)._dafnyType;
      }
    }
    public abstract _ISelfInfo DowncastClone();
    public bool IsSelf() {
      return ((this).is_ThisTyped) && (((this).dtor_rSelfName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self")));
    }
  }
  public class SelfInfo_NoSelf : SelfInfo {
    public SelfInfo_NoSelf() : base() {
    }
    public override _ISelfInfo DowncastClone() {
      if (this is _ISelfInfo dt) { return dt; }
      return new SelfInfo_NoSelf();
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.SelfInfo_NoSelf;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.SelfInfo.NoSelf";
      return s;
    }
  }
  public class SelfInfo_ThisTyped : SelfInfo {
    public readonly Dafny.ISequence<Dafny.Rune> _rSelfName;
    public readonly DAST._IType _dafnyType;
    public SelfInfo_ThisTyped(Dafny.ISequence<Dafny.Rune> rSelfName, DAST._IType dafnyType) : base() {
      this._rSelfName = rSelfName;
      this._dafnyType = dafnyType;
    }
    public override _ISelfInfo DowncastClone() {
      if (this is _ISelfInfo dt) { return dt; }
      return new SelfInfo_ThisTyped(_rSelfName, _dafnyType);
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.SelfInfo_ThisTyped;
      return oth != null && object.Equals(this._rSelfName, oth._rSelfName) && object.Equals(this._dafnyType, oth._dafnyType);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._rSelfName));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._dafnyType));
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.SelfInfo.ThisTyped";
      s += "(";
      s += this._rSelfName.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._dafnyType);
      s += ")";
      return s;
    }
  }

  public interface _IExternAttribute {
    bool is_NoExtern { get; }
    bool is_SimpleExtern { get; }
    bool is_AdvancedExtern { get; }
    bool is_UnsupportedExtern { get; }
    Dafny.ISequence<Dafny.Rune> dtor_overrideName { get; }
    Dafny.ISequence<Dafny.Rune> dtor_enclosingModule { get; }
    Dafny.ISequence<Dafny.Rune> dtor_reason { get; }
    _IExternAttribute DowncastClone();
  }
  public abstract class ExternAttribute : _IExternAttribute {
    public ExternAttribute() {
    }
    private static readonly DCOMP._IExternAttribute theDefault = create_NoExtern();
    public static DCOMP._IExternAttribute Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<DCOMP._IExternAttribute> _TYPE = new Dafny.TypeDescriptor<DCOMP._IExternAttribute>(DCOMP.ExternAttribute.Default());
    public static Dafny.TypeDescriptor<DCOMP._IExternAttribute> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IExternAttribute create_NoExtern() {
      return new ExternAttribute_NoExtern();
    }
    public static _IExternAttribute create_SimpleExtern(Dafny.ISequence<Dafny.Rune> overrideName) {
      return new ExternAttribute_SimpleExtern(overrideName);
    }
    public static _IExternAttribute create_AdvancedExtern(Dafny.ISequence<Dafny.Rune> enclosingModule, Dafny.ISequence<Dafny.Rune> overrideName) {
      return new ExternAttribute_AdvancedExtern(enclosingModule, overrideName);
    }
    public static _IExternAttribute create_UnsupportedExtern(Dafny.ISequence<Dafny.Rune> reason) {
      return new ExternAttribute_UnsupportedExtern(reason);
    }
    public bool is_NoExtern { get { return this is ExternAttribute_NoExtern; } }
    public bool is_SimpleExtern { get { return this is ExternAttribute_SimpleExtern; } }
    public bool is_AdvancedExtern { get { return this is ExternAttribute_AdvancedExtern; } }
    public bool is_UnsupportedExtern { get { return this is ExternAttribute_UnsupportedExtern; } }
    public Dafny.ISequence<Dafny.Rune> dtor_overrideName {
      get {
        var d = this;
        if (d is ExternAttribute_SimpleExtern) { return ((ExternAttribute_SimpleExtern)d)._overrideName; }
        return ((ExternAttribute_AdvancedExtern)d)._overrideName;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_enclosingModule {
      get {
        var d = this;
        return ((ExternAttribute_AdvancedExtern)d)._enclosingModule;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_reason {
      get {
        var d = this;
        return ((ExternAttribute_UnsupportedExtern)d)._reason;
      }
    }
    public abstract _IExternAttribute DowncastClone();
  }
  public class ExternAttribute_NoExtern : ExternAttribute {
    public ExternAttribute_NoExtern() : base() {
    }
    public override _IExternAttribute DowncastClone() {
      if (this is _IExternAttribute dt) { return dt; }
      return new ExternAttribute_NoExtern();
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.ExternAttribute_NoExtern;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.ExternAttribute.NoExtern";
      return s;
    }
  }
  public class ExternAttribute_SimpleExtern : ExternAttribute {
    public readonly Dafny.ISequence<Dafny.Rune> _overrideName;
    public ExternAttribute_SimpleExtern(Dafny.ISequence<Dafny.Rune> overrideName) : base() {
      this._overrideName = overrideName;
    }
    public override _IExternAttribute DowncastClone() {
      if (this is _IExternAttribute dt) { return dt; }
      return new ExternAttribute_SimpleExtern(_overrideName);
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.ExternAttribute_SimpleExtern;
      return oth != null && object.Equals(this._overrideName, oth._overrideName);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._overrideName));
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.ExternAttribute.SimpleExtern";
      s += "(";
      s += this._overrideName.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class ExternAttribute_AdvancedExtern : ExternAttribute {
    public readonly Dafny.ISequence<Dafny.Rune> _enclosingModule;
    public readonly Dafny.ISequence<Dafny.Rune> _overrideName;
    public ExternAttribute_AdvancedExtern(Dafny.ISequence<Dafny.Rune> enclosingModule, Dafny.ISequence<Dafny.Rune> overrideName) : base() {
      this._enclosingModule = enclosingModule;
      this._overrideName = overrideName;
    }
    public override _IExternAttribute DowncastClone() {
      if (this is _IExternAttribute dt) { return dt; }
      return new ExternAttribute_AdvancedExtern(_enclosingModule, _overrideName);
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.ExternAttribute_AdvancedExtern;
      return oth != null && object.Equals(this._enclosingModule, oth._enclosingModule) && object.Equals(this._overrideName, oth._overrideName);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 2;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._enclosingModule));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._overrideName));
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.ExternAttribute.AdvancedExtern";
      s += "(";
      s += this._enclosingModule.ToVerbatimString(true);
      s += ", ";
      s += this._overrideName.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class ExternAttribute_UnsupportedExtern : ExternAttribute {
    public readonly Dafny.ISequence<Dafny.Rune> _reason;
    public ExternAttribute_UnsupportedExtern(Dafny.ISequence<Dafny.Rune> reason) : base() {
      this._reason = reason;
    }
    public override _IExternAttribute DowncastClone() {
      if (this is _IExternAttribute dt) { return dt; }
      return new ExternAttribute_UnsupportedExtern(_reason);
    }
    public override bool Equals(object other) {
      var oth = other as DCOMP.ExternAttribute_UnsupportedExtern;
      return oth != null && object.Equals(this._reason, oth._reason);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 3;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._reason));
      return (int) hash;
    }
    public override string ToString() {
      string s = "DafnyToRustCompiler.ExternAttribute.UnsupportedExtern";
      s += "(";
      s += this._reason.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }

  public partial class COMP {
    public COMP() {
      this.error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.Default();
      this.optimizations = Dafny.Sequence<Func<RAST._IMod, RAST._IMod>>.Empty;
      this._UnicodeChars = false;
      this._ObjectType = DCOMP.ObjectType.Default();
    }
    public RAST._IType Object(RAST._IType underlying) {
      if (((this).ObjectType).is_RawPointers) {
        return RAST.__default.PtrType(underlying);
      } else {
        return RAST.__default.ObjectType(underlying);
      }
    }
    public Dafny.ISequence<Dafny.Rune> UnreachablePanicIfVerified(Dafny.ISequence<Dafny.Rune> optText) {
      if (((this).ObjectType).is_RawPointers) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unsafe { ::std::hint::unreachable_unchecked() }");
      } else if ((optText).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("panic!()");
      } else {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("panic!(\""), optText), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\")"));
      }
    }
    public Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> error {get; set;}
    public Dafny.ISequence<Func<RAST._IMod, RAST._IMod>> optimizations {get; set;}
    public void __ctor(bool unicodeChars, DCOMP._IObjectType objectType)
    {
      (this)._UnicodeChars = unicodeChars;
      (this)._ObjectType = objectType;
      (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      (this).optimizations = Dafny.Sequence<Func<RAST._IMod, RAST._IMod>>.FromElements(FactorPathsOptimization.__default.apply);
    }
    public static Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> ContainingPathToRust(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> containingPath) {
      return Std.Collections.Seq.__default.Map<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>(((System.Func<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>)((_1319_i) => {
        return DCOMP.__default.escapeName((_1319_i));
      })), containingPath);
    }
    public DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> GenModule(DAST._IModule mod, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> containingPath)
    {
      DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> s = DafnyCompilerRustUtils.SeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule>.Default();
      _System._ITuple2<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs54 = DafnyCompilerRustUtils.__default.DafnyNameToContainingPathAndName((mod).dtor_name, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements());
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1320_innerPath = _let_tmp_rhs54.dtor__0;
      Dafny.ISequence<Dafny.Rune> _1321_innerName = _let_tmp_rhs54.dtor__1;
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1322_containingPath;
      _1322_containingPath = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(containingPath, _1320_innerPath);
      Dafny.ISequence<Dafny.Rune> _1323_modName;
      _1323_modName = DCOMP.__default.escapeName(_1321_innerName);
      if (((mod).dtor_body).is_None) {
        s = DafnyCompilerRustUtils.GatheringModule.Wrap(DCOMP.COMP.ContainingPathToRust(_1322_containingPath), RAST.Mod.create_ExternMod(_1323_modName));
      } else {
        DCOMP._IExternAttribute _1324_optExtern;
        _1324_optExtern = DCOMP.__default.ExtractExternMod(mod);
        Dafny.ISequence<RAST._IModDecl> _1325_body;
        DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _1326_allmodules;
        Dafny.ISequence<RAST._IModDecl> _out15;
        DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _out16;
        (this).GenModuleBody(((mod).dtor_body).dtor_value, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_1322_containingPath, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_1321_innerName)), out _out15, out _out16);
        _1325_body = _out15;
        _1326_allmodules = _out16;
        if ((_1324_optExtern).is_SimpleExtern) {
          if ((mod).dtor_requiresExterns) {
            _1325_body = Dafny.Sequence<RAST._IModDecl>.Concat(Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_UseDecl(RAST.Use.create(RAST.Visibility.create_PUB(), (((RAST.__default.crate).MSel(DCOMP.COMP.DAFNY__EXTERN__MODULE)).MSel(DCOMP.__default.ReplaceDotByDoubleColon((_1324_optExtern).dtor_overrideName))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"))))), _1325_body);
          }
        } else if ((_1324_optExtern).is_AdvancedExtern) {
          (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Externs on modules can only have 1 string argument"));
        } else if ((_1324_optExtern).is_UnsupportedExtern) {
          (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some((_1324_optExtern).dtor_reason);
        }
        s = DafnyCompilerRustUtils.GatheringModule.MergeSeqMap(DafnyCompilerRustUtils.GatheringModule.Wrap(DCOMP.COMP.ContainingPathToRust(_1322_containingPath), RAST.Mod.create_Mod(_1323_modName, _1325_body)), _1326_allmodules);
      }
      return s;
    }
    public void GenModuleBody(Dafny.ISequence<DAST._IModuleItem> body, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> containingPath, out Dafny.ISequence<RAST._IModDecl> s, out DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> allmodules)
    {
      s = Dafny.Sequence<RAST._IModDecl>.Empty;
      allmodules = DafnyCompilerRustUtils.SeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule>.Default();
      s = Dafny.Sequence<RAST._IModDecl>.FromElements();
      allmodules = DafnyCompilerRustUtils.SeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule>.Empty();
      BigInteger _hi6 = new BigInteger((body).Count);
      for (BigInteger _1327_i = BigInteger.Zero; _1327_i < _hi6; _1327_i++) {
        Dafny.ISequence<RAST._IModDecl> _1328_generated = Dafny.Sequence<RAST._IModDecl>.Empty;
        DAST._IModuleItem _source69 = (body).Select(_1327_i);
        {
          if (_source69.is_Module) {
            DAST._IModule _1329_m = _source69.dtor_Module_a0;
            DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _1330_mm;
            DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _out17;
            _out17 = (this).GenModule(_1329_m, containingPath);
            _1330_mm = _out17;
            allmodules = DafnyCompilerRustUtils.GatheringModule.MergeSeqMap(allmodules, _1330_mm);
            _1328_generated = Dafny.Sequence<RAST._IModDecl>.FromElements();
            goto after_match2;
          }
        }
        {
          if (_source69.is_Class) {
            DAST._IClass _1331_c = _source69.dtor_Class_a0;
            Dafny.ISequence<RAST._IModDecl> _out18;
            _out18 = (this).GenClass(_1331_c, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(containingPath, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements((_1331_c).dtor_name)));
            _1328_generated = _out18;
            goto after_match2;
          }
        }
        {
          if (_source69.is_Trait) {
            DAST._ITrait _1332_t = _source69.dtor_Trait_a0;
            Dafny.ISequence<RAST._IModDecl> _out19;
            _out19 = (this).GenTrait(_1332_t, containingPath);
            _1328_generated = _out19;
            goto after_match2;
          }
        }
        {
          if (_source69.is_Newtype) {
            DAST._INewtype _1333_n = _source69.dtor_Newtype_a0;
            Dafny.ISequence<RAST._IModDecl> _out20;
            _out20 = (this).GenNewtype(_1333_n);
            _1328_generated = _out20;
            goto after_match2;
          }
        }
        {
          if (_source69.is_SynonymType) {
            DAST._ISynonymType _1334_s = _source69.dtor_SynonymType_a0;
            Dafny.ISequence<RAST._IModDecl> _out21;
            _out21 = (this).GenSynonymType(_1334_s);
            _1328_generated = _out21;
            goto after_match2;
          }
        }
        {
          DAST._IDatatype _1335_d = _source69.dtor_Datatype_a0;
          Dafny.ISequence<RAST._IModDecl> _out22;
          _out22 = (this).GenDatatype(_1335_d);
          _1328_generated = _out22;
        }
      after_match2: ;
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, _1328_generated);
      }
    }
    public void GenTypeParam(DAST._ITypeArgDecl tp, out DAST._IType typeArg, out RAST._ITypeParamDecl typeParam)
    {
      typeArg = DAST.Type.Default();
      typeParam = RAST.TypeParamDecl.Default();
      typeArg = DAST.Type.create_TypeArg((tp).dtor_name);
      Dafny.ISequence<RAST._IType> _1336_genTpConstraint;
      if (((tp).dtor_bounds).Contains(DAST.TypeArgBound.create_SupportsEquality())) {
        _1336_genTpConstraint = Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.DafnyTypeEq);
      } else {
        _1336_genTpConstraint = Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.DafnyType);
      }
      if (((tp).dtor_bounds).Contains(DAST.TypeArgBound.create_SupportsDefault())) {
        _1336_genTpConstraint = Dafny.Sequence<RAST._IType>.Concat(_1336_genTpConstraint, Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.DefaultTrait));
      }
      typeParam = RAST.TypeParamDecl.create(DCOMP.__default.escapeName(((tp).dtor_name)), _1336_genTpConstraint);
    }
    public void GenTypeParameters(Dafny.ISequence<DAST._ITypeArgDecl> @params, out Dafny.ISequence<DAST._IType> typeParamsSeq, out Dafny.ISequence<RAST._IType> typeParams, out Dafny.ISequence<RAST._ITypeParamDecl> constrainedTypeParams, out Dafny.ISequence<Dafny.Rune> whereConstraints)
    {
      typeParamsSeq = Dafny.Sequence<DAST._IType>.Empty;
      typeParams = Dafny.Sequence<RAST._IType>.Empty;
      constrainedTypeParams = Dafny.Sequence<RAST._ITypeParamDecl>.Empty;
      whereConstraints = Dafny.Sequence<Dafny.Rune>.Empty;
      typeParamsSeq = Dafny.Sequence<DAST._IType>.FromElements();
      typeParams = Dafny.Sequence<RAST._IType>.FromElements();
      constrainedTypeParams = Dafny.Sequence<RAST._ITypeParamDecl>.FromElements();
      whereConstraints = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
      if ((new BigInteger((@params).Count)).Sign == 1) {
        BigInteger _hi7 = new BigInteger((@params).Count);
        for (BigInteger _1337_tpI = BigInteger.Zero; _1337_tpI < _hi7; _1337_tpI++) {
          DAST._ITypeArgDecl _1338_tp;
          _1338_tp = (@params).Select(_1337_tpI);
          DAST._IType _1339_typeArg;
          RAST._ITypeParamDecl _1340_typeParam;
          DAST._IType _out23;
          RAST._ITypeParamDecl _out24;
          (this).GenTypeParam(_1338_tp, out _out23, out _out24);
          _1339_typeArg = _out23;
          _1340_typeParam = _out24;
          RAST._IType _1341_rType;
          RAST._IType _out25;
          _out25 = (this).GenType(_1339_typeArg, DCOMP.GenTypeContext.@default());
          _1341_rType = _out25;
          typeParamsSeq = Dafny.Sequence<DAST._IType>.Concat(typeParamsSeq, Dafny.Sequence<DAST._IType>.FromElements(_1339_typeArg));
          typeParams = Dafny.Sequence<RAST._IType>.Concat(typeParams, Dafny.Sequence<RAST._IType>.FromElements(_1341_rType));
          constrainedTypeParams = Dafny.Sequence<RAST._ITypeParamDecl>.Concat(constrainedTypeParams, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(_1340_typeParam));
        }
      }
    }
    public bool IsSameResolvedTypeAnyArgs(DAST._IResolvedType r1, DAST._IResolvedType r2)
    {
      return (((r1).dtor_path).Equals((r2).dtor_path)) && (object.Equals((r1).dtor_kind, (r2).dtor_kind));
    }
    public bool IsSameResolvedType(DAST._IResolvedType r1, DAST._IResolvedType r2)
    {
      return ((this).IsSameResolvedTypeAnyArgs(r1, r2)) && (((r1).dtor_typeArgs).Equals((r2).dtor_typeArgs));
    }
    public Dafny.ISet<Dafny.ISequence<Dafny.Rune>> GatherTypeParamNames(Dafny.ISet<Dafny.ISequence<Dafny.Rune>> types, RAST._IType typ)
    {
      return (typ).Fold<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>(types, ((System.Func<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>, RAST._IType, Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>)((_1342_types, _1343_currentType) => {
        return (((_1343_currentType).is_TIdentifier) ? (Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_1342_types, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements((_1343_currentType).dtor_name))) : (_1342_types));
      })));
    }
    public Dafny.ISequence<RAST._IModDecl> GenClass(DAST._IClass c, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> path)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      Dafny.ISequence<DAST._IType> _1344_typeParamsSeq;
      Dafny.ISequence<RAST._IType> _1345_rTypeParams;
      Dafny.ISequence<RAST._ITypeParamDecl> _1346_rTypeParamsDecls;
      Dafny.ISequence<Dafny.Rune> _1347_whereConstraints;
      Dafny.ISequence<DAST._IType> _out26;
      Dafny.ISequence<RAST._IType> _out27;
      Dafny.ISequence<RAST._ITypeParamDecl> _out28;
      Dafny.ISequence<Dafny.Rune> _out29;
      (this).GenTypeParameters((c).dtor_typeParams, out _out26, out _out27, out _out28, out _out29);
      _1344_typeParamsSeq = _out26;
      _1345_rTypeParams = _out27;
      _1346_rTypeParamsDecls = _out28;
      _1347_whereConstraints = _out29;
      Dafny.ISequence<Dafny.Rune> _1348_constrainedTypeParams;
      _1348_constrainedTypeParams = RAST.TypeParamDecl.ToStringMultiple(_1346_rTypeParamsDecls, Dafny.Sequence<Dafny.Rune>.Concat(RAST.__default.IND, RAST.__default.IND));
      Dafny.ISequence<RAST._IField> _1349_fields;
      _1349_fields = Dafny.Sequence<RAST._IField>.FromElements();
      Dafny.ISequence<RAST._IAssignIdentifier> _1350_fieldInits;
      _1350_fieldInits = Dafny.Sequence<RAST._IAssignIdentifier>.FromElements();
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1351_usedTypeParams;
      _1351_usedTypeParams = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      BigInteger _hi8 = new BigInteger(((c).dtor_fields).Count);
      for (BigInteger _1352_fieldI = BigInteger.Zero; _1352_fieldI < _hi8; _1352_fieldI++) {
        DAST._IField _1353_field;
        _1353_field = ((c).dtor_fields).Select(_1352_fieldI);
        RAST._IType _1354_fieldType;
        RAST._IType _out30;
        _out30 = (this).GenType(((_1353_field).dtor_formal).dtor_typ, DCOMP.GenTypeContext.@default());
        _1354_fieldType = _out30;
        _1351_usedTypeParams = (this).GatherTypeParamNames(_1351_usedTypeParams, _1354_fieldType);
        Dafny.ISequence<Dafny.Rune> _1355_fieldRustName;
        _1355_fieldRustName = DCOMP.__default.escapeVar(((_1353_field).dtor_formal).dtor_name);
        _1349_fields = Dafny.Sequence<RAST._IField>.Concat(_1349_fields, Dafny.Sequence<RAST._IField>.FromElements(RAST.Field.create(RAST.Visibility.create_PUB(), RAST.Formal.create(_1355_fieldRustName, _1354_fieldType))));
        Std.Wrappers._IOption<DAST._IExpression> _source70 = (_1353_field).dtor_defaultValue;
        {
          if (_source70.is_Some) {
            DAST._IExpression _1356_e = _source70.dtor_value;
            {
              RAST._IExpr _1357_expr;
              DCOMP._IOwnership _1358___v51;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1359___v52;
              RAST._IExpr _out31;
              DCOMP._IOwnership _out32;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out33;
              (this).GenExpr(_1356_e, DCOMP.SelfInfo.create_NoSelf(), DCOMP.Environment.Empty(), DCOMP.Ownership.create_OwnershipOwned(), out _out31, out _out32, out _out33);
              _1357_expr = _out31;
              _1358___v51 = _out32;
              _1359___v52 = _out33;
              _1350_fieldInits = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_1350_fieldInits, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(_1355_fieldRustName, _1357_expr)));
            }
            goto after_match3;
          }
        }
        {
          {
            RAST._IExpr _1360_default;
            _1360_default = RAST.__default.std__Default__default;
            if ((_1354_fieldType).IsObjectOrPointer()) {
              _1360_default = (_1354_fieldType).ToNullExpr();
            }
            _1350_fieldInits = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_1350_fieldInits, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(_1355_fieldRustName, _1360_default)));
          }
        }
      after_match3: ;
      }
      BigInteger _hi9 = new BigInteger(((c).dtor_typeParams).Count);
      for (BigInteger _1361_typeParamI = BigInteger.Zero; _1361_typeParamI < _hi9; _1361_typeParamI++) {
        DAST._IType _1362_typeArg;
        RAST._ITypeParamDecl _1363_typeParam;
        DAST._IType _out34;
        RAST._ITypeParamDecl _out35;
        (this).GenTypeParam(((c).dtor_typeParams).Select(_1361_typeParamI), out _out34, out _out35);
        _1362_typeArg = _out34;
        _1363_typeParam = _out35;
        RAST._IType _1364_rTypeArg;
        RAST._IType _out36;
        _out36 = (this).GenType(_1362_typeArg, DCOMP.GenTypeContext.@default());
        _1364_rTypeArg = _out36;
        if ((_1351_usedTypeParams).Contains((_1363_typeParam).dtor_name)) {
          goto continue_0;
        }
        _1349_fields = Dafny.Sequence<RAST._IField>.Concat(_1349_fields, Dafny.Sequence<RAST._IField>.FromElements(RAST.Field.create(RAST.Visibility.create_PRIV(), RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_phantom_type_param_"), Std.Strings.__default.OfNat(_1361_typeParamI)), RAST.Type.create_TypeApp((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("marker"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("PhantomData"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_1364_rTypeArg))))));
        _1350_fieldInits = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_1350_fieldInits, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_phantom_type_param_"), Std.Strings.__default.OfNat(_1361_typeParamI)), RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::std::marker::PhantomData")))));
      continue_0: ;
      }
    after_0: ;
      DCOMP._IExternAttribute _1365_extern;
      _1365_extern = DCOMP.__default.ExtractExtern((c).dtor_attributes, (c).dtor_name);
      Dafny.ISequence<Dafny.Rune> _1366_className = Dafny.Sequence<Dafny.Rune>.Empty;
      if ((_1365_extern).is_SimpleExtern) {
        _1366_className = (_1365_extern).dtor_overrideName;
      } else {
        _1366_className = DCOMP.__default.escapeName((c).dtor_name);
        if ((_1365_extern).is_AdvancedExtern) {
          (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Multi-argument externs not supported for classes yet"));
        }
      }
      RAST._IStruct _1367_struct;
      _1367_struct = RAST.Struct.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), _1366_className, _1346_rTypeParamsDecls, RAST.Fields.create_NamedFields(_1349_fields));
      s = Dafny.Sequence<RAST._IModDecl>.FromElements();
      if ((_1365_extern).is_NoExtern) {
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_StructDecl(_1367_struct)));
      }
      Dafny.ISequence<RAST._IImplMember> _1368_implBody;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _1369_traitBodies;
      Dafny.ISequence<RAST._IImplMember> _out37;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _out38;
      (this).GenClassImplBody((c).dtor_body, false, DAST.Type.create_UserDefined(DAST.ResolvedType.create(path, Dafny.Sequence<DAST._IType>.FromElements(), DAST.ResolvedTypeBase.create_Class(), (c).dtor_attributes, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<DAST._IType>.FromElements())), _1344_typeParamsSeq, out _out37, out _out38);
      _1368_implBody = _out37;
      _1369_traitBodies = _out38;
      if (((_1365_extern).is_NoExtern) && (!(_1366_className).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_default")))) {
        _1368_implBody = Dafny.Sequence<RAST._IImplMember>.Concat(Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PUB(), RAST.Fn.create((this).allocate__fn, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(), Std.Wrappers.Option<RAST._IType>.create_Some((this).Object(RAST.__default.SelfOwned)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(((((RAST.__default.dafny__runtime).MSel((this).allocate)).AsExpr()).ApplyType1(RAST.__default.SelfOwned)).Apply(Dafny.Sequence<RAST._IExpr>.FromElements()))))), _1368_implBody);
      }
      RAST._IType _1370_selfTypeForImpl = RAST.Type.Default();
      if (((_1365_extern).is_NoExtern) || ((_1365_extern).is_UnsupportedExtern)) {
        _1370_selfTypeForImpl = RAST.Type.create_TIdentifier(_1366_className);
      } else if ((_1365_extern).is_AdvancedExtern) {
        _1370_selfTypeForImpl = (((RAST.__default.crate).MSel((_1365_extern).dtor_enclosingModule)).MSel((_1365_extern).dtor_overrideName)).AsType();
      } else if ((_1365_extern).is_SimpleExtern) {
        _1370_selfTypeForImpl = RAST.Type.create_TIdentifier((_1365_extern).dtor_overrideName);
      }
      if ((new BigInteger((_1368_implBody).Count)).Sign == 1) {
        RAST._IImpl _1371_i;
        _1371_i = RAST.Impl.create_Impl(_1346_rTypeParamsDecls, RAST.Type.create_TypeApp(_1370_selfTypeForImpl, _1345_rTypeParams), _1347_whereConstraints, _1368_implBody);
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(_1371_i)));
      }
      RAST._IType _1372_genSelfPath;
      RAST._IType _out39;
      _out39 = DCOMP.COMP.GenPathType(path);
      _1372_genSelfPath = _out39;
      if (!(_1366_className).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_default"))) {
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1346_rTypeParamsDecls, (((RAST.__default.dafny__runtime).MSel((this).Upcast)).AsType()).Apply(Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_DynType(RAST.__default.AnyTrait))), RAST.Type.create_TypeApp(_1372_genSelfPath, _1345_rTypeParams), _1347_whereConstraints, Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_ImplMemberMacro((((RAST.__default.dafny__runtime).MSel((this).UpcastFnMacro)).AsExpr()).Apply1(RAST.Expr.create_ExprFromType(RAST.Type.create_DynType(RAST.__default.AnyTrait)))))))));
      }
      Dafny.ISequence<DAST._IType> _1373_superClasses;
      if ((_1366_className).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_default"))) {
        _1373_superClasses = Dafny.Sequence<DAST._IType>.FromElements();
      } else {
        _1373_superClasses = (c).dtor_superClasses;
      }
      BigInteger _hi10 = new BigInteger((_1373_superClasses).Count);
      for (BigInteger _1374_i = BigInteger.Zero; _1374_i < _hi10; _1374_i++) {
        DAST._IType _1375_superClass;
        _1375_superClass = (_1373_superClasses).Select(_1374_i);
        DAST._IType _source71 = _1375_superClass;
        {
          if (_source71.is_UserDefined) {
            DAST._IResolvedType resolved0 = _source71.dtor_resolved;
            Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1376_traitPath = resolved0.dtor_path;
            Dafny.ISequence<DAST._IType> _1377_typeArgs = resolved0.dtor_typeArgs;
            DAST._IResolvedTypeBase kind0 = resolved0.dtor_kind;
            if (kind0.is_Trait) {
              Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1378_properMethods = resolved0.dtor_properMethods;
              {
                RAST._IType _1379_pathStr;
                RAST._IType _out40;
                _out40 = DCOMP.COMP.GenPathType(_1376_traitPath);
                _1379_pathStr = _out40;
                Dafny.ISequence<RAST._IType> _1380_typeArgs;
                Dafny.ISequence<RAST._IType> _out41;
                _out41 = (this).GenTypeArgs(_1377_typeArgs, DCOMP.GenTypeContext.@default());
                _1380_typeArgs = _out41;
                Dafny.ISequence<RAST._IImplMember> _1381_body;
                _1381_body = Dafny.Sequence<RAST._IImplMember>.FromElements();
                if ((_1369_traitBodies).Contains(_1376_traitPath)) {
                  _1381_body = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.Select(_1369_traitBodies,_1376_traitPath);
                }
                RAST._IType _1382_traitType;
                _1382_traitType = RAST.Type.create_TypeApp(_1379_pathStr, _1380_typeArgs);
                if (!((_1365_extern).is_NoExtern)) {
                  if (((new BigInteger((_1381_body).Count)).Sign == 0) && ((new BigInteger((_1378_properMethods).Count)).Sign != 0)) {
                    goto continue_1;
                  }
                  if ((new BigInteger((_1381_body).Count)) != (new BigInteger((_1378_properMethods).Count))) {
                    (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Error: In the class "), RAST.__default.SeqToString<Dafny.ISequence<Dafny.Rune>>(path, ((System.Func<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>)((_1383_s) => {
  return ((_1383_s));
})), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("."))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", some proper methods of ")), (_1382_traitType)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" are marked {:extern} and some are not.")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" For the Rust compiler, please make all methods (")), RAST.__default.SeqToString<Dafny.ISequence<Dafny.Rune>>(_1378_properMethods, ((System.Func<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>)((_1384_s) => {
  return (_1384_s);
})), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")  bodiless and mark as {:extern} and implement them in a Rust file, ")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("or mark none of them as {:extern} and implement them in Dafny. ")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Alternatively, you can insert an intermediate trait that performs the partial implementation if feasible.")));
                  }
                }
                RAST._IModDecl _1385_x;
                _1385_x = RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1346_rTypeParamsDecls, _1382_traitType, RAST.Type.create_TypeApp(_1372_genSelfPath, _1345_rTypeParams), _1347_whereConstraints, _1381_body));
                s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(_1385_x));
                s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1346_rTypeParamsDecls, (((RAST.__default.dafny__runtime).MSel((this).Upcast)).AsType()).Apply(Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_DynType(_1382_traitType))), RAST.Type.create_TypeApp(_1372_genSelfPath, _1345_rTypeParams), _1347_whereConstraints, Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_ImplMemberMacro((((RAST.__default.dafny__runtime).MSel((this).UpcastFnMacro)).AsExpr()).Apply1(RAST.Expr.create_ExprFromType(RAST.Type.create_DynType(_1382_traitType)))))))));
              }
              goto after_match4;
            }
          }
        }
        {
        }
      after_match4: ;
      continue_1: ;
      }
    after_1: ;
      return s;
    }
    public Dafny.ISequence<RAST._IModDecl> GenTrait(DAST._ITrait t, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> containingPath)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      Dafny.ISequence<DAST._IType> _1386_typeParamsSeq;
      _1386_typeParamsSeq = Dafny.Sequence<DAST._IType>.FromElements();
      Dafny.ISequence<RAST._ITypeParamDecl> _1387_typeParamDecls;
      _1387_typeParamDecls = Dafny.Sequence<RAST._ITypeParamDecl>.FromElements();
      Dafny.ISequence<RAST._IType> _1388_typeParams;
      _1388_typeParams = Dafny.Sequence<RAST._IType>.FromElements();
      if ((new BigInteger(((t).dtor_typeParams).Count)).Sign == 1) {
        BigInteger _hi11 = new BigInteger(((t).dtor_typeParams).Count);
        for (BigInteger _1389_tpI = BigInteger.Zero; _1389_tpI < _hi11; _1389_tpI++) {
          DAST._ITypeArgDecl _1390_tp;
          _1390_tp = ((t).dtor_typeParams).Select(_1389_tpI);
          DAST._IType _1391_typeArg;
          RAST._ITypeParamDecl _1392_typeParamDecl;
          DAST._IType _out42;
          RAST._ITypeParamDecl _out43;
          (this).GenTypeParam(_1390_tp, out _out42, out _out43);
          _1391_typeArg = _out42;
          _1392_typeParamDecl = _out43;
          _1386_typeParamsSeq = Dafny.Sequence<DAST._IType>.Concat(_1386_typeParamsSeq, Dafny.Sequence<DAST._IType>.FromElements(_1391_typeArg));
          _1387_typeParamDecls = Dafny.Sequence<RAST._ITypeParamDecl>.Concat(_1387_typeParamDecls, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(_1392_typeParamDecl));
          RAST._IType _1393_typeParam;
          RAST._IType _out44;
          _out44 = (this).GenType(_1391_typeArg, DCOMP.GenTypeContext.@default());
          _1393_typeParam = _out44;
          _1388_typeParams = Dafny.Sequence<RAST._IType>.Concat(_1388_typeParams, Dafny.Sequence<RAST._IType>.FromElements(_1393_typeParam));
        }
      }
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1394_fullPath;
      _1394_fullPath = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(containingPath, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements((t).dtor_name));
      Dafny.ISequence<RAST._IImplMember> _1395_implBody;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _1396___v56;
      Dafny.ISequence<RAST._IImplMember> _out45;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _out46;
      (this).GenClassImplBody((t).dtor_body, true, DAST.Type.create_UserDefined(DAST.ResolvedType.create(_1394_fullPath, Dafny.Sequence<DAST._IType>.FromElements(), DAST.ResolvedTypeBase.create_Trait(), (t).dtor_attributes, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<DAST._IType>.FromElements())), _1386_typeParamsSeq, out _out45, out _out46);
      _1395_implBody = _out45;
      _1396___v56 = _out46;
      Dafny.ISequence<RAST._IType> _1397_parents;
      _1397_parents = Dafny.Sequence<RAST._IType>.FromElements();
      BigInteger _hi12 = new BigInteger(((t).dtor_parents).Count);
      for (BigInteger _1398_i = BigInteger.Zero; _1398_i < _hi12; _1398_i++) {
        RAST._IType _1399_tpe;
        RAST._IType _out47;
        _out47 = (this).GenType(((t).dtor_parents).Select(_1398_i), DCOMP.GenTypeContext.ForTraitParents());
        _1399_tpe = _out47;
        _1397_parents = Dafny.Sequence<RAST._IType>.Concat(Dafny.Sequence<RAST._IType>.Concat(_1397_parents, Dafny.Sequence<RAST._IType>.FromElements(_1399_tpe)), Dafny.Sequence<RAST._IType>.FromElements((((RAST.__default.dafny__runtime).MSel((this).Upcast)).AsType()).Apply1(RAST.Type.create_DynType(_1399_tpe))));
      }
      s = Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_TraitDecl(RAST.Trait.create(_1387_typeParamDecls, RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(DCOMP.__default.escapeName((t).dtor_name)), _1388_typeParams), _1397_parents, _1395_implBody)));
      return s;
    }
    public Dafny.ISequence<RAST._IModDecl> GenNewtype(DAST._INewtype c)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      Dafny.ISequence<DAST._IType> _1400_typeParamsSeq;
      Dafny.ISequence<RAST._IType> _1401_rTypeParams;
      Dafny.ISequence<RAST._ITypeParamDecl> _1402_rTypeParamsDecls;
      Dafny.ISequence<Dafny.Rune> _1403_whereConstraints;
      Dafny.ISequence<DAST._IType> _out48;
      Dafny.ISequence<RAST._IType> _out49;
      Dafny.ISequence<RAST._ITypeParamDecl> _out50;
      Dafny.ISequence<Dafny.Rune> _out51;
      (this).GenTypeParameters((c).dtor_typeParams, out _out48, out _out49, out _out50, out _out51);
      _1400_typeParamsSeq = _out48;
      _1401_rTypeParams = _out49;
      _1402_rTypeParamsDecls = _out50;
      _1403_whereConstraints = _out51;
      Dafny.ISequence<Dafny.Rune> _1404_constrainedTypeParams;
      _1404_constrainedTypeParams = RAST.TypeParamDecl.ToStringMultiple(_1402_rTypeParamsDecls, Dafny.Sequence<Dafny.Rune>.Concat(RAST.__default.IND, RAST.__default.IND));
      RAST._IType _1405_underlyingType = RAST.Type.Default();
      Std.Wrappers._IOption<RAST._IType> _source72 = DCOMP.COMP.NewtypeRangeToRustType((c).dtor_range);
      {
        if (_source72.is_Some) {
          RAST._IType _1406_v = _source72.dtor_value;
          _1405_underlyingType = _1406_v;
          goto after_match5;
        }
      }
      {
        RAST._IType _out52;
        _out52 = (this).GenType((c).dtor_base, DCOMP.GenTypeContext.@default());
        _1405_underlyingType = _out52;
      }
    after_match5: ;
      DAST._IType _1407_resultingType;
      _1407_resultingType = DAST.Type.create_UserDefined(DAST.ResolvedType.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<DAST._IType>.FromElements(), DAST.ResolvedTypeBase.create_Newtype((c).dtor_base, (c).dtor_range, false), (c).dtor_attributes, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<DAST._IType>.FromElements()));
      Dafny.ISequence<Dafny.Rune> _1408_newtypeName;
      _1408_newtypeName = DCOMP.__default.escapeName((c).dtor_name);
      s = Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_StructDecl(RAST.Struct.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#[derive(Clone, PartialEq)]"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#[repr(transparent)]")), _1408_newtypeName, _1402_rTypeParamsDecls, RAST.Fields.create_NamelessFields(Dafny.Sequence<RAST._INamelessField>.FromElements(RAST.NamelessField.create(RAST.Visibility.create_PUB(), _1405_underlyingType))))));
      RAST._IExpr _1409_fnBody;
      _1409_fnBody = RAST.Expr.create_Identifier(_1408_newtypeName);
      Std.Wrappers._IOption<DAST._IExpression> _source73 = (c).dtor_witnessExpr;
      {
        if (_source73.is_Some) {
          DAST._IExpression _1410_e = _source73.dtor_value;
          {
            DAST._IExpression _1411_e;
            if (object.Equals((c).dtor_base, _1407_resultingType)) {
              _1411_e = _1410_e;
            } else {
              _1411_e = DAST.Expression.create_Convert(_1410_e, (c).dtor_base, _1407_resultingType);
            }
            RAST._IExpr _1412_eStr;
            DCOMP._IOwnership _1413___v57;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1414___v58;
            RAST._IExpr _out53;
            DCOMP._IOwnership _out54;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out55;
            (this).GenExpr(_1411_e, DCOMP.SelfInfo.create_NoSelf(), DCOMP.Environment.Empty(), DCOMP.Ownership.create_OwnershipOwned(), out _out53, out _out54, out _out55);
            _1412_eStr = _out53;
            _1413___v57 = _out54;
            _1414___v58 = _out55;
            _1409_fnBody = (_1409_fnBody).Apply1(_1412_eStr);
          }
          goto after_match6;
        }
      }
      {
        {
          _1409_fnBody = (_1409_fnBody).Apply1(RAST.__default.std__Default__default);
        }
      }
    after_match6: ;
      RAST._IImplMember _1415_body;
      _1415_body = RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("default"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.__default.SelfOwned), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1409_fnBody)));
      Std.Wrappers._IOption<DAST._INewtypeConstraint> _source74 = (c).dtor_constraint;
      {
        if (_source74.is_None) {
          goto after_match7;
        }
      }
      {
        DAST._INewtypeConstraint value8 = _source74.dtor_value;
        DAST._IFormal _1416_formal = value8.dtor_variable;
        Dafny.ISequence<DAST._IStatement> _1417_constraintStmts = value8.dtor_constraintStmts;
        RAST._IExpr _1418_rStmts;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1419___v59;
        DCOMP._IEnvironment _1420_newEnv;
        RAST._IExpr _out56;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out57;
        DCOMP._IEnvironment _out58;
        (this).GenStmts(_1417_constraintStmts, DCOMP.SelfInfo.create_NoSelf(), DCOMP.Environment.Empty(), false, Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_None(), out _out56, out _out57, out _out58);
        _1418_rStmts = _out56;
        _1419___v59 = _out57;
        _1420_newEnv = _out58;
        Dafny.ISequence<RAST._IFormal> _1421_rFormals;
        Dafny.ISequence<RAST._IFormal> _out59;
        _out59 = (this).GenParams(Dafny.Sequence<DAST._IFormal>.FromElements(_1416_formal), false);
        _1421_rFormals = _out59;
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_Impl(_1402_rTypeParamsDecls, RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1408_newtypeName), _1401_rTypeParams), _1403_whereConstraints, Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PUB(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("is"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), _1421_rFormals, Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Bool()), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1418_rStmts))))))));
      }
    after_match7: ;
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1402_rTypeParamsDecls, RAST.__default.DefaultTrait, RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1408_newtypeName), _1401_rTypeParams), _1403_whereConstraints, Dafny.Sequence<RAST._IImplMember>.FromElements(_1415_body)))));
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1402_rTypeParamsDecls, RAST.__default.DafnyPrint, RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1408_newtypeName), _1401_rTypeParams), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt_print"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed, RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_formatter"), RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&mut ::std::fmt::Formatter"))), RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("in_seq"), RAST.Type.create_Bool())), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::std::fmt::Result"))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::dafny_runtime::DafnyPrint::fmt_print(&self.0, _formatter, in_seq)"))))))))));
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1402_rTypeParamsDecls, RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::std::ops::Deref")), RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1408_newtypeName), _1401_rTypeParams), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_RawImplMember(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("type Target = "), (_1405_underlyingType)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";"))), RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("deref"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Borrowed(((RAST.Path.create_Self()).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Target"))).AsType())), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&self.0"))))))))));
      return s;
    }
    public Dafny.ISequence<RAST._IModDecl> GenSynonymType(DAST._ISynonymType c)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      Dafny.ISequence<DAST._IType> _1422_typeParamsSeq;
      Dafny.ISequence<RAST._IType> _1423_rTypeParams;
      Dafny.ISequence<RAST._ITypeParamDecl> _1424_rTypeParamsDecls;
      Dafny.ISequence<Dafny.Rune> _1425_whereConstraints;
      Dafny.ISequence<DAST._IType> _out60;
      Dafny.ISequence<RAST._IType> _out61;
      Dafny.ISequence<RAST._ITypeParamDecl> _out62;
      Dafny.ISequence<Dafny.Rune> _out63;
      (this).GenTypeParameters((c).dtor_typeParams, out _out60, out _out61, out _out62, out _out63);
      _1422_typeParamsSeq = _out60;
      _1423_rTypeParams = _out61;
      _1424_rTypeParamsDecls = _out62;
      _1425_whereConstraints = _out63;
      Dafny.ISequence<Dafny.Rune> _1426_synonymTypeName;
      _1426_synonymTypeName = DCOMP.__default.escapeName((c).dtor_name);
      RAST._IType _1427_resultingType;
      RAST._IType _out64;
      _out64 = (this).GenType((c).dtor_base, DCOMP.GenTypeContext.@default());
      _1427_resultingType = _out64;
      s = Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_TypeDecl(RAST.TypeSynonym.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), _1426_synonymTypeName, _1424_rTypeParamsDecls, _1427_resultingType)));
      Dafny.ISequence<RAST._ITypeParamDecl> _1428_defaultConstrainedTypeParams;
      _1428_defaultConstrainedTypeParams = RAST.TypeParamDecl.AddConstraintsMultiple(_1424_rTypeParamsDecls, Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.DefaultTrait));
      Std.Wrappers._IOption<DAST._IExpression> _source75 = (c).dtor_witnessExpr;
      {
        if (_source75.is_Some) {
          DAST._IExpression _1429_e = _source75.dtor_value;
          {
            RAST._IExpr _1430_rStmts;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1431___v60;
            DCOMP._IEnvironment _1432_newEnv;
            RAST._IExpr _out65;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out66;
            DCOMP._IEnvironment _out67;
            (this).GenStmts((c).dtor_witnessStmts, DCOMP.SelfInfo.create_NoSelf(), DCOMP.Environment.Empty(), false, Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_None(), out _out65, out _out66, out _out67);
            _1430_rStmts = _out65;
            _1431___v60 = _out66;
            _1432_newEnv = _out67;
            RAST._IExpr _1433_rExpr;
            DCOMP._IOwnership _1434___v61;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1435___v62;
            RAST._IExpr _out68;
            DCOMP._IOwnership _out69;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out70;
            (this).GenExpr(_1429_e, DCOMP.SelfInfo.create_NoSelf(), _1432_newEnv, DCOMP.Ownership.create_OwnershipOwned(), out _out68, out _out69, out _out70);
            _1433_rExpr = _out68;
            _1434___v61 = _out69;
            _1435___v62 = _out70;
            Dafny.ISequence<Dafny.Rune> _1436_constantName;
            _1436_constantName = DCOMP.__default.escapeName(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_init_"), ((c).dtor_name)));
            s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_TopFnDecl(RAST.TopFnDecl.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), RAST.Visibility.create_PUB(), RAST.Fn.create(_1436_constantName, _1428_defaultConstrainedTypeParams, Dafny.Sequence<RAST._IFormal>.FromElements(), Std.Wrappers.Option<RAST._IType>.create_Some(_1427_resultingType), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some((_1430_rStmts).Then(_1433_rExpr)))))));
          }
          goto after_match8;
        }
      }
      {
      }
    after_match8: ;
      return s;
    }
    public bool TypeIsEq(DAST._IType t) {
      DAST._IType _source76 = t;
      {
        if (_source76.is_UserDefined) {
          return true;
        }
      }
      {
        if (_source76.is_Tuple) {
          Dafny.ISequence<DAST._IType> _1437_ts = _source76.dtor_Tuple_a0;
          return Dafny.Helpers.Id<Func<Dafny.ISequence<DAST._IType>, bool>>((_1438_ts) => Dafny.Helpers.Quantifier<DAST._IType>((_1438_ts).UniqueElements, true, (((_forall_var_5) => {
            DAST._IType _1439_t = (DAST._IType)_forall_var_5;
            return !((_1438_ts).Contains(_1439_t)) || ((this).TypeIsEq(_1439_t));
          }))))(_1437_ts);
        }
      }
      {
        if (_source76.is_Array) {
          DAST._IType _1440_t = _source76.dtor_element;
          return (this).TypeIsEq(_1440_t);
        }
      }
      {
        if (_source76.is_Seq) {
          DAST._IType _1441_t = _source76.dtor_element;
          return (this).TypeIsEq(_1441_t);
        }
      }
      {
        if (_source76.is_Set) {
          DAST._IType _1442_t = _source76.dtor_element;
          return (this).TypeIsEq(_1442_t);
        }
      }
      {
        if (_source76.is_Multiset) {
          DAST._IType _1443_t = _source76.dtor_element;
          return (this).TypeIsEq(_1443_t);
        }
      }
      {
        if (_source76.is_Map) {
          DAST._IType _1444_k = _source76.dtor_key;
          DAST._IType _1445_v = _source76.dtor_value;
          return ((this).TypeIsEq(_1444_k)) && ((this).TypeIsEq(_1445_v));
        }
      }
      {
        if (_source76.is_SetBuilder) {
          DAST._IType _1446_t = _source76.dtor_element;
          return (this).TypeIsEq(_1446_t);
        }
      }
      {
        if (_source76.is_MapBuilder) {
          DAST._IType _1447_k = _source76.dtor_key;
          DAST._IType _1448_v = _source76.dtor_value;
          return ((this).TypeIsEq(_1447_k)) && ((this).TypeIsEq(_1448_v));
        }
      }
      {
        if (_source76.is_Arrow) {
          return false;
        }
      }
      {
        if (_source76.is_Primitive) {
          return true;
        }
      }
      {
        if (_source76.is_Passthrough) {
          return true;
        }
      }
      {
        if (_source76.is_TypeArg) {
          Dafny.ISequence<Dafny.Rune> _1449_i = _source76.dtor_TypeArg_a0;
          return true;
        }
      }
      {
        return true;
      }
    }
    public bool DatatypeIsEq(DAST._IDatatype c) {
      return (!((c).dtor_isCo)) && (Dafny.Helpers.Id<Func<DAST._IDatatype, bool>>((_1450_c) => Dafny.Helpers.Quantifier<DAST._IDatatypeCtor>(((_1450_c).dtor_ctors).UniqueElements, true, (((_forall_var_6) => {
        DAST._IDatatypeCtor _1451_ctor = (DAST._IDatatypeCtor)_forall_var_6;
        return Dafny.Helpers.Quantifier<DAST._IDatatypeDtor>(((_1451_ctor).dtor_args).UniqueElements, true, (((_forall_var_7) => {
          DAST._IDatatypeDtor _1452_arg = (DAST._IDatatypeDtor)_forall_var_7;
          return !((((_1450_c).dtor_ctors).Contains(_1451_ctor)) && (((_1451_ctor).dtor_args).Contains(_1452_arg))) || ((this).TypeIsEq(((_1452_arg).dtor_formal).dtor_typ));
        })));
      }))))(c));
    }
    public Dafny.ISequence<RAST._IModDecl> GenDatatype(DAST._IDatatype c)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      Dafny.ISequence<DAST._IType> _1453_typeParamsSeq;
      Dafny.ISequence<RAST._IType> _1454_rTypeParams;
      Dafny.ISequence<RAST._ITypeParamDecl> _1455_rTypeParamsDecls;
      Dafny.ISequence<Dafny.Rune> _1456_whereConstraints;
      Dafny.ISequence<DAST._IType> _out71;
      Dafny.ISequence<RAST._IType> _out72;
      Dafny.ISequence<RAST._ITypeParamDecl> _out73;
      Dafny.ISequence<Dafny.Rune> _out74;
      (this).GenTypeParameters((c).dtor_typeParams, out _out71, out _out72, out _out73, out _out74);
      _1453_typeParamsSeq = _out71;
      _1454_rTypeParams = _out72;
      _1455_rTypeParamsDecls = _out73;
      _1456_whereConstraints = _out74;
      Dafny.ISequence<Dafny.Rune> _1457_datatypeName;
      _1457_datatypeName = DCOMP.__default.escapeName((c).dtor_name);
      Dafny.ISequence<RAST._IEnumCase> _1458_ctors;
      _1458_ctors = Dafny.Sequence<RAST._IEnumCase>.FromElements();
      Dafny.ISequence<DAST._IVariance> _1459_variances;
      _1459_variances = Std.Collections.Seq.__default.Map<DAST._ITypeArgDecl, DAST._IVariance>(((System.Func<DAST._ITypeArgDecl, DAST._IVariance>)((_1460_typeParamDecl) => {
        return (_1460_typeParamDecl).dtor_variance;
      })), (c).dtor_typeParams);
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1461_usedTypeParams;
      _1461_usedTypeParams = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      BigInteger _hi13 = new BigInteger(((c).dtor_ctors).Count);
      for (BigInteger _1462_i = BigInteger.Zero; _1462_i < _hi13; _1462_i++) {
        DAST._IDatatypeCtor _1463_ctor;
        _1463_ctor = ((c).dtor_ctors).Select(_1462_i);
        Dafny.ISequence<RAST._IField> _1464_ctorArgs;
        _1464_ctorArgs = Dafny.Sequence<RAST._IField>.FromElements();
        bool _1465_isNumeric;
        _1465_isNumeric = false;
        BigInteger _hi14 = new BigInteger(((_1463_ctor).dtor_args).Count);
        for (BigInteger _1466_j = BigInteger.Zero; _1466_j < _hi14; _1466_j++) {
          DAST._IDatatypeDtor _1467_dtor;
          _1467_dtor = ((_1463_ctor).dtor_args).Select(_1466_j);
          RAST._IType _1468_formalType;
          RAST._IType _out75;
          _out75 = (this).GenType(((_1467_dtor).dtor_formal).dtor_typ, DCOMP.GenTypeContext.@default());
          _1468_formalType = _out75;
          _1461_usedTypeParams = (this).GatherTypeParamNames(_1461_usedTypeParams, _1468_formalType);
          Dafny.ISequence<Dafny.Rune> _1469_formalName;
          _1469_formalName = DCOMP.__default.escapeVar(((_1467_dtor).dtor_formal).dtor_name);
          if (((_1466_j).Sign == 0) && ((Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")).Equals(_1469_formalName))) {
            _1465_isNumeric = true;
          }
          if ((((_1466_j).Sign != 0) && (_1465_isNumeric)) && (!(Std.Strings.__default.OfNat(_1466_j)).Equals(_1469_formalName))) {
            (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Formal extern names were supposed to be numeric but got "), _1469_formalName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" instead of ")), Std.Strings.__default.OfNat(_1466_j)));
            _1465_isNumeric = false;
          }
          if ((c).dtor_isCo) {
            _1464_ctorArgs = Dafny.Sequence<RAST._IField>.Concat(_1464_ctorArgs, Dafny.Sequence<RAST._IField>.FromElements(RAST.Field.create(RAST.Visibility.create_PRIV(), RAST.Formal.create(_1469_formalName, RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("LazyFieldWrapper"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_1468_formalType))))));
          } else {
            _1464_ctorArgs = Dafny.Sequence<RAST._IField>.Concat(_1464_ctorArgs, Dafny.Sequence<RAST._IField>.FromElements(RAST.Field.create(RAST.Visibility.create_PRIV(), RAST.Formal.create(_1469_formalName, _1468_formalType))));
          }
        }
        RAST._IFields _1470_namedFields;
        _1470_namedFields = RAST.Fields.create_NamedFields(_1464_ctorArgs);
        if (_1465_isNumeric) {
          _1470_namedFields = (_1470_namedFields).ToNamelessFields();
        }
        _1458_ctors = Dafny.Sequence<RAST._IEnumCase>.Concat(_1458_ctors, Dafny.Sequence<RAST._IEnumCase>.FromElements(RAST.EnumCase.create(DCOMP.__default.escapeName((_1463_ctor).dtor_name), _1470_namedFields)));
      }
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1471_unusedTypeParams;
      _1471_unusedTypeParams = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(Dafny.Helpers.Id<Func<Dafny.ISequence<RAST._ITypeParamDecl>, Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>>((_1472_rTypeParamsDecls) => ((System.Func<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>)(() => {
        var _coll9 = new System.Collections.Generic.List<Dafny.ISequence<Dafny.Rune>>();
        foreach (RAST._ITypeParamDecl _compr_10 in (_1472_rTypeParamsDecls).CloneAsArray()) {
          RAST._ITypeParamDecl _1473_tp = (RAST._ITypeParamDecl)_compr_10;
          if ((_1472_rTypeParamsDecls).Contains(_1473_tp)) {
            _coll9.Add((_1473_tp).dtor_name);
          }
        }
        return Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromCollection(_coll9);
      }))())(_1455_rTypeParamsDecls), _1461_usedTypeParams);
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1474_selfPath;
      _1474_selfPath = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements((c).dtor_name);
      Dafny.ISequence<RAST._IImplMember> _1475_implBodyRaw;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _1476_traitBodies;
      Dafny.ISequence<RAST._IImplMember> _out76;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _out77;
      (this).GenClassImplBody((c).dtor_body, false, DAST.Type.create_UserDefined(DAST.ResolvedType.create(_1474_selfPath, _1453_typeParamsSeq, DAST.ResolvedTypeBase.create_Datatype(_1459_variances), (c).dtor_attributes, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<DAST._IType>.FromElements())), _1453_typeParamsSeq, out _out76, out _out77);
      _1475_implBodyRaw = _out76;
      _1476_traitBodies = _out77;
      Dafny.ISequence<RAST._IImplMember> _1477_implBody;
      _1477_implBody = _1475_implBodyRaw;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1478_emittedFields;
      _1478_emittedFields = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      BigInteger _hi15 = new BigInteger(((c).dtor_ctors).Count);
      for (BigInteger _1479_i = BigInteger.Zero; _1479_i < _hi15; _1479_i++) {
        DAST._IDatatypeCtor _1480_ctor;
        _1480_ctor = ((c).dtor_ctors).Select(_1479_i);
        BigInteger _hi16 = new BigInteger(((_1480_ctor).dtor_args).Count);
        for (BigInteger _1481_j = BigInteger.Zero; _1481_j < _hi16; _1481_j++) {
          DAST._IDatatypeDtor _1482_dtor;
          _1482_dtor = ((_1480_ctor).dtor_args).Select(_1481_j);
          Dafny.ISequence<Dafny.Rune> _1483_callName;
          _1483_callName = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.GetOr((_1482_dtor).dtor_callName, DCOMP.__default.escapeVar(((_1482_dtor).dtor_formal).dtor_name));
          if (!((_1478_emittedFields).Contains(_1483_callName))) {
            _1478_emittedFields = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_1478_emittedFields, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_1483_callName));
            RAST._IType _1484_formalType;
            RAST._IType _out78;
            _out78 = (this).GenType(((_1482_dtor).dtor_formal).dtor_typ, DCOMP.GenTypeContext.@default());
            _1484_formalType = _out78;
            Dafny.ISequence<RAST._IMatchCase> _1485_cases;
            _1485_cases = Dafny.Sequence<RAST._IMatchCase>.FromElements();
            BigInteger _hi17 = new BigInteger(((c).dtor_ctors).Count);
            for (BigInteger _1486_k = BigInteger.Zero; _1486_k < _hi17; _1486_k++) {
              DAST._IDatatypeCtor _1487_ctor2;
              _1487_ctor2 = ((c).dtor_ctors).Select(_1486_k);
              Dafny.ISequence<Dafny.Rune> _1488_pattern;
              _1488_pattern = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1457_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), DCOMP.__default.escapeName((_1487_ctor2).dtor_name));
              Dafny.ISequence<Dafny.Rune> _1489_rhs = Dafny.Sequence<Dafny.Rune>.Empty;
              Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1490_hasMatchingField;
              _1490_hasMatchingField = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
              Dafny.ISequence<Dafny.Rune> _1491_patternInner;
              _1491_patternInner = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
              bool _1492_isNumeric;
              _1492_isNumeric = false;
              BigInteger _hi18 = new BigInteger(((_1487_ctor2).dtor_args).Count);
              for (BigInteger _1493_l = BigInteger.Zero; _1493_l < _hi18; _1493_l++) {
                DAST._IDatatypeDtor _1494_dtor2;
                _1494_dtor2 = ((_1487_ctor2).dtor_args).Select(_1493_l);
                Dafny.ISequence<Dafny.Rune> _1495_patternName;
                _1495_patternName = DCOMP.__default.escapeVar(((_1494_dtor2).dtor_formal).dtor_name);
                if (((_1493_l).Sign == 0) && ((_1495_patternName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")))) {
                  _1492_isNumeric = true;
                }
                if (_1492_isNumeric) {
                  _1495_patternName = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.GetOr((_1494_dtor2).dtor_callName, Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("v"), Std.Strings.__default.OfNat(_1493_l)));
                }
                if (object.Equals(((_1482_dtor).dtor_formal).dtor_name, ((_1494_dtor2).dtor_formal).dtor_name)) {
                  _1490_hasMatchingField = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(_1495_patternName);
                }
                _1491_patternInner = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1491_patternInner, _1495_patternName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "));
              }
              if (_1492_isNumeric) {
                _1488_pattern = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1488_pattern, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")), _1491_patternInner), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
              } else {
                _1488_pattern = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1488_pattern, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{")), _1491_patternInner), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
              }
              if ((_1490_hasMatchingField).is_Some) {
                if ((c).dtor_isCo) {
                  _1489_rhs = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::std::ops::Deref::deref(&"), (_1490_hasMatchingField).dtor_value), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".0)"));
                } else {
                  _1489_rhs = Dafny.Sequence<Dafny.Rune>.Concat((_1490_hasMatchingField).dtor_value, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
                }
              } else {
                _1489_rhs = (this).UnreachablePanicIfVerified(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("field does not exist on this variant"));
              }
              RAST._IMatchCase _1496_ctorMatch;
              _1496_ctorMatch = RAST.MatchCase.create(_1488_pattern, RAST.Expr.create_RawExpr(_1489_rhs));
              _1485_cases = Dafny.Sequence<RAST._IMatchCase>.Concat(_1485_cases, Dafny.Sequence<RAST._IMatchCase>.FromElements(_1496_ctorMatch));
            }
            if (((new BigInteger(((c).dtor_typeParams).Count)).Sign == 1) && ((new BigInteger((_1471_unusedTypeParams).Count)).Sign == 1)) {
              _1485_cases = Dafny.Sequence<RAST._IMatchCase>.Concat(_1485_cases, Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(_1457_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::_PhantomVariant(..)")), RAST.Expr.create_RawExpr((this).UnreachablePanicIfVerified(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))))));
            }
            RAST._IExpr _1497_methodBody;
            _1497_methodBody = RAST.Expr.create_Match(RAST.__default.self, _1485_cases);
            _1477_implBody = Dafny.Sequence<RAST._IImplMember>.Concat(_1477_implBody, Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PUB(), RAST.Fn.create(_1483_callName, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Borrowed(_1484_formalType)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1497_methodBody)))));
          }
        }
      }
      Dafny.ISequence<RAST._IType> _1498_coerceTypes;
      _1498_coerceTypes = Dafny.Sequence<RAST._IType>.FromElements();
      Dafny.ISequence<RAST._ITypeParamDecl> _1499_rCoerceTypeParams;
      _1499_rCoerceTypeParams = Dafny.Sequence<RAST._ITypeParamDecl>.FromElements();
      Dafny.ISequence<RAST._IFormal> _1500_coerceArguments;
      _1500_coerceArguments = Dafny.Sequence<RAST._IFormal>.FromElements();
      Dafny.IMap<DAST._IType,DAST._IType> _1501_coerceMap;
      _1501_coerceMap = Dafny.Map<DAST._IType, DAST._IType>.FromElements();
      Dafny.IMap<RAST._IType,RAST._IType> _1502_rCoerceMap;
      _1502_rCoerceMap = Dafny.Map<RAST._IType, RAST._IType>.FromElements();
      Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr> _1503_coerceMapToArg;
      _1503_coerceMapToArg = Dafny.Map<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>.FromElements();
      if ((new BigInteger(((c).dtor_typeParams).Count)).Sign == 1) {
        Dafny.ISequence<RAST._IType> _1504_types;
        _1504_types = Dafny.Sequence<RAST._IType>.FromElements();
        BigInteger _hi19 = new BigInteger(((c).dtor_typeParams).Count);
        for (BigInteger _1505_typeI = BigInteger.Zero; _1505_typeI < _hi19; _1505_typeI++) {
          DAST._ITypeArgDecl _1506_typeParam;
          _1506_typeParam = ((c).dtor_typeParams).Select(_1505_typeI);
          DAST._IType _1507_typeArg;
          RAST._ITypeParamDecl _1508_rTypeParamDecl;
          DAST._IType _out79;
          RAST._ITypeParamDecl _out80;
          (this).GenTypeParam(_1506_typeParam, out _out79, out _out80);
          _1507_typeArg = _out79;
          _1508_rTypeParamDecl = _out80;
          RAST._IType _1509_rTypeArg;
          RAST._IType _out81;
          _out81 = (this).GenType(_1507_typeArg, DCOMP.GenTypeContext.@default());
          _1509_rTypeArg = _out81;
          _1504_types = Dafny.Sequence<RAST._IType>.Concat(_1504_types, Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_TypeApp((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("marker"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("PhantomData"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_1509_rTypeArg))));
          if (((_1505_typeI) < (new BigInteger((_1459_variances).Count))) && (((_1459_variances).Select(_1505_typeI)).is_Nonvariant)) {
            _1498_coerceTypes = Dafny.Sequence<RAST._IType>.Concat(_1498_coerceTypes, Dafny.Sequence<RAST._IType>.FromElements(_1509_rTypeArg));
            goto continue0;
          }
          DAST._ITypeArgDecl _1510_coerceTypeParam;
          DAST._ITypeArgDecl _1511_dt__update__tmp_h0 = _1506_typeParam;
          Dafny.ISequence<Dafny.Rune> _1512_dt__update_hname_h0 = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_T"), Std.Strings.__default.OfNat(_1505_typeI));
          _1510_coerceTypeParam = DAST.TypeArgDecl.create(_1512_dt__update_hname_h0, (_1511_dt__update__tmp_h0).dtor_bounds, (_1511_dt__update__tmp_h0).dtor_variance);
          DAST._IType _1513_coerceTypeArg;
          RAST._ITypeParamDecl _1514_rCoerceTypeParamDecl;
          DAST._IType _out82;
          RAST._ITypeParamDecl _out83;
          (this).GenTypeParam(_1510_coerceTypeParam, out _out82, out _out83);
          _1513_coerceTypeArg = _out82;
          _1514_rCoerceTypeParamDecl = _out83;
          _1501_coerceMap = Dafny.Map<DAST._IType, DAST._IType>.Merge(_1501_coerceMap, Dafny.Map<DAST._IType, DAST._IType>.FromElements(new Dafny.Pair<DAST._IType, DAST._IType>(_1507_typeArg, _1513_coerceTypeArg)));
          RAST._IType _1515_rCoerceType;
          RAST._IType _out84;
          _out84 = (this).GenType(_1513_coerceTypeArg, DCOMP.GenTypeContext.@default());
          _1515_rCoerceType = _out84;
          _1502_rCoerceMap = Dafny.Map<RAST._IType, RAST._IType>.Merge(_1502_rCoerceMap, Dafny.Map<RAST._IType, RAST._IType>.FromElements(new Dafny.Pair<RAST._IType, RAST._IType>(_1509_rTypeArg, _1515_rCoerceType)));
          _1498_coerceTypes = Dafny.Sequence<RAST._IType>.Concat(_1498_coerceTypes, Dafny.Sequence<RAST._IType>.FromElements(_1515_rCoerceType));
          _1499_rCoerceTypeParams = Dafny.Sequence<RAST._ITypeParamDecl>.Concat(_1499_rCoerceTypeParams, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(_1514_rCoerceTypeParamDecl));
          Dafny.ISequence<Dafny.Rune> _1516_coerceFormal;
          _1516_coerceFormal = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("f_"), Std.Strings.__default.OfNat(_1505_typeI));
          _1503_coerceMapToArg = Dafny.Map<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>.Merge(_1503_coerceMapToArg, Dafny.Map<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>.FromElements(new Dafny.Pair<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>(_System.Tuple2<RAST._IType, RAST._IType>.create(_1509_rTypeArg, _1515_rCoerceType), (RAST.Expr.create_Identifier(_1516_coerceFormal)).Clone())));
          _1500_coerceArguments = Dafny.Sequence<RAST._IFormal>.Concat(_1500_coerceArguments, Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.create(_1516_coerceFormal, RAST.__default.Rc(RAST.Type.create_IntersectionType(RAST.Type.create_ImplType(RAST.Type.create_FnType(Dafny.Sequence<RAST._IType>.FromElements(_1509_rTypeArg), _1515_rCoerceType)), RAST.__default.StaticTrait)))));
        continue0: ;
        }
      after0: ;
        if ((new BigInteger((_1471_unusedTypeParams).Count)).Sign == 1) {
          _1458_ctors = Dafny.Sequence<RAST._IEnumCase>.Concat(_1458_ctors, Dafny.Sequence<RAST._IEnumCase>.FromElements(RAST.EnumCase.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_PhantomVariant"), RAST.Fields.create_NamelessFields(Std.Collections.Seq.__default.Map<RAST._IType, RAST._INamelessField>(((System.Func<RAST._IType, RAST._INamelessField>)((_1517_tpe) => {
  return RAST.NamelessField.create(RAST.Visibility.create_PRIV(), _1517_tpe);
})), _1504_types)))));
        }
      }
      bool _1518_cIsEq;
      _1518_cIsEq = (this).DatatypeIsEq(c);
      s = Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_EnumDecl(RAST.Enum.create(((_1518_cIsEq) ? (Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#[derive(PartialEq, Clone)]"))) : (Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#[derive(Clone)]")))), _1457_datatypeName, _1455_rTypeParamsDecls, _1458_ctors)), RAST.ModDecl.create_ImplDecl(RAST.Impl.create_Impl(_1455_rTypeParamsDecls, RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1457_datatypeName), _1454_rTypeParams), _1456_whereConstraints, _1477_implBody)));
      Dafny.ISequence<RAST._IMatchCase> _1519_printImplBodyCases;
      _1519_printImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.FromElements();
      Dafny.ISequence<RAST._IMatchCase> _1520_hashImplBodyCases;
      _1520_hashImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.FromElements();
      Dafny.ISequence<RAST._IMatchCase> _1521_coerceImplBodyCases;
      _1521_coerceImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.FromElements();
      BigInteger _hi20 = new BigInteger(((c).dtor_ctors).Count);
      for (BigInteger _1522_i = BigInteger.Zero; _1522_i < _hi20; _1522_i++) {
        DAST._IDatatypeCtor _1523_ctor;
        _1523_ctor = ((c).dtor_ctors).Select(_1522_i);
        Dafny.ISequence<Dafny.Rune> _1524_ctorMatch;
        _1524_ctorMatch = DCOMP.__default.escapeName((_1523_ctor).dtor_name);
        Dafny.ISequence<Dafny.Rune> _1525_modulePrefix;
        if (((((c).dtor_enclosingModule))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_module"))) {
          _1525_modulePrefix = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
        } else {
          _1525_modulePrefix = Dafny.Sequence<Dafny.Rune>.Concat((((c).dtor_enclosingModule)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("."));
        }
        Dafny.ISequence<Dafny.Rune> _1526_ctorName;
        _1526_ctorName = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1525_modulePrefix, ((c).dtor_name)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".")), ((_1523_ctor).dtor_name));
        if (((new BigInteger((_1526_ctorName).Count)) >= (new BigInteger(13))) && (((_1526_ctorName).Subsequence(BigInteger.Zero, new BigInteger(13))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_System.Tuple")))) {
          _1526_ctorName = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
        }
        RAST._IExpr _1527_printRhs;
        _1527_printRhs = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("write!(_formatter, \""), _1526_ctorName), (((_1523_ctor).dtor_hasAnyArgs) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(\")?")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\")?")))));
        RAST._IExpr _1528_hashRhs;
        _1528_hashRhs = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
        Dafny.ISequence<RAST._IAssignIdentifier> _1529_coerceRhsArgs;
        _1529_coerceRhsArgs = Dafny.Sequence<RAST._IAssignIdentifier>.FromElements();
        bool _1530_isNumeric;
        _1530_isNumeric = false;
        Dafny.ISequence<Dafny.Rune> _1531_ctorMatchInner;
        _1531_ctorMatchInner = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
        BigInteger _hi21 = new BigInteger(((_1523_ctor).dtor_args).Count);
        for (BigInteger _1532_j = BigInteger.Zero; _1532_j < _hi21; _1532_j++) {
          DAST._IDatatypeDtor _1533_dtor;
          _1533_dtor = ((_1523_ctor).dtor_args).Select(_1532_j);
          Dafny.ISequence<Dafny.Rune> _1534_patternName;
          _1534_patternName = DCOMP.__default.escapeVar(((_1533_dtor).dtor_formal).dtor_name);
          DAST._IType _1535_formalType;
          _1535_formalType = ((_1533_dtor).dtor_formal).dtor_typ;
          if (((_1532_j).Sign == 0) && ((_1534_patternName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")))) {
            _1530_isNumeric = true;
          }
          if (_1530_isNumeric) {
            _1534_patternName = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.GetOr((_1533_dtor).dtor_callName, Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("v"), Std.Strings.__default.OfNat(_1532_j)));
          }
          if ((_1535_formalType).is_Arrow) {
            _1528_hashRhs = (_1528_hashRhs).Then(((RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0"))).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("hash"))).Apply1(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_state"))));
          } else {
            _1528_hashRhs = (_1528_hashRhs).Then((((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("hash"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Hash"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("hash"))).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(_1534_patternName), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_state")))));
          }
          _1531_ctorMatchInner = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1531_ctorMatchInner, _1534_patternName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "));
          if ((_1532_j).Sign == 1) {
            _1527_printRhs = (_1527_printRhs).Then(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("write!(_formatter, \", \")?")));
          }
          _1527_printRhs = (_1527_printRhs).Then(RAST.Expr.create_RawExpr((((_1535_formalType).is_Arrow) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("write!(_formatter, \"<function>\")?")) : (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::dafny_runtime::DafnyPrint::fmt_print("), _1534_patternName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", _formatter, false)?"))))));
          RAST._IExpr _1536_coerceRhsArg = RAST.Expr.Default();
          RAST._IType _1537_formalTpe;
          RAST._IType _out85;
          _out85 = (this).GenType(_1535_formalType, DCOMP.GenTypeContext.@default());
          _1537_formalTpe = _out85;
          DAST._IType _1538_newFormalType;
          _1538_newFormalType = (_1535_formalType).Replace(_1501_coerceMap);
          RAST._IType _1539_newFormalTpe;
          _1539_newFormalTpe = (_1537_formalTpe).ReplaceMap(_1502_rCoerceMap);
          Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _1540_upcastConverter;
          _1540_upcastConverter = (this).UpcastConversionLambda(_1535_formalType, _1537_formalTpe, _1538_newFormalType, _1539_newFormalTpe, _1503_coerceMapToArg);
          if ((_1540_upcastConverter).is_Success) {
            RAST._IExpr _1541_coercionFunction;
            _1541_coercionFunction = (_1540_upcastConverter).dtor_value;
            _1536_coerceRhsArg = (_1541_coercionFunction).Apply1(RAST.Expr.create_Identifier(_1534_patternName));
          } else {
            (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not generate coercion function for contructor "), Std.Strings.__default.OfNat(_1532_j)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" of ")), _1457_datatypeName));
            _1536_coerceRhsArg = (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("todo!"))).Apply1(RAST.Expr.create_LiteralString((this.error).dtor_value, false, false));
          }
          _1529_coerceRhsArgs = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_1529_coerceRhsArgs, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(_1534_patternName, _1536_coerceRhsArg)));
        }
        RAST._IExpr _1542_coerceRhs;
        _1542_coerceRhs = RAST.Expr.create_StructBuild((RAST.Expr.create_Identifier(_1457_datatypeName)).FSel(DCOMP.__default.escapeName((_1523_ctor).dtor_name)), _1529_coerceRhsArgs);
        if (_1530_isNumeric) {
          _1524_ctorMatch = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1524_ctorMatch, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")), _1531_ctorMatchInner), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
        } else {
          _1524_ctorMatch = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1524_ctorMatch, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{")), _1531_ctorMatchInner), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
        }
        if ((_1523_ctor).dtor_hasAnyArgs) {
          _1527_printRhs = (_1527_printRhs).Then(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("write!(_formatter, \")\")?")));
        }
        _1527_printRhs = (_1527_printRhs).Then(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Ok(())")));
        _1519_printImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_1519_printImplBodyCases, Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1457_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), _1524_ctorMatch), RAST.Expr.create_Block(_1527_printRhs))));
        _1520_hashImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_1520_hashImplBodyCases, Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1457_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), _1524_ctorMatch), RAST.Expr.create_Block(_1528_hashRhs))));
        _1521_coerceImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_1521_coerceImplBodyCases, Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_1457_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), _1524_ctorMatch), RAST.Expr.create_Block(_1542_coerceRhs))));
      }
      if (((new BigInteger(((c).dtor_typeParams).Count)).Sign == 1) && ((new BigInteger((_1471_unusedTypeParams).Count)).Sign == 1)) {
        Dafny.ISequence<RAST._IMatchCase> _1543_extraCases;
        _1543_extraCases = Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(_1457_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::_PhantomVariant(..)")), RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{"), (this).UnreachablePanicIfVerified(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}")))));
        _1519_printImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_1519_printImplBodyCases, _1543_extraCases);
        _1520_hashImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_1520_hashImplBodyCases, _1543_extraCases);
        _1521_coerceImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_1521_coerceImplBodyCases, _1543_extraCases);
      }
      Dafny.ISequence<RAST._ITypeParamDecl> _1544_defaultConstrainedTypeParams;
      _1544_defaultConstrainedTypeParams = RAST.TypeParamDecl.AddConstraintsMultiple(_1455_rTypeParamsDecls, Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.DefaultTrait));
      Dafny.ISequence<RAST._ITypeParamDecl> _1545_rTypeParamsDeclsWithEq;
      _1545_rTypeParamsDeclsWithEq = RAST.TypeParamDecl.AddConstraintsMultiple(_1455_rTypeParamsDecls, Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.Eq));
      Dafny.ISequence<RAST._ITypeParamDecl> _1546_rTypeParamsDeclsWithHash;
      _1546_rTypeParamsDeclsWithHash = RAST.TypeParamDecl.AddConstraintsMultiple(_1455_rTypeParamsDecls, Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.Hash));
      RAST._IExpr _1547_printImplBody;
      _1547_printImplBody = RAST.Expr.create_Match(RAST.__default.self, _1519_printImplBodyCases);
      RAST._IExpr _1548_hashImplBody;
      _1548_hashImplBody = RAST.Expr.create_Match(RAST.__default.self, _1520_hashImplBodyCases);
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1455_rTypeParamsDecls, (((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Debug"))).AsType(), RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1457_datatypeName), _1454_rTypeParams), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed, RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("f"), RAST.Type.create_BorrowedMut((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Formatter"))).AsType()))), Std.Wrappers.Option<RAST._IType>.create_Some((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Result"))).AsType()), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyPrint"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt_print"))).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.__default.self, RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("f")), RAST.Expr.create_LiteralBool(true))))))))), RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1455_rTypeParamsDecls, RAST.__default.DafnyPrint, RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1457_datatypeName), _1454_rTypeParams), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt_print"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed, RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_formatter"), RAST.Type.create_BorrowedMut((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Formatter"))).AsType())), RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_in_seq"), RAST.Type.create_Bool())), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("std::fmt::Result"))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1547_printImplBody))))))));
      if ((new BigInteger((_1499_rCoerceTypeParams).Count)).Sign == 1) {
        RAST._IExpr _1549_coerceImplBody;
        _1549_coerceImplBody = RAST.Expr.create_Match(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this")), _1521_coerceImplBodyCases);
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_Impl(_1455_rTypeParamsDecls, RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1457_datatypeName), _1454_rTypeParams), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PUB(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("coerce"), _1499_rCoerceTypeParams, _1500_coerceArguments, Std.Wrappers.Option<RAST._IType>.create_Some(RAST.__default.Rc(RAST.Type.create_ImplType(RAST.Type.create_FnType(Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1457_datatypeName), _1454_rTypeParams)), RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1457_datatypeName), _1498_coerceTypes))))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.__default.RcNew(RAST.Expr.create_Lambda(Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"), RAST.__default.SelfOwned)), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1457_datatypeName), _1498_coerceTypes)), _1549_coerceImplBody))))))))));
      }
      if (_1518_cIsEq) {
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1545_rTypeParamsDeclsWithEq, RAST.__default.Eq, RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1457_datatypeName), _1454_rTypeParams), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<RAST._IImplMember>.FromElements()))));
      }
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1546_rTypeParamsDeclsWithHash, RAST.__default.Hash, RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1457_datatypeName), _1454_rTypeParams), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("hash"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(RAST.TypeParamDecl.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_H"), Dafny.Sequence<RAST._IType>.FromElements((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("hash"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Hasher"))).AsType()))), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed, RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_state"), RAST.Type.create_BorrowedMut(RAST.Type.create_TIdentifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_H"))))), Std.Wrappers.Option<RAST._IType>.create_None(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1548_hashImplBody))))))));
      if ((new BigInteger(((c).dtor_ctors).Count)).Sign == 1) {
        RAST._IExpr _1550_structName;
        _1550_structName = (RAST.Expr.create_Identifier(_1457_datatypeName)).FSel(DCOMP.__default.escapeName((((c).dtor_ctors).Select(BigInteger.Zero)).dtor_name));
        Dafny.ISequence<RAST._IAssignIdentifier> _1551_structAssignments;
        _1551_structAssignments = Dafny.Sequence<RAST._IAssignIdentifier>.FromElements();
        BigInteger _hi22 = new BigInteger(((((c).dtor_ctors).Select(BigInteger.Zero)).dtor_args).Count);
        for (BigInteger _1552_i = BigInteger.Zero; _1552_i < _hi22; _1552_i++) {
          DAST._IDatatypeDtor _1553_dtor;
          _1553_dtor = ((((c).dtor_ctors).Select(BigInteger.Zero)).dtor_args).Select(_1552_i);
          _1551_structAssignments = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_1551_structAssignments, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(DCOMP.__default.escapeVar(((_1553_dtor).dtor_formal).dtor_name), RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::std::default::Default::default()")))));
        }
        Dafny.ISequence<RAST._ITypeParamDecl> _1554_defaultConstrainedTypeParams;
        _1554_defaultConstrainedTypeParams = RAST.TypeParamDecl.AddConstraintsMultiple(_1455_rTypeParamsDecls, Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.DefaultTrait));
        RAST._IType _1555_fullType;
        _1555_fullType = RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_1457_datatypeName), _1454_rTypeParams);
        if (_1518_cIsEq) {
          s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1554_defaultConstrainedTypeParams, RAST.__default.DefaultTrait, _1555_fullType, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("default"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(), Std.Wrappers.Option<RAST._IType>.create_Some(_1555_fullType), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_StructBuild(_1550_structName, _1551_structAssignments)))))))));
        }
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1455_rTypeParamsDecls, ((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("convert"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("AsRef"))).AsType()).Apply1(_1555_fullType), RAST.Type.create_Borrowed(_1555_fullType), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.__default.SelfOwned), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.__default.self))))))));
      }
      return s;
    }
    public static RAST._IPath GenPath(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> p, bool escape)
    {
      RAST._IPath r = RAST.Path.Default();
      if ((new BigInteger((p).Count)).Sign == 0) {
        r = RAST.Path.create_Self();
        return r;
      } else {
        if (((((p).Select(BigInteger.Zero)))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("std"))) {
          r = RAST.Path.create_Global();
        } else if (((((p).Select(BigInteger.Zero)))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_System"))) {
          r = RAST.__default.dafny__runtime;
        } else {
          r = RAST.Path.create_Crate();
        }
        BigInteger _hi23 = new BigInteger((p).Count);
        for (BigInteger _1556_i = BigInteger.Zero; _1556_i < _hi23; _1556_i++) {
          Dafny.ISequence<Dafny.Rune> _1557_name;
          _1557_name = ((p).Select(_1556_i));
          if (escape) {
            _System._ITuple2<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs55 = DafnyCompilerRustUtils.__default.DafnyNameToContainingPathAndName(_1557_name, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements());
            Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1558_modules = _let_tmp_rhs55.dtor__0;
            Dafny.ISequence<Dafny.Rune> _1559_finalName = _let_tmp_rhs55.dtor__1;
            BigInteger _hi24 = new BigInteger((_1558_modules).Count);
            for (BigInteger _1560_j = BigInteger.Zero; _1560_j < _hi24; _1560_j++) {
              r = (r).MSel(DCOMP.__default.escapeName(((_1558_modules).Select(_1560_j))));
            }
            r = (r).MSel(DCOMP.__default.escapeName(_1559_finalName));
          } else {
            r = (r).MSel(DCOMP.__default.ReplaceDotByDoubleColon((_1557_name)));
          }
        }
      }
      return r;
    }
    public static RAST._IType GenPathType(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> p)
    {
      RAST._IType t = RAST.Type.Default();
      RAST._IPath _1561_p;
      RAST._IPath _out86;
      _out86 = DCOMP.COMP.GenPath(p, true);
      _1561_p = _out86;
      t = (_1561_p).AsType();
      return t;
    }
    public static RAST._IExpr GenPathExpr(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> p, bool escape)
    {
      RAST._IExpr e = RAST.Expr.Default();
      if ((new BigInteger((p).Count)).Sign == 0) {
        e = RAST.__default.self;
        return e;
      }
      RAST._IPath _1562_p;
      RAST._IPath _out87;
      _out87 = DCOMP.COMP.GenPath(p, escape);
      _1562_p = _out87;
      e = (_1562_p).AsExpr();
      return e;
    }
    public Dafny.ISequence<RAST._IType> GenTypeArgs(Dafny.ISequence<DAST._IType> args, bool genTypeContext)
    {
      Dafny.ISequence<RAST._IType> s = Dafny.Sequence<RAST._IType>.Empty;
      s = Dafny.Sequence<RAST._IType>.FromElements();
      BigInteger _hi25 = new BigInteger((args).Count);
      for (BigInteger _1563_i = BigInteger.Zero; _1563_i < _hi25; _1563_i++) {
        RAST._IType _1564_genTp;
        RAST._IType _out88;
        _out88 = (this).GenType((args).Select(_1563_i), genTypeContext);
        _1564_genTp = _out88;
        s = Dafny.Sequence<RAST._IType>.Concat(s, Dafny.Sequence<RAST._IType>.FromElements(_1564_genTp));
      }
      return s;
    }
    public bool IsRcWrapped(Dafny.ISequence<DAST._IAttribute> attributes) {
      return ((!(attributes).Contains(DAST.Attribute.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("auto-nongrowing-size"), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements()))) && (!(attributes).Contains(DAST.Attribute.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("rust_rc"), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("false")))))) || ((attributes).Contains(DAST.Attribute.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("rust_rc"), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("true")))));
    }
    public RAST._IType GenType(DAST._IType c, bool genTypeContext)
    {
      RAST._IType s = RAST.Type.Default();
      DAST._IType _source77 = c;
      {
        if (_source77.is_UserDefined) {
          DAST._IResolvedType _1565_resolved = _source77.dtor_resolved;
          {
            RAST._IType _1566_t;
            RAST._IType _out89;
            _out89 = DCOMP.COMP.GenPathType((_1565_resolved).dtor_path);
            _1566_t = _out89;
            Dafny.ISequence<RAST._IType> _1567_typeArgs;
            Dafny.ISequence<RAST._IType> _out90;
            _out90 = (this).GenTypeArgs((_1565_resolved).dtor_typeArgs, false);
            _1567_typeArgs = _out90;
            s = RAST.Type.create_TypeApp(_1566_t, _1567_typeArgs);
            DAST._IResolvedTypeBase _source78 = (_1565_resolved).dtor_kind;
            {
              if (_source78.is_Class) {
                {
                  s = (this).Object(s);
                }
                goto after_match10;
              }
            }
            {
              if (_source78.is_Datatype) {
                {
                  if ((this).IsRcWrapped((_1565_resolved).dtor_attributes)) {
                    s = RAST.__default.Rc(s);
                  }
                }
                goto after_match10;
              }
            }
            {
              if (_source78.is_Trait) {
                {
                  if (((_1565_resolved).dtor_path).Equals(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_System"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("object")))) {
                    s = RAST.__default.AnyTrait;
                  }
                  if (!((genTypeContext))) {
                    s = (this).Object(RAST.Type.create_DynType(s));
                  }
                }
                goto after_match10;
              }
            }
            {
              DAST._IType _1568_base = _source78.dtor_baseType;
              DAST._INewtypeRange _1569_range = _source78.dtor_range;
              bool _1570_erased = _source78.dtor_erase;
              {
                if (_1570_erased) {
                  Std.Wrappers._IOption<RAST._IType> _source79 = DCOMP.COMP.NewtypeRangeToRustType(_1569_range);
                  {
                    if (_source79.is_Some) {
                      RAST._IType _1571_v = _source79.dtor_value;
                      s = _1571_v;
                      goto after_match11;
                    }
                  }
                  {
                    RAST._IType _1572_underlying;
                    RAST._IType _out91;
                    _out91 = (this).GenType(_1568_base, DCOMP.GenTypeContext.@default());
                    _1572_underlying = _out91;
                    s = RAST.Type.create_TSynonym(s, _1572_underlying);
                  }
                after_match11: ;
                }
              }
            }
          after_match10: ;
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_Object) {
          {
            s = RAST.__default.AnyTrait;
            if (!((genTypeContext))) {
              s = (this).Object(RAST.Type.create_DynType(s));
            }
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_Tuple) {
          Dafny.ISequence<DAST._IType> _1573_types = _source77.dtor_Tuple_a0;
          {
            Dafny.ISequence<RAST._IType> _1574_args;
            _1574_args = Dafny.Sequence<RAST._IType>.FromElements();
            BigInteger _1575_i;
            _1575_i = BigInteger.Zero;
            while ((_1575_i) < (new BigInteger((_1573_types).Count))) {
              RAST._IType _1576_generated;
              RAST._IType _out92;
              _out92 = (this).GenType((_1573_types).Select(_1575_i), false);
              _1576_generated = _out92;
              _1574_args = Dafny.Sequence<RAST._IType>.Concat(_1574_args, Dafny.Sequence<RAST._IType>.FromElements(_1576_generated));
              _1575_i = (_1575_i) + (BigInteger.One);
            }
            if ((new BigInteger((_1573_types).Count)) <= (RAST.__default.MAX__TUPLE__SIZE)) {
              s = RAST.Type.create_TupleType(_1574_args);
            } else {
              s = RAST.__default.SystemTupleType(_1574_args);
            }
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_Array) {
          DAST._IType _1577_element = _source77.dtor_element;
          BigInteger _1578_dims = _source77.dtor_dims;
          {
            if ((_1578_dims) > (new BigInteger(16))) {
              s = RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<i>Array of dimensions greater than 16</i>"));
            } else {
              RAST._IType _1579_elem;
              RAST._IType _out93;
              _out93 = (this).GenType(_1577_element, false);
              _1579_elem = _out93;
              if ((_1578_dims) == (BigInteger.One)) {
                s = RAST.Type.create_Array(_1579_elem, Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None());
                s = (this).Object(s);
              } else {
                Dafny.ISequence<Dafny.Rune> _1580_n;
                _1580_n = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Array"), Std.Strings.__default.OfNat(_1578_dims));
                s = (((RAST.__default.dafny__runtime).MSel(_1580_n)).AsType()).Apply(Dafny.Sequence<RAST._IType>.FromElements(_1579_elem));
                s = (this).Object(s);
              }
            }
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_Seq) {
          DAST._IType _1581_element = _source77.dtor_element;
          {
            RAST._IType _1582_elem;
            RAST._IType _out94;
            _out94 = (this).GenType(_1581_element, false);
            _1582_elem = _out94;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Sequence"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_1582_elem));
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_Set) {
          DAST._IType _1583_element = _source77.dtor_element;
          {
            RAST._IType _1584_elem;
            RAST._IType _out95;
            _out95 = (this).GenType(_1583_element, false);
            _1584_elem = _out95;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Set"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_1584_elem));
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_Multiset) {
          DAST._IType _1585_element = _source77.dtor_element;
          {
            RAST._IType _1586_elem;
            RAST._IType _out96;
            _out96 = (this).GenType(_1585_element, false);
            _1586_elem = _out96;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Multiset"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_1586_elem));
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_Map) {
          DAST._IType _1587_key = _source77.dtor_key;
          DAST._IType _1588_value = _source77.dtor_value;
          {
            RAST._IType _1589_keyType;
            RAST._IType _out97;
            _out97 = (this).GenType(_1587_key, false);
            _1589_keyType = _out97;
            RAST._IType _1590_valueType;
            RAST._IType _out98;
            _out98 = (this).GenType(_1588_value, genTypeContext);
            _1590_valueType = _out98;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Map"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_1589_keyType, _1590_valueType));
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_MapBuilder) {
          DAST._IType _1591_key = _source77.dtor_key;
          DAST._IType _1592_value = _source77.dtor_value;
          {
            RAST._IType _1593_keyType;
            RAST._IType _out99;
            _out99 = (this).GenType(_1591_key, false);
            _1593_keyType = _out99;
            RAST._IType _1594_valueType;
            RAST._IType _out100;
            _out100 = (this).GenType(_1592_value, genTypeContext);
            _1594_valueType = _out100;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("MapBuilder"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_1593_keyType, _1594_valueType));
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_SetBuilder) {
          DAST._IType _1595_elem = _source77.dtor_element;
          {
            RAST._IType _1596_elemType;
            RAST._IType _out101;
            _out101 = (this).GenType(_1595_elem, false);
            _1596_elemType = _out101;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("SetBuilder"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_1596_elemType));
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_Arrow) {
          Dafny.ISequence<DAST._IType> _1597_args = _source77.dtor_args;
          DAST._IType _1598_result = _source77.dtor_result;
          {
            Dafny.ISequence<RAST._IType> _1599_argTypes;
            _1599_argTypes = Dafny.Sequence<RAST._IType>.FromElements();
            BigInteger _1600_i;
            _1600_i = BigInteger.Zero;
            while ((_1600_i) < (new BigInteger((_1597_args).Count))) {
              RAST._IType _1601_generated;
              RAST._IType _out102;
              _out102 = (this).GenType((_1597_args).Select(_1600_i), false);
              _1601_generated = _out102;
              _1599_argTypes = Dafny.Sequence<RAST._IType>.Concat(_1599_argTypes, Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_Borrowed(_1601_generated)));
              _1600_i = (_1600_i) + (BigInteger.One);
            }
            RAST._IType _1602_resultType;
            RAST._IType _out103;
            _out103 = (this).GenType(_1598_result, DCOMP.GenTypeContext.@default());
            _1602_resultType = _out103;
            s = RAST.__default.Rc(RAST.Type.create_DynType(RAST.Type.create_FnType(_1599_argTypes, _1602_resultType)));
          }
          goto after_match9;
        }
      }
      {
        if (_source77.is_TypeArg) {
          Dafny.ISequence<Dafny.Rune> _h90 = _source77.dtor_TypeArg_a0;
          Dafny.ISequence<Dafny.Rune> _1603_name = _h90;
          s = RAST.Type.create_TIdentifier(DCOMP.__default.escapeName(_1603_name));
          goto after_match9;
        }
      }
      {
        if (_source77.is_Primitive) {
          DAST._IPrimitive _1604_p = _source77.dtor_Primitive_a0;
          {
            DAST._IPrimitive _source80 = _1604_p;
            {
              if (_source80.is_Int) {
                s = ((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyInt"))).AsType();
                goto after_match12;
              }
            }
            {
              if (_source80.is_Real) {
                s = ((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("BigRational"))).AsType();
                goto after_match12;
              }
            }
            {
              if (_source80.is_String) {
                s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Sequence"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsType()));
                goto after_match12;
              }
            }
            {
              if (_source80.is_Bool) {
                s = RAST.Type.create_Bool();
                goto after_match12;
              }
            }
            {
              s = ((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsType();
            }
          after_match12: ;
          }
          goto after_match9;
        }
      }
      {
        Dafny.ISequence<Dafny.Rune> _1605_v = _source77.dtor_Passthrough_a0;
        s = RAST.__default.RawType(_1605_v);
      }
    after_match9: ;
      return s;
    }
    public bool EnclosingIsTrait(DAST._IType tpe) {
      return ((tpe).is_UserDefined) && ((((tpe).dtor_resolved).dtor_kind).is_Trait);
    }
    public void GenClassImplBody(Dafny.ISequence<DAST._IMethod> body, bool forTrait, DAST._IType enclosingType, Dafny.ISequence<DAST._IType> enclosingTypeParams, out Dafny.ISequence<RAST._IImplMember> s, out Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> traitBodies)
    {
      s = Dafny.Sequence<RAST._IImplMember>.Empty;
      traitBodies = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.Empty;
      s = Dafny.Sequence<RAST._IImplMember>.FromElements();
      traitBodies = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.FromElements();
      BigInteger _hi26 = new BigInteger((body).Count);
      for (BigInteger _1606_i = BigInteger.Zero; _1606_i < _hi26; _1606_i++) {
        DAST._IMethod _source81 = (body).Select(_1606_i);
        {
          DAST._IMethod _1607_m = _source81;
          {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> _source82 = (_1607_m).dtor_overridingPath;
            {
              if (_source82.is_Some) {
                Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1608_p = _source82.dtor_value;
                {
                  Dafny.ISequence<RAST._IImplMember> _1609_existing;
                  _1609_existing = Dafny.Sequence<RAST._IImplMember>.FromElements();
                  if ((traitBodies).Contains(_1608_p)) {
                    _1609_existing = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.Select(traitBodies,_1608_p);
                  }
                  if (((new BigInteger(((_1607_m).dtor_typeParams).Count)).Sign == 1) && ((this).EnclosingIsTrait(enclosingType))) {
                    (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Error: Rust does not support method with generic type parameters in traits"));
                  }
                  RAST._IImplMember _1610_genMethod;
                  RAST._IImplMember _out104;
                  _out104 = (this).GenMethod(_1607_m, true, enclosingType, enclosingTypeParams);
                  _1610_genMethod = _out104;
                  _1609_existing = Dafny.Sequence<RAST._IImplMember>.Concat(_1609_existing, Dafny.Sequence<RAST._IImplMember>.FromElements(_1610_genMethod));
                  traitBodies = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.Merge(traitBodies, Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.FromElements(new Dafny.Pair<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>(_1608_p, _1609_existing)));
                }
                goto after_match14;
              }
            }
            {
              {
                RAST._IImplMember _1611_generated;
                RAST._IImplMember _out105;
                _out105 = (this).GenMethod(_1607_m, forTrait, enclosingType, enclosingTypeParams);
                _1611_generated = _out105;
                s = Dafny.Sequence<RAST._IImplMember>.Concat(s, Dafny.Sequence<RAST._IImplMember>.FromElements(_1611_generated));
              }
            }
          after_match14: ;
          }
        }
      after_match13: ;
      }
    }
    public Dafny.ISequence<RAST._IFormal> GenParams(Dafny.ISequence<DAST._IFormal> @params, bool forLambda)
    {
      Dafny.ISequence<RAST._IFormal> s = Dafny.Sequence<RAST._IFormal>.Empty;
      s = Dafny.Sequence<RAST._IFormal>.FromElements();
      BigInteger _hi27 = new BigInteger((@params).Count);
      for (BigInteger _1612_i = BigInteger.Zero; _1612_i < _hi27; _1612_i++) {
        DAST._IFormal _1613_param;
        _1613_param = (@params).Select(_1612_i);
        RAST._IType _1614_paramType;
        RAST._IType _out106;
        _out106 = (this).GenType((_1613_param).dtor_typ, DCOMP.GenTypeContext.@default());
        _1614_paramType = _out106;
        if (((!((_1614_paramType).CanReadWithoutClone())) || (forLambda)) && (!((_1613_param).dtor_attributes).Contains(DCOMP.__default.AttributeOwned))) {
          _1614_paramType = RAST.Type.create_Borrowed(_1614_paramType);
        }
        s = Dafny.Sequence<RAST._IFormal>.Concat(s, Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.create(DCOMP.__default.escapeVar((_1613_param).dtor_name), _1614_paramType)));
      }
      return s;
    }
    public RAST._IImplMember GenMethod(DAST._IMethod m, bool forTrait, DAST._IType enclosingType, Dafny.ISequence<DAST._IType> enclosingTypeParams)
    {
      RAST._IImplMember s = RAST.ImplMember.Default();
      Dafny.ISequence<RAST._IFormal> _1615_params;
      Dafny.ISequence<RAST._IFormal> _out107;
      _out107 = (this).GenParams((m).dtor_params, false);
      _1615_params = _out107;
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1616_paramNames;
      _1616_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
      Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> _1617_paramTypes;
      _1617_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.FromElements();
      BigInteger _hi28 = new BigInteger(((m).dtor_params).Count);
      for (BigInteger _1618_paramI = BigInteger.Zero; _1618_paramI < _hi28; _1618_paramI++) {
        DAST._IFormal _1619_dafny__formal;
        _1619_dafny__formal = ((m).dtor_params).Select(_1618_paramI);
        RAST._IFormal _1620_formal;
        _1620_formal = (_1615_params).Select(_1618_paramI);
        Dafny.ISequence<Dafny.Rune> _1621_name;
        _1621_name = (_1620_formal).dtor_name;
        _1616_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_1616_paramNames, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_1621_name));
        _1617_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_1617_paramTypes, _1621_name, (_1620_formal).dtor_tpe);
      }
      Dafny.ISequence<Dafny.Rune> _1622_fnName;
      _1622_fnName = DCOMP.__default.escapeName((m).dtor_name);
      DCOMP._ISelfInfo _1623_selfIdent;
      _1623_selfIdent = DCOMP.SelfInfo.create_NoSelf();
      if (!((m).dtor_isStatic)) {
        Dafny.ISequence<Dafny.Rune> _1624_selfId;
        _1624_selfId = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self");
        if ((m).dtor_outVarsAreUninitFieldsToAssign) {
          _1624_selfId = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this");
        }
        var _pat_let_tv2 = enclosingTypeParams;
        DAST._IType _1625_instanceType;
        DAST._IType _source83 = enclosingType;
        {
          if (_source83.is_UserDefined) {
            DAST._IResolvedType _1626_r = _source83.dtor_resolved;
            _1625_instanceType = DAST.Type.create_UserDefined(Dafny.Helpers.Let<DAST._IResolvedType, DAST._IResolvedType>(_1626_r, _pat_let20_0 => Dafny.Helpers.Let<DAST._IResolvedType, DAST._IResolvedType>(_pat_let20_0, _1627_dt__update__tmp_h0 => Dafny.Helpers.Let<Dafny.ISequence<DAST._IType>, DAST._IResolvedType>(_pat_let_tv2, _pat_let21_0 => Dafny.Helpers.Let<Dafny.ISequence<DAST._IType>, DAST._IResolvedType>(_pat_let21_0, _1628_dt__update_htypeArgs_h0 => DAST.ResolvedType.create((_1627_dt__update__tmp_h0).dtor_path, _1628_dt__update_htypeArgs_h0, (_1627_dt__update__tmp_h0).dtor_kind, (_1627_dt__update__tmp_h0).dtor_attributes, (_1627_dt__update__tmp_h0).dtor_properMethods, (_1627_dt__update__tmp_h0).dtor_extendedTypes))))));
            goto after_match15;
          }
        }
        {
          _1625_instanceType = enclosingType;
        }
      after_match15: ;
        if (forTrait) {
          RAST._IFormal _1629_selfFormal;
          if ((m).dtor_wasFunction) {
            _1629_selfFormal = RAST.Formal.selfBorrowed;
          } else {
            _1629_selfFormal = RAST.Formal.selfBorrowedMut;
          }
          _1615_params = Dafny.Sequence<RAST._IFormal>.Concat(Dafny.Sequence<RAST._IFormal>.FromElements(_1629_selfFormal), _1615_params);
        } else {
          RAST._IType _1630_tpe;
          RAST._IType _out108;
          _out108 = (this).GenType(_1625_instanceType, DCOMP.GenTypeContext.@default());
          _1630_tpe = _out108;
          if ((_1624_selfId).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))) {
            if (((this).ObjectType).is_RcMut) {
              _1630_tpe = RAST.Type.create_Borrowed(_1630_tpe);
            }
          } else if ((_1624_selfId).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))) {
            if ((_1630_tpe).IsObjectOrPointer()) {
              if ((m).dtor_wasFunction) {
                _1630_tpe = RAST.__default.SelfBorrowed;
              } else {
                _1630_tpe = RAST.__default.SelfBorrowedMut;
              }
            } else {
              if ((((enclosingType).is_UserDefined) && ((((enclosingType).dtor_resolved).dtor_kind).is_Datatype)) && ((this).IsRcWrapped(((enclosingType).dtor_resolved).dtor_attributes))) {
                _1630_tpe = RAST.Type.create_Borrowed(RAST.__default.Rc(RAST.__default.SelfOwned));
              } else {
                _1630_tpe = RAST.Type.create_Borrowed(RAST.__default.SelfOwned);
              }
            }
          }
          _1615_params = Dafny.Sequence<RAST._IFormal>.Concat(Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.create(_1624_selfId, _1630_tpe)), _1615_params);
        }
        _1623_selfIdent = DCOMP.SelfInfo.create_ThisTyped(_1624_selfId, _1625_instanceType);
      }
      Dafny.ISequence<RAST._IType> _1631_retTypeArgs;
      _1631_retTypeArgs = Dafny.Sequence<RAST._IType>.FromElements();
      BigInteger _1632_typeI;
      _1632_typeI = BigInteger.Zero;
      while ((_1632_typeI) < (new BigInteger(((m).dtor_outTypes).Count))) {
        RAST._IType _1633_typeExpr;
        RAST._IType _out109;
        _out109 = (this).GenType(((m).dtor_outTypes).Select(_1632_typeI), DCOMP.GenTypeContext.@default());
        _1633_typeExpr = _out109;
        _1631_retTypeArgs = Dafny.Sequence<RAST._IType>.Concat(_1631_retTypeArgs, Dafny.Sequence<RAST._IType>.FromElements(_1633_typeExpr));
        _1632_typeI = (_1632_typeI) + (BigInteger.One);
      }
      RAST._IVisibility _1634_visibility;
      if (forTrait) {
        _1634_visibility = RAST.Visibility.create_PRIV();
      } else {
        _1634_visibility = RAST.Visibility.create_PUB();
      }
      Dafny.ISequence<DAST._ITypeArgDecl> _1635_typeParamsFiltered;
      _1635_typeParamsFiltered = Dafny.Sequence<DAST._ITypeArgDecl>.FromElements();
      BigInteger _hi29 = new BigInteger(((m).dtor_typeParams).Count);
      for (BigInteger _1636_typeParamI = BigInteger.Zero; _1636_typeParamI < _hi29; _1636_typeParamI++) {
        DAST._ITypeArgDecl _1637_typeParam;
        _1637_typeParam = ((m).dtor_typeParams).Select(_1636_typeParamI);
        if (!((enclosingTypeParams).Contains(DAST.Type.create_TypeArg((_1637_typeParam).dtor_name)))) {
          _1635_typeParamsFiltered = Dafny.Sequence<DAST._ITypeArgDecl>.Concat(_1635_typeParamsFiltered, Dafny.Sequence<DAST._ITypeArgDecl>.FromElements(_1637_typeParam));
        }
      }
      Dafny.ISequence<RAST._ITypeParamDecl> _1638_typeParams;
      _1638_typeParams = Dafny.Sequence<RAST._ITypeParamDecl>.FromElements();
      if ((new BigInteger((_1635_typeParamsFiltered).Count)).Sign == 1) {
        BigInteger _hi30 = new BigInteger((_1635_typeParamsFiltered).Count);
        for (BigInteger _1639_i = BigInteger.Zero; _1639_i < _hi30; _1639_i++) {
          DAST._IType _1640_typeArg;
          RAST._ITypeParamDecl _1641_rTypeParamDecl;
          DAST._IType _out110;
          RAST._ITypeParamDecl _out111;
          (this).GenTypeParam((_1635_typeParamsFiltered).Select(_1639_i), out _out110, out _out111);
          _1640_typeArg = _out110;
          _1641_rTypeParamDecl = _out111;
          RAST._ITypeParamDecl _1642_dt__update__tmp_h1 = _1641_rTypeParamDecl;
          Dafny.ISequence<RAST._IType> _1643_dt__update_hconstraints_h0 = (_1641_rTypeParamDecl).dtor_constraints;
          _1641_rTypeParamDecl = RAST.TypeParamDecl.create((_1642_dt__update__tmp_h1).dtor_name, _1643_dt__update_hconstraints_h0);
          _1638_typeParams = Dafny.Sequence<RAST._ITypeParamDecl>.Concat(_1638_typeParams, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(_1641_rTypeParamDecl));
        }
      }
      Std.Wrappers._IOption<RAST._IExpr> _1644_fBody = Std.Wrappers.Option<RAST._IExpr>.Default();
      DCOMP._IEnvironment _1645_env = DCOMP.Environment.Default();
      RAST._IExpr _1646_preBody;
      _1646_preBody = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1647_preAssignNames;
      _1647_preAssignNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
      Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> _1648_preAssignTypes;
      _1648_preAssignTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.FromElements();
      if ((m).dtor_hasBody) {
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> _1649_earlyReturn;
        _1649_earlyReturn = Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_None();
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> _source84 = (m).dtor_outVars;
        {
          if (_source84.is_Some) {
            Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1650_outVars = _source84.dtor_value;
            {
              if ((m).dtor_outVarsAreUninitFieldsToAssign) {
                _1649_earlyReturn = Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_Some(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements());
                BigInteger _hi31 = new BigInteger((_1650_outVars).Count);
                for (BigInteger _1651_outI = BigInteger.Zero; _1651_outI < _hi31; _1651_outI++) {
                  Dafny.ISequence<Dafny.Rune> _1652_outVar;
                  _1652_outVar = (_1650_outVars).Select(_1651_outI);
                  Dafny.ISequence<Dafny.Rune> _1653_outName;
                  _1653_outName = DCOMP.__default.escapeVar(_1652_outVar);
                  Dafny.ISequence<Dafny.Rune> _1654_tracker__name;
                  _1654_tracker__name = DCOMP.__default.AddAssignedPrefix(_1653_outName);
                  _1647_preAssignNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_1647_preAssignNames, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_1654_tracker__name));
                  _1648_preAssignTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_1648_preAssignTypes, _1654_tracker__name, RAST.Type.create_Bool());
                  _1646_preBody = (_1646_preBody).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _1654_tracker__name, Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Bool()), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_LiteralBool(false))));
                }
              } else {
                Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1655_tupleArgs;
                _1655_tupleArgs = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
                BigInteger _hi32 = new BigInteger((_1650_outVars).Count);
                for (BigInteger _1656_outI = BigInteger.Zero; _1656_outI < _hi32; _1656_outI++) {
                  Dafny.ISequence<Dafny.Rune> _1657_outVar;
                  _1657_outVar = (_1650_outVars).Select(_1656_outI);
                  RAST._IType _1658_outType;
                  RAST._IType _out112;
                  _out112 = (this).GenType(((m).dtor_outTypes).Select(_1656_outI), DCOMP.GenTypeContext.@default());
                  _1658_outType = _out112;
                  Dafny.ISequence<Dafny.Rune> _1659_outName;
                  _1659_outName = DCOMP.__default.escapeVar(_1657_outVar);
                  _1616_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_1616_paramNames, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_1659_outName));
                  RAST._IType _1660_outMaybeType;
                  if ((_1658_outType).CanReadWithoutClone()) {
                    _1660_outMaybeType = _1658_outType;
                  } else {
                    _1660_outMaybeType = RAST.__default.MaybePlaceboType(_1658_outType);
                  }
                  _1617_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_1617_paramTypes, _1659_outName, _1660_outMaybeType);
                  _1655_tupleArgs = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_1655_tupleArgs, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_1659_outName));
                }
                _1649_earlyReturn = Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_Some(_1655_tupleArgs);
              }
            }
            goto after_match16;
          }
        }
        {
        }
      after_match16: ;
        _1645_env = DCOMP.Environment.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_1647_preAssignNames, _1616_paramNames), Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Merge(_1648_preAssignTypes, _1617_paramTypes));
        RAST._IExpr _1661_body;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1662___v71;
        DCOMP._IEnvironment _1663___v72;
        RAST._IExpr _out113;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out114;
        DCOMP._IEnvironment _out115;
        (this).GenStmts((m).dtor_body, _1623_selfIdent, _1645_env, true, _1649_earlyReturn, out _out113, out _out114, out _out115);
        _1661_body = _out113;
        _1662___v71 = _out114;
        _1663___v72 = _out115;
        _1644_fBody = Std.Wrappers.Option<RAST._IExpr>.create_Some((_1646_preBody).Then(_1661_body));
      } else {
        _1645_env = DCOMP.Environment.create(_1616_paramNames, _1617_paramTypes);
        _1644_fBody = Std.Wrappers.Option<RAST._IExpr>.create_None();
      }
      s = RAST.ImplMember.create_FnDecl(_1634_visibility, RAST.Fn.create(_1622_fnName, _1638_typeParams, _1615_params, Std.Wrappers.Option<RAST._IType>.create_Some((((new BigInteger((_1631_retTypeArgs).Count)) == (BigInteger.One)) ? ((_1631_retTypeArgs).Select(BigInteger.Zero)) : (RAST.Type.create_TupleType(_1631_retTypeArgs)))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), _1644_fBody));
      return s;
    }
    public void GenStmts(Dafny.ISequence<DAST._IStatement> stmts, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, bool isLast, Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> earlyReturn, out RAST._IExpr generated, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents, out DCOMP._IEnvironment newEnv)
    {
      generated = RAST.Expr.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      newEnv = DCOMP.Environment.Default();
      generated = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1664_declarations;
      _1664_declarations = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      BigInteger _1665_i;
      _1665_i = BigInteger.Zero;
      newEnv = env;
      Dafny.ISequence<DAST._IStatement> _1666_stmts;
      _1666_stmts = stmts;
      while ((_1665_i) < (new BigInteger((_1666_stmts).Count))) {
        DAST._IStatement _1667_stmt;
        _1667_stmt = (_1666_stmts).Select(_1665_i);
        DAST._IStatement _source85 = _1667_stmt;
        {
          if (_source85.is_DeclareVar) {
            Dafny.ISequence<Dafny.Rune> _1668_name = _source85.dtor_name;
            DAST._IType _1669_optType = _source85.dtor_typ;
            Std.Wrappers._IOption<DAST._IExpression> maybeValue0 = _source85.dtor_maybeValue;
            if (maybeValue0.is_None) {
              if (((_1665_i) + (BigInteger.One)) < (new BigInteger((_1666_stmts).Count))) {
                DAST._IStatement _source86 = (_1666_stmts).Select((_1665_i) + (BigInteger.One));
                {
                  if (_source86.is_Assign) {
                    DAST._IAssignLhs lhs0 = _source86.dtor_lhs;
                    if (lhs0.is_Ident) {
                      Dafny.ISequence<Dafny.Rune> _1670_name2 = lhs0.dtor_ident;
                      DAST._IExpression _1671_rhs = _source86.dtor_value;
                      if (object.Equals(_1670_name2, _1668_name)) {
                        _1666_stmts = Dafny.Sequence<DAST._IStatement>.Concat(Dafny.Sequence<DAST._IStatement>.Concat((_1666_stmts).Subsequence(BigInteger.Zero, _1665_i), Dafny.Sequence<DAST._IStatement>.FromElements(DAST.Statement.create_DeclareVar(_1668_name, _1669_optType, Std.Wrappers.Option<DAST._IExpression>.create_Some(_1671_rhs)))), (_1666_stmts).Drop((_1665_i) + (new BigInteger(2))));
                        _1667_stmt = (_1666_stmts).Select(_1665_i);
                      }
                      goto after_match18;
                    }
                  }
                }
                {
                }
              after_match18: ;
              }
              goto after_match17;
            }
          }
        }
        {
        }
      after_match17: ;
        RAST._IExpr _1672_stmtExpr;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1673_recIdents;
        DCOMP._IEnvironment _1674_newEnv2;
        RAST._IExpr _out116;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out117;
        DCOMP._IEnvironment _out118;
        (this).GenStmt(_1667_stmt, selfIdent, newEnv, (isLast) && ((_1665_i) == ((new BigInteger((_1666_stmts).Count)) - (BigInteger.One))), earlyReturn, out _out116, out _out117, out _out118);
        _1672_stmtExpr = _out116;
        _1673_recIdents = _out117;
        _1674_newEnv2 = _out118;
        newEnv = _1674_newEnv2;
        DAST._IStatement _source87 = _1667_stmt;
        {
          if (_source87.is_DeclareVar) {
            Dafny.ISequence<Dafny.Rune> _1675_name = _source87.dtor_name;
            {
              _1664_declarations = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_1664_declarations, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(DCOMP.__default.escapeVar(_1675_name)));
            }
            goto after_match19;
          }
        }
        {
        }
      after_match19: ;
        readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_1673_recIdents, _1664_declarations));
        generated = (generated).Then(_1672_stmtExpr);
        _1665_i = (_1665_i) + (BigInteger.One);
        if ((_1672_stmtExpr).is_Return) {
          goto after_0;
        }
      continue_0: ;
      }
    after_0: ;
    }
    public void GenAssignLhs(DAST._IAssignLhs lhs, RAST._IExpr rhs, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, out RAST._IExpr generated, out bool needsIIFE, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents, out DCOMP._IEnvironment newEnv)
    {
      generated = RAST.Expr.Default();
      needsIIFE = false;
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      newEnv = DCOMP.Environment.Default();
      newEnv = env;
      DAST._IAssignLhs _source88 = lhs;
      {
        if (_source88.is_Ident) {
          Dafny.ISequence<Dafny.Rune> _1676_id = _source88.dtor_ident;
          {
            Dafny.ISequence<Dafny.Rune> _1677_idRust;
            _1677_idRust = DCOMP.__default.escapeVar(_1676_id);
            if (((env).IsBorrowed(_1677_idRust)) || ((env).IsBorrowedMut(_1677_idRust))) {
              generated = RAST.__default.AssignVar(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"), _1677_idRust), rhs);
            } else {
              generated = RAST.__default.AssignVar(_1677_idRust, rhs);
            }
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_1677_idRust);
            needsIIFE = false;
          }
          goto after_match20;
        }
      }
      {
        if (_source88.is_Select) {
          DAST._IExpression _1678_on = _source88.dtor_expr;
          Dafny.ISequence<Dafny.Rune> _1679_field = _source88.dtor_field;
          {
            Dafny.ISequence<Dafny.Rune> _1680_fieldName;
            _1680_fieldName = DCOMP.__default.escapeVar(_1679_field);
            RAST._IExpr _1681_onExpr;
            DCOMP._IOwnership _1682_onOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1683_recIdents;
            RAST._IExpr _out119;
            DCOMP._IOwnership _out120;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out121;
            (this).GenExpr(_1678_on, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out119, out _out120, out _out121);
            _1681_onExpr = _out119;
            _1682_onOwned = _out120;
            _1683_recIdents = _out121;
            RAST._IExpr _source89 = _1681_onExpr;
            {
              bool disjunctiveMatch11 = false;
              if (_source89.is_Call) {
                RAST._IExpr obj2 = _source89.dtor_obj;
                if (obj2.is_Select) {
                  RAST._IExpr obj3 = obj2.dtor_obj;
                  if (obj3.is_Identifier) {
                    Dafny.ISequence<Dafny.Rune> name4 = obj3.dtor_name;
                    if (object.Equals(name4, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))) {
                      Dafny.ISequence<Dafny.Rune> name5 = obj2.dtor_name;
                      if (object.Equals(name5, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("clone"))) {
                        disjunctiveMatch11 = true;
                      }
                    }
                  }
                }
              }
              if (_source89.is_Identifier) {
                Dafny.ISequence<Dafny.Rune> name6 = _source89.dtor_name;
                if (object.Equals(name6, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))) {
                  disjunctiveMatch11 = true;
                }
              }
              if (_source89.is_UnaryOp) {
                Dafny.ISequence<Dafny.Rune> op14 = _source89.dtor_op1;
                if (object.Equals(op14, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"))) {
                  RAST._IExpr underlying4 = _source89.dtor_underlying;
                  if (underlying4.is_Identifier) {
                    Dafny.ISequence<Dafny.Rune> name7 = underlying4.dtor_name;
                    if (object.Equals(name7, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))) {
                      disjunctiveMatch11 = true;
                    }
                  }
                }
              }
              if (disjunctiveMatch11) {
                Dafny.ISequence<Dafny.Rune> _1684_isAssignedVar;
                _1684_isAssignedVar = DCOMP.__default.AddAssignedPrefix(_1680_fieldName);
                if (((newEnv).dtor_names).Contains(_1684_isAssignedVar)) {
                  generated = (((RAST.__default.dafny__runtime).MSel((this).update__field__uninit__macro)).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements((this).thisInConstructor, RAST.Expr.create_Identifier(_1680_fieldName), RAST.Expr.create_Identifier(_1684_isAssignedVar), rhs));
                  newEnv = (newEnv).RemoveAssigned(_1684_isAssignedVar);
                } else {
                  (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Unespected field to assign whose isAssignedVar is not in the environment: "), _1684_isAssignedVar));
                  generated = RAST.__default.AssignMember(RAST.Expr.create_RawExpr((this.error).dtor_value), _1680_fieldName, rhs);
                }
                goto after_match21;
              }
            }
            {
              if (!object.Equals(_1681_onExpr, RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self")))) {
                _1681_onExpr = ((this).modify__macro).Apply1(_1681_onExpr);
              }
              generated = RAST.__default.AssignMember(_1681_onExpr, _1680_fieldName, rhs);
            }
          after_match21: ;
            readIdents = _1683_recIdents;
            needsIIFE = false;
          }
          goto after_match20;
        }
      }
      {
        DAST._IExpression _1685_on = _source88.dtor_expr;
        Dafny.ISequence<DAST._IExpression> _1686_indices = _source88.dtor_indices;
        {
          RAST._IExpr _1687_onExpr;
          DCOMP._IOwnership _1688_onOwned;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1689_recIdents;
          RAST._IExpr _out122;
          DCOMP._IOwnership _out123;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out124;
          (this).GenExpr(_1685_on, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out122, out _out123, out _out124);
          _1687_onExpr = _out122;
          _1688_onOwned = _out123;
          _1689_recIdents = _out124;
          readIdents = _1689_recIdents;
          _1687_onExpr = ((this).modify__macro).Apply1(_1687_onExpr);
          RAST._IExpr _1690_r;
          _1690_r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
          Dafny.ISequence<RAST._IExpr> _1691_indicesExpr;
          _1691_indicesExpr = Dafny.Sequence<RAST._IExpr>.FromElements();
          BigInteger _hi33 = new BigInteger((_1686_indices).Count);
          for (BigInteger _1692_i = BigInteger.Zero; _1692_i < _hi33; _1692_i++) {
            RAST._IExpr _1693_idx;
            DCOMP._IOwnership _1694___v81;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1695_recIdentsIdx;
            RAST._IExpr _out125;
            DCOMP._IOwnership _out126;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out127;
            (this).GenExpr((_1686_indices).Select(_1692_i), selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out125, out _out126, out _out127);
            _1693_idx = _out125;
            _1694___v81 = _out126;
            _1695_recIdentsIdx = _out127;
            Dafny.ISequence<Dafny.Rune> _1696_varName;
            _1696_varName = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("__idx"), Std.Strings.__default.OfNat(_1692_i));
            _1691_indicesExpr = Dafny.Sequence<RAST._IExpr>.Concat(_1691_indicesExpr, Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(_1696_varName)));
            _1690_r = (_1690_r).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), _1696_varName, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.__default.IntoUsize(_1693_idx))));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _1695_recIdentsIdx);
          }
          if ((new BigInteger((_1686_indices).Count)) > (BigInteger.One)) {
            _1687_onExpr = (_1687_onExpr).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("data"));
          }
          RAST._IExpr _1697_rhs;
          _1697_rhs = rhs;
          var _pat_let_tv3 = env;
          if (((_1687_onExpr).IsLhsIdentifier()) && (Dafny.Helpers.Let<Dafny.ISequence<Dafny.Rune>, bool>((_1687_onExpr).LhsIdentifierName(), _pat_let22_0 => Dafny.Helpers.Let<Dafny.ISequence<Dafny.Rune>, bool>(_pat_let22_0, _1698_name => (true) && (Dafny.Helpers.Let<Std.Wrappers._IOption<RAST._IType>, bool>((_pat_let_tv3).GetType(_1698_name), _pat_let23_0 => Dafny.Helpers.Let<Std.Wrappers._IOption<RAST._IType>, bool>(_pat_let23_0, _1699_tpe => ((_1699_tpe).is_Some) && (((_1699_tpe).dtor_value).IsUninitArray())))))))) {
            _1697_rhs = RAST.__default.MaybeUninitNew(_1697_rhs);
          }
          generated = (_1690_r).Then(RAST.Expr.create_Assign(Std.Wrappers.Option<RAST._IAssignLhs>.create_Some(RAST.AssignLhs.create_Index(_1687_onExpr, _1691_indicesExpr)), _1697_rhs));
          needsIIFE = true;
        }
      }
    after_match20: ;
    }
    public void GenStmt(DAST._IStatement stmt, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, bool isLast, Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> earlyReturn, out RAST._IExpr generated, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents, out DCOMP._IEnvironment newEnv)
    {
      generated = RAST.Expr.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      newEnv = DCOMP.Environment.Default();
      DAST._IStatement _source90 = stmt;
      {
        if (_source90.is_ConstructorNewSeparator) {
          Dafny.ISequence<DAST._IFormal> _1700_fields = _source90.dtor_fields;
          {
            generated = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            newEnv = env;
            BigInteger _hi34 = new BigInteger((_1700_fields).Count);
            for (BigInteger _1701_i = BigInteger.Zero; _1701_i < _hi34; _1701_i++) {
              DAST._IFormal _1702_field;
              _1702_field = (_1700_fields).Select(_1701_i);
              Dafny.ISequence<Dafny.Rune> _1703_fieldName;
              _1703_fieldName = DCOMP.__default.escapeVar((_1702_field).dtor_name);
              RAST._IType _1704_fieldTyp;
              RAST._IType _out128;
              _out128 = (this).GenType((_1702_field).dtor_typ, DCOMP.GenTypeContext.@default());
              _1704_fieldTyp = _out128;
              Dafny.ISequence<Dafny.Rune> _1705_isAssignedVar;
              _1705_isAssignedVar = DCOMP.__default.AddAssignedPrefix(_1703_fieldName);
              if (((newEnv).dtor_names).Contains(_1705_isAssignedVar)) {
                RAST._IExpr _1706_rhs;
                DCOMP._IOwnership _1707___v82;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1708___v83;
                RAST._IExpr _out129;
                DCOMP._IOwnership _out130;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out131;
                (this).GenExpr(DAST.Expression.create_InitializationValue((_1702_field).dtor_typ), selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out129, out _out130, out _out131);
                _1706_rhs = _out129;
                _1707___v82 = _out130;
                _1708___v83 = _out131;
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_1705_isAssignedVar));
                generated = (generated).Then((((RAST.__default.dafny__runtime).MSel((this).update__field__if__uninit__macro)).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this")), RAST.Expr.create_Identifier(_1703_fieldName), RAST.Expr.create_Identifier(_1705_isAssignedVar), _1706_rhs)));
                newEnv = (newEnv).RemoveAssigned(_1705_isAssignedVar);
              }
            }
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_DeclareVar) {
          Dafny.ISequence<Dafny.Rune> _1709_name = _source90.dtor_name;
          DAST._IType _1710_typ = _source90.dtor_typ;
          Std.Wrappers._IOption<DAST._IExpression> maybeValue1 = _source90.dtor_maybeValue;
          if (maybeValue1.is_Some) {
            DAST._IExpression _1711_expression = maybeValue1.dtor_value;
            {
              RAST._IType _1712_tpe;
              RAST._IType _out132;
              _out132 = (this).GenType(_1710_typ, DCOMP.GenTypeContext.@default());
              _1712_tpe = _out132;
              Dafny.ISequence<Dafny.Rune> _1713_varName;
              _1713_varName = DCOMP.__default.escapeVar(_1709_name);
              bool _1714_hasCopySemantics;
              _1714_hasCopySemantics = (_1712_tpe).CanReadWithoutClone();
              if (((_1711_expression).is_InitializationValue) && (!(_1714_hasCopySemantics))) {
                generated = RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _1713_varName, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(((((RAST.__default.MaybePlaceboPath).AsExpr()).ApplyType1(_1712_tpe)).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements())));
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
                newEnv = (env).AddAssigned(_1713_varName, RAST.__default.MaybePlaceboType(_1712_tpe));
              } else {
                RAST._IExpr _1715_expr = RAST.Expr.Default();
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1716_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
                if (((_1711_expression).is_InitializationValue) && ((_1712_tpe).IsObjectOrPointer())) {
                  _1715_expr = (_1712_tpe).ToNullExpr();
                  _1716_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
                } else {
                  DCOMP._IOwnership _1717_exprOwnership = DCOMP.Ownership.Default();
                  RAST._IExpr _out133;
                  DCOMP._IOwnership _out134;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out135;
                  (this).GenExpr(_1711_expression, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out133, out _out134, out _out135);
                  _1715_expr = _out133;
                  _1717_exprOwnership = _out134;
                  _1716_recIdents = _out135;
                }
                readIdents = _1716_recIdents;
                if ((_1711_expression).is_NewUninitArray) {
                  _1712_tpe = (_1712_tpe).TypeAtInitialization();
                } else {
                  _1712_tpe = _1712_tpe;
                }
                generated = RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _1713_varName, Std.Wrappers.Option<RAST._IType>.create_Some(_1712_tpe), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1715_expr));
                newEnv = (env).AddAssigned(_1713_varName, _1712_tpe);
              }
            }
            goto after_match22;
          }
        }
      }
      {
        if (_source90.is_DeclareVar) {
          Dafny.ISequence<Dafny.Rune> _1718_name = _source90.dtor_name;
          DAST._IType _1719_typ = _source90.dtor_typ;
          Std.Wrappers._IOption<DAST._IExpression> maybeValue2 = _source90.dtor_maybeValue;
          if (maybeValue2.is_None) {
            {
              DAST._IStatement _1720_newStmt;
              _1720_newStmt = DAST.Statement.create_DeclareVar(_1718_name, _1719_typ, Std.Wrappers.Option<DAST._IExpression>.create_Some(DAST.Expression.create_InitializationValue(_1719_typ)));
              RAST._IExpr _out136;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out137;
              DCOMP._IEnvironment _out138;
              (this).GenStmt(_1720_newStmt, selfIdent, env, isLast, earlyReturn, out _out136, out _out137, out _out138);
              generated = _out136;
              readIdents = _out137;
              newEnv = _out138;
            }
            goto after_match22;
          }
        }
      }
      {
        if (_source90.is_Assign) {
          DAST._IAssignLhs _1721_lhs = _source90.dtor_lhs;
          DAST._IExpression _1722_expression = _source90.dtor_value;
          {
            RAST._IExpr _1723_exprGen;
            DCOMP._IOwnership _1724___v84;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1725_exprIdents;
            RAST._IExpr _out139;
            DCOMP._IOwnership _out140;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out141;
            (this).GenExpr(_1722_expression, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out139, out _out140, out _out141);
            _1723_exprGen = _out139;
            _1724___v84 = _out140;
            _1725_exprIdents = _out141;
            if ((_1721_lhs).is_Ident) {
              Dafny.ISequence<Dafny.Rune> _1726_rustId;
              _1726_rustId = DCOMP.__default.escapeVar((_1721_lhs).dtor_ident);
              Std.Wrappers._IOption<RAST._IType> _1727_tpe;
              _1727_tpe = (env).GetType(_1726_rustId);
              if (((_1727_tpe).is_Some) && ((((_1727_tpe).dtor_value).ExtractMaybePlacebo()).is_Some)) {
                _1723_exprGen = RAST.__default.MaybePlacebo(_1723_exprGen);
              }
            }
            if (((_1721_lhs).is_Index) && (((_1721_lhs).dtor_expr).is_Ident)) {
              Dafny.ISequence<Dafny.Rune> _1728_rustId;
              _1728_rustId = DCOMP.__default.escapeVar(((_1721_lhs).dtor_expr).dtor_name);
              Std.Wrappers._IOption<RAST._IType> _1729_tpe;
              _1729_tpe = (env).GetType(_1728_rustId);
              if (((_1729_tpe).is_Some) && ((((_1729_tpe).dtor_value).ExtractMaybeUninitArrayElement()).is_Some)) {
                _1723_exprGen = RAST.__default.MaybeUninitNew(_1723_exprGen);
              }
            }
            RAST._IExpr _1730_lhsGen;
            bool _1731_needsIIFE;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1732_recIdents;
            DCOMP._IEnvironment _1733_resEnv;
            RAST._IExpr _out142;
            bool _out143;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out144;
            DCOMP._IEnvironment _out145;
            (this).GenAssignLhs(_1721_lhs, _1723_exprGen, selfIdent, env, out _out142, out _out143, out _out144, out _out145);
            _1730_lhsGen = _out142;
            _1731_needsIIFE = _out143;
            _1732_recIdents = _out144;
            _1733_resEnv = _out145;
            generated = _1730_lhsGen;
            newEnv = _1733_resEnv;
            if (_1731_needsIIFE) {
              generated = RAST.Expr.create_Block(generated);
            }
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_1732_recIdents, _1725_exprIdents);
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_If) {
          DAST._IExpression _1734_cond = _source90.dtor_cond;
          Dafny.ISequence<DAST._IStatement> _1735_thnDafny = _source90.dtor_thn;
          Dafny.ISequence<DAST._IStatement> _1736_elsDafny = _source90.dtor_els;
          {
            RAST._IExpr _1737_cond;
            DCOMP._IOwnership _1738___v85;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1739_recIdents;
            RAST._IExpr _out146;
            DCOMP._IOwnership _out147;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out148;
            (this).GenExpr(_1734_cond, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out146, out _out147, out _out148);
            _1737_cond = _out146;
            _1738___v85 = _out147;
            _1739_recIdents = _out148;
            Dafny.ISequence<Dafny.Rune> _1740_condString;
            _1740_condString = (_1737_cond)._ToString(DCOMP.__default.IND);
            readIdents = _1739_recIdents;
            RAST._IExpr _1741_thn;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1742_thnIdents;
            DCOMP._IEnvironment _1743_thnEnv;
            RAST._IExpr _out149;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out150;
            DCOMP._IEnvironment _out151;
            (this).GenStmts(_1735_thnDafny, selfIdent, env, isLast, earlyReturn, out _out149, out _out150, out _out151);
            _1741_thn = _out149;
            _1742_thnIdents = _out150;
            _1743_thnEnv = _out151;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _1742_thnIdents);
            RAST._IExpr _1744_els;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1745_elsIdents;
            DCOMP._IEnvironment _1746_elsEnv;
            RAST._IExpr _out152;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out153;
            DCOMP._IEnvironment _out154;
            (this).GenStmts(_1736_elsDafny, selfIdent, env, isLast, earlyReturn, out _out152, out _out153, out _out154);
            _1744_els = _out152;
            _1745_elsIdents = _out153;
            _1746_elsEnv = _out154;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _1745_elsIdents);
            newEnv = env;
            generated = RAST.Expr.create_IfExpr(_1737_cond, _1741_thn, _1744_els);
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_Labeled) {
          Dafny.ISequence<Dafny.Rune> _1747_lbl = _source90.dtor_lbl;
          Dafny.ISequence<DAST._IStatement> _1748_body = _source90.dtor_body;
          {
            RAST._IExpr _1749_body;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1750_bodyIdents;
            DCOMP._IEnvironment _1751_env2;
            RAST._IExpr _out155;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out156;
            DCOMP._IEnvironment _out157;
            (this).GenStmts(_1748_body, selfIdent, env, isLast, earlyReturn, out _out155, out _out156, out _out157);
            _1749_body = _out155;
            _1750_bodyIdents = _out156;
            _1751_env2 = _out157;
            readIdents = _1750_bodyIdents;
            generated = RAST.Expr.create_Labelled(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("label_"), _1747_lbl), RAST.Expr.create_Loop(Std.Wrappers.Option<RAST._IExpr>.create_None(), RAST.Expr.create_StmtExpr(_1749_body, RAST.Expr.create_Break(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None()))));
            newEnv = env;
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_While) {
          DAST._IExpression _1752_cond = _source90.dtor_cond;
          Dafny.ISequence<DAST._IStatement> _1753_body = _source90.dtor_body;
          {
            RAST._IExpr _1754_cond;
            DCOMP._IOwnership _1755___v86;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1756_recIdents;
            RAST._IExpr _out158;
            DCOMP._IOwnership _out159;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out160;
            (this).GenExpr(_1752_cond, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out158, out _out159, out _out160);
            _1754_cond = _out158;
            _1755___v86 = _out159;
            _1756_recIdents = _out160;
            readIdents = _1756_recIdents;
            RAST._IExpr _1757_bodyExpr;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1758_bodyIdents;
            DCOMP._IEnvironment _1759_bodyEnv;
            RAST._IExpr _out161;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out162;
            DCOMP._IEnvironment _out163;
            (this).GenStmts(_1753_body, selfIdent, env, false, earlyReturn, out _out161, out _out162, out _out163);
            _1757_bodyExpr = _out161;
            _1758_bodyIdents = _out162;
            _1759_bodyEnv = _out163;
            newEnv = env;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _1758_bodyIdents);
            generated = RAST.Expr.create_Loop(Std.Wrappers.Option<RAST._IExpr>.create_Some(_1754_cond), _1757_bodyExpr);
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_Foreach) {
          Dafny.ISequence<Dafny.Rune> _1760_boundName = _source90.dtor_boundName;
          DAST._IType _1761_boundType = _source90.dtor_boundType;
          DAST._IExpression _1762_overExpr = _source90.dtor_over;
          Dafny.ISequence<DAST._IStatement> _1763_body = _source90.dtor_body;
          {
            RAST._IExpr _1764_over;
            DCOMP._IOwnership _1765___v87;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1766_recIdents;
            RAST._IExpr _out164;
            DCOMP._IOwnership _out165;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out166;
            (this).GenExpr(_1762_overExpr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out164, out _out165, out _out166);
            _1764_over = _out164;
            _1765___v87 = _out165;
            _1766_recIdents = _out166;
            if (((_1762_overExpr).is_MapBoundedPool) || ((_1762_overExpr).is_SetBoundedPool)) {
              _1764_over = ((_1764_over).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cloned"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            }
            RAST._IType _1767_boundTpe;
            RAST._IType _out167;
            _out167 = (this).GenType(_1761_boundType, DCOMP.GenTypeContext.@default());
            _1767_boundTpe = _out167;
            readIdents = _1766_recIdents;
            Dafny.ISequence<Dafny.Rune> _1768_boundRName;
            _1768_boundRName = DCOMP.__default.escapeVar(_1760_boundName);
            RAST._IExpr _1769_bodyExpr;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1770_bodyIdents;
            DCOMP._IEnvironment _1771_bodyEnv;
            RAST._IExpr _out168;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out169;
            DCOMP._IEnvironment _out170;
            (this).GenStmts(_1763_body, selfIdent, (env).AddAssigned(_1768_boundRName, _1767_boundTpe), false, earlyReturn, out _out168, out _out169, out _out170);
            _1769_bodyExpr = _out168;
            _1770_bodyIdents = _out169;
            _1771_bodyEnv = _out170;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _1770_bodyIdents), Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_1768_boundRName));
            newEnv = env;
            generated = RAST.Expr.create_For(_1768_boundRName, _1764_over, _1769_bodyExpr);
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_Break) {
          Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1772_toLabel = _source90.dtor_toLabel;
          {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _source91 = _1772_toLabel;
            {
              if (_source91.is_Some) {
                Dafny.ISequence<Dafny.Rune> _1773_lbl = _source91.dtor_value;
                {
                  generated = RAST.Expr.create_Break(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("label_"), _1773_lbl)));
                }
                goto after_match23;
              }
            }
            {
              {
                generated = RAST.Expr.create_Break(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None());
              }
            }
          after_match23: ;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            newEnv = env;
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_TailRecursive) {
          Dafny.ISequence<DAST._IStatement> _1774_body = _source90.dtor_body;
          {
            generated = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
            if (!object.Equals(selfIdent, DCOMP.SelfInfo.create_NoSelf())) {
              RAST._IExpr _1775_selfClone;
              DCOMP._IOwnership _1776___v88;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1777___v89;
              RAST._IExpr _out171;
              DCOMP._IOwnership _out172;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out173;
              (this).GenIdent((selfIdent).dtor_rSelfName, selfIdent, DCOMP.Environment.Empty(), DCOMP.Ownership.create_OwnershipOwned(), out _out171, out _out172, out _out173);
              _1775_selfClone = _out171;
              _1776___v88 = _out172;
              _1777___v89 = _out173;
              generated = (generated).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_this"), Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1775_selfClone)));
            }
            newEnv = env;
            BigInteger _hi35 = new BigInteger(((env).dtor_names).Count);
            for (BigInteger _1778_paramI = BigInteger.Zero; _1778_paramI < _hi35; _1778_paramI++) {
              Dafny.ISequence<Dafny.Rune> _1779_param;
              _1779_param = ((env).dtor_names).Select(_1778_paramI);
              RAST._IExpr _1780_paramInit;
              DCOMP._IOwnership _1781___v90;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1782___v91;
              RAST._IExpr _out174;
              DCOMP._IOwnership _out175;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out176;
              (this).GenIdent(_1779_param, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out174, out _out175, out _out176);
              _1780_paramInit = _out174;
              _1781___v90 = _out175;
              _1782___v91 = _out176;
              generated = (generated).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _1779_param, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1780_paramInit)));
              if (((env).dtor_types).Contains(_1779_param)) {
                RAST._IType _1783_declaredType;
                _1783_declaredType = (Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Select((env).dtor_types,_1779_param)).ToOwned();
                newEnv = (newEnv).AddAssigned(_1779_param, _1783_declaredType);
              }
            }
            RAST._IExpr _1784_bodyExpr;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1785_bodyIdents;
            DCOMP._IEnvironment _1786_bodyEnv;
            RAST._IExpr _out177;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out178;
            DCOMP._IEnvironment _out179;
            (this).GenStmts(_1774_body, ((!object.Equals(selfIdent, DCOMP.SelfInfo.create_NoSelf())) ? (DCOMP.SelfInfo.create_ThisTyped(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_this"), (selfIdent).dtor_dafnyType)) : (DCOMP.SelfInfo.create_NoSelf())), newEnv, false, earlyReturn, out _out177, out _out178, out _out179);
            _1784_bodyExpr = _out177;
            _1785_bodyIdents = _out178;
            _1786_bodyEnv = _out179;
            readIdents = _1785_bodyIdents;
            generated = (generated).Then(RAST.Expr.create_Labelled(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("TAIL_CALL_START"), RAST.Expr.create_Loop(Std.Wrappers.Option<RAST._IExpr>.create_None(), _1784_bodyExpr)));
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_JumpTailCallStart) {
          {
            generated = RAST.Expr.create_Continue(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("TAIL_CALL_START")));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            newEnv = env;
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_Call) {
          DAST._IExpression _1787_on = _source90.dtor_on;
          DAST._ICallName _1788_name = _source90.dtor_callName;
          Dafny.ISequence<DAST._IType> _1789_typeArgs = _source90.dtor_typeArgs;
          Dafny.ISequence<DAST._IExpression> _1790_args = _source90.dtor_args;
          Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> _1791_maybeOutVars = _source90.dtor_outs;
          {
            Dafny.ISequence<RAST._IExpr> _1792_argExprs;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1793_recIdents;
            Dafny.ISequence<RAST._IType> _1794_typeExprs;
            Std.Wrappers._IOption<DAST._IResolvedType> _1795_fullNameQualifier;
            Dafny.ISequence<RAST._IExpr> _out180;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out181;
            Dafny.ISequence<RAST._IType> _out182;
            Std.Wrappers._IOption<DAST._IResolvedType> _out183;
            (this).GenArgs(selfIdent, _1788_name, _1789_typeArgs, _1790_args, env, out _out180, out _out181, out _out182, out _out183);
            _1792_argExprs = _out180;
            _1793_recIdents = _out181;
            _1794_typeExprs = _out182;
            _1795_fullNameQualifier = _out183;
            readIdents = _1793_recIdents;
            Std.Wrappers._IOption<DAST._IResolvedType> _source92 = _1795_fullNameQualifier;
            {
              if (_source92.is_Some) {
                DAST._IResolvedType value9 = _source92.dtor_value;
                Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1796_path = value9.dtor_path;
                Dafny.ISequence<DAST._IType> _1797_onTypeArgs = value9.dtor_typeArgs;
                DAST._IResolvedTypeBase _1798_base = value9.dtor_kind;
                RAST._IExpr _1799_fullPath;
                RAST._IExpr _out184;
                _out184 = DCOMP.COMP.GenPathExpr(_1796_path, true);
                _1799_fullPath = _out184;
                Dafny.ISequence<RAST._IType> _1800_onTypeExprs;
                Dafny.ISequence<RAST._IType> _out185;
                _out185 = (this).GenTypeArgs(_1797_onTypeArgs, DCOMP.GenTypeContext.@default());
                _1800_onTypeExprs = _out185;
                RAST._IExpr _1801_onExpr = RAST.Expr.Default();
                DCOMP._IOwnership _1802_recOwnership = DCOMP.Ownership.Default();
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1803_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
                if (((_1798_base).is_Trait) || ((_1798_base).is_Class)) {
                  RAST._IExpr _out186;
                  DCOMP._IOwnership _out187;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out188;
                  (this).GenExpr(_1787_on, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out186, out _out187, out _out188);
                  _1801_onExpr = _out186;
                  _1802_recOwnership = _out187;
                  _1803_recIdents = _out188;
                  _1801_onExpr = ((this).modify__macro).Apply1(_1801_onExpr);
                  readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _1803_recIdents);
                } else {
                  RAST._IExpr _out189;
                  DCOMP._IOwnership _out190;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out191;
                  (this).GenExpr(_1787_on, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowedMut(), out _out189, out _out190, out _out191);
                  _1801_onExpr = _out189;
                  _1802_recOwnership = _out190;
                  _1803_recIdents = _out191;
                  readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _1803_recIdents);
                }
                generated = ((((_1799_fullPath).ApplyType(_1800_onTypeExprs)).FSel(DCOMP.__default.escapeName((_1788_name).dtor_name))).ApplyType(_1794_typeExprs)).Apply(Dafny.Sequence<RAST._IExpr>.Concat(Dafny.Sequence<RAST._IExpr>.FromElements(_1801_onExpr), _1792_argExprs));
                goto after_match24;
              }
            }
            {
              RAST._IExpr _1804_onExpr;
              DCOMP._IOwnership _1805___v96;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1806_enclosingIdents;
              RAST._IExpr _out192;
              DCOMP._IOwnership _out193;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out194;
              (this).GenExpr(_1787_on, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out192, out _out193, out _out194);
              _1804_onExpr = _out192;
              _1805___v96 = _out193;
              _1806_enclosingIdents = _out194;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _1806_enclosingIdents);
              Dafny.ISequence<Dafny.Rune> _1807_renderedName;
              _1807_renderedName = (this).GetMethodName(_1787_on, _1788_name);
              DAST._IExpression _source93 = _1787_on;
              {
                bool disjunctiveMatch12 = false;
                if (_source93.is_Companion) {
                  disjunctiveMatch12 = true;
                }
                if (_source93.is_ExternCompanion) {
                  disjunctiveMatch12 = true;
                }
                if (disjunctiveMatch12) {
                  {
                    _1804_onExpr = (_1804_onExpr).FSel(_1807_renderedName);
                  }
                  goto after_match25;
                }
              }
              {
                {
                  if (!object.Equals(_1804_onExpr, RAST.__default.self)) {
                    DAST._ICallName _source94 = _1788_name;
                    {
                      if (_source94.is_CallName) {
                        Std.Wrappers._IOption<DAST._IType> onType0 = _source94.dtor_onType;
                        if (onType0.is_Some) {
                          DAST._IType _1808_tpe = onType0.dtor_value;
                          RAST._IType _1809_typ;
                          RAST._IType _out195;
                          _out195 = (this).GenType(_1808_tpe, DCOMP.GenTypeContext.@default());
                          _1809_typ = _out195;
                          if (((_1809_typ).IsObjectOrPointer()) && (!object.Equals(_1804_onExpr, RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))))) {
                            _1804_onExpr = ((this).modify__macro).Apply1(_1804_onExpr);
                          }
                          goto after_match26;
                        }
                      }
                    }
                    {
                    }
                  after_match26: ;
                  }
                  _1804_onExpr = (_1804_onExpr).Sel(_1807_renderedName);
                }
              }
            after_match25: ;
              generated = ((_1804_onExpr).ApplyType(_1794_typeExprs)).Apply(_1792_argExprs);
            }
          after_match24: ;
            if (((_1791_maybeOutVars).is_Some) && ((new BigInteger(((_1791_maybeOutVars).dtor_value).Count)) == (BigInteger.One))) {
              Dafny.ISequence<Dafny.Rune> _1810_outVar;
              _1810_outVar = DCOMP.__default.escapeVar(((_1791_maybeOutVars).dtor_value).Select(BigInteger.Zero));
              if (!((env).CanReadWithoutClone(_1810_outVar))) {
                generated = RAST.__default.MaybePlacebo(generated);
              }
              generated = RAST.__default.AssignVar(_1810_outVar, generated);
            } else if (((_1791_maybeOutVars).is_None) || ((new BigInteger(((_1791_maybeOutVars).dtor_value).Count)).Sign == 0)) {
            } else {
              Dafny.ISequence<Dafny.Rune> _1811_tmpVar;
              _1811_tmpVar = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_x");
              RAST._IExpr _1812_tmpId;
              _1812_tmpId = RAST.Expr.create_Identifier(_1811_tmpVar);
              generated = RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), _1811_tmpVar, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(generated));
              Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1813_outVars;
              _1813_outVars = (_1791_maybeOutVars).dtor_value;
              BigInteger _hi36 = new BigInteger((_1813_outVars).Count);
              for (BigInteger _1814_outI = BigInteger.Zero; _1814_outI < _hi36; _1814_outI++) {
                Dafny.ISequence<Dafny.Rune> _1815_outVar;
                _1815_outVar = DCOMP.__default.escapeVar((_1813_outVars).Select(_1814_outI));
                RAST._IExpr _1816_rhs;
                _1816_rhs = (_1812_tmpId).Sel(Std.Strings.__default.OfNat(_1814_outI));
                if (!((env).CanReadWithoutClone(_1815_outVar))) {
                  _1816_rhs = RAST.__default.MaybePlacebo(_1816_rhs);
                }
                generated = (generated).Then(RAST.__default.AssignVar(_1815_outVar, _1816_rhs));
              }
            }
            newEnv = env;
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_Return) {
          DAST._IExpression _1817_exprDafny = _source90.dtor_expr;
          {
            RAST._IExpr _1818_expr;
            DCOMP._IOwnership _1819___v106;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1820_recIdents;
            RAST._IExpr _out196;
            DCOMP._IOwnership _out197;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out198;
            (this).GenExpr(_1817_exprDafny, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out196, out _out197, out _out198);
            _1818_expr = _out196;
            _1819___v106 = _out197;
            _1820_recIdents = _out198;
            readIdents = _1820_recIdents;
            if (isLast) {
              generated = _1818_expr;
            } else {
              generated = RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_Some(_1818_expr));
            }
            newEnv = env;
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_EarlyReturn) {
          {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> _source95 = earlyReturn;
            {
              if (_source95.is_None) {
                generated = RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_None());
                goto after_match27;
              }
            }
            {
              Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1821_rustIdents = _source95.dtor_value;
              Dafny.ISequence<RAST._IExpr> _1822_tupleArgs;
              _1822_tupleArgs = Dafny.Sequence<RAST._IExpr>.FromElements();
              BigInteger _hi37 = new BigInteger((_1821_rustIdents).Count);
              for (BigInteger _1823_i = BigInteger.Zero; _1823_i < _hi37; _1823_i++) {
                RAST._IExpr _1824_rIdent;
                DCOMP._IOwnership _1825___v107;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1826___v108;
                RAST._IExpr _out199;
                DCOMP._IOwnership _out200;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out201;
                (this).GenIdent((_1821_rustIdents).Select(_1823_i), selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out199, out _out200, out _out201);
                _1824_rIdent = _out199;
                _1825___v107 = _out200;
                _1826___v108 = _out201;
                _1822_tupleArgs = Dafny.Sequence<RAST._IExpr>.Concat(_1822_tupleArgs, Dafny.Sequence<RAST._IExpr>.FromElements(_1824_rIdent));
              }
              if ((new BigInteger((_1822_tupleArgs).Count)) == (BigInteger.One)) {
                generated = RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_Some((_1822_tupleArgs).Select(BigInteger.Zero)));
              } else {
                generated = RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_Tuple(_1822_tupleArgs)));
              }
            }
          after_match27: ;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            newEnv = env;
          }
          goto after_match22;
        }
      }
      {
        if (_source90.is_Halt) {
          {
            generated = (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("panic!"))).Apply1(RAST.Expr.create_LiteralString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Halt"), false, false));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            newEnv = env;
          }
          goto after_match22;
        }
      }
      {
        DAST._IExpression _1827_e = _source90.dtor_Print_a0;
        {
          RAST._IExpr _1828_printedExpr;
          DCOMP._IOwnership _1829_recOwnership;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1830_recIdents;
          RAST._IExpr _out202;
          DCOMP._IOwnership _out203;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out204;
          (this).GenExpr(_1827_e, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out202, out _out203, out _out204);
          _1828_printedExpr = _out202;
          _1829_recOwnership = _out203;
          _1830_recIdents = _out204;
          generated = (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("print!"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_LiteralString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{}"), false, false), (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyPrintWrapper"))).AsExpr()).Apply1(_1828_printedExpr)));
          readIdents = _1830_recIdents;
          newEnv = env;
        }
      }
    after_match22: ;
    }
    public static Std.Wrappers._IOption<RAST._IType> NewtypeRangeToRustType(DAST._INewtypeRange range) {
      DAST._INewtypeRange _source96 = range;
      {
        if (_source96.is_NoRange) {
          return Std.Wrappers.Option<RAST._IType>.create_None();
        }
      }
      {
        if (_source96.is_U8) {
          return Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_U8());
        }
      }
      {
        if (_source96.is_U16) {
          return Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_U16());
        }
      }
      {
        if (_source96.is_U32) {
          return Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_U32());
        }
      }
      {
        if (_source96.is_U64) {
          return Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_U64());
        }
      }
      {
        if (_source96.is_U128) {
          return Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_U128());
        }
      }
      {
        if (_source96.is_I8) {
          return Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I8());
        }
      }
      {
        if (_source96.is_I16) {
          return Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I16());
        }
      }
      {
        if (_source96.is_I32) {
          return Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I32());
        }
      }
      {
        if (_source96.is_I64) {
          return Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I64());
        }
      }
      {
        if (_source96.is_I128) {
          return Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I128());
        }
      }
      {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      }
    }
    public void FromOwned(RAST._IExpr r, DCOMP._IOwnership expectedOwnership, out RAST._IExpr @out, out DCOMP._IOwnership resultingOwnership)
    {
      @out = RAST.Expr.Default();
      resultingOwnership = DCOMP.Ownership.Default();
      if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipOwnedBox())) {
        @out = RAST.__default.BoxNew(r);
        resultingOwnership = DCOMP.Ownership.create_OwnershipOwnedBox();
      } else if ((object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipOwned())) || (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipAutoBorrowed()))) {
        @out = r;
        resultingOwnership = DCOMP.Ownership.create_OwnershipOwned();
      } else if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipBorrowed())) {
        @out = RAST.__default.Borrow(r);
        resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowed();
      } else {
        @out = ((this).modify__macro).Apply1(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat((r)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/*TODO: Conversion from Borrowed or BorrowedMut to BorrowedMut*/"))));
        resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowedMut();
      }
    }
    public void FromOwnership(RAST._IExpr r, DCOMP._IOwnership ownership, DCOMP._IOwnership expectedOwnership, out RAST._IExpr @out, out DCOMP._IOwnership resultingOwnership)
    {
      @out = RAST.Expr.Default();
      resultingOwnership = DCOMP.Ownership.Default();
      if (object.Equals(ownership, expectedOwnership)) {
        @out = r;
        resultingOwnership = expectedOwnership;
        return ;
      }
      if (object.Equals(ownership, DCOMP.Ownership.create_OwnershipOwned())) {
        RAST._IExpr _out205;
        DCOMP._IOwnership _out206;
        (this).FromOwned(r, expectedOwnership, out _out205, out _out206);
        @out = _out205;
        resultingOwnership = _out206;
        return ;
      } else if (object.Equals(ownership, DCOMP.Ownership.create_OwnershipOwnedBox())) {
        RAST._IExpr _out207;
        DCOMP._IOwnership _out208;
        (this).FromOwned(RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"), r, DAST.Format.UnaryOpFormat.create_NoFormat()), expectedOwnership, out _out207, out _out208);
        @out = _out207;
        resultingOwnership = _out208;
      } else if ((object.Equals(ownership, DCOMP.Ownership.create_OwnershipBorrowed())) || (object.Equals(ownership, DCOMP.Ownership.create_OwnershipBorrowedMut()))) {
        if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipOwned())) {
          resultingOwnership = DCOMP.Ownership.create_OwnershipOwned();
          @out = (r).Clone();
        } else if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipOwnedBox())) {
          resultingOwnership = DCOMP.Ownership.create_OwnershipOwnedBox();
          @out = RAST.__default.BoxNew((r).Clone());
        } else if ((object.Equals(expectedOwnership, ownership)) || (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipAutoBorrowed()))) {
          resultingOwnership = ownership;
          @out = r;
        } else if ((object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipBorrowed())) && (object.Equals(ownership, DCOMP.Ownership.create_OwnershipBorrowedMut()))) {
          resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowed();
          @out = r;
        } else {
          resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowedMut();
          @out = RAST.__default.BorrowMut(r);
        }
      } else {
      }
    }
    public static bool OwnershipGuarantee(DCOMP._IOwnership expectedOwnership, DCOMP._IOwnership resultingOwnership)
    {
      return (!(!object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipAutoBorrowed())) || (object.Equals(resultingOwnership, expectedOwnership))) && (!object.Equals(resultingOwnership, DCOMP.Ownership.create_OwnershipAutoBorrowed()));
    }
    public void GenExprLiteral(DAST._IExpression e, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, DCOMP._IOwnership expectedOwnership, out RAST._IExpr r, out DCOMP._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = DCOMP.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _source97 = e;
      {
        if (_source97.is_Literal) {
          DAST._ILiteral _h170 = _source97.dtor_Literal_a0;
          if (_h170.is_BoolLiteral) {
            bool _1831_b = _h170.dtor_BoolLiteral_a0;
            {
              RAST._IExpr _out209;
              DCOMP._IOwnership _out210;
              (this).FromOwned(RAST.Expr.create_LiteralBool(_1831_b), expectedOwnership, out _out209, out _out210);
              r = _out209;
              resultingOwnership = _out210;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match28;
          }
        }
      }
      {
        if (_source97.is_Literal) {
          DAST._ILiteral _h171 = _source97.dtor_Literal_a0;
          if (_h171.is_IntLiteral) {
            Dafny.ISequence<Dafny.Rune> _1832_i = _h171.dtor_IntLiteral_a0;
            DAST._IType _1833_t = _h171.dtor_IntLiteral_a1;
            {
              DAST._IType _source98 = _1833_t;
              {
                if (_source98.is_Primitive) {
                  DAST._IPrimitive _h70 = _source98.dtor_Primitive_a0;
                  if (_h70.is_Int) {
                    {
                      if ((new BigInteger((_1832_i).Count)) <= (new BigInteger(4))) {
                        r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1(RAST.Expr.create_LiteralInt(_1832_i));
                      } else {
                        r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1(RAST.Expr.create_LiteralString(_1832_i, true, false));
                      }
                    }
                    goto after_match29;
                  }
                }
              }
              {
                DAST._IType _1834_o = _source98;
                {
                  RAST._IType _1835_genType;
                  RAST._IType _out211;
                  _out211 = (this).GenType(_1834_o, DCOMP.GenTypeContext.@default());
                  _1835_genType = _out211;
                  r = RAST.Expr.create_TypeAscription(RAST.Expr.create_RawExpr(_1832_i), _1835_genType);
                }
              }
            after_match29: ;
              RAST._IExpr _out212;
              DCOMP._IOwnership _out213;
              (this).FromOwned(r, expectedOwnership, out _out212, out _out213);
              r = _out212;
              resultingOwnership = _out213;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match28;
          }
        }
      }
      {
        if (_source97.is_Literal) {
          DAST._ILiteral _h172 = _source97.dtor_Literal_a0;
          if (_h172.is_DecLiteral) {
            Dafny.ISequence<Dafny.Rune> _1836_n = _h172.dtor_DecLiteral_a0;
            Dafny.ISequence<Dafny.Rune> _1837_d = _h172.dtor_DecLiteral_a1;
            DAST._IType _1838_t = _h172.dtor_DecLiteral_a2;
            {
              DAST._IType _source99 = _1838_t;
              {
                if (_source99.is_Primitive) {
                  DAST._IPrimitive _h71 = _source99.dtor_Primitive_a0;
                  if (_h71.is_Real) {
                    {
                      r = RAST.__default.RcNew(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::dafny_runtime::BigRational::new(::dafny_runtime::BigInt::parse_bytes(b\""), _1836_n), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\", 10).unwrap(), ::dafny_runtime::BigInt::parse_bytes(b\"")), _1837_d), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\", 10).unwrap())"))));
                    }
                    goto after_match30;
                  }
                }
              }
              {
                DAST._IType _1839_o = _source99;
                {
                  RAST._IType _1840_genType;
                  RAST._IType _out214;
                  _out214 = (this).GenType(_1839_o, DCOMP.GenTypeContext.@default());
                  _1840_genType = _out214;
                  r = RAST.Expr.create_TypeAscription(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), _1836_n), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".0 / ")), _1837_d), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".0")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"))), _1840_genType);
                }
              }
            after_match30: ;
              RAST._IExpr _out215;
              DCOMP._IOwnership _out216;
              (this).FromOwned(r, expectedOwnership, out _out215, out _out216);
              r = _out215;
              resultingOwnership = _out216;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match28;
          }
        }
      }
      {
        if (_source97.is_Literal) {
          DAST._ILiteral _h173 = _source97.dtor_Literal_a0;
          if (_h173.is_StringLiteral) {
            Dafny.ISequence<Dafny.Rune> _1841_l = _h173.dtor_StringLiteral_a0;
            bool _1842_verbatim = _h173.dtor_verbatim;
            {
              r = (((RAST.__default.dafny__runtime).MSel((this).string__of)).AsExpr()).Apply1(RAST.Expr.create_LiteralString(_1841_l, false, _1842_verbatim));
              RAST._IExpr _out217;
              DCOMP._IOwnership _out218;
              (this).FromOwned(r, expectedOwnership, out _out217, out _out218);
              r = _out217;
              resultingOwnership = _out218;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match28;
          }
        }
      }
      {
        if (_source97.is_Literal) {
          DAST._ILiteral _h174 = _source97.dtor_Literal_a0;
          if (_h174.is_CharLiteralUTF16) {
            BigInteger _1843_c = _h174.dtor_CharLiteralUTF16_a0;
            {
              r = RAST.Expr.create_LiteralInt(Std.Strings.__default.OfNat(_1843_c));
              r = RAST.Expr.create_TypeAscription(r, RAST.Type.create_U16());
              r = (((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsExpr()).Apply1(r);
              RAST._IExpr _out219;
              DCOMP._IOwnership _out220;
              (this).FromOwned(r, expectedOwnership, out _out219, out _out220);
              r = _out219;
              resultingOwnership = _out220;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match28;
          }
        }
      }
      {
        if (_source97.is_Literal) {
          DAST._ILiteral _h175 = _source97.dtor_Literal_a0;
          if (_h175.is_CharLiteral) {
            Dafny.Rune _1844_c = _h175.dtor_CharLiteral_a0;
            {
              r = RAST.Expr.create_LiteralInt(Std.Strings.__default.OfNat(new BigInteger((_1844_c).Value)));
              if (!((this).UnicodeChars)) {
                r = RAST.Expr.create_TypeAscription(r, RAST.Type.create_U16());
              } else {
                r = (((((((RAST.__default.@global).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("std"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("primitive"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("char"))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_u32"))).Apply1(r)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unwrap"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
              }
              r = (((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsExpr()).Apply1(r);
              RAST._IExpr _out221;
              DCOMP._IOwnership _out222;
              (this).FromOwned(r, expectedOwnership, out _out221, out _out222);
              r = _out221;
              resultingOwnership = _out222;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match28;
          }
        }
      }
      {
        DAST._ILiteral _h176 = _source97.dtor_Literal_a0;
        DAST._IType _1845_tpe = _h176.dtor_Null_a0;
        {
          RAST._IType _1846_tpeGen;
          RAST._IType _out223;
          _out223 = (this).GenType(_1845_tpe, DCOMP.GenTypeContext.@default());
          _1846_tpeGen = _out223;
          if (((this).ObjectType).is_RawPointers) {
            r = ((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("ptr"))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("null_mut"));
          } else {
            r = RAST.Expr.create_TypeAscription((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Object"))).AsExpr()).Apply1(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("None"))), _1846_tpeGen);
          }
          RAST._IExpr _out224;
          DCOMP._IOwnership _out225;
          (this).FromOwned(r, expectedOwnership, out _out224, out _out225);
          r = _out224;
          resultingOwnership = _out225;
          readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
          return ;
        }
      }
    after_match28: ;
    }
    public void GenExprBinary(DAST._IExpression e, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, DCOMP._IOwnership expectedOwnership, out RAST._IExpr r, out DCOMP._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = DCOMP.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _let_tmp_rhs56 = e;
      DAST._IBinOp _1847_op = _let_tmp_rhs56.dtor_op;
      DAST._IExpression _1848_lExpr = _let_tmp_rhs56.dtor_left;
      DAST._IExpression _1849_rExpr = _let_tmp_rhs56.dtor_right;
      DAST.Format._IBinaryOpFormat _1850_format = _let_tmp_rhs56.dtor_format2;
      bool _1851_becomesLeftCallsRight;
      DAST._IBinOp _source100 = _1847_op;
      {
        bool disjunctiveMatch13 = false;
        if (_source100.is_SetMerge) {
          disjunctiveMatch13 = true;
        }
        if (_source100.is_SetSubtraction) {
          disjunctiveMatch13 = true;
        }
        if (_source100.is_SetIntersection) {
          disjunctiveMatch13 = true;
        }
        if (_source100.is_SetDisjoint) {
          disjunctiveMatch13 = true;
        }
        if (_source100.is_MapMerge) {
          disjunctiveMatch13 = true;
        }
        if (_source100.is_MapSubtraction) {
          disjunctiveMatch13 = true;
        }
        if (_source100.is_MultisetMerge) {
          disjunctiveMatch13 = true;
        }
        if (_source100.is_MultisetSubtraction) {
          disjunctiveMatch13 = true;
        }
        if (_source100.is_MultisetIntersection) {
          disjunctiveMatch13 = true;
        }
        if (_source100.is_MultisetDisjoint) {
          disjunctiveMatch13 = true;
        }
        if (_source100.is_Concat) {
          disjunctiveMatch13 = true;
        }
        if (disjunctiveMatch13) {
          _1851_becomesLeftCallsRight = true;
          goto after_match31;
        }
      }
      {
        _1851_becomesLeftCallsRight = false;
      }
    after_match31: ;
      bool _1852_becomesRightCallsLeft;
      DAST._IBinOp _source101 = _1847_op;
      {
        if (_source101.is_In) {
          _1852_becomesRightCallsLeft = true;
          goto after_match32;
        }
      }
      {
        _1852_becomesRightCallsLeft = false;
      }
    after_match32: ;
      bool _1853_becomesCallLeftRight;
      DAST._IBinOp _source102 = _1847_op;
      {
        if (_source102.is_Eq) {
          bool referential0 = _source102.dtor_referential;
          if ((referential0) == (true)) {
            _1853_becomesCallLeftRight = false;
            goto after_match33;
          }
        }
      }
      {
        if (_source102.is_SetMerge) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        if (_source102.is_SetSubtraction) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        if (_source102.is_SetIntersection) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        if (_source102.is_SetDisjoint) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        if (_source102.is_MapMerge) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        if (_source102.is_MapSubtraction) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        if (_source102.is_MultisetMerge) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        if (_source102.is_MultisetSubtraction) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        if (_source102.is_MultisetIntersection) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        if (_source102.is_MultisetDisjoint) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        if (_source102.is_Concat) {
          _1853_becomesCallLeftRight = true;
          goto after_match33;
        }
      }
      {
        _1853_becomesCallLeftRight = false;
      }
    after_match33: ;
      DCOMP._IOwnership _1854_expectedLeftOwnership;
      if (_1851_becomesLeftCallsRight) {
        _1854_expectedLeftOwnership = DCOMP.Ownership.create_OwnershipAutoBorrowed();
      } else if ((_1852_becomesRightCallsLeft) || (_1853_becomesCallLeftRight)) {
        _1854_expectedLeftOwnership = DCOMP.Ownership.create_OwnershipBorrowed();
      } else {
        _1854_expectedLeftOwnership = DCOMP.Ownership.create_OwnershipOwned();
      }
      DCOMP._IOwnership _1855_expectedRightOwnership;
      if ((_1851_becomesLeftCallsRight) || (_1853_becomesCallLeftRight)) {
        _1855_expectedRightOwnership = DCOMP.Ownership.create_OwnershipBorrowed();
      } else if (_1852_becomesRightCallsLeft) {
        _1855_expectedRightOwnership = DCOMP.Ownership.create_OwnershipAutoBorrowed();
      } else {
        _1855_expectedRightOwnership = DCOMP.Ownership.create_OwnershipOwned();
      }
      RAST._IExpr _1856_left;
      DCOMP._IOwnership _1857___v113;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1858_recIdentsL;
      RAST._IExpr _out226;
      DCOMP._IOwnership _out227;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out228;
      (this).GenExpr(_1848_lExpr, selfIdent, env, _1854_expectedLeftOwnership, out _out226, out _out227, out _out228);
      _1856_left = _out226;
      _1857___v113 = _out227;
      _1858_recIdentsL = _out228;
      RAST._IExpr _1859_right;
      DCOMP._IOwnership _1860___v114;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1861_recIdentsR;
      RAST._IExpr _out229;
      DCOMP._IOwnership _out230;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out231;
      (this).GenExpr(_1849_rExpr, selfIdent, env, _1855_expectedRightOwnership, out _out229, out _out230, out _out231);
      _1859_right = _out229;
      _1860___v114 = _out230;
      _1861_recIdentsR = _out231;
      DAST._IBinOp _source103 = _1847_op;
      {
        if (_source103.is_In) {
          {
            r = ((_1859_right).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("contains"))).Apply1(_1856_left);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_SeqProperPrefix) {
          r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), _1856_left, _1859_right, _1850_format);
          goto after_match34;
        }
      }
      {
        if (_source103.is_SeqPrefix) {
          r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<="), _1856_left, _1859_right, _1850_format);
          goto after_match34;
        }
      }
      {
        if (_source103.is_SetMerge) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("merge"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_SetSubtraction) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("subtract"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_SetIntersection) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("intersect"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_Subset) {
          {
            r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<="), _1856_left, _1859_right, _1850_format);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_ProperSubset) {
          {
            r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), _1856_left, _1859_right, _1850_format);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_SetDisjoint) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("disjoint"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_MapMerge) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("merge"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_MapSubtraction) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("subtract"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_MultisetMerge) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("merge"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_MultisetSubtraction) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("subtract"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_MultisetIntersection) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("intersect"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_Submultiset) {
          {
            r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<="), _1856_left, _1859_right, _1850_format);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_ProperSubmultiset) {
          {
            r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), _1856_left, _1859_right, _1850_format);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_MultisetDisjoint) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("disjoint"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        if (_source103.is_Concat) {
          {
            r = ((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("concat"))).Apply1(_1859_right);
          }
          goto after_match34;
        }
      }
      {
        {
          if ((DCOMP.COMP.OpTable).Contains(_1847_op)) {
            r = RAST.Expr.create_BinaryOp(Dafny.Map<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>.Select(DCOMP.COMP.OpTable,_1847_op), _1856_left, _1859_right, _1850_format);
          } else {
            DAST._IBinOp _source104 = _1847_op;
            {
              if (_source104.is_Eq) {
                bool _1862_referential = _source104.dtor_referential;
                {
                  if (_1862_referential) {
                    if (((this).ObjectType).is_RawPointers) {
                      (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Cannot compare raw pointers yet - need to wrap them with a structure to ensure they are compared properly"));
                      r = RAST.Expr.create_RawExpr((this.error).dtor_value);
                    } else {
                      r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="), _1856_left, _1859_right, DAST.Format.BinaryOpFormat.create_NoFormat());
                    }
                  } else {
                    if (((_1849_rExpr).is_SeqValue) && ((new BigInteger(((_1849_rExpr).dtor_elements).Count)).Sign == 0)) {
                      r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="), ((((_1856_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("to_array"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements())).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("len"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements()), RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")), DAST.Format.BinaryOpFormat.create_NoFormat());
                    } else if (((_1848_lExpr).is_SeqValue) && ((new BigInteger(((_1848_lExpr).dtor_elements).Count)).Sign == 0)) {
                      r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="), RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")), ((((_1859_right).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("to_array"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements())).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("len"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements()), DAST.Format.BinaryOpFormat.create_NoFormat());
                    } else {
                      r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="), _1856_left, _1859_right, DAST.Format.BinaryOpFormat.create_NoFormat());
                    }
                  }
                }
                goto after_match35;
              }
            }
            {
              if (_source104.is_EuclidianDiv) {
                {
                  r = (RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::dafny_runtime::euclidian_division"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_1856_left, _1859_right));
                }
                goto after_match35;
              }
            }
            {
              if (_source104.is_EuclidianMod) {
                {
                  r = (RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::dafny_runtime::euclidian_modulo"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_1856_left, _1859_right));
                }
                goto after_match35;
              }
            }
            {
              Dafny.ISequence<Dafny.Rune> _1863_op = _source104.dtor_Passthrough_a0;
              {
                r = RAST.Expr.create_BinaryOp(_1863_op, _1856_left, _1859_right, _1850_format);
              }
            }
          after_match35: ;
          }
        }
      }
    after_match34: ;
      RAST._IExpr _out232;
      DCOMP._IOwnership _out233;
      (this).FromOwned(r, expectedOwnership, out _out232, out _out233);
      r = _out232;
      resultingOwnership = _out233;
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_1858_recIdentsL, _1861_recIdentsR);
      return ;
    }
    public void GenExprConvertToNewtype(DAST._IExpression e, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, DCOMP._IOwnership expectedOwnership, out RAST._IExpr r, out DCOMP._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = DCOMP.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _let_tmp_rhs57 = e;
      DAST._IExpression _1864_expr = _let_tmp_rhs57.dtor_value;
      DAST._IType _1865_fromTpe = _let_tmp_rhs57.dtor_from;
      DAST._IType _1866_toTpe = _let_tmp_rhs57.dtor_typ;
      DAST._IType _let_tmp_rhs58 = _1866_toTpe;
      DAST._IResolvedType _let_tmp_rhs59 = _let_tmp_rhs58.dtor_resolved;
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1867_path = _let_tmp_rhs59.dtor_path;
      Dafny.ISequence<DAST._IType> _1868_typeArgs = _let_tmp_rhs59.dtor_typeArgs;
      DAST._IResolvedTypeBase _let_tmp_rhs60 = _let_tmp_rhs59.dtor_kind;
      DAST._IType _1869_b = _let_tmp_rhs60.dtor_baseType;
      DAST._INewtypeRange _1870_range = _let_tmp_rhs60.dtor_range;
      bool _1871_erase = _let_tmp_rhs60.dtor_erase;
      Dafny.ISequence<DAST._IAttribute> _1872___v116 = _let_tmp_rhs59.dtor_attributes;
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1873___v117 = _let_tmp_rhs59.dtor_properMethods;
      Dafny.ISequence<DAST._IType> _1874___v118 = _let_tmp_rhs59.dtor_extendedTypes;
      Std.Wrappers._IOption<RAST._IType> _1875_nativeToType;
      _1875_nativeToType = DCOMP.COMP.NewtypeRangeToRustType(_1870_range);
      if (object.Equals(_1865_fromTpe, _1869_b)) {
        RAST._IExpr _1876_recursiveGen;
        DCOMP._IOwnership _1877_recOwned;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1878_recIdents;
        RAST._IExpr _out234;
        DCOMP._IOwnership _out235;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out236;
        (this).GenExpr(_1864_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out234, out _out235, out _out236);
        _1876_recursiveGen = _out234;
        _1877_recOwned = _out235;
        _1878_recIdents = _out236;
        readIdents = _1878_recIdents;
        Std.Wrappers._IOption<RAST._IType> _source105 = _1875_nativeToType;
        {
          if (_source105.is_Some) {
            RAST._IType _1879_v = _source105.dtor_value;
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("truncate!"))).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_1876_recursiveGen, RAST.Expr.create_ExprFromType(_1879_v)));
            RAST._IExpr _out237;
            DCOMP._IOwnership _out238;
            (this).FromOwned(r, expectedOwnership, out _out237, out _out238);
            r = _out237;
            resultingOwnership = _out238;
            goto after_match36;
          }
        }
        {
          if (_1871_erase) {
            r = _1876_recursiveGen;
          } else {
            RAST._IType _1880_rhsType;
            RAST._IType _out239;
            _out239 = (this).GenType(_1866_toTpe, DCOMP.GenTypeContext.@default());
            _1880_rhsType = _out239;
            r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat((_1880_rhsType)._ToString(DCOMP.__default.IND), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")), (_1876_recursiveGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")")));
          }
          RAST._IExpr _out240;
          DCOMP._IOwnership _out241;
          (this).FromOwnership(r, _1877_recOwned, expectedOwnership, out _out240, out _out241);
          r = _out240;
          resultingOwnership = _out241;
        }
      after_match36: ;
      } else {
        if ((_1875_nativeToType).is_Some) {
          DAST._IType _source106 = _1865_fromTpe;
          {
            if (_source106.is_UserDefined) {
              DAST._IResolvedType resolved1 = _source106.dtor_resolved;
              DAST._IResolvedTypeBase kind1 = resolved1.dtor_kind;
              if (kind1.is_Newtype) {
                DAST._IType _1881_b0 = kind1.dtor_baseType;
                DAST._INewtypeRange _1882_range0 = kind1.dtor_range;
                bool _1883_erase0 = kind1.dtor_erase;
                Dafny.ISequence<DAST._IAttribute> _1884_attributes0 = resolved1.dtor_attributes;
                {
                  Std.Wrappers._IOption<RAST._IType> _1885_nativeFromType;
                  _1885_nativeFromType = DCOMP.COMP.NewtypeRangeToRustType(_1882_range0);
                  if ((_1885_nativeFromType).is_Some) {
                    RAST._IExpr _1886_recursiveGen;
                    DCOMP._IOwnership _1887_recOwned;
                    Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1888_recIdents;
                    RAST._IExpr _out242;
                    DCOMP._IOwnership _out243;
                    Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out244;
                    (this).GenExpr(_1864_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out242, out _out243, out _out244);
                    _1886_recursiveGen = _out242;
                    _1887_recOwned = _out243;
                    _1888_recIdents = _out244;
                    RAST._IExpr _out245;
                    DCOMP._IOwnership _out246;
                    (this).FromOwnership(RAST.Expr.create_TypeAscription(_1886_recursiveGen, (_1875_nativeToType).dtor_value), _1887_recOwned, expectedOwnership, out _out245, out _out246);
                    r = _out245;
                    resultingOwnership = _out246;
                    readIdents = _1888_recIdents;
                    return ;
                  }
                }
                goto after_match37;
              }
            }
          }
          {
          }
        after_match37: ;
          if (object.Equals(_1865_fromTpe, DAST.Type.create_Primitive(DAST.Primitive.create_Char()))) {
            RAST._IExpr _1889_recursiveGen;
            DCOMP._IOwnership _1890_recOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1891_recIdents;
            RAST._IExpr _out247;
            DCOMP._IOwnership _out248;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out249;
            (this).GenExpr(_1864_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out247, out _out248, out _out249);
            _1889_recursiveGen = _out247;
            _1890_recOwned = _out248;
            _1891_recIdents = _out249;
            RAST._IExpr _out250;
            DCOMP._IOwnership _out251;
            (this).FromOwnership(RAST.Expr.create_TypeAscription((_1889_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")), (_1875_nativeToType).dtor_value), _1890_recOwned, expectedOwnership, out _out250, out _out251);
            r = _out250;
            resultingOwnership = _out251;
            readIdents = _1891_recIdents;
            return ;
          }
        }
        RAST._IExpr _out252;
        DCOMP._IOwnership _out253;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out254;
        (this).GenExpr(DAST.Expression.create_Convert(DAST.Expression.create_Convert(_1864_expr, _1865_fromTpe, _1869_b), _1869_b, _1866_toTpe), selfIdent, env, expectedOwnership, out _out252, out _out253, out _out254);
        r = _out252;
        resultingOwnership = _out253;
        readIdents = _out254;
      }
    }
    public void GenExprConvertFromNewtype(DAST._IExpression e, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, DCOMP._IOwnership expectedOwnership, out RAST._IExpr r, out DCOMP._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = DCOMP.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _let_tmp_rhs61 = e;
      DAST._IExpression _1892_expr = _let_tmp_rhs61.dtor_value;
      DAST._IType _1893_fromTpe = _let_tmp_rhs61.dtor_from;
      DAST._IType _1894_toTpe = _let_tmp_rhs61.dtor_typ;
      DAST._IType _let_tmp_rhs62 = _1893_fromTpe;
      DAST._IResolvedType _let_tmp_rhs63 = _let_tmp_rhs62.dtor_resolved;
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1895___v124 = _let_tmp_rhs63.dtor_path;
      Dafny.ISequence<DAST._IType> _1896___v125 = _let_tmp_rhs63.dtor_typeArgs;
      DAST._IResolvedTypeBase _let_tmp_rhs64 = _let_tmp_rhs63.dtor_kind;
      DAST._IType _1897_b = _let_tmp_rhs64.dtor_baseType;
      DAST._INewtypeRange _1898_range = _let_tmp_rhs64.dtor_range;
      bool _1899_erase = _let_tmp_rhs64.dtor_erase;
      Dafny.ISequence<DAST._IAttribute> _1900_attributes = _let_tmp_rhs63.dtor_attributes;
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1901___v126 = _let_tmp_rhs63.dtor_properMethods;
      Dafny.ISequence<DAST._IType> _1902___v127 = _let_tmp_rhs63.dtor_extendedTypes;
      Std.Wrappers._IOption<RAST._IType> _1903_nativeFromType;
      _1903_nativeFromType = DCOMP.COMP.NewtypeRangeToRustType(_1898_range);
      if (object.Equals(_1897_b, _1894_toTpe)) {
        RAST._IExpr _1904_recursiveGen;
        DCOMP._IOwnership _1905_recOwned;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1906_recIdents;
        RAST._IExpr _out255;
        DCOMP._IOwnership _out256;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out257;
        (this).GenExpr(_1892_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out255, out _out256, out _out257);
        _1904_recursiveGen = _out255;
        _1905_recOwned = _out256;
        _1906_recIdents = _out257;
        readIdents = _1906_recIdents;
        Std.Wrappers._IOption<RAST._IType> _source107 = _1903_nativeFromType;
        {
          if (_source107.is_Some) {
            RAST._IType _1907_v = _source107.dtor_value;
            RAST._IType _1908_toTpeRust;
            RAST._IType _out258;
            _out258 = (this).GenType(_1894_toTpe, DCOMP.GenTypeContext.@default());
            _1908_toTpeRust = _out258;
            r = ((((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("convert"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Into"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_1908_toTpeRust))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("into"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_1904_recursiveGen));
            RAST._IExpr _out259;
            DCOMP._IOwnership _out260;
            (this).FromOwned(r, expectedOwnership, out _out259, out _out260);
            r = _out259;
            resultingOwnership = _out260;
            goto after_match38;
          }
        }
        {
          if (_1899_erase) {
            r = _1904_recursiveGen;
          } else {
            r = (_1904_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0"));
          }
          RAST._IExpr _out261;
          DCOMP._IOwnership _out262;
          (this).FromOwnership(r, _1905_recOwned, expectedOwnership, out _out261, out _out262);
          r = _out261;
          resultingOwnership = _out262;
        }
      after_match38: ;
      } else {
        if ((_1903_nativeFromType).is_Some) {
          if (object.Equals(_1894_toTpe, DAST.Type.create_Primitive(DAST.Primitive.create_Char()))) {
            RAST._IExpr _1909_recursiveGen;
            DCOMP._IOwnership _1910_recOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1911_recIdents;
            RAST._IExpr _out263;
            DCOMP._IOwnership _out264;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out265;
            (this).GenExpr(_1892_expr, selfIdent, env, expectedOwnership, out _out263, out _out264, out _out265);
            _1909_recursiveGen = _out263;
            _1910_recOwned = _out264;
            _1911_recIdents = _out265;
            RAST._IExpr _out266;
            DCOMP._IOwnership _out267;
            (this).FromOwnership((((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsExpr()).Apply1(RAST.Expr.create_TypeAscription(_1909_recursiveGen, (this).DafnyCharUnderlying)), _1910_recOwned, expectedOwnership, out _out266, out _out267);
            r = _out266;
            resultingOwnership = _out267;
            readIdents = _1911_recIdents;
            return ;
          }
        }
        RAST._IExpr _out268;
        DCOMP._IOwnership _out269;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out270;
        (this).GenExpr(DAST.Expression.create_Convert(DAST.Expression.create_Convert(_1892_expr, _1893_fromTpe, _1897_b), _1897_b, _1894_toTpe), selfIdent, env, expectedOwnership, out _out268, out _out269, out _out270);
        r = _out268;
        resultingOwnership = _out269;
        readIdents = _out270;
      }
    }
    public bool IsBuiltinCollection(DAST._IType typ) {
      return ((((typ).is_Seq) || ((typ).is_Set)) || ((typ).is_Map)) || ((typ).is_Multiset);
    }
    public DAST._IType GetBuiltinCollectionElement(DAST._IType typ) {
      if ((typ).is_Map) {
        return (typ).dtor_value;
      } else {
        return (typ).dtor_element;
      }
    }
    public bool SameTypesButDifferentTypeParameters(DAST._IType fromType, RAST._IType fromTpe, DAST._IType toType, RAST._IType toTpe)
    {
      return (((((((fromTpe).is_TypeApp) && ((toTpe).is_TypeApp)) && (object.Equals((fromTpe).dtor_baseName, (toTpe).dtor_baseName))) && ((fromType).is_UserDefined)) && ((toType).is_UserDefined)) && ((this).IsSameResolvedTypeAnyArgs((fromType).dtor_resolved, (toType).dtor_resolved))) && ((((new BigInteger((((fromType).dtor_resolved).dtor_typeArgs).Count)) == (new BigInteger((((toType).dtor_resolved).dtor_typeArgs).Count))) && ((new BigInteger((((toType).dtor_resolved).dtor_typeArgs).Count)) == (new BigInteger(((fromTpe).dtor_arguments).Count)))) && ((new BigInteger(((fromTpe).dtor_arguments).Count)) == (new BigInteger(((toTpe).dtor_arguments).Count))));
    }
    public Std.Wrappers._IResult<Dafny.ISequence<__T>, __E> SeqResultToResultSeq<__T, __E>(Dafny.ISequence<Std.Wrappers._IResult<__T, __E>> xs) {
      if ((new BigInteger((xs).Count)).Sign == 0) {
        return Std.Wrappers.Result<Dafny.ISequence<__T>, __E>.create_Success(Dafny.Sequence<__T>.FromElements());
      } else {
        Std.Wrappers._IResult<__T, __E> _1912_valueOrError0 = (xs).Select(BigInteger.Zero);
        if ((_1912_valueOrError0).IsFailure()) {
          return (_1912_valueOrError0).PropagateFailure<Dafny.ISequence<__T>>();
        } else {
          __T _1913_head = (_1912_valueOrError0).Extract();
          Std.Wrappers._IResult<Dafny.ISequence<__T>, __E> _1914_valueOrError1 = (this).SeqResultToResultSeq<__T, __E>((xs).Drop(BigInteger.One));
          if ((_1914_valueOrError1).IsFailure()) {
            return (_1914_valueOrError1).PropagateFailure<Dafny.ISequence<__T>>();
          } else {
            Dafny.ISequence<__T> _1915_tail = (_1914_valueOrError1).Extract();
            return Std.Wrappers.Result<Dafny.ISequence<__T>, __E>.create_Success(Dafny.Sequence<__T>.Concat(Dafny.Sequence<__T>.FromElements(_1913_head), _1915_tail));
          }
        }
      }
    }
    public Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> UpcastConversionLambda(DAST._IType fromType, RAST._IType fromTpe, DAST._IType toType, RAST._IType toTpe, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr> typeParams)
    {
      var _pat_let_tv4 = fromType;
      var _pat_let_tv5 = fromTpe;
      var _pat_let_tv6 = toType;
      var _pat_let_tv7 = toTpe;
      var _pat_let_tv8 = typeParams;
      if (object.Equals(fromTpe, toTpe)) {
        return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("upcast_id"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(fromTpe))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements()));
      } else if (((fromTpe).IsObjectOrPointer()) && ((toTpe).IsObjectOrPointer())) {
        if (!(((toTpe).ObjectOrPointerUnderlying()).is_DynType)) {
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Failure(_System.Tuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>.create(fromType, fromTpe, toType, toTpe, typeParams));
        } else {
          RAST._IType _1916_fromTpeUnderlying = (fromTpe).ObjectOrPointerUnderlying();
          RAST._IType _1917_toTpeUnderlying = (toTpe).ObjectOrPointerUnderlying();
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(((((RAST.__default.dafny__runtime).MSel((this).upcast)).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_1916_fromTpeUnderlying, _1917_toTpeUnderlying))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements()));
        }
      } else if ((typeParams).Contains(_System.Tuple2<RAST._IType, RAST._IType>.create(fromTpe, toTpe))) {
        return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(Dafny.Map<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>.Select(typeParams,_System.Tuple2<RAST._IType, RAST._IType>.create(fromTpe, toTpe)));
      } else if (((fromTpe).IsRc()) && ((toTpe).IsRc())) {
        Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _1918_valueOrError0 = (this).UpcastConversionLambda(fromType, (fromTpe).RcUnderlying(), toType, (toTpe).RcUnderlying(), typeParams);
        if ((_1918_valueOrError0).IsFailure()) {
          return (_1918_valueOrError0).PropagateFailure<RAST._IExpr>();
        } else {
          RAST._IExpr _1919_lambda = (_1918_valueOrError0).Extract();
          if ((fromType).is_Arrow) {
            return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(_1919_lambda);
          } else {
            return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("rc_coerce"))).AsExpr()).Apply1(_1919_lambda));
          }
        }
      } else if ((this).SameTypesButDifferentTypeParameters(fromType, fromTpe, toType, toTpe)) {
        Dafny.ISequence<BigInteger> _1920_indices = ((((fromType).is_UserDefined) && ((((fromType).dtor_resolved).dtor_kind).is_Datatype)) ? (Std.Collections.Seq.__default.Filter<BigInteger>(Dafny.Helpers.Id<Func<RAST._IType, DAST._IType, Func<BigInteger, bool>>>((_1921_fromTpe, _1922_fromType) => ((System.Func<BigInteger, bool>)((_1923_i) => {
          return ((((_1923_i).Sign != -1) && ((_1923_i) < (new BigInteger(((_1921_fromTpe).dtor_arguments).Count)))) ? (!(((_1923_i).Sign != -1) && ((_1923_i) < (new BigInteger(((((_1922_fromType).dtor_resolved).dtor_kind).dtor_variances).Count)))) || (!((((((_1922_fromType).dtor_resolved).dtor_kind).dtor_variances).Select(_1923_i)).is_Nonvariant))) : (false));
        })))(fromTpe, fromType), ((System.Func<Dafny.ISequence<BigInteger>>) (() => {
          BigInteger dim14 = new BigInteger(((fromTpe).dtor_arguments).Count);
          var arr14 = new BigInteger[Dafny.Helpers.ToIntChecked(dim14, "array size exceeds memory limit")];
          for (int i14 = 0; i14 < dim14; i14++) {
            var _1924_i = (BigInteger) i14;
            arr14[(int)(_1924_i)] = _1924_i;
          }
          return Dafny.Sequence<BigInteger>.FromArray(arr14);
        }))())) : (((System.Func<Dafny.ISequence<BigInteger>>) (() => {
          BigInteger dim15 = new BigInteger(((fromTpe).dtor_arguments).Count);
          var arr15 = new BigInteger[Dafny.Helpers.ToIntChecked(dim15, "array size exceeds memory limit")];
          for (int i15 = 0; i15 < dim15; i15++) {
            var _1925_i = (BigInteger) i15;
            arr15[(int)(_1925_i)] = _1925_i;
          }
          return Dafny.Sequence<BigInteger>.FromArray(arr15);
        }))()));
        Std.Wrappers._IResult<Dafny.ISequence<RAST._IExpr>, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _1926_valueOrError1 = (this).SeqResultToResultSeq<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>(((System.Func<Dafny.ISequence<Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>>>) (() => {
          BigInteger dim16 = new BigInteger((_1920_indices).Count);
          var arr16 = new Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>[Dafny.Helpers.ToIntChecked(dim16, "array size exceeds memory limit")];
          for (int i16 = 0; i16 < dim16; i16++) {
            var _1927_j = (BigInteger) i16;
            arr16[(int)(_1927_j)] = Dafny.Helpers.Let<BigInteger, Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>>((_1920_indices).Select(_1927_j), _pat_let24_0 => Dafny.Helpers.Let<BigInteger, Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>>(_pat_let24_0, _1928_i => (this).UpcastConversionLambda((((_pat_let_tv4).dtor_resolved).dtor_typeArgs).Select(_1928_i), ((_pat_let_tv5).dtor_arguments).Select(_1928_i), (((_pat_let_tv6).dtor_resolved).dtor_typeArgs).Select(_1928_i), ((_pat_let_tv7).dtor_arguments).Select(_1928_i), _pat_let_tv8)));
          }
          return Dafny.Sequence<Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>>.FromArray(arr16);
        }))());
        if ((_1926_valueOrError1).IsFailure()) {
          return (_1926_valueOrError1).PropagateFailure<RAST._IExpr>();
        } else {
          Dafny.ISequence<RAST._IExpr> _1929_lambdas = (_1926_valueOrError1).Extract();
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success((((RAST.Expr.create_ExprFromType((fromTpe).dtor_baseName)).ApplyType(((System.Func<Dafny.ISequence<RAST._IType>>) (() => {
  BigInteger dim17 = new BigInteger(((fromTpe).dtor_arguments).Count);
  var arr17 = new RAST._IType[Dafny.Helpers.ToIntChecked(dim17, "array size exceeds memory limit")];
  for (int i17 = 0; i17 < dim17; i17++) {
    var _1930_i = (BigInteger) i17;
    arr17[(int)(_1930_i)] = ((fromTpe).dtor_arguments).Select(_1930_i);
  }
  return Dafny.Sequence<RAST._IType>.FromArray(arr17);
}))())).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("coerce"))).Apply(_1929_lambdas));
        }
      } else if (((((fromTpe).IsBuiltinCollection()) && ((toTpe).IsBuiltinCollection())) && ((this).IsBuiltinCollection(fromType))) && ((this).IsBuiltinCollection(toType))) {
        RAST._IType _1931_newFromTpe = (fromTpe).GetBuiltinCollectionElement();
        RAST._IType _1932_newToTpe = (toTpe).GetBuiltinCollectionElement();
        DAST._IType _1933_newFromType = (this).GetBuiltinCollectionElement(fromType);
        DAST._IType _1934_newToType = (this).GetBuiltinCollectionElement(toType);
        Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _1935_valueOrError2 = (this).UpcastConversionLambda(_1933_newFromType, _1931_newFromTpe, _1934_newToType, _1932_newToTpe, typeParams);
        if ((_1935_valueOrError2).IsFailure()) {
          return (_1935_valueOrError2).PropagateFailure<RAST._IExpr>();
        } else {
          RAST._IExpr _1936_coerceArg = (_1935_valueOrError2).Extract();
          RAST._IPath _1937_collectionType = (RAST.__default.dafny__runtime).MSel(((((fromTpe).Expand()).dtor_baseName).dtor_path).dtor_name);
          RAST._IExpr _1938_baseType = (((((((fromTpe).Expand()).dtor_baseName).dtor_path).dtor_name).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Map"))) ? (((_1937_collectionType).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements((((fromTpe).Expand()).dtor_arguments).Select(BigInteger.Zero), _1931_newFromTpe))) : (((_1937_collectionType).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_1931_newFromTpe))));
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(((_1938_baseType).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("coerce"))).Apply1(_1936_coerceArg));
        }
      } else if ((((((((((fromTpe).is_DynType) && (((fromTpe).dtor_underlying).is_FnType)) && ((toTpe).is_DynType)) && (((toTpe).dtor_underlying).is_FnType)) && ((((fromTpe).dtor_underlying).dtor_arguments).Equals(((toTpe).dtor_underlying).dtor_arguments))) && ((fromType).is_Arrow)) && ((toType).is_Arrow)) && ((new BigInteger((((fromTpe).dtor_underlying).dtor_arguments).Count)) == (BigInteger.One))) && (((((fromTpe).dtor_underlying).dtor_arguments).Select(BigInteger.Zero)).is_Borrowed)) {
        Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _1939_valueOrError3 = (this).UpcastConversionLambda((fromType).dtor_result, ((fromTpe).dtor_underlying).dtor_returnType, (toType).dtor_result, ((toTpe).dtor_underlying).dtor_returnType, typeParams);
        if ((_1939_valueOrError3).IsFailure()) {
          return (_1939_valueOrError3).PropagateFailure<RAST._IExpr>();
        } else {
          RAST._IExpr _1940_lambda = (_1939_valueOrError3).Extract();
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fn1_coerce"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(((((fromTpe).dtor_underlying).dtor_arguments).Select(BigInteger.Zero)).dtor_underlying, ((fromTpe).dtor_underlying).dtor_returnType, ((toTpe).dtor_underlying).dtor_returnType))).Apply1(_1940_lambda));
        }
      } else {
        return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Failure(_System.Tuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>.create(fromType, fromTpe, toType, toTpe, typeParams));
      }
    }
    public bool IsDowncastConversion(RAST._IType fromTpe, RAST._IType toTpe)
    {
      if (((fromTpe).IsObjectOrPointer()) && ((toTpe).IsObjectOrPointer())) {
        return (((fromTpe).ObjectOrPointerUnderlying()).is_DynType) && (!(((toTpe).ObjectOrPointerUnderlying()).is_DynType));
      } else {
        return false;
      }
    }
    public void GenExprConvertOther(DAST._IExpression e, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, DCOMP._IOwnership expectedOwnership, out RAST._IExpr r, out DCOMP._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = DCOMP.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _let_tmp_rhs65 = e;
      DAST._IExpression _1941_expr = _let_tmp_rhs65.dtor_value;
      DAST._IType _1942_fromTpe = _let_tmp_rhs65.dtor_from;
      DAST._IType _1943_toTpe = _let_tmp_rhs65.dtor_typ;
      RAST._IType _1944_fromTpeGen;
      RAST._IType _out271;
      _out271 = (this).GenType(_1942_fromTpe, DCOMP.GenTypeContext.@default());
      _1944_fromTpeGen = _out271;
      RAST._IType _1945_toTpeGen;
      RAST._IType _out272;
      _out272 = (this).GenType(_1943_toTpe, DCOMP.GenTypeContext.@default());
      _1945_toTpeGen = _out272;
      Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _1946_upcastConverter;
      _1946_upcastConverter = (this).UpcastConversionLambda(_1942_fromTpe, _1944_fromTpeGen, _1943_toTpe, _1945_toTpeGen, Dafny.Map<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>.FromElements());
      if ((_1946_upcastConverter).is_Success) {
        RAST._IExpr _1947_conversionLambda;
        _1947_conversionLambda = (_1946_upcastConverter).dtor_value;
        RAST._IExpr _1948_recursiveGen;
        DCOMP._IOwnership _1949_recOwned;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1950_recIdents;
        RAST._IExpr _out273;
        DCOMP._IOwnership _out274;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out275;
        (this).GenExpr(_1941_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out273, out _out274, out _out275);
        _1948_recursiveGen = _out273;
        _1949_recOwned = _out274;
        _1950_recIdents = _out275;
        readIdents = _1950_recIdents;
        r = (_1947_conversionLambda).Apply1(_1948_recursiveGen);
        RAST._IExpr _out276;
        DCOMP._IOwnership _out277;
        (this).FromOwnership(r, DCOMP.Ownership.create_OwnershipOwned(), expectedOwnership, out _out276, out _out277);
        r = _out276;
        resultingOwnership = _out277;
      } else if ((this).IsDowncastConversion(_1944_fromTpeGen, _1945_toTpeGen)) {
        RAST._IExpr _1951_recursiveGen;
        DCOMP._IOwnership _1952_recOwned;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1953_recIdents;
        RAST._IExpr _out278;
        DCOMP._IOwnership _out279;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out280;
        (this).GenExpr(_1941_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out278, out _out279, out _out280);
        _1951_recursiveGen = _out278;
        _1952_recOwned = _out279;
        _1953_recIdents = _out280;
        readIdents = _1953_recIdents;
        _1945_toTpeGen = (_1945_toTpeGen).ObjectOrPointerUnderlying();
        r = (((RAST.__default.dafny__runtime).MSel((this).downcast)).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_1951_recursiveGen, RAST.Expr.create_ExprFromType(_1945_toTpeGen)));
        RAST._IExpr _out281;
        DCOMP._IOwnership _out282;
        (this).FromOwnership(r, DCOMP.Ownership.create_OwnershipOwned(), expectedOwnership, out _out281, out _out282);
        r = _out281;
        resultingOwnership = _out282;
      } else {
        RAST._IExpr _1954_recursiveGen;
        DCOMP._IOwnership _1955_recOwned;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1956_recIdents;
        RAST._IExpr _out283;
        DCOMP._IOwnership _out284;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out285;
        (this).GenExpr(_1941_expr, selfIdent, env, expectedOwnership, out _out283, out _out284, out _out285);
        _1954_recursiveGen = _out283;
        _1955_recOwned = _out284;
        _1956_recIdents = _out285;
        readIdents = _1956_recIdents;
        Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _let_tmp_rhs66 = _1946_upcastConverter;
        _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>> _let_tmp_rhs67 = _let_tmp_rhs66.dtor_error;
        DAST._IType _1957_fromType = _let_tmp_rhs67.dtor__0;
        RAST._IType _1958_fromTpeGen = _let_tmp_rhs67.dtor__1;
        DAST._IType _1959_toType = _let_tmp_rhs67.dtor__2;
        RAST._IType _1960_toTpeGen = _let_tmp_rhs67.dtor__3;
        Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr> _1961_m = _let_tmp_rhs67.dtor__4;
        Dafny.ISequence<Dafny.Rune> _1962_msg;
        _1962_msg = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/* <i>Coercion from "), (_1958_fromTpeGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" to ")), (_1960_toTpeGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("</i> not yet implemented */"));
        (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(_1962_msg);
        r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat((_1954_recursiveGen)._ToString(DCOMP.__default.IND), _1962_msg));
        RAST._IExpr _out286;
        DCOMP._IOwnership _out287;
        (this).FromOwnership(r, _1955_recOwned, expectedOwnership, out _out286, out _out287);
        r = _out286;
        resultingOwnership = _out287;
      }
    }
    public void GenExprConvert(DAST._IExpression e, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, DCOMP._IOwnership expectedOwnership, out RAST._IExpr r, out DCOMP._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = DCOMP.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _let_tmp_rhs68 = e;
      DAST._IExpression _1963_expr = _let_tmp_rhs68.dtor_value;
      DAST._IType _1964_fromTpe = _let_tmp_rhs68.dtor_from;
      DAST._IType _1965_toTpe = _let_tmp_rhs68.dtor_typ;
      if (object.Equals(_1964_fromTpe, _1965_toTpe)) {
        RAST._IExpr _1966_recursiveGen;
        DCOMP._IOwnership _1967_recOwned;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1968_recIdents;
        RAST._IExpr _out288;
        DCOMP._IOwnership _out289;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out290;
        (this).GenExpr(_1963_expr, selfIdent, env, expectedOwnership, out _out288, out _out289, out _out290);
        _1966_recursiveGen = _out288;
        _1967_recOwned = _out289;
        _1968_recIdents = _out290;
        r = _1966_recursiveGen;
        RAST._IExpr _out291;
        DCOMP._IOwnership _out292;
        (this).FromOwnership(r, _1967_recOwned, expectedOwnership, out _out291, out _out292);
        r = _out291;
        resultingOwnership = _out292;
        readIdents = _1968_recIdents;
      } else {
        _System._ITuple2<DAST._IType, DAST._IType> _source108 = _System.Tuple2<DAST._IType, DAST._IType>.create(_1964_fromTpe, _1965_toTpe);
        {
          DAST._IType _10 = _source108.dtor__1;
          if (_10.is_UserDefined) {
            DAST._IResolvedType resolved2 = _10.dtor_resolved;
            DAST._IResolvedTypeBase kind2 = resolved2.dtor_kind;
            if (kind2.is_Newtype) {
              DAST._IType _1969_b = kind2.dtor_baseType;
              DAST._INewtypeRange _1970_range = kind2.dtor_range;
              bool _1971_erase = kind2.dtor_erase;
              Dafny.ISequence<DAST._IAttribute> _1972_attributes = resolved2.dtor_attributes;
              {
                RAST._IExpr _out293;
                DCOMP._IOwnership _out294;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out295;
                (this).GenExprConvertToNewtype(e, selfIdent, env, expectedOwnership, out _out293, out _out294, out _out295);
                r = _out293;
                resultingOwnership = _out294;
                readIdents = _out295;
              }
              goto after_match39;
            }
          }
        }
        {
          DAST._IType _00 = _source108.dtor__0;
          if (_00.is_UserDefined) {
            DAST._IResolvedType resolved3 = _00.dtor_resolved;
            DAST._IResolvedTypeBase kind3 = resolved3.dtor_kind;
            if (kind3.is_Newtype) {
              DAST._IType _1973_b = kind3.dtor_baseType;
              DAST._INewtypeRange _1974_range = kind3.dtor_range;
              bool _1975_erase = kind3.dtor_erase;
              Dafny.ISequence<DAST._IAttribute> _1976_attributes = resolved3.dtor_attributes;
              {
                RAST._IExpr _out296;
                DCOMP._IOwnership _out297;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out298;
                (this).GenExprConvertFromNewtype(e, selfIdent, env, expectedOwnership, out _out296, out _out297, out _out298);
                r = _out296;
                resultingOwnership = _out297;
                readIdents = _out298;
              }
              goto after_match39;
            }
          }
        }
        {
          DAST._IType _01 = _source108.dtor__0;
          if (_01.is_Primitive) {
            DAST._IPrimitive _h72 = _01.dtor_Primitive_a0;
            if (_h72.is_Int) {
              DAST._IType _11 = _source108.dtor__1;
              if (_11.is_Primitive) {
                DAST._IPrimitive _h73 = _11.dtor_Primitive_a0;
                if (_h73.is_Real) {
                  {
                    RAST._IExpr _1977_recursiveGen;
                    DCOMP._IOwnership _1978___v138;
                    Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1979_recIdents;
                    RAST._IExpr _out299;
                    DCOMP._IOwnership _out300;
                    Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out301;
                    (this).GenExpr(_1963_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out299, out _out300, out _out301);
                    _1977_recursiveGen = _out299;
                    _1978___v138 = _out300;
                    _1979_recIdents = _out301;
                    r = RAST.__default.RcNew(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::dafny_runtime::BigRational::from_integer("), (_1977_recursiveGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"))));
                    RAST._IExpr _out302;
                    DCOMP._IOwnership _out303;
                    (this).FromOwned(r, expectedOwnership, out _out302, out _out303);
                    r = _out302;
                    resultingOwnership = _out303;
                    readIdents = _1979_recIdents;
                  }
                  goto after_match39;
                }
              }
            }
          }
        }
        {
          DAST._IType _02 = _source108.dtor__0;
          if (_02.is_Primitive) {
            DAST._IPrimitive _h74 = _02.dtor_Primitive_a0;
            if (_h74.is_Real) {
              DAST._IType _12 = _source108.dtor__1;
              if (_12.is_Primitive) {
                DAST._IPrimitive _h75 = _12.dtor_Primitive_a0;
                if (_h75.is_Int) {
                  {
                    RAST._IExpr _1980_recursiveGen;
                    DCOMP._IOwnership _1981___v139;
                    Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1982_recIdents;
                    RAST._IExpr _out304;
                    DCOMP._IOwnership _out305;
                    Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out306;
                    (this).GenExpr(_1963_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out304, out _out305, out _out306);
                    _1980_recursiveGen = _out304;
                    _1981___v139 = _out305;
                    _1982_recIdents = _out306;
                    r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::dafny_runtime::dafny_rational_to_int("), (_1980_recursiveGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")")));
                    RAST._IExpr _out307;
                    DCOMP._IOwnership _out308;
                    (this).FromOwned(r, expectedOwnership, out _out307, out _out308);
                    r = _out307;
                    resultingOwnership = _out308;
                    readIdents = _1982_recIdents;
                  }
                  goto after_match39;
                }
              }
            }
          }
        }
        {
          DAST._IType _03 = _source108.dtor__0;
          if (_03.is_Primitive) {
            DAST._IPrimitive _h76 = _03.dtor_Primitive_a0;
            if (_h76.is_Int) {
              DAST._IType _13 = _source108.dtor__1;
              if (_13.is_Passthrough) {
                {
                  RAST._IType _1983_rhsType;
                  RAST._IType _out309;
                  _out309 = (this).GenType(_1965_toTpe, DCOMP.GenTypeContext.@default());
                  _1983_rhsType = _out309;
                  RAST._IExpr _1984_recursiveGen;
                  DCOMP._IOwnership _1985___v141;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1986_recIdents;
                  RAST._IExpr _out310;
                  DCOMP._IOwnership _out311;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out312;
                  (this).GenExpr(_1963_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out310, out _out311, out _out312);
                  _1984_recursiveGen = _out310;
                  _1985___v141 = _out311;
                  _1986_recIdents = _out312;
                  r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), (_1983_rhsType)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" as ::dafny_runtime::NumCast>::from(")), (_1984_recursiveGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(").unwrap()")));
                  RAST._IExpr _out313;
                  DCOMP._IOwnership _out314;
                  (this).FromOwned(r, expectedOwnership, out _out313, out _out314);
                  r = _out313;
                  resultingOwnership = _out314;
                  readIdents = _1986_recIdents;
                }
                goto after_match39;
              }
            }
          }
        }
        {
          DAST._IType _04 = _source108.dtor__0;
          if (_04.is_Passthrough) {
            DAST._IType _14 = _source108.dtor__1;
            if (_14.is_Primitive) {
              DAST._IPrimitive _h77 = _14.dtor_Primitive_a0;
              if (_h77.is_Int) {
                {
                  RAST._IType _1987_rhsType;
                  RAST._IType _out315;
                  _out315 = (this).GenType(_1964_fromTpe, DCOMP.GenTypeContext.@default());
                  _1987_rhsType = _out315;
                  RAST._IExpr _1988_recursiveGen;
                  DCOMP._IOwnership _1989___v143;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1990_recIdents;
                  RAST._IExpr _out316;
                  DCOMP._IOwnership _out317;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out318;
                  (this).GenExpr(_1963_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out316, out _out317, out _out318);
                  _1988_recursiveGen = _out316;
                  _1989___v143 = _out317;
                  _1990_recIdents = _out318;
                  r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::dafny_runtime::DafnyInt::new(::std::rc::Rc::new(::dafny_runtime::BigInt::from("), (_1988_recursiveGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")))")));
                  RAST._IExpr _out319;
                  DCOMP._IOwnership _out320;
                  (this).FromOwned(r, expectedOwnership, out _out319, out _out320);
                  r = _out319;
                  resultingOwnership = _out320;
                  readIdents = _1990_recIdents;
                }
                goto after_match39;
              }
            }
          }
        }
        {
          DAST._IType _05 = _source108.dtor__0;
          if (_05.is_Primitive) {
            DAST._IPrimitive _h78 = _05.dtor_Primitive_a0;
            if (_h78.is_Int) {
              DAST._IType _15 = _source108.dtor__1;
              if (_15.is_Primitive) {
                DAST._IPrimitive _h79 = _15.dtor_Primitive_a0;
                if (_h79.is_Char) {
                  {
                    RAST._IType _1991_rhsType;
                    RAST._IType _out321;
                    _out321 = (this).GenType(_1965_toTpe, DCOMP.GenTypeContext.@default());
                    _1991_rhsType = _out321;
                    RAST._IExpr _1992_recursiveGen;
                    DCOMP._IOwnership _1993___v144;
                    Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1994_recIdents;
                    RAST._IExpr _out322;
                    DCOMP._IOwnership _out323;
                    Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out324;
                    (this).GenExpr(_1963_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out322, out _out323, out _out324);
                    _1992_recursiveGen = _out322;
                    _1993___v144 = _out323;
                    _1994_recIdents = _out324;
                    r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::dafny_runtime::"), (this).DafnyChar), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")), (((this).UnicodeChars) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("char::from_u32(<u32")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<u16")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" as ::dafny_runtime::NumCast>::from(")), (_1992_recursiveGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(").unwrap())")), (((this).UnicodeChars) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".unwrap())")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))));
                    RAST._IExpr _out325;
                    DCOMP._IOwnership _out326;
                    (this).FromOwned(r, expectedOwnership, out _out325, out _out326);
                    r = _out325;
                    resultingOwnership = _out326;
                    readIdents = _1994_recIdents;
                  }
                  goto after_match39;
                }
              }
            }
          }
        }
        {
          DAST._IType _06 = _source108.dtor__0;
          if (_06.is_Primitive) {
            DAST._IPrimitive _h710 = _06.dtor_Primitive_a0;
            if (_h710.is_Char) {
              DAST._IType _16 = _source108.dtor__1;
              if (_16.is_Primitive) {
                DAST._IPrimitive _h711 = _16.dtor_Primitive_a0;
                if (_h711.is_Int) {
                  {
                    RAST._IType _1995_rhsType;
                    RAST._IType _out327;
                    _out327 = (this).GenType(_1964_fromTpe, DCOMP.GenTypeContext.@default());
                    _1995_rhsType = _out327;
                    RAST._IExpr _1996_recursiveGen;
                    DCOMP._IOwnership _1997___v145;
                    Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1998_recIdents;
                    RAST._IExpr _out328;
                    DCOMP._IOwnership _out329;
                    Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out330;
                    (this).GenExpr(_1963_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out328, out _out329, out _out330);
                    _1996_recursiveGen = _out328;
                    _1997___v145 = _out329;
                    _1998_recIdents = _out330;
                    r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1((_1996_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")));
                    RAST._IExpr _out331;
                    DCOMP._IOwnership _out332;
                    (this).FromOwned(r, expectedOwnership, out _out331, out _out332);
                    r = _out331;
                    resultingOwnership = _out332;
                    readIdents = _1998_recIdents;
                  }
                  goto after_match39;
                }
              }
            }
          }
        }
        {
          DAST._IType _07 = _source108.dtor__0;
          if (_07.is_Passthrough) {
            DAST._IType _17 = _source108.dtor__1;
            if (_17.is_Passthrough) {
              {
                RAST._IExpr _1999_recursiveGen;
                DCOMP._IOwnership _2000___v148;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2001_recIdents;
                RAST._IExpr _out333;
                DCOMP._IOwnership _out334;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out335;
                (this).GenExpr(_1963_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out333, out _out334, out _out335);
                _1999_recursiveGen = _out333;
                _2000___v148 = _out334;
                _2001_recIdents = _out335;
                RAST._IType _2002_toTpeGen;
                RAST._IType _out336;
                _out336 = (this).GenType(_1965_toTpe, DCOMP.GenTypeContext.@default());
                _2002_toTpeGen = _out336;
                r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(("), (_1999_recursiveGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(") as ")), (_2002_toTpeGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")")));
                RAST._IExpr _out337;
                DCOMP._IOwnership _out338;
                (this).FromOwned(r, expectedOwnership, out _out337, out _out338);
                r = _out337;
                resultingOwnership = _out338;
                readIdents = _2001_recIdents;
              }
              goto after_match39;
            }
          }
        }
        {
          {
            RAST._IExpr _out339;
            DCOMP._IOwnership _out340;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out341;
            (this).GenExprConvertOther(e, selfIdent, env, expectedOwnership, out _out339, out _out340, out _out341);
            r = _out339;
            resultingOwnership = _out340;
            readIdents = _out341;
          }
        }
      after_match39: ;
      }
      return ;
    }
    public void GenIdent(Dafny.ISequence<Dafny.Rune> rName, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, DCOMP._IOwnership expectedOwnership, out RAST._IExpr r, out DCOMP._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = DCOMP.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      r = RAST.Expr.create_Identifier(rName);
      Std.Wrappers._IOption<RAST._IType> _2003_tpe;
      _2003_tpe = (env).GetType(rName);
      Std.Wrappers._IOption<RAST._IType> _2004_placeboOpt;
      if ((_2003_tpe).is_Some) {
        _2004_placeboOpt = ((_2003_tpe).dtor_value).ExtractMaybePlacebo();
      } else {
        _2004_placeboOpt = Std.Wrappers.Option<RAST._IType>.create_None();
      }
      bool _2005_currentlyBorrowed;
      _2005_currentlyBorrowed = (env).IsBorrowed(rName);
      bool _2006_noNeedOfClone;
      _2006_noNeedOfClone = (env).CanReadWithoutClone(rName);
      if ((_2004_placeboOpt).is_Some) {
        r = ((r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("read"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
        _2005_currentlyBorrowed = false;
        _2006_noNeedOfClone = true;
        _2003_tpe = Std.Wrappers.Option<RAST._IType>.create_Some((_2004_placeboOpt).dtor_value);
      }
      if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipAutoBorrowed())) {
        if (_2005_currentlyBorrowed) {
          resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowed();
        } else {
          resultingOwnership = DCOMP.Ownership.create_OwnershipOwned();
        }
      } else if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipBorrowedMut())) {
        if ((rName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))) {
          resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowedMut();
        } else {
          if (((_2003_tpe).is_Some) && (((_2003_tpe).dtor_value).IsObjectOrPointer())) {
            r = ((this).modify__macro).Apply1(r);
          } else {
            r = RAST.__default.BorrowMut(r);
          }
        }
        resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowedMut();
      } else if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipOwned())) {
        bool _2007_needObjectFromRef;
        _2007_needObjectFromRef = ((((selfIdent).is_ThisTyped) && ((selfIdent).IsSelf())) && (((selfIdent).dtor_rSelfName).Equals(rName))) && (((System.Func<bool>)(() => {
          DAST._IType _source109 = (selfIdent).dtor_dafnyType;
          {
            if (_source109.is_UserDefined) {
              DAST._IResolvedType resolved4 = _source109.dtor_resolved;
              DAST._IResolvedTypeBase _2008_base = resolved4.dtor_kind;
              Dafny.ISequence<DAST._IAttribute> _2009_attributes = resolved4.dtor_attributes;
              return ((_2008_base).is_Class) || ((_2008_base).is_Trait);
            }
          }
          {
            return false;
          }
        }))());
        if (_2007_needObjectFromRef) {
          r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Object"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"))))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_ref"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(r));
        } else {
          if (!(_2006_noNeedOfClone)) {
            r = (r).Clone();
          }
        }
        resultingOwnership = DCOMP.Ownership.create_OwnershipOwned();
      } else if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipOwnedBox())) {
        if (!(_2006_noNeedOfClone)) {
          r = (r).Clone();
        }
        r = RAST.__default.BoxNew(r);
        resultingOwnership = DCOMP.Ownership.create_OwnershipOwnedBox();
      } else if (_2005_currentlyBorrowed) {
        resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowed();
      } else {
        if (!(rName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))) {
          if (((_2003_tpe).is_Some) && (((_2003_tpe).dtor_value).IsPointer())) {
            r = ((this).read__macro).Apply1(r);
          } else {
            r = RAST.__default.Borrow(r);
          }
        }
        resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowed();
      }
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(rName);
      return ;
    }
    public bool HasExternAttributeRenamingModule(Dafny.ISequence<DAST._IAttribute> attributes) {
      return Dafny.Helpers.Id<Func<Dafny.ISequence<DAST._IAttribute>, bool>>((_2010_attributes) => Dafny.Helpers.Quantifier<DAST._IAttribute>((_2010_attributes).UniqueElements, false, (((_exists_var_2) => {
        DAST._IAttribute _2011_attribute = (DAST._IAttribute)_exists_var_2;
        return ((_2010_attributes).Contains(_2011_attribute)) && ((((_2011_attribute).dtor_name).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("extern"))) && ((new BigInteger(((_2011_attribute).dtor_args).Count)) == (new BigInteger(2))));
      }))))(attributes);
    }
    public void GenArgs(DCOMP._ISelfInfo selfIdent, DAST._ICallName name, Dafny.ISequence<DAST._IType> typeArgs, Dafny.ISequence<DAST._IExpression> args, DCOMP._IEnvironment env, out Dafny.ISequence<RAST._IExpr> argExprs, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents, out Dafny.ISequence<RAST._IType> typeExprs, out Std.Wrappers._IOption<DAST._IResolvedType> fullNameQualifier)
    {
      argExprs = Dafny.Sequence<RAST._IExpr>.Empty;
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      typeExprs = Dafny.Sequence<RAST._IType>.Empty;
      fullNameQualifier = Std.Wrappers.Option<DAST._IResolvedType>.Default();
      argExprs = Dafny.Sequence<RAST._IExpr>.FromElements();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      Dafny.ISequence<DAST._IFormal> _2012_signature;
      if ((name).is_CallName) {
        if ((((name).dtor_receiverArg).is_Some) && ((name).dtor_receiverAsArgument)) {
          _2012_signature = Dafny.Sequence<DAST._IFormal>.Concat(Dafny.Sequence<DAST._IFormal>.FromElements(((name).dtor_receiverArg).dtor_value), ((name).dtor_signature));
        } else {
          _2012_signature = ((name).dtor_signature);
        }
      } else {
        _2012_signature = Dafny.Sequence<DAST._IFormal>.FromElements();
      }
      BigInteger _hi38 = new BigInteger((args).Count);
      for (BigInteger _2013_i = BigInteger.Zero; _2013_i < _hi38; _2013_i++) {
        DCOMP._IOwnership _2014_argOwnership;
        _2014_argOwnership = DCOMP.Ownership.create_OwnershipBorrowed();
        if ((_2013_i) < (new BigInteger((_2012_signature).Count))) {
          RAST._IType _2015_tpe;
          RAST._IType _out342;
          _out342 = (this).GenType(((_2012_signature).Select(_2013_i)).dtor_typ, DCOMP.GenTypeContext.@default());
          _2015_tpe = _out342;
          if ((_2015_tpe).CanReadWithoutClone()) {
            _2014_argOwnership = DCOMP.Ownership.create_OwnershipOwned();
          }
        }
        RAST._IExpr _2016_argExpr;
        DCOMP._IOwnership _2017___v155;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2018_argIdents;
        RAST._IExpr _out343;
        DCOMP._IOwnership _out344;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out345;
        (this).GenExpr((args).Select(_2013_i), selfIdent, env, _2014_argOwnership, out _out343, out _out344, out _out345);
        _2016_argExpr = _out343;
        _2017___v155 = _out344;
        _2018_argIdents = _out345;
        argExprs = Dafny.Sequence<RAST._IExpr>.Concat(argExprs, Dafny.Sequence<RAST._IExpr>.FromElements(_2016_argExpr));
        readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2018_argIdents);
      }
      typeExprs = Dafny.Sequence<RAST._IType>.FromElements();
      BigInteger _hi39 = new BigInteger((typeArgs).Count);
      for (BigInteger _2019_typeI = BigInteger.Zero; _2019_typeI < _hi39; _2019_typeI++) {
        RAST._IType _2020_typeExpr;
        RAST._IType _out346;
        _out346 = (this).GenType((typeArgs).Select(_2019_typeI), DCOMP.GenTypeContext.@default());
        _2020_typeExpr = _out346;
        typeExprs = Dafny.Sequence<RAST._IType>.Concat(typeExprs, Dafny.Sequence<RAST._IType>.FromElements(_2020_typeExpr));
      }
      DAST._ICallName _source110 = name;
      {
        if (_source110.is_CallName) {
          Dafny.ISequence<Dafny.Rune> _2021_nameIdent = _source110.dtor_name;
          Std.Wrappers._IOption<DAST._IType> onType1 = _source110.dtor_onType;
          if (onType1.is_Some) {
            DAST._IType value10 = onType1.dtor_value;
            if (value10.is_UserDefined) {
              DAST._IResolvedType _2022_resolvedType = value10.dtor_resolved;
              if ((((_2022_resolvedType).dtor_kind).is_Trait) || (Dafny.Helpers.Id<Func<DAST._IResolvedType, Dafny.ISequence<Dafny.Rune>, bool>>((_2023_resolvedType, _2024_nameIdent) => Dafny.Helpers.Quantifier<Dafny.ISequence<Dafny.Rune>>(Dafny.Helpers.SingleValue<Dafny.ISequence<Dafny.Rune>>(_2024_nameIdent), true, (((_forall_var_8) => {
                Dafny.ISequence<Dafny.Rune> _2025_m = (Dafny.ISequence<Dafny.Rune>)_forall_var_8;
                return !(((_2023_resolvedType).dtor_properMethods).Contains(_2025_m)) || (!object.Equals(_2025_m, _2024_nameIdent));
              }))))(_2022_resolvedType, _2021_nameIdent))) {
                fullNameQualifier = Std.Wrappers.Option<DAST._IResolvedType>.create_Some(Std.Wrappers.Option<DAST._IResolvedType>.GetOr(DCOMP.__default.TraitTypeContainingMethod(_2022_resolvedType, (_2021_nameIdent)), _2022_resolvedType));
              } else {
                fullNameQualifier = Std.Wrappers.Option<DAST._IResolvedType>.create_None();
              }
              goto after_match40;
            }
          }
        }
      }
      {
        fullNameQualifier = Std.Wrappers.Option<DAST._IResolvedType>.create_None();
      }
    after_match40: ;
      if ((((((fullNameQualifier).is_Some) && ((selfIdent).is_ThisTyped)) && (((selfIdent).dtor_dafnyType).is_UserDefined)) && ((this).IsSameResolvedType(((selfIdent).dtor_dafnyType).dtor_resolved, (fullNameQualifier).dtor_value))) && (!((this).HasExternAttributeRenamingModule(((fullNameQualifier).dtor_value).dtor_attributes)))) {
        fullNameQualifier = Std.Wrappers.Option<DAST._IResolvedType>.create_None();
      }
    }
    public Dafny.ISequence<Dafny.Rune> GetMethodName(DAST._IExpression @on, DAST._ICallName name)
    {
      DAST._ICallName _source111 = name;
      {
        if (_source111.is_CallName) {
          Dafny.ISequence<Dafny.Rune> _2026_ident = _source111.dtor_name;
          if ((@on).is_ExternCompanion) {
            return (_2026_ident);
          } else {
            return DCOMP.__default.escapeName(_2026_ident);
          }
        }
      }
      {
        bool disjunctiveMatch14 = false;
        if (_source111.is_MapBuilderAdd) {
          disjunctiveMatch14 = true;
        }
        if (_source111.is_SetBuilderAdd) {
          disjunctiveMatch14 = true;
        }
        if (disjunctiveMatch14) {
          return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("add");
        }
      }
      {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("build");
      }
    }
    public void GenExpr(DAST._IExpression e, DCOMP._ISelfInfo selfIdent, DCOMP._IEnvironment env, DCOMP._IOwnership expectedOwnership, out RAST._IExpr r, out DCOMP._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = DCOMP.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _source112 = e;
      {
        if (_source112.is_Literal) {
          RAST._IExpr _out347;
          DCOMP._IOwnership _out348;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out349;
          (this).GenExprLiteral(e, selfIdent, env, expectedOwnership, out _out347, out _out348, out _out349);
          r = _out347;
          resultingOwnership = _out348;
          readIdents = _out349;
          goto after_match41;
        }
      }
      {
        if (_source112.is_Ident) {
          Dafny.ISequence<Dafny.Rune> _2027_name = _source112.dtor_name;
          {
            RAST._IExpr _out350;
            DCOMP._IOwnership _out351;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out352;
            (this).GenIdent(DCOMP.__default.escapeVar(_2027_name), selfIdent, env, expectedOwnership, out _out350, out _out351, out _out352);
            r = _out350;
            resultingOwnership = _out351;
            readIdents = _out352;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_ExternCompanion) {
          Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2028_path = _source112.dtor_ExternCompanion_a0;
          {
            RAST._IExpr _out353;
            _out353 = DCOMP.COMP.GenPathExpr(_2028_path, false);
            r = _out353;
            if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipBorrowed())) {
              resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowed();
            } else if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipOwned())) {
              resultingOwnership = DCOMP.Ownership.create_OwnershipOwned();
            } else {
              RAST._IExpr _out354;
              DCOMP._IOwnership _out355;
              (this).FromOwned(r, expectedOwnership, out _out354, out _out355);
              r = _out354;
              resultingOwnership = _out355;
            }
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_Companion) {
          Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2029_path = _source112.dtor_Companion_a0;
          Dafny.ISequence<DAST._IType> _2030_typeArgs = _source112.dtor_typeArgs;
          {
            RAST._IExpr _out356;
            _out356 = DCOMP.COMP.GenPathExpr(_2029_path, true);
            r = _out356;
            if ((new BigInteger((_2030_typeArgs).Count)).Sign == 1) {
              Dafny.ISequence<RAST._IType> _2031_typeExprs;
              _2031_typeExprs = Dafny.Sequence<RAST._IType>.FromElements();
              BigInteger _hi40 = new BigInteger((_2030_typeArgs).Count);
              for (BigInteger _2032_i = BigInteger.Zero; _2032_i < _hi40; _2032_i++) {
                RAST._IType _2033_typeExpr;
                RAST._IType _out357;
                _out357 = (this).GenType((_2030_typeArgs).Select(_2032_i), DCOMP.GenTypeContext.@default());
                _2033_typeExpr = _out357;
                _2031_typeExprs = Dafny.Sequence<RAST._IType>.Concat(_2031_typeExprs, Dafny.Sequence<RAST._IType>.FromElements(_2033_typeExpr));
              }
              r = (r).ApplyType(_2031_typeExprs);
            }
            if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipBorrowed())) {
              resultingOwnership = DCOMP.Ownership.create_OwnershipBorrowed();
            } else if (object.Equals(expectedOwnership, DCOMP.Ownership.create_OwnershipOwned())) {
              resultingOwnership = DCOMP.Ownership.create_OwnershipOwned();
            } else {
              RAST._IExpr _out358;
              DCOMP._IOwnership _out359;
              (this).FromOwned(r, expectedOwnership, out _out358, out _out359);
              r = _out358;
              resultingOwnership = _out359;
            }
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_InitializationValue) {
          DAST._IType _2034_typ = _source112.dtor_typ;
          {
            RAST._IType _2035_typExpr;
            RAST._IType _out360;
            _out360 = (this).GenType(_2034_typ, DCOMP.GenTypeContext.@default());
            _2035_typExpr = _out360;
            if ((_2035_typExpr).IsObjectOrPointer()) {
              r = (_2035_typExpr).ToNullExpr();
            } else {
              r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), (_2035_typExpr)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" as std::default::Default>::default()")));
            }
            RAST._IExpr _out361;
            DCOMP._IOwnership _out362;
            (this).FromOwned(r, expectedOwnership, out _out361, out _out362);
            r = _out361;
            resultingOwnership = _out362;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_Tuple) {
          Dafny.ISequence<DAST._IExpression> _2036_values = _source112.dtor_Tuple_a0;
          {
            Dafny.ISequence<RAST._IExpr> _2037_exprs;
            _2037_exprs = Dafny.Sequence<RAST._IExpr>.FromElements();
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            BigInteger _hi41 = new BigInteger((_2036_values).Count);
            for (BigInteger _2038_i = BigInteger.Zero; _2038_i < _hi41; _2038_i++) {
              RAST._IExpr _2039_recursiveGen;
              DCOMP._IOwnership _2040___v165;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2041_recIdents;
              RAST._IExpr _out363;
              DCOMP._IOwnership _out364;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out365;
              (this).GenExpr((_2036_values).Select(_2038_i), selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out363, out _out364, out _out365);
              _2039_recursiveGen = _out363;
              _2040___v165 = _out364;
              _2041_recIdents = _out365;
              _2037_exprs = Dafny.Sequence<RAST._IExpr>.Concat(_2037_exprs, Dafny.Sequence<RAST._IExpr>.FromElements(_2039_recursiveGen));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2041_recIdents);
            }
            if ((new BigInteger((_2036_values).Count)) <= (RAST.__default.MAX__TUPLE__SIZE)) {
              r = RAST.Expr.create_Tuple(_2037_exprs);
            } else {
              r = RAST.__default.SystemTuple(_2037_exprs);
            }
            RAST._IExpr _out366;
            DCOMP._IOwnership _out367;
            (this).FromOwned(r, expectedOwnership, out _out366, out _out367);
            r = _out366;
            resultingOwnership = _out367;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_New) {
          Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2042_path = _source112.dtor_path;
          Dafny.ISequence<DAST._IType> _2043_typeArgs = _source112.dtor_typeArgs;
          Dafny.ISequence<DAST._IExpression> _2044_args = _source112.dtor_args;
          {
            RAST._IExpr _out368;
            _out368 = DCOMP.COMP.GenPathExpr(_2042_path, true);
            r = _out368;
            if ((new BigInteger((_2043_typeArgs).Count)).Sign == 1) {
              Dafny.ISequence<RAST._IType> _2045_typeExprs;
              _2045_typeExprs = Dafny.Sequence<RAST._IType>.FromElements();
              BigInteger _hi42 = new BigInteger((_2043_typeArgs).Count);
              for (BigInteger _2046_i = BigInteger.Zero; _2046_i < _hi42; _2046_i++) {
                RAST._IType _2047_typeExpr;
                RAST._IType _out369;
                _out369 = (this).GenType((_2043_typeArgs).Select(_2046_i), DCOMP.GenTypeContext.@default());
                _2047_typeExpr = _out369;
                _2045_typeExprs = Dafny.Sequence<RAST._IType>.Concat(_2045_typeExprs, Dafny.Sequence<RAST._IType>.FromElements(_2047_typeExpr));
              }
              r = (r).ApplyType(_2045_typeExprs);
            }
            r = (r).FSel((this).allocate__fn);
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            Dafny.ISequence<RAST._IExpr> _2048_arguments;
            _2048_arguments = Dafny.Sequence<RAST._IExpr>.FromElements();
            BigInteger _hi43 = new BigInteger((_2044_args).Count);
            for (BigInteger _2049_i = BigInteger.Zero; _2049_i < _hi43; _2049_i++) {
              RAST._IExpr _2050_recursiveGen;
              DCOMP._IOwnership _2051___v166;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2052_recIdents;
              RAST._IExpr _out370;
              DCOMP._IOwnership _out371;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out372;
              (this).GenExpr((_2044_args).Select(_2049_i), selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out370, out _out371, out _out372);
              _2050_recursiveGen = _out370;
              _2051___v166 = _out371;
              _2052_recIdents = _out372;
              _2048_arguments = Dafny.Sequence<RAST._IExpr>.Concat(_2048_arguments, Dafny.Sequence<RAST._IExpr>.FromElements(_2050_recursiveGen));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2052_recIdents);
            }
            r = (r).Apply(_2048_arguments);
            RAST._IExpr _out373;
            DCOMP._IOwnership _out374;
            (this).FromOwned(r, expectedOwnership, out _out373, out _out374);
            r = _out373;
            resultingOwnership = _out374;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_NewUninitArray) {
          Dafny.ISequence<DAST._IExpression> _2053_dims = _source112.dtor_dims;
          DAST._IType _2054_typ = _source112.dtor_typ;
          {
            if ((new BigInteger(16)) < (new BigInteger((_2053_dims).Count))) {
              Dafny.ISequence<Dafny.Rune> _2055_msg;
              _2055_msg = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Unsupported: Creation of arrays of more than 16 dimensions");
              if ((this.error).is_None) {
                (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(_2055_msg);
              }
              r = RAST.Expr.create_RawExpr(_2055_msg);
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            } else {
              r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
              RAST._IType _2056_typeGen;
              RAST._IType _out375;
              _out375 = (this).GenType(_2054_typ, DCOMP.GenTypeContext.@default());
              _2056_typeGen = _out375;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              Dafny.ISequence<RAST._IExpr> _2057_dimExprs;
              _2057_dimExprs = Dafny.Sequence<RAST._IExpr>.FromElements();
              BigInteger _hi44 = new BigInteger((_2053_dims).Count);
              for (BigInteger _2058_i = BigInteger.Zero; _2058_i < _hi44; _2058_i++) {
                RAST._IExpr _2059_recursiveGen;
                DCOMP._IOwnership _2060___v167;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2061_recIdents;
                RAST._IExpr _out376;
                DCOMP._IOwnership _out377;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out378;
                (this).GenExpr((_2053_dims).Select(_2058_i), selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out376, out _out377, out _out378);
                _2059_recursiveGen = _out376;
                _2060___v167 = _out377;
                _2061_recIdents = _out378;
                _2057_dimExprs = Dafny.Sequence<RAST._IExpr>.Concat(_2057_dimExprs, Dafny.Sequence<RAST._IExpr>.FromElements(RAST.__default.IntoUsize(_2059_recursiveGen)));
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2061_recIdents);
              }
              if ((new BigInteger((_2053_dims).Count)) > (BigInteger.One)) {
                Dafny.ISequence<Dafny.Rune> _2062_class__name;
                _2062_class__name = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Array"), Std.Strings.__default.OfNat(new BigInteger((_2053_dims).Count)));
                r = (((((RAST.__default.dafny__runtime).MSel(_2062_class__name)).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_2056_typeGen))).FSel((this).placebos__usize)).Apply(_2057_dimExprs);
              } else {
                r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("array"))).AsExpr()).FSel((this).placebos__usize)).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_2056_typeGen))).Apply(_2057_dimExprs);
              }
            }
            RAST._IExpr _out379;
            DCOMP._IOwnership _out380;
            (this).FromOwned(r, expectedOwnership, out _out379, out _out380);
            r = _out379;
            resultingOwnership = _out380;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_ArrayIndexToInt) {
          DAST._IExpression _2063_underlying = _source112.dtor_value;
          {
            RAST._IExpr _2064_recursiveGen;
            DCOMP._IOwnership _2065___v168;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2066_recIdents;
            RAST._IExpr _out381;
            DCOMP._IOwnership _out382;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out383;
            (this).GenExpr(_2063_underlying, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out381, out _out382, out _out383);
            _2064_recursiveGen = _out381;
            _2065___v168 = _out382;
            _2066_recIdents = _out383;
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1(_2064_recursiveGen);
            readIdents = _2066_recIdents;
            RAST._IExpr _out384;
            DCOMP._IOwnership _out385;
            (this).FromOwned(r, expectedOwnership, out _out384, out _out385);
            r = _out384;
            resultingOwnership = _out385;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_FinalizeNewArray) {
          DAST._IExpression _2067_underlying = _source112.dtor_value;
          DAST._IType _2068_typ = _source112.dtor_typ;
          {
            RAST._IType _2069_tpe;
            RAST._IType _out386;
            _out386 = (this).GenType(_2068_typ, DCOMP.GenTypeContext.@default());
            _2069_tpe = _out386;
            RAST._IExpr _2070_recursiveGen;
            DCOMP._IOwnership _2071___v169;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2072_recIdents;
            RAST._IExpr _out387;
            DCOMP._IOwnership _out388;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out389;
            (this).GenExpr(_2067_underlying, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out387, out _out388, out _out389);
            _2070_recursiveGen = _out387;
            _2071___v169 = _out388;
            _2072_recIdents = _out389;
            readIdents = _2072_recIdents;
            if ((_2069_tpe).IsObjectOrPointer()) {
              RAST._IType _2073_t;
              _2073_t = (_2069_tpe).ObjectOrPointerUnderlying();
              if ((_2073_t).is_Array) {
                r = ((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("array"))).AsExpr()).FSel((this).array__construct)).Apply1(_2070_recursiveGen);
              } else if ((_2073_t).IsMultiArray()) {
                Dafny.ISequence<Dafny.Rune> _2074_c;
                _2074_c = (_2073_t).MultiArrayClass();
                r = ((((RAST.__default.dafny__runtime).MSel(_2074_c)).AsExpr()).FSel((this).array__construct)).Apply1(_2070_recursiveGen);
              } else {
                (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Finalize New Array with a pointer or object type to something that is not an array or a multi array: "), (_2069_tpe)._ToString(DCOMP.__default.IND)));
                r = RAST.Expr.create_RawExpr((this.error).dtor_value);
              }
            } else {
              (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Finalize New Array with a type that is not a pointer or an object: "), (_2069_tpe)._ToString(DCOMP.__default.IND)));
              r = RAST.Expr.create_RawExpr((this.error).dtor_value);
            }
            RAST._IExpr _out390;
            DCOMP._IOwnership _out391;
            (this).FromOwned(r, expectedOwnership, out _out390, out _out391);
            r = _out390;
            resultingOwnership = _out391;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_DatatypeValue) {
          DAST._IResolvedType _2075_datatypeType = _source112.dtor_datatypeType;
          Dafny.ISequence<DAST._IType> _2076_typeArgs = _source112.dtor_typeArgs;
          Dafny.ISequence<Dafny.Rune> _2077_variant = _source112.dtor_variant;
          bool _2078_isCo = _source112.dtor_isCo;
          Dafny.ISequence<_System._ITuple2<Dafny.ISequence<Dafny.Rune>, DAST._IExpression>> _2079_values = _source112.dtor_contents;
          {
            RAST._IExpr _out392;
            _out392 = DCOMP.COMP.GenPathExpr((_2075_datatypeType).dtor_path, true);
            r = _out392;
            Dafny.ISequence<RAST._IType> _2080_genTypeArgs;
            _2080_genTypeArgs = Dafny.Sequence<RAST._IType>.FromElements();
            BigInteger _hi45 = new BigInteger((_2076_typeArgs).Count);
            for (BigInteger _2081_i = BigInteger.Zero; _2081_i < _hi45; _2081_i++) {
              RAST._IType _2082_typeExpr;
              RAST._IType _out393;
              _out393 = (this).GenType((_2076_typeArgs).Select(_2081_i), DCOMP.GenTypeContext.@default());
              _2082_typeExpr = _out393;
              _2080_genTypeArgs = Dafny.Sequence<RAST._IType>.Concat(_2080_genTypeArgs, Dafny.Sequence<RAST._IType>.FromElements(_2082_typeExpr));
            }
            if ((new BigInteger((_2076_typeArgs).Count)).Sign == 1) {
              r = (r).ApplyType(_2080_genTypeArgs);
            }
            r = (r).FSel(DCOMP.__default.escapeName(_2077_variant));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            Dafny.ISequence<RAST._IAssignIdentifier> _2083_assignments;
            _2083_assignments = Dafny.Sequence<RAST._IAssignIdentifier>.FromElements();
            BigInteger _hi46 = new BigInteger((_2079_values).Count);
            for (BigInteger _2084_i = BigInteger.Zero; _2084_i < _hi46; _2084_i++) {
              _System._ITuple2<Dafny.ISequence<Dafny.Rune>, DAST._IExpression> _let_tmp_rhs69 = (_2079_values).Select(_2084_i);
              Dafny.ISequence<Dafny.Rune> _2085_name = _let_tmp_rhs69.dtor__0;
              DAST._IExpression _2086_value = _let_tmp_rhs69.dtor__1;
              if (_2078_isCo) {
                RAST._IExpr _2087_recursiveGen;
                DCOMP._IOwnership _2088___v170;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2089_recIdents;
                RAST._IExpr _out394;
                DCOMP._IOwnership _out395;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out396;
                (this).GenExpr(_2086_value, selfIdent, DCOMP.Environment.Empty(), DCOMP.Ownership.create_OwnershipOwned(), out _out394, out _out395, out _out396);
                _2087_recursiveGen = _out394;
                _2088___v170 = _out395;
                _2089_recIdents = _out396;
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2089_recIdents);
                Dafny.ISequence<Dafny.Rune> _2090_allReadCloned;
                _2090_allReadCloned = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
                while (!(_2089_recIdents).Equals(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements())) {
                  Dafny.ISequence<Dafny.Rune> _2091_next;
                  foreach (Dafny.ISequence<Dafny.Rune> _assign_such_that_3 in (_2089_recIdents).Elements) {
                    _2091_next = (Dafny.ISequence<Dafny.Rune>)_assign_such_that_3;
                    if ((_2089_recIdents).Contains(_2091_next)) {
                      goto after__ASSIGN_SUCH_THAT_3;
                    }
                  }
                  throw new System.Exception("assign-such-that search produced no value (line 4740)");
                after__ASSIGN_SUCH_THAT_3: ;
                  _2090_allReadCloned = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2090_allReadCloned, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("let ")), _2091_next), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" = ")), _2091_next), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".clone();\n"));
                  _2089_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_2089_recIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_2091_next));
                }
                Dafny.ISequence<Dafny.Rune> _2092_wasAssigned;
                _2092_wasAssigned = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::dafny_runtime::LazyFieldWrapper(::dafny_runtime::Lazy::new(::std::boxed::Box::new({\n"), _2090_allReadCloned), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("move || (")), (_2087_recursiveGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")})))"));
                _2083_assignments = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_2083_assignments, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(DCOMP.__default.escapeVar(_2085_name), RAST.Expr.create_RawExpr(_2092_wasAssigned))));
              } else {
                RAST._IExpr _2093_recursiveGen;
                DCOMP._IOwnership _2094___v171;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2095_recIdents;
                RAST._IExpr _out397;
                DCOMP._IOwnership _out398;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out399;
                (this).GenExpr(_2086_value, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out397, out _out398, out _out399);
                _2093_recursiveGen = _out397;
                _2094___v171 = _out398;
                _2095_recIdents = _out399;
                _2083_assignments = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_2083_assignments, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(DCOMP.__default.escapeVar(_2085_name), _2093_recursiveGen)));
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2095_recIdents);
              }
            }
            r = RAST.Expr.create_StructBuild(r, _2083_assignments);
            if ((this).IsRcWrapped((_2075_datatypeType).dtor_attributes)) {
              r = RAST.__default.RcNew(r);
            }
            RAST._IExpr _out400;
            DCOMP._IOwnership _out401;
            (this).FromOwned(r, expectedOwnership, out _out400, out _out401);
            r = _out400;
            resultingOwnership = _out401;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_Convert) {
          {
            RAST._IExpr _out402;
            DCOMP._IOwnership _out403;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out404;
            (this).GenExprConvert(e, selfIdent, env, expectedOwnership, out _out402, out _out403, out _out404);
            r = _out402;
            resultingOwnership = _out403;
            readIdents = _out404;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_SeqConstruct) {
          DAST._IExpression _2096_length = _source112.dtor_length;
          DAST._IExpression _2097_expr = _source112.dtor_elem;
          {
            RAST._IExpr _2098_recursiveGen;
            DCOMP._IOwnership _2099___v175;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2100_recIdents;
            RAST._IExpr _out405;
            DCOMP._IOwnership _out406;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out407;
            (this).GenExpr(_2097_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out405, out _out406, out _out407);
            _2098_recursiveGen = _out405;
            _2099___v175 = _out406;
            _2100_recIdents = _out407;
            RAST._IExpr _2101_lengthGen;
            DCOMP._IOwnership _2102___v176;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2103_lengthIdents;
            RAST._IExpr _out408;
            DCOMP._IOwnership _out409;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out410;
            (this).GenExpr(_2096_length, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out408, out _out409, out _out410);
            _2101_lengthGen = _out408;
            _2102___v176 = _out409;
            _2103_lengthIdents = _out410;
            r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{\nlet _initializer = "), (_2098_recursiveGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";\n::dafny_runtime::integer_range(::dafny_runtime::Zero::zero(), ")), (_2101_lengthGen)._ToString(DCOMP.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(").map(|i| _initializer(&i)).collect::<::dafny_runtime::Sequence<_>>()\n}")));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_2100_recIdents, _2103_lengthIdents);
            RAST._IExpr _out411;
            DCOMP._IOwnership _out412;
            (this).FromOwned(r, expectedOwnership, out _out411, out _out412);
            r = _out411;
            resultingOwnership = _out412;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_SeqValue) {
          Dafny.ISequence<DAST._IExpression> _2104_exprs = _source112.dtor_elements;
          DAST._IType _2105_typ = _source112.dtor_typ;
          {
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            RAST._IType _2106_genTpe;
            RAST._IType _out413;
            _out413 = (this).GenType(_2105_typ, DCOMP.GenTypeContext.@default());
            _2106_genTpe = _out413;
            BigInteger _2107_i;
            _2107_i = BigInteger.Zero;
            Dafny.ISequence<RAST._IExpr> _2108_args;
            _2108_args = Dafny.Sequence<RAST._IExpr>.FromElements();
            while ((_2107_i) < (new BigInteger((_2104_exprs).Count))) {
              RAST._IExpr _2109_recursiveGen;
              DCOMP._IOwnership _2110___v177;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2111_recIdents;
              RAST._IExpr _out414;
              DCOMP._IOwnership _out415;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out416;
              (this).GenExpr((_2104_exprs).Select(_2107_i), selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out414, out _out415, out _out416);
              _2109_recursiveGen = _out414;
              _2110___v177 = _out415;
              _2111_recIdents = _out416;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2111_recIdents);
              _2108_args = Dafny.Sequence<RAST._IExpr>.Concat(_2108_args, Dafny.Sequence<RAST._IExpr>.FromElements(_2109_recursiveGen));
              _2107_i = (_2107_i) + (BigInteger.One);
            }
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("seq!"))).AsExpr()).Apply(_2108_args);
            if ((new BigInteger((_2108_args).Count)).Sign == 0) {
              r = RAST.Expr.create_TypeAscription(r, (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Sequence"))).AsType()).Apply1(_2106_genTpe));
            }
            RAST._IExpr _out417;
            DCOMP._IOwnership _out418;
            (this).FromOwned(r, expectedOwnership, out _out417, out _out418);
            r = _out417;
            resultingOwnership = _out418;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_SetValue) {
          Dafny.ISequence<DAST._IExpression> _2112_exprs = _source112.dtor_elements;
          {
            Dafny.ISequence<RAST._IExpr> _2113_generatedValues;
            _2113_generatedValues = Dafny.Sequence<RAST._IExpr>.FromElements();
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            BigInteger _2114_i;
            _2114_i = BigInteger.Zero;
            while ((_2114_i) < (new BigInteger((_2112_exprs).Count))) {
              RAST._IExpr _2115_recursiveGen;
              DCOMP._IOwnership _2116___v178;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2117_recIdents;
              RAST._IExpr _out419;
              DCOMP._IOwnership _out420;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out421;
              (this).GenExpr((_2112_exprs).Select(_2114_i), selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out419, out _out420, out _out421);
              _2115_recursiveGen = _out419;
              _2116___v178 = _out420;
              _2117_recIdents = _out421;
              _2113_generatedValues = Dafny.Sequence<RAST._IExpr>.Concat(_2113_generatedValues, Dafny.Sequence<RAST._IExpr>.FromElements(_2115_recursiveGen));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2117_recIdents);
              _2114_i = (_2114_i) + (BigInteger.One);
            }
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("set!"))).AsExpr()).Apply(_2113_generatedValues);
            RAST._IExpr _out422;
            DCOMP._IOwnership _out423;
            (this).FromOwned(r, expectedOwnership, out _out422, out _out423);
            r = _out422;
            resultingOwnership = _out423;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_MultisetValue) {
          Dafny.ISequence<DAST._IExpression> _2118_exprs = _source112.dtor_elements;
          {
            Dafny.ISequence<RAST._IExpr> _2119_generatedValues;
            _2119_generatedValues = Dafny.Sequence<RAST._IExpr>.FromElements();
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            BigInteger _2120_i;
            _2120_i = BigInteger.Zero;
            while ((_2120_i) < (new BigInteger((_2118_exprs).Count))) {
              RAST._IExpr _2121_recursiveGen;
              DCOMP._IOwnership _2122___v179;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2123_recIdents;
              RAST._IExpr _out424;
              DCOMP._IOwnership _out425;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out426;
              (this).GenExpr((_2118_exprs).Select(_2120_i), selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out424, out _out425, out _out426);
              _2121_recursiveGen = _out424;
              _2122___v179 = _out425;
              _2123_recIdents = _out426;
              _2119_generatedValues = Dafny.Sequence<RAST._IExpr>.Concat(_2119_generatedValues, Dafny.Sequence<RAST._IExpr>.FromElements(_2121_recursiveGen));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2123_recIdents);
              _2120_i = (_2120_i) + (BigInteger.One);
            }
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("multiset!"))).AsExpr()).Apply(_2119_generatedValues);
            RAST._IExpr _out427;
            DCOMP._IOwnership _out428;
            (this).FromOwned(r, expectedOwnership, out _out427, out _out428);
            r = _out427;
            resultingOwnership = _out428;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_ToMultiset) {
          DAST._IExpression _2124_expr = _source112.dtor_ToMultiset_a0;
          {
            RAST._IExpr _2125_recursiveGen;
            DCOMP._IOwnership _2126___v180;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2127_recIdents;
            RAST._IExpr _out429;
            DCOMP._IOwnership _out430;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out431;
            (this).GenExpr(_2124_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out429, out _out430, out _out431);
            _2125_recursiveGen = _out429;
            _2126___v180 = _out430;
            _2127_recIdents = _out431;
            r = ((_2125_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_dafny_multiset"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            readIdents = _2127_recIdents;
            RAST._IExpr _out432;
            DCOMP._IOwnership _out433;
            (this).FromOwned(r, expectedOwnership, out _out432, out _out433);
            r = _out432;
            resultingOwnership = _out433;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_MapValue) {
          Dafny.ISequence<_System._ITuple2<DAST._IExpression, DAST._IExpression>> _2128_mapElems = _source112.dtor_mapElems;
          {
            Dafny.ISequence<_System._ITuple2<RAST._IExpr, RAST._IExpr>> _2129_generatedValues;
            _2129_generatedValues = Dafny.Sequence<_System._ITuple2<RAST._IExpr, RAST._IExpr>>.FromElements();
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            BigInteger _2130_i;
            _2130_i = BigInteger.Zero;
            while ((_2130_i) < (new BigInteger((_2128_mapElems).Count))) {
              RAST._IExpr _2131_recursiveGenKey;
              DCOMP._IOwnership _2132___v181;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2133_recIdentsKey;
              RAST._IExpr _out434;
              DCOMP._IOwnership _out435;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out436;
              (this).GenExpr(((_2128_mapElems).Select(_2130_i)).dtor__0, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out434, out _out435, out _out436);
              _2131_recursiveGenKey = _out434;
              _2132___v181 = _out435;
              _2133_recIdentsKey = _out436;
              RAST._IExpr _2134_recursiveGenValue;
              DCOMP._IOwnership _2135___v182;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2136_recIdentsValue;
              RAST._IExpr _out437;
              DCOMP._IOwnership _out438;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out439;
              (this).GenExpr(((_2128_mapElems).Select(_2130_i)).dtor__1, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out437, out _out438, out _out439);
              _2134_recursiveGenValue = _out437;
              _2135___v182 = _out438;
              _2136_recIdentsValue = _out439;
              _2129_generatedValues = Dafny.Sequence<_System._ITuple2<RAST._IExpr, RAST._IExpr>>.Concat(_2129_generatedValues, Dafny.Sequence<_System._ITuple2<RAST._IExpr, RAST._IExpr>>.FromElements(_System.Tuple2<RAST._IExpr, RAST._IExpr>.create(_2131_recursiveGenKey, _2134_recursiveGenValue)));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2133_recIdentsKey), _2136_recIdentsValue);
              _2130_i = (_2130_i) + (BigInteger.One);
            }
            _2130_i = BigInteger.Zero;
            Dafny.ISequence<RAST._IExpr> _2137_arguments;
            _2137_arguments = Dafny.Sequence<RAST._IExpr>.FromElements();
            while ((_2130_i) < (new BigInteger((_2129_generatedValues).Count))) {
              RAST._IExpr _2138_genKey;
              _2138_genKey = ((_2129_generatedValues).Select(_2130_i)).dtor__0;
              RAST._IExpr _2139_genValue;
              _2139_genValue = ((_2129_generatedValues).Select(_2130_i)).dtor__1;
              _2137_arguments = Dafny.Sequence<RAST._IExpr>.Concat(_2137_arguments, Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=>"), _2138_genKey, _2139_genValue, DAST.Format.BinaryOpFormat.create_NoFormat())));
              _2130_i = (_2130_i) + (BigInteger.One);
            }
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("map!"))).AsExpr()).Apply(_2137_arguments);
            RAST._IExpr _out440;
            DCOMP._IOwnership _out441;
            (this).FromOwned(r, expectedOwnership, out _out440, out _out441);
            r = _out440;
            resultingOwnership = _out441;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_SeqUpdate) {
          DAST._IExpression _2140_expr = _source112.dtor_expr;
          DAST._IExpression _2141_index = _source112.dtor_indexExpr;
          DAST._IExpression _2142_value = _source112.dtor_value;
          {
            RAST._IExpr _2143_exprR;
            DCOMP._IOwnership _2144___v183;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2145_exprIdents;
            RAST._IExpr _out442;
            DCOMP._IOwnership _out443;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out444;
            (this).GenExpr(_2140_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out442, out _out443, out _out444);
            _2143_exprR = _out442;
            _2144___v183 = _out443;
            _2145_exprIdents = _out444;
            RAST._IExpr _2146_indexR;
            DCOMP._IOwnership _2147_indexOwnership;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2148_indexIdents;
            RAST._IExpr _out445;
            DCOMP._IOwnership _out446;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out447;
            (this).GenExpr(_2141_index, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out445, out _out446, out _out447);
            _2146_indexR = _out445;
            _2147_indexOwnership = _out446;
            _2148_indexIdents = _out447;
            RAST._IExpr _2149_valueR;
            DCOMP._IOwnership _2150_valueOwnership;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2151_valueIdents;
            RAST._IExpr _out448;
            DCOMP._IOwnership _out449;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out450;
            (this).GenExpr(_2142_value, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out448, out _out449, out _out450);
            _2149_valueR = _out448;
            _2150_valueOwnership = _out449;
            _2151_valueIdents = _out450;
            r = ((_2143_exprR).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_index"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_2146_indexR, _2149_valueR));
            RAST._IExpr _out451;
            DCOMP._IOwnership _out452;
            (this).FromOwned(r, expectedOwnership, out _out451, out _out452);
            r = _out451;
            resultingOwnership = _out452;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_2145_exprIdents, _2148_indexIdents), _2151_valueIdents);
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_MapUpdate) {
          DAST._IExpression _2152_expr = _source112.dtor_expr;
          DAST._IExpression _2153_index = _source112.dtor_indexExpr;
          DAST._IExpression _2154_value = _source112.dtor_value;
          {
            RAST._IExpr _2155_exprR;
            DCOMP._IOwnership _2156___v184;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2157_exprIdents;
            RAST._IExpr _out453;
            DCOMP._IOwnership _out454;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out455;
            (this).GenExpr(_2152_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out453, out _out454, out _out455);
            _2155_exprR = _out453;
            _2156___v184 = _out454;
            _2157_exprIdents = _out455;
            RAST._IExpr _2158_indexR;
            DCOMP._IOwnership _2159_indexOwnership;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2160_indexIdents;
            RAST._IExpr _out456;
            DCOMP._IOwnership _out457;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out458;
            (this).GenExpr(_2153_index, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out456, out _out457, out _out458);
            _2158_indexR = _out456;
            _2159_indexOwnership = _out457;
            _2160_indexIdents = _out458;
            RAST._IExpr _2161_valueR;
            DCOMP._IOwnership _2162_valueOwnership;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2163_valueIdents;
            RAST._IExpr _out459;
            DCOMP._IOwnership _out460;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out461;
            (this).GenExpr(_2154_value, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out459, out _out460, out _out461);
            _2161_valueR = _out459;
            _2162_valueOwnership = _out460;
            _2163_valueIdents = _out461;
            r = ((_2155_exprR).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_index"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_2158_indexR, _2161_valueR));
            RAST._IExpr _out462;
            DCOMP._IOwnership _out463;
            (this).FromOwned(r, expectedOwnership, out _out462, out _out463);
            r = _out462;
            resultingOwnership = _out463;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_2157_exprIdents, _2160_indexIdents), _2163_valueIdents);
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_This) {
          {
            DCOMP._ISelfInfo _source113 = selfIdent;
            {
              if (_source113.is_ThisTyped) {
                Dafny.ISequence<Dafny.Rune> _2164_id = _source113.dtor_rSelfName;
                DAST._IType _2165_dafnyType = _source113.dtor_dafnyType;
                {
                  RAST._IExpr _out464;
                  DCOMP._IOwnership _out465;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out466;
                  (this).GenIdent(_2164_id, selfIdent, env, expectedOwnership, out _out464, out _out465, out _out466);
                  r = _out464;
                  resultingOwnership = _out465;
                  readIdents = _out466;
                }
                goto after_match42;
              }
            }
            {
              DCOMP._ISelfInfo _2166_None = _source113;
              {
                r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("panic!(\"this outside of a method\")"));
                RAST._IExpr _out467;
                DCOMP._IOwnership _out468;
                (this).FromOwned(r, expectedOwnership, out _out467, out _out468);
                r = _out467;
                resultingOwnership = _out468;
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              }
            }
          after_match42: ;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_Ite) {
          DAST._IExpression _2167_cond = _source112.dtor_cond;
          DAST._IExpression _2168_t = _source112.dtor_thn;
          DAST._IExpression _2169_f = _source112.dtor_els;
          {
            RAST._IExpr _2170_cond;
            DCOMP._IOwnership _2171___v185;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2172_recIdentsCond;
            RAST._IExpr _out469;
            DCOMP._IOwnership _out470;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out471;
            (this).GenExpr(_2167_cond, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out469, out _out470, out _out471);
            _2170_cond = _out469;
            _2171___v185 = _out470;
            _2172_recIdentsCond = _out471;
            RAST._IExpr _2173_fExpr;
            DCOMP._IOwnership _2174_fOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2175_recIdentsF;
            RAST._IExpr _out472;
            DCOMP._IOwnership _out473;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out474;
            (this).GenExpr(_2169_f, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out472, out _out473, out _out474);
            _2173_fExpr = _out472;
            _2174_fOwned = _out473;
            _2175_recIdentsF = _out474;
            RAST._IExpr _2176_tExpr;
            DCOMP._IOwnership _2177___v186;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2178_recIdentsT;
            RAST._IExpr _out475;
            DCOMP._IOwnership _out476;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out477;
            (this).GenExpr(_2168_t, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out475, out _out476, out _out477);
            _2176_tExpr = _out475;
            _2177___v186 = _out476;
            _2178_recIdentsT = _out477;
            r = RAST.Expr.create_IfExpr(_2170_cond, _2176_tExpr, _2173_fExpr);
            RAST._IExpr _out478;
            DCOMP._IOwnership _out479;
            (this).FromOwnership(r, DCOMP.Ownership.create_OwnershipOwned(), expectedOwnership, out _out478, out _out479);
            r = _out478;
            resultingOwnership = _out479;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_2172_recIdentsCond, _2178_recIdentsT), _2175_recIdentsF);
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_UnOp) {
          DAST._IUnaryOp unOp0 = _source112.dtor_unOp;
          if (unOp0.is_Not) {
            DAST._IExpression _2179_e = _source112.dtor_expr;
            DAST.Format._IUnaryOpFormat _2180_format = _source112.dtor_format1;
            {
              RAST._IExpr _2181_recursiveGen;
              DCOMP._IOwnership _2182___v187;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2183_recIdents;
              RAST._IExpr _out480;
              DCOMP._IOwnership _out481;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out482;
              (this).GenExpr(_2179_e, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out480, out _out481, out _out482);
              _2181_recursiveGen = _out480;
              _2182___v187 = _out481;
              _2183_recIdents = _out482;
              r = RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"), _2181_recursiveGen, _2180_format);
              RAST._IExpr _out483;
              DCOMP._IOwnership _out484;
              (this).FromOwned(r, expectedOwnership, out _out483, out _out484);
              r = _out483;
              resultingOwnership = _out484;
              readIdents = _2183_recIdents;
              return ;
            }
            goto after_match41;
          }
        }
      }
      {
        if (_source112.is_UnOp) {
          DAST._IUnaryOp unOp1 = _source112.dtor_unOp;
          if (unOp1.is_BitwiseNot) {
            DAST._IExpression _2184_e = _source112.dtor_expr;
            DAST.Format._IUnaryOpFormat _2185_format = _source112.dtor_format1;
            {
              RAST._IExpr _2186_recursiveGen;
              DCOMP._IOwnership _2187___v188;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2188_recIdents;
              RAST._IExpr _out485;
              DCOMP._IOwnership _out486;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out487;
              (this).GenExpr(_2184_e, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out485, out _out486, out _out487);
              _2186_recursiveGen = _out485;
              _2187___v188 = _out486;
              _2188_recIdents = _out487;
              r = RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("~"), _2186_recursiveGen, _2185_format);
              RAST._IExpr _out488;
              DCOMP._IOwnership _out489;
              (this).FromOwned(r, expectedOwnership, out _out488, out _out489);
              r = _out488;
              resultingOwnership = _out489;
              readIdents = _2188_recIdents;
              return ;
            }
            goto after_match41;
          }
        }
      }
      {
        if (_source112.is_UnOp) {
          DAST._IUnaryOp unOp2 = _source112.dtor_unOp;
          if (unOp2.is_Cardinality) {
            DAST._IExpression _2189_e = _source112.dtor_expr;
            DAST.Format._IUnaryOpFormat _2190_format = _source112.dtor_format1;
            {
              RAST._IExpr _2191_recursiveGen;
              DCOMP._IOwnership _2192_recOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2193_recIdents;
              RAST._IExpr _out490;
              DCOMP._IOwnership _out491;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out492;
              (this).GenExpr(_2189_e, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out490, out _out491, out _out492);
              _2191_recursiveGen = _out490;
              _2192_recOwned = _out491;
              _2193_recIdents = _out492;
              r = ((_2191_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cardinality"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
              RAST._IExpr _out493;
              DCOMP._IOwnership _out494;
              (this).FromOwned(r, expectedOwnership, out _out493, out _out494);
              r = _out493;
              resultingOwnership = _out494;
              readIdents = _2193_recIdents;
              return ;
            }
            goto after_match41;
          }
        }
      }
      {
        if (_source112.is_BinOp) {
          RAST._IExpr _out495;
          DCOMP._IOwnership _out496;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out497;
          (this).GenExprBinary(e, selfIdent, env, expectedOwnership, out _out495, out _out496, out _out497);
          r = _out495;
          resultingOwnership = _out496;
          readIdents = _out497;
          goto after_match41;
        }
      }
      {
        if (_source112.is_ArrayLen) {
          DAST._IExpression _2194_expr = _source112.dtor_expr;
          DAST._IType _2195_exprType = _source112.dtor_exprType;
          BigInteger _2196_dim = _source112.dtor_dim;
          bool _2197_native = _source112.dtor_native;
          {
            RAST._IExpr _2198_recursiveGen;
            DCOMP._IOwnership _2199___v193;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2200_recIdents;
            RAST._IExpr _out498;
            DCOMP._IOwnership _out499;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out500;
            (this).GenExpr(_2194_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out498, out _out499, out _out500);
            _2198_recursiveGen = _out498;
            _2199___v193 = _out499;
            _2200_recIdents = _out500;
            RAST._IType _2201_arrayType;
            RAST._IType _out501;
            _out501 = (this).GenType(_2195_exprType, DCOMP.GenTypeContext.@default());
            _2201_arrayType = _out501;
            if (!((_2201_arrayType).IsObjectOrPointer())) {
              Dafny.ISequence<Dafny.Rune> _2202_msg;
              _2202_msg = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Array length of something not an array but "), (_2201_arrayType)._ToString(DCOMP.__default.IND));
              (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(_2202_msg);
              r = RAST.Expr.create_RawExpr(_2202_msg);
            } else {
              RAST._IType _2203_underlying;
              _2203_underlying = (_2201_arrayType).ObjectOrPointerUnderlying();
              if (((_2196_dim).Sign == 0) && ((_2203_underlying).is_Array)) {
                r = ((((this).read__macro).Apply1(_2198_recursiveGen)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("len"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
              } else {
                if ((_2196_dim).Sign == 0) {
                  r = (((((this).read__macro).Apply1(_2198_recursiveGen)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("data"))).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("len"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
                } else {
                  r = ((((this).read__macro).Apply1(_2198_recursiveGen)).Sel(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("length"), Std.Strings.__default.OfNat(_2196_dim)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_usize")))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
                }
              }
              if (!(_2197_native)) {
                r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1(r);
              }
            }
            RAST._IExpr _out502;
            DCOMP._IOwnership _out503;
            (this).FromOwned(r, expectedOwnership, out _out502, out _out503);
            r = _out502;
            resultingOwnership = _out503;
            readIdents = _2200_recIdents;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_MapKeys) {
          DAST._IExpression _2204_expr = _source112.dtor_expr;
          {
            RAST._IExpr _2205_recursiveGen;
            DCOMP._IOwnership _2206___v194;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2207_recIdents;
            RAST._IExpr _out504;
            DCOMP._IOwnership _out505;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out506;
            (this).GenExpr(_2204_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out504, out _out505, out _out506);
            _2205_recursiveGen = _out504;
            _2206___v194 = _out505;
            _2207_recIdents = _out506;
            readIdents = _2207_recIdents;
            r = ((_2205_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("keys"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            RAST._IExpr _out507;
            DCOMP._IOwnership _out508;
            (this).FromOwned(r, expectedOwnership, out _out507, out _out508);
            r = _out507;
            resultingOwnership = _out508;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_MapValues) {
          DAST._IExpression _2208_expr = _source112.dtor_expr;
          {
            RAST._IExpr _2209_recursiveGen;
            DCOMP._IOwnership _2210___v195;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2211_recIdents;
            RAST._IExpr _out509;
            DCOMP._IOwnership _out510;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out511;
            (this).GenExpr(_2208_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out509, out _out510, out _out511);
            _2209_recursiveGen = _out509;
            _2210___v195 = _out510;
            _2211_recIdents = _out511;
            readIdents = _2211_recIdents;
            r = ((_2209_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("values"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            RAST._IExpr _out512;
            DCOMP._IOwnership _out513;
            (this).FromOwned(r, expectedOwnership, out _out512, out _out513);
            r = _out512;
            resultingOwnership = _out513;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_MapItems) {
          DAST._IExpression _2212_expr = _source112.dtor_expr;
          {
            RAST._IExpr _2213_recursiveGen;
            DCOMP._IOwnership _2214___v196;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2215_recIdents;
            RAST._IExpr _out514;
            DCOMP._IOwnership _out515;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out516;
            (this).GenExpr(_2212_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out514, out _out515, out _out516);
            _2213_recursiveGen = _out514;
            _2214___v196 = _out515;
            _2215_recIdents = _out516;
            readIdents = _2215_recIdents;
            r = ((_2213_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("items"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            RAST._IExpr _out517;
            DCOMP._IOwnership _out518;
            (this).FromOwned(r, expectedOwnership, out _out517, out _out518);
            r = _out517;
            resultingOwnership = _out518;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_SelectFn) {
          DAST._IExpression _2216_on = _source112.dtor_expr;
          Dafny.ISequence<Dafny.Rune> _2217_field = _source112.dtor_field;
          bool _2218_isDatatype = _source112.dtor_onDatatype;
          bool _2219_isStatic = _source112.dtor_isStatic;
          bool _2220_isConstant = _source112.dtor_isConstant;
          Dafny.ISequence<DAST._IType> _2221_arguments = _source112.dtor_arguments;
          {
            RAST._IExpr _2222_onExpr;
            DCOMP._IOwnership _2223_onOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2224_recIdents;
            RAST._IExpr _out519;
            DCOMP._IOwnership _out520;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out521;
            (this).GenExpr(_2216_on, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out519, out _out520, out _out521);
            _2222_onExpr = _out519;
            _2223_onOwned = _out520;
            _2224_recIdents = _out521;
            Dafny.ISequence<Dafny.Rune> _2225_s = Dafny.Sequence<Dafny.Rune>.Empty;
            Dafny.ISequence<Dafny.Rune> _2226_onString;
            _2226_onString = (_2222_onExpr)._ToString(DCOMP.__default.IND);
            if (_2219_isStatic) {
              DCOMP._IEnvironment _2227_lEnv;
              _2227_lEnv = env;
              Dafny.ISequence<_System._ITuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType>> _2228_args;
              _2228_args = Dafny.Sequence<_System._ITuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType>>.FromElements();
              _2225_s = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("|");
              BigInteger _hi47 = new BigInteger((_2221_arguments).Count);
              for (BigInteger _2229_i = BigInteger.Zero; _2229_i < _hi47; _2229_i++) {
                if ((_2229_i).Sign == 1) {
                  _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "));
                }
                RAST._IType _2230_ty;
                RAST._IType _out522;
                _out522 = (this).GenType((_2221_arguments).Select(_2229_i), DCOMP.GenTypeContext.@default());
                _2230_ty = _out522;
                RAST._IType _2231_bTy;
                _2231_bTy = RAST.Type.create_Borrowed(_2230_ty);
                Dafny.ISequence<Dafny.Rune> _2232_name;
                _2232_name = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"), Std.Strings.__default.OfInt(_2229_i));
                _2227_lEnv = (_2227_lEnv).AddAssigned(_2232_name, _2231_bTy);
                _2228_args = Dafny.Sequence<_System._ITuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType>>.Concat(_2228_args, Dafny.Sequence<_System._ITuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType>>.FromElements(_System.Tuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType>.create(_2232_name, _2230_ty)));
                _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, _2232_name), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(": ")), (_2231_bTy)._ToString(DCOMP.__default.IND));
              }
              _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("| ")), _2226_onString), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), DCOMP.__default.escapeVar(_2217_field)), ((_2220_isConstant) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("()")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("));
              BigInteger _hi48 = new BigInteger((_2228_args).Count);
              for (BigInteger _2233_i = BigInteger.Zero; _2233_i < _hi48; _2233_i++) {
                if ((_2233_i).Sign == 1) {
                  _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "));
                }
                _System._ITuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType> _let_tmp_rhs70 = (_2228_args).Select(_2233_i);
                Dafny.ISequence<Dafny.Rune> _2234_name = _let_tmp_rhs70.dtor__0;
                RAST._IType _2235_ty = _let_tmp_rhs70.dtor__1;
                RAST._IExpr _2236_rIdent;
                DCOMP._IOwnership _2237___v197;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2238___v198;
                RAST._IExpr _out523;
                DCOMP._IOwnership _out524;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out525;
                (this).GenIdent(_2234_name, selfIdent, _2227_lEnv, (((_2235_ty).CanReadWithoutClone()) ? (DCOMP.Ownership.create_OwnershipOwned()) : (DCOMP.Ownership.create_OwnershipBorrowed())), out _out523, out _out524, out _out525);
                _2236_rIdent = _out523;
                _2237___v197 = _out524;
                _2238___v198 = _out525;
                _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, (_2236_rIdent)._ToString(DCOMP.__default.IND));
              }
              _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
            } else {
              _2225_s = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{\n");
              _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("let callTarget = (")), _2226_onString), ((object.Equals(_2223_onOwned, DCOMP.Ownership.create_OwnershipOwned())) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString(").clone()")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";\n"));
              Dafny.ISequence<Dafny.Rune> _2239_args;
              _2239_args = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
              BigInteger _2240_i;
              _2240_i = BigInteger.Zero;
              while ((_2240_i) < (new BigInteger((_2221_arguments).Count))) {
                if ((_2240_i).Sign == 1) {
                  _2239_args = Dafny.Sequence<Dafny.Rune>.Concat(_2239_args, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "));
                }
                _2239_args = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2239_args, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("arg")), Std.Strings.__default.OfNat(_2240_i));
                _2240_i = (_2240_i) + (BigInteger.One);
              }
              _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("move |")), _2239_args), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("| {\n"));
              _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("callTarget.")), DCOMP.__default.escapeVar(_2217_field)), ((_2220_isConstant) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("()")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")), _2239_args), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")\n"));
              _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}\n"));
              _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(_2225_s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
            }
            Dafny.ISequence<Dafny.Rune> _2241_typeShape;
            _2241_typeShape = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dyn ::std::ops::Fn(");
            BigInteger _2242_i;
            _2242_i = BigInteger.Zero;
            while ((_2242_i) < (new BigInteger((_2221_arguments).Count))) {
              if ((_2242_i).Sign == 1) {
                _2241_typeShape = Dafny.Sequence<Dafny.Rune>.Concat(_2241_typeShape, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "));
              }
              _2241_typeShape = Dafny.Sequence<Dafny.Rune>.Concat(_2241_typeShape, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&_"));
              _2242_i = (_2242_i) + (BigInteger.One);
            }
            _2241_typeShape = Dafny.Sequence<Dafny.Rune>.Concat(_2241_typeShape, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(") -> _"));
            _2225_s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::std::rc::Rc::new("), _2225_s), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(") as ::std::rc::Rc<")), _2241_typeShape), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">"));
            r = RAST.Expr.create_RawExpr(_2225_s);
            RAST._IExpr _out526;
            DCOMP._IOwnership _out527;
            (this).FromOwned(r, expectedOwnership, out _out526, out _out527);
            r = _out526;
            resultingOwnership = _out527;
            readIdents = _2224_recIdents;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_Select) {
          DAST._IExpression _2243_on = _source112.dtor_expr;
          Dafny.ISequence<Dafny.Rune> _2244_field = _source112.dtor_field;
          bool _2245_isConstant = _source112.dtor_isConstant;
          bool _2246_isDatatype = _source112.dtor_onDatatype;
          DAST._IType _2247_fieldType = _source112.dtor_fieldType;
          {
            if (((_2243_on).is_Companion) || ((_2243_on).is_ExternCompanion)) {
              RAST._IExpr _2248_onExpr;
              DCOMP._IOwnership _2249_onOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2250_recIdents;
              RAST._IExpr _out528;
              DCOMP._IOwnership _out529;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out530;
              (this).GenExpr(_2243_on, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out528, out _out529, out _out530);
              _2248_onExpr = _out528;
              _2249_onOwned = _out529;
              _2250_recIdents = _out530;
              r = ((_2248_onExpr).FSel(DCOMP.__default.escapeVar(_2244_field))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
              RAST._IExpr _out531;
              DCOMP._IOwnership _out532;
              (this).FromOwned(r, expectedOwnership, out _out531, out _out532);
              r = _out531;
              resultingOwnership = _out532;
              readIdents = _2250_recIdents;
              return ;
            } else if (_2246_isDatatype) {
              RAST._IExpr _2251_onExpr;
              DCOMP._IOwnership _2252_onOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2253_recIdents;
              RAST._IExpr _out533;
              DCOMP._IOwnership _out534;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out535;
              (this).GenExpr(_2243_on, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out533, out _out534, out _out535);
              _2251_onExpr = _out533;
              _2252_onOwned = _out534;
              _2253_recIdents = _out535;
              r = ((_2251_onExpr).Sel(DCOMP.__default.escapeVar(_2244_field))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
              RAST._IType _2254_typ;
              RAST._IType _out536;
              _out536 = (this).GenType(_2247_fieldType, DCOMP.GenTypeContext.@default());
              _2254_typ = _out536;
              RAST._IExpr _out537;
              DCOMP._IOwnership _out538;
              (this).FromOwnership(r, DCOMP.Ownership.create_OwnershipBorrowed(), expectedOwnership, out _out537, out _out538);
              r = _out537;
              resultingOwnership = _out538;
              readIdents = _2253_recIdents;
            } else {
              RAST._IExpr _2255_onExpr;
              DCOMP._IOwnership _2256_onOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2257_recIdents;
              RAST._IExpr _out539;
              DCOMP._IOwnership _out540;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out541;
              (this).GenExpr(_2243_on, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out539, out _out540, out _out541);
              _2255_onExpr = _out539;
              _2256_onOwned = _out540;
              _2257_recIdents = _out541;
              r = _2255_onExpr;
              if (!object.Equals(_2255_onExpr, RAST.__default.self)) {
                RAST._IExpr _source114 = _2255_onExpr;
                {
                  if (_source114.is_UnaryOp) {
                    Dafny.ISequence<Dafny.Rune> op15 = _source114.dtor_op1;
                    if (object.Equals(op15, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"))) {
                      RAST._IExpr underlying5 = _source114.dtor_underlying;
                      if (underlying5.is_Identifier) {
                        Dafny.ISequence<Dafny.Rune> name8 = underlying5.dtor_name;
                        if (object.Equals(name8, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))) {
                          r = RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"));
                          goto after_match43;
                        }
                      }
                    }
                  }
                }
                {
                }
              after_match43: ;
                if (((this).ObjectType).is_RcMut) {
                  r = (r).Clone();
                }
                r = ((this).read__macro).Apply1(r);
              }
              r = (r).Sel(DCOMP.__default.escapeVar(_2244_field));
              if (_2245_isConstant) {
                r = (r).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
              }
              r = (r).Clone();
              RAST._IExpr _out542;
              DCOMP._IOwnership _out543;
              (this).FromOwned(r, expectedOwnership, out _out542, out _out543);
              r = _out542;
              resultingOwnership = _out543;
              readIdents = _2257_recIdents;
            }
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_Index) {
          DAST._IExpression _2258_on = _source112.dtor_expr;
          DAST._ICollKind _2259_collKind = _source112.dtor_collKind;
          Dafny.ISequence<DAST._IExpression> _2260_indices = _source112.dtor_indices;
          {
            RAST._IExpr _2261_onExpr;
            DCOMP._IOwnership _2262_onOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2263_recIdents;
            RAST._IExpr _out544;
            DCOMP._IOwnership _out545;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out546;
            (this).GenExpr(_2258_on, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out544, out _out545, out _out546);
            _2261_onExpr = _out544;
            _2262_onOwned = _out545;
            _2263_recIdents = _out546;
            readIdents = _2263_recIdents;
            r = _2261_onExpr;
            bool _2264_hadArray;
            _2264_hadArray = false;
            if (object.Equals(_2259_collKind, DAST.CollKind.create_Array())) {
              r = ((this).read__macro).Apply1(r);
              _2264_hadArray = true;
              if ((new BigInteger((_2260_indices).Count)) > (BigInteger.One)) {
                r = (r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("data"));
              }
            }
            BigInteger _hi49 = new BigInteger((_2260_indices).Count);
            for (BigInteger _2265_i = BigInteger.Zero; _2265_i < _hi49; _2265_i++) {
              if (object.Equals(_2259_collKind, DAST.CollKind.create_Array())) {
                RAST._IExpr _2266_idx;
                DCOMP._IOwnership _2267_idxOwned;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2268_recIdentsIdx;
                RAST._IExpr _out547;
                DCOMP._IOwnership _out548;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out549;
                (this).GenExpr((_2260_indices).Select(_2265_i), selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out547, out _out548, out _out549);
                _2266_idx = _out547;
                _2267_idxOwned = _out548;
                _2268_recIdentsIdx = _out549;
                _2266_idx = RAST.__default.IntoUsize(_2266_idx);
                r = RAST.Expr.create_SelectIndex(r, _2266_idx);
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2268_recIdentsIdx);
              } else {
                RAST._IExpr _2269_idx;
                DCOMP._IOwnership _2270_idxOwned;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2271_recIdentsIdx;
                RAST._IExpr _out550;
                DCOMP._IOwnership _out551;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out552;
                (this).GenExpr((_2260_indices).Select(_2265_i), selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out550, out _out551, out _out552);
                _2269_idx = _out550;
                _2270_idxOwned = _out551;
                _2271_recIdentsIdx = _out552;
                r = ((r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("get"))).Apply1(_2269_idx);
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2271_recIdentsIdx);
              }
            }
            if (_2264_hadArray) {
              r = (r).Clone();
            }
            RAST._IExpr _out553;
            DCOMP._IOwnership _out554;
            (this).FromOwned(r, expectedOwnership, out _out553, out _out554);
            r = _out553;
            resultingOwnership = _out554;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_IndexRange) {
          DAST._IExpression _2272_on = _source112.dtor_expr;
          bool _2273_isArray = _source112.dtor_isArray;
          Std.Wrappers._IOption<DAST._IExpression> _2274_low = _source112.dtor_low;
          Std.Wrappers._IOption<DAST._IExpression> _2275_high = _source112.dtor_high;
          {
            RAST._IExpr _2276_onExpr;
            DCOMP._IOwnership _2277_onOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2278_recIdents;
            RAST._IExpr _out555;
            DCOMP._IOwnership _out556;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out557;
            (this).GenExpr(_2272_on, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out555, out _out556, out _out557);
            _2276_onExpr = _out555;
            _2277_onOwned = _out556;
            _2278_recIdents = _out557;
            readIdents = _2278_recIdents;
            Dafny.ISequence<Dafny.Rune> _2279_methodName;
            if ((_2274_low).is_Some) {
              if ((_2275_high).is_Some) {
                _2279_methodName = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("slice");
              } else {
                _2279_methodName = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("drop");
              }
            } else if ((_2275_high).is_Some) {
              _2279_methodName = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("take");
            } else {
              _2279_methodName = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
            }
            Dafny.ISequence<RAST._IExpr> _2280_arguments;
            _2280_arguments = Dafny.Sequence<RAST._IExpr>.FromElements();
            Std.Wrappers._IOption<DAST._IExpression> _source115 = _2274_low;
            {
              if (_source115.is_Some) {
                DAST._IExpression _2281_l = _source115.dtor_value;
                {
                  RAST._IExpr _2282_lExpr;
                  DCOMP._IOwnership _2283___v201;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2284_recIdentsL;
                  RAST._IExpr _out558;
                  DCOMP._IOwnership _out559;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out560;
                  (this).GenExpr(_2281_l, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out558, out _out559, out _out560);
                  _2282_lExpr = _out558;
                  _2283___v201 = _out559;
                  _2284_recIdentsL = _out560;
                  _2280_arguments = Dafny.Sequence<RAST._IExpr>.Concat(_2280_arguments, Dafny.Sequence<RAST._IExpr>.FromElements(_2282_lExpr));
                  readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2284_recIdentsL);
                }
                goto after_match44;
              }
            }
            {
            }
          after_match44: ;
            Std.Wrappers._IOption<DAST._IExpression> _source116 = _2275_high;
            {
              if (_source116.is_Some) {
                DAST._IExpression _2285_h = _source116.dtor_value;
                {
                  RAST._IExpr _2286_hExpr;
                  DCOMP._IOwnership _2287___v202;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2288_recIdentsH;
                  RAST._IExpr _out561;
                  DCOMP._IOwnership _out562;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out563;
                  (this).GenExpr(_2285_h, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out561, out _out562, out _out563);
                  _2286_hExpr = _out561;
                  _2287___v202 = _out562;
                  _2288_recIdentsH = _out563;
                  _2280_arguments = Dafny.Sequence<RAST._IExpr>.Concat(_2280_arguments, Dafny.Sequence<RAST._IExpr>.FromElements(_2286_hExpr));
                  readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2288_recIdentsH);
                }
                goto after_match45;
              }
            }
            {
            }
          after_match45: ;
            r = _2276_onExpr;
            if (_2273_isArray) {
              if (!(_2279_methodName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) {
                _2279_methodName = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"), _2279_methodName);
              }
              r = ((RAST.__default.dafny__runtime__Sequence).FSel(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_array"), _2279_methodName))).Apply(_2280_arguments);
            } else {
              if (!(_2279_methodName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) {
                r = ((r).Sel(_2279_methodName)).Apply(_2280_arguments);
              }
            }
            RAST._IExpr _out564;
            DCOMP._IOwnership _out565;
            (this).FromOwned(r, expectedOwnership, out _out564, out _out565);
            r = _out564;
            resultingOwnership = _out565;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_TupleSelect) {
          DAST._IExpression _2289_on = _source112.dtor_expr;
          BigInteger _2290_idx = _source112.dtor_index;
          DAST._IType _2291_fieldType = _source112.dtor_fieldType;
          {
            RAST._IExpr _2292_onExpr;
            DCOMP._IOwnership _2293_onOwnership;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2294_recIdents;
            RAST._IExpr _out566;
            DCOMP._IOwnership _out567;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out568;
            (this).GenExpr(_2289_on, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out566, out _out567, out _out568);
            _2292_onExpr = _out566;
            _2293_onOwnership = _out567;
            _2294_recIdents = _out568;
            Dafny.ISequence<Dafny.Rune> _2295_selName;
            _2295_selName = Std.Strings.__default.OfNat(_2290_idx);
            DAST._IType _source117 = _2291_fieldType;
            {
              if (_source117.is_Tuple) {
                Dafny.ISequence<DAST._IType> _2296_tps = _source117.dtor_Tuple_a0;
                if (((_2291_fieldType).is_Tuple) && ((new BigInteger((_2296_tps).Count)) > (RAST.__default.MAX__TUPLE__SIZE))) {
                  _2295_selName = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"), _2295_selName);
                }
                goto after_match46;
              }
            }
            {
            }
          after_match46: ;
            r = ((_2292_onExpr).Sel(_2295_selName)).Clone();
            RAST._IExpr _out569;
            DCOMP._IOwnership _out570;
            (this).FromOwnership(r, DCOMP.Ownership.create_OwnershipOwned(), expectedOwnership, out _out569, out _out570);
            r = _out569;
            resultingOwnership = _out570;
            readIdents = _2294_recIdents;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_Call) {
          DAST._IExpression _2297_on = _source112.dtor_on;
          DAST._ICallName _2298_name = _source112.dtor_callName;
          Dafny.ISequence<DAST._IType> _2299_typeArgs = _source112.dtor_typeArgs;
          Dafny.ISequence<DAST._IExpression> _2300_args = _source112.dtor_args;
          {
            Dafny.ISequence<RAST._IExpr> _2301_argExprs;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2302_recIdents;
            Dafny.ISequence<RAST._IType> _2303_typeExprs;
            Std.Wrappers._IOption<DAST._IResolvedType> _2304_fullNameQualifier;
            Dafny.ISequence<RAST._IExpr> _out571;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out572;
            Dafny.ISequence<RAST._IType> _out573;
            Std.Wrappers._IOption<DAST._IResolvedType> _out574;
            (this).GenArgs(selfIdent, _2298_name, _2299_typeArgs, _2300_args, env, out _out571, out _out572, out _out573, out _out574);
            _2301_argExprs = _out571;
            _2302_recIdents = _out572;
            _2303_typeExprs = _out573;
            _2304_fullNameQualifier = _out574;
            readIdents = _2302_recIdents;
            Std.Wrappers._IOption<DAST._IResolvedType> _source118 = _2304_fullNameQualifier;
            {
              if (_source118.is_Some) {
                DAST._IResolvedType value11 = _source118.dtor_value;
                Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2305_path = value11.dtor_path;
                Dafny.ISequence<DAST._IType> _2306_onTypeArgs = value11.dtor_typeArgs;
                DAST._IResolvedTypeBase _2307_base = value11.dtor_kind;
                RAST._IExpr _2308_fullPath;
                RAST._IExpr _out575;
                _out575 = DCOMP.COMP.GenPathExpr(_2305_path, true);
                _2308_fullPath = _out575;
                Dafny.ISequence<RAST._IType> _2309_onTypeExprs;
                Dafny.ISequence<RAST._IType> _out576;
                _out576 = (this).GenTypeArgs(_2306_onTypeArgs, DCOMP.GenTypeContext.@default());
                _2309_onTypeExprs = _out576;
                RAST._IExpr _2310_onExpr = RAST.Expr.Default();
                DCOMP._IOwnership _2311_recOwnership = DCOMP.Ownership.Default();
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2312_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
                if (((_2307_base).is_Trait) || ((_2307_base).is_Class)) {
                  RAST._IExpr _out577;
                  DCOMP._IOwnership _out578;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out579;
                  (this).GenExpr(_2297_on, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out577, out _out578, out _out579);
                  _2310_onExpr = _out577;
                  _2311_recOwnership = _out578;
                  _2312_recIdents = _out579;
                  _2310_onExpr = ((this).read__macro).Apply1(_2310_onExpr);
                  readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2312_recIdents);
                } else {
                  RAST._IExpr _out580;
                  DCOMP._IOwnership _out581;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out582;
                  (this).GenExpr(_2297_on, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out580, out _out581, out _out582);
                  _2310_onExpr = _out580;
                  _2311_recOwnership = _out581;
                  _2312_recIdents = _out582;
                  readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2312_recIdents);
                }
                r = ((((_2308_fullPath).ApplyType(_2309_onTypeExprs)).FSel(DCOMP.__default.escapeName((_2298_name).dtor_name))).ApplyType(_2303_typeExprs)).Apply(Dafny.Sequence<RAST._IExpr>.Concat(Dafny.Sequence<RAST._IExpr>.FromElements(_2310_onExpr), _2301_argExprs));
                RAST._IExpr _out583;
                DCOMP._IOwnership _out584;
                (this).FromOwned(r, expectedOwnership, out _out583, out _out584);
                r = _out583;
                resultingOwnership = _out584;
                goto after_match47;
              }
            }
            {
              RAST._IExpr _2313_onExpr;
              DCOMP._IOwnership _2314___v208;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2315_recIdents;
              RAST._IExpr _out585;
              DCOMP._IOwnership _out586;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out587;
              (this).GenExpr(_2297_on, selfIdent, env, DCOMP.Ownership.create_OwnershipAutoBorrowed(), out _out585, out _out586, out _out587);
              _2313_onExpr = _out585;
              _2314___v208 = _out586;
              _2315_recIdents = _out587;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2315_recIdents);
              Dafny.ISequence<Dafny.Rune> _2316_renderedName;
              _2316_renderedName = (this).GetMethodName(_2297_on, _2298_name);
              DAST._IExpression _source119 = _2297_on;
              {
                bool disjunctiveMatch15 = false;
                if (_source119.is_Companion) {
                  disjunctiveMatch15 = true;
                }
                if (_source119.is_ExternCompanion) {
                  disjunctiveMatch15 = true;
                }
                if (disjunctiveMatch15) {
                  {
                    _2313_onExpr = (_2313_onExpr).FSel(_2316_renderedName);
                  }
                  goto after_match48;
                }
              }
              {
                {
                  if (!object.Equals(_2313_onExpr, RAST.__default.self)) {
                    DAST._ICallName _source120 = _2298_name;
                    {
                      if (_source120.is_CallName) {
                        Std.Wrappers._IOption<DAST._IType> onType2 = _source120.dtor_onType;
                        if (onType2.is_Some) {
                          DAST._IType _2317_tpe = onType2.dtor_value;
                          RAST._IType _2318_typ;
                          RAST._IType _out588;
                          _out588 = (this).GenType(_2317_tpe, DCOMP.GenTypeContext.@default());
                          _2318_typ = _out588;
                          if ((_2318_typ).IsObjectOrPointer()) {
                            _2313_onExpr = ((this).read__macro).Apply1(_2313_onExpr);
                          }
                          goto after_match49;
                        }
                      }
                    }
                    {
                    }
                  after_match49: ;
                  }
                  _2313_onExpr = (_2313_onExpr).Sel(_2316_renderedName);
                }
              }
            after_match48: ;
              r = ((_2313_onExpr).ApplyType(_2303_typeExprs)).Apply(_2301_argExprs);
              RAST._IExpr _out589;
              DCOMP._IOwnership _out590;
              (this).FromOwned(r, expectedOwnership, out _out589, out _out590);
              r = _out589;
              resultingOwnership = _out590;
              return ;
            }
          after_match47: ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_Lambda) {
          Dafny.ISequence<DAST._IFormal> _2319_paramsDafny = _source112.dtor_params;
          DAST._IType _2320_retType = _source112.dtor_retType;
          Dafny.ISequence<DAST._IStatement> _2321_body = _source112.dtor_body;
          {
            Dafny.ISequence<RAST._IFormal> _2322_params;
            Dafny.ISequence<RAST._IFormal> _out591;
            _out591 = (this).GenParams(_2319_paramsDafny, true);
            _2322_params = _out591;
            Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2323_paramNames;
            _2323_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
            Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> _2324_paramTypesMap;
            _2324_paramTypesMap = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.FromElements();
            BigInteger _hi50 = new BigInteger((_2322_params).Count);
            for (BigInteger _2325_i = BigInteger.Zero; _2325_i < _hi50; _2325_i++) {
              Dafny.ISequence<Dafny.Rune> _2326_name;
              _2326_name = ((_2322_params).Select(_2325_i)).dtor_name;
              _2323_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_2323_paramNames, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_2326_name));
              _2324_paramTypesMap = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_2324_paramTypesMap, _2326_name, ((_2322_params).Select(_2325_i)).dtor_tpe);
            }
            DCOMP._IEnvironment _2327_subEnv;
            _2327_subEnv = ((env).ToOwned()).merge(DCOMP.Environment.create(_2323_paramNames, _2324_paramTypesMap));
            RAST._IExpr _2328_recursiveGen;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2329_recIdents;
            DCOMP._IEnvironment _2330___v218;
            RAST._IExpr _out592;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out593;
            DCOMP._IEnvironment _out594;
            (this).GenStmts(_2321_body, ((!object.Equals(selfIdent, DCOMP.SelfInfo.create_NoSelf())) ? (DCOMP.SelfInfo.create_ThisTyped(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_this"), (selfIdent).dtor_dafnyType)) : (DCOMP.SelfInfo.create_NoSelf())), _2327_subEnv, true, Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_None(), out _out592, out _out593, out _out594);
            _2328_recursiveGen = _out592;
            _2329_recIdents = _out593;
            _2330___v218 = _out594;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            _2329_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_2329_recIdents, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>>((_2331_paramNames) => ((System.Func<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>)(() => {
              var _coll10 = new System.Collections.Generic.List<Dafny.ISequence<Dafny.Rune>>();
              foreach (Dafny.ISequence<Dafny.Rune> _compr_11 in (_2331_paramNames).CloneAsArray()) {
                Dafny.ISequence<Dafny.Rune> _2332_name = (Dafny.ISequence<Dafny.Rune>)_compr_11;
                if ((_2331_paramNames).Contains(_2332_name)) {
                  _coll10.Add(_2332_name);
                }
              }
              return Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromCollection(_coll10);
            }))())(_2323_paramNames));
            RAST._IExpr _2333_allReadCloned;
            _2333_allReadCloned = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
            while (!(_2329_recIdents).Equals(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements())) {
              Dafny.ISequence<Dafny.Rune> _2334_next;
              foreach (Dafny.ISequence<Dafny.Rune> _assign_such_that_4 in (_2329_recIdents).Elements) {
                _2334_next = (Dafny.ISequence<Dafny.Rune>)_assign_such_that_4;
                if ((_2329_recIdents).Contains(_2334_next)) {
                  goto after__ASSIGN_SUCH_THAT_4;
                }
              }
              throw new System.Exception("assign-such-that search produced no value (line 5242)");
            after__ASSIGN_SUCH_THAT_4: ;
              if ((!object.Equals(selfIdent, DCOMP.SelfInfo.create_NoSelf())) && ((_2334_next).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_this")))) {
                RAST._IExpr _2335_selfCloned;
                DCOMP._IOwnership _2336___v219;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2337___v220;
                RAST._IExpr _out595;
                DCOMP._IOwnership _out596;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out597;
                (this).GenIdent(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"), selfIdent, DCOMP.Environment.Empty(), DCOMP.Ownership.create_OwnershipOwned(), out _out595, out _out596, out _out597);
                _2335_selfCloned = _out595;
                _2336___v219 = _out596;
                _2337___v220 = _out597;
                _2333_allReadCloned = (_2333_allReadCloned).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_this"), Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(_2335_selfCloned)));
              } else if (!((_2323_paramNames).Contains(_2334_next))) {
                RAST._IExpr _2338_copy;
                _2338_copy = (RAST.Expr.create_Identifier(_2334_next)).Clone();
                _2333_allReadCloned = (_2333_allReadCloned).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _2334_next, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(_2338_copy)));
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_2334_next));
              }
              _2329_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_2329_recIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_2334_next));
            }
            RAST._IType _2339_retTypeGen;
            RAST._IType _out598;
            _out598 = (this).GenType(_2320_retType, DCOMP.GenTypeContext.@default());
            _2339_retTypeGen = _out598;
            r = RAST.Expr.create_Block((_2333_allReadCloned).Then(RAST.__default.RcNew(RAST.Expr.create_Lambda(_2322_params, Std.Wrappers.Option<RAST._IType>.create_Some(_2339_retTypeGen), RAST.Expr.create_Block(_2328_recursiveGen)))));
            RAST._IExpr _out599;
            DCOMP._IOwnership _out600;
            (this).FromOwned(r, expectedOwnership, out _out599, out _out600);
            r = _out599;
            resultingOwnership = _out600;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_BetaRedex) {
          Dafny.ISequence<_System._ITuple2<DAST._IFormal, DAST._IExpression>> _2340_values = _source112.dtor_values;
          DAST._IType _2341_retType = _source112.dtor_retType;
          DAST._IExpression _2342_expr = _source112.dtor_expr;
          {
            Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2343_paramNames;
            _2343_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
            Dafny.ISequence<RAST._IFormal> _2344_paramFormals;
            Dafny.ISequence<RAST._IFormal> _out601;
            _out601 = (this).GenParams(Std.Collections.Seq.__default.Map<_System._ITuple2<DAST._IFormal, DAST._IExpression>, DAST._IFormal>(((System.Func<_System._ITuple2<DAST._IFormal, DAST._IExpression>, DAST._IFormal>)((_2345_value) => {
              return (_2345_value).dtor__0;
            })), _2340_values), false);
            _2344_paramFormals = _out601;
            Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> _2346_paramTypes;
            _2346_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.FromElements();
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2347_paramNamesSet;
            _2347_paramNamesSet = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            BigInteger _hi51 = new BigInteger((_2340_values).Count);
            for (BigInteger _2348_i = BigInteger.Zero; _2348_i < _hi51; _2348_i++) {
              Dafny.ISequence<Dafny.Rune> _2349_name;
              _2349_name = (((_2340_values).Select(_2348_i)).dtor__0).dtor_name;
              Dafny.ISequence<Dafny.Rune> _2350_rName;
              _2350_rName = DCOMP.__default.escapeVar(_2349_name);
              _2343_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_2343_paramNames, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_2350_rName));
              _2346_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_2346_paramTypes, _2350_rName, ((_2344_paramFormals).Select(_2348_i)).dtor_tpe);
              _2347_paramNamesSet = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_2347_paramNamesSet, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_2350_rName));
            }
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
            BigInteger _hi52 = new BigInteger((_2340_values).Count);
            for (BigInteger _2351_i = BigInteger.Zero; _2351_i < _hi52; _2351_i++) {
              RAST._IType _2352_typeGen;
              RAST._IType _out602;
              _out602 = (this).GenType((((_2340_values).Select(_2351_i)).dtor__0).dtor_typ, DCOMP.GenTypeContext.@default());
              _2352_typeGen = _out602;
              RAST._IExpr _2353_valueGen;
              DCOMP._IOwnership _2354___v221;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2355_recIdents;
              RAST._IExpr _out603;
              DCOMP._IOwnership _out604;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out605;
              (this).GenExpr(((_2340_values).Select(_2351_i)).dtor__1, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out603, out _out604, out _out605);
              _2353_valueGen = _out603;
              _2354___v221 = _out604;
              _2355_recIdents = _out605;
              r = (r).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), DCOMP.__default.escapeVar((((_2340_values).Select(_2351_i)).dtor__0).dtor_name), Std.Wrappers.Option<RAST._IType>.create_Some(_2352_typeGen), Std.Wrappers.Option<RAST._IExpr>.create_Some(_2353_valueGen)));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2355_recIdents);
            }
            DCOMP._IEnvironment _2356_newEnv;
            _2356_newEnv = DCOMP.Environment.create(_2343_paramNames, _2346_paramTypes);
            RAST._IExpr _2357_recGen;
            DCOMP._IOwnership _2358_recOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2359_recIdents;
            RAST._IExpr _out606;
            DCOMP._IOwnership _out607;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out608;
            (this).GenExpr(_2342_expr, selfIdent, _2356_newEnv, expectedOwnership, out _out606, out _out607, out _out608);
            _2357_recGen = _out606;
            _2358_recOwned = _out607;
            _2359_recIdents = _out608;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_2359_recIdents, _2347_paramNamesSet);
            r = RAST.Expr.create_Block((r).Then(_2357_recGen));
            RAST._IExpr _out609;
            DCOMP._IOwnership _out610;
            (this).FromOwnership(r, _2358_recOwned, expectedOwnership, out _out609, out _out610);
            r = _out609;
            resultingOwnership = _out610;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_IIFE) {
          Dafny.ISequence<Dafny.Rune> _2360_name = _source112.dtor_ident;
          DAST._IType _2361_tpe = _source112.dtor_typ;
          DAST._IExpression _2362_value = _source112.dtor_value;
          DAST._IExpression _2363_iifeBody = _source112.dtor_iifeBody;
          {
            RAST._IExpr _2364_valueGen;
            DCOMP._IOwnership _2365___v222;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2366_recIdents;
            RAST._IExpr _out611;
            DCOMP._IOwnership _out612;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out613;
            (this).GenExpr(_2362_value, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out611, out _out612, out _out613);
            _2364_valueGen = _out611;
            _2365___v222 = _out612;
            _2366_recIdents = _out613;
            readIdents = _2366_recIdents;
            RAST._IType _2367_valueTypeGen;
            RAST._IType _out614;
            _out614 = (this).GenType(_2361_tpe, DCOMP.GenTypeContext.@default());
            _2367_valueTypeGen = _out614;
            Dafny.ISequence<Dafny.Rune> _2368_iifeVar;
            _2368_iifeVar = DCOMP.__default.escapeVar(_2360_name);
            RAST._IExpr _2369_bodyGen;
            DCOMP._IOwnership _2370___v223;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2371_bodyIdents;
            RAST._IExpr _out615;
            DCOMP._IOwnership _out616;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out617;
            (this).GenExpr(_2363_iifeBody, selfIdent, (env).AddAssigned(_2368_iifeVar, _2367_valueTypeGen), DCOMP.Ownership.create_OwnershipOwned(), out _out615, out _out616, out _out617);
            _2369_bodyGen = _out615;
            _2370___v223 = _out616;
            _2371_bodyIdents = _out617;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_2371_bodyIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_2368_iifeVar)));
            r = RAST.Expr.create_Block((RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), _2368_iifeVar, Std.Wrappers.Option<RAST._IType>.create_Some(_2367_valueTypeGen), Std.Wrappers.Option<RAST._IExpr>.create_Some(_2364_valueGen))).Then(_2369_bodyGen));
            RAST._IExpr _out618;
            DCOMP._IOwnership _out619;
            (this).FromOwned(r, expectedOwnership, out _out618, out _out619);
            r = _out618;
            resultingOwnership = _out619;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_Apply) {
          DAST._IExpression _2372_func = _source112.dtor_expr;
          Dafny.ISequence<DAST._IExpression> _2373_args = _source112.dtor_args;
          {
            RAST._IExpr _2374_funcExpr;
            DCOMP._IOwnership _2375___v224;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2376_recIdents;
            RAST._IExpr _out620;
            DCOMP._IOwnership _out621;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out622;
            (this).GenExpr(_2372_func, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out620, out _out621, out _out622);
            _2374_funcExpr = _out620;
            _2375___v224 = _out621;
            _2376_recIdents = _out622;
            readIdents = _2376_recIdents;
            Dafny.ISequence<RAST._IExpr> _2377_rArgs;
            _2377_rArgs = Dafny.Sequence<RAST._IExpr>.FromElements();
            BigInteger _hi53 = new BigInteger((_2373_args).Count);
            for (BigInteger _2378_i = BigInteger.Zero; _2378_i < _hi53; _2378_i++) {
              RAST._IExpr _2379_argExpr;
              DCOMP._IOwnership _2380_argOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2381_argIdents;
              RAST._IExpr _out623;
              DCOMP._IOwnership _out624;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out625;
              (this).GenExpr((_2373_args).Select(_2378_i), selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out623, out _out624, out _out625);
              _2379_argExpr = _out623;
              _2380_argOwned = _out624;
              _2381_argIdents = _out625;
              _2377_rArgs = Dafny.Sequence<RAST._IExpr>.Concat(_2377_rArgs, Dafny.Sequence<RAST._IExpr>.FromElements(_2379_argExpr));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _2381_argIdents);
            }
            r = (_2374_funcExpr).Apply(_2377_rArgs);
            RAST._IExpr _out626;
            DCOMP._IOwnership _out627;
            (this).FromOwned(r, expectedOwnership, out _out626, out _out627);
            r = _out626;
            resultingOwnership = _out627;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_TypeTest) {
          DAST._IExpression _2382_on = _source112.dtor_on;
          Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2383_dType = _source112.dtor_dType;
          Dafny.ISequence<Dafny.Rune> _2384_variant = _source112.dtor_variant;
          {
            RAST._IExpr _2385_exprGen;
            DCOMP._IOwnership _2386___v225;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2387_recIdents;
            RAST._IExpr _out628;
            DCOMP._IOwnership _out629;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out630;
            (this).GenExpr(_2382_on, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out628, out _out629, out _out630);
            _2385_exprGen = _out628;
            _2386___v225 = _out629;
            _2387_recIdents = _out630;
            RAST._IType _2388_dTypePath;
            RAST._IType _out631;
            _out631 = DCOMP.COMP.GenPathType(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_2383_dType, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_2384_variant)));
            _2388_dTypePath = _out631;
            r = (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("matches!"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(((_2385_exprGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements()), RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.Concat((_2388_dTypePath)._ToString(DCOMP.__default.IND), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{ .. }")))));
            RAST._IExpr _out632;
            DCOMP._IOwnership _out633;
            (this).FromOwned(r, expectedOwnership, out _out632, out _out633);
            r = _out632;
            resultingOwnership = _out633;
            readIdents = _2387_recIdents;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_Is) {
          DAST._IExpression _2389_expr = _source112.dtor_expr;
          DAST._IType _2390_fromType = _source112.dtor_fromType;
          DAST._IType _2391_toType = _source112.dtor_toType;
          {
            RAST._IExpr _2392_expr;
            DCOMP._IOwnership _2393_recOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2394_recIdents;
            RAST._IExpr _out634;
            DCOMP._IOwnership _out635;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out636;
            (this).GenExpr(_2389_expr, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out634, out _out635, out _out636);
            _2392_expr = _out634;
            _2393_recOwned = _out635;
            _2394_recIdents = _out636;
            RAST._IType _2395_fromType;
            RAST._IType _out637;
            _out637 = (this).GenType(_2390_fromType, DCOMP.GenTypeContext.@default());
            _2395_fromType = _out637;
            RAST._IType _2396_toType;
            RAST._IType _out638;
            _out638 = (this).GenType(_2391_toType, DCOMP.GenTypeContext.@default());
            _2396_toType = _out638;
            if (((_2395_fromType).IsObjectOrPointer()) && ((_2396_toType).IsObjectOrPointer())) {
              r = (((_2392_expr).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("is_instance_of"))).ApplyType(Dafny.Sequence<RAST._IType>.FromElements((_2396_toType).ObjectOrPointerUnderlying()))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            } else {
              (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Source and/or target types of type test is/are not Object or Ptr"));
              r = RAST.Expr.create_RawExpr((this.error).dtor_value);
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            }
            RAST._IExpr _out639;
            DCOMP._IOwnership _out640;
            (this).FromOwnership(r, _2393_recOwned, expectedOwnership, out _out639, out _out640);
            r = _out639;
            resultingOwnership = _out640;
            readIdents = _2394_recIdents;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_BoolBoundedPool) {
          {
            r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("[false, true]"));
            RAST._IExpr _out641;
            DCOMP._IOwnership _out642;
            (this).FromOwned(r, expectedOwnership, out _out641, out _out642);
            r = _out641;
            resultingOwnership = _out642;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_SetBoundedPool) {
          DAST._IExpression _2397_of = _source112.dtor_of;
          {
            RAST._IExpr _2398_exprGen;
            DCOMP._IOwnership _2399___v226;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2400_recIdents;
            RAST._IExpr _out643;
            DCOMP._IOwnership _out644;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out645;
            (this).GenExpr(_2397_of, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out643, out _out644, out _out645);
            _2398_exprGen = _out643;
            _2399___v226 = _out644;
            _2400_recIdents = _out645;
            r = ((_2398_exprGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("iter"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            RAST._IExpr _out646;
            DCOMP._IOwnership _out647;
            (this).FromOwned(r, expectedOwnership, out _out646, out _out647);
            r = _out646;
            resultingOwnership = _out647;
            readIdents = _2400_recIdents;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_SeqBoundedPool) {
          DAST._IExpression _2401_of = _source112.dtor_of;
          bool _2402_includeDuplicates = _source112.dtor_includeDuplicates;
          {
            RAST._IExpr _2403_exprGen;
            DCOMP._IOwnership _2404___v227;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2405_recIdents;
            RAST._IExpr _out648;
            DCOMP._IOwnership _out649;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out650;
            (this).GenExpr(_2401_of, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out648, out _out649, out _out650);
            _2403_exprGen = _out648;
            _2404___v227 = _out649;
            _2405_recIdents = _out650;
            r = ((_2403_exprGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("iter"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            if (!(_2402_includeDuplicates)) {
              r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("itertools"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Itertools"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unique"))).AsExpr()).Apply1(r);
            }
            RAST._IExpr _out651;
            DCOMP._IOwnership _out652;
            (this).FromOwned(r, expectedOwnership, out _out651, out _out652);
            r = _out651;
            resultingOwnership = _out652;
            readIdents = _2405_recIdents;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_MapBoundedPool) {
          DAST._IExpression _2406_of = _source112.dtor_of;
          {
            RAST._IExpr _2407_exprGen;
            DCOMP._IOwnership _2408___v228;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2409_recIdents;
            RAST._IExpr _out653;
            DCOMP._IOwnership _out654;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out655;
            (this).GenExpr(_2406_of, selfIdent, env, DCOMP.Ownership.create_OwnershipBorrowed(), out _out653, out _out654, out _out655);
            _2407_exprGen = _out653;
            _2408___v228 = _out654;
            _2409_recIdents = _out655;
            r = ((((_2407_exprGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("keys"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements())).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("iter"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            readIdents = _2409_recIdents;
            RAST._IExpr _out656;
            DCOMP._IOwnership _out657;
            (this).FromOwned(r, expectedOwnership, out _out656, out _out657);
            r = _out656;
            resultingOwnership = _out657;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_IntRange) {
          DAST._IExpression _2410_lo = _source112.dtor_lo;
          DAST._IExpression _2411_hi = _source112.dtor_hi;
          bool _2412_up = _source112.dtor_up;
          {
            RAST._IExpr _2413_lo;
            DCOMP._IOwnership _2414___v229;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2415_recIdentsLo;
            RAST._IExpr _out658;
            DCOMP._IOwnership _out659;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out660;
            (this).GenExpr(_2410_lo, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out658, out _out659, out _out660);
            _2413_lo = _out658;
            _2414___v229 = _out659;
            _2415_recIdentsLo = _out660;
            RAST._IExpr _2416_hi;
            DCOMP._IOwnership _2417___v230;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2418_recIdentsHi;
            RAST._IExpr _out661;
            DCOMP._IOwnership _out662;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out663;
            (this).GenExpr(_2411_hi, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out661, out _out662, out _out663);
            _2416_hi = _out661;
            _2417___v230 = _out662;
            _2418_recIdentsHi = _out663;
            if (_2412_up) {
              r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("integer_range"))).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_2413_lo, _2416_hi));
            } else {
              r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("integer_range_down"))).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_2416_hi, _2413_lo));
            }
            RAST._IExpr _out664;
            DCOMP._IOwnership _out665;
            (this).FromOwned(r, expectedOwnership, out _out664, out _out665);
            r = _out664;
            resultingOwnership = _out665;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_2415_recIdentsLo, _2418_recIdentsHi);
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_UnboundedIntRange) {
          DAST._IExpression _2419_start = _source112.dtor_start;
          bool _2420_up = _source112.dtor_up;
          {
            RAST._IExpr _2421_start;
            DCOMP._IOwnership _2422___v231;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2423_recIdentStart;
            RAST._IExpr _out666;
            DCOMP._IOwnership _out667;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out668;
            (this).GenExpr(_2419_start, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out666, out _out667, out _out668);
            _2421_start = _out666;
            _2422___v231 = _out667;
            _2423_recIdentStart = _out668;
            if (_2420_up) {
              r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("integer_range_unbounded"))).AsExpr()).Apply1(_2421_start);
            } else {
              r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("integer_range_down_unbounded"))).AsExpr()).Apply1(_2421_start);
            }
            RAST._IExpr _out669;
            DCOMP._IOwnership _out670;
            (this).FromOwned(r, expectedOwnership, out _out669, out _out670);
            r = _out669;
            resultingOwnership = _out670;
            readIdents = _2423_recIdentStart;
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_MapBuilder) {
          DAST._IType _2424_keyType = _source112.dtor_keyType;
          DAST._IType _2425_valueType = _source112.dtor_valueType;
          {
            RAST._IType _2426_kType;
            RAST._IType _out671;
            _out671 = (this).GenType(_2424_keyType, DCOMP.GenTypeContext.@default());
            _2426_kType = _out671;
            RAST._IType _2427_vType;
            RAST._IType _out672;
            _out672 = (this).GenType(_2425_valueType, DCOMP.GenTypeContext.@default());
            _2427_vType = _out672;
            r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("MapBuilder"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_2426_kType, _2427_vType))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            RAST._IExpr _out673;
            DCOMP._IOwnership _out674;
            (this).FromOwned(r, expectedOwnership, out _out673, out _out674);
            r = _out673;
            resultingOwnership = _out674;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            return ;
          }
          goto after_match41;
        }
      }
      {
        if (_source112.is_SetBuilder) {
          DAST._IType _2428_elemType = _source112.dtor_elemType;
          {
            RAST._IType _2429_eType;
            RAST._IType _out675;
            _out675 = (this).GenType(_2428_elemType, DCOMP.GenTypeContext.@default());
            _2429_eType = _out675;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("SetBuilder"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_2429_eType))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
            RAST._IExpr _out676;
            DCOMP._IOwnership _out677;
            (this).FromOwned(r, expectedOwnership, out _out676, out _out677);
            r = _out676;
            resultingOwnership = _out677;
            return ;
          }
          goto after_match41;
        }
      }
      {
        DAST._IType _2430_elemType = _source112.dtor_elemType;
        DAST._IExpression _2431_collection = _source112.dtor_collection;
        bool _2432_is__forall = _source112.dtor_is__forall;
        DAST._IExpression _2433_lambda = _source112.dtor_lambda;
        {
          RAST._IType _2434_tpe;
          RAST._IType _out678;
          _out678 = (this).GenType(_2430_elemType, DCOMP.GenTypeContext.@default());
          _2434_tpe = _out678;
          RAST._IExpr _2435_collectionGen;
          DCOMP._IOwnership _2436___v232;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2437_recIdents;
          RAST._IExpr _out679;
          DCOMP._IOwnership _out680;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out681;
          (this).GenExpr(_2431_collection, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out679, out _out680, out _out681);
          _2435_collectionGen = _out679;
          _2436___v232 = _out680;
          _2437_recIdents = _out681;
          Dafny.ISequence<DAST._IAttribute> _2438_extraAttributes;
          _2438_extraAttributes = Dafny.Sequence<DAST._IAttribute>.FromElements();
          if ((((_2431_collection).is_IntRange) || ((_2431_collection).is_UnboundedIntRange)) || ((_2431_collection).is_SeqBoundedPool)) {
            _2438_extraAttributes = Dafny.Sequence<DAST._IAttribute>.FromElements(DCOMP.__default.AttributeOwned);
          }
          if ((_2433_lambda).is_Lambda) {
            Dafny.ISequence<DAST._IFormal> _2439_formals;
            _2439_formals = (_2433_lambda).dtor_params;
            Dafny.ISequence<DAST._IFormal> _2440_newFormals;
            _2440_newFormals = Dafny.Sequence<DAST._IFormal>.FromElements();
            BigInteger _hi54 = new BigInteger((_2439_formals).Count);
            for (BigInteger _2441_i = BigInteger.Zero; _2441_i < _hi54; _2441_i++) {
              var _pat_let_tv9 = _2438_extraAttributes;
              var _pat_let_tv10 = _2439_formals;
              _2440_newFormals = Dafny.Sequence<DAST._IFormal>.Concat(_2440_newFormals, Dafny.Sequence<DAST._IFormal>.FromElements(Dafny.Helpers.Let<DAST._IFormal, DAST._IFormal>((_2439_formals).Select(_2441_i), _pat_let25_0 => Dafny.Helpers.Let<DAST._IFormal, DAST._IFormal>(_pat_let25_0, _2442_dt__update__tmp_h0 => Dafny.Helpers.Let<Dafny.ISequence<DAST._IAttribute>, DAST._IFormal>(Dafny.Sequence<DAST._IAttribute>.Concat(_pat_let_tv9, ((_pat_let_tv10).Select(_2441_i)).dtor_attributes), _pat_let26_0 => Dafny.Helpers.Let<Dafny.ISequence<DAST._IAttribute>, DAST._IFormal>(_pat_let26_0, _2443_dt__update_hattributes_h0 => DAST.Formal.create((_2442_dt__update__tmp_h0).dtor_name, (_2442_dt__update__tmp_h0).dtor_typ, _2443_dt__update_hattributes_h0)))))));
            }
            DAST._IExpression _2444_newLambda;
            DAST._IExpression _2445_dt__update__tmp_h1 = _2433_lambda;
            Dafny.ISequence<DAST._IFormal> _2446_dt__update_hparams_h0 = _2440_newFormals;
            _2444_newLambda = DAST.Expression.create_Lambda(_2446_dt__update_hparams_h0, (_2445_dt__update__tmp_h1).dtor_retType, (_2445_dt__update__tmp_h1).dtor_body);
            RAST._IExpr _2447_lambdaGen;
            DCOMP._IOwnership _2448___v233;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2449_recLambdaIdents;
            RAST._IExpr _out682;
            DCOMP._IOwnership _out683;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out684;
            (this).GenExpr(_2444_newLambda, selfIdent, env, DCOMP.Ownership.create_OwnershipOwned(), out _out682, out _out683, out _out684);
            _2447_lambdaGen = _out682;
            _2448___v233 = _out683;
            _2449_recLambdaIdents = _out684;
            Dafny.ISequence<Dafny.Rune> _2450_fn;
            if (_2432_is__forall) {
              _2450_fn = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("all");
            } else {
              _2450_fn = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("any");
            }
            r = ((_2435_collectionGen).Sel(_2450_fn)).Apply1(((_2447_lambdaGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements()));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_2437_recIdents, _2449_recLambdaIdents);
          } else {
            (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Quantifier without an inline lambda"));
            r = RAST.Expr.create_RawExpr((this.error).dtor_value);
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
          }
          RAST._IExpr _out685;
          DCOMP._IOwnership _out686;
          (this).FromOwned(r, expectedOwnership, out _out685, out _out686);
          r = _out685;
          resultingOwnership = _out686;
        }
      }
    after_match41: ;
    }
    public Dafny.ISequence<Dafny.Rune> Compile(Dafny.ISequence<DAST._IModule> p, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> externalFiles)
    {
      Dafny.ISequence<Dafny.Rune> s = Dafny.Sequence<Dafny.Rune>.Empty;
      s = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#![allow(warnings, unconditional_panic)]\n");
      s = Dafny.Sequence<Dafny.Rune>.Concat(s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#![allow(nonstandard_style)]\n"));
      Dafny.ISequence<RAST._IModDecl> _2451_externUseDecls;
      _2451_externUseDecls = Dafny.Sequence<RAST._IModDecl>.FromElements();
      BigInteger _hi55 = new BigInteger((externalFiles).Count);
      for (BigInteger _2452_i = BigInteger.Zero; _2452_i < _hi55; _2452_i++) {
        Dafny.ISequence<Dafny.Rune> _2453_externalFile;
        _2453_externalFile = (externalFiles).Select(_2452_i);
        Dafny.ISequence<Dafny.Rune> _2454_externalMod;
        _2454_externalMod = _2453_externalFile;
        if (((new BigInteger((_2453_externalFile).Count)) > (new BigInteger(3))) && (((_2453_externalFile).Drop((new BigInteger((_2453_externalFile).Count)) - (new BigInteger(3)))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".rs")))) {
          _2454_externalMod = (_2453_externalFile).Subsequence(BigInteger.Zero, (new BigInteger((_2453_externalFile).Count)) - (new BigInteger(3)));
        } else {
          (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Unrecognized external file "), _2453_externalFile), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(". External file must be *.rs files")));
        }
        RAST._IMod _2455_externMod;
        _2455_externMod = RAST.Mod.create_ExternMod(_2454_externalMod);
        s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(s, (_2455_externMod)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"));
        _2451_externUseDecls = Dafny.Sequence<RAST._IModDecl>.Concat(_2451_externUseDecls, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_UseDecl(RAST.Use.create(RAST.Visibility.create_PUB(), ((RAST.__default.crate).MSel(_2454_externalMod)).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"))))));
      }
      if (!(_2451_externUseDecls).Equals(Dafny.Sequence<RAST._IModDecl>.FromElements())) {
        s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(s, (RAST.Mod.create_Mod(DCOMP.COMP.DAFNY__EXTERN__MODULE, _2451_externUseDecls))._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"));
      }
      DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _2456_allModules;
      _2456_allModules = DafnyCompilerRustUtils.SeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule>.Empty();
      BigInteger _hi56 = new BigInteger((p).Count);
      for (BigInteger _2457_i = BigInteger.Zero; _2457_i < _hi56; _2457_i++) {
        DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _2458_m;
        DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _out687;
        _out687 = (this).GenModule((p).Select(_2457_i), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements());
        _2458_m = _out687;
        _2456_allModules = DafnyCompilerRustUtils.GatheringModule.MergeSeqMap(_2456_allModules, _2458_m);
      }
      BigInteger _hi57 = new BigInteger(((_2456_allModules).dtor_keys).Count);
      for (BigInteger _2459_i = BigInteger.Zero; _2459_i < _hi57; _2459_i++) {
        if (!((_2456_allModules).dtor_values).Contains(((_2456_allModules).dtor_keys).Select(_2459_i))) {
          goto continue_0;
        }
        RAST._IMod _2460_m;
        _2460_m = (Dafny.Map<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule>.Select((_2456_allModules).dtor_values,((_2456_allModules).dtor_keys).Select(_2459_i))).ToRust();
        BigInteger _hi58 = new BigInteger((this.optimizations).Count);
        for (BigInteger _2461_j = BigInteger.Zero; _2461_j < _hi58; _2461_j++) {
          _2460_m = Dafny.Helpers.Id<Func<RAST._IMod, RAST._IMod>>((this.optimizations).Select(_2461_j))(_2460_m);
        }
        s = Dafny.Sequence<Dafny.Rune>.Concat(s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"));
        s = Dafny.Sequence<Dafny.Rune>.Concat(s, (_2460_m)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")));
      continue_0: ;
      }
    after_0: ;
      return s;
    }
    public static Dafny.ISequence<Dafny.Rune> EmitCallToMain(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> fullName)
    {
      Dafny.ISequence<Dafny.Rune> s = Dafny.Sequence<Dafny.Rune>.Empty;
      s = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\nfn main() {\n");
      BigInteger _2462_i;
      _2462_i = BigInteger.Zero;
      while ((_2462_i) < (new BigInteger((fullName).Count))) {
        if ((_2462_i).Sign == 1) {
          s = Dafny.Sequence<Dafny.Rune>.Concat(s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::"));
        }
        s = Dafny.Sequence<Dafny.Rune>.Concat(s, DCOMP.__default.escapeName((fullName).Select(_2462_i)));
        _2462_i = (_2462_i) + (BigInteger.One);
      }
      s = Dafny.Sequence<Dafny.Rune>.Concat(s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("();\n}"));
      return s;
    }
    public bool _UnicodeChars {get; set;}
    public bool UnicodeChars { get {
      return this._UnicodeChars;
    } }
    public Dafny.ISequence<Dafny.Rune> DafnyChar { get {
      if ((this).UnicodeChars) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyChar");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyCharUTF16");
      }
    } }
    public RAST._IType DafnyCharUnderlying { get {
      if ((this).UnicodeChars) {
        return RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("char"));
      } else {
        return RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u16"));
      }
    } }
    public Dafny.ISequence<Dafny.Rune> string__of { get {
      if ((this).UnicodeChars) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("string_of");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("string_utf16_of");
      }
    } }
    public DCOMP._IObjectType _ObjectType {get; set;}
    public DCOMP._IObjectType ObjectType { get {
      return this._ObjectType;
    } }
    public Dafny.ISequence<Dafny.Rune> allocate { get {
      if (((this).ObjectType).is_RawPointers) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("allocate");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("allocate_object");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> allocate__fn { get {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"), (this).allocate);
    } }
    public Dafny.ISequence<Dafny.Rune> update__field__uninit__macro { get {
      if (((this).ObjectType).is_RawPointers) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_uninit!");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_uninit_object!");
      }
    } }
    public RAST._IExpr thisInConstructor { get {
      if (((this).ObjectType).is_RawPointers) {
        return RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"));
      } else {
        return (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))).Clone();
      }
    } }
    public Dafny.ISequence<Dafny.Rune> array__construct { get {
      if (((this).ObjectType).is_RawPointers) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("construct");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("construct_object");
      }
    } }
    public RAST._IExpr modify__macro { get {
      return ((RAST.__default.dafny__runtime).MSel(((((this).ObjectType).is_RawPointers) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("modify!")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("md!"))))).AsExpr();
    } }
    public RAST._IExpr read__macro { get {
      return ((RAST.__default.dafny__runtime).MSel(((((this).ObjectType).is_RawPointers) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("read!")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("rd!"))))).AsExpr();
    } }
    public Dafny.ISequence<Dafny.Rune> placebos__usize { get {
      if (((this).ObjectType).is_RawPointers) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("placebos_usize");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("placebos_usize_object");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> update__field__if__uninit__macro { get {
      if (((this).ObjectType).is_RawPointers) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_if_uninit!");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_if_uninit_object!");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> Upcast { get {
      if (((this).ObjectType).is_RawPointers) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Upcast");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("UpcastObject");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> UpcastFnMacro { get {
      return Dafny.Sequence<Dafny.Rune>.Concat((this).Upcast, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Fn!"));
    } }
    public Dafny.ISequence<Dafny.Rune> upcast { get {
      if (((this).ObjectType).is_RawPointers) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("upcast");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("upcast_object");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> downcast { get {
      if (((this).ObjectType).is_RawPointers) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cast!");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cast_object!");
      }
    } }
    public static Dafny.IMap<DAST._IBinOp,Dafny.ISequence<Dafny.Rune>> OpTable { get {
      return Dafny.Map<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>.FromElements(new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_Mod(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("%")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_And(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&&")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_Or(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("||")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_Div(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_Lt(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_LtChar(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_Plus(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("+")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_Minus(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("-")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_Times(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_BitwiseAnd(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_BitwiseOr(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("|")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_BitwiseXor(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("^")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_BitwiseShiftRight(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">>")), new Dafny.Pair<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>(DAST.BinOp.create_BitwiseShiftLeft(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<<")));
    } }
    public static Dafny.ISequence<Dafny.Rune> DAFNY__EXTERN__MODULE { get {
      return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_dafny_externs");
    } }
  }
} // end of namespace DCOMP