let Library = (function() {
  let $module = {};

  $module.LibClass = class LibClass {
    // static method CallMeInt(x: int) returns (y: int, z: int)
    static CallMeInt(x) {
      let y = x.plus(new BigNumber(1));
      let z = y.plus(y);
      return [y, z];
    }
    // static method CallMeNative(x: MyInt, b: bool) returns (y: MyInt)
    static CallMeNative(x, b) {
      let y = b ? x + 1 : x - 1;
      return y;
    }
  };

  $module.OtherClass = class OtherClass {
    static CallMe() {
      return "OtherClass.CallMe";
    }
  }

  $module.AllDafny = class AllDafny {
    // Just here so the generated class can extend it
  }

  $module.Mixed = class Mixed {
    constructor() { }

    // static method P()
    static P() {
      process.stdout.write("Mixed.P\n");
    }

    IP() {
      process.stdout.write("Mixed.IP\n");
    }

    static G() {
      return new BigNumber(1);
    }

    IG() {
      return new BigNumber(2);
    }
  }
  $module.AllExtern = class AllExtern {
    static P() {
      process.stdout.write("AllExtern.P\n");
    }
  };

  return $module;
})();

let Nested = (function() {
  let $module = {};

  $module.Library = (function() {
    let $module2 = {};

    $module2.Foo = function() {
      process.stdout.write("Nested.Library.Foo\n");
    }

    return $module2;
  })();
  
  return $module;
})();
