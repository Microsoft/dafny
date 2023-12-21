// Class LowSurrogateCodePoint
// Dafny class LowSurrogateCodePoint compiled into Java
package Std.Unicode.Base;

import Std.Wrappers.*;
import Std.BoundedInts.*;
import Std.Base64.*;
import Std.Math.*;
import Std.Collections.Seq.*;
import Std.Collections.Array.*;
import Std.Collections.Imap.*;
import Std.Collections.Map.*;
import Std.Collections.Set.*;
import Std.DynamicArray.*;
import Std.FileIO.*;
import Std.Arithmetic.MulInternals.*;
import Std.Arithmetic.ModInternals.*;
import Std.Arithmetic.DivInternals.*;
import Std.Arithmetic.DivMod.*;
import Std.Arithmetic.Power.*;
import Std.Arithmetic.Logarithm.*;
import Std.Arithmetic.Power2.*;
import Std.Strings.HexConversion.*;
import Std.Strings.DecimalConversion.*;
import Std.Strings.CharStrEscaping.*;
import Std.Strings.*;

@SuppressWarnings({"unchecked", "deprecation"})
public class LowSurrogateCodePoint {
  public LowSurrogateCodePoint() {
  }
  public static int Witness = __default.LOW__SURROGATE__MIN();
  public static int defaultValue() {
    return Witness;
  }
  private static final dafny.TypeDescriptor<java.lang.Integer> _TYPE = dafny.TypeDescriptor.intWithDefault(Witness);
  public static dafny.TypeDescriptor<java.lang.Integer> _typeDescriptor() {
    return (dafny.TypeDescriptor<java.lang.Integer>) (dafny.TypeDescriptor<?>) _TYPE;
  }
}
