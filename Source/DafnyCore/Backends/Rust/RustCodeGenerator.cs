using System;
using System.IO;
using System.Linq;
using Dafny;
using DCOMP;

namespace Microsoft.Dafny.Compilers {

  class RustCodeGenerator : DafnyWrittenCodeGenerator {

    public override ISequence<Rune> Compile(Sequence<DAST.Module> program) {
      var c = new COMP();
      c.__ctor(true);
      return c.Compile(program);
    }

    public override ISequence<Rune> EmitCallToMain(string fullName) {
      var splitByDot = fullName.Split('.');
      var convertedToUnicode = Sequence<Sequence<Rune>>.FromArray(splitByDot.Select(s => (Sequence<Rune>)Sequence<Rune>.UnicodeFromString(s)).ToArray());
      return COMP.EmitCallToMain(convertedToUnicode);
    }

  }

}