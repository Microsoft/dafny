//-----------------------------------------------------------------------------
//
// Copyright by the contributors to the Dafny Project
// SPDX-License-Identifier: MIT
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using static Microsoft.Dafny.ResolutionErrors;

namespace Microsoft.Dafny {
  class ConstantFolder {
    /// <summary>
    /// Returns the largest value that can be stored in bitvector type "t".
    /// </summary>
    public static BigInteger MaxBv(Type t) {
      Contract.Requires(t != null);
      Contract.Requires(t.IsBitVectorType);
      return MaxBv(t.AsBitVectorType.Width);
    }

    /// <summary>
    /// Returns the largest value that can be stored in bitvector type of "bits" width.
    /// </summary>
    public static BigInteger MaxBv(int bits) {
      Contract.Requires(0 <= bits);
      return BigInteger.Pow(new BigInteger(2), bits) - BigInteger.One;
    }

    /// <summary>
    /// Folds "e" into an integer literal, if possible.
    /// Returns "null" if not possible (or not easy).
    /// </summary>
    public static BigInteger? GetConst(Expression e) {
      var ee = GetAnyConst(e.Resolved ?? e, new Stack<ConstantField>());
      return ee as BigInteger?;
    }

    // Returns null if the argument is a constrained newtype (recursively)
    // Returns the transitive base type if the argument is recursively unconstrained
    static Type AsUnconstrainedType(Type t) {
      while (true) {
        if (t.AsNewtype == null) {
          return t;
        }

        if (t.AsNewtype.Constraint != null) {
          return null;
        }

        t = t.AsNewtype.BaseType;
      }
    }

