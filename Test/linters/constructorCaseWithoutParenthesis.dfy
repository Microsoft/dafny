// RUN: %dafny /compile:0 /noParenthesisConstructor "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

module WithWarning {
  datatype Color = Red | Green | ShadesOfGray(nat) 
  method MonochromaticMethod(c: Color) returns (x: bool) {
    return match c
      case ShadesOfGray => true
      case anythingElse => false;
  }
  method MonoMethod(c: Color) returns (x: bool) {
    return match c
      case Green => true
      case anythingElse => false;  
  }
  function method MonochromaticFunction(c: Color) : bool {
    match c
      case ShadesOfGray => true
      case anythingElse => false      
  }
  method MonochromaticMethodloop(c: Color) returns (x: bool)  {
    var test := false;
    while test    
    {
       test := match c
         case ShadesOfGray => true      
         case anythingElse => false;
    }
    return false; 
  }
   
}

module WithoutWarning {
  datatype Color = Red | Green | ShadesOfGray(nat) | MixColors(nat, nat) 
  method MonochromaticMethod(c: Color) returns (x: bool) {
        return match c
          case ShadesOfGray(_) => true
          case Green() => true
          case anythingElse => false;
  }
  method TestMethod(c: Color) returns (x: bool) {          
        return match c
          case MixColors(cat, dog) => true
          case anythingElse => false;
  }    
  method MonoMethod(c: Color) returns (x: bool) {               
        return match c
          case Green() => true
          case anythingElse => false;
  }
  function method MonochromaticFunction(c: Color) : bool {
        match c
          case ShadesOfGray(_) => true
          case anythingElse => false
  }
  method MonochromaticMethodloop(c: Color) returns (x: bool)  {
        while false {
          x := match c
              case ShadesOfGray(_) => true
              case anythingElse => false;
        }
      return false; 
  }
  method Main() {
        var x := MonochromaticMethod(Green); 
        print MonochromaticFunction(Green);
        var y := MonochromaticMethodloop(Green);
      }
}



