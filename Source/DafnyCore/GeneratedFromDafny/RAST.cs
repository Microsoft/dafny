// Dafny program the_program compiled into C#
// To recompile, you will need the libraries
//     System.Runtime.Numerics.dll System.Collections.Immutable.dll
// but the 'dotnet' tool in net6.0 should pick those up automatically.
// Optionally, you may want to include compiler switches like
//     /debug /nowarn:162,164,168,183,219,436,1717,1718

using System;
using System.Numerics;
using System.Collections;

namespace RAST {

  public partial class __default {
    public static Dafny.ISequence<Dafny.Rune> SeqToString<__T>(Dafny.ISequence<__T> s, Func<__T, Dafny.ISequence<Dafny.Rune>> f, Dafny.ISequence<Dafny.Rune> separator)
    {
      if ((new BigInteger((s).Count)).Sign == 0) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
      } else {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Helpers.Id<Func<__T, Dafny.ISequence<Dafny.Rune>>>(f)((s).Select(BigInteger.Zero)), (((new BigInteger((s).Count)) > (BigInteger.One)) ? (Dafny.Sequence<Dafny.Rune>.Concat(separator, RAST.__default.SeqToString<__T>((s).Drop(BigInteger.One), f, separator))) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))));
      }
    }
    public static RAST._IType ObjectType(RAST._IType underlying) {
      return ((RAST.__default.dafny__runtime__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Object"))).Apply(Dafny.Sequence<RAST._IType>.FromElements(underlying));
    }
    public static RAST._IType Rc(RAST._IType underlying) {
      return RAST.Type.create_TypeApp(RAST.__default.RcType, Dafny.Sequence<RAST._IType>.FromElements(underlying));
    }
    public static RAST._IType RefCell(RAST._IType underlying) {
      return RAST.Type.create_TypeApp(((RAST.__default.std__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cell"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("RefCell")), Dafny.Sequence<RAST._IType>.FromElements(underlying));
    }
    public static RAST._IType Vec(RAST._IType underlying) {
      return RAST.Type.create_TypeApp(((RAST.__default.std__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("vec"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Vec")), Dafny.Sequence<RAST._IType>.FromElements(underlying));
    }
    public static RAST._IExpr NewVec(Dafny.ISequence<RAST._IExpr> elements) {
      return (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("vec!"))).Apply(elements);
    }
    public static RAST._IExpr Clone(RAST._IExpr underlying) {
      return (RAST.Expr.create_Select(underlying, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("clone"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
    }
    public static RAST._IExpr Borrow(RAST._IExpr underlying) {
      return RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), underlying, DAST.Format.UnaryOpFormat.create_NoFormat());
    }
    public static RAST._IExpr BorrowMut(RAST._IExpr underlying) {
      return RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&mut"), underlying, DAST.Format.UnaryOpFormat.create_NoFormat());
    }
    public static RAST._IType RawType(Dafny.ISequence<Dafny.Rune> content) {
      return RAST.Type.create_TIdentifier(content);
    }
    public static RAST._IType Box(RAST._IType content) {
      return RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Box")), Dafny.Sequence<RAST._IType>.FromElements(content));
    }
    public static RAST._IExpr BoxNew(RAST._IExpr content) {
      return ((RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Box"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(content));
    }
    public static bool IsImmutableConversion(RAST._IType fromTpe, RAST._IType toTpe)
    {
      _System._ITuple2<RAST._IType, RAST._IType> _source25 = _System.Tuple2<RAST._IType, RAST._IType>.create(fromTpe, toTpe);
      RAST._IType _830___mcc_h0 = _source25.dtor__0;
      RAST._IType _831___mcc_h1 = _source25.dtor__1;
      RAST._IType _source26 = _830___mcc_h0;
      if (_source26.is_SelfOwned) {
        return false;
      } else if (_source26.is_U8) {
        return false;
      } else if (_source26.is_U16) {
        return false;
      } else if (_source26.is_U32) {
        return false;
      } else if (_source26.is_U64) {
        return false;
      } else if (_source26.is_U128) {
        return false;
      } else if (_source26.is_I8) {
        return false;
      } else if (_source26.is_I16) {
        return false;
      } else if (_source26.is_I32) {
        return false;
      } else if (_source26.is_I64) {
        return false;
      } else if (_source26.is_I128) {
        return false;
      } else if (_source26.is_Bool) {
        return false;
      } else if (_source26.is_TIdentifier) {
        Dafny.ISequence<Dafny.Rune> _832___mcc_h4 = _source26.dtor_name;
        return false;
      } else if (_source26.is_TMemberSelect) {
        RAST._IType _833___mcc_h6 = _source26.dtor_base;
        Dafny.ISequence<Dafny.Rune> _834___mcc_h7 = _source26.dtor_name;
        return false;
      } else if (_source26.is_TypeApp) {
        RAST._IType _835___mcc_h10 = _source26.dtor_baseName;
        Dafny.ISequence<RAST._IType> _836___mcc_h11 = _source26.dtor_arguments;
        RAST._IType _source27 = _835___mcc_h10;
        if (_source27.is_SelfOwned) {
          return false;
        } else if (_source27.is_U8) {
          return false;
        } else if (_source27.is_U16) {
          return false;
        } else if (_source27.is_U32) {
          return false;
        } else if (_source27.is_U64) {
          return false;
        } else if (_source27.is_U128) {
          return false;
        } else if (_source27.is_I8) {
          return false;
        } else if (_source27.is_I16) {
          return false;
        } else if (_source27.is_I32) {
          return false;
        } else if (_source27.is_I64) {
          return false;
        } else if (_source27.is_I128) {
          return false;
        } else if (_source27.is_Bool) {
          return false;
        } else if (_source27.is_TIdentifier) {
          Dafny.ISequence<Dafny.Rune> _837___mcc_h14 = _source27.dtor_name;
          return false;
        } else if (_source27.is_TMemberSelect) {
          RAST._IType _838___mcc_h16 = _source27.dtor_base;
          Dafny.ISequence<Dafny.Rune> _839___mcc_h17 = _source27.dtor_name;
          RAST._IType _source28 = _838___mcc_h16;
          if (_source28.is_SelfOwned) {
            return false;
          } else if (_source28.is_U8) {
            return false;
          } else if (_source28.is_U16) {
            return false;
          } else if (_source28.is_U32) {
            return false;
          } else if (_source28.is_U64) {
            return false;
          } else if (_source28.is_U128) {
            return false;
          } else if (_source28.is_I8) {
            return false;
          } else if (_source28.is_I16) {
            return false;
          } else if (_source28.is_I32) {
            return false;
          } else if (_source28.is_I64) {
            return false;
          } else if (_source28.is_I128) {
            return false;
          } else if (_source28.is_Bool) {
            return false;
          } else if (_source28.is_TIdentifier) {
            Dafny.ISequence<Dafny.Rune> _840___mcc_h20 = _source28.dtor_name;
            return false;
          } else if (_source28.is_TMemberSelect) {
            RAST._IType _841___mcc_h22 = _source28.dtor_base;
            Dafny.ISequence<Dafny.Rune> _842___mcc_h23 = _source28.dtor_name;
            RAST._IType _source29 = _841___mcc_h22;
            if (_source29.is_SelfOwned) {
              return false;
            } else if (_source29.is_U8) {
              return false;
            } else if (_source29.is_U16) {
              return false;
            } else if (_source29.is_U32) {
              return false;
            } else if (_source29.is_U64) {
              return false;
            } else if (_source29.is_U128) {
              return false;
            } else if (_source29.is_I8) {
              return false;
            } else if (_source29.is_I16) {
              return false;
            } else if (_source29.is_I32) {
              return false;
            } else if (_source29.is_I64) {
              return false;
            } else if (_source29.is_I128) {
              return false;
            } else if (_source29.is_Bool) {
              return false;
            } else if (_source29.is_TIdentifier) {
              Dafny.ISequence<Dafny.Rune> _843___mcc_h26 = _source29.dtor_name;
              if (object.Equals(_843___mcc_h26, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) {
                if (object.Equals(_842___mcc_h23, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime"))) {
                  RAST._IType _source30 = _831___mcc_h1;
                  if (_source30.is_SelfOwned) {
                    return false;
                  } else if (_source30.is_U8) {
                    return false;
                  } else if (_source30.is_U16) {
                    return false;
                  } else if (_source30.is_U32) {
                    return false;
                  } else if (_source30.is_U64) {
                    return false;
                  } else if (_source30.is_U128) {
                    return false;
                  } else if (_source30.is_I8) {
                    return false;
                  } else if (_source30.is_I16) {
                    return false;
                  } else if (_source30.is_I32) {
                    return false;
                  } else if (_source30.is_I64) {
                    return false;
                  } else if (_source30.is_I128) {
                    return false;
                  } else if (_source30.is_Bool) {
                    return false;
                  } else if (_source30.is_TIdentifier) {
                    Dafny.ISequence<Dafny.Rune> _844___mcc_h28 = _source30.dtor_name;
                    return false;
                  } else if (_source30.is_TMemberSelect) {
                    RAST._IType _845___mcc_h30 = _source30.dtor_base;
                    Dafny.ISequence<Dafny.Rune> _846___mcc_h31 = _source30.dtor_name;
                    return false;
                  } else if (_source30.is_TypeApp) {
                    RAST._IType _847___mcc_h34 = _source30.dtor_baseName;
                    Dafny.ISequence<RAST._IType> _848___mcc_h35 = _source30.dtor_arguments;
                    RAST._IType _source31 = _847___mcc_h34;
                    if (_source31.is_SelfOwned) {
                      return false;
                    } else if (_source31.is_U8) {
                      return false;
                    } else if (_source31.is_U16) {
                      return false;
                    } else if (_source31.is_U32) {
                      return false;
                    } else if (_source31.is_U64) {
                      return false;
                    } else if (_source31.is_U128) {
                      return false;
                    } else if (_source31.is_I8) {
                      return false;
                    } else if (_source31.is_I16) {
                      return false;
                    } else if (_source31.is_I32) {
                      return false;
                    } else if (_source31.is_I64) {
                      return false;
                    } else if (_source31.is_I128) {
                      return false;
                    } else if (_source31.is_Bool) {
                      return false;
                    } else if (_source31.is_TIdentifier) {
                      Dafny.ISequence<Dafny.Rune> _849___mcc_h38 = _source31.dtor_name;
                      return false;
                    } else if (_source31.is_TMemberSelect) {
                      RAST._IType _850___mcc_h40 = _source31.dtor_base;
                      Dafny.ISequence<Dafny.Rune> _851___mcc_h41 = _source31.dtor_name;
                      RAST._IType _source32 = _850___mcc_h40;
                      if (_source32.is_SelfOwned) {
                        return false;
                      } else if (_source32.is_U8) {
                        return false;
                      } else if (_source32.is_U16) {
                        return false;
                      } else if (_source32.is_U32) {
                        return false;
                      } else if (_source32.is_U64) {
                        return false;
                      } else if (_source32.is_U128) {
                        return false;
                      } else if (_source32.is_I8) {
                        return false;
                      } else if (_source32.is_I16) {
                        return false;
                      } else if (_source32.is_I32) {
                        return false;
                      } else if (_source32.is_I64) {
                        return false;
                      } else if (_source32.is_I128) {
                        return false;
                      } else if (_source32.is_Bool) {
                        return false;
                      } else if (_source32.is_TIdentifier) {
                        Dafny.ISequence<Dafny.Rune> _852___mcc_h44 = _source32.dtor_name;
                        return false;
                      } else if (_source32.is_TMemberSelect) {
                        RAST._IType _853___mcc_h46 = _source32.dtor_base;
                        Dafny.ISequence<Dafny.Rune> _854___mcc_h47 = _source32.dtor_name;
                        RAST._IType _source33 = _853___mcc_h46;
                        if (_source33.is_SelfOwned) {
                          return false;
                        } else if (_source33.is_U8) {
                          return false;
                        } else if (_source33.is_U16) {
                          return false;
                        } else if (_source33.is_U32) {
                          return false;
                        } else if (_source33.is_U64) {
                          return false;
                        } else if (_source33.is_U128) {
                          return false;
                        } else if (_source33.is_I8) {
                          return false;
                        } else if (_source33.is_I16) {
                          return false;
                        } else if (_source33.is_I32) {
                          return false;
                        } else if (_source33.is_I64) {
                          return false;
                        } else if (_source33.is_I128) {
                          return false;
                        } else if (_source33.is_Bool) {
                          return false;
                        } else if (_source33.is_TIdentifier) {
                          Dafny.ISequence<Dafny.Rune> _855___mcc_h50 = _source33.dtor_name;
                          if (object.Equals(_855___mcc_h50, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) {
                            if (object.Equals(_854___mcc_h47, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime"))) {
                              Dafny.ISequence<RAST._IType> _856_elems2 = _848___mcc_h35;
                              Dafny.ISequence<Dafny.Rune> _857_tpe2 = _851___mcc_h41;
                              Dafny.ISequence<RAST._IType> _858_elems1 = _836___mcc_h11;
                              Dafny.ISequence<Dafny.Rune> _859_tpe1 = _839___mcc_h17;
                              return ((_859_tpe1).Equals(_857_tpe2)) && ((((((_859_tpe1).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Set"))) || ((_859_tpe1).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Sequence")))) || ((_859_tpe1).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Multiset")))) || ((_859_tpe1).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Map")))) || ((_859_tpe1).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Object"))));
                            } else {
                              return false;
                            }
                          } else {
                            return false;
                          }
                        } else if (_source33.is_TMemberSelect) {
                          RAST._IType _860___mcc_h52 = _source33.dtor_base;
                          Dafny.ISequence<Dafny.Rune> _861___mcc_h53 = _source33.dtor_name;
                          return false;
                        } else if (_source33.is_TypeApp) {
                          RAST._IType _862___mcc_h56 = _source33.dtor_baseName;
                          Dafny.ISequence<RAST._IType> _863___mcc_h57 = _source33.dtor_arguments;
                          return false;
                        } else if (_source33.is_Borrowed) {
                          RAST._IType _864___mcc_h60 = _source33.dtor_underlying;
                          return false;
                        } else if (_source33.is_BorrowedMut) {
                          RAST._IType _865___mcc_h62 = _source33.dtor_underlying;
                          return false;
                        } else if (_source33.is_Pointer) {
                          RAST._IType _866___mcc_h64 = _source33.dtor_underlying;
                          return false;
                        } else if (_source33.is_PointerMut) {
                          RAST._IType _867___mcc_h66 = _source33.dtor_underlying;
                          return false;
                        } else if (_source33.is_ImplType) {
                          RAST._IType _868___mcc_h68 = _source33.dtor_underlying;
                          return false;
                        } else if (_source33.is_DynType) {
                          RAST._IType _869___mcc_h70 = _source33.dtor_underlying;
                          return false;
                        } else if (_source33.is_TupleType) {
                          Dafny.ISequence<RAST._IType> _870___mcc_h72 = _source33.dtor_arguments;
                          return false;
                        } else if (_source33.is_FnType) {
                          Dafny.ISequence<RAST._IType> _871___mcc_h74 = _source33.dtor_arguments;
                          RAST._IType _872___mcc_h75 = _source33.dtor_returnType;
                          return false;
                        } else if (_source33.is_IntersectionType) {
                          RAST._IType _873___mcc_h78 = _source33.dtor_left;
                          RAST._IType _874___mcc_h79 = _source33.dtor_right;
                          return false;
                        } else {
                          RAST._IType _875___mcc_h82 = _source33.dtor_underlying;
                          Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _876___mcc_h83 = _source33.dtor_size;
                          return false;
                        }
                      } else if (_source32.is_TypeApp) {
                        RAST._IType _877___mcc_h86 = _source32.dtor_baseName;
                        Dafny.ISequence<RAST._IType> _878___mcc_h87 = _source32.dtor_arguments;
                        return false;
                      } else if (_source32.is_Borrowed) {
                        RAST._IType _879___mcc_h90 = _source32.dtor_underlying;
                        return false;
                      } else if (_source32.is_BorrowedMut) {
                        RAST._IType _880___mcc_h92 = _source32.dtor_underlying;
                        return false;
                      } else if (_source32.is_Pointer) {
                        RAST._IType _881___mcc_h94 = _source32.dtor_underlying;
                        return false;
                      } else if (_source32.is_PointerMut) {
                        RAST._IType _882___mcc_h96 = _source32.dtor_underlying;
                        return false;
                      } else if (_source32.is_ImplType) {
                        RAST._IType _883___mcc_h98 = _source32.dtor_underlying;
                        return false;
                      } else if (_source32.is_DynType) {
                        RAST._IType _884___mcc_h100 = _source32.dtor_underlying;
                        return false;
                      } else if (_source32.is_TupleType) {
                        Dafny.ISequence<RAST._IType> _885___mcc_h102 = _source32.dtor_arguments;
                        return false;
                      } else if (_source32.is_FnType) {
                        Dafny.ISequence<RAST._IType> _886___mcc_h104 = _source32.dtor_arguments;
                        RAST._IType _887___mcc_h105 = _source32.dtor_returnType;
                        return false;
                      } else if (_source32.is_IntersectionType) {
                        RAST._IType _888___mcc_h108 = _source32.dtor_left;
                        RAST._IType _889___mcc_h109 = _source32.dtor_right;
                        return false;
                      } else {
                        RAST._IType _890___mcc_h112 = _source32.dtor_underlying;
                        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _891___mcc_h113 = _source32.dtor_size;
                        return false;
                      }
                    } else if (_source31.is_TypeApp) {
                      RAST._IType _892___mcc_h116 = _source31.dtor_baseName;
                      Dafny.ISequence<RAST._IType> _893___mcc_h117 = _source31.dtor_arguments;
                      return false;
                    } else if (_source31.is_Borrowed) {
                      RAST._IType _894___mcc_h120 = _source31.dtor_underlying;
                      return false;
                    } else if (_source31.is_BorrowedMut) {
                      RAST._IType _895___mcc_h122 = _source31.dtor_underlying;
                      return false;
                    } else if (_source31.is_Pointer) {
                      RAST._IType _896___mcc_h124 = _source31.dtor_underlying;
                      return false;
                    } else if (_source31.is_PointerMut) {
                      RAST._IType _897___mcc_h126 = _source31.dtor_underlying;
                      return false;
                    } else if (_source31.is_ImplType) {
                      RAST._IType _898___mcc_h128 = _source31.dtor_underlying;
                      return false;
                    } else if (_source31.is_DynType) {
                      RAST._IType _899___mcc_h130 = _source31.dtor_underlying;
                      return false;
                    } else if (_source31.is_TupleType) {
                      Dafny.ISequence<RAST._IType> _900___mcc_h132 = _source31.dtor_arguments;
                      return false;
                    } else if (_source31.is_FnType) {
                      Dafny.ISequence<RAST._IType> _901___mcc_h134 = _source31.dtor_arguments;
                      RAST._IType _902___mcc_h135 = _source31.dtor_returnType;
                      return false;
                    } else if (_source31.is_IntersectionType) {
                      RAST._IType _903___mcc_h138 = _source31.dtor_left;
                      RAST._IType _904___mcc_h139 = _source31.dtor_right;
                      return false;
                    } else {
                      RAST._IType _905___mcc_h142 = _source31.dtor_underlying;
                      Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _906___mcc_h143 = _source31.dtor_size;
                      return false;
                    }
                  } else if (_source30.is_Borrowed) {
                    RAST._IType _907___mcc_h146 = _source30.dtor_underlying;
                    return false;
                  } else if (_source30.is_BorrowedMut) {
                    RAST._IType _908___mcc_h148 = _source30.dtor_underlying;
                    return false;
                  } else if (_source30.is_Pointer) {
                    RAST._IType _909___mcc_h150 = _source30.dtor_underlying;
                    return false;
                  } else if (_source30.is_PointerMut) {
                    RAST._IType _910___mcc_h152 = _source30.dtor_underlying;
                    return false;
                  } else if (_source30.is_ImplType) {
                    RAST._IType _911___mcc_h154 = _source30.dtor_underlying;
                    return false;
                  } else if (_source30.is_DynType) {
                    RAST._IType _912___mcc_h156 = _source30.dtor_underlying;
                    return false;
                  } else if (_source30.is_TupleType) {
                    Dafny.ISequence<RAST._IType> _913___mcc_h158 = _source30.dtor_arguments;
                    return false;
                  } else if (_source30.is_FnType) {
                    Dafny.ISequence<RAST._IType> _914___mcc_h160 = _source30.dtor_arguments;
                    RAST._IType _915___mcc_h161 = _source30.dtor_returnType;
                    return false;
                  } else if (_source30.is_IntersectionType) {
                    RAST._IType _916___mcc_h164 = _source30.dtor_left;
                    RAST._IType _917___mcc_h165 = _source30.dtor_right;
                    return false;
                  } else {
                    RAST._IType _918___mcc_h168 = _source30.dtor_underlying;
                    Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _919___mcc_h169 = _source30.dtor_size;
                    return false;
                  }
                } else {
                  return false;
                }
              } else {
                return false;
              }
            } else if (_source29.is_TMemberSelect) {
              RAST._IType _920___mcc_h172 = _source29.dtor_base;
              Dafny.ISequence<Dafny.Rune> _921___mcc_h173 = _source29.dtor_name;
              return false;
            } else if (_source29.is_TypeApp) {
              RAST._IType _922___mcc_h176 = _source29.dtor_baseName;
              Dafny.ISequence<RAST._IType> _923___mcc_h177 = _source29.dtor_arguments;
              return false;
            } else if (_source29.is_Borrowed) {
              RAST._IType _924___mcc_h180 = _source29.dtor_underlying;
              return false;
            } else if (_source29.is_BorrowedMut) {
              RAST._IType _925___mcc_h182 = _source29.dtor_underlying;
              return false;
            } else if (_source29.is_Pointer) {
              RAST._IType _926___mcc_h184 = _source29.dtor_underlying;
              return false;
            } else if (_source29.is_PointerMut) {
              RAST._IType _927___mcc_h186 = _source29.dtor_underlying;
              return false;
            } else if (_source29.is_ImplType) {
              RAST._IType _928___mcc_h188 = _source29.dtor_underlying;
              return false;
            } else if (_source29.is_DynType) {
              RAST._IType _929___mcc_h190 = _source29.dtor_underlying;
              return false;
            } else if (_source29.is_TupleType) {
              Dafny.ISequence<RAST._IType> _930___mcc_h192 = _source29.dtor_arguments;
              return false;
            } else if (_source29.is_FnType) {
              Dafny.ISequence<RAST._IType> _931___mcc_h194 = _source29.dtor_arguments;
              RAST._IType _932___mcc_h195 = _source29.dtor_returnType;
              return false;
            } else if (_source29.is_IntersectionType) {
              RAST._IType _933___mcc_h198 = _source29.dtor_left;
              RAST._IType _934___mcc_h199 = _source29.dtor_right;
              return false;
            } else {
              RAST._IType _935___mcc_h202 = _source29.dtor_underlying;
              Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _936___mcc_h203 = _source29.dtor_size;
              return false;
            }
          } else if (_source28.is_TypeApp) {
            RAST._IType _937___mcc_h206 = _source28.dtor_baseName;
            Dafny.ISequence<RAST._IType> _938___mcc_h207 = _source28.dtor_arguments;
            return false;
          } else if (_source28.is_Borrowed) {
            RAST._IType _939___mcc_h210 = _source28.dtor_underlying;
            return false;
          } else if (_source28.is_BorrowedMut) {
            RAST._IType _940___mcc_h212 = _source28.dtor_underlying;
            return false;
          } else if (_source28.is_Pointer) {
            RAST._IType _941___mcc_h214 = _source28.dtor_underlying;
            return false;
          } else if (_source28.is_PointerMut) {
            RAST._IType _942___mcc_h216 = _source28.dtor_underlying;
            return false;
          } else if (_source28.is_ImplType) {
            RAST._IType _943___mcc_h218 = _source28.dtor_underlying;
            return false;
          } else if (_source28.is_DynType) {
            RAST._IType _944___mcc_h220 = _source28.dtor_underlying;
            return false;
          } else if (_source28.is_TupleType) {
            Dafny.ISequence<RAST._IType> _945___mcc_h222 = _source28.dtor_arguments;
            return false;
          } else if (_source28.is_FnType) {
            Dafny.ISequence<RAST._IType> _946___mcc_h224 = _source28.dtor_arguments;
            RAST._IType _947___mcc_h225 = _source28.dtor_returnType;
            return false;
          } else if (_source28.is_IntersectionType) {
            RAST._IType _948___mcc_h228 = _source28.dtor_left;
            RAST._IType _949___mcc_h229 = _source28.dtor_right;
            return false;
          } else {
            RAST._IType _950___mcc_h232 = _source28.dtor_underlying;
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _951___mcc_h233 = _source28.dtor_size;
            return false;
          }
        } else if (_source27.is_TypeApp) {
          RAST._IType _952___mcc_h236 = _source27.dtor_baseName;
          Dafny.ISequence<RAST._IType> _953___mcc_h237 = _source27.dtor_arguments;
          return false;
        } else if (_source27.is_Borrowed) {
          RAST._IType _954___mcc_h240 = _source27.dtor_underlying;
          return false;
        } else if (_source27.is_BorrowedMut) {
          RAST._IType _955___mcc_h242 = _source27.dtor_underlying;
          return false;
        } else if (_source27.is_Pointer) {
          RAST._IType _956___mcc_h244 = _source27.dtor_underlying;
          return false;
        } else if (_source27.is_PointerMut) {
          RAST._IType _957___mcc_h246 = _source27.dtor_underlying;
          return false;
        } else if (_source27.is_ImplType) {
          RAST._IType _958___mcc_h248 = _source27.dtor_underlying;
          return false;
        } else if (_source27.is_DynType) {
          RAST._IType _959___mcc_h250 = _source27.dtor_underlying;
          return false;
        } else if (_source27.is_TupleType) {
          Dafny.ISequence<RAST._IType> _960___mcc_h252 = _source27.dtor_arguments;
          return false;
        } else if (_source27.is_FnType) {
          Dafny.ISequence<RAST._IType> _961___mcc_h254 = _source27.dtor_arguments;
          RAST._IType _962___mcc_h255 = _source27.dtor_returnType;
          return false;
        } else if (_source27.is_IntersectionType) {
          RAST._IType _963___mcc_h258 = _source27.dtor_left;
          RAST._IType _964___mcc_h259 = _source27.dtor_right;
          return false;
        } else {
          RAST._IType _965___mcc_h262 = _source27.dtor_underlying;
          Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _966___mcc_h263 = _source27.dtor_size;
          return false;
        }
      } else if (_source26.is_Borrowed) {
        RAST._IType _967___mcc_h266 = _source26.dtor_underlying;
        return false;
      } else if (_source26.is_BorrowedMut) {
        RAST._IType _968___mcc_h268 = _source26.dtor_underlying;
        return false;
      } else if (_source26.is_Pointer) {
        RAST._IType _969___mcc_h270 = _source26.dtor_underlying;
        return false;
      } else if (_source26.is_PointerMut) {
        RAST._IType _970___mcc_h272 = _source26.dtor_underlying;
        return false;
      } else if (_source26.is_ImplType) {
        RAST._IType _971___mcc_h274 = _source26.dtor_underlying;
        return false;
      } else if (_source26.is_DynType) {
        RAST._IType _972___mcc_h276 = _source26.dtor_underlying;
        return false;
      } else if (_source26.is_TupleType) {
        Dafny.ISequence<RAST._IType> _973___mcc_h278 = _source26.dtor_arguments;
        return false;
      } else if (_source26.is_FnType) {
        Dafny.ISequence<RAST._IType> _974___mcc_h280 = _source26.dtor_arguments;
        RAST._IType _975___mcc_h281 = _source26.dtor_returnType;
        return false;
      } else if (_source26.is_IntersectionType) {
        RAST._IType _976___mcc_h284 = _source26.dtor_left;
        RAST._IType _977___mcc_h285 = _source26.dtor_right;
        return false;
      } else {
        RAST._IType _978___mcc_h288 = _source26.dtor_underlying;
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _979___mcc_h289 = _source26.dtor_size;
        return false;
      }
    }
    public static RAST._IType MaybeUninitType(RAST._IType underlying) {
      return (RAST.__default.MaybeUninitPath).Apply(Dafny.Sequence<RAST._IType>.FromElements(underlying));
    }
    public static RAST._IExpr MaybeUninitNew(RAST._IExpr underlying) {
      return ((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("mem"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("MaybeUninit"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(underlying));
    }
    public static RAST._IType MaybePlaceboType(RAST._IType underlying) {
      return ((RAST.__default.dafny__runtime__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("MaybePlacebo"))).Apply1(underlying);
    }
    public static Dafny.ISequence<Dafny.Rune> AddIndent(Dafny.ISequence<Dafny.Rune> raw, Dafny.ISequence<Dafny.Rune> ind)
    {
      Dafny.ISequence<Dafny.Rune> _980___accumulator = Dafny.Sequence<Dafny.Rune>.FromElements();
    TAIL_CALL_START: ;
      if ((new BigInteger((raw).Count)).Sign == 0) {
        return Dafny.Sequence<Dafny.Rune>.Concat(_980___accumulator, raw);
      } else if ((Dafny.Sequence<Dafny.Rune>.UnicodeFromString("[({")).Contains((raw).Select(BigInteger.Zero))) {
        _980___accumulator = Dafny.Sequence<Dafny.Rune>.Concat(_980___accumulator, Dafny.Sequence<Dafny.Rune>.FromElements((raw).Select(BigInteger.Zero)));
        Dafny.ISequence<Dafny.Rune> _in103 = (raw).Drop(BigInteger.One);
        Dafny.ISequence<Dafny.Rune> _in104 = Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND);
        raw = _in103;
        ind = _in104;
        goto TAIL_CALL_START;
      } else if (((Dafny.Sequence<Dafny.Rune>.UnicodeFromString("})]")).Contains((raw).Select(BigInteger.Zero))) && ((new BigInteger((ind).Count)) > (new BigInteger(2)))) {
        _980___accumulator = Dafny.Sequence<Dafny.Rune>.Concat(_980___accumulator, Dafny.Sequence<Dafny.Rune>.FromElements((raw).Select(BigInteger.Zero)));
        Dafny.ISequence<Dafny.Rune> _in105 = (raw).Drop(BigInteger.One);
        Dafny.ISequence<Dafny.Rune> _in106 = (ind).Take((new BigInteger((ind).Count)) - (new BigInteger(2)));
        raw = _in105;
        ind = _in106;
        goto TAIL_CALL_START;
      } else if (((raw).Select(BigInteger.Zero)) == (new Dafny.Rune('\n'))) {
        _980___accumulator = Dafny.Sequence<Dafny.Rune>.Concat(_980___accumulator, Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.FromElements((raw).Select(BigInteger.Zero)), ind));
        Dafny.ISequence<Dafny.Rune> _in107 = (raw).Drop(BigInteger.One);
        Dafny.ISequence<Dafny.Rune> _in108 = ind;
        raw = _in107;
        ind = _in108;
        goto TAIL_CALL_START;
      } else {
        _980___accumulator = Dafny.Sequence<Dafny.Rune>.Concat(_980___accumulator, Dafny.Sequence<Dafny.Rune>.FromElements((raw).Select(BigInteger.Zero)));
        Dafny.ISequence<Dafny.Rune> _in109 = (raw).Drop(BigInteger.One);
        Dafny.ISequence<Dafny.Rune> _in110 = ind;
        raw = _in109;
        ind = _in110;
        goto TAIL_CALL_START;
      }
    }
    public static BigInteger max(BigInteger i, BigInteger j)
    {
      if ((i) < (j)) {
        return j;
      } else {
        return i;
      }
    }
    public static RAST._IExpr AssignVar(Dafny.ISequence<Dafny.Rune> name, RAST._IExpr rhs)
    {
      return RAST.Expr.create_Assign(Std.Wrappers.Option<RAST._IAssignLhs>.create_Some(RAST.AssignLhs.create_LocalVar(name)), rhs);
    }
    public static RAST._IExpr AssignMember(RAST._IExpr @on, Dafny.ISequence<Dafny.Rune> field, RAST._IExpr rhs)
    {
      return RAST.Expr.create_Assign(Std.Wrappers.Option<RAST._IAssignLhs>.create_Some(RAST.AssignLhs.create_SelectMember(@on, field)), rhs);
    }
    public static RAST._IExpr MaybePlacebo(RAST._IExpr underlying) {
      return (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("MaybePlacebo"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from"))).Apply1(underlying);
    }
    public static RAST._IExpr RcNew(RAST._IExpr underlying) {
      return RAST.Expr.create_Call(RAST.__default.std__rc__Rc__new, Dafny.Sequence<RAST._IExpr>.FromElements(underlying));
    }
    public static RAST._IType SelfBorrowed { get {
      return RAST.Type.create_Borrowed(RAST.Type.create_SelfOwned());
    } }
    public static RAST._IType SelfBorrowedMut { get {
      return RAST.Type.create_BorrowedMut(RAST.Type.create_SelfOwned());
    } }
    public static RAST._IType SelfPointer { get {
      return RAST.Type.create_Pointer(RAST.Type.create_SelfOwned());
    } }
    public static RAST._IType SelfPointerMut { get {
      return RAST.Type.create_PointerMut(RAST.Type.create_SelfOwned());
    } }
    public static RAST._IType global__type { get {
      return RAST.Type.create_TIdentifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
    } }
    public static RAST._IType std__type { get {
      return (RAST.__default.global__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("std"));
    } }
    public static RAST._IType RcType { get {
      return ((RAST.__default.std__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("rc"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Rc"));
    } }
    public static RAST._IType dafny__runtime__type { get {
      return (RAST.__default.global__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime"));
    } }
    public static RAST._IType super__type { get {
      return RAST.Type.create_TIdentifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("super"));
    } }
    public static RAST._IType cell__type { get {
      return (RAST.__default.std__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cell"));
    } }
    public static RAST._IType refcell__type { get {
      return (RAST.__default.cell__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("RefCell"));
    } }
    public static RAST._IType CloneTrait { get {
      return RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Clone"));
    } }
    public static RAST._IType DefaultTrait { get {
      return ((RAST.__default.std__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("default"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Default"));
    } }
    public static RAST._IType StaticTrait { get {
      return RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("'static"));
    } }
    public static RAST._IType DafnyType { get {
      return (RAST.__default.dafny__runtime__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyType"));
    } }
    public static RAST._IType DafnyPrint { get {
      return (RAST.__default.dafny__runtime__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyPrint"));
    } }
    public static RAST._IType DafnyTypeEq { get {
      return (RAST.__default.dafny__runtime__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyTypeEq"));
    } }
    public static RAST._IType Eq { get {
      return RAST.Type.create_TIdentifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Eq"));
    } }
    public static RAST._IType Hash { get {
      return ((RAST.__default.std__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("hash"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Hash"));
    } }
    public static RAST._IType DafnyInt { get {
      return (RAST.__default.dafny__runtime__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyInt"));
    } }
    public static RAST._IType MaybeUninitPath { get {
      return ((RAST.__default.std__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("mem"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("MaybeUninit"));
    } }
    public static RAST._IExpr @global { get {
      return RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
    } }
    public static RAST._IExpr std { get {
      return (RAST.__default.@global).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("std"));
    } }
    public static Dafny.ISequence<Dafny.Rune> IND { get {
      return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("  ");
    } }
    public static RAST._IExpr self { get {
      return RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"));
    } }
    public static RAST._IExpr dafny__runtime { get {
      return (RAST.__default.@global).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime"));
    } }
    public static RAST._IExpr dafny__runtime__Set { get {
      return (RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Set"));
    } }
    public static RAST._IExpr dafny__runtime__Set__from__array { get {
      return (RAST.__default.dafny__runtime__Set).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_array"));
    } }
    public static RAST._IExpr dafny__runtime__Sequence { get {
      return (RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Sequence"));
    } }
    public static RAST._IExpr Sequence__from__array__owned { get {
      return (RAST.__default.dafny__runtime__Sequence).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_array_owned"));
    } }
    public static RAST._IExpr Sequence__from__array { get {
      return (RAST.__default.dafny__runtime__Sequence).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_array"));
    } }
    public static RAST._IExpr dafny__runtime__Multiset { get {
      return (RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Multiset"));
    } }
    public static RAST._IExpr dafny__runtime__Multiset__from__array { get {
      return (RAST.__default.dafny__runtime__Multiset).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_array"));
    } }
    public static RAST._IExpr std__rc { get {
      return (RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("rc"));
    } }
    public static RAST._IExpr std__rc__Rc { get {
      return (RAST.__default.std__rc).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Rc"));
    } }
    public static RAST._IExpr std__rc__Rc__new { get {
      return (RAST.__default.std__rc__Rc).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"));
    } }
    public static RAST._IExpr std__Default__default { get {
      return ((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("default"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Default"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("default"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
    } }
  }

  public interface _IMod {
    bool is_Mod { get; }
    bool is_ExternMod { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    Dafny.ISequence<RAST._IModDecl> dtor_body { get; }
    _IMod DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public abstract class Mod : _IMod {
    public Mod() {
    }
    private static readonly RAST._IMod theDefault = create_Mod(Dafny.Sequence<Dafny.Rune>.Empty, Dafny.Sequence<RAST._IModDecl>.Empty);
    public static RAST._IMod Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IMod> _TYPE = new Dafny.TypeDescriptor<RAST._IMod>(RAST.Mod.Default());
    public static Dafny.TypeDescriptor<RAST._IMod> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IMod create_Mod(Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._IModDecl> body) {
      return new Mod_Mod(name, body);
    }
    public static _IMod create_ExternMod(Dafny.ISequence<Dafny.Rune> name) {
      return new Mod_ExternMod(name);
    }
    public bool is_Mod { get { return this is Mod_Mod; } }
    public bool is_ExternMod { get { return this is Mod_ExternMod; } }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        var d = this;
        if (d is Mod_Mod) { return ((Mod_Mod)d)._i_name; }
        return ((Mod_ExternMod)d)._i_name;
      }
    }
    public Dafny.ISequence<RAST._IModDecl> dtor_body {
      get {
        var d = this;
        return ((Mod_Mod)d)._i_body;
      }
    }
    public abstract _IMod DowncastClone();
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      RAST._IMod _source34 = this;
      if (_source34.is_Mod) {
        Dafny.ISequence<Dafny.Rune> _981___mcc_h0 = _source34.dtor_name;
        Dafny.ISequence<RAST._IModDecl> _982___mcc_h1 = _source34.dtor_body;
        Dafny.ISequence<RAST._IModDecl> _983_body = _982___mcc_h1;
        Dafny.ISequence<Dafny.Rune> _984_name = _981___mcc_h0;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("pub mod "), _984_name), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), ind), RAST.__default.IND), RAST.__default.SeqToString<RAST._IModDecl>(_983_body, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IModDecl, Dafny.ISequence<Dafny.Rune>>>>((_985_ind) => ((System.Func<RAST._IModDecl, Dafny.ISequence<Dafny.Rune>>)((_986_modDecl) => {
          return (_986_modDecl)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_985_ind, RAST.__default.IND));
        })))(ind), Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n\n"), ind), RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
      } else {
        Dafny.ISequence<Dafny.Rune> _987___mcc_h2 = _source34.dtor_name;
        Dafny.ISequence<Dafny.Rune> _988_name = _987___mcc_h2;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("pub mod "), _988_name), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";"));
      }
    }
  }
  public class Mod_Mod : Mod {
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public readonly Dafny.ISequence<RAST._IModDecl> _i_body;
    public Mod_Mod(Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._IModDecl> body) : base() {
      this._i_name = name;
      this._i_body = body;
    }
    public override _IMod DowncastClone() {
      if (this is _IMod dt) { return dt; }
      return new Mod_Mod(_i_name, _i_body);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Mod_Mod;
      return oth != null && object.Equals(this._i_name, oth._i_name) && object.Equals(this._i_body, oth._i_body);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_body));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Mod.Mod";
      s += "(";
      s += this._i_name.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_body);
      s += ")";
      return s;
    }
  }
  public class Mod_ExternMod : Mod {
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public Mod_ExternMod(Dafny.ISequence<Dafny.Rune> name) : base() {
      this._i_name = name;
    }
    public override _IMod DowncastClone() {
      if (this is _IMod dt) { return dt; }
      return new Mod_ExternMod(_i_name);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Mod_ExternMod;
      return oth != null && object.Equals(this._i_name, oth._i_name);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Mod.ExternMod";
      s += "(";
      s += this._i_name.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }

  public interface _IModDecl {
    bool is_RawDecl { get; }
    bool is_ModDecl { get; }
    bool is_StructDecl { get; }
    bool is_TypeDecl { get; }
    bool is_ConstDecl { get; }
    bool is_EnumDecl { get; }
    bool is_ImplDecl { get; }
    bool is_TraitDecl { get; }
    bool is_TopFnDecl { get; }
    Dafny.ISequence<Dafny.Rune> dtor_body { get; }
    RAST._IMod dtor_mod { get; }
    RAST._IStruct dtor_struct { get; }
    RAST._ITypeSynonym dtor_tpe { get; }
    RAST._IConstant dtor_c { get; }
    RAST._IEnum dtor_enum { get; }
    RAST._IImpl dtor_impl { get; }
    RAST._ITrait dtor_tr { get; }
    RAST._ITopFnDecl dtor_fn { get; }
    _IModDecl DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public abstract class ModDecl : _IModDecl {
    public ModDecl() {
    }
    private static readonly RAST._IModDecl theDefault = create_RawDecl(Dafny.Sequence<Dafny.Rune>.Empty);
    public static RAST._IModDecl Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IModDecl> _TYPE = new Dafny.TypeDescriptor<RAST._IModDecl>(RAST.ModDecl.Default());
    public static Dafny.TypeDescriptor<RAST._IModDecl> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IModDecl create_RawDecl(Dafny.ISequence<Dafny.Rune> body) {
      return new ModDecl_RawDecl(body);
    }
    public static _IModDecl create_ModDecl(RAST._IMod mod) {
      return new ModDecl_ModDecl(mod);
    }
    public static _IModDecl create_StructDecl(RAST._IStruct @struct) {
      return new ModDecl_StructDecl(@struct);
    }
    public static _IModDecl create_TypeDecl(RAST._ITypeSynonym tpe) {
      return new ModDecl_TypeDecl(tpe);
    }
    public static _IModDecl create_ConstDecl(RAST._IConstant c) {
      return new ModDecl_ConstDecl(c);
    }
    public static _IModDecl create_EnumDecl(RAST._IEnum @enum) {
      return new ModDecl_EnumDecl(@enum);
    }
    public static _IModDecl create_ImplDecl(RAST._IImpl impl) {
      return new ModDecl_ImplDecl(impl);
    }
    public static _IModDecl create_TraitDecl(RAST._ITrait tr) {
      return new ModDecl_TraitDecl(tr);
    }
    public static _IModDecl create_TopFnDecl(RAST._ITopFnDecl fn) {
      return new ModDecl_TopFnDecl(fn);
    }
    public bool is_RawDecl { get { return this is ModDecl_RawDecl; } }
    public bool is_ModDecl { get { return this is ModDecl_ModDecl; } }
    public bool is_StructDecl { get { return this is ModDecl_StructDecl; } }
    public bool is_TypeDecl { get { return this is ModDecl_TypeDecl; } }
    public bool is_ConstDecl { get { return this is ModDecl_ConstDecl; } }
    public bool is_EnumDecl { get { return this is ModDecl_EnumDecl; } }
    public bool is_ImplDecl { get { return this is ModDecl_ImplDecl; } }
    public bool is_TraitDecl { get { return this is ModDecl_TraitDecl; } }
    public bool is_TopFnDecl { get { return this is ModDecl_TopFnDecl; } }
    public Dafny.ISequence<Dafny.Rune> dtor_body {
      get {
        var d = this;
        return ((ModDecl_RawDecl)d)._i_body;
      }
    }
    public RAST._IMod dtor_mod {
      get {
        var d = this;
        return ((ModDecl_ModDecl)d)._i_mod;
      }
    }
    public RAST._IStruct dtor_struct {
      get {
        var d = this;
        return ((ModDecl_StructDecl)d)._i_struct;
      }
    }
    public RAST._ITypeSynonym dtor_tpe {
      get {
        var d = this;
        return ((ModDecl_TypeDecl)d)._i_tpe;
      }
    }
    public RAST._IConstant dtor_c {
      get {
        var d = this;
        return ((ModDecl_ConstDecl)d)._i_c;
      }
    }
    public RAST._IEnum dtor_enum {
      get {
        var d = this;
        return ((ModDecl_EnumDecl)d)._i_enum;
      }
    }
    public RAST._IImpl dtor_impl {
      get {
        var d = this;
        return ((ModDecl_ImplDecl)d)._i_impl;
      }
    }
    public RAST._ITrait dtor_tr {
      get {
        var d = this;
        return ((ModDecl_TraitDecl)d)._i_tr;
      }
    }
    public RAST._ITopFnDecl dtor_fn {
      get {
        var d = this;
        return ((ModDecl_TopFnDecl)d)._i_fn;
      }
    }
    public abstract _IModDecl DowncastClone();
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      if ((this).is_ModDecl) {
        return ((this).dtor_mod)._ToString(ind);
      } else if ((this).is_StructDecl) {
        return ((this).dtor_struct)._ToString(ind);
      } else if ((this).is_ImplDecl) {
        return ((this).dtor_impl)._ToString(ind);
      } else if ((this).is_EnumDecl) {
        return ((this).dtor_enum)._ToString(ind);
      } else if ((this).is_TypeDecl) {
        return ((this).dtor_tpe)._ToString(ind);
      } else if ((this).is_ConstDecl) {
        return ((this).dtor_c)._ToString(ind);
      } else if ((this).is_TraitDecl) {
        return ((this).dtor_tr)._ToString(ind);
      } else if ((this).is_TopFnDecl) {
        return ((this).dtor_fn)._ToString(ind);
      } else {
        return (this).dtor_body;
      }
    }
  }
  public class ModDecl_RawDecl : ModDecl {
    public readonly Dafny.ISequence<Dafny.Rune> _i_body;
    public ModDecl_RawDecl(Dafny.ISequence<Dafny.Rune> body) : base() {
      this._i_body = body;
    }
    public override _IModDecl DowncastClone() {
      if (this is _IModDecl dt) { return dt; }
      return new ModDecl_RawDecl(_i_body);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ModDecl_RawDecl;
      return oth != null && object.Equals(this._i_body, oth._i_body);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_body));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ModDecl.RawDecl";
      s += "(";
      s += this._i_body.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class ModDecl_ModDecl : ModDecl {
    public readonly RAST._IMod _i_mod;
    public ModDecl_ModDecl(RAST._IMod mod) : base() {
      this._i_mod = mod;
    }
    public override _IModDecl DowncastClone() {
      if (this is _IModDecl dt) { return dt; }
      return new ModDecl_ModDecl(_i_mod);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ModDecl_ModDecl;
      return oth != null && object.Equals(this._i_mod, oth._i_mod);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_mod));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ModDecl.ModDecl";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_mod);
      s += ")";
      return s;
    }
  }
  public class ModDecl_StructDecl : ModDecl {
    public readonly RAST._IStruct _i_struct;
    public ModDecl_StructDecl(RAST._IStruct @struct) : base() {
      this._i_struct = @struct;
    }
    public override _IModDecl DowncastClone() {
      if (this is _IModDecl dt) { return dt; }
      return new ModDecl_StructDecl(_i_struct);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ModDecl_StructDecl;
      return oth != null && object.Equals(this._i_struct, oth._i_struct);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 2;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_struct));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ModDecl.StructDecl";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_struct);
      s += ")";
      return s;
    }
  }
  public class ModDecl_TypeDecl : ModDecl {
    public readonly RAST._ITypeSynonym _i_tpe;
    public ModDecl_TypeDecl(RAST._ITypeSynonym tpe) : base() {
      this._i_tpe = tpe;
    }
    public override _IModDecl DowncastClone() {
      if (this is _IModDecl dt) { return dt; }
      return new ModDecl_TypeDecl(_i_tpe);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ModDecl_TypeDecl;
      return oth != null && object.Equals(this._i_tpe, oth._i_tpe);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 3;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tpe));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ModDecl.TypeDecl";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_tpe);
      s += ")";
      return s;
    }
  }
  public class ModDecl_ConstDecl : ModDecl {
    public readonly RAST._IConstant _i_c;
    public ModDecl_ConstDecl(RAST._IConstant c) : base() {
      this._i_c = c;
    }
    public override _IModDecl DowncastClone() {
      if (this is _IModDecl dt) { return dt; }
      return new ModDecl_ConstDecl(_i_c);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ModDecl_ConstDecl;
      return oth != null && object.Equals(this._i_c, oth._i_c);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 4;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_c));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ModDecl.ConstDecl";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_c);
      s += ")";
      return s;
    }
  }
  public class ModDecl_EnumDecl : ModDecl {
    public readonly RAST._IEnum _i_enum;
    public ModDecl_EnumDecl(RAST._IEnum @enum) : base() {
      this._i_enum = @enum;
    }
    public override _IModDecl DowncastClone() {
      if (this is _IModDecl dt) { return dt; }
      return new ModDecl_EnumDecl(_i_enum);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ModDecl_EnumDecl;
      return oth != null && object.Equals(this._i_enum, oth._i_enum);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 5;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_enum));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ModDecl.EnumDecl";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_enum);
      s += ")";
      return s;
    }
  }
  public class ModDecl_ImplDecl : ModDecl {
    public readonly RAST._IImpl _i_impl;
    public ModDecl_ImplDecl(RAST._IImpl impl) : base() {
      this._i_impl = impl;
    }
    public override _IModDecl DowncastClone() {
      if (this is _IModDecl dt) { return dt; }
      return new ModDecl_ImplDecl(_i_impl);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ModDecl_ImplDecl;
      return oth != null && object.Equals(this._i_impl, oth._i_impl);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 6;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_impl));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ModDecl.ImplDecl";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_impl);
      s += ")";
      return s;
    }
  }
  public class ModDecl_TraitDecl : ModDecl {
    public readonly RAST._ITrait _i_tr;
    public ModDecl_TraitDecl(RAST._ITrait tr) : base() {
      this._i_tr = tr;
    }
    public override _IModDecl DowncastClone() {
      if (this is _IModDecl dt) { return dt; }
      return new ModDecl_TraitDecl(_i_tr);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ModDecl_TraitDecl;
      return oth != null && object.Equals(this._i_tr, oth._i_tr);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 7;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tr));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ModDecl.TraitDecl";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_tr);
      s += ")";
      return s;
    }
  }
  public class ModDecl_TopFnDecl : ModDecl {
    public readonly RAST._ITopFnDecl _i_fn;
    public ModDecl_TopFnDecl(RAST._ITopFnDecl fn) : base() {
      this._i_fn = fn;
    }
    public override _IModDecl DowncastClone() {
      if (this is _IModDecl dt) { return dt; }
      return new ModDecl_TopFnDecl(_i_fn);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ModDecl_TopFnDecl;
      return oth != null && object.Equals(this._i_fn, oth._i_fn);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 8;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_fn));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ModDecl.TopFnDecl";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_fn);
      s += ")";
      return s;
    }
  }

  public interface _ITopFnDecl {
    bool is_TopFn { get; }
    Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_attributes { get; }
    RAST._IVisibility dtor_visibility { get; }
    RAST._IFn dtor_fn { get; }
    _ITopFnDecl DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class TopFnDecl : _ITopFnDecl {
    public readonly Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _i_attributes;
    public readonly RAST._IVisibility _i_visibility;
    public readonly RAST._IFn _i_fn;
    public TopFnDecl(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, RAST._IVisibility visibility, RAST._IFn fn) {
      this._i_attributes = attributes;
      this._i_visibility = visibility;
      this._i_fn = fn;
    }
    public _ITopFnDecl DowncastClone() {
      if (this is _ITopFnDecl dt) { return dt; }
      return new TopFnDecl(_i_attributes, _i_visibility, _i_fn);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.TopFnDecl;
      return oth != null && object.Equals(this._i_attributes, oth._i_attributes) && object.Equals(this._i_visibility, oth._i_visibility) && object.Equals(this._i_fn, oth._i_fn);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_attributes));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_visibility));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_fn));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.TopFnDecl.TopFn";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_attributes);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_visibility);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_fn);
      s += ")";
      return s;
    }
    private static readonly RAST._ITopFnDecl theDefault = create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Empty, RAST.Visibility.Default(), RAST.Fn.Default());
    public static RAST._ITopFnDecl Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._ITopFnDecl> _TYPE = new Dafny.TypeDescriptor<RAST._ITopFnDecl>(RAST.TopFnDecl.Default());
    public static Dafny.TypeDescriptor<RAST._ITopFnDecl> _TypeDescriptor() {
      return _TYPE;
    }
    public static _ITopFnDecl create(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, RAST._IVisibility visibility, RAST._IFn fn) {
      return new TopFnDecl(attributes, visibility, fn);
    }
    public static _ITopFnDecl create_TopFn(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, RAST._IVisibility visibility, RAST._IFn fn) {
      return create(attributes, visibility, fn);
    }
    public bool is_TopFn { get { return true; } }
    public Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_attributes {
      get {
        return this._i_attributes;
      }
    }
    public RAST._IVisibility dtor_visibility {
      get {
        return this._i_visibility;
      }
    }
    public RAST._IFn dtor_fn {
      get {
        return this._i_fn;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(RAST.Attribute.ToStringMultiple((this).dtor_attributes, ind), ((this).dtor_visibility)._ToString()), ((this).dtor_fn)._ToString(ind));
    }
  }

  public interface _IAttribute {
    bool is_RawAttribute { get; }
    Dafny.ISequence<Dafny.Rune> dtor_content { get; }
  }
  public class Attribute : _IAttribute {
    public readonly Dafny.ISequence<Dafny.Rune> _i_content;
    public Attribute(Dafny.ISequence<Dafny.Rune> content) {
      this._i_content = content;
    }
    public static Dafny.ISequence<Dafny.Rune> DowncastClone(Dafny.ISequence<Dafny.Rune> _this) {
      return _this;
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Attribute;
      return oth != null && object.Equals(this._i_content, oth._i_content);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_content));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Attribute.RawAttribute";
      s += "(";
      s += this._i_content.ToVerbatimString(true);
      s += ")";
      return s;
    }
    private static readonly Dafny.ISequence<Dafny.Rune> theDefault = Dafny.Sequence<Dafny.Rune>.Empty;
    public static Dafny.ISequence<Dafny.Rune> Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<Dafny.ISequence<Dafny.Rune>> _TYPE = new Dafny.TypeDescriptor<Dafny.ISequence<Dafny.Rune>>(Dafny.Sequence<Dafny.Rune>.Empty);
    public static Dafny.TypeDescriptor<Dafny.ISequence<Dafny.Rune>> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IAttribute create(Dafny.ISequence<Dafny.Rune> content) {
      return new Attribute(content);
    }
    public static _IAttribute create_RawAttribute(Dafny.ISequence<Dafny.Rune> content) {
      return create(content);
    }
    public bool is_RawAttribute { get { return true; } }
    public Dafny.ISequence<Dafny.Rune> dtor_content {
      get {
        return this._i_content;
      }
    }
    public static Dafny.ISequence<Dafny.Rune> ToStringMultiple(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> ind)
    {
      return RAST.__default.SeqToString<Dafny.ISequence<Dafny.Rune>>(attributes, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>>>((_989_ind) => ((System.Func<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>)((_990_attribute) => {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat((_990_attribute), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), _989_ind);
      })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
    }
  }

  public interface _IStruct {
    bool is_Struct { get; }
    Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_attributes { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams { get; }
    RAST._IFields dtor_fields { get; }
    _IStruct DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class Struct : _IStruct {
    public readonly Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _i_attributes;
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public readonly Dafny.ISequence<RAST._ITypeParamDecl> _i_typeParams;
    public readonly RAST._IFields _i_fields;
    public Struct(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IFields fields) {
      this._i_attributes = attributes;
      this._i_name = name;
      this._i_typeParams = typeParams;
      this._i_fields = fields;
    }
    public _IStruct DowncastClone() {
      if (this is _IStruct dt) { return dt; }
      return new Struct(_i_attributes, _i_name, _i_typeParams, _i_fields);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Struct;
      return oth != null && object.Equals(this._i_attributes, oth._i_attributes) && object.Equals(this._i_name, oth._i_name) && object.Equals(this._i_typeParams, oth._i_typeParams) && object.Equals(this._i_fields, oth._i_fields);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_attributes));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_typeParams));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_fields));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Struct.Struct";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_attributes);
      s += ", ";
      s += this._i_name.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_typeParams);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_fields);
      s += ")";
      return s;
    }
    private static readonly RAST._IStruct theDefault = create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Empty, Dafny.Sequence<Dafny.Rune>.Empty, Dafny.Sequence<RAST._ITypeParamDecl>.Empty, RAST.Fields.Default());
    public static RAST._IStruct Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IStruct> _TYPE = new Dafny.TypeDescriptor<RAST._IStruct>(RAST.Struct.Default());
    public static Dafny.TypeDescriptor<RAST._IStruct> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IStruct create(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IFields fields) {
      return new Struct(attributes, name, typeParams, fields);
    }
    public static _IStruct create_Struct(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IFields fields) {
      return create(attributes, name, typeParams, fields);
    }
    public bool is_Struct { get { return true; } }
    public Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_attributes {
      get {
        return this._i_attributes;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        return this._i_name;
      }
    }
    public Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams {
      get {
        return this._i_typeParams;
      }
    }
    public RAST._IFields dtor_fields {
      get {
        return this._i_fields;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(RAST.Attribute.ToStringMultiple((this).dtor_attributes, ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("pub struct ")), (this).dtor_name), RAST.TypeParamDecl.ToStringMultiple((this).dtor_typeParams, ind)), ((this).dtor_fields)._ToString(ind, ((this).dtor_fields).is_NamedFields)), ((((this).dtor_fields).is_NamelessFields) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))));
    }
  }

  public interface _ITypeSynonym {
    bool is_TypeSynonym { get; }
    Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_attributes { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams { get; }
    RAST._IType dtor_tpe { get; }
    _ITypeSynonym DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class TypeSynonym : _ITypeSynonym {
    public readonly Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _i_attributes;
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public readonly Dafny.ISequence<RAST._ITypeParamDecl> _i_typeParams;
    public readonly RAST._IType _i_tpe;
    public TypeSynonym(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IType tpe) {
      this._i_attributes = attributes;
      this._i_name = name;
      this._i_typeParams = typeParams;
      this._i_tpe = tpe;
    }
    public _ITypeSynonym DowncastClone() {
      if (this is _ITypeSynonym dt) { return dt; }
      return new TypeSynonym(_i_attributes, _i_name, _i_typeParams, _i_tpe);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.TypeSynonym;
      return oth != null && object.Equals(this._i_attributes, oth._i_attributes) && object.Equals(this._i_name, oth._i_name) && object.Equals(this._i_typeParams, oth._i_typeParams) && object.Equals(this._i_tpe, oth._i_tpe);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_attributes));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_typeParams));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tpe));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.TypeSynonym.TypeSynonym";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_attributes);
      s += ", ";
      s += this._i_name.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_typeParams);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_tpe);
      s += ")";
      return s;
    }
    private static readonly RAST._ITypeSynonym theDefault = create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Empty, Dafny.Sequence<Dafny.Rune>.Empty, Dafny.Sequence<RAST._ITypeParamDecl>.Empty, RAST.Type.Default());
    public static RAST._ITypeSynonym Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._ITypeSynonym> _TYPE = new Dafny.TypeDescriptor<RAST._ITypeSynonym>(RAST.TypeSynonym.Default());
    public static Dafny.TypeDescriptor<RAST._ITypeSynonym> _TypeDescriptor() {
      return _TYPE;
    }
    public static _ITypeSynonym create(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IType tpe) {
      return new TypeSynonym(attributes, name, typeParams, tpe);
    }
    public static _ITypeSynonym create_TypeSynonym(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IType tpe) {
      return create(attributes, name, typeParams, tpe);
    }
    public bool is_TypeSynonym { get { return true; } }
    public Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_attributes {
      get {
        return this._i_attributes;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        return this._i_name;
      }
    }
    public Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams {
      get {
        return this._i_typeParams;
      }
    }
    public RAST._IType dtor_tpe {
      get {
        return this._i_tpe;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(RAST.Attribute.ToStringMultiple((this).dtor_attributes, ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("pub type ")), (this).dtor_name), RAST.TypeParamDecl.ToStringMultiple((this).dtor_typeParams, ind)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" = ")), ((this).dtor_tpe)._ToString(ind)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";"));
    }
  }

  public interface _IConstant {
    bool is_Constant { get; }
    Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_attributes { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    RAST._IType dtor_tpe { get; }
    RAST._IExpr dtor_value { get; }
    _IConstant DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class Constant : _IConstant {
    public readonly Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _i_attributes;
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public readonly RAST._IType _i_tpe;
    public readonly RAST._IExpr _i_value;
    public Constant(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, RAST._IType tpe, RAST._IExpr @value) {
      this._i_attributes = attributes;
      this._i_name = name;
      this._i_tpe = tpe;
      this._i_value = @value;
    }
    public _IConstant DowncastClone() {
      if (this is _IConstant dt) { return dt; }
      return new Constant(_i_attributes, _i_name, _i_tpe, _i_value);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Constant;
      return oth != null && object.Equals(this._i_attributes, oth._i_attributes) && object.Equals(this._i_name, oth._i_name) && object.Equals(this._i_tpe, oth._i_tpe) && object.Equals(this._i_value, oth._i_value);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_attributes));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tpe));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_value));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Constant.Constant";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_attributes);
      s += ", ";
      s += this._i_name.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_tpe);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_value);
      s += ")";
      return s;
    }
    private static readonly RAST._IConstant theDefault = create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Empty, Dafny.Sequence<Dafny.Rune>.Empty, RAST.Type.Default(), RAST.Expr.Default());
    public static RAST._IConstant Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IConstant> _TYPE = new Dafny.TypeDescriptor<RAST._IConstant>(RAST.Constant.Default());
    public static Dafny.TypeDescriptor<RAST._IConstant> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IConstant create(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, RAST._IType tpe, RAST._IExpr @value) {
      return new Constant(attributes, name, tpe, @value);
    }
    public static _IConstant create_Constant(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, RAST._IType tpe, RAST._IExpr @value) {
      return create(attributes, name, tpe, @value);
    }
    public bool is_Constant { get { return true; } }
    public Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_attributes {
      get {
        return this._i_attributes;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        return this._i_name;
      }
    }
    public RAST._IType dtor_tpe {
      get {
        return this._i_tpe;
      }
    }
    public RAST._IExpr dtor_value {
      get {
        return this._i_value;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(RAST.Attribute.ToStringMultiple((this).dtor_attributes, ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("pub const ")), (this).dtor_name), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(": ")), ((this).dtor_tpe)._ToString(ind)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=")), ((this).dtor_value)._ToString(ind)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";"));
    }
  }

  public interface _INamelessField {
    bool is_NamelessField { get; }
    RAST._IVisibility dtor_visibility { get; }
    RAST._IType dtor_tpe { get; }
    _INamelessField DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class NamelessField : _INamelessField {
    public readonly RAST._IVisibility _i_visibility;
    public readonly RAST._IType _i_tpe;
    public NamelessField(RAST._IVisibility visibility, RAST._IType tpe) {
      this._i_visibility = visibility;
      this._i_tpe = tpe;
    }
    public _INamelessField DowncastClone() {
      if (this is _INamelessField dt) { return dt; }
      return new NamelessField(_i_visibility, _i_tpe);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.NamelessField;
      return oth != null && object.Equals(this._i_visibility, oth._i_visibility) && object.Equals(this._i_tpe, oth._i_tpe);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_visibility));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tpe));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.NamelessField.NamelessField";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_visibility);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_tpe);
      s += ")";
      return s;
    }
    private static readonly RAST._INamelessField theDefault = create(RAST.Visibility.Default(), RAST.Type.Default());
    public static RAST._INamelessField Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._INamelessField> _TYPE = new Dafny.TypeDescriptor<RAST._INamelessField>(RAST.NamelessField.Default());
    public static Dafny.TypeDescriptor<RAST._INamelessField> _TypeDescriptor() {
      return _TYPE;
    }
    public static _INamelessField create(RAST._IVisibility visibility, RAST._IType tpe) {
      return new NamelessField(visibility, tpe);
    }
    public static _INamelessField create_NamelessField(RAST._IVisibility visibility, RAST._IType tpe) {
      return create(visibility, tpe);
    }
    public bool is_NamelessField { get { return true; } }
    public RAST._IVisibility dtor_visibility {
      get {
        return this._i_visibility;
      }
    }
    public RAST._IType dtor_tpe {
      get {
        return this._i_tpe;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat(((this).dtor_visibility)._ToString(), ((this).dtor_tpe)._ToString(ind));
    }
  }

  public interface _IField {
    bool is_Field { get; }
    RAST._IVisibility dtor_visibility { get; }
    RAST._IFormal dtor_formal { get; }
    _IField DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class Field : _IField {
    public readonly RAST._IVisibility _i_visibility;
    public readonly RAST._IFormal _i_formal;
    public Field(RAST._IVisibility visibility, RAST._IFormal formal) {
      this._i_visibility = visibility;
      this._i_formal = formal;
    }
    public _IField DowncastClone() {
      if (this is _IField dt) { return dt; }
      return new Field(_i_visibility, _i_formal);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Field;
      return oth != null && object.Equals(this._i_visibility, oth._i_visibility) && object.Equals(this._i_formal, oth._i_formal);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_visibility));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_formal));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Field.Field";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_visibility);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_formal);
      s += ")";
      return s;
    }
    private static readonly RAST._IField theDefault = create(RAST.Visibility.Default(), RAST.Formal.Default());
    public static RAST._IField Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IField> _TYPE = new Dafny.TypeDescriptor<RAST._IField>(RAST.Field.Default());
    public static Dafny.TypeDescriptor<RAST._IField> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IField create(RAST._IVisibility visibility, RAST._IFormal formal) {
      return new Field(visibility, formal);
    }
    public static _IField create_Field(RAST._IVisibility visibility, RAST._IFormal formal) {
      return create(visibility, formal);
    }
    public bool is_Field { get { return true; } }
    public RAST._IVisibility dtor_visibility {
      get {
        return this._i_visibility;
      }
    }
    public RAST._IFormal dtor_formal {
      get {
        return this._i_formal;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat(((this).dtor_visibility)._ToString(), ((this).dtor_formal)._ToString(ind));
    }
  }

  public interface _IFields {
    bool is_NamedFields { get; }
    bool is_NamelessFields { get; }
    Dafny.ISequence<RAST._IField> dtor_fields { get; }
    Dafny.ISequence<RAST._INamelessField> dtor_types { get; }
    _IFields DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind, bool newLine);
  }
  public abstract class Fields : _IFields {
    public Fields() {
    }
    private static readonly RAST._IFields theDefault = create_NamedFields(Dafny.Sequence<RAST._IField>.Empty);
    public static RAST._IFields Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IFields> _TYPE = new Dafny.TypeDescriptor<RAST._IFields>(RAST.Fields.Default());
    public static Dafny.TypeDescriptor<RAST._IFields> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IFields create_NamedFields(Dafny.ISequence<RAST._IField> fields) {
      return new Fields_NamedFields(fields);
    }
    public static _IFields create_NamelessFields(Dafny.ISequence<RAST._INamelessField> types) {
      return new Fields_NamelessFields(types);
    }
    public bool is_NamedFields { get { return this is Fields_NamedFields; } }
    public bool is_NamelessFields { get { return this is Fields_NamelessFields; } }
    public Dafny.ISequence<RAST._IField> dtor_fields {
      get {
        var d = this;
        return ((Fields_NamedFields)d)._i_fields;
      }
    }
    public Dafny.ISequence<RAST._INamelessField> dtor_types {
      get {
        var d = this;
        return ((Fields_NamelessFields)d)._i_types;
      }
    }
    public abstract _IFields DowncastClone();
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind, bool newLine)
    {
      if ((this).is_NamedFields) {
        Dafny.ISequence<Dafny.Rune> _991_separator = ((newLine) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(",\n"), ind), RAST.__default.IND)) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", ")));
        _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs40 = (((newLine) && ((new BigInteger(((this).dtor_fields).Count)).Sign == 1)) ? (_System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind), RAST.__default.IND), Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind))) : ((((new BigInteger(((this).dtor_fields).Count)).Sign == 1) ? (_System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" "), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" "))) : (_System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))))));
        Dafny.ISequence<Dafny.Rune> _992_beginSpace = _let_tmp_rhs40.dtor__0;
        Dafny.ISequence<Dafny.Rune> _993_endSpace = _let_tmp_rhs40.dtor__1;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {"), _992_beginSpace), RAST.__default.SeqToString<RAST._IField>((this).dtor_fields, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IField, Dafny.ISequence<Dafny.Rune>>>>((_994_ind) => ((System.Func<RAST._IField, Dafny.ISequence<Dafny.Rune>>)((_995_field) => {
          return (_995_field)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_994_ind, RAST.__default.IND));
        })))(ind), _991_separator)), _993_endSpace), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
      } else {
        Dafny.ISequence<Dafny.Rune> _996_separator = ((newLine) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(",\n"), ind), RAST.__default.IND)) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", ")));
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), RAST.__default.SeqToString<RAST._INamelessField>((this).dtor_types, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._INamelessField, Dafny.ISequence<Dafny.Rune>>>>((_997_ind) => ((System.Func<RAST._INamelessField, Dafny.ISequence<Dafny.Rune>>)((_998_t) => {
          return (_998_t)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_997_ind, RAST.__default.IND));
        })))(ind), _996_separator)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
      }
    }
  }
  public class Fields_NamedFields : Fields {
    public readonly Dafny.ISequence<RAST._IField> _i_fields;
    public Fields_NamedFields(Dafny.ISequence<RAST._IField> fields) : base() {
      this._i_fields = fields;
    }
    public override _IFields DowncastClone() {
      if (this is _IFields dt) { return dt; }
      return new Fields_NamedFields(_i_fields);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Fields_NamedFields;
      return oth != null && object.Equals(this._i_fields, oth._i_fields);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_fields));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Fields.NamedFields";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_fields);
      s += ")";
      return s;
    }
  }
  public class Fields_NamelessFields : Fields {
    public readonly Dafny.ISequence<RAST._INamelessField> _i_types;
    public Fields_NamelessFields(Dafny.ISequence<RAST._INamelessField> types) : base() {
      this._i_types = types;
    }
    public override _IFields DowncastClone() {
      if (this is _IFields dt) { return dt; }
      return new Fields_NamelessFields(_i_types);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Fields_NamelessFields;
      return oth != null && object.Equals(this._i_types, oth._i_types);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_types));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Fields.NamelessFields";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_types);
      s += ")";
      return s;
    }
  }

  public interface _IEnumCase {
    bool is_EnumCase { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    RAST._IFields dtor_fields { get; }
    _IEnumCase DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind, bool newLine);
  }
  public class EnumCase : _IEnumCase {
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public readonly RAST._IFields _i_fields;
    public EnumCase(Dafny.ISequence<Dafny.Rune> name, RAST._IFields fields) {
      this._i_name = name;
      this._i_fields = fields;
    }
    public _IEnumCase DowncastClone() {
      if (this is _IEnumCase dt) { return dt; }
      return new EnumCase(_i_name, _i_fields);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.EnumCase;
      return oth != null && object.Equals(this._i_name, oth._i_name) && object.Equals(this._i_fields, oth._i_fields);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_fields));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.EnumCase.EnumCase";
      s += "(";
      s += this._i_name.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_fields);
      s += ")";
      return s;
    }
    private static readonly RAST._IEnumCase theDefault = create(Dafny.Sequence<Dafny.Rune>.Empty, RAST.Fields.Default());
    public static RAST._IEnumCase Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IEnumCase> _TYPE = new Dafny.TypeDescriptor<RAST._IEnumCase>(RAST.EnumCase.Default());
    public static Dafny.TypeDescriptor<RAST._IEnumCase> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IEnumCase create(Dafny.ISequence<Dafny.Rune> name, RAST._IFields fields) {
      return new EnumCase(name, fields);
    }
    public static _IEnumCase create_EnumCase(Dafny.ISequence<Dafny.Rune> name, RAST._IFields fields) {
      return create(name, fields);
    }
    public bool is_EnumCase { get { return true; } }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        return this._i_name;
      }
    }
    public RAST._IFields dtor_fields {
      get {
        return this._i_fields;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind, bool newLine)
    {
      return Dafny.Sequence<Dafny.Rune>.Concat((this).dtor_name, ((this).dtor_fields)._ToString(ind, newLine));
    }
  }

  public interface _IEnum {
    bool is_Enum { get; }
    Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_attributes { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams { get; }
    Dafny.ISequence<RAST._IEnumCase> dtor_variants { get; }
    _IEnum DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class Enum : _IEnum {
    public readonly Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _i_attributes;
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public readonly Dafny.ISequence<RAST._ITypeParamDecl> _i_typeParams;
    public readonly Dafny.ISequence<RAST._IEnumCase> _i_variants;
    public Enum(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, Dafny.ISequence<RAST._IEnumCase> variants) {
      this._i_attributes = attributes;
      this._i_name = name;
      this._i_typeParams = typeParams;
      this._i_variants = variants;
    }
    public _IEnum DowncastClone() {
      if (this is _IEnum dt) { return dt; }
      return new Enum(_i_attributes, _i_name, _i_typeParams, _i_variants);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Enum;
      return oth != null && object.Equals(this._i_attributes, oth._i_attributes) && object.Equals(this._i_name, oth._i_name) && object.Equals(this._i_typeParams, oth._i_typeParams) && object.Equals(this._i_variants, oth._i_variants);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_attributes));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_typeParams));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_variants));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Enum.Enum";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_attributes);
      s += ", ";
      s += this._i_name.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_typeParams);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_variants);
      s += ")";
      return s;
    }
    private static readonly RAST._IEnum theDefault = create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Empty, Dafny.Sequence<Dafny.Rune>.Empty, Dafny.Sequence<RAST._ITypeParamDecl>.Empty, Dafny.Sequence<RAST._IEnumCase>.Empty);
    public static RAST._IEnum Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IEnum> _TYPE = new Dafny.TypeDescriptor<RAST._IEnum>(RAST.Enum.Default());
    public static Dafny.TypeDescriptor<RAST._IEnum> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IEnum create(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, Dafny.ISequence<RAST._IEnumCase> variants) {
      return new Enum(attributes, name, typeParams, variants);
    }
    public static _IEnum create_Enum(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> attributes, Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, Dafny.ISequence<RAST._IEnumCase> variants) {
      return create(attributes, name, typeParams, variants);
    }
    public bool is_Enum { get { return true; } }
    public Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_attributes {
      get {
        return this._i_attributes;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        return this._i_name;
      }
    }
    public Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams {
      get {
        return this._i_typeParams;
      }
    }
    public Dafny.ISequence<RAST._IEnumCase> dtor_variants {
      get {
        return this._i_variants;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(RAST.Attribute.ToStringMultiple((this).dtor_attributes, ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("pub enum ")), (this).dtor_name), RAST.TypeParamDecl.ToStringMultiple((this).dtor_typeParams, ind)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {")), RAST.__default.SeqToString<RAST._IEnumCase>((this).dtor_variants, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IEnumCase, Dafny.ISequence<Dafny.Rune>>>>((_999_ind) => ((System.Func<RAST._IEnumCase, Dafny.ISequence<Dafny.Rune>>)((_1000_variant) => {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), _999_ind), RAST.__default.IND), (_1000_variant)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_999_ind, RAST.__default.IND), true));
      })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(","))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
    }
  }

  public interface _ITypeParamDecl {
    bool is_TypeParamDecl { get; }
    Dafny.ISequence<Dafny.Rune> dtor_content { get; }
    Dafny.ISequence<RAST._IType> dtor_constraints { get; }
    _ITypeParamDecl DowncastClone();
    RAST._ITypeParamDecl AddConstraints(Dafny.ISequence<RAST._IType> constraints);
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class TypeParamDecl : _ITypeParamDecl {
    public readonly Dafny.ISequence<Dafny.Rune> _i_content;
    public readonly Dafny.ISequence<RAST._IType> _i_constraints;
    public TypeParamDecl(Dafny.ISequence<Dafny.Rune> content, Dafny.ISequence<RAST._IType> constraints) {
      this._i_content = content;
      this._i_constraints = constraints;
    }
    public _ITypeParamDecl DowncastClone() {
      if (this is _ITypeParamDecl dt) { return dt; }
      return new TypeParamDecl(_i_content, _i_constraints);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.TypeParamDecl;
      return oth != null && object.Equals(this._i_content, oth._i_content) && object.Equals(this._i_constraints, oth._i_constraints);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_content));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_constraints));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.TypeParamDecl.TypeParamDecl";
      s += "(";
      s += this._i_content.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_constraints);
      s += ")";
      return s;
    }
    private static readonly RAST._ITypeParamDecl theDefault = create(Dafny.Sequence<Dafny.Rune>.Empty, Dafny.Sequence<RAST._IType>.Empty);
    public static RAST._ITypeParamDecl Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._ITypeParamDecl> _TYPE = new Dafny.TypeDescriptor<RAST._ITypeParamDecl>(RAST.TypeParamDecl.Default());
    public static Dafny.TypeDescriptor<RAST._ITypeParamDecl> _TypeDescriptor() {
      return _TYPE;
    }
    public static _ITypeParamDecl create(Dafny.ISequence<Dafny.Rune> content, Dafny.ISequence<RAST._IType> constraints) {
      return new TypeParamDecl(content, constraints);
    }
    public static _ITypeParamDecl create_TypeParamDecl(Dafny.ISequence<Dafny.Rune> content, Dafny.ISequence<RAST._IType> constraints) {
      return create(content, constraints);
    }
    public bool is_TypeParamDecl { get { return true; } }
    public Dafny.ISequence<Dafny.Rune> dtor_content {
      get {
        return this._i_content;
      }
    }
    public Dafny.ISequence<RAST._IType> dtor_constraints {
      get {
        return this._i_constraints;
      }
    }
    public static Dafny.ISequence<Dafny.Rune> ToStringMultiple(Dafny.ISequence<RAST._ITypeParamDecl> typeParams, Dafny.ISequence<Dafny.Rune> ind)
    {
      if ((new BigInteger((typeParams).Count)).Sign == 0) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
      } else {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), RAST.__default.SeqToString<RAST._ITypeParamDecl>(typeParams, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._ITypeParamDecl, Dafny.ISequence<Dafny.Rune>>>>((_1001_ind) => ((System.Func<RAST._ITypeParamDecl, Dafny.ISequence<Dafny.Rune>>)((_1002_t) => {
          return (_1002_t)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_1001_ind, RAST.__default.IND));
        })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">"));
      }
    }
    public static Dafny.ISequence<RAST._ITypeParamDecl> AddConstraintsMultiple(Dafny.ISequence<RAST._ITypeParamDecl> typeParams, Dafny.ISequence<RAST._IType> constraints)
    {
      Dafny.ISequence<RAST._ITypeParamDecl> _1003___accumulator = Dafny.Sequence<RAST._ITypeParamDecl>.FromElements();
    TAIL_CALL_START: ;
      if ((new BigInteger((typeParams).Count)).Sign == 0) {
        return Dafny.Sequence<RAST._ITypeParamDecl>.Concat(_1003___accumulator, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements());
      } else {
        _1003___accumulator = Dafny.Sequence<RAST._ITypeParamDecl>.Concat(_1003___accumulator, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(((typeParams).Select(BigInteger.Zero)).AddConstraints(constraints)));
        Dafny.ISequence<RAST._ITypeParamDecl> _in111 = (typeParams).Drop(BigInteger.One);
        Dafny.ISequence<RAST._IType> _in112 = constraints;
        typeParams = _in111;
        constraints = _in112;
        goto TAIL_CALL_START;
      }
    }
    public RAST._ITypeParamDecl AddConstraints(Dafny.ISequence<RAST._IType> constraints) {
      RAST._ITypeParamDecl _1004_dt__update__tmp_h0 = this;
      Dafny.ISequence<RAST._IType> _1005_dt__update_hconstraints_h0 = Dafny.Sequence<RAST._IType>.Concat((this).dtor_constraints, constraints);
      return RAST.TypeParamDecl.create((_1004_dt__update__tmp_h0).dtor_content, _1005_dt__update_hconstraints_h0);
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat((this).dtor_content, (((new BigInteger(((this).dtor_constraints).Count)).Sign == 0) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")) : (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(": "), RAST.__default.SeqToString<RAST._IType>((this).dtor_constraints, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IType, Dafny.ISequence<Dafny.Rune>>>>((_1006_ind) => ((System.Func<RAST._IType, Dafny.ISequence<Dafny.Rune>>)((_1007_t) => {
        return (_1007_t)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_1006_ind, RAST.__default.IND));
      })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" + "))))));
    }
  }

  public interface _IType {
    bool is_SelfOwned { get; }
    bool is_U8 { get; }
    bool is_U16 { get; }
    bool is_U32 { get; }
    bool is_U64 { get; }
    bool is_U128 { get; }
    bool is_I8 { get; }
    bool is_I16 { get; }
    bool is_I32 { get; }
    bool is_I64 { get; }
    bool is_I128 { get; }
    bool is_Bool { get; }
    bool is_TIdentifier { get; }
    bool is_TMemberSelect { get; }
    bool is_TypeApp { get; }
    bool is_Borrowed { get; }
    bool is_BorrowedMut { get; }
    bool is_Pointer { get; }
    bool is_PointerMut { get; }
    bool is_ImplType { get; }
    bool is_DynType { get; }
    bool is_TupleType { get; }
    bool is_FnType { get; }
    bool is_IntersectionType { get; }
    bool is_Array { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    RAST._IType dtor_base { get; }
    RAST._IType dtor_baseName { get; }
    Dafny.ISequence<RAST._IType> dtor_arguments { get; }
    RAST._IType dtor_underlying { get; }
    RAST._IType dtor_returnType { get; }
    RAST._IType dtor_left { get; }
    RAST._IType dtor_right { get; }
    Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> dtor_size { get; }
    _IType DowncastClone();
    bool CanReadWithoutClone();
    bool IsSelfPointer();
    Std.Wrappers._IOption<RAST._IType> ExtractMaybePlacebo();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
    RAST._IType MSel(Dafny.ISequence<Dafny.Rune> name);
    RAST._IType Apply1(RAST._IType arg);
    RAST._IType Apply(Dafny.ISequence<RAST._IType> args);
    RAST._IType ToOwned();
    RAST._IExpr ToNullExpr();
    bool IsMultiArray();
    Dafny.ISequence<Dafny.Rune> MultiArrayClass();
    RAST._IType TypeAtInitialization();
    bool IsMaybeUninit();
    bool IsUninitArray();
    bool IsObject();
    bool IsPointer();
    bool IsObjectOrPointer();
    RAST._IType ObjectOrPointerUnderlying();
  }
  public abstract class Type : _IType {
    public Type() {
    }
    private static readonly RAST._IType theDefault = create_SelfOwned();
    public static RAST._IType Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IType> _TYPE = new Dafny.TypeDescriptor<RAST._IType>(RAST.Type.Default());
    public static Dafny.TypeDescriptor<RAST._IType> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IType create_SelfOwned() {
      return new Type_SelfOwned();
    }
    public static _IType create_U8() {
      return new Type_U8();
    }
    public static _IType create_U16() {
      return new Type_U16();
    }
    public static _IType create_U32() {
      return new Type_U32();
    }
    public static _IType create_U64() {
      return new Type_U64();
    }
    public static _IType create_U128() {
      return new Type_U128();
    }
    public static _IType create_I8() {
      return new Type_I8();
    }
    public static _IType create_I16() {
      return new Type_I16();
    }
    public static _IType create_I32() {
      return new Type_I32();
    }
    public static _IType create_I64() {
      return new Type_I64();
    }
    public static _IType create_I128() {
      return new Type_I128();
    }
    public static _IType create_Bool() {
      return new Type_Bool();
    }
    public static _IType create_TIdentifier(Dafny.ISequence<Dafny.Rune> name) {
      return new Type_TIdentifier(name);
    }
    public static _IType create_TMemberSelect(RAST._IType @base, Dafny.ISequence<Dafny.Rune> name) {
      return new Type_TMemberSelect(@base, name);
    }
    public static _IType create_TypeApp(RAST._IType baseName, Dafny.ISequence<RAST._IType> arguments) {
      return new Type_TypeApp(baseName, arguments);
    }
    public static _IType create_Borrowed(RAST._IType underlying) {
      return new Type_Borrowed(underlying);
    }
    public static _IType create_BorrowedMut(RAST._IType underlying) {
      return new Type_BorrowedMut(underlying);
    }
    public static _IType create_Pointer(RAST._IType underlying) {
      return new Type_Pointer(underlying);
    }
    public static _IType create_PointerMut(RAST._IType underlying) {
      return new Type_PointerMut(underlying);
    }
    public static _IType create_ImplType(RAST._IType underlying) {
      return new Type_ImplType(underlying);
    }
    public static _IType create_DynType(RAST._IType underlying) {
      return new Type_DynType(underlying);
    }
    public static _IType create_TupleType(Dafny.ISequence<RAST._IType> arguments) {
      return new Type_TupleType(arguments);
    }
    public static _IType create_FnType(Dafny.ISequence<RAST._IType> arguments, RAST._IType returnType) {
      return new Type_FnType(arguments, returnType);
    }
    public static _IType create_IntersectionType(RAST._IType left, RAST._IType right) {
      return new Type_IntersectionType(left, right);
    }
    public static _IType create_Array(RAST._IType underlying, Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> size) {
      return new Type_Array(underlying, size);
    }
    public bool is_SelfOwned { get { return this is Type_SelfOwned; } }
    public bool is_U8 { get { return this is Type_U8; } }
    public bool is_U16 { get { return this is Type_U16; } }
    public bool is_U32 { get { return this is Type_U32; } }
    public bool is_U64 { get { return this is Type_U64; } }
    public bool is_U128 { get { return this is Type_U128; } }
    public bool is_I8 { get { return this is Type_I8; } }
    public bool is_I16 { get { return this is Type_I16; } }
    public bool is_I32 { get { return this is Type_I32; } }
    public bool is_I64 { get { return this is Type_I64; } }
    public bool is_I128 { get { return this is Type_I128; } }
    public bool is_Bool { get { return this is Type_Bool; } }
    public bool is_TIdentifier { get { return this is Type_TIdentifier; } }
    public bool is_TMemberSelect { get { return this is Type_TMemberSelect; } }
    public bool is_TypeApp { get { return this is Type_TypeApp; } }
    public bool is_Borrowed { get { return this is Type_Borrowed; } }
    public bool is_BorrowedMut { get { return this is Type_BorrowedMut; } }
    public bool is_Pointer { get { return this is Type_Pointer; } }
    public bool is_PointerMut { get { return this is Type_PointerMut; } }
    public bool is_ImplType { get { return this is Type_ImplType; } }
    public bool is_DynType { get { return this is Type_DynType; } }
    public bool is_TupleType { get { return this is Type_TupleType; } }
    public bool is_FnType { get { return this is Type_FnType; } }
    public bool is_IntersectionType { get { return this is Type_IntersectionType; } }
    public bool is_Array { get { return this is Type_Array; } }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        var d = this;
        if (d is Type_TIdentifier) { return ((Type_TIdentifier)d)._i_name; }
        return ((Type_TMemberSelect)d)._i_name;
      }
    }
    public RAST._IType dtor_base {
      get {
        var d = this;
        return ((Type_TMemberSelect)d)._i_base;
      }
    }
    public RAST._IType dtor_baseName {
      get {
        var d = this;
        return ((Type_TypeApp)d)._i_baseName;
      }
    }
    public Dafny.ISequence<RAST._IType> dtor_arguments {
      get {
        var d = this;
        if (d is Type_TypeApp) { return ((Type_TypeApp)d)._i_arguments; }
        if (d is Type_TupleType) { return ((Type_TupleType)d)._i_arguments; }
        return ((Type_FnType)d)._i_arguments;
      }
    }
    public RAST._IType dtor_underlying {
      get {
        var d = this;
        if (d is Type_Borrowed) { return ((Type_Borrowed)d)._i_underlying; }
        if (d is Type_BorrowedMut) { return ((Type_BorrowedMut)d)._i_underlying; }
        if (d is Type_Pointer) { return ((Type_Pointer)d)._i_underlying; }
        if (d is Type_PointerMut) { return ((Type_PointerMut)d)._i_underlying; }
        if (d is Type_ImplType) { return ((Type_ImplType)d)._i_underlying; }
        if (d is Type_DynType) { return ((Type_DynType)d)._i_underlying; }
        return ((Type_Array)d)._i_underlying;
      }
    }
    public RAST._IType dtor_returnType {
      get {
        var d = this;
        return ((Type_FnType)d)._i_returnType;
      }
    }
    public RAST._IType dtor_left {
      get {
        var d = this;
        return ((Type_IntersectionType)d)._i_left;
      }
    }
    public RAST._IType dtor_right {
      get {
        var d = this;
        return ((Type_IntersectionType)d)._i_right;
      }
    }
    public Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> dtor_size {
      get {
        var d = this;
        return ((Type_Array)d)._i_size;
      }
    }
    public abstract _IType DowncastClone();
    public bool CanReadWithoutClone() {
      return (((((((((((((this).is_U8) || ((this).is_U16)) || ((this).is_U32)) || ((this).is_U64)) || ((this).is_U128)) || ((this).is_I8)) || ((this).is_I16)) || ((this).is_I32)) || ((this).is_I64)) || ((this).is_I128)) || ((this).is_Bool)) || ((this).is_Pointer)) || ((this).is_PointerMut);
    }
    public bool IsSelfPointer() {
      return (((((this).is_Borrowed) && (((this).dtor_underlying).is_PointerMut)) && ((((this).dtor_underlying).dtor_underlying).is_SelfOwned)) || (((this).is_PointerMut) && (((this).dtor_underlying).is_SelfOwned))) || (((((this).is_PointerMut) && (((this).dtor_underlying).is_TypeApp)) && ((new BigInteger((((this).dtor_underlying).dtor_arguments).Count)).Sign == 0)) && ((((this).dtor_underlying).dtor_baseName).is_SelfOwned));
    }
    public Std.Wrappers._IOption<RAST._IType> ExtractMaybePlacebo() {
      RAST._IType _source35 = this;
      if (_source35.is_SelfOwned) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_U8) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_U16) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_U32) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_U64) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_U128) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_I8) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_I16) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_I32) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_I64) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_I128) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_Bool) {
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_TIdentifier) {
        Dafny.ISequence<Dafny.Rune> _1008___mcc_h0 = _source35.dtor_name;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_TMemberSelect) {
        RAST._IType _1009___mcc_h2 = _source35.dtor_base;
        Dafny.ISequence<Dafny.Rune> _1010___mcc_h3 = _source35.dtor_name;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_TypeApp) {
        RAST._IType _1011___mcc_h6 = _source35.dtor_baseName;
        Dafny.ISequence<RAST._IType> _1012___mcc_h7 = _source35.dtor_arguments;
        Dafny.ISequence<RAST._IType> _1013_arguments = _1012___mcc_h7;
        RAST._IType _1014_wrapper = _1011___mcc_h6;
        if (((object.Equals(_1014_wrapper, RAST.Type.create_TIdentifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("MaybePlacebo")))) || (object.Equals(_1014_wrapper, (RAST.__default.dafny__runtime__type).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("MaybePlacebo"))))) && ((new BigInteger((_1013_arguments).Count)) == (BigInteger.One))) {
          return Std.Wrappers.Option<RAST._IType>.create_Some((_1013_arguments).Select(BigInteger.Zero));
        } else {
          return Std.Wrappers.Option<RAST._IType>.create_None();
        }
      } else if (_source35.is_Borrowed) {
        RAST._IType _1015___mcc_h10 = _source35.dtor_underlying;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_BorrowedMut) {
        RAST._IType _1016___mcc_h12 = _source35.dtor_underlying;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_Pointer) {
        RAST._IType _1017___mcc_h14 = _source35.dtor_underlying;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_PointerMut) {
        RAST._IType _1018___mcc_h16 = _source35.dtor_underlying;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_ImplType) {
        RAST._IType _1019___mcc_h18 = _source35.dtor_underlying;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_DynType) {
        RAST._IType _1020___mcc_h20 = _source35.dtor_underlying;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_TupleType) {
        Dafny.ISequence<RAST._IType> _1021___mcc_h22 = _source35.dtor_arguments;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_FnType) {
        Dafny.ISequence<RAST._IType> _1022___mcc_h24 = _source35.dtor_arguments;
        RAST._IType _1023___mcc_h25 = _source35.dtor_returnType;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else if (_source35.is_IntersectionType) {
        RAST._IType _1024___mcc_h28 = _source35.dtor_left;
        RAST._IType _1025___mcc_h29 = _source35.dtor_right;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      } else {
        RAST._IType _1026___mcc_h32 = _source35.dtor_underlying;
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1027___mcc_h33 = _source35.dtor_size;
        return Std.Wrappers.Option<RAST._IType>.create_None();
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      RAST._IType _source36 = this;
      if (_source36.is_SelfOwned) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Self");
      } else if (_source36.is_U8) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u8");
      } else if (_source36.is_U16) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u16");
      } else if (_source36.is_U32) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u32");
      } else if (_source36.is_U64) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u64");
      } else if (_source36.is_U128) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u128");
      } else if (_source36.is_I8) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i8");
      } else if (_source36.is_I16) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i16");
      } else if (_source36.is_I32) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i32");
      } else if (_source36.is_I64) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i64");
      } else if (_source36.is_I128) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i128");
      } else if (_source36.is_Bool) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("bool");
      } else if (_source36.is_TIdentifier) {
        Dafny.ISequence<Dafny.Rune> _1028___mcc_h0 = _source36.dtor_name;
        Dafny.ISequence<Dafny.Rune> _1029_underlying = _1028___mcc_h0;
        return _1029_underlying;
      } else if (_source36.is_TMemberSelect) {
        RAST._IType _1030___mcc_h1 = _source36.dtor_base;
        Dafny.ISequence<Dafny.Rune> _1031___mcc_h2 = _source36.dtor_name;
        Dafny.ISequence<Dafny.Rune> _1032_name = _1031___mcc_h2;
        RAST._IType _1033_underlying = _1030___mcc_h1;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat((_1033_underlying)._ToString(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), _1032_name);
      } else if (_source36.is_TypeApp) {
        RAST._IType _1034___mcc_h3 = _source36.dtor_baseName;
        Dafny.ISequence<RAST._IType> _1035___mcc_h4 = _source36.dtor_arguments;
        Dafny.ISequence<RAST._IType> _1036_args = _1035___mcc_h4;
        RAST._IType _1037_base = _1034___mcc_h3;
        return Dafny.Sequence<Dafny.Rune>.Concat((_1037_base)._ToString(ind), (((_1036_args).Equals(Dafny.Sequence<RAST._IType>.FromElements())) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")) : (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), RAST.__default.SeqToString<RAST._IType>(_1036_args, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IType, Dafny.ISequence<Dafny.Rune>>>>((_1038_ind) => ((System.Func<RAST._IType, Dafny.ISequence<Dafny.Rune>>)((_1039_arg) => {
          return (_1039_arg)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_1038_ind, RAST.__default.IND));
        })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">")))));
      } else if (_source36.is_Borrowed) {
        RAST._IType _1040___mcc_h5 = _source36.dtor_underlying;
        RAST._IType _1041_underlying = _1040___mcc_h5;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), (_1041_underlying)._ToString(ind));
      } else if (_source36.is_BorrowedMut) {
        RAST._IType _1042___mcc_h6 = _source36.dtor_underlying;
        RAST._IType _1043_underlying = _1042___mcc_h6;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&mut "), (_1043_underlying)._ToString(ind));
      } else if (_source36.is_Pointer) {
        RAST._IType _1044___mcc_h7 = _source36.dtor_underlying;
        RAST._IType _1045_underlying = _1044___mcc_h7;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*const "), (_1045_underlying)._ToString(ind));
      } else if (_source36.is_PointerMut) {
        RAST._IType _1046___mcc_h8 = _source36.dtor_underlying;
        RAST._IType _1047_underlying = _1046___mcc_h8;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*mut "), (_1047_underlying)._ToString(ind));
      } else if (_source36.is_ImplType) {
        RAST._IType _1048___mcc_h9 = _source36.dtor_underlying;
        RAST._IType _1049_underlying = _1048___mcc_h9;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("impl "), (_1049_underlying)._ToString(ind));
      } else if (_source36.is_DynType) {
        RAST._IType _1050___mcc_h10 = _source36.dtor_underlying;
        RAST._IType _1051_underlying = _1050___mcc_h10;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dyn "), (_1051_underlying)._ToString(ind));
      } else if (_source36.is_TupleType) {
        Dafny.ISequence<RAST._IType> _1052___mcc_h11 = _source36.dtor_arguments;
        Dafny.ISequence<RAST._IType> _1053_args = _1052___mcc_h11;
        if ((_1053_args).Equals(Dafny.Sequence<RAST._IType>.FromElements())) {
          return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("()");
        } else {
          return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), RAST.__default.SeqToString<RAST._IType>(_1053_args, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IType, Dafny.ISequence<Dafny.Rune>>>>((_1054_ind) => ((System.Func<RAST._IType, Dafny.ISequence<Dafny.Rune>>)((_1055_arg) => {
            return (_1055_arg)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_1054_ind, RAST.__default.IND));
          })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
        }
      } else if (_source36.is_FnType) {
        Dafny.ISequence<RAST._IType> _1056___mcc_h12 = _source36.dtor_arguments;
        RAST._IType _1057___mcc_h13 = _source36.dtor_returnType;
        RAST._IType _1058_returnType = _1057___mcc_h13;
        Dafny.ISequence<RAST._IType> _1059_arguments = _1056___mcc_h12;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::std::ops::Fn("), RAST.__default.SeqToString<RAST._IType>(_1059_arguments, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IType, Dafny.ISequence<Dafny.Rune>>>>((_1060_ind) => ((System.Func<RAST._IType, Dafny.ISequence<Dafny.Rune>>)((_1061_arg) => {
          return (_1061_arg)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_1060_ind, RAST.__default.IND));
        })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(") -> ")), (_1058_returnType)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND)));
      } else if (_source36.is_IntersectionType) {
        RAST._IType _1062___mcc_h14 = _source36.dtor_left;
        RAST._IType _1063___mcc_h15 = _source36.dtor_right;
        RAST._IType _1064_right = _1063___mcc_h15;
        RAST._IType _1065_left = _1062___mcc_h14;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat((_1065_left)._ToString(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" + ")), (_1064_right)._ToString(ind));
      } else {
        RAST._IType _1066___mcc_h16 = _source36.dtor_underlying;
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1067___mcc_h17 = _source36.dtor_size;
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1068_size = _1067___mcc_h17;
        RAST._IType _1069_underlying = _1066___mcc_h16;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("["), (_1069_underlying)._ToString(ind)), (((_1068_size).is_Some) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("; "), (_1068_size).dtor_value)) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("]"));
      }
    }
    public RAST._IType MSel(Dafny.ISequence<Dafny.Rune> name) {
      return RAST.Type.create_TMemberSelect(this, name);
    }
    public RAST._IType Apply1(RAST._IType arg) {
      return RAST.Type.create_TypeApp(this, Dafny.Sequence<RAST._IType>.FromElements(arg));
    }
    public RAST._IType Apply(Dafny.ISequence<RAST._IType> args) {
      return RAST.Type.create_TypeApp(this, args);
    }
    public RAST._IType ToOwned() {
      RAST._IType _source37 = this;
      if (_source37.is_SelfOwned) {
        RAST._IType _1070_x = this;
        return _1070_x;
      } else if (_source37.is_U8) {
        RAST._IType _1071_x = this;
        return _1071_x;
      } else if (_source37.is_U16) {
        RAST._IType _1072_x = this;
        return _1072_x;
      } else if (_source37.is_U32) {
        RAST._IType _1073_x = this;
        return _1073_x;
      } else if (_source37.is_U64) {
        RAST._IType _1074_x = this;
        return _1074_x;
      } else if (_source37.is_U128) {
        RAST._IType _1075_x = this;
        return _1075_x;
      } else if (_source37.is_I8) {
        RAST._IType _1076_x = this;
        return _1076_x;
      } else if (_source37.is_I16) {
        RAST._IType _1077_x = this;
        return _1077_x;
      } else if (_source37.is_I32) {
        RAST._IType _1078_x = this;
        return _1078_x;
      } else if (_source37.is_I64) {
        RAST._IType _1079_x = this;
        return _1079_x;
      } else if (_source37.is_I128) {
        RAST._IType _1080_x = this;
        return _1080_x;
      } else if (_source37.is_Bool) {
        RAST._IType _1081_x = this;
        return _1081_x;
      } else if (_source37.is_TIdentifier) {
        Dafny.ISequence<Dafny.Rune> _1082___mcc_h0 = _source37.dtor_name;
        RAST._IType _1083_x = this;
        return _1083_x;
      } else if (_source37.is_TMemberSelect) {
        RAST._IType _1084___mcc_h2 = _source37.dtor_base;
        Dafny.ISequence<Dafny.Rune> _1085___mcc_h3 = _source37.dtor_name;
        RAST._IType _1086_x = this;
        return _1086_x;
      } else if (_source37.is_TypeApp) {
        RAST._IType _1087___mcc_h6 = _source37.dtor_baseName;
        Dafny.ISequence<RAST._IType> _1088___mcc_h7 = _source37.dtor_arguments;
        RAST._IType _1089_x = this;
        return _1089_x;
      } else if (_source37.is_Borrowed) {
        RAST._IType _1090___mcc_h10 = _source37.dtor_underlying;
        RAST._IType _1091_x = _1090___mcc_h10;
        return _1091_x;
      } else if (_source37.is_BorrowedMut) {
        RAST._IType _1092___mcc_h12 = _source37.dtor_underlying;
        RAST._IType _1093_x = _1092___mcc_h12;
        return _1093_x;
      } else if (_source37.is_Pointer) {
        RAST._IType _1094___mcc_h14 = _source37.dtor_underlying;
        RAST._IType _1095_x = this;
        return _1095_x;
      } else if (_source37.is_PointerMut) {
        RAST._IType _1096___mcc_h16 = _source37.dtor_underlying;
        RAST._IType _1097_x = this;
        return _1097_x;
      } else if (_source37.is_ImplType) {
        RAST._IType _1098___mcc_h18 = _source37.dtor_underlying;
        RAST._IType _1099_x = this;
        return _1099_x;
      } else if (_source37.is_DynType) {
        RAST._IType _1100___mcc_h20 = _source37.dtor_underlying;
        RAST._IType _1101_x = this;
        return _1101_x;
      } else if (_source37.is_TupleType) {
        Dafny.ISequence<RAST._IType> _1102___mcc_h22 = _source37.dtor_arguments;
        RAST._IType _1103_x = this;
        return _1103_x;
      } else if (_source37.is_FnType) {
        Dafny.ISequence<RAST._IType> _1104___mcc_h24 = _source37.dtor_arguments;
        RAST._IType _1105___mcc_h25 = _source37.dtor_returnType;
        RAST._IType _1106_x = this;
        return _1106_x;
      } else if (_source37.is_IntersectionType) {
        RAST._IType _1107___mcc_h28 = _source37.dtor_left;
        RAST._IType _1108___mcc_h29 = _source37.dtor_right;
        RAST._IType _1109_x = this;
        return _1109_x;
      } else {
        RAST._IType _1110___mcc_h32 = _source37.dtor_underlying;
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1111___mcc_h33 = _source37.dtor_size;
        RAST._IType _1112_x = this;
        return _1112_x;
      }
    }
    public RAST._IExpr ToNullExpr() {
      if ((this).IsObject()) {
        return ((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Object"))).Apply1((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("option"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Option"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("None")));
      } else {
        RAST._IType _1113_underlying = (this).dtor_underlying;
        Dafny.ISequence<Dafny.Rune> _1114_n = (((this).is_PointerMut) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("null_mut")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("null")));
        if (((_1113_underlying).is_Array) && (((_1113_underlying).dtor_size).is_None)) {
          return ((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("ptr"))).MSel(_1114_n)).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_Array((_1113_underlying).dtor_underlying, Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")))))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
        } else {
          return (((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("ptr"))).MSel(_1114_n)).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
        }
      }
    }
    public bool IsMultiArray() {
      return ((this).is_TypeApp) && (Dafny.Helpers.Let<RAST._IType, bool>((this).dtor_baseName, _pat_let6_0 => Dafny.Helpers.Let<RAST._IType, bool>(_pat_let6_0, _1115_baseName => Dafny.Helpers.Let<Dafny.ISequence<RAST._IType>, bool>((this).dtor_arguments, _pat_let7_0 => Dafny.Helpers.Let<Dafny.ISequence<RAST._IType>, bool>(_pat_let7_0, _1116_args => (((((new BigInteger((_1116_args).Count)) == (BigInteger.One)) && ((_1115_baseName).is_TMemberSelect)) && (object.Equals((_1115_baseName).dtor_base, RAST.__default.dafny__runtime__type))) && ((new BigInteger(((_1115_baseName).dtor_name).Count)) >= (new BigInteger(5)))) && ((((_1115_baseName).dtor_name).Subsequence(BigInteger.Zero, new BigInteger(5))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Array"))))))));
    }
    public Dafny.ISequence<Dafny.Rune> MultiArrayClass() {
      return ((this).dtor_baseName).dtor_name;
    }
    public RAST._IType TypeAtInitialization() {
      if ((this).IsObjectOrPointer()) {
        RAST._IType _1117_s = (this).ObjectOrPointerUnderlying();
        if (((_1117_s).is_Array) && (((_1117_s).dtor_size).is_None)) {
          RAST._IType _1118_newUnderlying = RAST.Type.create_Array(RAST.__default.MaybeUninitType((_1117_s).dtor_underlying), Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None());
          if ((this).IsObject()) {
            return RAST.__default.ObjectType(_1118_newUnderlying);
          } else {
            return RAST.Type.create_PointerMut(_1118_newUnderlying);
          }
        } else if ((_1117_s).IsMultiArray()) {
          RAST._IType _1119_newUnderlying = RAST.Type.create_TypeApp((_1117_s).dtor_baseName, Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.MaybeUninitType(((_1117_s).dtor_arguments).Select(BigInteger.Zero))));
          if ((this).IsObject()) {
            return RAST.__default.ObjectType(_1119_newUnderlying);
          } else {
            return RAST.Type.create_PointerMut(_1119_newUnderlying);
          }
        } else {
          return this;
        }
      } else {
        return this;
      }
    }
    public bool IsMaybeUninit() {
      return (((this).is_TypeApp) && (object.Equals((this).dtor_baseName, RAST.__default.MaybeUninitPath))) && ((new BigInteger(((this).dtor_arguments).Count)) == (BigInteger.One));
    }
    public bool IsUninitArray() {
      if ((this).IsObjectOrPointer()) {
        RAST._IType _1120_s = (this).ObjectOrPointerUnderlying();
        if (((_1120_s).is_Array) && (((_1120_s).dtor_size).is_None)) {
          return ((_1120_s).dtor_underlying).IsMaybeUninit();
        } else if ((_1120_s).IsMultiArray()) {
          return (((_1120_s).dtor_arguments).Select(BigInteger.Zero)).IsMaybeUninit();
        } else {
          return false;
        }
      } else {
        return false;
      }
    }
    public bool IsObject() {
      RAST._IType _source38 = this;
      if (_source38.is_SelfOwned) {
        return false;
      } else if (_source38.is_U8) {
        return false;
      } else if (_source38.is_U16) {
        return false;
      } else if (_source38.is_U32) {
        return false;
      } else if (_source38.is_U64) {
        return false;
      } else if (_source38.is_U128) {
        return false;
      } else if (_source38.is_I8) {
        return false;
      } else if (_source38.is_I16) {
        return false;
      } else if (_source38.is_I32) {
        return false;
      } else if (_source38.is_I64) {
        return false;
      } else if (_source38.is_I128) {
        return false;
      } else if (_source38.is_Bool) {
        return false;
      } else if (_source38.is_TIdentifier) {
        Dafny.ISequence<Dafny.Rune> _1121___mcc_h0 = _source38.dtor_name;
        return false;
      } else if (_source38.is_TMemberSelect) {
        RAST._IType _1122___mcc_h2 = _source38.dtor_base;
        Dafny.ISequence<Dafny.Rune> _1123___mcc_h3 = _source38.dtor_name;
        return false;
      } else if (_source38.is_TypeApp) {
        RAST._IType _1124___mcc_h6 = _source38.dtor_baseName;
        Dafny.ISequence<RAST._IType> _1125___mcc_h7 = _source38.dtor_arguments;
        RAST._IType _source39 = _1124___mcc_h6;
        if (_source39.is_SelfOwned) {
          return false;
        } else if (_source39.is_U8) {
          return false;
        } else if (_source39.is_U16) {
          return false;
        } else if (_source39.is_U32) {
          return false;
        } else if (_source39.is_U64) {
          return false;
        } else if (_source39.is_U128) {
          return false;
        } else if (_source39.is_I8) {
          return false;
        } else if (_source39.is_I16) {
          return false;
        } else if (_source39.is_I32) {
          return false;
        } else if (_source39.is_I64) {
          return false;
        } else if (_source39.is_I128) {
          return false;
        } else if (_source39.is_Bool) {
          return false;
        } else if (_source39.is_TIdentifier) {
          Dafny.ISequence<Dafny.Rune> _1126___mcc_h10 = _source39.dtor_name;
          return false;
        } else if (_source39.is_TMemberSelect) {
          RAST._IType _1127___mcc_h12 = _source39.dtor_base;
          Dafny.ISequence<Dafny.Rune> _1128___mcc_h13 = _source39.dtor_name;
          RAST._IType _source40 = _1127___mcc_h12;
          if (_source40.is_SelfOwned) {
            return false;
          } else if (_source40.is_U8) {
            return false;
          } else if (_source40.is_U16) {
            return false;
          } else if (_source40.is_U32) {
            return false;
          } else if (_source40.is_U64) {
            return false;
          } else if (_source40.is_U128) {
            return false;
          } else if (_source40.is_I8) {
            return false;
          } else if (_source40.is_I16) {
            return false;
          } else if (_source40.is_I32) {
            return false;
          } else if (_source40.is_I64) {
            return false;
          } else if (_source40.is_I128) {
            return false;
          } else if (_source40.is_Bool) {
            return false;
          } else if (_source40.is_TIdentifier) {
            Dafny.ISequence<Dafny.Rune> _1129___mcc_h16 = _source40.dtor_name;
            return false;
          } else if (_source40.is_TMemberSelect) {
            RAST._IType _1130___mcc_h18 = _source40.dtor_base;
            Dafny.ISequence<Dafny.Rune> _1131___mcc_h19 = _source40.dtor_name;
            RAST._IType _source41 = _1130___mcc_h18;
            if (_source41.is_SelfOwned) {
              return false;
            } else if (_source41.is_U8) {
              return false;
            } else if (_source41.is_U16) {
              return false;
            } else if (_source41.is_U32) {
              return false;
            } else if (_source41.is_U64) {
              return false;
            } else if (_source41.is_U128) {
              return false;
            } else if (_source41.is_I8) {
              return false;
            } else if (_source41.is_I16) {
              return false;
            } else if (_source41.is_I32) {
              return false;
            } else if (_source41.is_I64) {
              return false;
            } else if (_source41.is_I128) {
              return false;
            } else if (_source41.is_Bool) {
              return false;
            } else if (_source41.is_TIdentifier) {
              Dafny.ISequence<Dafny.Rune> _1132___mcc_h22 = _source41.dtor_name;
              if (object.Equals(_1132___mcc_h22, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) {
                if (object.Equals(_1131___mcc_h19, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime"))) {
                  if (object.Equals(_1128___mcc_h13, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Object"))) {
                    Dafny.ISequence<RAST._IType> _1133_elems1 = _1125___mcc_h7;
                    return (new BigInteger((_1133_elems1).Count)) == (BigInteger.One);
                  } else {
                    return false;
                  }
                } else {
                  return false;
                }
              } else {
                return false;
              }
            } else if (_source41.is_TMemberSelect) {
              RAST._IType _1134___mcc_h24 = _source41.dtor_base;
              Dafny.ISequence<Dafny.Rune> _1135___mcc_h25 = _source41.dtor_name;
              return false;
            } else if (_source41.is_TypeApp) {
              RAST._IType _1136___mcc_h28 = _source41.dtor_baseName;
              Dafny.ISequence<RAST._IType> _1137___mcc_h29 = _source41.dtor_arguments;
              return false;
            } else if (_source41.is_Borrowed) {
              RAST._IType _1138___mcc_h32 = _source41.dtor_underlying;
              return false;
            } else if (_source41.is_BorrowedMut) {
              RAST._IType _1139___mcc_h34 = _source41.dtor_underlying;
              return false;
            } else if (_source41.is_Pointer) {
              RAST._IType _1140___mcc_h36 = _source41.dtor_underlying;
              return false;
            } else if (_source41.is_PointerMut) {
              RAST._IType _1141___mcc_h38 = _source41.dtor_underlying;
              return false;
            } else if (_source41.is_ImplType) {
              RAST._IType _1142___mcc_h40 = _source41.dtor_underlying;
              return false;
            } else if (_source41.is_DynType) {
              RAST._IType _1143___mcc_h42 = _source41.dtor_underlying;
              return false;
            } else if (_source41.is_TupleType) {
              Dafny.ISequence<RAST._IType> _1144___mcc_h44 = _source41.dtor_arguments;
              return false;
            } else if (_source41.is_FnType) {
              Dafny.ISequence<RAST._IType> _1145___mcc_h46 = _source41.dtor_arguments;
              RAST._IType _1146___mcc_h47 = _source41.dtor_returnType;
              return false;
            } else if (_source41.is_IntersectionType) {
              RAST._IType _1147___mcc_h50 = _source41.dtor_left;
              RAST._IType _1148___mcc_h51 = _source41.dtor_right;
              return false;
            } else {
              RAST._IType _1149___mcc_h54 = _source41.dtor_underlying;
              Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1150___mcc_h55 = _source41.dtor_size;
              return false;
            }
          } else if (_source40.is_TypeApp) {
            RAST._IType _1151___mcc_h58 = _source40.dtor_baseName;
            Dafny.ISequence<RAST._IType> _1152___mcc_h59 = _source40.dtor_arguments;
            return false;
          } else if (_source40.is_Borrowed) {
            RAST._IType _1153___mcc_h62 = _source40.dtor_underlying;
            return false;
          } else if (_source40.is_BorrowedMut) {
            RAST._IType _1154___mcc_h64 = _source40.dtor_underlying;
            return false;
          } else if (_source40.is_Pointer) {
            RAST._IType _1155___mcc_h66 = _source40.dtor_underlying;
            return false;
          } else if (_source40.is_PointerMut) {
            RAST._IType _1156___mcc_h68 = _source40.dtor_underlying;
            return false;
          } else if (_source40.is_ImplType) {
            RAST._IType _1157___mcc_h70 = _source40.dtor_underlying;
            return false;
          } else if (_source40.is_DynType) {
            RAST._IType _1158___mcc_h72 = _source40.dtor_underlying;
            return false;
          } else if (_source40.is_TupleType) {
            Dafny.ISequence<RAST._IType> _1159___mcc_h74 = _source40.dtor_arguments;
            return false;
          } else if (_source40.is_FnType) {
            Dafny.ISequence<RAST._IType> _1160___mcc_h76 = _source40.dtor_arguments;
            RAST._IType _1161___mcc_h77 = _source40.dtor_returnType;
            return false;
          } else if (_source40.is_IntersectionType) {
            RAST._IType _1162___mcc_h80 = _source40.dtor_left;
            RAST._IType _1163___mcc_h81 = _source40.dtor_right;
            return false;
          } else {
            RAST._IType _1164___mcc_h84 = _source40.dtor_underlying;
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1165___mcc_h85 = _source40.dtor_size;
            return false;
          }
        } else if (_source39.is_TypeApp) {
          RAST._IType _1166___mcc_h88 = _source39.dtor_baseName;
          Dafny.ISequence<RAST._IType> _1167___mcc_h89 = _source39.dtor_arguments;
          return false;
        } else if (_source39.is_Borrowed) {
          RAST._IType _1168___mcc_h92 = _source39.dtor_underlying;
          return false;
        } else if (_source39.is_BorrowedMut) {
          RAST._IType _1169___mcc_h94 = _source39.dtor_underlying;
          return false;
        } else if (_source39.is_Pointer) {
          RAST._IType _1170___mcc_h96 = _source39.dtor_underlying;
          return false;
        } else if (_source39.is_PointerMut) {
          RAST._IType _1171___mcc_h98 = _source39.dtor_underlying;
          return false;
        } else if (_source39.is_ImplType) {
          RAST._IType _1172___mcc_h100 = _source39.dtor_underlying;
          return false;
        } else if (_source39.is_DynType) {
          RAST._IType _1173___mcc_h102 = _source39.dtor_underlying;
          return false;
        } else if (_source39.is_TupleType) {
          Dafny.ISequence<RAST._IType> _1174___mcc_h104 = _source39.dtor_arguments;
          return false;
        } else if (_source39.is_FnType) {
          Dafny.ISequence<RAST._IType> _1175___mcc_h106 = _source39.dtor_arguments;
          RAST._IType _1176___mcc_h107 = _source39.dtor_returnType;
          return false;
        } else if (_source39.is_IntersectionType) {
          RAST._IType _1177___mcc_h110 = _source39.dtor_left;
          RAST._IType _1178___mcc_h111 = _source39.dtor_right;
          return false;
        } else {
          RAST._IType _1179___mcc_h114 = _source39.dtor_underlying;
          Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1180___mcc_h115 = _source39.dtor_size;
          return false;
        }
      } else if (_source38.is_Borrowed) {
        RAST._IType _1181___mcc_h118 = _source38.dtor_underlying;
        return false;
      } else if (_source38.is_BorrowedMut) {
        RAST._IType _1182___mcc_h120 = _source38.dtor_underlying;
        return false;
      } else if (_source38.is_Pointer) {
        RAST._IType _1183___mcc_h122 = _source38.dtor_underlying;
        return false;
      } else if (_source38.is_PointerMut) {
        RAST._IType _1184___mcc_h124 = _source38.dtor_underlying;
        return false;
      } else if (_source38.is_ImplType) {
        RAST._IType _1185___mcc_h126 = _source38.dtor_underlying;
        return false;
      } else if (_source38.is_DynType) {
        RAST._IType _1186___mcc_h128 = _source38.dtor_underlying;
        return false;
      } else if (_source38.is_TupleType) {
        Dafny.ISequence<RAST._IType> _1187___mcc_h130 = _source38.dtor_arguments;
        return false;
      } else if (_source38.is_FnType) {
        Dafny.ISequence<RAST._IType> _1188___mcc_h132 = _source38.dtor_arguments;
        RAST._IType _1189___mcc_h133 = _source38.dtor_returnType;
        return false;
      } else if (_source38.is_IntersectionType) {
        RAST._IType _1190___mcc_h136 = _source38.dtor_left;
        RAST._IType _1191___mcc_h137 = _source38.dtor_right;
        return false;
      } else {
        RAST._IType _1192___mcc_h140 = _source38.dtor_underlying;
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1193___mcc_h141 = _source38.dtor_size;
        return false;
      }
    }
    public bool IsPointer() {
      return ((this).is_Pointer) || ((this).is_PointerMut);
    }
    public bool IsObjectOrPointer() {
      return ((this).IsPointer()) || ((this).IsObject());
    }
    public RAST._IType ObjectOrPointerUnderlying() {
      if (((this).is_PointerMut) || ((this).is_Pointer)) {
        return (this).dtor_underlying;
      } else {
        RAST._IType _source42 = this;
        RAST._IType _1194___mcc_h3 = _source42.dtor_baseName;
        Dafny.ISequence<RAST._IType> _1195___mcc_h4 = _source42.dtor_arguments;
        RAST._IType _source43 = _1194___mcc_h3;
        RAST._IType _1196___mcc_h6 = _source43.dtor_base;
        Dafny.ISequence<Dafny.Rune> _1197___mcc_h7 = _source43.dtor_name;
        RAST._IType _source44 = _1196___mcc_h6;
        RAST._IType _1198___mcc_h9 = _source44.dtor_base;
        Dafny.ISequence<Dafny.Rune> _1199___mcc_h10 = _source44.dtor_name;
        RAST._IType _source45 = _1198___mcc_h9;
        Dafny.ISequence<Dafny.Rune> _1200___mcc_h11 = _source45.dtor_name;
        Dafny.ISequence<RAST._IType> _1201_elems1 = _1195___mcc_h4;
        Dafny.ISequence<Dafny.Rune> _1202_tpe1 = _1197___mcc_h7;
        return (_1201_elems1).Select(BigInteger.Zero);
      }
    }
  }
  public class Type_SelfOwned : Type {
    public Type_SelfOwned() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_SelfOwned();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_SelfOwned;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.SelfOwned";
      return s;
    }
  }
  public class Type_U8 : Type {
    public Type_U8() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_U8();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_U8;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.U8";
      return s;
    }
  }
  public class Type_U16 : Type {
    public Type_U16() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_U16();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_U16;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 2;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.U16";
      return s;
    }
  }
  public class Type_U32 : Type {
    public Type_U32() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_U32();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_U32;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 3;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.U32";
      return s;
    }
  }
  public class Type_U64 : Type {
    public Type_U64() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_U64();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_U64;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 4;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.U64";
      return s;
    }
  }
  public class Type_U128 : Type {
    public Type_U128() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_U128();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_U128;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 5;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.U128";
      return s;
    }
  }
  public class Type_I8 : Type {
    public Type_I8() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_I8();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_I8;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 6;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.I8";
      return s;
    }
  }
  public class Type_I16 : Type {
    public Type_I16() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_I16();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_I16;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 7;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.I16";
      return s;
    }
  }
  public class Type_I32 : Type {
    public Type_I32() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_I32();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_I32;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 8;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.I32";
      return s;
    }
  }
  public class Type_I64 : Type {
    public Type_I64() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_I64();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_I64;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 9;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.I64";
      return s;
    }
  }
  public class Type_I128 : Type {
    public Type_I128() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_I128();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_I128;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 10;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.I128";
      return s;
    }
  }
  public class Type_Bool : Type {
    public Type_Bool() : base() {
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_Bool();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_Bool;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 11;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.Bool";
      return s;
    }
  }
  public class Type_TIdentifier : Type {
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public Type_TIdentifier(Dafny.ISequence<Dafny.Rune> name) : base() {
      this._i_name = name;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_TIdentifier(_i_name);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_TIdentifier;
      return oth != null && object.Equals(this._i_name, oth._i_name);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 12;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.TIdentifier";
      s += "(";
      s += this._i_name.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class Type_TMemberSelect : Type {
    public readonly RAST._IType _i_base;
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public Type_TMemberSelect(RAST._IType @base, Dafny.ISequence<Dafny.Rune> name) : base() {
      this._i_base = @base;
      this._i_name = name;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_TMemberSelect(_i_base, _i_name);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_TMemberSelect;
      return oth != null && object.Equals(this._i_base, oth._i_base) && object.Equals(this._i_name, oth._i_name);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 13;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_base));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.TMemberSelect";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_base);
      s += ", ";
      s += this._i_name.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class Type_TypeApp : Type {
    public readonly RAST._IType _i_baseName;
    public readonly Dafny.ISequence<RAST._IType> _i_arguments;
    public Type_TypeApp(RAST._IType baseName, Dafny.ISequence<RAST._IType> arguments) : base() {
      this._i_baseName = baseName;
      this._i_arguments = arguments;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_TypeApp(_i_baseName, _i_arguments);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_TypeApp;
      return oth != null && object.Equals(this._i_baseName, oth._i_baseName) && object.Equals(this._i_arguments, oth._i_arguments);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 14;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_baseName));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_arguments));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.TypeApp";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_baseName);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_arguments);
      s += ")";
      return s;
    }
  }
  public class Type_Borrowed : Type {
    public readonly RAST._IType _i_underlying;
    public Type_Borrowed(RAST._IType underlying) : base() {
      this._i_underlying = underlying;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_Borrowed(_i_underlying);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_Borrowed;
      return oth != null && object.Equals(this._i_underlying, oth._i_underlying);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 15;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.Borrowed";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ")";
      return s;
    }
  }
  public class Type_BorrowedMut : Type {
    public readonly RAST._IType _i_underlying;
    public Type_BorrowedMut(RAST._IType underlying) : base() {
      this._i_underlying = underlying;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_BorrowedMut(_i_underlying);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_BorrowedMut;
      return oth != null && object.Equals(this._i_underlying, oth._i_underlying);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 16;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.BorrowedMut";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ")";
      return s;
    }
  }
  public class Type_Pointer : Type {
    public readonly RAST._IType _i_underlying;
    public Type_Pointer(RAST._IType underlying) : base() {
      this._i_underlying = underlying;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_Pointer(_i_underlying);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_Pointer;
      return oth != null && object.Equals(this._i_underlying, oth._i_underlying);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 17;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.Pointer";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ")";
      return s;
    }
  }
  public class Type_PointerMut : Type {
    public readonly RAST._IType _i_underlying;
    public Type_PointerMut(RAST._IType underlying) : base() {
      this._i_underlying = underlying;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_PointerMut(_i_underlying);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_PointerMut;
      return oth != null && object.Equals(this._i_underlying, oth._i_underlying);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 18;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.PointerMut";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ")";
      return s;
    }
  }
  public class Type_ImplType : Type {
    public readonly RAST._IType _i_underlying;
    public Type_ImplType(RAST._IType underlying) : base() {
      this._i_underlying = underlying;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_ImplType(_i_underlying);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_ImplType;
      return oth != null && object.Equals(this._i_underlying, oth._i_underlying);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 19;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.ImplType";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ")";
      return s;
    }
  }
  public class Type_DynType : Type {
    public readonly RAST._IType _i_underlying;
    public Type_DynType(RAST._IType underlying) : base() {
      this._i_underlying = underlying;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_DynType(_i_underlying);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_DynType;
      return oth != null && object.Equals(this._i_underlying, oth._i_underlying);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 20;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.DynType";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ")";
      return s;
    }
  }
  public class Type_TupleType : Type {
    public readonly Dafny.ISequence<RAST._IType> _i_arguments;
    public Type_TupleType(Dafny.ISequence<RAST._IType> arguments) : base() {
      this._i_arguments = arguments;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_TupleType(_i_arguments);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_TupleType;
      return oth != null && object.Equals(this._i_arguments, oth._i_arguments);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 21;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_arguments));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.TupleType";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_arguments);
      s += ")";
      return s;
    }
  }
  public class Type_FnType : Type {
    public readonly Dafny.ISequence<RAST._IType> _i_arguments;
    public readonly RAST._IType _i_returnType;
    public Type_FnType(Dafny.ISequence<RAST._IType> arguments, RAST._IType returnType) : base() {
      this._i_arguments = arguments;
      this._i_returnType = returnType;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_FnType(_i_arguments, _i_returnType);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_FnType;
      return oth != null && object.Equals(this._i_arguments, oth._i_arguments) && object.Equals(this._i_returnType, oth._i_returnType);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 22;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_arguments));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_returnType));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.FnType";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_arguments);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_returnType);
      s += ")";
      return s;
    }
  }
  public class Type_IntersectionType : Type {
    public readonly RAST._IType _i_left;
    public readonly RAST._IType _i_right;
    public Type_IntersectionType(RAST._IType left, RAST._IType right) : base() {
      this._i_left = left;
      this._i_right = right;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_IntersectionType(_i_left, _i_right);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_IntersectionType;
      return oth != null && object.Equals(this._i_left, oth._i_left) && object.Equals(this._i_right, oth._i_right);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 23;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_left));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_right));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.IntersectionType";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_left);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_right);
      s += ")";
      return s;
    }
  }
  public class Type_Array : Type {
    public readonly RAST._IType _i_underlying;
    public readonly Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _i_size;
    public Type_Array(RAST._IType underlying, Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> size) : base() {
      this._i_underlying = underlying;
      this._i_size = size;
    }
    public override _IType DowncastClone() {
      if (this is _IType dt) { return dt; }
      return new Type_Array(_i_underlying, _i_size);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Type_Array;
      return oth != null && object.Equals(this._i_underlying, oth._i_underlying) && object.Equals(this._i_size, oth._i_size);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 24;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_size));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Type.Array";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_size);
      s += ")";
      return s;
    }
  }

  public interface _ITrait {
    bool is_Trait { get; }
    Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams { get; }
    RAST._IType dtor_tpe { get; }
    Dafny.ISequence<Dafny.Rune> dtor_where { get; }
    Dafny.ISequence<RAST._IImplMember> dtor_body { get; }
    _ITrait DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class Trait : _ITrait {
    public readonly Dafny.ISequence<RAST._ITypeParamDecl> _i_typeParams;
    public readonly RAST._IType _i_tpe;
    public readonly Dafny.ISequence<Dafny.Rune> _i_where;
    public readonly Dafny.ISequence<RAST._IImplMember> _i_body;
    public Trait(Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IType tpe, Dafny.ISequence<Dafny.Rune> @where, Dafny.ISequence<RAST._IImplMember> body) {
      this._i_typeParams = typeParams;
      this._i_tpe = tpe;
      this._i_where = @where;
      this._i_body = body;
    }
    public _ITrait DowncastClone() {
      if (this is _ITrait dt) { return dt; }
      return new Trait(_i_typeParams, _i_tpe, _i_where, _i_body);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Trait;
      return oth != null && object.Equals(this._i_typeParams, oth._i_typeParams) && object.Equals(this._i_tpe, oth._i_tpe) && object.Equals(this._i_where, oth._i_where) && object.Equals(this._i_body, oth._i_body);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_typeParams));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tpe));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_where));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_body));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Trait.Trait";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_typeParams);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_tpe);
      s += ", ";
      s += this._i_where.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_body);
      s += ")";
      return s;
    }
    private static readonly RAST._ITrait theDefault = create(Dafny.Sequence<RAST._ITypeParamDecl>.Empty, RAST.Type.Default(), Dafny.Sequence<Dafny.Rune>.Empty, Dafny.Sequence<RAST._IImplMember>.Empty);
    public static RAST._ITrait Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._ITrait> _TYPE = new Dafny.TypeDescriptor<RAST._ITrait>(RAST.Trait.Default());
    public static Dafny.TypeDescriptor<RAST._ITrait> _TypeDescriptor() {
      return _TYPE;
    }
    public static _ITrait create(Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IType tpe, Dafny.ISequence<Dafny.Rune> @where, Dafny.ISequence<RAST._IImplMember> body) {
      return new Trait(typeParams, tpe, @where, body);
    }
    public static _ITrait create_Trait(Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IType tpe, Dafny.ISequence<Dafny.Rune> @where, Dafny.ISequence<RAST._IImplMember> body) {
      return create(typeParams, tpe, @where, body);
    }
    public bool is_Trait { get { return true; } }
    public Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams {
      get {
        return this._i_typeParams;
      }
    }
    public RAST._IType dtor_tpe {
      get {
        return this._i_tpe;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_where {
      get {
        return this._i_where;
      }
    }
    public Dafny.ISequence<RAST._IImplMember> dtor_body {
      get {
        return this._i_body;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("pub trait "), RAST.TypeParamDecl.ToStringMultiple((this).dtor_typeParams, ind)), ((this).dtor_tpe)._ToString(ind)), ((!((this).dtor_where).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind), RAST.__default.IND), (this).dtor_where)) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {")), RAST.__default.SeqToString<RAST._IImplMember>((this).dtor_body, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IImplMember, Dafny.ISequence<Dafny.Rune>>>>((_1203_ind) => ((System.Func<RAST._IImplMember, Dafny.ISequence<Dafny.Rune>>)((_1204_member) => {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), _1203_ind), RAST.__default.IND), (_1204_member)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_1203_ind, RAST.__default.IND)));
      })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), (((new BigInteger(((this).dtor_body).Count)).Sign == 0) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")) : (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind)))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
    }
  }

  public interface _IImpl {
    bool is_ImplFor { get; }
    bool is_Impl { get; }
    Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams { get; }
    RAST._IType dtor_tpe { get; }
    RAST._IType dtor_forType { get; }
    Dafny.ISequence<Dafny.Rune> dtor_where { get; }
    Dafny.ISequence<RAST._IImplMember> dtor_body { get; }
    _IImpl DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public abstract class Impl : _IImpl {
    public Impl() {
    }
    private static readonly RAST._IImpl theDefault = create_ImplFor(Dafny.Sequence<RAST._ITypeParamDecl>.Empty, RAST.Type.Default(), RAST.Type.Default(), Dafny.Sequence<Dafny.Rune>.Empty, Dafny.Sequence<RAST._IImplMember>.Empty);
    public static RAST._IImpl Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IImpl> _TYPE = new Dafny.TypeDescriptor<RAST._IImpl>(RAST.Impl.Default());
    public static Dafny.TypeDescriptor<RAST._IImpl> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IImpl create_ImplFor(Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IType tpe, RAST._IType forType, Dafny.ISequence<Dafny.Rune> @where, Dafny.ISequence<RAST._IImplMember> body) {
      return new Impl_ImplFor(typeParams, tpe, forType, @where, body);
    }
    public static _IImpl create_Impl(Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IType tpe, Dafny.ISequence<Dafny.Rune> @where, Dafny.ISequence<RAST._IImplMember> body) {
      return new Impl_Impl(typeParams, tpe, @where, body);
    }
    public bool is_ImplFor { get { return this is Impl_ImplFor; } }
    public bool is_Impl { get { return this is Impl_Impl; } }
    public Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams {
      get {
        var d = this;
        if (d is Impl_ImplFor) { return ((Impl_ImplFor)d)._i_typeParams; }
        return ((Impl_Impl)d)._i_typeParams;
      }
    }
    public RAST._IType dtor_tpe {
      get {
        var d = this;
        if (d is Impl_ImplFor) { return ((Impl_ImplFor)d)._i_tpe; }
        return ((Impl_Impl)d)._i_tpe;
      }
    }
    public RAST._IType dtor_forType {
      get {
        var d = this;
        return ((Impl_ImplFor)d)._i_forType;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_where {
      get {
        var d = this;
        if (d is Impl_ImplFor) { return ((Impl_ImplFor)d)._i_where; }
        return ((Impl_Impl)d)._i_where;
      }
    }
    public Dafny.ISequence<RAST._IImplMember> dtor_body {
      get {
        var d = this;
        if (d is Impl_ImplFor) { return ((Impl_ImplFor)d)._i_body; }
        return ((Impl_Impl)d)._i_body;
      }
    }
    public abstract _IImpl DowncastClone();
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("impl"), RAST.TypeParamDecl.ToStringMultiple((this).dtor_typeParams, ind)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" ")), ((this).dtor_tpe)._ToString(ind)), (((this).is_ImplFor) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind), RAST.__default.IND), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("for ")), ((this).dtor_forType)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND)))) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), ((!((this).dtor_where).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind), RAST.__default.IND), (this).dtor_where)) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {")), RAST.__default.SeqToString<RAST._IImplMember>((this).dtor_body, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IImplMember, Dafny.ISequence<Dafny.Rune>>>>((_1205_ind) => ((System.Func<RAST._IImplMember, Dafny.ISequence<Dafny.Rune>>)((_1206_member) => {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), _1205_ind), RAST.__default.IND), (_1206_member)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_1205_ind, RAST.__default.IND)));
      })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), (((new BigInteger(((this).dtor_body).Count)).Sign == 0) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")) : (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind)))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
    }
  }
  public class Impl_ImplFor : Impl {
    public readonly Dafny.ISequence<RAST._ITypeParamDecl> _i_typeParams;
    public readonly RAST._IType _i_tpe;
    public readonly RAST._IType _i_forType;
    public readonly Dafny.ISequence<Dafny.Rune> _i_where;
    public readonly Dafny.ISequence<RAST._IImplMember> _i_body;
    public Impl_ImplFor(Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IType tpe, RAST._IType forType, Dafny.ISequence<Dafny.Rune> @where, Dafny.ISequence<RAST._IImplMember> body) : base() {
      this._i_typeParams = typeParams;
      this._i_tpe = tpe;
      this._i_forType = forType;
      this._i_where = @where;
      this._i_body = body;
    }
    public override _IImpl DowncastClone() {
      if (this is _IImpl dt) { return dt; }
      return new Impl_ImplFor(_i_typeParams, _i_tpe, _i_forType, _i_where, _i_body);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Impl_ImplFor;
      return oth != null && object.Equals(this._i_typeParams, oth._i_typeParams) && object.Equals(this._i_tpe, oth._i_tpe) && object.Equals(this._i_forType, oth._i_forType) && object.Equals(this._i_where, oth._i_where) && object.Equals(this._i_body, oth._i_body);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_typeParams));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tpe));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_forType));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_where));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_body));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Impl.ImplFor";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_typeParams);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_tpe);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_forType);
      s += ", ";
      s += this._i_where.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_body);
      s += ")";
      return s;
    }
  }
  public class Impl_Impl : Impl {
    public readonly Dafny.ISequence<RAST._ITypeParamDecl> _i_typeParams;
    public readonly RAST._IType _i_tpe;
    public readonly Dafny.ISequence<Dafny.Rune> _i_where;
    public readonly Dafny.ISequence<RAST._IImplMember> _i_body;
    public Impl_Impl(Dafny.ISequence<RAST._ITypeParamDecl> typeParams, RAST._IType tpe, Dafny.ISequence<Dafny.Rune> @where, Dafny.ISequence<RAST._IImplMember> body) : base() {
      this._i_typeParams = typeParams;
      this._i_tpe = tpe;
      this._i_where = @where;
      this._i_body = body;
    }
    public override _IImpl DowncastClone() {
      if (this is _IImpl dt) { return dt; }
      return new Impl_Impl(_i_typeParams, _i_tpe, _i_where, _i_body);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Impl_Impl;
      return oth != null && object.Equals(this._i_typeParams, oth._i_typeParams) && object.Equals(this._i_tpe, oth._i_tpe) && object.Equals(this._i_where, oth._i_where) && object.Equals(this._i_body, oth._i_body);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_typeParams));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tpe));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_where));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_body));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Impl.Impl";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_typeParams);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_tpe);
      s += ", ";
      s += this._i_where.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_body);
      s += ")";
      return s;
    }
  }

  public interface _IImplMember {
    bool is_RawImplMember { get; }
    bool is_FnDecl { get; }
    Dafny.ISequence<Dafny.Rune> dtor_content { get; }
    RAST._IVisibility dtor_pub { get; }
    RAST._IFn dtor_fun { get; }
    _IImplMember DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public abstract class ImplMember : _IImplMember {
    public ImplMember() {
    }
    private static readonly RAST._IImplMember theDefault = create_RawImplMember(Dafny.Sequence<Dafny.Rune>.Empty);
    public static RAST._IImplMember Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IImplMember> _TYPE = new Dafny.TypeDescriptor<RAST._IImplMember>(RAST.ImplMember.Default());
    public static Dafny.TypeDescriptor<RAST._IImplMember> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IImplMember create_RawImplMember(Dafny.ISequence<Dafny.Rune> content) {
      return new ImplMember_RawImplMember(content);
    }
    public static _IImplMember create_FnDecl(RAST._IVisibility pub, RAST._IFn fun) {
      return new ImplMember_FnDecl(pub, fun);
    }
    public bool is_RawImplMember { get { return this is ImplMember_RawImplMember; } }
    public bool is_FnDecl { get { return this is ImplMember_FnDecl; } }
    public Dafny.ISequence<Dafny.Rune> dtor_content {
      get {
        var d = this;
        return ((ImplMember_RawImplMember)d)._i_content;
      }
    }
    public RAST._IVisibility dtor_pub {
      get {
        var d = this;
        return ((ImplMember_FnDecl)d)._i_pub;
      }
    }
    public RAST._IFn dtor_fun {
      get {
        var d = this;
        return ((ImplMember_FnDecl)d)._i_fun;
      }
    }
    public abstract _IImplMember DowncastClone();
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      if ((this).is_FnDecl) {
        return Dafny.Sequence<Dafny.Rune>.Concat(((this).dtor_pub)._ToString(), ((this).dtor_fun)._ToString(ind));
      } else {
        return (this).dtor_content;
      }
    }
  }
  public class ImplMember_RawImplMember : ImplMember {
    public readonly Dafny.ISequence<Dafny.Rune> _i_content;
    public ImplMember_RawImplMember(Dafny.ISequence<Dafny.Rune> content) : base() {
      this._i_content = content;
    }
    public override _IImplMember DowncastClone() {
      if (this is _IImplMember dt) { return dt; }
      return new ImplMember_RawImplMember(_i_content);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ImplMember_RawImplMember;
      return oth != null && object.Equals(this._i_content, oth._i_content);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_content));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ImplMember.RawImplMember";
      s += "(";
      s += this._i_content.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class ImplMember_FnDecl : ImplMember {
    public readonly RAST._IVisibility _i_pub;
    public readonly RAST._IFn _i_fun;
    public ImplMember_FnDecl(RAST._IVisibility pub, RAST._IFn fun) : base() {
      this._i_pub = pub;
      this._i_fun = fun;
    }
    public override _IImplMember DowncastClone() {
      if (this is _IImplMember dt) { return dt; }
      return new ImplMember_FnDecl(_i_pub, _i_fun);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.ImplMember_FnDecl;
      return oth != null && object.Equals(this._i_pub, oth._i_pub) && object.Equals(this._i_fun, oth._i_fun);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_pub));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_fun));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.ImplMember.FnDecl";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_pub);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_fun);
      s += ")";
      return s;
    }
  }

  public interface _IVisibility {
    bool is_PUB { get; }
    bool is_PRIV { get; }
    _IVisibility DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString();
  }
  public abstract class Visibility : _IVisibility {
    public Visibility() {
    }
    private static readonly RAST._IVisibility theDefault = create_PUB();
    public static RAST._IVisibility Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IVisibility> _TYPE = new Dafny.TypeDescriptor<RAST._IVisibility>(RAST.Visibility.Default());
    public static Dafny.TypeDescriptor<RAST._IVisibility> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IVisibility create_PUB() {
      return new Visibility_PUB();
    }
    public static _IVisibility create_PRIV() {
      return new Visibility_PRIV();
    }
    public bool is_PUB { get { return this is Visibility_PUB; } }
    public bool is_PRIV { get { return this is Visibility_PRIV; } }
    public static System.Collections.Generic.IEnumerable<_IVisibility> AllSingletonConstructors {
      get {
        yield return Visibility.create_PUB();
        yield return Visibility.create_PRIV();
      }
    }
    public abstract _IVisibility DowncastClone();
    public Dafny.ISequence<Dafny.Rune> _ToString() {
      if ((this).is_PUB) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("pub ");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
      }
    }
  }
  public class Visibility_PUB : Visibility {
    public Visibility_PUB() : base() {
    }
    public override _IVisibility DowncastClone() {
      if (this is _IVisibility dt) { return dt; }
      return new Visibility_PUB();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Visibility_PUB;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Visibility.PUB";
      return s;
    }
  }
  public class Visibility_PRIV : Visibility {
    public Visibility_PRIV() : base() {
    }
    public override _IVisibility DowncastClone() {
      if (this is _IVisibility dt) { return dt; }
      return new Visibility_PRIV();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Visibility_PRIV;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Visibility.PRIV";
      return s;
    }
  }

  public interface _IFormal {
    bool is_Formal { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    RAST._IType dtor_tpe { get; }
    _IFormal DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class Formal : _IFormal {
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public readonly RAST._IType _i_tpe;
    public Formal(Dafny.ISequence<Dafny.Rune> name, RAST._IType tpe) {
      this._i_name = name;
      this._i_tpe = tpe;
    }
    public _IFormal DowncastClone() {
      if (this is _IFormal dt) { return dt; }
      return new Formal(_i_name, _i_tpe);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Formal;
      return oth != null && object.Equals(this._i_name, oth._i_name) && object.Equals(this._i_tpe, oth._i_tpe);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tpe));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Formal.Formal";
      s += "(";
      s += this._i_name.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_tpe);
      s += ")";
      return s;
    }
    private static readonly RAST._IFormal theDefault = create(Dafny.Sequence<Dafny.Rune>.Empty, RAST.Type.Default());
    public static RAST._IFormal Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IFormal> _TYPE = new Dafny.TypeDescriptor<RAST._IFormal>(RAST.Formal.Default());
    public static Dafny.TypeDescriptor<RAST._IFormal> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IFormal create(Dafny.ISequence<Dafny.Rune> name, RAST._IType tpe) {
      return new Formal(name, tpe);
    }
    public static _IFormal create_Formal(Dafny.ISequence<Dafny.Rune> name, RAST._IType tpe) {
      return create(name, tpe);
    }
    public bool is_Formal { get { return true; } }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        return this._i_name;
      }
    }
    public RAST._IType dtor_tpe {
      get {
        return this._i_tpe;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      if ((((this).dtor_name).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))) && (object.Equals((this).dtor_tpe, RAST.Type.create_SelfOwned()))) {
        return (this).dtor_name;
      } else if ((((this).dtor_name).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))) && (object.Equals((this).dtor_tpe, RAST.__default.SelfBorrowed))) {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), (this).dtor_name);
      } else if ((((this).dtor_name).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))) && (object.Equals((this).dtor_tpe, RAST.__default.SelfBorrowedMut))) {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&mut "), (this).dtor_name);
      } else {
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat((this).dtor_name, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(": ")), ((this).dtor_tpe)._ToString(ind));
      }
    }
    public static RAST._IFormal selfBorrowed { get {
      return RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"), RAST.__default.SelfBorrowed);
    } }
    public static RAST._IFormal selfOwned { get {
      return RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"), RAST.Type.create_SelfOwned());
    } }
    public static RAST._IFormal selfBorrowedMut { get {
      return RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"), RAST.__default.SelfBorrowedMut);
    } }
  }

  public interface _IPattern {
    bool is_RawPattern { get; }
    Dafny.ISequence<Dafny.Rune> dtor_content { get; }
  }
  public class Pattern : _IPattern {
    public readonly Dafny.ISequence<Dafny.Rune> _i_content;
    public Pattern(Dafny.ISequence<Dafny.Rune> content) {
      this._i_content = content;
    }
    public static Dafny.ISequence<Dafny.Rune> DowncastClone(Dafny.ISequence<Dafny.Rune> _this) {
      return _this;
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Pattern;
      return oth != null && object.Equals(this._i_content, oth._i_content);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_content));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Pattern.RawPattern";
      s += "(";
      s += this._i_content.ToVerbatimString(true);
      s += ")";
      return s;
    }
    private static readonly Dafny.ISequence<Dafny.Rune> theDefault = Dafny.Sequence<Dafny.Rune>.Empty;
    public static Dafny.ISequence<Dafny.Rune> Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<Dafny.ISequence<Dafny.Rune>> _TYPE = new Dafny.TypeDescriptor<Dafny.ISequence<Dafny.Rune>>(Dafny.Sequence<Dafny.Rune>.Empty);
    public static Dafny.TypeDescriptor<Dafny.ISequence<Dafny.Rune>> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IPattern create(Dafny.ISequence<Dafny.Rune> content) {
      return new Pattern(content);
    }
    public static _IPattern create_RawPattern(Dafny.ISequence<Dafny.Rune> content) {
      return create(content);
    }
    public bool is_RawPattern { get { return true; } }
    public Dafny.ISequence<Dafny.Rune> dtor_content {
      get {
        return this._i_content;
      }
    }
    public static Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> _this, Dafny.ISequence<Dafny.Rune> ind) {
      return (_this);
    }
  }

  public interface _IMatchCase {
    bool is_MatchCase { get; }
    Dafny.ISequence<Dafny.Rune> dtor_pattern { get; }
    RAST._IExpr dtor_rhs { get; }
    _IMatchCase DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class MatchCase : _IMatchCase {
    public readonly Dafny.ISequence<Dafny.Rune> _i_pattern;
    public readonly RAST._IExpr _i_rhs;
    public MatchCase(Dafny.ISequence<Dafny.Rune> pattern, RAST._IExpr rhs) {
      this._i_pattern = pattern;
      this._i_rhs = rhs;
    }
    public _IMatchCase DowncastClone() {
      if (this is _IMatchCase dt) { return dt; }
      return new MatchCase(_i_pattern, _i_rhs);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.MatchCase;
      return oth != null && object.Equals(this._i_pattern, oth._i_pattern) && object.Equals(this._i_rhs, oth._i_rhs);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_pattern));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_rhs));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.MatchCase.MatchCase";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_pattern);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_rhs);
      s += ")";
      return s;
    }
    private static readonly RAST._IMatchCase theDefault = create(Dafny.Sequence<Dafny.Rune>.Empty, RAST.Expr.Default());
    public static RAST._IMatchCase Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IMatchCase> _TYPE = new Dafny.TypeDescriptor<RAST._IMatchCase>(RAST.MatchCase.Default());
    public static Dafny.TypeDescriptor<RAST._IMatchCase> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IMatchCase create(Dafny.ISequence<Dafny.Rune> pattern, RAST._IExpr rhs) {
      return new MatchCase(pattern, rhs);
    }
    public static _IMatchCase create_MatchCase(Dafny.ISequence<Dafny.Rune> pattern, RAST._IExpr rhs) {
      return create(pattern, rhs);
    }
    public bool is_MatchCase { get { return true; } }
    public Dafny.ISequence<Dafny.Rune> dtor_pattern {
      get {
        return this._i_pattern;
      }
    }
    public RAST._IExpr dtor_rhs {
      get {
        return this._i_rhs;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      Dafny.ISequence<Dafny.Rune> _1207_newIndent = ((((this).dtor_rhs).is_Block) ? (ind) : (Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND)));
      Dafny.ISequence<Dafny.Rune> _1208_rhsString = ((this).dtor_rhs)._ToString(_1207_newIndent);
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(RAST.Pattern._ToString((this).dtor_pattern, ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" =>")), ((((_1208_rhsString).Contains(new Dafny.Rune('\n'))) && (((_1208_rhsString).Select(BigInteger.Zero)) != (new Dafny.Rune('{')))) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind), RAST.__default.IND), _1208_rhsString)) : (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" "), _1208_rhsString))));
    }
  }

  public interface _IAssignIdentifier {
    bool is_AssignIdentifier { get; }
    Dafny.ISequence<Dafny.Rune> dtor_identifier { get; }
    RAST._IExpr dtor_rhs { get; }
    _IAssignIdentifier DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class AssignIdentifier : _IAssignIdentifier {
    public readonly Dafny.ISequence<Dafny.Rune> _i_identifier;
    public readonly RAST._IExpr _i_rhs;
    public AssignIdentifier(Dafny.ISequence<Dafny.Rune> identifier, RAST._IExpr rhs) {
      this._i_identifier = identifier;
      this._i_rhs = rhs;
    }
    public _IAssignIdentifier DowncastClone() {
      if (this is _IAssignIdentifier dt) { return dt; }
      return new AssignIdentifier(_i_identifier, _i_rhs);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.AssignIdentifier;
      return oth != null && object.Equals(this._i_identifier, oth._i_identifier) && object.Equals(this._i_rhs, oth._i_rhs);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_identifier));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_rhs));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.AssignIdentifier.AssignIdentifier";
      s += "(";
      s += this._i_identifier.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_rhs);
      s += ")";
      return s;
    }
    private static readonly RAST._IAssignIdentifier theDefault = create(Dafny.Sequence<Dafny.Rune>.Empty, RAST.Expr.Default());
    public static RAST._IAssignIdentifier Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IAssignIdentifier> _TYPE = new Dafny.TypeDescriptor<RAST._IAssignIdentifier>(RAST.AssignIdentifier.Default());
    public static Dafny.TypeDescriptor<RAST._IAssignIdentifier> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IAssignIdentifier create(Dafny.ISequence<Dafny.Rune> identifier, RAST._IExpr rhs) {
      return new AssignIdentifier(identifier, rhs);
    }
    public static _IAssignIdentifier create_AssignIdentifier(Dafny.ISequence<Dafny.Rune> identifier, RAST._IExpr rhs) {
      return create(identifier, rhs);
    }
    public bool is_AssignIdentifier { get { return true; } }
    public Dafny.ISequence<Dafny.Rune> dtor_identifier {
      get {
        return this._i_identifier;
      }
    }
    public RAST._IExpr dtor_rhs {
      get {
        return this._i_rhs;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat((this).dtor_identifier, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(": ")), ((this).dtor_rhs)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND)));
    }
  }

  public interface _IDeclareType {
    bool is_MUT { get; }
    bool is_CONST { get; }
    _IDeclareType DowncastClone();
  }
  public abstract class DeclareType : _IDeclareType {
    public DeclareType() {
    }
    private static readonly RAST._IDeclareType theDefault = create_MUT();
    public static RAST._IDeclareType Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IDeclareType> _TYPE = new Dafny.TypeDescriptor<RAST._IDeclareType>(RAST.DeclareType.Default());
    public static Dafny.TypeDescriptor<RAST._IDeclareType> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IDeclareType create_MUT() {
      return new DeclareType_MUT();
    }
    public static _IDeclareType create_CONST() {
      return new DeclareType_CONST();
    }
    public bool is_MUT { get { return this is DeclareType_MUT; } }
    public bool is_CONST { get { return this is DeclareType_CONST; } }
    public static System.Collections.Generic.IEnumerable<_IDeclareType> AllSingletonConstructors {
      get {
        yield return DeclareType.create_MUT();
        yield return DeclareType.create_CONST();
      }
    }
    public abstract _IDeclareType DowncastClone();
  }
  public class DeclareType_MUT : DeclareType {
    public DeclareType_MUT() : base() {
    }
    public override _IDeclareType DowncastClone() {
      if (this is _IDeclareType dt) { return dt; }
      return new DeclareType_MUT();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.DeclareType_MUT;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.DeclareType.MUT";
      return s;
    }
  }
  public class DeclareType_CONST : DeclareType {
    public DeclareType_CONST() : base() {
    }
    public override _IDeclareType DowncastClone() {
      if (this is _IDeclareType dt) { return dt; }
      return new DeclareType_CONST();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.DeclareType_CONST;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.DeclareType.CONST";
      return s;
    }
  }

  public interface _IAssociativity {
    bool is_LeftToRight { get; }
    bool is_RightToLeft { get; }
    bool is_RequiresParentheses { get; }
    _IAssociativity DowncastClone();
  }
  public abstract class Associativity : _IAssociativity {
    public Associativity() {
    }
    private static readonly RAST._IAssociativity theDefault = create_LeftToRight();
    public static RAST._IAssociativity Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IAssociativity> _TYPE = new Dafny.TypeDescriptor<RAST._IAssociativity>(RAST.Associativity.Default());
    public static Dafny.TypeDescriptor<RAST._IAssociativity> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IAssociativity create_LeftToRight() {
      return new Associativity_LeftToRight();
    }
    public static _IAssociativity create_RightToLeft() {
      return new Associativity_RightToLeft();
    }
    public static _IAssociativity create_RequiresParentheses() {
      return new Associativity_RequiresParentheses();
    }
    public bool is_LeftToRight { get { return this is Associativity_LeftToRight; } }
    public bool is_RightToLeft { get { return this is Associativity_RightToLeft; } }
    public bool is_RequiresParentheses { get { return this is Associativity_RequiresParentheses; } }
    public static System.Collections.Generic.IEnumerable<_IAssociativity> AllSingletonConstructors {
      get {
        yield return Associativity.create_LeftToRight();
        yield return Associativity.create_RightToLeft();
        yield return Associativity.create_RequiresParentheses();
      }
    }
    public abstract _IAssociativity DowncastClone();
  }
  public class Associativity_LeftToRight : Associativity {
    public Associativity_LeftToRight() : base() {
    }
    public override _IAssociativity DowncastClone() {
      if (this is _IAssociativity dt) { return dt; }
      return new Associativity_LeftToRight();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Associativity_LeftToRight;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Associativity.LeftToRight";
      return s;
    }
  }
  public class Associativity_RightToLeft : Associativity {
    public Associativity_RightToLeft() : base() {
    }
    public override _IAssociativity DowncastClone() {
      if (this is _IAssociativity dt) { return dt; }
      return new Associativity_RightToLeft();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Associativity_RightToLeft;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Associativity.RightToLeft";
      return s;
    }
  }
  public class Associativity_RequiresParentheses : Associativity {
    public Associativity_RequiresParentheses() : base() {
    }
    public override _IAssociativity DowncastClone() {
      if (this is _IAssociativity dt) { return dt; }
      return new Associativity_RequiresParentheses();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Associativity_RequiresParentheses;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 2;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Associativity.RequiresParentheses";
      return s;
    }
  }

  public interface _IPrintingInfo {
    bool is_UnknownPrecedence { get; }
    bool is_Precedence { get; }
    bool is_SuffixPrecedence { get; }
    bool is_PrecedenceAssociativity { get; }
    BigInteger dtor_precedence { get; }
    RAST._IAssociativity dtor_associativity { get; }
    _IPrintingInfo DowncastClone();
    bool NeedParenthesesFor(RAST._IPrintingInfo underlying);
    bool NeedParenthesesForLeft(RAST._IPrintingInfo underlying);
    bool NeedParenthesesForRight(RAST._IPrintingInfo underlying);
  }
  public abstract class PrintingInfo : _IPrintingInfo {
    public PrintingInfo() {
    }
    private static readonly RAST._IPrintingInfo theDefault = create_UnknownPrecedence();
    public static RAST._IPrintingInfo Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IPrintingInfo> _TYPE = new Dafny.TypeDescriptor<RAST._IPrintingInfo>(RAST.PrintingInfo.Default());
    public static Dafny.TypeDescriptor<RAST._IPrintingInfo> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IPrintingInfo create_UnknownPrecedence() {
      return new PrintingInfo_UnknownPrecedence();
    }
    public static _IPrintingInfo create_Precedence(BigInteger precedence) {
      return new PrintingInfo_Precedence(precedence);
    }
    public static _IPrintingInfo create_SuffixPrecedence(BigInteger precedence) {
      return new PrintingInfo_SuffixPrecedence(precedence);
    }
    public static _IPrintingInfo create_PrecedenceAssociativity(BigInteger precedence, RAST._IAssociativity associativity) {
      return new PrintingInfo_PrecedenceAssociativity(precedence, associativity);
    }
    public bool is_UnknownPrecedence { get { return this is PrintingInfo_UnknownPrecedence; } }
    public bool is_Precedence { get { return this is PrintingInfo_Precedence; } }
    public bool is_SuffixPrecedence { get { return this is PrintingInfo_SuffixPrecedence; } }
    public bool is_PrecedenceAssociativity { get { return this is PrintingInfo_PrecedenceAssociativity; } }
    public BigInteger dtor_precedence {
      get {
        var d = this;
        if (d is PrintingInfo_Precedence) { return ((PrintingInfo_Precedence)d)._i_precedence; }
        if (d is PrintingInfo_SuffixPrecedence) { return ((PrintingInfo_SuffixPrecedence)d)._i_precedence; }
        return ((PrintingInfo_PrecedenceAssociativity)d)._i_precedence;
      }
    }
    public RAST._IAssociativity dtor_associativity {
      get {
        var d = this;
        return ((PrintingInfo_PrecedenceAssociativity)d)._i_associativity;
      }
    }
    public abstract _IPrintingInfo DowncastClone();
    public bool NeedParenthesesFor(RAST._IPrintingInfo underlying) {
      if ((this).is_UnknownPrecedence) {
        return true;
      } else if ((underlying).is_UnknownPrecedence) {
        return true;
      } else if (((this).dtor_precedence) <= ((underlying).dtor_precedence)) {
        return true;
      } else {
        return false;
      }
    }
    public bool NeedParenthesesForLeft(RAST._IPrintingInfo underlying) {
      if ((this).is_UnknownPrecedence) {
        return true;
      } else if ((underlying).is_UnknownPrecedence) {
        return true;
      } else if (((this).dtor_precedence) <= ((underlying).dtor_precedence)) {
        return ((((this).dtor_precedence) < ((underlying).dtor_precedence)) || (!((this).is_PrecedenceAssociativity))) || (!(((this).dtor_associativity).is_LeftToRight));
      } else {
        return false;
      }
    }
    public bool NeedParenthesesForRight(RAST._IPrintingInfo underlying) {
      if ((this).is_UnknownPrecedence) {
        return true;
      } else if ((underlying).is_UnknownPrecedence) {
        return true;
      } else if (((this).dtor_precedence) <= ((underlying).dtor_precedence)) {
        return ((((this).dtor_precedence) < ((underlying).dtor_precedence)) || (!((this).is_PrecedenceAssociativity))) || (!(((this).dtor_associativity).is_RightToLeft));
      } else {
        return false;
      }
    }
  }
  public class PrintingInfo_UnknownPrecedence : PrintingInfo {
    public PrintingInfo_UnknownPrecedence() : base() {
    }
    public override _IPrintingInfo DowncastClone() {
      if (this is _IPrintingInfo dt) { return dt; }
      return new PrintingInfo_UnknownPrecedence();
    }
    public override bool Equals(object other) {
      var oth = other as RAST.PrintingInfo_UnknownPrecedence;
      return oth != null;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.PrintingInfo.UnknownPrecedence";
      return s;
    }
  }
  public class PrintingInfo_Precedence : PrintingInfo {
    public readonly BigInteger _i_precedence;
    public PrintingInfo_Precedence(BigInteger precedence) : base() {
      this._i_precedence = precedence;
    }
    public override _IPrintingInfo DowncastClone() {
      if (this is _IPrintingInfo dt) { return dt; }
      return new PrintingInfo_Precedence(_i_precedence);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.PrintingInfo_Precedence;
      return oth != null && this._i_precedence == oth._i_precedence;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_precedence));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.PrintingInfo.Precedence";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_precedence);
      s += ")";
      return s;
    }
  }
  public class PrintingInfo_SuffixPrecedence : PrintingInfo {
    public readonly BigInteger _i_precedence;
    public PrintingInfo_SuffixPrecedence(BigInteger precedence) : base() {
      this._i_precedence = precedence;
    }
    public override _IPrintingInfo DowncastClone() {
      if (this is _IPrintingInfo dt) { return dt; }
      return new PrintingInfo_SuffixPrecedence(_i_precedence);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.PrintingInfo_SuffixPrecedence;
      return oth != null && this._i_precedence == oth._i_precedence;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 2;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_precedence));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.PrintingInfo.SuffixPrecedence";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_precedence);
      s += ")";
      return s;
    }
  }
  public class PrintingInfo_PrecedenceAssociativity : PrintingInfo {
    public readonly BigInteger _i_precedence;
    public readonly RAST._IAssociativity _i_associativity;
    public PrintingInfo_PrecedenceAssociativity(BigInteger precedence, RAST._IAssociativity associativity) : base() {
      this._i_precedence = precedence;
      this._i_associativity = associativity;
    }
    public override _IPrintingInfo DowncastClone() {
      if (this is _IPrintingInfo dt) { return dt; }
      return new PrintingInfo_PrecedenceAssociativity(_i_precedence, _i_associativity);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.PrintingInfo_PrecedenceAssociativity;
      return oth != null && this._i_precedence == oth._i_precedence && object.Equals(this._i_associativity, oth._i_associativity);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 3;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_precedence));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_associativity));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.PrintingInfo.PrecedenceAssociativity";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_precedence);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_associativity);
      s += ")";
      return s;
    }
  }

  public interface _IAssignLhs {
    bool is_LocalVar { get; }
    bool is_SelectMember { get; }
    bool is_ExtractTuple { get; }
    bool is_Index { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    RAST._IExpr dtor_on { get; }
    Dafny.ISequence<Dafny.Rune> dtor_field { get; }
    Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_names { get; }
    RAST._IExpr dtor_expr { get; }
    Dafny.ISequence<RAST._IExpr> dtor_indices { get; }
    _IAssignLhs DowncastClone();
  }
  public abstract class AssignLhs : _IAssignLhs {
    public AssignLhs() {
    }
    private static readonly RAST._IAssignLhs theDefault = create_LocalVar(Dafny.Sequence<Dafny.Rune>.Empty);
    public static RAST._IAssignLhs Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IAssignLhs> _TYPE = new Dafny.TypeDescriptor<RAST._IAssignLhs>(RAST.AssignLhs.Default());
    public static Dafny.TypeDescriptor<RAST._IAssignLhs> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IAssignLhs create_LocalVar(Dafny.ISequence<Dafny.Rune> name) {
      return new AssignLhs_LocalVar(name);
    }
    public static _IAssignLhs create_SelectMember(RAST._IExpr @on, Dafny.ISequence<Dafny.Rune> field) {
      return new AssignLhs_SelectMember(@on, field);
    }
    public static _IAssignLhs create_ExtractTuple(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> names) {
      return new AssignLhs_ExtractTuple(names);
    }
    public static _IAssignLhs create_Index(RAST._IExpr expr, Dafny.ISequence<RAST._IExpr> indices) {
      return new AssignLhs_Index(expr, indices);
    }
    public bool is_LocalVar { get { return this is AssignLhs_LocalVar; } }
    public bool is_SelectMember { get { return this is AssignLhs_SelectMember; } }
    public bool is_ExtractTuple { get { return this is AssignLhs_ExtractTuple; } }
    public bool is_Index { get { return this is AssignLhs_Index; } }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        var d = this;
        return ((AssignLhs_LocalVar)d)._i_name;
      }
    }
    public RAST._IExpr dtor_on {
      get {
        var d = this;
        return ((AssignLhs_SelectMember)d)._i_on;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_field {
      get {
        var d = this;
        return ((AssignLhs_SelectMember)d)._i_field;
      }
    }
    public Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> dtor_names {
      get {
        var d = this;
        return ((AssignLhs_ExtractTuple)d)._i_names;
      }
    }
    public RAST._IExpr dtor_expr {
      get {
        var d = this;
        return ((AssignLhs_Index)d)._i_expr;
      }
    }
    public Dafny.ISequence<RAST._IExpr> dtor_indices {
      get {
        var d = this;
        return ((AssignLhs_Index)d)._i_indices;
      }
    }
    public abstract _IAssignLhs DowncastClone();
  }
  public class AssignLhs_LocalVar : AssignLhs {
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public AssignLhs_LocalVar(Dafny.ISequence<Dafny.Rune> name) : base() {
      this._i_name = name;
    }
    public override _IAssignLhs DowncastClone() {
      if (this is _IAssignLhs dt) { return dt; }
      return new AssignLhs_LocalVar(_i_name);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.AssignLhs_LocalVar;
      return oth != null && object.Equals(this._i_name, oth._i_name);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.AssignLhs.LocalVar";
      s += "(";
      s += this._i_name.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class AssignLhs_SelectMember : AssignLhs {
    public readonly RAST._IExpr _i_on;
    public readonly Dafny.ISequence<Dafny.Rune> _i_field;
    public AssignLhs_SelectMember(RAST._IExpr @on, Dafny.ISequence<Dafny.Rune> field) : base() {
      this._i_on = @on;
      this._i_field = field;
    }
    public override _IAssignLhs DowncastClone() {
      if (this is _IAssignLhs dt) { return dt; }
      return new AssignLhs_SelectMember(_i_on, _i_field);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.AssignLhs_SelectMember;
      return oth != null && object.Equals(this._i_on, oth._i_on) && object.Equals(this._i_field, oth._i_field);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_on));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_field));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.AssignLhs.SelectMember";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_on);
      s += ", ";
      s += this._i_field.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class AssignLhs_ExtractTuple : AssignLhs {
    public readonly Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _i_names;
    public AssignLhs_ExtractTuple(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> names) : base() {
      this._i_names = names;
    }
    public override _IAssignLhs DowncastClone() {
      if (this is _IAssignLhs dt) { return dt; }
      return new AssignLhs_ExtractTuple(_i_names);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.AssignLhs_ExtractTuple;
      return oth != null && object.Equals(this._i_names, oth._i_names);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 2;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_names));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.AssignLhs.ExtractTuple";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_names);
      s += ")";
      return s;
    }
  }
  public class AssignLhs_Index : AssignLhs {
    public readonly RAST._IExpr _i_expr;
    public readonly Dafny.ISequence<RAST._IExpr> _i_indices;
    public AssignLhs_Index(RAST._IExpr expr, Dafny.ISequence<RAST._IExpr> indices) : base() {
      this._i_expr = expr;
      this._i_indices = indices;
    }
    public override _IAssignLhs DowncastClone() {
      if (this is _IAssignLhs dt) { return dt; }
      return new AssignLhs_Index(_i_expr, _i_indices);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.AssignLhs_Index;
      return oth != null && object.Equals(this._i_expr, oth._i_expr) && object.Equals(this._i_indices, oth._i_indices);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 3;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_expr));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_indices));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.AssignLhs.Index";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_expr);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_indices);
      s += ")";
      return s;
    }
  }

  public interface _IExpr {
    bool is_RawExpr { get; }
    bool is_ExprFromType { get; }
    bool is_Identifier { get; }
    bool is_Match { get; }
    bool is_StmtExpr { get; }
    bool is_Block { get; }
    bool is_StructBuild { get; }
    bool is_Tuple { get; }
    bool is_UnaryOp { get; }
    bool is_BinaryOp { get; }
    bool is_TypeAscription { get; }
    bool is_LiteralInt { get; }
    bool is_LiteralBool { get; }
    bool is_LiteralString { get; }
    bool is_DeclareVar { get; }
    bool is_Assign { get; }
    bool is_IfExpr { get; }
    bool is_Loop { get; }
    bool is_For { get; }
    bool is_Labelled { get; }
    bool is_Break { get; }
    bool is_Continue { get; }
    bool is_Return { get; }
    bool is_CallType { get; }
    bool is_Call { get; }
    bool is_Select { get; }
    bool is_MemberSelect { get; }
    bool is_Lambda { get; }
    Dafny.ISequence<Dafny.Rune> dtor_content { get; }
    RAST._IType dtor_tpe { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    RAST._IExpr dtor_matchee { get; }
    Dafny.ISequence<RAST._IMatchCase> dtor_cases { get; }
    RAST._IExpr dtor_stmt { get; }
    RAST._IExpr dtor_rhs { get; }
    RAST._IExpr dtor_underlying { get; }
    Dafny.ISequence<RAST._IAssignIdentifier> dtor_assignments { get; }
    Dafny.ISequence<RAST._IExpr> dtor_arguments { get; }
    Dafny.ISequence<Dafny.Rune> dtor_op1 { get; }
    DAST.Format._IUnaryOpFormat dtor_format { get; }
    Dafny.ISequence<Dafny.Rune> dtor_op2 { get; }
    RAST._IExpr dtor_left { get; }
    RAST._IExpr dtor_right { get; }
    DAST.Format._IBinaryOpFormat dtor_format2 { get; }
    Dafny.ISequence<Dafny.Rune> dtor_value { get; }
    bool dtor_bvalue { get; }
    bool dtor_binary { get; }
    RAST._IDeclareType dtor_declareType { get; }
    Std.Wrappers._IOption<RAST._IType> dtor_optType { get; }
    Std.Wrappers._IOption<RAST._IExpr> dtor_optRhs { get; }
    Std.Wrappers._IOption<RAST._IAssignLhs> dtor_names { get; }
    RAST._IExpr dtor_cond { get; }
    RAST._IExpr dtor_thn { get; }
    RAST._IExpr dtor_els { get; }
    Std.Wrappers._IOption<RAST._IExpr> dtor_optCond { get; }
    RAST._IExpr dtor_range { get; }
    RAST._IExpr dtor_body { get; }
    Dafny.ISequence<Dafny.Rune> dtor_lbl { get; }
    Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> dtor_optLbl { get; }
    Std.Wrappers._IOption<RAST._IExpr> dtor_optExpr { get; }
    RAST._IExpr dtor_obj { get; }
    Dafny.ISequence<RAST._IType> dtor_typeParameters { get; }
    Dafny.ISequence<RAST._IFormal> dtor_params { get; }
    Std.Wrappers._IOption<RAST._IType> dtor_retType { get; }
    _IExpr DowncastClone();
    bool NoExtraSemicolonAfter();
    RAST._IPrintingInfo printingInfo { get; }
    RAST._IExpr Optimize();
    bool LeftRequiresParentheses(RAST._IExpr left);
    _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> LeftParentheses(RAST._IExpr left);
    bool RightRequiresParentheses(RAST._IExpr right);
    _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> RightParentheses(RAST._IExpr right);
    Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> RightMostIdentifier();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
    RAST._IExpr Then(RAST._IExpr rhs2);
    RAST._IExpr Sel(Dafny.ISequence<Dafny.Rune> name);
    RAST._IExpr MSel(Dafny.ISequence<Dafny.Rune> name);
    RAST._IExpr ApplyType(Dafny.ISequence<RAST._IType> typeParameters);
    RAST._IExpr ApplyType1(RAST._IType typeParameter);
    RAST._IExpr Apply(Dafny.ISequence<RAST._IExpr> arguments);
    RAST._IExpr Apply1(RAST._IExpr argument);
    bool IsLhsIdentifier();
    Dafny.ISequence<Dafny.Rune> LhsIdentifierName();
  }
  public abstract class Expr : _IExpr {
    public Expr() {
    }
    private static readonly RAST._IExpr theDefault = create_RawExpr(Dafny.Sequence<Dafny.Rune>.Empty);
    public static RAST._IExpr Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IExpr> _TYPE = new Dafny.TypeDescriptor<RAST._IExpr>(RAST.Expr.Default());
    public static Dafny.TypeDescriptor<RAST._IExpr> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IExpr create_RawExpr(Dafny.ISequence<Dafny.Rune> content) {
      return new Expr_RawExpr(content);
    }
    public static _IExpr create_ExprFromType(RAST._IType tpe) {
      return new Expr_ExprFromType(tpe);
    }
    public static _IExpr create_Identifier(Dafny.ISequence<Dafny.Rune> name) {
      return new Expr_Identifier(name);
    }
    public static _IExpr create_Match(RAST._IExpr matchee, Dafny.ISequence<RAST._IMatchCase> cases) {
      return new Expr_Match(matchee, cases);
    }
    public static _IExpr create_StmtExpr(RAST._IExpr stmt, RAST._IExpr rhs) {
      return new Expr_StmtExpr(stmt, rhs);
    }
    public static _IExpr create_Block(RAST._IExpr underlying) {
      return new Expr_Block(underlying);
    }
    public static _IExpr create_StructBuild(RAST._IExpr underlying, Dafny.ISequence<RAST._IAssignIdentifier> assignments) {
      return new Expr_StructBuild(underlying, assignments);
    }
    public static _IExpr create_Tuple(Dafny.ISequence<RAST._IExpr> arguments) {
      return new Expr_Tuple(arguments);
    }
    public static _IExpr create_UnaryOp(Dafny.ISequence<Dafny.Rune> op1, RAST._IExpr underlying, DAST.Format._IUnaryOpFormat format) {
      return new Expr_UnaryOp(op1, underlying, format);
    }
    public static _IExpr create_BinaryOp(Dafny.ISequence<Dafny.Rune> op2, RAST._IExpr left, RAST._IExpr right, DAST.Format._IBinaryOpFormat format2) {
      return new Expr_BinaryOp(op2, left, right, format2);
    }
    public static _IExpr create_TypeAscription(RAST._IExpr left, RAST._IType tpe) {
      return new Expr_TypeAscription(left, tpe);
    }
    public static _IExpr create_LiteralInt(Dafny.ISequence<Dafny.Rune> @value) {
      return new Expr_LiteralInt(@value);
    }
    public static _IExpr create_LiteralBool(bool bvalue) {
      return new Expr_LiteralBool(bvalue);
    }
    public static _IExpr create_LiteralString(Dafny.ISequence<Dafny.Rune> @value, bool binary) {
      return new Expr_LiteralString(@value, binary);
    }
    public static _IExpr create_DeclareVar(RAST._IDeclareType declareType, Dafny.ISequence<Dafny.Rune> name, Std.Wrappers._IOption<RAST._IType> optType, Std.Wrappers._IOption<RAST._IExpr> optRhs) {
      return new Expr_DeclareVar(declareType, name, optType, optRhs);
    }
    public static _IExpr create_Assign(Std.Wrappers._IOption<RAST._IAssignLhs> names, RAST._IExpr rhs) {
      return new Expr_Assign(names, rhs);
    }
    public static _IExpr create_IfExpr(RAST._IExpr cond, RAST._IExpr thn, RAST._IExpr els) {
      return new Expr_IfExpr(cond, thn, els);
    }
    public static _IExpr create_Loop(Std.Wrappers._IOption<RAST._IExpr> optCond, RAST._IExpr underlying) {
      return new Expr_Loop(optCond, underlying);
    }
    public static _IExpr create_For(Dafny.ISequence<Dafny.Rune> name, RAST._IExpr range, RAST._IExpr body) {
      return new Expr_For(name, range, body);
    }
    public static _IExpr create_Labelled(Dafny.ISequence<Dafny.Rune> lbl, RAST._IExpr underlying) {
      return new Expr_Labelled(lbl, underlying);
    }
    public static _IExpr create_Break(Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> optLbl) {
      return new Expr_Break(optLbl);
    }
    public static _IExpr create_Continue(Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> optLbl) {
      return new Expr_Continue(optLbl);
    }
    public static _IExpr create_Return(Std.Wrappers._IOption<RAST._IExpr> optExpr) {
      return new Expr_Return(optExpr);
    }
    public static _IExpr create_CallType(RAST._IExpr obj, Dafny.ISequence<RAST._IType> typeParameters) {
      return new Expr_CallType(obj, typeParameters);
    }
    public static _IExpr create_Call(RAST._IExpr obj, Dafny.ISequence<RAST._IExpr> arguments) {
      return new Expr_Call(obj, arguments);
    }
    public static _IExpr create_Select(RAST._IExpr obj, Dafny.ISequence<Dafny.Rune> name) {
      return new Expr_Select(obj, name);
    }
    public static _IExpr create_MemberSelect(RAST._IExpr obj, Dafny.ISequence<Dafny.Rune> name) {
      return new Expr_MemberSelect(obj, name);
    }
    public static _IExpr create_Lambda(Dafny.ISequence<RAST._IFormal> @params, Std.Wrappers._IOption<RAST._IType> retType, RAST._IExpr body) {
      return new Expr_Lambda(@params, retType, body);
    }
    public bool is_RawExpr { get { return this is Expr_RawExpr; } }
    public bool is_ExprFromType { get { return this is Expr_ExprFromType; } }
    public bool is_Identifier { get { return this is Expr_Identifier; } }
    public bool is_Match { get { return this is Expr_Match; } }
    public bool is_StmtExpr { get { return this is Expr_StmtExpr; } }
    public bool is_Block { get { return this is Expr_Block; } }
    public bool is_StructBuild { get { return this is Expr_StructBuild; } }
    public bool is_Tuple { get { return this is Expr_Tuple; } }
    public bool is_UnaryOp { get { return this is Expr_UnaryOp; } }
    public bool is_BinaryOp { get { return this is Expr_BinaryOp; } }
    public bool is_TypeAscription { get { return this is Expr_TypeAscription; } }
    public bool is_LiteralInt { get { return this is Expr_LiteralInt; } }
    public bool is_LiteralBool { get { return this is Expr_LiteralBool; } }
    public bool is_LiteralString { get { return this is Expr_LiteralString; } }
    public bool is_DeclareVar { get { return this is Expr_DeclareVar; } }
    public bool is_Assign { get { return this is Expr_Assign; } }
    public bool is_IfExpr { get { return this is Expr_IfExpr; } }
    public bool is_Loop { get { return this is Expr_Loop; } }
    public bool is_For { get { return this is Expr_For; } }
    public bool is_Labelled { get { return this is Expr_Labelled; } }
    public bool is_Break { get { return this is Expr_Break; } }
    public bool is_Continue { get { return this is Expr_Continue; } }
    public bool is_Return { get { return this is Expr_Return; } }
    public bool is_CallType { get { return this is Expr_CallType; } }
    public bool is_Call { get { return this is Expr_Call; } }
    public bool is_Select { get { return this is Expr_Select; } }
    public bool is_MemberSelect { get { return this is Expr_MemberSelect; } }
    public bool is_Lambda { get { return this is Expr_Lambda; } }
    public Dafny.ISequence<Dafny.Rune> dtor_content {
      get {
        var d = this;
        return ((Expr_RawExpr)d)._i_content;
      }
    }
    public RAST._IType dtor_tpe {
      get {
        var d = this;
        if (d is Expr_ExprFromType) { return ((Expr_ExprFromType)d)._i_tpe; }
        return ((Expr_TypeAscription)d)._i_tpe;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        var d = this;
        if (d is Expr_Identifier) { return ((Expr_Identifier)d)._i_name; }
        if (d is Expr_DeclareVar) { return ((Expr_DeclareVar)d)._i_name; }
        if (d is Expr_For) { return ((Expr_For)d)._i_name; }
        if (d is Expr_Select) { return ((Expr_Select)d)._i_name; }
        return ((Expr_MemberSelect)d)._i_name;
      }
    }
    public RAST._IExpr dtor_matchee {
      get {
        var d = this;
        return ((Expr_Match)d)._i_matchee;
      }
    }
    public Dafny.ISequence<RAST._IMatchCase> dtor_cases {
      get {
        var d = this;
        return ((Expr_Match)d)._i_cases;
      }
    }
    public RAST._IExpr dtor_stmt {
      get {
        var d = this;
        return ((Expr_StmtExpr)d)._i_stmt;
      }
    }
    public RAST._IExpr dtor_rhs {
      get {
        var d = this;
        if (d is Expr_StmtExpr) { return ((Expr_StmtExpr)d)._i_rhs; }
        return ((Expr_Assign)d)._i_rhs;
      }
    }
    public RAST._IExpr dtor_underlying {
      get {
        var d = this;
        if (d is Expr_Block) { return ((Expr_Block)d)._i_underlying; }
        if (d is Expr_StructBuild) { return ((Expr_StructBuild)d)._i_underlying; }
        if (d is Expr_UnaryOp) { return ((Expr_UnaryOp)d)._i_underlying; }
        if (d is Expr_Loop) { return ((Expr_Loop)d)._i_underlying; }
        return ((Expr_Labelled)d)._i_underlying;
      }
    }
    public Dafny.ISequence<RAST._IAssignIdentifier> dtor_assignments {
      get {
        var d = this;
        return ((Expr_StructBuild)d)._i_assignments;
      }
    }
    public Dafny.ISequence<RAST._IExpr> dtor_arguments {
      get {
        var d = this;
        if (d is Expr_Tuple) { return ((Expr_Tuple)d)._i_arguments; }
        return ((Expr_Call)d)._i_arguments;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_op1 {
      get {
        var d = this;
        return ((Expr_UnaryOp)d)._i_op1;
      }
    }
    public DAST.Format._IUnaryOpFormat dtor_format {
      get {
        var d = this;
        return ((Expr_UnaryOp)d)._i_format;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_op2 {
      get {
        var d = this;
        return ((Expr_BinaryOp)d)._i_op2;
      }
    }
    public RAST._IExpr dtor_left {
      get {
        var d = this;
        if (d is Expr_BinaryOp) { return ((Expr_BinaryOp)d)._i_left; }
        return ((Expr_TypeAscription)d)._i_left;
      }
    }
    public RAST._IExpr dtor_right {
      get {
        var d = this;
        return ((Expr_BinaryOp)d)._i_right;
      }
    }
    public DAST.Format._IBinaryOpFormat dtor_format2 {
      get {
        var d = this;
        return ((Expr_BinaryOp)d)._i_format2;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_value {
      get {
        var d = this;
        if (d is Expr_LiteralInt) { return ((Expr_LiteralInt)d)._i_value; }
        return ((Expr_LiteralString)d)._i_value;
      }
    }
    public bool dtor_bvalue {
      get {
        var d = this;
        return ((Expr_LiteralBool)d)._i_bvalue;
      }
    }
    public bool dtor_binary {
      get {
        var d = this;
        return ((Expr_LiteralString)d)._i_binary;
      }
    }
    public RAST._IDeclareType dtor_declareType {
      get {
        var d = this;
        return ((Expr_DeclareVar)d)._i_declareType;
      }
    }
    public Std.Wrappers._IOption<RAST._IType> dtor_optType {
      get {
        var d = this;
        return ((Expr_DeclareVar)d)._i_optType;
      }
    }
    public Std.Wrappers._IOption<RAST._IExpr> dtor_optRhs {
      get {
        var d = this;
        return ((Expr_DeclareVar)d)._i_optRhs;
      }
    }
    public Std.Wrappers._IOption<RAST._IAssignLhs> dtor_names {
      get {
        var d = this;
        return ((Expr_Assign)d)._i_names;
      }
    }
    public RAST._IExpr dtor_cond {
      get {
        var d = this;
        return ((Expr_IfExpr)d)._i_cond;
      }
    }
    public RAST._IExpr dtor_thn {
      get {
        var d = this;
        return ((Expr_IfExpr)d)._i_thn;
      }
    }
    public RAST._IExpr dtor_els {
      get {
        var d = this;
        return ((Expr_IfExpr)d)._i_els;
      }
    }
    public Std.Wrappers._IOption<RAST._IExpr> dtor_optCond {
      get {
        var d = this;
        return ((Expr_Loop)d)._i_optCond;
      }
    }
    public RAST._IExpr dtor_range {
      get {
        var d = this;
        return ((Expr_For)d)._i_range;
      }
    }
    public RAST._IExpr dtor_body {
      get {
        var d = this;
        if (d is Expr_For) { return ((Expr_For)d)._i_body; }
        return ((Expr_Lambda)d)._i_body;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_lbl {
      get {
        var d = this;
        return ((Expr_Labelled)d)._i_lbl;
      }
    }
    public Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> dtor_optLbl {
      get {
        var d = this;
        if (d is Expr_Break) { return ((Expr_Break)d)._i_optLbl; }
        return ((Expr_Continue)d)._i_optLbl;
      }
    }
    public Std.Wrappers._IOption<RAST._IExpr> dtor_optExpr {
      get {
        var d = this;
        return ((Expr_Return)d)._i_optExpr;
      }
    }
    public RAST._IExpr dtor_obj {
      get {
        var d = this;
        if (d is Expr_CallType) { return ((Expr_CallType)d)._i_obj; }
        if (d is Expr_Call) { return ((Expr_Call)d)._i_obj; }
        if (d is Expr_Select) { return ((Expr_Select)d)._i_obj; }
        return ((Expr_MemberSelect)d)._i_obj;
      }
    }
    public Dafny.ISequence<RAST._IType> dtor_typeParameters {
      get {
        var d = this;
        return ((Expr_CallType)d)._i_typeParameters;
      }
    }
    public Dafny.ISequence<RAST._IFormal> dtor_params {
      get {
        var d = this;
        return ((Expr_Lambda)d)._i_params;
      }
    }
    public Std.Wrappers._IOption<RAST._IType> dtor_retType {
      get {
        var d = this;
        return ((Expr_Lambda)d)._i_retType;
      }
    }
    public abstract _IExpr DowncastClone();
    public bool NoExtraSemicolonAfter() {
      return (((((((this).is_DeclareVar) || ((this).is_Assign)) || ((this).is_Break)) || ((this).is_Continue)) || ((this).is_Return)) || ((this).is_For)) || ((((this).is_RawExpr) && ((new BigInteger(((this).dtor_content).Count)).Sign == 1)) && ((((this).dtor_content).Select((new BigInteger(((this).dtor_content).Count)) - (BigInteger.One))) == (new Dafny.Rune(';'))));
    }
    public RAST._IExpr Optimize() {
      RAST._IExpr _source46 = this;
      if (_source46.is_RawExpr) {
        Dafny.ISequence<Dafny.Rune> _1209___mcc_h0 = _source46.dtor_content;
        return this;
      } else if (_source46.is_ExprFromType) {
        RAST._IType _1210___mcc_h2 = _source46.dtor_tpe;
        return this;
      } else if (_source46.is_Identifier) {
        Dafny.ISequence<Dafny.Rune> _1211___mcc_h4 = _source46.dtor_name;
        return this;
      } else if (_source46.is_Match) {
        RAST._IExpr _1212___mcc_h6 = _source46.dtor_matchee;
        Dafny.ISequence<RAST._IMatchCase> _1213___mcc_h7 = _source46.dtor_cases;
        return this;
      } else if (_source46.is_StmtExpr) {
        RAST._IExpr _1214___mcc_h10 = _source46.dtor_stmt;
        RAST._IExpr _1215___mcc_h11 = _source46.dtor_rhs;
        RAST._IExpr _source47 = _1214___mcc_h10;
        if (_source47.is_RawExpr) {
          Dafny.ISequence<Dafny.Rune> _1216___mcc_h14 = _source47.dtor_content;
          return this;
        } else if (_source47.is_ExprFromType) {
          RAST._IType _1217___mcc_h16 = _source47.dtor_tpe;
          return this;
        } else if (_source47.is_Identifier) {
          Dafny.ISequence<Dafny.Rune> _1218___mcc_h18 = _source47.dtor_name;
          return this;
        } else if (_source47.is_Match) {
          RAST._IExpr _1219___mcc_h20 = _source47.dtor_matchee;
          Dafny.ISequence<RAST._IMatchCase> _1220___mcc_h21 = _source47.dtor_cases;
          return this;
        } else if (_source47.is_StmtExpr) {
          RAST._IExpr _1221___mcc_h24 = _source47.dtor_stmt;
          RAST._IExpr _1222___mcc_h25 = _source47.dtor_rhs;
          return this;
        } else if (_source47.is_Block) {
          RAST._IExpr _1223___mcc_h28 = _source47.dtor_underlying;
          return this;
        } else if (_source47.is_StructBuild) {
          RAST._IExpr _1224___mcc_h30 = _source47.dtor_underlying;
          Dafny.ISequence<RAST._IAssignIdentifier> _1225___mcc_h31 = _source47.dtor_assignments;
          return this;
        } else if (_source47.is_Tuple) {
          Dafny.ISequence<RAST._IExpr> _1226___mcc_h34 = _source47.dtor_arguments;
          return this;
        } else if (_source47.is_UnaryOp) {
          Dafny.ISequence<Dafny.Rune> _1227___mcc_h36 = _source47.dtor_op1;
          RAST._IExpr _1228___mcc_h37 = _source47.dtor_underlying;
          DAST.Format._IUnaryOpFormat _1229___mcc_h38 = _source47.dtor_format;
          return this;
        } else if (_source47.is_BinaryOp) {
          Dafny.ISequence<Dafny.Rune> _1230___mcc_h42 = _source47.dtor_op2;
          RAST._IExpr _1231___mcc_h43 = _source47.dtor_left;
          RAST._IExpr _1232___mcc_h44 = _source47.dtor_right;
          DAST.Format._IBinaryOpFormat _1233___mcc_h45 = _source47.dtor_format2;
          return this;
        } else if (_source47.is_TypeAscription) {
          RAST._IExpr _1234___mcc_h50 = _source47.dtor_left;
          RAST._IType _1235___mcc_h51 = _source47.dtor_tpe;
          return this;
        } else if (_source47.is_LiteralInt) {
          Dafny.ISequence<Dafny.Rune> _1236___mcc_h54 = _source47.dtor_value;
          return this;
        } else if (_source47.is_LiteralBool) {
          bool _1237___mcc_h56 = _source47.dtor_bvalue;
          return this;
        } else if (_source47.is_LiteralString) {
          Dafny.ISequence<Dafny.Rune> _1238___mcc_h58 = _source47.dtor_value;
          bool _1239___mcc_h59 = _source47.dtor_binary;
          return this;
        } else if (_source47.is_DeclareVar) {
          RAST._IDeclareType _1240___mcc_h62 = _source47.dtor_declareType;
          Dafny.ISequence<Dafny.Rune> _1241___mcc_h63 = _source47.dtor_name;
          Std.Wrappers._IOption<RAST._IType> _1242___mcc_h64 = _source47.dtor_optType;
          Std.Wrappers._IOption<RAST._IExpr> _1243___mcc_h65 = _source47.dtor_optRhs;
          Std.Wrappers._IOption<RAST._IType> _source48 = _1242___mcc_h64;
          if (_source48.is_None) {
            return this;
          } else {
            RAST._IType _1244___mcc_h70 = _source48.dtor_value;
            Std.Wrappers._IOption<RAST._IExpr> _source49 = _1243___mcc_h65;
            if (_source49.is_None) {
              RAST._IExpr _source50 = _1215___mcc_h11;
              if (_source50.is_RawExpr) {
                Dafny.ISequence<Dafny.Rune> _1245___mcc_h72 = _source50.dtor_content;
                return this;
              } else if (_source50.is_ExprFromType) {
                RAST._IType _1246___mcc_h74 = _source50.dtor_tpe;
                return this;
              } else if (_source50.is_Identifier) {
                Dafny.ISequence<Dafny.Rune> _1247___mcc_h76 = _source50.dtor_name;
                return this;
              } else if (_source50.is_Match) {
                RAST._IExpr _1248___mcc_h78 = _source50.dtor_matchee;
                Dafny.ISequence<RAST._IMatchCase> _1249___mcc_h79 = _source50.dtor_cases;
                return this;
              } else if (_source50.is_StmtExpr) {
                RAST._IExpr _1250___mcc_h82 = _source50.dtor_stmt;
                RAST._IExpr _1251___mcc_h83 = _source50.dtor_rhs;
                RAST._IExpr _source51 = _1250___mcc_h82;
                if (_source51.is_RawExpr) {
                  Dafny.ISequence<Dafny.Rune> _1252___mcc_h86 = _source51.dtor_content;
                  return this;
                } else if (_source51.is_ExprFromType) {
                  RAST._IType _1253___mcc_h88 = _source51.dtor_tpe;
                  return this;
                } else if (_source51.is_Identifier) {
                  Dafny.ISequence<Dafny.Rune> _1254___mcc_h90 = _source51.dtor_name;
                  return this;
                } else if (_source51.is_Match) {
                  RAST._IExpr _1255___mcc_h92 = _source51.dtor_matchee;
                  Dafny.ISequence<RAST._IMatchCase> _1256___mcc_h93 = _source51.dtor_cases;
                  return this;
                } else if (_source51.is_StmtExpr) {
                  RAST._IExpr _1257___mcc_h96 = _source51.dtor_stmt;
                  RAST._IExpr _1258___mcc_h97 = _source51.dtor_rhs;
                  return this;
                } else if (_source51.is_Block) {
                  RAST._IExpr _1259___mcc_h100 = _source51.dtor_underlying;
                  return this;
                } else if (_source51.is_StructBuild) {
                  RAST._IExpr _1260___mcc_h102 = _source51.dtor_underlying;
                  Dafny.ISequence<RAST._IAssignIdentifier> _1261___mcc_h103 = _source51.dtor_assignments;
                  return this;
                } else if (_source51.is_Tuple) {
                  Dafny.ISequence<RAST._IExpr> _1262___mcc_h106 = _source51.dtor_arguments;
                  return this;
                } else if (_source51.is_UnaryOp) {
                  Dafny.ISequence<Dafny.Rune> _1263___mcc_h108 = _source51.dtor_op1;
                  RAST._IExpr _1264___mcc_h109 = _source51.dtor_underlying;
                  DAST.Format._IUnaryOpFormat _1265___mcc_h110 = _source51.dtor_format;
                  return this;
                } else if (_source51.is_BinaryOp) {
                  Dafny.ISequence<Dafny.Rune> _1266___mcc_h114 = _source51.dtor_op2;
                  RAST._IExpr _1267___mcc_h115 = _source51.dtor_left;
                  RAST._IExpr _1268___mcc_h116 = _source51.dtor_right;
                  DAST.Format._IBinaryOpFormat _1269___mcc_h117 = _source51.dtor_format2;
                  return this;
                } else if (_source51.is_TypeAscription) {
                  RAST._IExpr _1270___mcc_h122 = _source51.dtor_left;
                  RAST._IType _1271___mcc_h123 = _source51.dtor_tpe;
                  return this;
                } else if (_source51.is_LiteralInt) {
                  Dafny.ISequence<Dafny.Rune> _1272___mcc_h126 = _source51.dtor_value;
                  return this;
                } else if (_source51.is_LiteralBool) {
                  bool _1273___mcc_h128 = _source51.dtor_bvalue;
                  return this;
                } else if (_source51.is_LiteralString) {
                  Dafny.ISequence<Dafny.Rune> _1274___mcc_h130 = _source51.dtor_value;
                  bool _1275___mcc_h131 = _source51.dtor_binary;
                  return this;
                } else if (_source51.is_DeclareVar) {
                  RAST._IDeclareType _1276___mcc_h134 = _source51.dtor_declareType;
                  Dafny.ISequence<Dafny.Rune> _1277___mcc_h135 = _source51.dtor_name;
                  Std.Wrappers._IOption<RAST._IType> _1278___mcc_h136 = _source51.dtor_optType;
                  Std.Wrappers._IOption<RAST._IExpr> _1279___mcc_h137 = _source51.dtor_optRhs;
                  return this;
                } else if (_source51.is_Assign) {
                  Std.Wrappers._IOption<RAST._IAssignLhs> _1280___mcc_h142 = _source51.dtor_names;
                  RAST._IExpr _1281___mcc_h143 = _source51.dtor_rhs;
                  RAST._IExpr _1282_last = _1251___mcc_h83;
                  RAST._IExpr _1283_rhs = _1281___mcc_h143;
                  Std.Wrappers._IOption<RAST._IAssignLhs> _1284_name2 = _1280___mcc_h142;
                  RAST._IType _1285_tpe = _1244___mcc_h70;
                  Dafny.ISequence<Dafny.Rune> _1286_name = _1241___mcc_h63;
                  RAST._IDeclareType _1287_mod = _1240___mcc_h62;
                  if (object.Equals(_1284_name2, Std.Wrappers.Option<RAST._IAssignLhs>.create_Some(RAST.AssignLhs.create_LocalVar(_1286_name)))) {
                    RAST._IExpr _1288_rewriting = RAST.Expr.create_StmtExpr(RAST.Expr.create_DeclareVar(_1287_mod, _1286_name, Std.Wrappers.Option<RAST._IType>.create_Some(_1285_tpe), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1283_rhs)), _1282_last);
                    return _1288_rewriting;
                  } else {
                    return this;
                  }
                } else if (_source51.is_IfExpr) {
                  RAST._IExpr _1289___mcc_h146 = _source51.dtor_cond;
                  RAST._IExpr _1290___mcc_h147 = _source51.dtor_thn;
                  RAST._IExpr _1291___mcc_h148 = _source51.dtor_els;
                  return this;
                } else if (_source51.is_Loop) {
                  Std.Wrappers._IOption<RAST._IExpr> _1292___mcc_h152 = _source51.dtor_optCond;
                  RAST._IExpr _1293___mcc_h153 = _source51.dtor_underlying;
                  return this;
                } else if (_source51.is_For) {
                  Dafny.ISequence<Dafny.Rune> _1294___mcc_h156 = _source51.dtor_name;
                  RAST._IExpr _1295___mcc_h157 = _source51.dtor_range;
                  RAST._IExpr _1296___mcc_h158 = _source51.dtor_body;
                  return this;
                } else if (_source51.is_Labelled) {
                  Dafny.ISequence<Dafny.Rune> _1297___mcc_h162 = _source51.dtor_lbl;
                  RAST._IExpr _1298___mcc_h163 = _source51.dtor_underlying;
                  return this;
                } else if (_source51.is_Break) {
                  Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1299___mcc_h166 = _source51.dtor_optLbl;
                  return this;
                } else if (_source51.is_Continue) {
                  Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1300___mcc_h168 = _source51.dtor_optLbl;
                  return this;
                } else if (_source51.is_Return) {
                  Std.Wrappers._IOption<RAST._IExpr> _1301___mcc_h170 = _source51.dtor_optExpr;
                  return this;
                } else if (_source51.is_CallType) {
                  RAST._IExpr _1302___mcc_h172 = _source51.dtor_obj;
                  Dafny.ISequence<RAST._IType> _1303___mcc_h173 = _source51.dtor_typeParameters;
                  return this;
                } else if (_source51.is_Call) {
                  RAST._IExpr _1304___mcc_h176 = _source51.dtor_obj;
                  Dafny.ISequence<RAST._IExpr> _1305___mcc_h177 = _source51.dtor_arguments;
                  return this;
                } else if (_source51.is_Select) {
                  RAST._IExpr _1306___mcc_h180 = _source51.dtor_obj;
                  Dafny.ISequence<Dafny.Rune> _1307___mcc_h181 = _source51.dtor_name;
                  return this;
                } else if (_source51.is_MemberSelect) {
                  RAST._IExpr _1308___mcc_h184 = _source51.dtor_obj;
                  Dafny.ISequence<Dafny.Rune> _1309___mcc_h185 = _source51.dtor_name;
                  return this;
                } else {
                  Dafny.ISequence<RAST._IFormal> _1310___mcc_h188 = _source51.dtor_params;
                  Std.Wrappers._IOption<RAST._IType> _1311___mcc_h189 = _source51.dtor_retType;
                  RAST._IExpr _1312___mcc_h190 = _source51.dtor_body;
                  return this;
                }
              } else if (_source50.is_Block) {
                RAST._IExpr _1313___mcc_h194 = _source50.dtor_underlying;
                return this;
              } else if (_source50.is_StructBuild) {
                RAST._IExpr _1314___mcc_h196 = _source50.dtor_underlying;
                Dafny.ISequence<RAST._IAssignIdentifier> _1315___mcc_h197 = _source50.dtor_assignments;
                return this;
              } else if (_source50.is_Tuple) {
                Dafny.ISequence<RAST._IExpr> _1316___mcc_h200 = _source50.dtor_arguments;
                return this;
              } else if (_source50.is_UnaryOp) {
                Dafny.ISequence<Dafny.Rune> _1317___mcc_h202 = _source50.dtor_op1;
                RAST._IExpr _1318___mcc_h203 = _source50.dtor_underlying;
                DAST.Format._IUnaryOpFormat _1319___mcc_h204 = _source50.dtor_format;
                return this;
              } else if (_source50.is_BinaryOp) {
                Dafny.ISequence<Dafny.Rune> _1320___mcc_h208 = _source50.dtor_op2;
                RAST._IExpr _1321___mcc_h209 = _source50.dtor_left;
                RAST._IExpr _1322___mcc_h210 = _source50.dtor_right;
                DAST.Format._IBinaryOpFormat _1323___mcc_h211 = _source50.dtor_format2;
                return this;
              } else if (_source50.is_TypeAscription) {
                RAST._IExpr _1324___mcc_h216 = _source50.dtor_left;
                RAST._IType _1325___mcc_h217 = _source50.dtor_tpe;
                return this;
              } else if (_source50.is_LiteralInt) {
                Dafny.ISequence<Dafny.Rune> _1326___mcc_h220 = _source50.dtor_value;
                return this;
              } else if (_source50.is_LiteralBool) {
                bool _1327___mcc_h222 = _source50.dtor_bvalue;
                return this;
              } else if (_source50.is_LiteralString) {
                Dafny.ISequence<Dafny.Rune> _1328___mcc_h224 = _source50.dtor_value;
                bool _1329___mcc_h225 = _source50.dtor_binary;
                return this;
              } else if (_source50.is_DeclareVar) {
                RAST._IDeclareType _1330___mcc_h228 = _source50.dtor_declareType;
                Dafny.ISequence<Dafny.Rune> _1331___mcc_h229 = _source50.dtor_name;
                Std.Wrappers._IOption<RAST._IType> _1332___mcc_h230 = _source50.dtor_optType;
                Std.Wrappers._IOption<RAST._IExpr> _1333___mcc_h231 = _source50.dtor_optRhs;
                return this;
              } else if (_source50.is_Assign) {
                Std.Wrappers._IOption<RAST._IAssignLhs> _1334___mcc_h236 = _source50.dtor_names;
                RAST._IExpr _1335___mcc_h237 = _source50.dtor_rhs;
                return this;
              } else if (_source50.is_IfExpr) {
                RAST._IExpr _1336___mcc_h240 = _source50.dtor_cond;
                RAST._IExpr _1337___mcc_h241 = _source50.dtor_thn;
                RAST._IExpr _1338___mcc_h242 = _source50.dtor_els;
                return this;
              } else if (_source50.is_Loop) {
                Std.Wrappers._IOption<RAST._IExpr> _1339___mcc_h246 = _source50.dtor_optCond;
                RAST._IExpr _1340___mcc_h247 = _source50.dtor_underlying;
                return this;
              } else if (_source50.is_For) {
                Dafny.ISequence<Dafny.Rune> _1341___mcc_h250 = _source50.dtor_name;
                RAST._IExpr _1342___mcc_h251 = _source50.dtor_range;
                RAST._IExpr _1343___mcc_h252 = _source50.dtor_body;
                return this;
              } else if (_source50.is_Labelled) {
                Dafny.ISequence<Dafny.Rune> _1344___mcc_h256 = _source50.dtor_lbl;
                RAST._IExpr _1345___mcc_h257 = _source50.dtor_underlying;
                return this;
              } else if (_source50.is_Break) {
                Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1346___mcc_h260 = _source50.dtor_optLbl;
                return this;
              } else if (_source50.is_Continue) {
                Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1347___mcc_h262 = _source50.dtor_optLbl;
                return this;
              } else if (_source50.is_Return) {
                Std.Wrappers._IOption<RAST._IExpr> _1348___mcc_h264 = _source50.dtor_optExpr;
                return this;
              } else if (_source50.is_CallType) {
                RAST._IExpr _1349___mcc_h266 = _source50.dtor_obj;
                Dafny.ISequence<RAST._IType> _1350___mcc_h267 = _source50.dtor_typeParameters;
                return this;
              } else if (_source50.is_Call) {
                RAST._IExpr _1351___mcc_h270 = _source50.dtor_obj;
                Dafny.ISequence<RAST._IExpr> _1352___mcc_h271 = _source50.dtor_arguments;
                return this;
              } else if (_source50.is_Select) {
                RAST._IExpr _1353___mcc_h274 = _source50.dtor_obj;
                Dafny.ISequence<Dafny.Rune> _1354___mcc_h275 = _source50.dtor_name;
                return this;
              } else if (_source50.is_MemberSelect) {
                RAST._IExpr _1355___mcc_h278 = _source50.dtor_obj;
                Dafny.ISequence<Dafny.Rune> _1356___mcc_h279 = _source50.dtor_name;
                return this;
              } else {
                Dafny.ISequence<RAST._IFormal> _1357___mcc_h282 = _source50.dtor_params;
                Std.Wrappers._IOption<RAST._IType> _1358___mcc_h283 = _source50.dtor_retType;
                RAST._IExpr _1359___mcc_h284 = _source50.dtor_body;
                return this;
              }
            } else {
              RAST._IExpr _1360___mcc_h288 = _source49.dtor_value;
              return this;
            }
          }
        } else if (_source47.is_Assign) {
          Std.Wrappers._IOption<RAST._IAssignLhs> _1361___mcc_h290 = _source47.dtor_names;
          RAST._IExpr _1362___mcc_h291 = _source47.dtor_rhs;
          return this;
        } else if (_source47.is_IfExpr) {
          RAST._IExpr _1363___mcc_h294 = _source47.dtor_cond;
          RAST._IExpr _1364___mcc_h295 = _source47.dtor_thn;
          RAST._IExpr _1365___mcc_h296 = _source47.dtor_els;
          RAST._IExpr _source52 = _1363___mcc_h294;
          if (_source52.is_RawExpr) {
            Dafny.ISequence<Dafny.Rune> _1366___mcc_h300 = _source52.dtor_content;
            return this;
          } else if (_source52.is_ExprFromType) {
            RAST._IType _1367___mcc_h302 = _source52.dtor_tpe;
            return this;
          } else if (_source52.is_Identifier) {
            Dafny.ISequence<Dafny.Rune> _1368___mcc_h304 = _source52.dtor_name;
            return this;
          } else if (_source52.is_Match) {
            RAST._IExpr _1369___mcc_h306 = _source52.dtor_matchee;
            Dafny.ISequence<RAST._IMatchCase> _1370___mcc_h307 = _source52.dtor_cases;
            return this;
          } else if (_source52.is_StmtExpr) {
            RAST._IExpr _1371___mcc_h310 = _source52.dtor_stmt;
            RAST._IExpr _1372___mcc_h311 = _source52.dtor_rhs;
            return this;
          } else if (_source52.is_Block) {
            RAST._IExpr _1373___mcc_h314 = _source52.dtor_underlying;
            return this;
          } else if (_source52.is_StructBuild) {
            RAST._IExpr _1374___mcc_h316 = _source52.dtor_underlying;
            Dafny.ISequence<RAST._IAssignIdentifier> _1375___mcc_h317 = _source52.dtor_assignments;
            return this;
          } else if (_source52.is_Tuple) {
            Dafny.ISequence<RAST._IExpr> _1376___mcc_h320 = _source52.dtor_arguments;
            return this;
          } else if (_source52.is_UnaryOp) {
            Dafny.ISequence<Dafny.Rune> _1377___mcc_h322 = _source52.dtor_op1;
            RAST._IExpr _1378___mcc_h323 = _source52.dtor_underlying;
            DAST.Format._IUnaryOpFormat _1379___mcc_h324 = _source52.dtor_format;
            if (object.Equals(_1377___mcc_h322, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"))) {
              RAST._IExpr _source53 = _1378___mcc_h323;
              if (_source53.is_RawExpr) {
                Dafny.ISequence<Dafny.Rune> _1380___mcc_h328 = _source53.dtor_content;
                return this;
              } else if (_source53.is_ExprFromType) {
                RAST._IType _1381___mcc_h330 = _source53.dtor_tpe;
                return this;
              } else if (_source53.is_Identifier) {
                Dafny.ISequence<Dafny.Rune> _1382___mcc_h332 = _source53.dtor_name;
                return this;
              } else if (_source53.is_Match) {
                RAST._IExpr _1383___mcc_h334 = _source53.dtor_matchee;
                Dafny.ISequence<RAST._IMatchCase> _1384___mcc_h335 = _source53.dtor_cases;
                return this;
              } else if (_source53.is_StmtExpr) {
                RAST._IExpr _1385___mcc_h338 = _source53.dtor_stmt;
                RAST._IExpr _1386___mcc_h339 = _source53.dtor_rhs;
                return this;
              } else if (_source53.is_Block) {
                RAST._IExpr _1387___mcc_h342 = _source53.dtor_underlying;
                return this;
              } else if (_source53.is_StructBuild) {
                RAST._IExpr _1388___mcc_h344 = _source53.dtor_underlying;
                Dafny.ISequence<RAST._IAssignIdentifier> _1389___mcc_h345 = _source53.dtor_assignments;
                return this;
              } else if (_source53.is_Tuple) {
                Dafny.ISequence<RAST._IExpr> _1390___mcc_h348 = _source53.dtor_arguments;
                return this;
              } else if (_source53.is_UnaryOp) {
                Dafny.ISequence<Dafny.Rune> _1391___mcc_h350 = _source53.dtor_op1;
                RAST._IExpr _1392___mcc_h351 = _source53.dtor_underlying;
                DAST.Format._IUnaryOpFormat _1393___mcc_h352 = _source53.dtor_format;
                return this;
              } else if (_source53.is_BinaryOp) {
                Dafny.ISequence<Dafny.Rune> _1394___mcc_h356 = _source53.dtor_op2;
                RAST._IExpr _1395___mcc_h357 = _source53.dtor_left;
                RAST._IExpr _1396___mcc_h358 = _source53.dtor_right;
                DAST.Format._IBinaryOpFormat _1397___mcc_h359 = _source53.dtor_format2;
                if (object.Equals(_1394___mcc_h356, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="))) {
                  RAST._IExpr _source54 = _1364___mcc_h295;
                  if (_source54.is_RawExpr) {
                    Dafny.ISequence<Dafny.Rune> _1398___mcc_h364 = _source54.dtor_content;
                    if (object.Equals(_1398___mcc_h364, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("panic!(\"Halt\");"))) {
                      RAST._IExpr _source55 = _1365___mcc_h296;
                      if (_source55.is_RawExpr) {
                        Dafny.ISequence<Dafny.Rune> _1399___mcc_h366 = _source55.dtor_content;
                        if (object.Equals(_1399___mcc_h366, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) {
                          RAST._IExpr _1400_last = _1215___mcc_h11;
                          DAST.Format._IUnaryOpFormat _1401_of = _1379___mcc_h324;
                          DAST.Format._IBinaryOpFormat _1402_f = _1397___mcc_h359;
                          RAST._IExpr _1403_b = _1396___mcc_h358;
                          RAST._IExpr _1404_a = _1395___mcc_h357;
                          RAST._IExpr _1405_rewriting = RAST.Expr.create_StmtExpr((RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("assert_eq!"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_1404_a, _1403_b)), _1400_last);
                          return _1405_rewriting;
                        } else {
                          return this;
                        }
                      } else if (_source55.is_ExprFromType) {
                        RAST._IType _1406___mcc_h368 = _source55.dtor_tpe;
                        return this;
                      } else if (_source55.is_Identifier) {
                        Dafny.ISequence<Dafny.Rune> _1407___mcc_h370 = _source55.dtor_name;
                        return this;
                      } else if (_source55.is_Match) {
                        RAST._IExpr _1408___mcc_h372 = _source55.dtor_matchee;
                        Dafny.ISequence<RAST._IMatchCase> _1409___mcc_h373 = _source55.dtor_cases;
                        return this;
                      } else if (_source55.is_StmtExpr) {
                        RAST._IExpr _1410___mcc_h376 = _source55.dtor_stmt;
                        RAST._IExpr _1411___mcc_h377 = _source55.dtor_rhs;
                        return this;
                      } else if (_source55.is_Block) {
                        RAST._IExpr _1412___mcc_h380 = _source55.dtor_underlying;
                        return this;
                      } else if (_source55.is_StructBuild) {
                        RAST._IExpr _1413___mcc_h382 = _source55.dtor_underlying;
                        Dafny.ISequence<RAST._IAssignIdentifier> _1414___mcc_h383 = _source55.dtor_assignments;
                        return this;
                      } else if (_source55.is_Tuple) {
                        Dafny.ISequence<RAST._IExpr> _1415___mcc_h386 = _source55.dtor_arguments;
                        return this;
                      } else if (_source55.is_UnaryOp) {
                        Dafny.ISequence<Dafny.Rune> _1416___mcc_h388 = _source55.dtor_op1;
                        RAST._IExpr _1417___mcc_h389 = _source55.dtor_underlying;
                        DAST.Format._IUnaryOpFormat _1418___mcc_h390 = _source55.dtor_format;
                        return this;
                      } else if (_source55.is_BinaryOp) {
                        Dafny.ISequence<Dafny.Rune> _1419___mcc_h394 = _source55.dtor_op2;
                        RAST._IExpr _1420___mcc_h395 = _source55.dtor_left;
                        RAST._IExpr _1421___mcc_h396 = _source55.dtor_right;
                        DAST.Format._IBinaryOpFormat _1422___mcc_h397 = _source55.dtor_format2;
                        return this;
                      } else if (_source55.is_TypeAscription) {
                        RAST._IExpr _1423___mcc_h402 = _source55.dtor_left;
                        RAST._IType _1424___mcc_h403 = _source55.dtor_tpe;
                        return this;
                      } else if (_source55.is_LiteralInt) {
                        Dafny.ISequence<Dafny.Rune> _1425___mcc_h406 = _source55.dtor_value;
                        return this;
                      } else if (_source55.is_LiteralBool) {
                        bool _1426___mcc_h408 = _source55.dtor_bvalue;
                        return this;
                      } else if (_source55.is_LiteralString) {
                        Dafny.ISequence<Dafny.Rune> _1427___mcc_h410 = _source55.dtor_value;
                        bool _1428___mcc_h411 = _source55.dtor_binary;
                        return this;
                      } else if (_source55.is_DeclareVar) {
                        RAST._IDeclareType _1429___mcc_h414 = _source55.dtor_declareType;
                        Dafny.ISequence<Dafny.Rune> _1430___mcc_h415 = _source55.dtor_name;
                        Std.Wrappers._IOption<RAST._IType> _1431___mcc_h416 = _source55.dtor_optType;
                        Std.Wrappers._IOption<RAST._IExpr> _1432___mcc_h417 = _source55.dtor_optRhs;
                        return this;
                      } else if (_source55.is_Assign) {
                        Std.Wrappers._IOption<RAST._IAssignLhs> _1433___mcc_h422 = _source55.dtor_names;
                        RAST._IExpr _1434___mcc_h423 = _source55.dtor_rhs;
                        return this;
                      } else if (_source55.is_IfExpr) {
                        RAST._IExpr _1435___mcc_h426 = _source55.dtor_cond;
                        RAST._IExpr _1436___mcc_h427 = _source55.dtor_thn;
                        RAST._IExpr _1437___mcc_h428 = _source55.dtor_els;
                        return this;
                      } else if (_source55.is_Loop) {
                        Std.Wrappers._IOption<RAST._IExpr> _1438___mcc_h432 = _source55.dtor_optCond;
                        RAST._IExpr _1439___mcc_h433 = _source55.dtor_underlying;
                        return this;
                      } else if (_source55.is_For) {
                        Dafny.ISequence<Dafny.Rune> _1440___mcc_h436 = _source55.dtor_name;
                        RAST._IExpr _1441___mcc_h437 = _source55.dtor_range;
                        RAST._IExpr _1442___mcc_h438 = _source55.dtor_body;
                        return this;
                      } else if (_source55.is_Labelled) {
                        Dafny.ISequence<Dafny.Rune> _1443___mcc_h442 = _source55.dtor_lbl;
                        RAST._IExpr _1444___mcc_h443 = _source55.dtor_underlying;
                        return this;
                      } else if (_source55.is_Break) {
                        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1445___mcc_h446 = _source55.dtor_optLbl;
                        return this;
                      } else if (_source55.is_Continue) {
                        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1446___mcc_h448 = _source55.dtor_optLbl;
                        return this;
                      } else if (_source55.is_Return) {
                        Std.Wrappers._IOption<RAST._IExpr> _1447___mcc_h450 = _source55.dtor_optExpr;
                        return this;
                      } else if (_source55.is_CallType) {
                        RAST._IExpr _1448___mcc_h452 = _source55.dtor_obj;
                        Dafny.ISequence<RAST._IType> _1449___mcc_h453 = _source55.dtor_typeParameters;
                        return this;
                      } else if (_source55.is_Call) {
                        RAST._IExpr _1450___mcc_h456 = _source55.dtor_obj;
                        Dafny.ISequence<RAST._IExpr> _1451___mcc_h457 = _source55.dtor_arguments;
                        return this;
                      } else if (_source55.is_Select) {
                        RAST._IExpr _1452___mcc_h460 = _source55.dtor_obj;
                        Dafny.ISequence<Dafny.Rune> _1453___mcc_h461 = _source55.dtor_name;
                        return this;
                      } else if (_source55.is_MemberSelect) {
                        RAST._IExpr _1454___mcc_h464 = _source55.dtor_obj;
                        Dafny.ISequence<Dafny.Rune> _1455___mcc_h465 = _source55.dtor_name;
                        return this;
                      } else {
                        Dafny.ISequence<RAST._IFormal> _1456___mcc_h468 = _source55.dtor_params;
                        Std.Wrappers._IOption<RAST._IType> _1457___mcc_h469 = _source55.dtor_retType;
                        RAST._IExpr _1458___mcc_h470 = _source55.dtor_body;
                        return this;
                      }
                    } else {
                      return this;
                    }
                  } else if (_source54.is_ExprFromType) {
                    RAST._IType _1459___mcc_h474 = _source54.dtor_tpe;
                    return this;
                  } else if (_source54.is_Identifier) {
                    Dafny.ISequence<Dafny.Rune> _1460___mcc_h476 = _source54.dtor_name;
                    return this;
                  } else if (_source54.is_Match) {
                    RAST._IExpr _1461___mcc_h478 = _source54.dtor_matchee;
                    Dafny.ISequence<RAST._IMatchCase> _1462___mcc_h479 = _source54.dtor_cases;
                    return this;
                  } else if (_source54.is_StmtExpr) {
                    RAST._IExpr _1463___mcc_h482 = _source54.dtor_stmt;
                    RAST._IExpr _1464___mcc_h483 = _source54.dtor_rhs;
                    return this;
                  } else if (_source54.is_Block) {
                    RAST._IExpr _1465___mcc_h486 = _source54.dtor_underlying;
                    return this;
                  } else if (_source54.is_StructBuild) {
                    RAST._IExpr _1466___mcc_h488 = _source54.dtor_underlying;
                    Dafny.ISequence<RAST._IAssignIdentifier> _1467___mcc_h489 = _source54.dtor_assignments;
                    return this;
                  } else if (_source54.is_Tuple) {
                    Dafny.ISequence<RAST._IExpr> _1468___mcc_h492 = _source54.dtor_arguments;
                    return this;
                  } else if (_source54.is_UnaryOp) {
                    Dafny.ISequence<Dafny.Rune> _1469___mcc_h494 = _source54.dtor_op1;
                    RAST._IExpr _1470___mcc_h495 = _source54.dtor_underlying;
                    DAST.Format._IUnaryOpFormat _1471___mcc_h496 = _source54.dtor_format;
                    return this;
                  } else if (_source54.is_BinaryOp) {
                    Dafny.ISequence<Dafny.Rune> _1472___mcc_h500 = _source54.dtor_op2;
                    RAST._IExpr _1473___mcc_h501 = _source54.dtor_left;
                    RAST._IExpr _1474___mcc_h502 = _source54.dtor_right;
                    DAST.Format._IBinaryOpFormat _1475___mcc_h503 = _source54.dtor_format2;
                    return this;
                  } else if (_source54.is_TypeAscription) {
                    RAST._IExpr _1476___mcc_h508 = _source54.dtor_left;
                    RAST._IType _1477___mcc_h509 = _source54.dtor_tpe;
                    return this;
                  } else if (_source54.is_LiteralInt) {
                    Dafny.ISequence<Dafny.Rune> _1478___mcc_h512 = _source54.dtor_value;
                    return this;
                  } else if (_source54.is_LiteralBool) {
                    bool _1479___mcc_h514 = _source54.dtor_bvalue;
                    return this;
                  } else if (_source54.is_LiteralString) {
                    Dafny.ISequence<Dafny.Rune> _1480___mcc_h516 = _source54.dtor_value;
                    bool _1481___mcc_h517 = _source54.dtor_binary;
                    return this;
                  } else if (_source54.is_DeclareVar) {
                    RAST._IDeclareType _1482___mcc_h520 = _source54.dtor_declareType;
                    Dafny.ISequence<Dafny.Rune> _1483___mcc_h521 = _source54.dtor_name;
                    Std.Wrappers._IOption<RAST._IType> _1484___mcc_h522 = _source54.dtor_optType;
                    Std.Wrappers._IOption<RAST._IExpr> _1485___mcc_h523 = _source54.dtor_optRhs;
                    return this;
                  } else if (_source54.is_Assign) {
                    Std.Wrappers._IOption<RAST._IAssignLhs> _1486___mcc_h528 = _source54.dtor_names;
                    RAST._IExpr _1487___mcc_h529 = _source54.dtor_rhs;
                    return this;
                  } else if (_source54.is_IfExpr) {
                    RAST._IExpr _1488___mcc_h532 = _source54.dtor_cond;
                    RAST._IExpr _1489___mcc_h533 = _source54.dtor_thn;
                    RAST._IExpr _1490___mcc_h534 = _source54.dtor_els;
                    return this;
                  } else if (_source54.is_Loop) {
                    Std.Wrappers._IOption<RAST._IExpr> _1491___mcc_h538 = _source54.dtor_optCond;
                    RAST._IExpr _1492___mcc_h539 = _source54.dtor_underlying;
                    return this;
                  } else if (_source54.is_For) {
                    Dafny.ISequence<Dafny.Rune> _1493___mcc_h542 = _source54.dtor_name;
                    RAST._IExpr _1494___mcc_h543 = _source54.dtor_range;
                    RAST._IExpr _1495___mcc_h544 = _source54.dtor_body;
                    return this;
                  } else if (_source54.is_Labelled) {
                    Dafny.ISequence<Dafny.Rune> _1496___mcc_h548 = _source54.dtor_lbl;
                    RAST._IExpr _1497___mcc_h549 = _source54.dtor_underlying;
                    return this;
                  } else if (_source54.is_Break) {
                    Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1498___mcc_h552 = _source54.dtor_optLbl;
                    return this;
                  } else if (_source54.is_Continue) {
                    Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1499___mcc_h554 = _source54.dtor_optLbl;
                    return this;
                  } else if (_source54.is_Return) {
                    Std.Wrappers._IOption<RAST._IExpr> _1500___mcc_h556 = _source54.dtor_optExpr;
                    return this;
                  } else if (_source54.is_CallType) {
                    RAST._IExpr _1501___mcc_h558 = _source54.dtor_obj;
                    Dafny.ISequence<RAST._IType> _1502___mcc_h559 = _source54.dtor_typeParameters;
                    return this;
                  } else if (_source54.is_Call) {
                    RAST._IExpr _1503___mcc_h562 = _source54.dtor_obj;
                    Dafny.ISequence<RAST._IExpr> _1504___mcc_h563 = _source54.dtor_arguments;
                    return this;
                  } else if (_source54.is_Select) {
                    RAST._IExpr _1505___mcc_h566 = _source54.dtor_obj;
                    Dafny.ISequence<Dafny.Rune> _1506___mcc_h567 = _source54.dtor_name;
                    return this;
                  } else if (_source54.is_MemberSelect) {
                    RAST._IExpr _1507___mcc_h570 = _source54.dtor_obj;
                    Dafny.ISequence<Dafny.Rune> _1508___mcc_h571 = _source54.dtor_name;
                    return this;
                  } else {
                    Dafny.ISequence<RAST._IFormal> _1509___mcc_h574 = _source54.dtor_params;
                    Std.Wrappers._IOption<RAST._IType> _1510___mcc_h575 = _source54.dtor_retType;
                    RAST._IExpr _1511___mcc_h576 = _source54.dtor_body;
                    return this;
                  }
                } else {
                  return this;
                }
              } else if (_source53.is_TypeAscription) {
                RAST._IExpr _1512___mcc_h580 = _source53.dtor_left;
                RAST._IType _1513___mcc_h581 = _source53.dtor_tpe;
                return this;
              } else if (_source53.is_LiteralInt) {
                Dafny.ISequence<Dafny.Rune> _1514___mcc_h584 = _source53.dtor_value;
                return this;
              } else if (_source53.is_LiteralBool) {
                bool _1515___mcc_h586 = _source53.dtor_bvalue;
                return this;
              } else if (_source53.is_LiteralString) {
                Dafny.ISequence<Dafny.Rune> _1516___mcc_h588 = _source53.dtor_value;
                bool _1517___mcc_h589 = _source53.dtor_binary;
                return this;
              } else if (_source53.is_DeclareVar) {
                RAST._IDeclareType _1518___mcc_h592 = _source53.dtor_declareType;
                Dafny.ISequence<Dafny.Rune> _1519___mcc_h593 = _source53.dtor_name;
                Std.Wrappers._IOption<RAST._IType> _1520___mcc_h594 = _source53.dtor_optType;
                Std.Wrappers._IOption<RAST._IExpr> _1521___mcc_h595 = _source53.dtor_optRhs;
                return this;
              } else if (_source53.is_Assign) {
                Std.Wrappers._IOption<RAST._IAssignLhs> _1522___mcc_h600 = _source53.dtor_names;
                RAST._IExpr _1523___mcc_h601 = _source53.dtor_rhs;
                return this;
              } else if (_source53.is_IfExpr) {
                RAST._IExpr _1524___mcc_h604 = _source53.dtor_cond;
                RAST._IExpr _1525___mcc_h605 = _source53.dtor_thn;
                RAST._IExpr _1526___mcc_h606 = _source53.dtor_els;
                return this;
              } else if (_source53.is_Loop) {
                Std.Wrappers._IOption<RAST._IExpr> _1527___mcc_h610 = _source53.dtor_optCond;
                RAST._IExpr _1528___mcc_h611 = _source53.dtor_underlying;
                return this;
              } else if (_source53.is_For) {
                Dafny.ISequence<Dafny.Rune> _1529___mcc_h614 = _source53.dtor_name;
                RAST._IExpr _1530___mcc_h615 = _source53.dtor_range;
                RAST._IExpr _1531___mcc_h616 = _source53.dtor_body;
                return this;
              } else if (_source53.is_Labelled) {
                Dafny.ISequence<Dafny.Rune> _1532___mcc_h620 = _source53.dtor_lbl;
                RAST._IExpr _1533___mcc_h621 = _source53.dtor_underlying;
                return this;
              } else if (_source53.is_Break) {
                Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1534___mcc_h624 = _source53.dtor_optLbl;
                return this;
              } else if (_source53.is_Continue) {
                Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1535___mcc_h626 = _source53.dtor_optLbl;
                return this;
              } else if (_source53.is_Return) {
                Std.Wrappers._IOption<RAST._IExpr> _1536___mcc_h628 = _source53.dtor_optExpr;
                return this;
              } else if (_source53.is_CallType) {
                RAST._IExpr _1537___mcc_h630 = _source53.dtor_obj;
                Dafny.ISequence<RAST._IType> _1538___mcc_h631 = _source53.dtor_typeParameters;
                return this;
              } else if (_source53.is_Call) {
                RAST._IExpr _1539___mcc_h634 = _source53.dtor_obj;
                Dafny.ISequence<RAST._IExpr> _1540___mcc_h635 = _source53.dtor_arguments;
                return this;
              } else if (_source53.is_Select) {
                RAST._IExpr _1541___mcc_h638 = _source53.dtor_obj;
                Dafny.ISequence<Dafny.Rune> _1542___mcc_h639 = _source53.dtor_name;
                return this;
              } else if (_source53.is_MemberSelect) {
                RAST._IExpr _1543___mcc_h642 = _source53.dtor_obj;
                Dafny.ISequence<Dafny.Rune> _1544___mcc_h643 = _source53.dtor_name;
                return this;
              } else {
                Dafny.ISequence<RAST._IFormal> _1545___mcc_h646 = _source53.dtor_params;
                Std.Wrappers._IOption<RAST._IType> _1546___mcc_h647 = _source53.dtor_retType;
                RAST._IExpr _1547___mcc_h648 = _source53.dtor_body;
                return this;
              }
            } else {
              return this;
            }
          } else if (_source52.is_BinaryOp) {
            Dafny.ISequence<Dafny.Rune> _1548___mcc_h652 = _source52.dtor_op2;
            RAST._IExpr _1549___mcc_h653 = _source52.dtor_left;
            RAST._IExpr _1550___mcc_h654 = _source52.dtor_right;
            DAST.Format._IBinaryOpFormat _1551___mcc_h655 = _source52.dtor_format2;
            return this;
          } else if (_source52.is_TypeAscription) {
            RAST._IExpr _1552___mcc_h660 = _source52.dtor_left;
            RAST._IType _1553___mcc_h661 = _source52.dtor_tpe;
            return this;
          } else if (_source52.is_LiteralInt) {
            Dafny.ISequence<Dafny.Rune> _1554___mcc_h664 = _source52.dtor_value;
            return this;
          } else if (_source52.is_LiteralBool) {
            bool _1555___mcc_h666 = _source52.dtor_bvalue;
            return this;
          } else if (_source52.is_LiteralString) {
            Dafny.ISequence<Dafny.Rune> _1556___mcc_h668 = _source52.dtor_value;
            bool _1557___mcc_h669 = _source52.dtor_binary;
            return this;
          } else if (_source52.is_DeclareVar) {
            RAST._IDeclareType _1558___mcc_h672 = _source52.dtor_declareType;
            Dafny.ISequence<Dafny.Rune> _1559___mcc_h673 = _source52.dtor_name;
            Std.Wrappers._IOption<RAST._IType> _1560___mcc_h674 = _source52.dtor_optType;
            Std.Wrappers._IOption<RAST._IExpr> _1561___mcc_h675 = _source52.dtor_optRhs;
            return this;
          } else if (_source52.is_Assign) {
            Std.Wrappers._IOption<RAST._IAssignLhs> _1562___mcc_h680 = _source52.dtor_names;
            RAST._IExpr _1563___mcc_h681 = _source52.dtor_rhs;
            return this;
          } else if (_source52.is_IfExpr) {
            RAST._IExpr _1564___mcc_h684 = _source52.dtor_cond;
            RAST._IExpr _1565___mcc_h685 = _source52.dtor_thn;
            RAST._IExpr _1566___mcc_h686 = _source52.dtor_els;
            return this;
          } else if (_source52.is_Loop) {
            Std.Wrappers._IOption<RAST._IExpr> _1567___mcc_h690 = _source52.dtor_optCond;
            RAST._IExpr _1568___mcc_h691 = _source52.dtor_underlying;
            return this;
          } else if (_source52.is_For) {
            Dafny.ISequence<Dafny.Rune> _1569___mcc_h694 = _source52.dtor_name;
            RAST._IExpr _1570___mcc_h695 = _source52.dtor_range;
            RAST._IExpr _1571___mcc_h696 = _source52.dtor_body;
            return this;
          } else if (_source52.is_Labelled) {
            Dafny.ISequence<Dafny.Rune> _1572___mcc_h700 = _source52.dtor_lbl;
            RAST._IExpr _1573___mcc_h701 = _source52.dtor_underlying;
            return this;
          } else if (_source52.is_Break) {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1574___mcc_h704 = _source52.dtor_optLbl;
            return this;
          } else if (_source52.is_Continue) {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1575___mcc_h706 = _source52.dtor_optLbl;
            return this;
          } else if (_source52.is_Return) {
            Std.Wrappers._IOption<RAST._IExpr> _1576___mcc_h708 = _source52.dtor_optExpr;
            return this;
          } else if (_source52.is_CallType) {
            RAST._IExpr _1577___mcc_h710 = _source52.dtor_obj;
            Dafny.ISequence<RAST._IType> _1578___mcc_h711 = _source52.dtor_typeParameters;
            return this;
          } else if (_source52.is_Call) {
            RAST._IExpr _1579___mcc_h714 = _source52.dtor_obj;
            Dafny.ISequence<RAST._IExpr> _1580___mcc_h715 = _source52.dtor_arguments;
            return this;
          } else if (_source52.is_Select) {
            RAST._IExpr _1581___mcc_h718 = _source52.dtor_obj;
            Dafny.ISequence<Dafny.Rune> _1582___mcc_h719 = _source52.dtor_name;
            return this;
          } else if (_source52.is_MemberSelect) {
            RAST._IExpr _1583___mcc_h722 = _source52.dtor_obj;
            Dafny.ISequence<Dafny.Rune> _1584___mcc_h723 = _source52.dtor_name;
            return this;
          } else {
            Dafny.ISequence<RAST._IFormal> _1585___mcc_h726 = _source52.dtor_params;
            Std.Wrappers._IOption<RAST._IType> _1586___mcc_h727 = _source52.dtor_retType;
            RAST._IExpr _1587___mcc_h728 = _source52.dtor_body;
            return this;
          }
        } else if (_source47.is_Loop) {
          Std.Wrappers._IOption<RAST._IExpr> _1588___mcc_h732 = _source47.dtor_optCond;
          RAST._IExpr _1589___mcc_h733 = _source47.dtor_underlying;
          return this;
        } else if (_source47.is_For) {
          Dafny.ISequence<Dafny.Rune> _1590___mcc_h736 = _source47.dtor_name;
          RAST._IExpr _1591___mcc_h737 = _source47.dtor_range;
          RAST._IExpr _1592___mcc_h738 = _source47.dtor_body;
          return this;
        } else if (_source47.is_Labelled) {
          Dafny.ISequence<Dafny.Rune> _1593___mcc_h742 = _source47.dtor_lbl;
          RAST._IExpr _1594___mcc_h743 = _source47.dtor_underlying;
          return this;
        } else if (_source47.is_Break) {
          Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1595___mcc_h746 = _source47.dtor_optLbl;
          return this;
        } else if (_source47.is_Continue) {
          Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1596___mcc_h748 = _source47.dtor_optLbl;
          return this;
        } else if (_source47.is_Return) {
          Std.Wrappers._IOption<RAST._IExpr> _1597___mcc_h750 = _source47.dtor_optExpr;
          return this;
        } else if (_source47.is_CallType) {
          RAST._IExpr _1598___mcc_h752 = _source47.dtor_obj;
          Dafny.ISequence<RAST._IType> _1599___mcc_h753 = _source47.dtor_typeParameters;
          return this;
        } else if (_source47.is_Call) {
          RAST._IExpr _1600___mcc_h756 = _source47.dtor_obj;
          Dafny.ISequence<RAST._IExpr> _1601___mcc_h757 = _source47.dtor_arguments;
          return this;
        } else if (_source47.is_Select) {
          RAST._IExpr _1602___mcc_h760 = _source47.dtor_obj;
          Dafny.ISequence<Dafny.Rune> _1603___mcc_h761 = _source47.dtor_name;
          return this;
        } else if (_source47.is_MemberSelect) {
          RAST._IExpr _1604___mcc_h764 = _source47.dtor_obj;
          Dafny.ISequence<Dafny.Rune> _1605___mcc_h765 = _source47.dtor_name;
          return this;
        } else {
          Dafny.ISequence<RAST._IFormal> _1606___mcc_h768 = _source47.dtor_params;
          Std.Wrappers._IOption<RAST._IType> _1607___mcc_h769 = _source47.dtor_retType;
          RAST._IExpr _1608___mcc_h770 = _source47.dtor_body;
          return this;
        }
      } else if (_source46.is_Block) {
        RAST._IExpr _1609___mcc_h774 = _source46.dtor_underlying;
        return this;
      } else if (_source46.is_StructBuild) {
        RAST._IExpr _1610___mcc_h776 = _source46.dtor_underlying;
        Dafny.ISequence<RAST._IAssignIdentifier> _1611___mcc_h777 = _source46.dtor_assignments;
        return this;
      } else if (_source46.is_Tuple) {
        Dafny.ISequence<RAST._IExpr> _1612___mcc_h780 = _source46.dtor_arguments;
        return this;
      } else if (_source46.is_UnaryOp) {
        Dafny.ISequence<Dafny.Rune> _1613___mcc_h782 = _source46.dtor_op1;
        RAST._IExpr _1614___mcc_h783 = _source46.dtor_underlying;
        DAST.Format._IUnaryOpFormat _1615___mcc_h784 = _source46.dtor_format;
        if (object.Equals(_1613___mcc_h782, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"))) {
          RAST._IExpr _source56 = _1614___mcc_h783;
          if (_source56.is_RawExpr) {
            Dafny.ISequence<Dafny.Rune> _1616___mcc_h788 = _source56.dtor_content;
            return this;
          } else if (_source56.is_ExprFromType) {
            RAST._IType _1617___mcc_h790 = _source56.dtor_tpe;
            return this;
          } else if (_source56.is_Identifier) {
            Dafny.ISequence<Dafny.Rune> _1618___mcc_h792 = _source56.dtor_name;
            return this;
          } else if (_source56.is_Match) {
            RAST._IExpr _1619___mcc_h794 = _source56.dtor_matchee;
            Dafny.ISequence<RAST._IMatchCase> _1620___mcc_h795 = _source56.dtor_cases;
            return this;
          } else if (_source56.is_StmtExpr) {
            RAST._IExpr _1621___mcc_h798 = _source56.dtor_stmt;
            RAST._IExpr _1622___mcc_h799 = _source56.dtor_rhs;
            return this;
          } else if (_source56.is_Block) {
            RAST._IExpr _1623___mcc_h802 = _source56.dtor_underlying;
            return this;
          } else if (_source56.is_StructBuild) {
            RAST._IExpr _1624___mcc_h804 = _source56.dtor_underlying;
            Dafny.ISequence<RAST._IAssignIdentifier> _1625___mcc_h805 = _source56.dtor_assignments;
            return this;
          } else if (_source56.is_Tuple) {
            Dafny.ISequence<RAST._IExpr> _1626___mcc_h808 = _source56.dtor_arguments;
            return this;
          } else if (_source56.is_UnaryOp) {
            Dafny.ISequence<Dafny.Rune> _1627___mcc_h810 = _source56.dtor_op1;
            RAST._IExpr _1628___mcc_h811 = _source56.dtor_underlying;
            DAST.Format._IUnaryOpFormat _1629___mcc_h812 = _source56.dtor_format;
            return this;
          } else if (_source56.is_BinaryOp) {
            Dafny.ISequence<Dafny.Rune> _1630___mcc_h816 = _source56.dtor_op2;
            RAST._IExpr _1631___mcc_h817 = _source56.dtor_left;
            RAST._IExpr _1632___mcc_h818 = _source56.dtor_right;
            DAST.Format._IBinaryOpFormat _1633___mcc_h819 = _source56.dtor_format2;
            return this;
          } else if (_source56.is_TypeAscription) {
            RAST._IExpr _1634___mcc_h824 = _source56.dtor_left;
            RAST._IType _1635___mcc_h825 = _source56.dtor_tpe;
            return this;
          } else if (_source56.is_LiteralInt) {
            Dafny.ISequence<Dafny.Rune> _1636___mcc_h828 = _source56.dtor_value;
            return this;
          } else if (_source56.is_LiteralBool) {
            bool _1637___mcc_h830 = _source56.dtor_bvalue;
            return this;
          } else if (_source56.is_LiteralString) {
            Dafny.ISequence<Dafny.Rune> _1638___mcc_h832 = _source56.dtor_value;
            bool _1639___mcc_h833 = _source56.dtor_binary;
            return this;
          } else if (_source56.is_DeclareVar) {
            RAST._IDeclareType _1640___mcc_h836 = _source56.dtor_declareType;
            Dafny.ISequence<Dafny.Rune> _1641___mcc_h837 = _source56.dtor_name;
            Std.Wrappers._IOption<RAST._IType> _1642___mcc_h838 = _source56.dtor_optType;
            Std.Wrappers._IOption<RAST._IExpr> _1643___mcc_h839 = _source56.dtor_optRhs;
            return this;
          } else if (_source56.is_Assign) {
            Std.Wrappers._IOption<RAST._IAssignLhs> _1644___mcc_h844 = _source56.dtor_names;
            RAST._IExpr _1645___mcc_h845 = _source56.dtor_rhs;
            return this;
          } else if (_source56.is_IfExpr) {
            RAST._IExpr _1646___mcc_h848 = _source56.dtor_cond;
            RAST._IExpr _1647___mcc_h849 = _source56.dtor_thn;
            RAST._IExpr _1648___mcc_h850 = _source56.dtor_els;
            return this;
          } else if (_source56.is_Loop) {
            Std.Wrappers._IOption<RAST._IExpr> _1649___mcc_h854 = _source56.dtor_optCond;
            RAST._IExpr _1650___mcc_h855 = _source56.dtor_underlying;
            return this;
          } else if (_source56.is_For) {
            Dafny.ISequence<Dafny.Rune> _1651___mcc_h858 = _source56.dtor_name;
            RAST._IExpr _1652___mcc_h859 = _source56.dtor_range;
            RAST._IExpr _1653___mcc_h860 = _source56.dtor_body;
            return this;
          } else if (_source56.is_Labelled) {
            Dafny.ISequence<Dafny.Rune> _1654___mcc_h864 = _source56.dtor_lbl;
            RAST._IExpr _1655___mcc_h865 = _source56.dtor_underlying;
            return this;
          } else if (_source56.is_Break) {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1656___mcc_h868 = _source56.dtor_optLbl;
            return this;
          } else if (_source56.is_Continue) {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1657___mcc_h870 = _source56.dtor_optLbl;
            return this;
          } else if (_source56.is_Return) {
            Std.Wrappers._IOption<RAST._IExpr> _1658___mcc_h872 = _source56.dtor_optExpr;
            return this;
          } else if (_source56.is_CallType) {
            RAST._IExpr _1659___mcc_h874 = _source56.dtor_obj;
            Dafny.ISequence<RAST._IType> _1660___mcc_h875 = _source56.dtor_typeParameters;
            return this;
          } else if (_source56.is_Call) {
            RAST._IExpr _1661___mcc_h878 = _source56.dtor_obj;
            Dafny.ISequence<RAST._IExpr> _1662___mcc_h879 = _source56.dtor_arguments;
            RAST._IExpr _source57 = _1661___mcc_h878;
            if (_source57.is_RawExpr) {
              Dafny.ISequence<Dafny.Rune> _1663___mcc_h882 = _source57.dtor_content;
              return this;
            } else if (_source57.is_ExprFromType) {
              RAST._IType _1664___mcc_h884 = _source57.dtor_tpe;
              return this;
            } else if (_source57.is_Identifier) {
              Dafny.ISequence<Dafny.Rune> _1665___mcc_h886 = _source57.dtor_name;
              return this;
            } else if (_source57.is_Match) {
              RAST._IExpr _1666___mcc_h888 = _source57.dtor_matchee;
              Dafny.ISequence<RAST._IMatchCase> _1667___mcc_h889 = _source57.dtor_cases;
              return this;
            } else if (_source57.is_StmtExpr) {
              RAST._IExpr _1668___mcc_h892 = _source57.dtor_stmt;
              RAST._IExpr _1669___mcc_h893 = _source57.dtor_rhs;
              return this;
            } else if (_source57.is_Block) {
              RAST._IExpr _1670___mcc_h896 = _source57.dtor_underlying;
              return this;
            } else if (_source57.is_StructBuild) {
              RAST._IExpr _1671___mcc_h898 = _source57.dtor_underlying;
              Dafny.ISequence<RAST._IAssignIdentifier> _1672___mcc_h899 = _source57.dtor_assignments;
              return this;
            } else if (_source57.is_Tuple) {
              Dafny.ISequence<RAST._IExpr> _1673___mcc_h902 = _source57.dtor_arguments;
              return this;
            } else if (_source57.is_UnaryOp) {
              Dafny.ISequence<Dafny.Rune> _1674___mcc_h904 = _source57.dtor_op1;
              RAST._IExpr _1675___mcc_h905 = _source57.dtor_underlying;
              DAST.Format._IUnaryOpFormat _1676___mcc_h906 = _source57.dtor_format;
              return this;
            } else if (_source57.is_BinaryOp) {
              Dafny.ISequence<Dafny.Rune> _1677___mcc_h910 = _source57.dtor_op2;
              RAST._IExpr _1678___mcc_h911 = _source57.dtor_left;
              RAST._IExpr _1679___mcc_h912 = _source57.dtor_right;
              DAST.Format._IBinaryOpFormat _1680___mcc_h913 = _source57.dtor_format2;
              return this;
            } else if (_source57.is_TypeAscription) {
              RAST._IExpr _1681___mcc_h918 = _source57.dtor_left;
              RAST._IType _1682___mcc_h919 = _source57.dtor_tpe;
              return this;
            } else if (_source57.is_LiteralInt) {
              Dafny.ISequence<Dafny.Rune> _1683___mcc_h922 = _source57.dtor_value;
              return this;
            } else if (_source57.is_LiteralBool) {
              bool _1684___mcc_h924 = _source57.dtor_bvalue;
              return this;
            } else if (_source57.is_LiteralString) {
              Dafny.ISequence<Dafny.Rune> _1685___mcc_h926 = _source57.dtor_value;
              bool _1686___mcc_h927 = _source57.dtor_binary;
              return this;
            } else if (_source57.is_DeclareVar) {
              RAST._IDeclareType _1687___mcc_h930 = _source57.dtor_declareType;
              Dafny.ISequence<Dafny.Rune> _1688___mcc_h931 = _source57.dtor_name;
              Std.Wrappers._IOption<RAST._IType> _1689___mcc_h932 = _source57.dtor_optType;
              Std.Wrappers._IOption<RAST._IExpr> _1690___mcc_h933 = _source57.dtor_optRhs;
              return this;
            } else if (_source57.is_Assign) {
              Std.Wrappers._IOption<RAST._IAssignLhs> _1691___mcc_h938 = _source57.dtor_names;
              RAST._IExpr _1692___mcc_h939 = _source57.dtor_rhs;
              return this;
            } else if (_source57.is_IfExpr) {
              RAST._IExpr _1693___mcc_h942 = _source57.dtor_cond;
              RAST._IExpr _1694___mcc_h943 = _source57.dtor_thn;
              RAST._IExpr _1695___mcc_h944 = _source57.dtor_els;
              return this;
            } else if (_source57.is_Loop) {
              Std.Wrappers._IOption<RAST._IExpr> _1696___mcc_h948 = _source57.dtor_optCond;
              RAST._IExpr _1697___mcc_h949 = _source57.dtor_underlying;
              return this;
            } else if (_source57.is_For) {
              Dafny.ISequence<Dafny.Rune> _1698___mcc_h952 = _source57.dtor_name;
              RAST._IExpr _1699___mcc_h953 = _source57.dtor_range;
              RAST._IExpr _1700___mcc_h954 = _source57.dtor_body;
              return this;
            } else if (_source57.is_Labelled) {
              Dafny.ISequence<Dafny.Rune> _1701___mcc_h958 = _source57.dtor_lbl;
              RAST._IExpr _1702___mcc_h959 = _source57.dtor_underlying;
              return this;
            } else if (_source57.is_Break) {
              Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1703___mcc_h962 = _source57.dtor_optLbl;
              return this;
            } else if (_source57.is_Continue) {
              Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1704___mcc_h964 = _source57.dtor_optLbl;
              return this;
            } else if (_source57.is_Return) {
              Std.Wrappers._IOption<RAST._IExpr> _1705___mcc_h966 = _source57.dtor_optExpr;
              return this;
            } else if (_source57.is_CallType) {
              RAST._IExpr _1706___mcc_h968 = _source57.dtor_obj;
              Dafny.ISequence<RAST._IType> _1707___mcc_h969 = _source57.dtor_typeParameters;
              return this;
            } else if (_source57.is_Call) {
              RAST._IExpr _1708___mcc_h972 = _source57.dtor_obj;
              Dafny.ISequence<RAST._IExpr> _1709___mcc_h973 = _source57.dtor_arguments;
              return this;
            } else if (_source57.is_Select) {
              RAST._IExpr _1710___mcc_h976 = _source57.dtor_obj;
              Dafny.ISequence<Dafny.Rune> _1711___mcc_h977 = _source57.dtor_name;
              if (object.Equals(_1711___mcc_h977, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("clone"))) {
                DAST.Format._IUnaryOpFormat _1712_format = _1615___mcc_h784;
                Dafny.ISequence<RAST._IExpr> _1713_args = _1662___mcc_h879;
                RAST._IExpr _1714_underlying = _1710___mcc_h976;
                if ((_1713_args).Equals(Dafny.Sequence<RAST._IExpr>.FromElements())) {
                  return RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), _1714_underlying, _1712_format);
                } else {
                  return this;
                }
              } else {
                return this;
              }
            } else if (_source57.is_MemberSelect) {
              RAST._IExpr _1715___mcc_h980 = _source57.dtor_obj;
              Dafny.ISequence<Dafny.Rune> _1716___mcc_h981 = _source57.dtor_name;
              return this;
            } else {
              Dafny.ISequence<RAST._IFormal> _1717___mcc_h984 = _source57.dtor_params;
              Std.Wrappers._IOption<RAST._IType> _1718___mcc_h985 = _source57.dtor_retType;
              RAST._IExpr _1719___mcc_h986 = _source57.dtor_body;
              return this;
            }
          } else if (_source56.is_Select) {
            RAST._IExpr _1720___mcc_h990 = _source56.dtor_obj;
            Dafny.ISequence<Dafny.Rune> _1721___mcc_h991 = _source56.dtor_name;
            return this;
          } else if (_source56.is_MemberSelect) {
            RAST._IExpr _1722___mcc_h994 = _source56.dtor_obj;
            Dafny.ISequence<Dafny.Rune> _1723___mcc_h995 = _source56.dtor_name;
            return this;
          } else {
            Dafny.ISequence<RAST._IFormal> _1724___mcc_h998 = _source56.dtor_params;
            Std.Wrappers._IOption<RAST._IType> _1725___mcc_h999 = _source56.dtor_retType;
            RAST._IExpr _1726___mcc_h1000 = _source56.dtor_body;
            return this;
          }
        } else if (object.Equals(_1613___mcc_h782, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"))) {
          RAST._IExpr _source58 = _1614___mcc_h783;
          if (_source58.is_RawExpr) {
            Dafny.ISequence<Dafny.Rune> _1727___mcc_h1004 = _source58.dtor_content;
            return this;
          } else if (_source58.is_ExprFromType) {
            RAST._IType _1728___mcc_h1006 = _source58.dtor_tpe;
            return this;
          } else if (_source58.is_Identifier) {
            Dafny.ISequence<Dafny.Rune> _1729___mcc_h1008 = _source58.dtor_name;
            return this;
          } else if (_source58.is_Match) {
            RAST._IExpr _1730___mcc_h1010 = _source58.dtor_matchee;
            Dafny.ISequence<RAST._IMatchCase> _1731___mcc_h1011 = _source58.dtor_cases;
            return this;
          } else if (_source58.is_StmtExpr) {
            RAST._IExpr _1732___mcc_h1014 = _source58.dtor_stmt;
            RAST._IExpr _1733___mcc_h1015 = _source58.dtor_rhs;
            return this;
          } else if (_source58.is_Block) {
            RAST._IExpr _1734___mcc_h1018 = _source58.dtor_underlying;
            return this;
          } else if (_source58.is_StructBuild) {
            RAST._IExpr _1735___mcc_h1020 = _source58.dtor_underlying;
            Dafny.ISequence<RAST._IAssignIdentifier> _1736___mcc_h1021 = _source58.dtor_assignments;
            return this;
          } else if (_source58.is_Tuple) {
            Dafny.ISequence<RAST._IExpr> _1737___mcc_h1024 = _source58.dtor_arguments;
            return this;
          } else if (_source58.is_UnaryOp) {
            Dafny.ISequence<Dafny.Rune> _1738___mcc_h1026 = _source58.dtor_op1;
            RAST._IExpr _1739___mcc_h1027 = _source58.dtor_underlying;
            DAST.Format._IUnaryOpFormat _1740___mcc_h1028 = _source58.dtor_format;
            return this;
          } else if (_source58.is_BinaryOp) {
            Dafny.ISequence<Dafny.Rune> _1741___mcc_h1032 = _source58.dtor_op2;
            RAST._IExpr _1742___mcc_h1033 = _source58.dtor_left;
            RAST._IExpr _1743___mcc_h1034 = _source58.dtor_right;
            DAST.Format._IBinaryOpFormat _1744___mcc_h1035 = _source58.dtor_format2;
            if (object.Equals(_1741___mcc_h1032, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="))) {
              DAST.Format._IUnaryOpFormat _source59 = _1615___mcc_h784;
              if (_source59.is_NoFormat) {
                return this;
              } else {
                DAST.Format._IBinaryOpFormat _1745_format = _1744___mcc_h1035;
                RAST._IExpr _1746_right = _1743___mcc_h1034;
                RAST._IExpr _1747_left = _1742___mcc_h1033;
                return RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!="), _1747_left, _1746_right, DAST.Format.BinaryOpFormat.create_NoFormat());
              }
            } else if (object.Equals(_1741___mcc_h1032, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"))) {
              DAST.Format._IBinaryOpFormat _source60 = _1744___mcc_h1035;
              if (_source60.is_NoFormat) {
                DAST.Format._IUnaryOpFormat _source61 = _1615___mcc_h784;
                if (_source61.is_NoFormat) {
                  return this;
                } else {
                  RAST._IExpr _1748_right = _1743___mcc_h1034;
                  RAST._IExpr _1749_left = _1742___mcc_h1033;
                  return RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">="), _1749_left, _1748_right, DAST.Format.BinaryOpFormat.create_NoFormat());
                }
              } else if (_source60.is_ImpliesFormat) {
                return this;
              } else if (_source60.is_EquivalenceFormat) {
                return this;
              } else {
                DAST.Format._IUnaryOpFormat _source62 = _1615___mcc_h784;
                if (_source62.is_NoFormat) {
                  return this;
                } else {
                  RAST._IExpr _1750_right = _1743___mcc_h1034;
                  RAST._IExpr _1751_left = _1742___mcc_h1033;
                  return RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<="), _1750_right, _1751_left, DAST.Format.BinaryOpFormat.create_NoFormat());
                }
              }
            } else {
              return this;
            }
          } else if (_source58.is_TypeAscription) {
            RAST._IExpr _1752___mcc_h1040 = _source58.dtor_left;
            RAST._IType _1753___mcc_h1041 = _source58.dtor_tpe;
            return this;
          } else if (_source58.is_LiteralInt) {
            Dafny.ISequence<Dafny.Rune> _1754___mcc_h1044 = _source58.dtor_value;
            return this;
          } else if (_source58.is_LiteralBool) {
            bool _1755___mcc_h1046 = _source58.dtor_bvalue;
            return this;
          } else if (_source58.is_LiteralString) {
            Dafny.ISequence<Dafny.Rune> _1756___mcc_h1048 = _source58.dtor_value;
            bool _1757___mcc_h1049 = _source58.dtor_binary;
            return this;
          } else if (_source58.is_DeclareVar) {
            RAST._IDeclareType _1758___mcc_h1052 = _source58.dtor_declareType;
            Dafny.ISequence<Dafny.Rune> _1759___mcc_h1053 = _source58.dtor_name;
            Std.Wrappers._IOption<RAST._IType> _1760___mcc_h1054 = _source58.dtor_optType;
            Std.Wrappers._IOption<RAST._IExpr> _1761___mcc_h1055 = _source58.dtor_optRhs;
            return this;
          } else if (_source58.is_Assign) {
            Std.Wrappers._IOption<RAST._IAssignLhs> _1762___mcc_h1060 = _source58.dtor_names;
            RAST._IExpr _1763___mcc_h1061 = _source58.dtor_rhs;
            return this;
          } else if (_source58.is_IfExpr) {
            RAST._IExpr _1764___mcc_h1064 = _source58.dtor_cond;
            RAST._IExpr _1765___mcc_h1065 = _source58.dtor_thn;
            RAST._IExpr _1766___mcc_h1066 = _source58.dtor_els;
            return this;
          } else if (_source58.is_Loop) {
            Std.Wrappers._IOption<RAST._IExpr> _1767___mcc_h1070 = _source58.dtor_optCond;
            RAST._IExpr _1768___mcc_h1071 = _source58.dtor_underlying;
            return this;
          } else if (_source58.is_For) {
            Dafny.ISequence<Dafny.Rune> _1769___mcc_h1074 = _source58.dtor_name;
            RAST._IExpr _1770___mcc_h1075 = _source58.dtor_range;
            RAST._IExpr _1771___mcc_h1076 = _source58.dtor_body;
            return this;
          } else if (_source58.is_Labelled) {
            Dafny.ISequence<Dafny.Rune> _1772___mcc_h1080 = _source58.dtor_lbl;
            RAST._IExpr _1773___mcc_h1081 = _source58.dtor_underlying;
            return this;
          } else if (_source58.is_Break) {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1774___mcc_h1084 = _source58.dtor_optLbl;
            return this;
          } else if (_source58.is_Continue) {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1775___mcc_h1086 = _source58.dtor_optLbl;
            return this;
          } else if (_source58.is_Return) {
            Std.Wrappers._IOption<RAST._IExpr> _1776___mcc_h1088 = _source58.dtor_optExpr;
            return this;
          } else if (_source58.is_CallType) {
            RAST._IExpr _1777___mcc_h1090 = _source58.dtor_obj;
            Dafny.ISequence<RAST._IType> _1778___mcc_h1091 = _source58.dtor_typeParameters;
            return this;
          } else if (_source58.is_Call) {
            RAST._IExpr _1779___mcc_h1094 = _source58.dtor_obj;
            Dafny.ISequence<RAST._IExpr> _1780___mcc_h1095 = _source58.dtor_arguments;
            return this;
          } else if (_source58.is_Select) {
            RAST._IExpr _1781___mcc_h1098 = _source58.dtor_obj;
            Dafny.ISequence<Dafny.Rune> _1782___mcc_h1099 = _source58.dtor_name;
            return this;
          } else if (_source58.is_MemberSelect) {
            RAST._IExpr _1783___mcc_h1102 = _source58.dtor_obj;
            Dafny.ISequence<Dafny.Rune> _1784___mcc_h1103 = _source58.dtor_name;
            return this;
          } else {
            Dafny.ISequence<RAST._IFormal> _1785___mcc_h1106 = _source58.dtor_params;
            Std.Wrappers._IOption<RAST._IType> _1786___mcc_h1107 = _source58.dtor_retType;
            RAST._IExpr _1787___mcc_h1108 = _source58.dtor_body;
            return this;
          }
        } else {
          return this;
        }
      } else if (_source46.is_BinaryOp) {
        Dafny.ISequence<Dafny.Rune> _1788___mcc_h1112 = _source46.dtor_op2;
        RAST._IExpr _1789___mcc_h1113 = _source46.dtor_left;
        RAST._IExpr _1790___mcc_h1114 = _source46.dtor_right;
        DAST.Format._IBinaryOpFormat _1791___mcc_h1115 = _source46.dtor_format2;
        return this;
      } else if (_source46.is_TypeAscription) {
        RAST._IExpr _1792___mcc_h1120 = _source46.dtor_left;
        RAST._IType _1793___mcc_h1121 = _source46.dtor_tpe;
        return this;
      } else if (_source46.is_LiteralInt) {
        Dafny.ISequence<Dafny.Rune> _1794___mcc_h1124 = _source46.dtor_value;
        return this;
      } else if (_source46.is_LiteralBool) {
        bool _1795___mcc_h1126 = _source46.dtor_bvalue;
        return this;
      } else if (_source46.is_LiteralString) {
        Dafny.ISequence<Dafny.Rune> _1796___mcc_h1128 = _source46.dtor_value;
        bool _1797___mcc_h1129 = _source46.dtor_binary;
        return this;
      } else if (_source46.is_DeclareVar) {
        RAST._IDeclareType _1798___mcc_h1132 = _source46.dtor_declareType;
        Dafny.ISequence<Dafny.Rune> _1799___mcc_h1133 = _source46.dtor_name;
        Std.Wrappers._IOption<RAST._IType> _1800___mcc_h1134 = _source46.dtor_optType;
        Std.Wrappers._IOption<RAST._IExpr> _1801___mcc_h1135 = _source46.dtor_optRhs;
        return this;
      } else if (_source46.is_Assign) {
        Std.Wrappers._IOption<RAST._IAssignLhs> _1802___mcc_h1140 = _source46.dtor_names;
        RAST._IExpr _1803___mcc_h1141 = _source46.dtor_rhs;
        return this;
      } else if (_source46.is_IfExpr) {
        RAST._IExpr _1804___mcc_h1144 = _source46.dtor_cond;
        RAST._IExpr _1805___mcc_h1145 = _source46.dtor_thn;
        RAST._IExpr _1806___mcc_h1146 = _source46.dtor_els;
        return this;
      } else if (_source46.is_Loop) {
        Std.Wrappers._IOption<RAST._IExpr> _1807___mcc_h1150 = _source46.dtor_optCond;
        RAST._IExpr _1808___mcc_h1151 = _source46.dtor_underlying;
        return this;
      } else if (_source46.is_For) {
        Dafny.ISequence<Dafny.Rune> _1809___mcc_h1154 = _source46.dtor_name;
        RAST._IExpr _1810___mcc_h1155 = _source46.dtor_range;
        RAST._IExpr _1811___mcc_h1156 = _source46.dtor_body;
        return this;
      } else if (_source46.is_Labelled) {
        Dafny.ISequence<Dafny.Rune> _1812___mcc_h1160 = _source46.dtor_lbl;
        RAST._IExpr _1813___mcc_h1161 = _source46.dtor_underlying;
        return this;
      } else if (_source46.is_Break) {
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1814___mcc_h1164 = _source46.dtor_optLbl;
        return this;
      } else if (_source46.is_Continue) {
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1815___mcc_h1166 = _source46.dtor_optLbl;
        return this;
      } else if (_source46.is_Return) {
        Std.Wrappers._IOption<RAST._IExpr> _1816___mcc_h1168 = _source46.dtor_optExpr;
        return this;
      } else if (_source46.is_CallType) {
        RAST._IExpr _1817___mcc_h1170 = _source46.dtor_obj;
        Dafny.ISequence<RAST._IType> _1818___mcc_h1171 = _source46.dtor_typeParameters;
        return this;
      } else if (_source46.is_Call) {
        RAST._IExpr _1819___mcc_h1174 = _source46.dtor_obj;
        Dafny.ISequence<RAST._IExpr> _1820___mcc_h1175 = _source46.dtor_arguments;
        RAST._IExpr _source63 = _1819___mcc_h1174;
        if (_source63.is_RawExpr) {
          Dafny.ISequence<Dafny.Rune> _1821___mcc_h1178 = _source63.dtor_content;
          return this;
        } else if (_source63.is_ExprFromType) {
          RAST._IType _1822___mcc_h1180 = _source63.dtor_tpe;
          return this;
        } else if (_source63.is_Identifier) {
          Dafny.ISequence<Dafny.Rune> _1823___mcc_h1182 = _source63.dtor_name;
          return this;
        } else if (_source63.is_Match) {
          RAST._IExpr _1824___mcc_h1184 = _source63.dtor_matchee;
          Dafny.ISequence<RAST._IMatchCase> _1825___mcc_h1185 = _source63.dtor_cases;
          return this;
        } else if (_source63.is_StmtExpr) {
          RAST._IExpr _1826___mcc_h1188 = _source63.dtor_stmt;
          RAST._IExpr _1827___mcc_h1189 = _source63.dtor_rhs;
          return this;
        } else if (_source63.is_Block) {
          RAST._IExpr _1828___mcc_h1192 = _source63.dtor_underlying;
          return this;
        } else if (_source63.is_StructBuild) {
          RAST._IExpr _1829___mcc_h1194 = _source63.dtor_underlying;
          Dafny.ISequence<RAST._IAssignIdentifier> _1830___mcc_h1195 = _source63.dtor_assignments;
          return this;
        } else if (_source63.is_Tuple) {
          Dafny.ISequence<RAST._IExpr> _1831___mcc_h1198 = _source63.dtor_arguments;
          return this;
        } else if (_source63.is_UnaryOp) {
          Dafny.ISequence<Dafny.Rune> _1832___mcc_h1200 = _source63.dtor_op1;
          RAST._IExpr _1833___mcc_h1201 = _source63.dtor_underlying;
          DAST.Format._IUnaryOpFormat _1834___mcc_h1202 = _source63.dtor_format;
          return this;
        } else if (_source63.is_BinaryOp) {
          Dafny.ISequence<Dafny.Rune> _1835___mcc_h1206 = _source63.dtor_op2;
          RAST._IExpr _1836___mcc_h1207 = _source63.dtor_left;
          RAST._IExpr _1837___mcc_h1208 = _source63.dtor_right;
          DAST.Format._IBinaryOpFormat _1838___mcc_h1209 = _source63.dtor_format2;
          return this;
        } else if (_source63.is_TypeAscription) {
          RAST._IExpr _1839___mcc_h1214 = _source63.dtor_left;
          RAST._IType _1840___mcc_h1215 = _source63.dtor_tpe;
          return this;
        } else if (_source63.is_LiteralInt) {
          Dafny.ISequence<Dafny.Rune> _1841___mcc_h1218 = _source63.dtor_value;
          return this;
        } else if (_source63.is_LiteralBool) {
          bool _1842___mcc_h1220 = _source63.dtor_bvalue;
          return this;
        } else if (_source63.is_LiteralString) {
          Dafny.ISequence<Dafny.Rune> _1843___mcc_h1222 = _source63.dtor_value;
          bool _1844___mcc_h1223 = _source63.dtor_binary;
          return this;
        } else if (_source63.is_DeclareVar) {
          RAST._IDeclareType _1845___mcc_h1226 = _source63.dtor_declareType;
          Dafny.ISequence<Dafny.Rune> _1846___mcc_h1227 = _source63.dtor_name;
          Std.Wrappers._IOption<RAST._IType> _1847___mcc_h1228 = _source63.dtor_optType;
          Std.Wrappers._IOption<RAST._IExpr> _1848___mcc_h1229 = _source63.dtor_optRhs;
          return this;
        } else if (_source63.is_Assign) {
          Std.Wrappers._IOption<RAST._IAssignLhs> _1849___mcc_h1234 = _source63.dtor_names;
          RAST._IExpr _1850___mcc_h1235 = _source63.dtor_rhs;
          return this;
        } else if (_source63.is_IfExpr) {
          RAST._IExpr _1851___mcc_h1238 = _source63.dtor_cond;
          RAST._IExpr _1852___mcc_h1239 = _source63.dtor_thn;
          RAST._IExpr _1853___mcc_h1240 = _source63.dtor_els;
          return this;
        } else if (_source63.is_Loop) {
          Std.Wrappers._IOption<RAST._IExpr> _1854___mcc_h1244 = _source63.dtor_optCond;
          RAST._IExpr _1855___mcc_h1245 = _source63.dtor_underlying;
          return this;
        } else if (_source63.is_For) {
          Dafny.ISequence<Dafny.Rune> _1856___mcc_h1248 = _source63.dtor_name;
          RAST._IExpr _1857___mcc_h1249 = _source63.dtor_range;
          RAST._IExpr _1858___mcc_h1250 = _source63.dtor_body;
          return this;
        } else if (_source63.is_Labelled) {
          Dafny.ISequence<Dafny.Rune> _1859___mcc_h1254 = _source63.dtor_lbl;
          RAST._IExpr _1860___mcc_h1255 = _source63.dtor_underlying;
          return this;
        } else if (_source63.is_Break) {
          Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1861___mcc_h1258 = _source63.dtor_optLbl;
          return this;
        } else if (_source63.is_Continue) {
          Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1862___mcc_h1260 = _source63.dtor_optLbl;
          return this;
        } else if (_source63.is_Return) {
          Std.Wrappers._IOption<RAST._IExpr> _1863___mcc_h1262 = _source63.dtor_optExpr;
          return this;
        } else if (_source63.is_CallType) {
          RAST._IExpr _1864___mcc_h1264 = _source63.dtor_obj;
          Dafny.ISequence<RAST._IType> _1865___mcc_h1265 = _source63.dtor_typeParameters;
          return this;
        } else if (_source63.is_Call) {
          RAST._IExpr _1866___mcc_h1268 = _source63.dtor_obj;
          Dafny.ISequence<RAST._IExpr> _1867___mcc_h1269 = _source63.dtor_arguments;
          return this;
        } else if (_source63.is_Select) {
          RAST._IExpr _1868___mcc_h1272 = _source63.dtor_obj;
          Dafny.ISequence<Dafny.Rune> _1869___mcc_h1273 = _source63.dtor_name;
          return this;
        } else if (_source63.is_MemberSelect) {
          RAST._IExpr _1870___mcc_h1276 = _source63.dtor_obj;
          Dafny.ISequence<Dafny.Rune> _1871___mcc_h1277 = _source63.dtor_name;
          if (object.Equals(_1871___mcc_h1277, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("truncate!"))) {
            Dafny.ISequence<RAST._IExpr> _1872_args = _1820___mcc_h1175;
            RAST._IExpr _1873_r = _1870___mcc_h1276;
            if (((!object.Equals(_1873_r, RAST.__default.dafny__runtime)) && (!object.Equals(_1873_r, RAST.__default.@global))) || ((new BigInteger((_1872_args).Count)) != (new BigInteger(2)))) {
              return this;
            } else {
              RAST._IExpr _1874_expr = (_1872_args).Select(BigInteger.Zero);
              RAST._IExpr _1875_tpeExpr = (_1872_args).Select(BigInteger.One);
              if (!((_1875_tpeExpr).is_ExprFromType)) {
                return this;
              } else {
                RAST._IType _1876_tpe = (_1875_tpeExpr).dtor_tpe;
                if (((((((((((_1876_tpe).is_U8) || ((_1876_tpe).is_U16)) || ((_1876_tpe).is_U32)) || ((_1876_tpe).is_U64)) || ((_1876_tpe).is_U128)) || ((_1876_tpe).is_I8)) || ((_1876_tpe).is_I16)) || ((_1876_tpe).is_I32)) || ((_1876_tpe).is_I64)) || ((_1876_tpe).is_I128)) {
                  RAST._IExpr _source64 = _1874_expr;
                  if (_source64.is_RawExpr) {
                    Dafny.ISequence<Dafny.Rune> _1877___mcc_h1300 = _source64.dtor_content;
                    return this;
                  } else if (_source64.is_ExprFromType) {
                    RAST._IType _1878___mcc_h1302 = _source64.dtor_tpe;
                    return this;
                  } else if (_source64.is_Identifier) {
                    Dafny.ISequence<Dafny.Rune> _1879___mcc_h1304 = _source64.dtor_name;
                    return this;
                  } else if (_source64.is_Match) {
                    RAST._IExpr _1880___mcc_h1306 = _source64.dtor_matchee;
                    Dafny.ISequence<RAST._IMatchCase> _1881___mcc_h1307 = _source64.dtor_cases;
                    return this;
                  } else if (_source64.is_StmtExpr) {
                    RAST._IExpr _1882___mcc_h1310 = _source64.dtor_stmt;
                    RAST._IExpr _1883___mcc_h1311 = _source64.dtor_rhs;
                    return this;
                  } else if (_source64.is_Block) {
                    RAST._IExpr _1884___mcc_h1314 = _source64.dtor_underlying;
                    return this;
                  } else if (_source64.is_StructBuild) {
                    RAST._IExpr _1885___mcc_h1316 = _source64.dtor_underlying;
                    Dafny.ISequence<RAST._IAssignIdentifier> _1886___mcc_h1317 = _source64.dtor_assignments;
                    return this;
                  } else if (_source64.is_Tuple) {
                    Dafny.ISequence<RAST._IExpr> _1887___mcc_h1320 = _source64.dtor_arguments;
                    return this;
                  } else if (_source64.is_UnaryOp) {
                    Dafny.ISequence<Dafny.Rune> _1888___mcc_h1322 = _source64.dtor_op1;
                    RAST._IExpr _1889___mcc_h1323 = _source64.dtor_underlying;
                    DAST.Format._IUnaryOpFormat _1890___mcc_h1324 = _source64.dtor_format;
                    return this;
                  } else if (_source64.is_BinaryOp) {
                    Dafny.ISequence<Dafny.Rune> _1891___mcc_h1328 = _source64.dtor_op2;
                    RAST._IExpr _1892___mcc_h1329 = _source64.dtor_left;
                    RAST._IExpr _1893___mcc_h1330 = _source64.dtor_right;
                    DAST.Format._IBinaryOpFormat _1894___mcc_h1331 = _source64.dtor_format2;
                    return this;
                  } else if (_source64.is_TypeAscription) {
                    RAST._IExpr _1895___mcc_h1336 = _source64.dtor_left;
                    RAST._IType _1896___mcc_h1337 = _source64.dtor_tpe;
                    return this;
                  } else if (_source64.is_LiteralInt) {
                    Dafny.ISequence<Dafny.Rune> _1897___mcc_h1340 = _source64.dtor_value;
                    return this;
                  } else if (_source64.is_LiteralBool) {
                    bool _1898___mcc_h1342 = _source64.dtor_bvalue;
                    return this;
                  } else if (_source64.is_LiteralString) {
                    Dafny.ISequence<Dafny.Rune> _1899___mcc_h1344 = _source64.dtor_value;
                    bool _1900___mcc_h1345 = _source64.dtor_binary;
                    return this;
                  } else if (_source64.is_DeclareVar) {
                    RAST._IDeclareType _1901___mcc_h1348 = _source64.dtor_declareType;
                    Dafny.ISequence<Dafny.Rune> _1902___mcc_h1349 = _source64.dtor_name;
                    Std.Wrappers._IOption<RAST._IType> _1903___mcc_h1350 = _source64.dtor_optType;
                    Std.Wrappers._IOption<RAST._IExpr> _1904___mcc_h1351 = _source64.dtor_optRhs;
                    return this;
                  } else if (_source64.is_Assign) {
                    Std.Wrappers._IOption<RAST._IAssignLhs> _1905___mcc_h1356 = _source64.dtor_names;
                    RAST._IExpr _1906___mcc_h1357 = _source64.dtor_rhs;
                    return this;
                  } else if (_source64.is_IfExpr) {
                    RAST._IExpr _1907___mcc_h1360 = _source64.dtor_cond;
                    RAST._IExpr _1908___mcc_h1361 = _source64.dtor_thn;
                    RAST._IExpr _1909___mcc_h1362 = _source64.dtor_els;
                    return this;
                  } else if (_source64.is_Loop) {
                    Std.Wrappers._IOption<RAST._IExpr> _1910___mcc_h1366 = _source64.dtor_optCond;
                    RAST._IExpr _1911___mcc_h1367 = _source64.dtor_underlying;
                    return this;
                  } else if (_source64.is_For) {
                    Dafny.ISequence<Dafny.Rune> _1912___mcc_h1370 = _source64.dtor_name;
                    RAST._IExpr _1913___mcc_h1371 = _source64.dtor_range;
                    RAST._IExpr _1914___mcc_h1372 = _source64.dtor_body;
                    return this;
                  } else if (_source64.is_Labelled) {
                    Dafny.ISequence<Dafny.Rune> _1915___mcc_h1376 = _source64.dtor_lbl;
                    RAST._IExpr _1916___mcc_h1377 = _source64.dtor_underlying;
                    return this;
                  } else if (_source64.is_Break) {
                    Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1917___mcc_h1380 = _source64.dtor_optLbl;
                    return this;
                  } else if (_source64.is_Continue) {
                    Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1918___mcc_h1382 = _source64.dtor_optLbl;
                    return this;
                  } else if (_source64.is_Return) {
                    Std.Wrappers._IOption<RAST._IExpr> _1919___mcc_h1384 = _source64.dtor_optExpr;
                    return this;
                  } else if (_source64.is_CallType) {
                    RAST._IExpr _1920___mcc_h1386 = _source64.dtor_obj;
                    Dafny.ISequence<RAST._IType> _1921___mcc_h1387 = _source64.dtor_typeParameters;
                    return this;
                  } else if (_source64.is_Call) {
                    RAST._IExpr _1922___mcc_h1390 = _source64.dtor_obj;
                    Dafny.ISequence<RAST._IExpr> _1923___mcc_h1391 = _source64.dtor_arguments;
                    RAST._IExpr _source65 = _1922___mcc_h1390;
                    if (_source65.is_RawExpr) {
                      Dafny.ISequence<Dafny.Rune> _1924___mcc_h1394 = _source65.dtor_content;
                      return this;
                    } else if (_source65.is_ExprFromType) {
                      RAST._IType _1925___mcc_h1396 = _source65.dtor_tpe;
                      return this;
                    } else if (_source65.is_Identifier) {
                      Dafny.ISequence<Dafny.Rune> _1926___mcc_h1398 = _source65.dtor_name;
                      return this;
                    } else if (_source65.is_Match) {
                      RAST._IExpr _1927___mcc_h1400 = _source65.dtor_matchee;
                      Dafny.ISequence<RAST._IMatchCase> _1928___mcc_h1401 = _source65.dtor_cases;
                      return this;
                    } else if (_source65.is_StmtExpr) {
                      RAST._IExpr _1929___mcc_h1404 = _source65.dtor_stmt;
                      RAST._IExpr _1930___mcc_h1405 = _source65.dtor_rhs;
                      return this;
                    } else if (_source65.is_Block) {
                      RAST._IExpr _1931___mcc_h1408 = _source65.dtor_underlying;
                      return this;
                    } else if (_source65.is_StructBuild) {
                      RAST._IExpr _1932___mcc_h1410 = _source65.dtor_underlying;
                      Dafny.ISequence<RAST._IAssignIdentifier> _1933___mcc_h1411 = _source65.dtor_assignments;
                      return this;
                    } else if (_source65.is_Tuple) {
                      Dafny.ISequence<RAST._IExpr> _1934___mcc_h1414 = _source65.dtor_arguments;
                      return this;
                    } else if (_source65.is_UnaryOp) {
                      Dafny.ISequence<Dafny.Rune> _1935___mcc_h1416 = _source65.dtor_op1;
                      RAST._IExpr _1936___mcc_h1417 = _source65.dtor_underlying;
                      DAST.Format._IUnaryOpFormat _1937___mcc_h1418 = _source65.dtor_format;
                      return this;
                    } else if (_source65.is_BinaryOp) {
                      Dafny.ISequence<Dafny.Rune> _1938___mcc_h1422 = _source65.dtor_op2;
                      RAST._IExpr _1939___mcc_h1423 = _source65.dtor_left;
                      RAST._IExpr _1940___mcc_h1424 = _source65.dtor_right;
                      DAST.Format._IBinaryOpFormat _1941___mcc_h1425 = _source65.dtor_format2;
                      return this;
                    } else if (_source65.is_TypeAscription) {
                      RAST._IExpr _1942___mcc_h1430 = _source65.dtor_left;
                      RAST._IType _1943___mcc_h1431 = _source65.dtor_tpe;
                      return this;
                    } else if (_source65.is_LiteralInt) {
                      Dafny.ISequence<Dafny.Rune> _1944___mcc_h1434 = _source65.dtor_value;
                      return this;
                    } else if (_source65.is_LiteralBool) {
                      bool _1945___mcc_h1436 = _source65.dtor_bvalue;
                      return this;
                    } else if (_source65.is_LiteralString) {
                      Dafny.ISequence<Dafny.Rune> _1946___mcc_h1438 = _source65.dtor_value;
                      bool _1947___mcc_h1439 = _source65.dtor_binary;
                      return this;
                    } else if (_source65.is_DeclareVar) {
                      RAST._IDeclareType _1948___mcc_h1442 = _source65.dtor_declareType;
                      Dafny.ISequence<Dafny.Rune> _1949___mcc_h1443 = _source65.dtor_name;
                      Std.Wrappers._IOption<RAST._IType> _1950___mcc_h1444 = _source65.dtor_optType;
                      Std.Wrappers._IOption<RAST._IExpr> _1951___mcc_h1445 = _source65.dtor_optRhs;
                      return this;
                    } else if (_source65.is_Assign) {
                      Std.Wrappers._IOption<RAST._IAssignLhs> _1952___mcc_h1450 = _source65.dtor_names;
                      RAST._IExpr _1953___mcc_h1451 = _source65.dtor_rhs;
                      return this;
                    } else if (_source65.is_IfExpr) {
                      RAST._IExpr _1954___mcc_h1454 = _source65.dtor_cond;
                      RAST._IExpr _1955___mcc_h1455 = _source65.dtor_thn;
                      RAST._IExpr _1956___mcc_h1456 = _source65.dtor_els;
                      return this;
                    } else if (_source65.is_Loop) {
                      Std.Wrappers._IOption<RAST._IExpr> _1957___mcc_h1460 = _source65.dtor_optCond;
                      RAST._IExpr _1958___mcc_h1461 = _source65.dtor_underlying;
                      return this;
                    } else if (_source65.is_For) {
                      Dafny.ISequence<Dafny.Rune> _1959___mcc_h1464 = _source65.dtor_name;
                      RAST._IExpr _1960___mcc_h1465 = _source65.dtor_range;
                      RAST._IExpr _1961___mcc_h1466 = _source65.dtor_body;
                      return this;
                    } else if (_source65.is_Labelled) {
                      Dafny.ISequence<Dafny.Rune> _1962___mcc_h1470 = _source65.dtor_lbl;
                      RAST._IExpr _1963___mcc_h1471 = _source65.dtor_underlying;
                      return this;
                    } else if (_source65.is_Break) {
                      Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1964___mcc_h1474 = _source65.dtor_optLbl;
                      return this;
                    } else if (_source65.is_Continue) {
                      Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1965___mcc_h1476 = _source65.dtor_optLbl;
                      return this;
                    } else if (_source65.is_Return) {
                      Std.Wrappers._IOption<RAST._IExpr> _1966___mcc_h1478 = _source65.dtor_optExpr;
                      return this;
                    } else if (_source65.is_CallType) {
                      RAST._IExpr _1967___mcc_h1480 = _source65.dtor_obj;
                      Dafny.ISequence<RAST._IType> _1968___mcc_h1481 = _source65.dtor_typeParameters;
                      return this;
                    } else if (_source65.is_Call) {
                      RAST._IExpr _1969___mcc_h1484 = _source65.dtor_obj;
                      Dafny.ISequence<RAST._IExpr> _1970___mcc_h1485 = _source65.dtor_arguments;
                      return this;
                    } else if (_source65.is_Select) {
                      RAST._IExpr _1971___mcc_h1488 = _source65.dtor_obj;
                      Dafny.ISequence<Dafny.Rune> _1972___mcc_h1489 = _source65.dtor_name;
                      return this;
                    } else if (_source65.is_MemberSelect) {
                      RAST._IExpr _1973___mcc_h1492 = _source65.dtor_obj;
                      Dafny.ISequence<Dafny.Rune> _1974___mcc_h1493 = _source65.dtor_name;
                      if (object.Equals(_1974___mcc_h1493, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))) {
                        Dafny.ISequence<RAST._IExpr> _1975_args = _1923___mcc_h1391;
                        RAST._IExpr _1976_base = _1973___mcc_h1492;
                        if (((new BigInteger((_1975_args).Count)) == (BigInteger.One)) && ((object.Equals(_1976_base, RAST.__default.dafny__runtime)) || (object.Equals(_1976_base, RAST.__default.@global)))) {
                          RAST._IExpr _source66 = (_1975_args).Select(BigInteger.Zero);
                          if (_source66.is_RawExpr) {
                            Dafny.ISequence<Dafny.Rune> _1977___mcc_h1516 = _source66.dtor_content;
                            return this;
                          } else if (_source66.is_ExprFromType) {
                            RAST._IType _1978___mcc_h1518 = _source66.dtor_tpe;
                            return this;
                          } else if (_source66.is_Identifier) {
                            Dafny.ISequence<Dafny.Rune> _1979___mcc_h1520 = _source66.dtor_name;
                            return this;
                          } else if (_source66.is_Match) {
                            RAST._IExpr _1980___mcc_h1522 = _source66.dtor_matchee;
                            Dafny.ISequence<RAST._IMatchCase> _1981___mcc_h1523 = _source66.dtor_cases;
                            return this;
                          } else if (_source66.is_StmtExpr) {
                            RAST._IExpr _1982___mcc_h1526 = _source66.dtor_stmt;
                            RAST._IExpr _1983___mcc_h1527 = _source66.dtor_rhs;
                            return this;
                          } else if (_source66.is_Block) {
                            RAST._IExpr _1984___mcc_h1530 = _source66.dtor_underlying;
                            return this;
                          } else if (_source66.is_StructBuild) {
                            RAST._IExpr _1985___mcc_h1532 = _source66.dtor_underlying;
                            Dafny.ISequence<RAST._IAssignIdentifier> _1986___mcc_h1533 = _source66.dtor_assignments;
                            return this;
                          } else if (_source66.is_Tuple) {
                            Dafny.ISequence<RAST._IExpr> _1987___mcc_h1536 = _source66.dtor_arguments;
                            return this;
                          } else if (_source66.is_UnaryOp) {
                            Dafny.ISequence<Dafny.Rune> _1988___mcc_h1538 = _source66.dtor_op1;
                            RAST._IExpr _1989___mcc_h1539 = _source66.dtor_underlying;
                            DAST.Format._IUnaryOpFormat _1990___mcc_h1540 = _source66.dtor_format;
                            return this;
                          } else if (_source66.is_BinaryOp) {
                            Dafny.ISequence<Dafny.Rune> _1991___mcc_h1544 = _source66.dtor_op2;
                            RAST._IExpr _1992___mcc_h1545 = _source66.dtor_left;
                            RAST._IExpr _1993___mcc_h1546 = _source66.dtor_right;
                            DAST.Format._IBinaryOpFormat _1994___mcc_h1547 = _source66.dtor_format2;
                            return this;
                          } else if (_source66.is_TypeAscription) {
                            RAST._IExpr _1995___mcc_h1552 = _source66.dtor_left;
                            RAST._IType _1996___mcc_h1553 = _source66.dtor_tpe;
                            return this;
                          } else if (_source66.is_LiteralInt) {
                            Dafny.ISequence<Dafny.Rune> _1997___mcc_h1556 = _source66.dtor_value;
                            Dafny.ISequence<Dafny.Rune> _1998_number = _1997___mcc_h1556;
                            return RAST.Expr.create_LiteralInt(_1998_number);
                          } else if (_source66.is_LiteralBool) {
                            bool _1999___mcc_h1558 = _source66.dtor_bvalue;
                            return this;
                          } else if (_source66.is_LiteralString) {
                            Dafny.ISequence<Dafny.Rune> _2000___mcc_h1560 = _source66.dtor_value;
                            bool _2001___mcc_h1561 = _source66.dtor_binary;
                            Dafny.ISequence<Dafny.Rune> _2002_number = _2000___mcc_h1560;
                            return RAST.Expr.create_LiteralInt(_2002_number);
                          } else if (_source66.is_DeclareVar) {
                            RAST._IDeclareType _2003___mcc_h1564 = _source66.dtor_declareType;
                            Dafny.ISequence<Dafny.Rune> _2004___mcc_h1565 = _source66.dtor_name;
                            Std.Wrappers._IOption<RAST._IType> _2005___mcc_h1566 = _source66.dtor_optType;
                            Std.Wrappers._IOption<RAST._IExpr> _2006___mcc_h1567 = _source66.dtor_optRhs;
                            return this;
                          } else if (_source66.is_Assign) {
                            Std.Wrappers._IOption<RAST._IAssignLhs> _2007___mcc_h1572 = _source66.dtor_names;
                            RAST._IExpr _2008___mcc_h1573 = _source66.dtor_rhs;
                            return this;
                          } else if (_source66.is_IfExpr) {
                            RAST._IExpr _2009___mcc_h1576 = _source66.dtor_cond;
                            RAST._IExpr _2010___mcc_h1577 = _source66.dtor_thn;
                            RAST._IExpr _2011___mcc_h1578 = _source66.dtor_els;
                            return this;
                          } else if (_source66.is_Loop) {
                            Std.Wrappers._IOption<RAST._IExpr> _2012___mcc_h1582 = _source66.dtor_optCond;
                            RAST._IExpr _2013___mcc_h1583 = _source66.dtor_underlying;
                            return this;
                          } else if (_source66.is_For) {
                            Dafny.ISequence<Dafny.Rune> _2014___mcc_h1586 = _source66.dtor_name;
                            RAST._IExpr _2015___mcc_h1587 = _source66.dtor_range;
                            RAST._IExpr _2016___mcc_h1588 = _source66.dtor_body;
                            return this;
                          } else if (_source66.is_Labelled) {
                            Dafny.ISequence<Dafny.Rune> _2017___mcc_h1592 = _source66.dtor_lbl;
                            RAST._IExpr _2018___mcc_h1593 = _source66.dtor_underlying;
                            return this;
                          } else if (_source66.is_Break) {
                            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _2019___mcc_h1596 = _source66.dtor_optLbl;
                            return this;
                          } else if (_source66.is_Continue) {
                            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _2020___mcc_h1598 = _source66.dtor_optLbl;
                            return this;
                          } else if (_source66.is_Return) {
                            Std.Wrappers._IOption<RAST._IExpr> _2021___mcc_h1600 = _source66.dtor_optExpr;
                            return this;
                          } else if (_source66.is_CallType) {
                            RAST._IExpr _2022___mcc_h1602 = _source66.dtor_obj;
                            Dafny.ISequence<RAST._IType> _2023___mcc_h1603 = _source66.dtor_typeParameters;
                            return this;
                          } else if (_source66.is_Call) {
                            RAST._IExpr _2024___mcc_h1606 = _source66.dtor_obj;
                            Dafny.ISequence<RAST._IExpr> _2025___mcc_h1607 = _source66.dtor_arguments;
                            return this;
                          } else if (_source66.is_Select) {
                            RAST._IExpr _2026___mcc_h1610 = _source66.dtor_obj;
                            Dafny.ISequence<Dafny.Rune> _2027___mcc_h1611 = _source66.dtor_name;
                            return this;
                          } else if (_source66.is_MemberSelect) {
                            RAST._IExpr _2028___mcc_h1614 = _source66.dtor_obj;
                            Dafny.ISequence<Dafny.Rune> _2029___mcc_h1615 = _source66.dtor_name;
                            return this;
                          } else {
                            Dafny.ISequence<RAST._IFormal> _2030___mcc_h1618 = _source66.dtor_params;
                            Std.Wrappers._IOption<RAST._IType> _2031___mcc_h1619 = _source66.dtor_retType;
                            RAST._IExpr _2032___mcc_h1620 = _source66.dtor_body;
                            return this;
                          }
                        } else {
                          return this;
                        }
                      } else {
                        return this;
                      }
                    } else {
                      Dafny.ISequence<RAST._IFormal> _2033___mcc_h1496 = _source65.dtor_params;
                      Std.Wrappers._IOption<RAST._IType> _2034___mcc_h1497 = _source65.dtor_retType;
                      RAST._IExpr _2035___mcc_h1498 = _source65.dtor_body;
                      return this;
                    }
                  } else if (_source64.is_Select) {
                    RAST._IExpr _2036___mcc_h1502 = _source64.dtor_obj;
                    Dafny.ISequence<Dafny.Rune> _2037___mcc_h1503 = _source64.dtor_name;
                    return this;
                  } else if (_source64.is_MemberSelect) {
                    RAST._IExpr _2038___mcc_h1506 = _source64.dtor_obj;
                    Dafny.ISequence<Dafny.Rune> _2039___mcc_h1507 = _source64.dtor_name;
                    return this;
                  } else {
                    Dafny.ISequence<RAST._IFormal> _2040___mcc_h1510 = _source64.dtor_params;
                    Std.Wrappers._IOption<RAST._IType> _2041___mcc_h1511 = _source64.dtor_retType;
                    RAST._IExpr _2042___mcc_h1512 = _source64.dtor_body;
                    return this;
                  }
                } else {
                  return this;
                }
              }
            }
          } else {
            return this;
          }
        } else {
          Dafny.ISequence<RAST._IFormal> _2043___mcc_h1280 = _source63.dtor_params;
          Std.Wrappers._IOption<RAST._IType> _2044___mcc_h1281 = _source63.dtor_retType;
          RAST._IExpr _2045___mcc_h1282 = _source63.dtor_body;
          return this;
        }
      } else if (_source46.is_Select) {
        RAST._IExpr _2046___mcc_h1286 = _source46.dtor_obj;
        Dafny.ISequence<Dafny.Rune> _2047___mcc_h1287 = _source46.dtor_name;
        return this;
      } else if (_source46.is_MemberSelect) {
        RAST._IExpr _2048___mcc_h1290 = _source46.dtor_obj;
        Dafny.ISequence<Dafny.Rune> _2049___mcc_h1291 = _source46.dtor_name;
        return this;
      } else {
        Dafny.ISequence<RAST._IFormal> _2050___mcc_h1294 = _source46.dtor_params;
        Std.Wrappers._IOption<RAST._IType> _2051___mcc_h1295 = _source46.dtor_retType;
        RAST._IExpr _2052___mcc_h1296 = _source46.dtor_body;
        return this;
      }
    }
    public bool LeftRequiresParentheses(RAST._IExpr left) {
      return ((this).printingInfo).NeedParenthesesForLeft((left).printingInfo);
    }
    public _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> LeftParentheses(RAST._IExpr left) {
      if ((this).LeftRequiresParentheses(left)) {
        return _System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
      } else {
        return _System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
      }
    }
    public bool RightRequiresParentheses(RAST._IExpr right) {
      return ((this).printingInfo).NeedParenthesesForRight((right).printingInfo);
    }
    public _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> RightParentheses(RAST._IExpr right) {
      if ((this).RightRequiresParentheses(right)) {
        return _System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
      } else {
        return _System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
      }
    }
    public Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> RightMostIdentifier() {
      RAST._IExpr _source67 = this;
      if (_source67.is_RawExpr) {
        Dafny.ISequence<Dafny.Rune> _2053___mcc_h0 = _source67.dtor_content;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_ExprFromType) {
        RAST._IType _2054___mcc_h2 = _source67.dtor_tpe;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Identifier) {
        Dafny.ISequence<Dafny.Rune> _2055___mcc_h4 = _source67.dtor_name;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Match) {
        RAST._IExpr _2056___mcc_h6 = _source67.dtor_matchee;
        Dafny.ISequence<RAST._IMatchCase> _2057___mcc_h7 = _source67.dtor_cases;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_StmtExpr) {
        RAST._IExpr _2058___mcc_h10 = _source67.dtor_stmt;
        RAST._IExpr _2059___mcc_h11 = _source67.dtor_rhs;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Block) {
        RAST._IExpr _2060___mcc_h14 = _source67.dtor_underlying;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_StructBuild) {
        RAST._IExpr _2061___mcc_h16 = _source67.dtor_underlying;
        Dafny.ISequence<RAST._IAssignIdentifier> _2062___mcc_h17 = _source67.dtor_assignments;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Tuple) {
        Dafny.ISequence<RAST._IExpr> _2063___mcc_h20 = _source67.dtor_arguments;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_UnaryOp) {
        Dafny.ISequence<Dafny.Rune> _2064___mcc_h22 = _source67.dtor_op1;
        RAST._IExpr _2065___mcc_h23 = _source67.dtor_underlying;
        DAST.Format._IUnaryOpFormat _2066___mcc_h24 = _source67.dtor_format;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_BinaryOp) {
        Dafny.ISequence<Dafny.Rune> _2067___mcc_h28 = _source67.dtor_op2;
        RAST._IExpr _2068___mcc_h29 = _source67.dtor_left;
        RAST._IExpr _2069___mcc_h30 = _source67.dtor_right;
        DAST.Format._IBinaryOpFormat _2070___mcc_h31 = _source67.dtor_format2;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_TypeAscription) {
        RAST._IExpr _2071___mcc_h36 = _source67.dtor_left;
        RAST._IType _2072___mcc_h37 = _source67.dtor_tpe;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_LiteralInt) {
        Dafny.ISequence<Dafny.Rune> _2073___mcc_h40 = _source67.dtor_value;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_LiteralBool) {
        bool _2074___mcc_h42 = _source67.dtor_bvalue;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_LiteralString) {
        Dafny.ISequence<Dafny.Rune> _2075___mcc_h44 = _source67.dtor_value;
        bool _2076___mcc_h45 = _source67.dtor_binary;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_DeclareVar) {
        RAST._IDeclareType _2077___mcc_h48 = _source67.dtor_declareType;
        Dafny.ISequence<Dafny.Rune> _2078___mcc_h49 = _source67.dtor_name;
        Std.Wrappers._IOption<RAST._IType> _2079___mcc_h50 = _source67.dtor_optType;
        Std.Wrappers._IOption<RAST._IExpr> _2080___mcc_h51 = _source67.dtor_optRhs;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Assign) {
        Std.Wrappers._IOption<RAST._IAssignLhs> _2081___mcc_h56 = _source67.dtor_names;
        RAST._IExpr _2082___mcc_h57 = _source67.dtor_rhs;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_IfExpr) {
        RAST._IExpr _2083___mcc_h60 = _source67.dtor_cond;
        RAST._IExpr _2084___mcc_h61 = _source67.dtor_thn;
        RAST._IExpr _2085___mcc_h62 = _source67.dtor_els;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Loop) {
        Std.Wrappers._IOption<RAST._IExpr> _2086___mcc_h66 = _source67.dtor_optCond;
        RAST._IExpr _2087___mcc_h67 = _source67.dtor_underlying;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_For) {
        Dafny.ISequence<Dafny.Rune> _2088___mcc_h70 = _source67.dtor_name;
        RAST._IExpr _2089___mcc_h71 = _source67.dtor_range;
        RAST._IExpr _2090___mcc_h72 = _source67.dtor_body;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Labelled) {
        Dafny.ISequence<Dafny.Rune> _2091___mcc_h76 = _source67.dtor_lbl;
        RAST._IExpr _2092___mcc_h77 = _source67.dtor_underlying;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Break) {
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _2093___mcc_h80 = _source67.dtor_optLbl;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Continue) {
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _2094___mcc_h82 = _source67.dtor_optLbl;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Return) {
        Std.Wrappers._IOption<RAST._IExpr> _2095___mcc_h84 = _source67.dtor_optExpr;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_CallType) {
        RAST._IExpr _2096___mcc_h86 = _source67.dtor_obj;
        Dafny.ISequence<RAST._IType> _2097___mcc_h87 = _source67.dtor_typeParameters;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Call) {
        RAST._IExpr _2098___mcc_h90 = _source67.dtor_obj;
        Dafny.ISequence<RAST._IExpr> _2099___mcc_h91 = _source67.dtor_arguments;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_Select) {
        RAST._IExpr _2100___mcc_h94 = _source67.dtor_obj;
        Dafny.ISequence<Dafny.Rune> _2101___mcc_h95 = _source67.dtor_name;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      } else if (_source67.is_MemberSelect) {
        RAST._IExpr _2102___mcc_h98 = _source67.dtor_obj;
        Dafny.ISequence<Dafny.Rune> _2103___mcc_h99 = _source67.dtor_name;
        Dafny.ISequence<Dafny.Rune> _2104_id = _2103___mcc_h99;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(_2104_id);
      } else {
        Dafny.ISequence<RAST._IFormal> _2105___mcc_h102 = _source67.dtor_params;
        Std.Wrappers._IOption<RAST._IType> _2106___mcc_h103 = _source67.dtor_retType;
        RAST._IExpr _2107___mcc_h104 = _source67.dtor_body;
        return Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      var _pat_let_tv4 = ind;
      var _pat_let_tv5 = ind;
      var _pat_let_tv6 = ind;
      var _pat_let_tv7 = ind;
      RAST._IExpr _source68 = (this).Optimize();
      if (_source68.is_RawExpr) {
        Dafny.ISequence<Dafny.Rune> _2108___mcc_h0 = _source68.dtor_content;
        RAST._IExpr _2109_r = (this).Optimize();
        return RAST.__default.AddIndent((_2109_r).dtor_content, ind);
      } else if (_source68.is_ExprFromType) {
        RAST._IType _2110___mcc_h2 = _source68.dtor_tpe;
        RAST._IType _2111_t = _2110___mcc_h2;
        return (_2111_t)._ToString(ind);
      } else if (_source68.is_Identifier) {
        Dafny.ISequence<Dafny.Rune> _2112___mcc_h4 = _source68.dtor_name;
        Dafny.ISequence<Dafny.Rune> _2113_name = _2112___mcc_h4;
        return _2113_name;
      } else if (_source68.is_Match) {
        RAST._IExpr _2114___mcc_h6 = _source68.dtor_matchee;
        Dafny.ISequence<RAST._IMatchCase> _2115___mcc_h7 = _source68.dtor_cases;
        Dafny.ISequence<RAST._IMatchCase> _2116_cases = _2115___mcc_h7;
        RAST._IExpr _2117_matchee = _2114___mcc_h6;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("match "), (_2117_matchee)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {")), RAST.__default.SeqToString<RAST._IMatchCase>(_2116_cases, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IMatchCase, Dafny.ISequence<Dafny.Rune>>>>((_2118_ind) => ((System.Func<RAST._IMatchCase, Dafny.ISequence<Dafny.Rune>>)((_2119_c) => {
          return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), _2118_ind), RAST.__default.IND), (_2119_c)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_2118_ind, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(","));
        })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
      } else if (_source68.is_StmtExpr) {
        RAST._IExpr _2120___mcc_h10 = _source68.dtor_stmt;
        RAST._IExpr _2121___mcc_h11 = _source68.dtor_rhs;
        RAST._IExpr _2122_rhs = _2121___mcc_h11;
        RAST._IExpr _2123_stmt = _2120___mcc_h10;
        if (((_2123_stmt).is_RawExpr) && (((_2123_stmt).dtor_content).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))) {
          return (_2122_rhs)._ToString(ind);
        } else {
          return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat((_2123_stmt)._ToString(ind), (((_2123_stmt).NoExtraSemicolonAfter()) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), ind), (_2122_rhs)._ToString(ind));
        }
      } else if (_source68.is_Block) {
        RAST._IExpr _2124___mcc_h14 = _source68.dtor_underlying;
        RAST._IExpr _2125_underlying = _2124___mcc_h14;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{\n"), ind), RAST.__default.IND), (_2125_underlying)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
      } else if (_source68.is_StructBuild) {
        RAST._IExpr _2126___mcc_h16 = _source68.dtor_underlying;
        Dafny.ISequence<RAST._IAssignIdentifier> _2127___mcc_h17 = _source68.dtor_assignments;
        Dafny.ISequence<RAST._IAssignIdentifier> _2128_assignments = _2127___mcc_h17;
        RAST._IExpr _2129_name = _2126___mcc_h16;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat((_2129_name)._ToString(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {")), RAST.__default.SeqToString<RAST._IAssignIdentifier>(_2128_assignments, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IAssignIdentifier, Dafny.ISequence<Dafny.Rune>>>>((_2130_ind) => ((System.Func<RAST._IAssignIdentifier, Dafny.ISequence<Dafny.Rune>>)((_2131_assignment) => {
          return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), _2130_ind), RAST.__default.IND), (_2131_assignment)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_2130_ind, RAST.__default.IND)));
        })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(","))), (((new BigInteger((_2128_assignments).Count)).Sign == 1) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind)) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
      } else if (_source68.is_Tuple) {
        Dafny.ISequence<RAST._IExpr> _2132___mcc_h20 = _source68.dtor_arguments;
        Dafny.ISequence<RAST._IExpr> _2133_arguments = _2132___mcc_h20;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), RAST.__default.SeqToString<RAST._IExpr>(_2133_arguments, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IExpr, Dafny.ISequence<Dafny.Rune>>>>((_2134_ind) => ((System.Func<RAST._IExpr, Dafny.ISequence<Dafny.Rune>>)((_2135_arg) => {
          return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), _2134_ind), RAST.__default.IND), (_2135_arg)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_2134_ind, RAST.__default.IND)));
        })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(","))), (((new BigInteger((_2133_arguments).Count)).Sign == 1) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind)) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
      } else if (_source68.is_UnaryOp) {
        Dafny.ISequence<Dafny.Rune> _2136___mcc_h22 = _source68.dtor_op1;
        RAST._IExpr _2137___mcc_h23 = _source68.dtor_underlying;
        DAST.Format._IUnaryOpFormat _2138___mcc_h24 = _source68.dtor_format;
        DAST.Format._IUnaryOpFormat _2139_format = _2138___mcc_h24;
        RAST._IExpr _2140_underlying = _2137___mcc_h23;
        Dafny.ISequence<Dafny.Rune> _2141_op = _2136___mcc_h22;
        _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs41 = ((((this).printingInfo).NeedParenthesesFor((_2140_underlying).printingInfo)) ? (_System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"))) : (_System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))));
        Dafny.ISequence<Dafny.Rune> _2142_leftP = _let_tmp_rhs41.dtor__0;
        Dafny.ISequence<Dafny.Rune> _2143_rightP = _let_tmp_rhs41.dtor__1;
        Dafny.ISequence<Dafny.Rune> _2144_leftOp = ((((_2141_op).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&mut"))) && (!(_2142_leftP).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")))) ? (Dafny.Sequence<Dafny.Rune>.Concat(_2141_op, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" "))) : ((((_2141_op).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("?"))) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")) : (_2141_op))));
        Dafny.ISequence<Dafny.Rune> _2145_rightOp = (((_2141_op).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("?"))) ? (_2141_op) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")));
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2144_leftOp, _2142_leftP), (_2140_underlying)._ToString(ind)), _2143_rightP), _2145_rightOp);
      } else if (_source68.is_BinaryOp) {
        Dafny.ISequence<Dafny.Rune> _2146___mcc_h28 = _source68.dtor_op2;
        RAST._IExpr _2147___mcc_h29 = _source68.dtor_left;
        RAST._IExpr _2148___mcc_h30 = _source68.dtor_right;
        DAST.Format._IBinaryOpFormat _2149___mcc_h31 = _source68.dtor_format2;
        DAST.Format._IBinaryOpFormat _2150_format = _2149___mcc_h31;
        RAST._IExpr _2151_right = _2148___mcc_h30;
        RAST._IExpr _2152_left = _2147___mcc_h29;
        Dafny.ISequence<Dafny.Rune> _2153_op2 = _2146___mcc_h28;
        _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs42 = (this).LeftParentheses(_2152_left);
        Dafny.ISequence<Dafny.Rune> _2154_leftLeftP = _let_tmp_rhs42.dtor__0;
        Dafny.ISequence<Dafny.Rune> _2155_leftRighP = _let_tmp_rhs42.dtor__1;
        _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs43 = (this).RightParentheses(_2151_right);
        Dafny.ISequence<Dafny.Rune> _2156_rightLeftP = _let_tmp_rhs43.dtor__0;
        Dafny.ISequence<Dafny.Rune> _2157_rightRightP = _let_tmp_rhs43.dtor__1;
        Dafny.ISequence<Dafny.Rune> _2158_opRendered = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" "), _2153_op2), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" "));
        Dafny.ISequence<Dafny.Rune> _2159_indLeft = (((_2154_leftLeftP).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("))) ? (Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND)) : (ind));
        Dafny.ISequence<Dafny.Rune> _2160_indRight = (((_2156_rightLeftP).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("))) ? (Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND)) : (ind));
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2154_leftLeftP, (_2152_left)._ToString(_2159_indLeft)), _2155_leftRighP), _2158_opRendered), _2156_rightLeftP), (_2151_right)._ToString(_2160_indRight)), _2157_rightRightP);
      } else if (_source68.is_TypeAscription) {
        RAST._IExpr _2161___mcc_h36 = _source68.dtor_left;
        RAST._IType _2162___mcc_h37 = _source68.dtor_tpe;
        RAST._IType _2163_tpe = _2162___mcc_h37;
        RAST._IExpr _2164_left = _2161___mcc_h36;
        _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs44 = (this).LeftParentheses(_2164_left);
        Dafny.ISequence<Dafny.Rune> _2165_leftLeftP = _let_tmp_rhs44.dtor__0;
        Dafny.ISequence<Dafny.Rune> _2166_leftRightP = _let_tmp_rhs44.dtor__1;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2165_leftLeftP, (_2164_left)._ToString(RAST.__default.IND)), _2166_leftRightP), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" as ")), (_2163_tpe)._ToString(RAST.__default.IND));
      } else if (_source68.is_LiteralInt) {
        Dafny.ISequence<Dafny.Rune> _2167___mcc_h40 = _source68.dtor_value;
        Dafny.ISequence<Dafny.Rune> _2168_number = _2167___mcc_h40;
        return _2168_number;
      } else if (_source68.is_LiteralBool) {
        bool _2169___mcc_h42 = _source68.dtor_bvalue;
        bool _2170_b = _2169___mcc_h42;
        if (_2170_b) {
          return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("true");
        } else {
          return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("false");
        }
      } else if (_source68.is_LiteralString) {
        Dafny.ISequence<Dafny.Rune> _2171___mcc_h44 = _source68.dtor_value;
        bool _2172___mcc_h45 = _source68.dtor_binary;
        bool _2173_binary = _2172___mcc_h45;
        Dafny.ISequence<Dafny.Rune> _2174_characters = _2171___mcc_h44;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(((_2173_binary) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("b")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\"")), _2174_characters), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\""));
      } else if (_source68.is_DeclareVar) {
        RAST._IDeclareType _2175___mcc_h48 = _source68.dtor_declareType;
        Dafny.ISequence<Dafny.Rune> _2176___mcc_h49 = _source68.dtor_name;
        Std.Wrappers._IOption<RAST._IType> _2177___mcc_h50 = _source68.dtor_optType;
        Std.Wrappers._IOption<RAST._IExpr> _2178___mcc_h51 = _source68.dtor_optRhs;
        Std.Wrappers._IOption<RAST._IExpr> _2179_optExpr = _2178___mcc_h51;
        Std.Wrappers._IOption<RAST._IType> _2180_optType = _2177___mcc_h50;
        Dafny.ISequence<Dafny.Rune> _2181_name = _2176___mcc_h49;
        RAST._IDeclareType _2182_declareType = _2175___mcc_h48;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("let "), ((object.Equals(_2182_declareType, RAST.DeclareType.create_MUT())) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("mut ")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), _2181_name), (((_2180_optType).is_Some) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(": "), ((_2180_optType).dtor_value)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND)))) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), (((_2179_optExpr).is_Some) ? (Dafny.Helpers.Let<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>(((_2179_optExpr).dtor_value)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND)), _pat_let8_0 => Dafny.Helpers.Let<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>(_pat_let8_0, _2183_optExprString => (((_2183_optExprString).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("= /*issue with empty RHS*/"), ((((_2179_optExpr).dtor_value).is_RawExpr) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Empty Raw expr")) : (((((_2179_optExpr).dtor_value).is_LiteralString) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Empty string literal")) : (((((_2179_optExpr).dtor_value).is_LiteralInt) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Empty int literal")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Another case"))))))))) : (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" = "), _2183_optExprString)))))) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";"));
      } else if (_source68.is_Assign) {
        Std.Wrappers._IOption<RAST._IAssignLhs> _2184___mcc_h56 = _source68.dtor_names;
        RAST._IExpr _2185___mcc_h57 = _source68.dtor_rhs;
        RAST._IExpr _2186_expr = _2185___mcc_h57;
        Std.Wrappers._IOption<RAST._IAssignLhs> _2187_names = _2184___mcc_h56;
        Dafny.ISequence<Dafny.Rune> _2188_lhs = ((System.Func<Std.Wrappers._IOption<RAST._IAssignLhs>, Dafny.ISequence<Dafny.Rune>>)((_source69) => {
          if (_source69.is_None) {
            return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_ = ");
          } else {
            RAST._IAssignLhs _2189___mcc_h108 = _source69.dtor_value;
            RAST._IAssignLhs _source70 = _2189___mcc_h108;
            if (_source70.is_LocalVar) {
              Dafny.ISequence<Dafny.Rune> _2190___mcc_h109 = _source70.dtor_name;
              Dafny.ISequence<Dafny.Rune> _2191_name = _2190___mcc_h109;
              return Dafny.Sequence<Dafny.Rune>.Concat(_2191_name, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" = "));
            } else if (_source70.is_SelectMember) {
              RAST._IExpr _2192___mcc_h110 = _source70.dtor_on;
              Dafny.ISequence<Dafny.Rune> _2193___mcc_h111 = _source70.dtor_field;
              Dafny.ISequence<Dafny.Rune> _2194_field = _2193___mcc_h111;
              RAST._IExpr _2195_member = _2192___mcc_h110;
              _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs45 = (RAST.Expr.create_Select(_2195_member, _2194_field)).LeftParentheses(_2195_member);
              Dafny.ISequence<Dafny.Rune> _2196_leftP = _let_tmp_rhs45.dtor__0;
              Dafny.ISequence<Dafny.Rune> _2197_rightP = _let_tmp_rhs45.dtor__1;
              return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2196_leftP, (_2195_member)._ToString(_pat_let_tv4)), _2197_rightP), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".")), _2194_field), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" = "));
            } else if (_source70.is_ExtractTuple) {
              Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2198___mcc_h112 = _source70.dtor_names;
              Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2199_names = _2198___mcc_h112;
              return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), RAST.__default.SeqToString<Dafny.ISequence<Dafny.Rune>>(_2199_names, ((System.Func<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>)((_2200_name) => {
                return _2200_name;
              })), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(","))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(") = "));
            } else {
              RAST._IExpr _2201___mcc_h113 = _source70.dtor_expr;
              Dafny.ISequence<RAST._IExpr> _2202___mcc_h114 = _source70.dtor_indices;
              Dafny.ISequence<RAST._IExpr> _2203_indices = _2202___mcc_h114;
              RAST._IExpr _2204_e = _2201___mcc_h113;
              _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs46 = (RAST.Expr.create_Call(_2204_e, _2203_indices)).LeftParentheses(_2204_e);
              Dafny.ISequence<Dafny.Rune> _2205_leftP = _let_tmp_rhs46.dtor__0;
              Dafny.ISequence<Dafny.Rune> _2206_rightP = _let_tmp_rhs46.dtor__1;
              return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2205_leftP, (_2204_e)._ToString(_pat_let_tv5)), _2206_rightP), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("[")), RAST.__default.SeqToString<RAST._IExpr>(_2203_indices, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IExpr, Dafny.ISequence<Dafny.Rune>>>>((_2207_ind) => ((System.Func<RAST._IExpr, Dafny.ISequence<Dafny.Rune>>)((_2208_index) => {
                return (_2208_index)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_2207_ind, RAST.__default.IND));
              })))(_pat_let_tv6), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("]["))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("] = "));
            }
          }
        }))(_2187_names);
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2188_lhs, (_2186_expr)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";"));
      } else if (_source68.is_IfExpr) {
        RAST._IExpr _2209___mcc_h60 = _source68.dtor_cond;
        RAST._IExpr _2210___mcc_h61 = _source68.dtor_thn;
        RAST._IExpr _2211___mcc_h62 = _source68.dtor_els;
        RAST._IExpr _2212_els = _2211___mcc_h62;
        RAST._IExpr _2213_thn = _2210___mcc_h61;
        RAST._IExpr _2214_cond = _2209___mcc_h60;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("if "), (_2214_cond)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {\n")), ind), RAST.__default.IND), (_2213_thn)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}")), ((object.Equals(_2212_els, RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")) : (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" else {\n"), ind), RAST.__default.IND), (_2212_els)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}")))));
      } else if (_source68.is_Loop) {
        Std.Wrappers._IOption<RAST._IExpr> _2215___mcc_h66 = _source68.dtor_optCond;
        RAST._IExpr _2216___mcc_h67 = _source68.dtor_underlying;
        RAST._IExpr _2217_underlying = _2216___mcc_h67;
        Std.Wrappers._IOption<RAST._IExpr> _2218_optCond = _2215___mcc_h66;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(((System.Func<Std.Wrappers._IOption<RAST._IExpr>, Dafny.ISequence<Dafny.Rune>>)((_source71) => {
          if (_source71.is_None) {
            return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("loop");
          } else {
            RAST._IExpr _2219___mcc_h115 = _source71.dtor_value;
            RAST._IExpr _2220_c = _2219___mcc_h115;
            return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("while "), (_2220_c)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_pat_let_tv7, RAST.__default.IND)));
          }
        }))(_2218_optCond), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {\n")), ind), RAST.__default.IND), (_2217_underlying)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
      } else if (_source68.is_For) {
        Dafny.ISequence<Dafny.Rune> _2221___mcc_h70 = _source68.dtor_name;
        RAST._IExpr _2222___mcc_h71 = _source68.dtor_range;
        RAST._IExpr _2223___mcc_h72 = _source68.dtor_body;
        RAST._IExpr _2224_body = _2223___mcc_h72;
        RAST._IExpr _2225_range = _2222___mcc_h71;
        Dafny.ISequence<Dafny.Rune> _2226_name = _2221___mcc_h70;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("for "), _2226_name), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" in ")), (_2225_range)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {\n")), ind), RAST.__default.IND), (_2224_body)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
      } else if (_source68.is_Labelled) {
        Dafny.ISequence<Dafny.Rune> _2227___mcc_h76 = _source68.dtor_lbl;
        RAST._IExpr _2228___mcc_h77 = _source68.dtor_underlying;
        RAST._IExpr _2229_underlying = _2228___mcc_h77;
        Dafny.ISequence<Dafny.Rune> _2230_name = _2227___mcc_h76;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("'"), _2230_name), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(": ")), (_2229_underlying)._ToString(ind));
      } else if (_source68.is_Break) {
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _2231___mcc_h80 = _source68.dtor_optLbl;
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _2232_optLbl = _2231___mcc_h80;
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _source72 = _2232_optLbl;
        if (_source72.is_None) {
          return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("break;");
        } else {
          Dafny.ISequence<Dafny.Rune> _2233___mcc_h116 = _source72.dtor_value;
          Dafny.ISequence<Dafny.Rune> _2234_lbl = _2233___mcc_h116;
          return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("break '"), _2234_lbl), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";"));
        }
      } else if (_source68.is_Continue) {
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _2235___mcc_h82 = _source68.dtor_optLbl;
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _2236_optLbl = _2235___mcc_h82;
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _source73 = _2236_optLbl;
        if (_source73.is_None) {
          return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("continue;");
        } else {
          Dafny.ISequence<Dafny.Rune> _2237___mcc_h117 = _source73.dtor_value;
          Dafny.ISequence<Dafny.Rune> _2238_lbl = _2237___mcc_h117;
          return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("continue '"), _2238_lbl), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";"));
        }
      } else if (_source68.is_Return) {
        Std.Wrappers._IOption<RAST._IExpr> _2239___mcc_h84 = _source68.dtor_optExpr;
        Std.Wrappers._IOption<RAST._IExpr> _2240_optExpr = _2239___mcc_h84;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("return"), (((_2240_optExpr).is_Some) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" "), ((_2240_optExpr).dtor_value)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(ind, RAST.__default.IND)))) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";"));
      } else if (_source68.is_CallType) {
        RAST._IExpr _2241___mcc_h86 = _source68.dtor_obj;
        Dafny.ISequence<RAST._IType> _2242___mcc_h87 = _source68.dtor_typeParameters;
        Dafny.ISequence<RAST._IType> _2243_tpes = _2242___mcc_h87;
        RAST._IExpr _2244_expr = _2241___mcc_h86;
        _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs47 = (this).LeftParentheses(_2244_expr);
        Dafny.ISequence<Dafny.Rune> _2245_leftP = _let_tmp_rhs47.dtor__0;
        Dafny.ISequence<Dafny.Rune> _2246_rightP = _let_tmp_rhs47.dtor__1;
        if ((_2243_tpes).Equals(Dafny.Sequence<RAST._IType>.FromElements())) {
          return (_2244_expr)._ToString(ind);
        } else {
          return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2245_leftP, (_2244_expr)._ToString(ind)), _2246_rightP), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::<")), RAST.__default.SeqToString<RAST._IType>(_2243_tpes, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IType, Dafny.ISequence<Dafny.Rune>>>>((_2247_ind) => ((System.Func<RAST._IType, Dafny.ISequence<Dafny.Rune>>)((_2248_tpe) => {
            return (_2248_tpe)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_2247_ind, RAST.__default.IND));
          })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">"));
        }
      } else if (_source68.is_Call) {
        RAST._IExpr _2249___mcc_h90 = _source68.dtor_obj;
        Dafny.ISequence<RAST._IExpr> _2250___mcc_h91 = _source68.dtor_arguments;
        Dafny.ISequence<RAST._IExpr> _2251_args = _2250___mcc_h91;
        RAST._IExpr _2252_expr = _2249___mcc_h90;
        _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs48 = (this).LeftParentheses(_2252_expr);
        Dafny.ISequence<Dafny.Rune> _2253_leftP = _let_tmp_rhs48.dtor__0;
        Dafny.ISequence<Dafny.Rune> _2254_rightP = _let_tmp_rhs48.dtor__1;
        _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs49 = ((System.Func<Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>>, _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>>)((_source74) => {
          if (_source74.is_None) {
            return _System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
          } else {
            Dafny.ISequence<Dafny.Rune> _2255___mcc_h118 = _source74.dtor_value;
            if (object.Equals(_2255___mcc_h118, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("seq!"))) {
              return _System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("["), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("]"));
            } else if (object.Equals(_2255___mcc_h118, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("map!"))) {
              return _System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("["), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("]"));
            } else if (object.Equals(_2255___mcc_h118, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("set!"))) {
              return _System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
            } else if (object.Equals(_2255___mcc_h118, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("multiset!"))) {
              return _System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
            } else {
              return _System.Tuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
            }
          }
        }))((_2252_expr).RightMostIdentifier());
        Dafny.ISequence<Dafny.Rune> _2256_leftCallP = _let_tmp_rhs49.dtor__0;
        Dafny.ISequence<Dafny.Rune> _2257_rightCallP = _let_tmp_rhs49.dtor__1;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2253_leftP, (_2252_expr)._ToString(ind)), _2254_rightP), _2256_leftCallP), RAST.__default.SeqToString<RAST._IExpr>(_2251_args, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IExpr, Dafny.ISequence<Dafny.Rune>>>>((_2258_ind) => ((System.Func<RAST._IExpr, Dafny.ISequence<Dafny.Rune>>)((_2259_arg) => {
          return (_2259_arg)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_2258_ind, RAST.__default.IND));
        })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "))), _2257_rightCallP);
      } else if (_source68.is_Select) {
        RAST._IExpr _2260___mcc_h94 = _source68.dtor_obj;
        Dafny.ISequence<Dafny.Rune> _2261___mcc_h95 = _source68.dtor_name;
        Dafny.ISequence<Dafny.Rune> _2262_name = _2261___mcc_h95;
        RAST._IExpr _2263_expression = _2260___mcc_h94;
        _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs50 = (this).LeftParentheses(_2263_expression);
        Dafny.ISequence<Dafny.Rune> _2264_leftP = _let_tmp_rhs50.dtor__0;
        Dafny.ISequence<Dafny.Rune> _2265_rightP = _let_tmp_rhs50.dtor__1;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2264_leftP, (_2263_expression)._ToString(ind)), _2265_rightP), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".")), _2262_name);
      } else if (_source68.is_MemberSelect) {
        RAST._IExpr _2266___mcc_h98 = _source68.dtor_obj;
        Dafny.ISequence<Dafny.Rune> _2267___mcc_h99 = _source68.dtor_name;
        Dafny.ISequence<Dafny.Rune> _2268_name = _2267___mcc_h99;
        RAST._IExpr _2269_expression = _2266___mcc_h98;
        _System._ITuple2<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs51 = (this).LeftParentheses(_2269_expression);
        Dafny.ISequence<Dafny.Rune> _2270_leftP = _let_tmp_rhs51.dtor__0;
        Dafny.ISequence<Dafny.Rune> _2271_rightP = _let_tmp_rhs51.dtor__1;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_2270_leftP, (_2269_expression)._ToString(ind)), _2271_rightP), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), _2268_name);
      } else {
        Dafny.ISequence<RAST._IFormal> _2272___mcc_h102 = _source68.dtor_params;
        Std.Wrappers._IOption<RAST._IType> _2273___mcc_h103 = _source68.dtor_retType;
        RAST._IExpr _2274___mcc_h104 = _source68.dtor_body;
        RAST._IExpr _2275_body = _2274___mcc_h104;
        Std.Wrappers._IOption<RAST._IType> _2276_retType = _2273___mcc_h103;
        Dafny.ISequence<RAST._IFormal> _2277_params = _2272___mcc_h102;
        return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("move |"), RAST.__default.SeqToString<RAST._IFormal>(_2277_params, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IFormal, Dafny.ISequence<Dafny.Rune>>>>((_2278_ind) => ((System.Func<RAST._IFormal, Dafny.ISequence<Dafny.Rune>>)((_2279_arg) => {
          return (_2279_arg)._ToString(_2278_ind);
        })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(","))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("| ")), (((_2276_retType).is_Some) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("-> "), ((_2276_retType).dtor_value)._ToString(ind)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" "))) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), (_2275_body)._ToString(ind));
      }
    }
    public RAST._IExpr Then(RAST._IExpr rhs2) {
      if ((this).is_StmtExpr) {
        return RAST.Expr.create_StmtExpr((this).dtor_stmt, ((this).dtor_rhs).Then(rhs2));
      } else if (object.Equals(this, RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))) {
        return rhs2;
      } else {
        return RAST.Expr.create_StmtExpr(this, rhs2);
      }
    }
    public RAST._IExpr Sel(Dafny.ISequence<Dafny.Rune> name) {
      return RAST.Expr.create_Select(this, name);
    }
    public RAST._IExpr MSel(Dafny.ISequence<Dafny.Rune> name) {
      return RAST.Expr.create_MemberSelect(this, name);
    }
    public RAST._IExpr ApplyType(Dafny.ISequence<RAST._IType> typeParameters) {
      return RAST.Expr.create_CallType(this, typeParameters);
    }
    public RAST._IExpr ApplyType1(RAST._IType typeParameter) {
      return RAST.Expr.create_CallType(this, Dafny.Sequence<RAST._IType>.FromElements(typeParameter));
    }
    public RAST._IExpr Apply(Dafny.ISequence<RAST._IExpr> arguments) {
      return RAST.Expr.create_Call(this, arguments);
    }
    public RAST._IExpr Apply1(RAST._IExpr argument) {
      return RAST.Expr.create_Call(this, Dafny.Sequence<RAST._IExpr>.FromElements(argument));
    }
    public bool IsLhsIdentifier() {
      return ((this).is_Identifier) || (((this).is_Call) && ((((object.Equals((this).dtor_obj, (RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("modify!")))) && ((new BigInteger(((this).dtor_arguments).Count)) == (BigInteger.One))) && ((((this).dtor_arguments).Select(BigInteger.Zero)).is_Identifier)) || (((object.Equals((this).dtor_obj, (RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("md!")))) && ((new BigInteger(((this).dtor_arguments).Count)) == (BigInteger.One))) && (Dafny.Helpers.Let<RAST._IExpr, bool>(((this).dtor_arguments).Select(BigInteger.Zero), _pat_let9_0 => Dafny.Helpers.Let<RAST._IExpr, bool>(_pat_let9_0, _2280_lhs => (((_2280_lhs).is_Call) && (((_2280_lhs).dtor_obj).is_Select)) && ((((_2280_lhs).dtor_obj).dtor_obj).is_Identifier)))))));
    }
    public Dafny.ISequence<Dafny.Rune> LhsIdentifierName() {
      if ((this).is_Identifier) {
        return (this).dtor_name;
      } else if (object.Equals((this).dtor_obj, (RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("modify!")))) {
        return (((this).dtor_arguments).Select(BigInteger.Zero)).dtor_name;
      } else {
        return (((((this).dtor_arguments).Select(BigInteger.Zero)).dtor_obj).dtor_obj).dtor_name;
      }
    }
    public RAST._IPrintingInfo printingInfo { get {
      RAST._IExpr _source75 = this;
      if (_source75.is_RawExpr) {
        Dafny.ISequence<Dafny.Rune> _2281___mcc_h0 = _source75.dtor_content;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_ExprFromType) {
        RAST._IType _2282___mcc_h2 = _source75.dtor_tpe;
        return RAST.PrintingInfo.create_Precedence(BigInteger.One);
      } else if (_source75.is_Identifier) {
        Dafny.ISequence<Dafny.Rune> _2283___mcc_h4 = _source75.dtor_name;
        return RAST.PrintingInfo.create_Precedence(BigInteger.One);
      } else if (_source75.is_Match) {
        RAST._IExpr _2284___mcc_h6 = _source75.dtor_matchee;
        Dafny.ISequence<RAST._IMatchCase> _2285___mcc_h7 = _source75.dtor_cases;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_StmtExpr) {
        RAST._IExpr _2286___mcc_h10 = _source75.dtor_stmt;
        RAST._IExpr _2287___mcc_h11 = _source75.dtor_rhs;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_Block) {
        RAST._IExpr _2288___mcc_h14 = _source75.dtor_underlying;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_StructBuild) {
        RAST._IExpr _2289___mcc_h16 = _source75.dtor_underlying;
        Dafny.ISequence<RAST._IAssignIdentifier> _2290___mcc_h17 = _source75.dtor_assignments;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_Tuple) {
        Dafny.ISequence<RAST._IExpr> _2291___mcc_h20 = _source75.dtor_arguments;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_UnaryOp) {
        Dafny.ISequence<Dafny.Rune> _2292___mcc_h22 = _source75.dtor_op1;
        RAST._IExpr _2293___mcc_h23 = _source75.dtor_underlying;
        DAST.Format._IUnaryOpFormat _2294___mcc_h24 = _source75.dtor_format;
        DAST.Format._IUnaryOpFormat _2295_format = _2294___mcc_h24;
        RAST._IExpr _2296_underlying = _2293___mcc_h23;
        Dafny.ISequence<Dafny.Rune> _2297_op = _2292___mcc_h22;
        if (object.Equals(_2297_op, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("?"))) {
          return RAST.PrintingInfo.create_SuffixPrecedence(new BigInteger(5));
        } else if (object.Equals(_2297_op, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("-"))) {
          return RAST.PrintingInfo.create_Precedence(new BigInteger(6));
        } else if (object.Equals(_2297_op, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"))) {
          return RAST.PrintingInfo.create_Precedence(new BigInteger(6));
        } else if (object.Equals(_2297_op, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"))) {
          return RAST.PrintingInfo.create_Precedence(new BigInteger(6));
        } else if (object.Equals(_2297_op, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"))) {
          return RAST.PrintingInfo.create_Precedence(new BigInteger(6));
        } else if (object.Equals(_2297_op, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&mut"))) {
          return RAST.PrintingInfo.create_Precedence(new BigInteger(6));
        } else {
          return RAST.PrintingInfo.create_UnknownPrecedence();
        }
      } else if (_source75.is_BinaryOp) {
        Dafny.ISequence<Dafny.Rune> _2298___mcc_h28 = _source75.dtor_op2;
        RAST._IExpr _2299___mcc_h29 = _source75.dtor_left;
        RAST._IExpr _2300___mcc_h30 = _source75.dtor_right;
        DAST.Format._IBinaryOpFormat _2301___mcc_h31 = _source75.dtor_format2;
        DAST.Format._IBinaryOpFormat _2302_format = _2301___mcc_h31;
        RAST._IExpr _2303_right = _2300___mcc_h30;
        RAST._IExpr _2304_left = _2299___mcc_h29;
        Dafny.ISequence<Dafny.Rune> _2305_op2 = _2298___mcc_h28;
        if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(20), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(20), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("%"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(20), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("+"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(30), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("-"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(30), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<<"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(40), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">>"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(40), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(50), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("^"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(60), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("|"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(70), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&&"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(90), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("||"))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(100), RAST.Associativity.create_LeftToRight());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".."))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RequiresParentheses());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("..="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RequiresParentheses());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("+="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("-="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("%="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("|="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("^="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<<="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else if (object.Equals(_2305_op2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">>="))) {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft());
        } else {
          return RAST.PrintingInfo.create_PrecedenceAssociativity(BigInteger.Zero, RAST.Associativity.create_RequiresParentheses());
        }
      } else if (_source75.is_TypeAscription) {
        RAST._IExpr _2306___mcc_h36 = _source75.dtor_left;
        RAST._IType _2307___mcc_h37 = _source75.dtor_tpe;
        RAST._IType _2308_tpe = _2307___mcc_h37;
        RAST._IExpr _2309_left = _2306___mcc_h36;
        return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(10), RAST.Associativity.create_LeftToRight());
      } else if (_source75.is_LiteralInt) {
        Dafny.ISequence<Dafny.Rune> _2310___mcc_h40 = _source75.dtor_value;
        return RAST.PrintingInfo.create_Precedence(BigInteger.One);
      } else if (_source75.is_LiteralBool) {
        bool _2311___mcc_h42 = _source75.dtor_bvalue;
        return RAST.PrintingInfo.create_Precedence(BigInteger.One);
      } else if (_source75.is_LiteralString) {
        Dafny.ISequence<Dafny.Rune> _2312___mcc_h44 = _source75.dtor_value;
        bool _2313___mcc_h45 = _source75.dtor_binary;
        return RAST.PrintingInfo.create_Precedence(BigInteger.One);
      } else if (_source75.is_DeclareVar) {
        RAST._IDeclareType _2314___mcc_h48 = _source75.dtor_declareType;
        Dafny.ISequence<Dafny.Rune> _2315___mcc_h49 = _source75.dtor_name;
        Std.Wrappers._IOption<RAST._IType> _2316___mcc_h50 = _source75.dtor_optType;
        Std.Wrappers._IOption<RAST._IExpr> _2317___mcc_h51 = _source75.dtor_optRhs;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_Assign) {
        Std.Wrappers._IOption<RAST._IAssignLhs> _2318___mcc_h56 = _source75.dtor_names;
        RAST._IExpr _2319___mcc_h57 = _source75.dtor_rhs;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_IfExpr) {
        RAST._IExpr _2320___mcc_h60 = _source75.dtor_cond;
        RAST._IExpr _2321___mcc_h61 = _source75.dtor_thn;
        RAST._IExpr _2322___mcc_h62 = _source75.dtor_els;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_Loop) {
        Std.Wrappers._IOption<RAST._IExpr> _2323___mcc_h66 = _source75.dtor_optCond;
        RAST._IExpr _2324___mcc_h67 = _source75.dtor_underlying;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_For) {
        Dafny.ISequence<Dafny.Rune> _2325___mcc_h70 = _source75.dtor_name;
        RAST._IExpr _2326___mcc_h71 = _source75.dtor_range;
        RAST._IExpr _2327___mcc_h72 = _source75.dtor_body;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_Labelled) {
        Dafny.ISequence<Dafny.Rune> _2328___mcc_h76 = _source75.dtor_lbl;
        RAST._IExpr _2329___mcc_h77 = _source75.dtor_underlying;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_Break) {
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _2330___mcc_h80 = _source75.dtor_optLbl;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_Continue) {
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _2331___mcc_h82 = _source75.dtor_optLbl;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_Return) {
        Std.Wrappers._IOption<RAST._IExpr> _2332___mcc_h84 = _source75.dtor_optExpr;
        return RAST.PrintingInfo.create_UnknownPrecedence();
      } else if (_source75.is_CallType) {
        RAST._IExpr _2333___mcc_h86 = _source75.dtor_obj;
        Dafny.ISequence<RAST._IType> _2334___mcc_h87 = _source75.dtor_typeParameters;
        return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(2), RAST.Associativity.create_LeftToRight());
      } else if (_source75.is_Call) {
        RAST._IExpr _2335___mcc_h90 = _source75.dtor_obj;
        Dafny.ISequence<RAST._IExpr> _2336___mcc_h91 = _source75.dtor_arguments;
        return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(2), RAST.Associativity.create_LeftToRight());
      } else if (_source75.is_Select) {
        RAST._IExpr _2337___mcc_h94 = _source75.dtor_obj;
        Dafny.ISequence<Dafny.Rune> _2338___mcc_h95 = _source75.dtor_name;
        Dafny.ISequence<Dafny.Rune> _2339_name = _2338___mcc_h95;
        RAST._IExpr _2340_underlying = _2337___mcc_h94;
        return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(2), RAST.Associativity.create_LeftToRight());
      } else if (_source75.is_MemberSelect) {
        RAST._IExpr _2341___mcc_h98 = _source75.dtor_obj;
        Dafny.ISequence<Dafny.Rune> _2342___mcc_h99 = _source75.dtor_name;
        Dafny.ISequence<Dafny.Rune> _2343_name = _2342___mcc_h99;
        RAST._IExpr _2344_underlying = _2341___mcc_h98;
        return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(2), RAST.Associativity.create_LeftToRight());
      } else {
        Dafny.ISequence<RAST._IFormal> _2345___mcc_h102 = _source75.dtor_params;
        Std.Wrappers._IOption<RAST._IType> _2346___mcc_h103 = _source75.dtor_retType;
        RAST._IExpr _2347___mcc_h104 = _source75.dtor_body;
        return RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(300), RAST.Associativity.create_LeftToRight());
      }
    } }
  }
  public class Expr_RawExpr : Expr {
    public readonly Dafny.ISequence<Dafny.Rune> _i_content;
    public Expr_RawExpr(Dafny.ISequence<Dafny.Rune> content) : base() {
      this._i_content = content;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_RawExpr(_i_content);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_RawExpr;
      return oth != null && object.Equals(this._i_content, oth._i_content);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_content));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.RawExpr";
      s += "(";
      s += this._i_content.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class Expr_ExprFromType : Expr {
    public readonly RAST._IType _i_tpe;
    public Expr_ExprFromType(RAST._IType tpe) : base() {
      this._i_tpe = tpe;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_ExprFromType(_i_tpe);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_ExprFromType;
      return oth != null && object.Equals(this._i_tpe, oth._i_tpe);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 1;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tpe));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.ExprFromType";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_tpe);
      s += ")";
      return s;
    }
  }
  public class Expr_Identifier : Expr {
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public Expr_Identifier(Dafny.ISequence<Dafny.Rune> name) : base() {
      this._i_name = name;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Identifier(_i_name);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Identifier;
      return oth != null && object.Equals(this._i_name, oth._i_name);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 2;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Identifier";
      s += "(";
      s += this._i_name.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class Expr_Match : Expr {
    public readonly RAST._IExpr _i_matchee;
    public readonly Dafny.ISequence<RAST._IMatchCase> _i_cases;
    public Expr_Match(RAST._IExpr matchee, Dafny.ISequence<RAST._IMatchCase> cases) : base() {
      this._i_matchee = matchee;
      this._i_cases = cases;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Match(_i_matchee, _i_cases);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Match;
      return oth != null && object.Equals(this._i_matchee, oth._i_matchee) && object.Equals(this._i_cases, oth._i_cases);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 3;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_matchee));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_cases));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Match";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_matchee);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_cases);
      s += ")";
      return s;
    }
  }
  public class Expr_StmtExpr : Expr {
    public readonly RAST._IExpr _i_stmt;
    public readonly RAST._IExpr _i_rhs;
    public Expr_StmtExpr(RAST._IExpr stmt, RAST._IExpr rhs) : base() {
      this._i_stmt = stmt;
      this._i_rhs = rhs;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_StmtExpr(_i_stmt, _i_rhs);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_StmtExpr;
      return oth != null && object.Equals(this._i_stmt, oth._i_stmt) && object.Equals(this._i_rhs, oth._i_rhs);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 4;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_stmt));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_rhs));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.StmtExpr";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_stmt);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_rhs);
      s += ")";
      return s;
    }
  }
  public class Expr_Block : Expr {
    public readonly RAST._IExpr _i_underlying;
    public Expr_Block(RAST._IExpr underlying) : base() {
      this._i_underlying = underlying;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Block(_i_underlying);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Block;
      return oth != null && object.Equals(this._i_underlying, oth._i_underlying);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 5;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Block";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ")";
      return s;
    }
  }
  public class Expr_StructBuild : Expr {
    public readonly RAST._IExpr _i_underlying;
    public readonly Dafny.ISequence<RAST._IAssignIdentifier> _i_assignments;
    public Expr_StructBuild(RAST._IExpr underlying, Dafny.ISequence<RAST._IAssignIdentifier> assignments) : base() {
      this._i_underlying = underlying;
      this._i_assignments = assignments;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_StructBuild(_i_underlying, _i_assignments);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_StructBuild;
      return oth != null && object.Equals(this._i_underlying, oth._i_underlying) && object.Equals(this._i_assignments, oth._i_assignments);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 6;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_assignments));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.StructBuild";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_assignments);
      s += ")";
      return s;
    }
  }
  public class Expr_Tuple : Expr {
    public readonly Dafny.ISequence<RAST._IExpr> _i_arguments;
    public Expr_Tuple(Dafny.ISequence<RAST._IExpr> arguments) : base() {
      this._i_arguments = arguments;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Tuple(_i_arguments);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Tuple;
      return oth != null && object.Equals(this._i_arguments, oth._i_arguments);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 7;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_arguments));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Tuple";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_arguments);
      s += ")";
      return s;
    }
  }
  public class Expr_UnaryOp : Expr {
    public readonly Dafny.ISequence<Dafny.Rune> _i_op1;
    public readonly RAST._IExpr _i_underlying;
    public readonly DAST.Format._IUnaryOpFormat _i_format;
    public Expr_UnaryOp(Dafny.ISequence<Dafny.Rune> op1, RAST._IExpr underlying, DAST.Format._IUnaryOpFormat format) : base() {
      this._i_op1 = op1;
      this._i_underlying = underlying;
      this._i_format = format;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_UnaryOp(_i_op1, _i_underlying, _i_format);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_UnaryOp;
      return oth != null && object.Equals(this._i_op1, oth._i_op1) && object.Equals(this._i_underlying, oth._i_underlying) && object.Equals(this._i_format, oth._i_format);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 8;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_op1));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_format));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.UnaryOp";
      s += "(";
      s += this._i_op1.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_format);
      s += ")";
      return s;
    }
  }
  public class Expr_BinaryOp : Expr {
    public readonly Dafny.ISequence<Dafny.Rune> _i_op2;
    public readonly RAST._IExpr _i_left;
    public readonly RAST._IExpr _i_right;
    public readonly DAST.Format._IBinaryOpFormat _i_format2;
    public Expr_BinaryOp(Dafny.ISequence<Dafny.Rune> op2, RAST._IExpr left, RAST._IExpr right, DAST.Format._IBinaryOpFormat format2) : base() {
      this._i_op2 = op2;
      this._i_left = left;
      this._i_right = right;
      this._i_format2 = format2;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_BinaryOp(_i_op2, _i_left, _i_right, _i_format2);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_BinaryOp;
      return oth != null && object.Equals(this._i_op2, oth._i_op2) && object.Equals(this._i_left, oth._i_left) && object.Equals(this._i_right, oth._i_right) && object.Equals(this._i_format2, oth._i_format2);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 9;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_op2));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_left));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_right));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_format2));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.BinaryOp";
      s += "(";
      s += this._i_op2.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_left);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_right);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_format2);
      s += ")";
      return s;
    }
  }
  public class Expr_TypeAscription : Expr {
    public readonly RAST._IExpr _i_left;
    public readonly RAST._IType _i_tpe;
    public Expr_TypeAscription(RAST._IExpr left, RAST._IType tpe) : base() {
      this._i_left = left;
      this._i_tpe = tpe;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_TypeAscription(_i_left, _i_tpe);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_TypeAscription;
      return oth != null && object.Equals(this._i_left, oth._i_left) && object.Equals(this._i_tpe, oth._i_tpe);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 10;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_left));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_tpe));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.TypeAscription";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_left);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_tpe);
      s += ")";
      return s;
    }
  }
  public class Expr_LiteralInt : Expr {
    public readonly Dafny.ISequence<Dafny.Rune> _i_value;
    public Expr_LiteralInt(Dafny.ISequence<Dafny.Rune> @value) : base() {
      this._i_value = @value;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_LiteralInt(_i_value);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_LiteralInt;
      return oth != null && object.Equals(this._i_value, oth._i_value);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 11;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_value));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.LiteralInt";
      s += "(";
      s += this._i_value.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class Expr_LiteralBool : Expr {
    public readonly bool _i_bvalue;
    public Expr_LiteralBool(bool bvalue) : base() {
      this._i_bvalue = bvalue;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_LiteralBool(_i_bvalue);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_LiteralBool;
      return oth != null && this._i_bvalue == oth._i_bvalue;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 12;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_bvalue));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.LiteralBool";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_bvalue);
      s += ")";
      return s;
    }
  }
  public class Expr_LiteralString : Expr {
    public readonly Dafny.ISequence<Dafny.Rune> _i_value;
    public readonly bool _i_binary;
    public Expr_LiteralString(Dafny.ISequence<Dafny.Rune> @value, bool binary) : base() {
      this._i_value = @value;
      this._i_binary = binary;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_LiteralString(_i_value, _i_binary);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_LiteralString;
      return oth != null && object.Equals(this._i_value, oth._i_value) && this._i_binary == oth._i_binary;
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 13;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_value));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_binary));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.LiteralString";
      s += "(";
      s += this._i_value.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_binary);
      s += ")";
      return s;
    }
  }
  public class Expr_DeclareVar : Expr {
    public readonly RAST._IDeclareType _i_declareType;
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public readonly Std.Wrappers._IOption<RAST._IType> _i_optType;
    public readonly Std.Wrappers._IOption<RAST._IExpr> _i_optRhs;
    public Expr_DeclareVar(RAST._IDeclareType declareType, Dafny.ISequence<Dafny.Rune> name, Std.Wrappers._IOption<RAST._IType> optType, Std.Wrappers._IOption<RAST._IExpr> optRhs) : base() {
      this._i_declareType = declareType;
      this._i_name = name;
      this._i_optType = optType;
      this._i_optRhs = optRhs;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_DeclareVar(_i_declareType, _i_name, _i_optType, _i_optRhs);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_DeclareVar;
      return oth != null && object.Equals(this._i_declareType, oth._i_declareType) && object.Equals(this._i_name, oth._i_name) && object.Equals(this._i_optType, oth._i_optType) && object.Equals(this._i_optRhs, oth._i_optRhs);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 14;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_declareType));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_optType));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_optRhs));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.DeclareVar";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_declareType);
      s += ", ";
      s += this._i_name.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_optType);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_optRhs);
      s += ")";
      return s;
    }
  }
  public class Expr_Assign : Expr {
    public readonly Std.Wrappers._IOption<RAST._IAssignLhs> _i_names;
    public readonly RAST._IExpr _i_rhs;
    public Expr_Assign(Std.Wrappers._IOption<RAST._IAssignLhs> names, RAST._IExpr rhs) : base() {
      this._i_names = names;
      this._i_rhs = rhs;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Assign(_i_names, _i_rhs);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Assign;
      return oth != null && object.Equals(this._i_names, oth._i_names) && object.Equals(this._i_rhs, oth._i_rhs);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 15;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_names));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_rhs));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Assign";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_names);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_rhs);
      s += ")";
      return s;
    }
  }
  public class Expr_IfExpr : Expr {
    public readonly RAST._IExpr _i_cond;
    public readonly RAST._IExpr _i_thn;
    public readonly RAST._IExpr _i_els;
    public Expr_IfExpr(RAST._IExpr cond, RAST._IExpr thn, RAST._IExpr els) : base() {
      this._i_cond = cond;
      this._i_thn = thn;
      this._i_els = els;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_IfExpr(_i_cond, _i_thn, _i_els);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_IfExpr;
      return oth != null && object.Equals(this._i_cond, oth._i_cond) && object.Equals(this._i_thn, oth._i_thn) && object.Equals(this._i_els, oth._i_els);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 16;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_cond));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_thn));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_els));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.IfExpr";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_cond);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_thn);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_els);
      s += ")";
      return s;
    }
  }
  public class Expr_Loop : Expr {
    public readonly Std.Wrappers._IOption<RAST._IExpr> _i_optCond;
    public readonly RAST._IExpr _i_underlying;
    public Expr_Loop(Std.Wrappers._IOption<RAST._IExpr> optCond, RAST._IExpr underlying) : base() {
      this._i_optCond = optCond;
      this._i_underlying = underlying;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Loop(_i_optCond, _i_underlying);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Loop;
      return oth != null && object.Equals(this._i_optCond, oth._i_optCond) && object.Equals(this._i_underlying, oth._i_underlying);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 17;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_optCond));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Loop";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_optCond);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ")";
      return s;
    }
  }
  public class Expr_For : Expr {
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public readonly RAST._IExpr _i_range;
    public readonly RAST._IExpr _i_body;
    public Expr_For(Dafny.ISequence<Dafny.Rune> name, RAST._IExpr range, RAST._IExpr body) : base() {
      this._i_name = name;
      this._i_range = range;
      this._i_body = body;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_For(_i_name, _i_range, _i_body);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_For;
      return oth != null && object.Equals(this._i_name, oth._i_name) && object.Equals(this._i_range, oth._i_range) && object.Equals(this._i_body, oth._i_body);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 18;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_range));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_body));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.For";
      s += "(";
      s += this._i_name.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_range);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_body);
      s += ")";
      return s;
    }
  }
  public class Expr_Labelled : Expr {
    public readonly Dafny.ISequence<Dafny.Rune> _i_lbl;
    public readonly RAST._IExpr _i_underlying;
    public Expr_Labelled(Dafny.ISequence<Dafny.Rune> lbl, RAST._IExpr underlying) : base() {
      this._i_lbl = lbl;
      this._i_underlying = underlying;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Labelled(_i_lbl, _i_underlying);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Labelled;
      return oth != null && object.Equals(this._i_lbl, oth._i_lbl) && object.Equals(this._i_underlying, oth._i_underlying);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 19;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_lbl));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_underlying));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Labelled";
      s += "(";
      s += this._i_lbl.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_underlying);
      s += ")";
      return s;
    }
  }
  public class Expr_Break : Expr {
    public readonly Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _i_optLbl;
    public Expr_Break(Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> optLbl) : base() {
      this._i_optLbl = optLbl;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Break(_i_optLbl);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Break;
      return oth != null && object.Equals(this._i_optLbl, oth._i_optLbl);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 20;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_optLbl));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Break";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_optLbl);
      s += ")";
      return s;
    }
  }
  public class Expr_Continue : Expr {
    public readonly Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _i_optLbl;
    public Expr_Continue(Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> optLbl) : base() {
      this._i_optLbl = optLbl;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Continue(_i_optLbl);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Continue;
      return oth != null && object.Equals(this._i_optLbl, oth._i_optLbl);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 21;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_optLbl));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Continue";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_optLbl);
      s += ")";
      return s;
    }
  }
  public class Expr_Return : Expr {
    public readonly Std.Wrappers._IOption<RAST._IExpr> _i_optExpr;
    public Expr_Return(Std.Wrappers._IOption<RAST._IExpr> optExpr) : base() {
      this._i_optExpr = optExpr;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Return(_i_optExpr);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Return;
      return oth != null && object.Equals(this._i_optExpr, oth._i_optExpr);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 22;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_optExpr));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Return";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_optExpr);
      s += ")";
      return s;
    }
  }
  public class Expr_CallType : Expr {
    public readonly RAST._IExpr _i_obj;
    public readonly Dafny.ISequence<RAST._IType> _i_typeParameters;
    public Expr_CallType(RAST._IExpr obj, Dafny.ISequence<RAST._IType> typeParameters) : base() {
      this._i_obj = obj;
      this._i_typeParameters = typeParameters;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_CallType(_i_obj, _i_typeParameters);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_CallType;
      return oth != null && object.Equals(this._i_obj, oth._i_obj) && object.Equals(this._i_typeParameters, oth._i_typeParameters);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 23;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_obj));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_typeParameters));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.CallType";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_obj);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_typeParameters);
      s += ")";
      return s;
    }
  }
  public class Expr_Call : Expr {
    public readonly RAST._IExpr _i_obj;
    public readonly Dafny.ISequence<RAST._IExpr> _i_arguments;
    public Expr_Call(RAST._IExpr obj, Dafny.ISequence<RAST._IExpr> arguments) : base() {
      this._i_obj = obj;
      this._i_arguments = arguments;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Call(_i_obj, _i_arguments);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Call;
      return oth != null && object.Equals(this._i_obj, oth._i_obj) && object.Equals(this._i_arguments, oth._i_arguments);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 24;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_obj));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_arguments));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Call";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_obj);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_arguments);
      s += ")";
      return s;
    }
  }
  public class Expr_Select : Expr {
    public readonly RAST._IExpr _i_obj;
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public Expr_Select(RAST._IExpr obj, Dafny.ISequence<Dafny.Rune> name) : base() {
      this._i_obj = obj;
      this._i_name = name;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Select(_i_obj, _i_name);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Select;
      return oth != null && object.Equals(this._i_obj, oth._i_obj) && object.Equals(this._i_name, oth._i_name);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 25;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_obj));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Select";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_obj);
      s += ", ";
      s += this._i_name.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class Expr_MemberSelect : Expr {
    public readonly RAST._IExpr _i_obj;
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public Expr_MemberSelect(RAST._IExpr obj, Dafny.ISequence<Dafny.Rune> name) : base() {
      this._i_obj = obj;
      this._i_name = name;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_MemberSelect(_i_obj, _i_name);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_MemberSelect;
      return oth != null && object.Equals(this._i_obj, oth._i_obj) && object.Equals(this._i_name, oth._i_name);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 26;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_obj));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.MemberSelect";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_obj);
      s += ", ";
      s += this._i_name.ToVerbatimString(true);
      s += ")";
      return s;
    }
  }
  public class Expr_Lambda : Expr {
    public readonly Dafny.ISequence<RAST._IFormal> _i_params;
    public readonly Std.Wrappers._IOption<RAST._IType> _i_retType;
    public readonly RAST._IExpr _i_body;
    public Expr_Lambda(Dafny.ISequence<RAST._IFormal> @params, Std.Wrappers._IOption<RAST._IType> retType, RAST._IExpr body) : base() {
      this._i_params = @params;
      this._i_retType = retType;
      this._i_body = body;
    }
    public override _IExpr DowncastClone() {
      if (this is _IExpr dt) { return dt; }
      return new Expr_Lambda(_i_params, _i_retType, _i_body);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Expr_Lambda;
      return oth != null && object.Equals(this._i_params, oth._i_params) && object.Equals(this._i_retType, oth._i_retType) && object.Equals(this._i_body, oth._i_body);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 27;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_params));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_retType));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_body));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Expr.Lambda";
      s += "(";
      s += Dafny.Helpers.ToString(this._i_params);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_retType);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_body);
      s += ")";
      return s;
    }
  }

  public interface _IFn {
    bool is_Fn { get; }
    Dafny.ISequence<Dafny.Rune> dtor_name { get; }
    Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams { get; }
    Dafny.ISequence<RAST._IFormal> dtor_formals { get; }
    Std.Wrappers._IOption<RAST._IType> dtor_returnType { get; }
    Dafny.ISequence<Dafny.Rune> dtor_where { get; }
    Std.Wrappers._IOption<RAST._IExpr> dtor_body { get; }
    _IFn DowncastClone();
    Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind);
  }
  public class Fn : _IFn {
    public readonly Dafny.ISequence<Dafny.Rune> _i_name;
    public readonly Dafny.ISequence<RAST._ITypeParamDecl> _i_typeParams;
    public readonly Dafny.ISequence<RAST._IFormal> _i_formals;
    public readonly Std.Wrappers._IOption<RAST._IType> _i_returnType;
    public readonly Dafny.ISequence<Dafny.Rune> _i_where;
    public readonly Std.Wrappers._IOption<RAST._IExpr> _i_body;
    public Fn(Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, Dafny.ISequence<RAST._IFormal> formals, Std.Wrappers._IOption<RAST._IType> returnType, Dafny.ISequence<Dafny.Rune> @where, Std.Wrappers._IOption<RAST._IExpr> body) {
      this._i_name = name;
      this._i_typeParams = typeParams;
      this._i_formals = formals;
      this._i_returnType = returnType;
      this._i_where = @where;
      this._i_body = body;
    }
    public _IFn DowncastClone() {
      if (this is _IFn dt) { return dt; }
      return new Fn(_i_name, _i_typeParams, _i_formals, _i_returnType, _i_where, _i_body);
    }
    public override bool Equals(object other) {
      var oth = other as RAST.Fn;
      return oth != null && object.Equals(this._i_name, oth._i_name) && object.Equals(this._i_typeParams, oth._i_typeParams) && object.Equals(this._i_formals, oth._i_formals) && object.Equals(this._i_returnType, oth._i_returnType) && object.Equals(this._i_where, oth._i_where) && object.Equals(this._i_body, oth._i_body);
    }
    public override int GetHashCode() {
      ulong hash = 5381;
      hash = ((hash << 5) + hash) + 0;
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_name));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_typeParams));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_formals));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_returnType));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_where));
      hash = ((hash << 5) + hash) + ((ulong)Dafny.Helpers.GetHashCode(this._i_body));
      return (int) hash;
    }
    public override string ToString() {
      string s = "RAST.Fn.Fn";
      s += "(";
      s += this._i_name.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_typeParams);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_formals);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_returnType);
      s += ", ";
      s += this._i_where.ToVerbatimString(true);
      s += ", ";
      s += Dafny.Helpers.ToString(this._i_body);
      s += ")";
      return s;
    }
    private static readonly RAST._IFn theDefault = create(Dafny.Sequence<Dafny.Rune>.Empty, Dafny.Sequence<RAST._ITypeParamDecl>.Empty, Dafny.Sequence<RAST._IFormal>.Empty, Std.Wrappers.Option<RAST._IType>.Default(), Dafny.Sequence<Dafny.Rune>.Empty, Std.Wrappers.Option<RAST._IExpr>.Default());
    public static RAST._IFn Default() {
      return theDefault;
    }
    private static readonly Dafny.TypeDescriptor<RAST._IFn> _TYPE = new Dafny.TypeDescriptor<RAST._IFn>(RAST.Fn.Default());
    public static Dafny.TypeDescriptor<RAST._IFn> _TypeDescriptor() {
      return _TYPE;
    }
    public static _IFn create(Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, Dafny.ISequence<RAST._IFormal> formals, Std.Wrappers._IOption<RAST._IType> returnType, Dafny.ISequence<Dafny.Rune> @where, Std.Wrappers._IOption<RAST._IExpr> body) {
      return new Fn(name, typeParams, formals, returnType, @where, body);
    }
    public static _IFn create_Fn(Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<RAST._ITypeParamDecl> typeParams, Dafny.ISequence<RAST._IFormal> formals, Std.Wrappers._IOption<RAST._IType> returnType, Dafny.ISequence<Dafny.Rune> @where, Std.Wrappers._IOption<RAST._IExpr> body) {
      return create(name, typeParams, formals, returnType, @where, body);
    }
    public bool is_Fn { get { return true; } }
    public Dafny.ISequence<Dafny.Rune> dtor_name {
      get {
        return this._i_name;
      }
    }
    public Dafny.ISequence<RAST._ITypeParamDecl> dtor_typeParams {
      get {
        return this._i_typeParams;
      }
    }
    public Dafny.ISequence<RAST._IFormal> dtor_formals {
      get {
        return this._i_formals;
      }
    }
    public Std.Wrappers._IOption<RAST._IType> dtor_returnType {
      get {
        return this._i_returnType;
      }
    }
    public Dafny.ISequence<Dafny.Rune> dtor_where {
      get {
        return this._i_where;
      }
    }
    public Std.Wrappers._IOption<RAST._IExpr> dtor_body {
      get {
        return this._i_body;
      }
    }
    public Dafny.ISequence<Dafny.Rune> _ToString(Dafny.ISequence<Dafny.Rune> ind) {
      var _pat_let_tv8 = ind;
      var _pat_let_tv9 = ind;
      var _pat_let_tv10 = ind;
      var _pat_let_tv11 = ind;
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fn "), (this).dtor_name), RAST.TypeParamDecl.ToStringMultiple((this).dtor_typeParams, ind)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")), RAST.__default.SeqToString<RAST._IFormal>((this).dtor_formals, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.Rune>, Func<RAST._IFormal, Dafny.ISequence<Dafny.Rune>>>>((_2348_ind) => ((System.Func<RAST._IFormal, Dafny.ISequence<Dafny.Rune>>)((_2349_formal) => {
        return (_2349_formal)._ToString(_2348_ind);
      })))(ind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")")), ((System.Func<Std.Wrappers._IOption<RAST._IType>, Dafny.ISequence<Dafny.Rune>>)((_source76) => {
        if (_source76.is_None) {
          return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
        } else {
          RAST._IType _2350___mcc_h0 = _source76.dtor_value;
          RAST._IType _2351_t = _2350___mcc_h0;
          return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" -> "), (_2351_t)._ToString(_pat_let_tv8));
        }
      }))((this).dtor_returnType)), ((((this).dtor_where).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")) : (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"), ind), RAST.__default.IND), (this).dtor_where)))), ((System.Func<Std.Wrappers._IOption<RAST._IExpr>, Dafny.ISequence<Dafny.Rune>>)((_source77) => {
        if (_source77.is_None) {
          return Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";");
        } else {
          RAST._IExpr _2352___mcc_h2 = _source77.dtor_value;
          RAST._IExpr _2353_body = _2352___mcc_h2;
          return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" {\n"), _pat_let_tv9), RAST.__default.IND), (_2353_body)._ToString(Dafny.Sequence<Dafny.Rune>.Concat(_pat_let_tv10, RAST.__default.IND))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n")), _pat_let_tv11), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
        }
      }))((this).dtor_body));
    }
  }
} // end of namespace RAST