    static object GetAnyConst(Expression e, Stack<ConstantField> constants) {
      if (e is LiteralExpr l) {
        return l.Value;
      } else if (e is UnaryOpExpr un) {
        if (un.ResolvedOp == UnaryOpExpr.ResolvedOpcode.BoolNot && GetAnyConst(un.E, constants) is bool b) {
          return !b;
        }
        if (un.ResolvedOp == UnaryOpExpr.ResolvedOpcode.BVNot && GetAnyConst(un.E, constants) is BigInteger i) {
          return ((BigInteger.One << un.Type.AsBitVectorType.Width) - 1) ^ i;
        }
        // TODO: This only handles strings; generalize to other collections?
        if (un.ResolvedOp == UnaryOpExpr.ResolvedOpcode.SeqLength && GetAnyConst(un.E, constants) is string ss) {
          return (BigInteger)(ss.Length);
        }
      } else if (e is MemberSelectExpr m) {
        if (m.Member is ConstantField c && c.IsStatic && c.Rhs != null) {
          // This aspect of type resolution happens before the check for cyclic references
          // so we have to do a check here as well. If cyclic, null is silently returned,
          // counting on the later error message to alert the user.
          if (constants.Contains(c)) { return null; }
          constants.Push(c);
          var o = GetAnyConst(c.Rhs, constants);
          constants.Pop();
          return o;
        } else if (m.Member is SpecialField sf) {
          var nm = sf.Name;
          if (nm == "Floor") {
            var ee = GetAnyConst(m.Obj, constants);
            if (ee != null && m.Obj.Type.IsNumericBased(Type.NumericPersuasion.Real)) {
              ((BaseTypes.BigDec)ee).FloorCeiling(out var f, out _);
              return f;
            }
          }
        }
      } else if (e is BinaryExpr bin) {
        var e0 = GetAnyConst(bin.E0, constants);
        var e1 = GetAnyConst(bin.E1, constants);
        var isBool = bin.E0.Type == Type.Bool && bin.E1.Type == Type.Bool;
        var shortCircuit = isBool && (bin.ResolvedOp == BinaryExpr.ResolvedOpcode.And
                                      || bin.ResolvedOp == BinaryExpr.ResolvedOpcode.Or
                                      || bin.ResolvedOp == BinaryExpr.ResolvedOpcode.Imp);

        if (e0 == null || (!shortCircuit && e1 == null)) {
          return null;
        }
        var isAnyReal = bin.E0.Type.IsNumericBased(Type.NumericPersuasion.Real)
                        && bin.E1.Type.IsNumericBased(Type.NumericPersuasion.Real);
        var isAnyInt = bin.E0.Type.IsNumericBased(Type.NumericPersuasion.Int)
                       && bin.E1.Type.IsNumericBased(Type.NumericPersuasion.Int);
        var isReal = bin.Type.IsRealType;
        var isInt = bin.Type.IsIntegerType;
        var isBv = bin.E0.Type.IsBitVectorType;
        var isString = e0 is string && e1 is string;
        switch (bin.ResolvedOp) {
          case BinaryExpr.ResolvedOpcode.Add:
            if (isInt) {
              return (BigInteger)e0 + (BigInteger)e1;
            }
            if (isBv) {
              return ((BigInteger)e0 + (BigInteger)e1) & MaxBv(bin.Type);
            }
            if (isReal) {
              return (BaseTypes.BigDec)e0 + (BaseTypes.BigDec)e1;
            }
            break;
          case BinaryExpr.ResolvedOpcode.Concat:
            if (isString) {
              return (string)e0 + (string)e1;
            }
            break;
          case BinaryExpr.ResolvedOpcode.Sub:
            if (isInt) {
              return (BigInteger)e0 - (BigInteger)e1;
            }
            if (isBv) {
              return ((BigInteger)e0 - (BigInteger)e1) & MaxBv(bin.Type);
            }
            if (isReal) {
              return (BaseTypes.BigDec)e0 - (BaseTypes.BigDec)e1;
            }
            // Allow a special case: If the result type is a newtype that is integer-based (i.e., isInt && !isInteger)
            // then we generally do not fold the operations, because we do not determine whether the
            // result of the operation satisfies the new type constraint. However, on the occasion that
            // a newtype aliases int without a constraint, it occurs that a value of the newtype is initialized
            // with a negative value, which is represented as "0 - N", that is, it comes to this case. It
            // is a nuisance not to constant-fold the result, as not doing so can alter the determination
            // of the representation type.
            if (isAnyInt && AsUnconstrainedType(bin.Type) != null) {
              return ((BigInteger)e0) - ((BigInteger)e1);
            }
            break;
          case BinaryExpr.ResolvedOpcode.Mul:
            if (isInt) {
              return (BigInteger)e0 * (BigInteger)e1;
            }
            if (isBv) {
              return ((BigInteger)e0 * (BigInteger)e1) & MaxBv(bin.Type);
            }
            if (isReal) {
              return (BaseTypes.BigDec)e0 * (BaseTypes.BigDec)e1;
            }
            break;
          case BinaryExpr.ResolvedOpcode.BitwiseAnd:
            Contract.Assert(isBv);
            return (BigInteger)e0 & (BigInteger)e1;
          case BinaryExpr.ResolvedOpcode.BitwiseOr:
            Contract.Assert(isBv);
            return (BigInteger)e0 | (BigInteger)e1;
          case BinaryExpr.ResolvedOpcode.BitwiseXor:
            Contract.Assert(isBv);
            return (BigInteger)e0 ^ (BigInteger)e1;
          case BinaryExpr.ResolvedOpcode.Div:
            if (isInt) {
              if ((BigInteger)e1 == 0) {
                return null; // Divide by zero
              } else {
                var a0 = (BigInteger)e0;
                var a1 = (BigInteger)e1;
                var d = a0 / a1;
                return a0 >= 0 || a0 == d * a1 ? d : a1 > 0 ? d - 1 : d + 1;
              }
            }
            if (isBv) {
              if ((BigInteger)e1 == 0) {
                return null; // Divide by zero
              } else {
                return ((BigInteger)e0) / ((BigInteger)e1);
              }
            }
            if (isReal) {
              if ((BaseTypes.BigDec)e1 == BaseTypes.BigDec.ZERO) {
                return null; // Divide by zero
              } else {
                // BigDec does not have divide and is not a representation of rationals, so we don't do constant folding
                return null;
              }
            }

            break;
          case BinaryExpr.ResolvedOpcode.Mod:
            if (isInt) {
              if ((BigInteger)e1 == 0) {
                return null; // Mod by zero
              } else {
                var a = BigInteger.Abs((BigInteger)e1);
                var d = (BigInteger)e0 % a;
                return (BigInteger)e0 >= 0 ? d : d + a;
              }
            }
            if (isBv) {
              if ((BigInteger)e1 == 0) {
                return null; // Mod by zero
              } else {
                return (BigInteger)e0 % (BigInteger)e1;
              }
            }
            break;
          case BinaryExpr.ResolvedOpcode.LeftShift: {
              if ((BigInteger)e1 < 0) {
                return null; // Negative shift
              }
              if ((BigInteger)e1 > bin.Type.AsBitVectorType.Width) {
                return null; // Shift is too large
              }
              return ((BigInteger)e0 << (int)(BigInteger)e1) & MaxBv(bin.E0.Type);
            }
          case BinaryExpr.ResolvedOpcode.RightShift: {
              if ((BigInteger)e1 < 0) {
                return null; // Negative shift
              }
              if ((BigInteger)e1 > bin.Type.AsBitVectorType.Width) {
                return null; // Shift too large
              }
              return (BigInteger)e0 >> (int)(BigInteger)e1;
            }
          case BinaryExpr.ResolvedOpcode.And: {
              if ((bool)e0 && e1 == null) {
                return null;
              }
              return (bool)e0 && (bool)e1;
            }
          case BinaryExpr.ResolvedOpcode.Or: {
              if (!(bool)e0 && e1 == null) {
                return null;
              }
              return (bool)e0 || (bool)e1;
            }
          case BinaryExpr.ResolvedOpcode.Imp: { // ==> and <==
              if ((bool)e0 && e1 == null) {
                return null;
              }
              return !(bool)e0 || (bool)e1;
            }
          case BinaryExpr.ResolvedOpcode.Iff: return (bool)e0 == (bool)e1; // <==>
          case BinaryExpr.ResolvedOpcode.Gt:
            if (isAnyInt) {
              return (BigInteger)e0 > (BigInteger)e1;
            }
            if (isBv) {
              return (BigInteger)e0 > (BigInteger)e1;
            }
            if (isAnyReal) {
              return (BaseTypes.BigDec)e0 > (BaseTypes.BigDec)e1;
            }
            break;
          case BinaryExpr.ResolvedOpcode.GtChar:
            if (bin.E0.Type.IsCharType) {
              return ((string)e0)[0] > ((string)e1)[0];
            }
            break;
          case BinaryExpr.ResolvedOpcode.Ge:
            if (isAnyInt) {
              return (BigInteger)e0 >= (BigInteger)e1;
            }
            if (isBv) {
              return (BigInteger)e0 >= (BigInteger)e1;
            }
            if (isAnyReal) {
              return (BaseTypes.BigDec)e0 >= (BaseTypes.BigDec)e1;
            }
            break;
          case BinaryExpr.ResolvedOpcode.GeChar:
            if (bin.E0.Type.IsCharType) {
              return ((string)e0)[0] >= ((string)e1)[0];
            }
            break;
          case BinaryExpr.ResolvedOpcode.Lt:
            if (isAnyInt) {
              return (BigInteger)e0 < (BigInteger)e1;
            }
            if (isBv) {
              return (BigInteger)e0 < (BigInteger)e1;
            }
            if (isAnyReal) {
              return (BaseTypes.BigDec)e0 < (BaseTypes.BigDec)e1;
            }
            break;
          case BinaryExpr.ResolvedOpcode.LtChar:
            if (bin.E0.Type.IsCharType) {
              return ((string)e0)[0] < ((string)e1)[0];
            }
            break;
          case BinaryExpr.ResolvedOpcode.ProperPrefix:
            if (isString) {
              return ((string)e1).StartsWith((string)e0) && !((string)e1).Equals((string)e0);
            }
            break;
          case BinaryExpr.ResolvedOpcode.Le:
            if (isAnyInt) {
              return (BigInteger)e0 <= (BigInteger)e1;
            }
            if (isBv) {
              return (BigInteger)e0 <= (BigInteger)e1;
            }
            if (isAnyReal) {
              return (BaseTypes.BigDec)e0 <= (BaseTypes.BigDec)e1;
            }
            break;
          case BinaryExpr.ResolvedOpcode.LeChar:
            if (bin.E0.Type.IsCharType) {
              return ((string)e0)[0] <= ((string)e1)[0];
            }
            break;
          case BinaryExpr.ResolvedOpcode.Prefix:
            if (isString) {
              return ((string)e1).StartsWith((string)e0);
            }
            break;
          case BinaryExpr.ResolvedOpcode.EqCommon: {
              if (isBool) {
                return (bool)e0 == (bool)e1;
              } else if (isAnyInt || isBv) {
                return (BigInteger)e0 == (BigInteger)e1;
              } else if (isAnyReal) {
                return (BaseTypes.BigDec)e0 == (BaseTypes.BigDec)e1;
              } else if (bin.E0.Type.IsCharType) {
                return ((string)e0)[0] == ((string)e1)[0];
              }
              break;
            }
          case BinaryExpr.ResolvedOpcode.SeqEq:
            if (isString) {
              return (string)e0 == (string)e1;
            }
            break;
          case BinaryExpr.ResolvedOpcode.SeqNeq:
            if (isString) {
              return (string)e0 != (string)e1;
            }
            break;
          case BinaryExpr.ResolvedOpcode.NeqCommon: {
              if (isBool) {
                return (bool)e0 != (bool)e1;
              } else if (isAnyInt || isBv) {
                return (BigInteger)e0 != (BigInteger)e1;
              } else if (isAnyReal) {
                return (BaseTypes.BigDec)e0 != (BaseTypes.BigDec)e1;
              } else if (bin.E0.Type.IsCharType) {
                return ((string)e0)[0] != ((string)e1)[0];
              } else if (isString) {
                return (string)e0 != (string)e1;
              }
              break;
            }
        }
      } else if (e is ConversionExpr ce) {
        var o = GetAnyConst(ce.E, constants);
        if (o == null || ce.E.Type == ce.Type) {
          return o;
        }

        if (ce.E.Type.IsNumericBased(Type.NumericPersuasion.Real) && ce.Type.IsBitVectorType) {
          ((BaseTypes.BigDec)o).FloorCeiling(out var ff, out _);
          if (ff < 0 || ff > MaxBv(ce.Type)) {
            return null; // Out of range
          }
          if (((BaseTypes.BigDec)o) != BaseTypes.BigDec.FromBigInt(ff)) {
            return null; // Out of range
          }
          return ff;
        }

        if (ce.E.Type.IsNumericBased(Type.NumericPersuasion.Real) && ce.Type.IsNumericBased(Type.NumericPersuasion.Int)) {
          ((BaseTypes.BigDec)o).FloorCeiling(out var ff, out _);
          if (AsUnconstrainedType(ce.Type) == null) {
            return null;
          }
          if (((BaseTypes.BigDec)o) != BaseTypes.BigDec.FromBigInt(ff)) {
            return null; // Argument not an integer
          }
          return ff;
        }

        if (ce.E.Type.IsBitVectorType && ce.Type.IsNumericBased(Type.NumericPersuasion.Int)) {
          if (AsUnconstrainedType(ce.Type) == null) {
            return null;
          }
          return o;
        }

        if (ce.E.Type.IsBitVectorType && ce.Type.IsNumericBased(Type.NumericPersuasion.Real)) {
          if (AsUnconstrainedType(ce.Type) == null) {
            return null;
          }
          return BaseTypes.BigDec.FromBigInt((BigInteger)o);
        }

        if (ce.E.Type.IsNumericBased(Type.NumericPersuasion.Int) && ce.Type.IsBitVectorType) {
          var b = (BigInteger)o;
          if (b < 0 || b > MaxBv(ce.Type)) {
            return null; // Argument out of range
          }
          return o;
        }

        if (ce.E.Type.IsNumericBased(Type.NumericPersuasion.Int) && ce.Type.IsNumericBased(Type.NumericPersuasion.Int)) {
          // This case includes int-based newtypes to int-based new types
          if (AsUnconstrainedType(ce.Type) == null) {
            return null;
          }
          return o;
        }

        if (ce.E.Type.IsNumericBased(Type.NumericPersuasion.Real) && ce.Type.IsNumericBased(Type.NumericPersuasion.Real)) {
          // This case includes real-based newtypes to real-based new types
          if (AsUnconstrainedType(ce.Type) == null) {
            return null;
          }
          return o;
        }

        if (ce.E.Type.IsBitVectorType && ce.Type.IsBitVectorType) {
          var b = (BigInteger)o;
          if (b < 0 || b > MaxBv(ce.Type)) {
            return null; // Argument out of range
          }
          return o;
        }

        if (ce.E.Type.IsNumericBased(Type.NumericPersuasion.Int) && ce.Type.IsNumericBased(Type.NumericPersuasion.Real)) {
          if (AsUnconstrainedType(ce.Type) == null) {
            return null;
          }
          return BaseTypes.BigDec.FromBigInt((BigInteger)o);
        }

        if (ce.E.Type.IsCharType && ce.Type.IsNumericBased(Type.NumericPersuasion.Int)) {
          var c = ((String)o)[0];
          if (AsUnconstrainedType(ce.Type) == null) {
            return null;
          }
          return new BigInteger(((string)o)[0]);
        }

        if (ce.E.Type.IsCharType && ce.Type.IsBitVectorType) {
          var c = ((String)o)[0];
          if ((int)c > MaxBv(ce.Type)) {
            return null; // Argument out of range
          }
          return new BigInteger(((string)o)[0]);
        }

        if ((ce.E.Type.IsNumericBased(Type.NumericPersuasion.Int) || ce.E.Type.IsBitVectorType) && ce.Type.IsCharType) {
          var b = (BigInteger)o;
          if (b < BigInteger.Zero || b > new BigInteger(65535)) {
            return null; // Argument out of range
          }
          return ((char)(int)b).ToString();
        }

        if (ce.E.Type.IsCharType && ce.Type.IsNumericBased(Type.NumericPersuasion.Real)) {
          if (AsUnconstrainedType(ce.Type) == null) {
            return null;
          }
          return BaseTypes.BigDec.FromInt(((string)o)[0]);
        }

        if (ce.E.Type.IsNumericBased(Type.NumericPersuasion.Real) && ce.Type.IsCharType) {
          ((BaseTypes.BigDec)o).FloorCeiling(out var ff, out _);
          if (((BaseTypes.BigDec)o) != BaseTypes.BigDec.FromBigInt(ff)) {
            return null; // Argument not an integer
          }
          if (ff < BigInteger.Zero || ff > new BigInteger(65535)) {
            return null; // Argument out of range
          }
          return ((char)(int)ff).ToString();
        }

      } else if (e is SeqSelectExpr sse) {
        var b = GetAnyConst(sse.Seq, constants) as string;
        var index = (BigInteger)GetAnyConst(sse.E0, constants);
        if (b == null) {
          return null;
        }
        if (index < 0 || index >= b.Length || index > Int32.MaxValue) {
          return null; // Index out of range
        }
        return b[(int)index].ToString();
      } else if (e is ITEExpr ite) {
        var b = GetAnyConst(ite.Test, constants);
        if (b == null) {
          return null;
        }
        return ((bool)b) ? GetAnyConst(ite.Thn, constants) : GetAnyConst(ite.Els, constants);
      } else if (e is ConcreteSyntaxExpression n) {
        return GetAnyConst(n.ResolvedExpression, constants);
      } else {
        return null;
      }

      return null;
    }
  }
}
