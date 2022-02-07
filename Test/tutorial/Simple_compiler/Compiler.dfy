// RUN: %dafny /compile:0 /spillTargetCode:3 "%s"
// RUN: dotnet run --project %S/SimpleCompiler.csproj -- %S/example_input.calc > "%t"
// RUN: %diff "%s.expect" "%t"

/// A simple compiler pipeline
/// ==========================
///
/// This file implements and verifies the Dafny part of the compiler pipeline
/// documented in README.md.
///
/// Source language
/// ---------------

module DafnyAST {

/// We have two kinds of AST nodes:
///
/// 1. Expressions:
///    Constants, variable accesses, or binary operations.

  datatype BinOp =
    | Add
    | Sub

  datatype Expr =
    | Const(n: int)
    | Var(v: string)
    | Op(op: BinOp, e1: Expr, e2: Expr)

/// 2. Statements:
///     Either no-ops or print statements (or a sequence of two statements)

  datatype Stmt =
    | Skip
    | Print(e: Expr)
    | Assign(v: string, e: Expr)
    | Seq(s1: Stmt, s2: Stmt)

/// The semantics of expressions and statements can be defined using recursive interpreters:

  type Context = map<string, int>

  function method interpExpr(e: Expr, ctx: Context): int {
    match e {
      case Const(n) => n
      case Var(v) => if v in ctx.Keys then ctx[v] else 0
      case Op(Add, e1, e2) =>
        interpExpr(e1, ctx) + interpExpr(e2, ctx)
      case Op(Sub, e1, e2) =>
        interpExpr(e1, ctx) - interpExpr(e2, ctx)
    }
  }

  datatype InterpResult = InterpResult(ctx: Context, output: seq<int>)

  function method interpStmt'(s: Stmt, ctx: Context) : InterpResult
  {
    match s {
      case Skip => InterpResult(ctx, [])
      case Print(e) => InterpResult(ctx, [interpExpr(e, ctx)])
      case Assign(v, e) => InterpResult(ctx[v := interpExpr(e, ctx)], [])
      case Seq(s1, s2) =>
        var InterpResult(ctx1, o1) := interpStmt'(s1, ctx);
        var InterpResult(ctx2, o2) := interpStmt'(s2, ctx1);
        InterpResult(ctx2, o1 + o2)
    }
  }

  function method interpStmt(s: Stmt, ctx: Context) : seq<int> {
    interpStmt'(s, ctx).output
  }
}


/// Transforming Dafny ASTs
/// -----------------------
///
/// Before compiling, here is a simple rewriting pass that eliminates unnecessary operations (`0 + x → x`, etc.), along with its (fully automated) proofs.

module Rewriter {
  import opened DafnyAST

  function method simplifyExpr(e: Expr) : Expr
    ensures forall ctx: Context ::
              DafnyAST.interpExpr(simplifyExpr(e), ctx) ==
              DafnyAST.interpExpr(e, ctx)
  {
    match e {
      case Const(n) => e
      case Var(v) => e
      case Op(op, e1, e2) =>
        match (op, simplifyExpr(e1), simplifyExpr(e2)) {
          case (_, Const(0), Const(0)) => Const(0)
          case (Add, Const(0), e2) => e2
          case (Add, e1, Const(0)) => e1
          case (Sub, e1, Const(0)) => e1
          case (_, e1, e2) => Op(op, e1, e2)
        }
    }
  }

  function method simplifyStmt(s: Stmt) : Stmt
    ensures forall ctx: Context ::
              DafnyAST.interpStmt'(simplifyStmt(s), ctx) ==
              DafnyAST.interpStmt'(s, ctx)
  {
    match s {
      case Skip =>
        Skip
      case Print(e) =>
        Print(simplifyExpr(e))
      case Assign(v, e) =>
        Assign(v, simplifyExpr(e))
      case Seq(s1, s2) =>
        match (simplifyStmt(s1), simplifyStmt(s2)) {
          case (s1', Skip) =>
            assert forall o: seq<int> :: o + [] == o; // FIXME
            // assert forall ctx: Context ::
            //   DafnyAST.interpStmt'(s1', ctx) ==
            //   DafnyAST.interpStmt'(s, ctx) by {
            //     forall ctx: Context
            //       ensures DafnyAST.interpStmt'(s1', ctx) ==
            //               DafnyAST.interpStmt'(s, ctx) {
            //       calc {
            //           DafnyAST.interpStmt'(s, ctx);
            //         ==
            //           DafnyAST.interpStmt'(Seq(s1, s2), ctx);
            //         ==
            //           var (ctx1, o1) := interpStmt'(s1, ctx);
            //           var (ctx2, o2) := interpStmt'(s2, ctx1);
            //           (ctx2, o1 + o2);
            //         ==
            //           var (ctx1, o1) := interpStmt'(s1', ctx);
            //           var (ctx2, o2) := interpStmt'(Skip, ctx1);
            //           (ctx2, o1 + o2);
            //         ==
            //           var (ctx1, o1) := interpStmt'(s1', ctx);
            //           var (ctx2, o2) := (ctx1, []);
            //           (ctx2, o1 + o2);
            //         ==
            //           var (ctx1, o1) := interpStmt'(s1', ctx);
            //           (ctx1, o1 + []);
            //         ==
            //           var (ctx1, o1) := interpStmt'(s1', ctx);
            //           (ctx1, o1);
            //         ==
            //           interpStmt'(s1', ctx);
            //       }
            //     }
            //   }
            s1'
          case (Skip, s2') =>
            assert forall o: seq<int> :: [] + o == o; // WISH: why is this needed, why didn't I need it in the previous version (without contexts), and how should I have found this faster?
            // assert forall ctx: Context ::
            //   DafnyAST.interpStmt'(s2', ctx) ==
            //   DafnyAST.interpStmt'(s, ctx) by {
            //     forall ctx: Context
            //       ensures DafnyAST.interpStmt'(s2', ctx) ==
            //               DafnyAST.interpStmt'(s, ctx) {
            //       calc {
            //           DafnyAST.interpStmt'(s, ctx);
            //         ==
            //           DafnyAST.interpStmt'(Seq(s1, s2), ctx);
            //         ==
            //           var (ctx1, o1) := interpStmt'(s1, ctx);
            //           var (ctx2, o2) := interpStmt'(s2, ctx1);
            //           (ctx2, o1 + o2);
            //         ==
            //           var (ctx1, o1) := interpStmt'(Skip, ctx);
            //           var (ctx2, o2) := interpStmt'(s2', ctx1);
            //           (ctx2, o1 + o2);
            //         ==
            //           var (ctx1, o1) := (ctx, []);
            //           var (ctx2, o2) := interpStmt'(s2', ctx1);
            //           (ctx2, o1 + o2);
            //         ==
            //           var (ctx2, o2) := interpStmt'(s2', ctx);
            //           (ctx2, [] + o2);
            //         ==
            //           var (ctx2, o2) := interpStmt'(s2', ctx);
            //           (ctx2, o2);
            //         ==
            //           interpStmt'(s2', ctx);
            //       }
            //     }
            //   }
            s2'
          case (s1', s2') => Seq(s1', s2')
        }
    }
  }
}

/// Target language
/// ---------------
///
/// Our target language targets a very simple stack machine.  The machine reads and executes instructions sequentially and modifies its state by pushing or popping from a data stack and writing to an output channel.  Local variables are stored in an infinite register file.
///
/// For convenient, programs are represented using a simple linked-list type:

module LinkedList {
  datatype List<T> =
    | Cons(hd: T, tl: List<T>)
    | Nil

  function method Concat<T>(l1: List<T>, l2: List<T>) : List<T> {
    match l1 {
      case Nil => l2
      case Cons(h, t) => Cons(h, Concat<T>(t, l2))
    }
  }
}

module StackMachine {
  import opened LinkedList

/// The machine has 4 instructions:
///
/// 1. `PushConst(n)` adds `n` to the machine's stack.
/// 2. `PushVar(v)` reads register `v` and pushes its value to the machine's stack.
/// 3. `PopAdd` removes the top two elements of stack, sums them, and pushes the result.
/// 4. `PopSub` removes the top two elements of stack, subtract the first one from the second one, and pushes the result.
/// 5. `PopPrint` removes the top element of the stack and writes it to the output channel.
/// 6. `PopVar(v)` removes the top element of the stack and stores it in register `v`.

  datatype Instr =
    | PushConst(n: int)
    | PushVar(v: string)
    | PopAdd
    | PopSub
    | PopPrint
    | PopVar(v: string)

/// Programs are modeled using a linked list:

  type Prog = List<Instr>

/// And semantics are given using an interpreter whose state combines a stack of values, a map of local variables (registers), and a list of outputs.

  type RegisterFile = map<string, int>
  datatype State = State(stack: List<int>,
                         regs: RegisterFile,
                         output: seq<int>)

  function method interpInstr(instr: Instr, st: State) : State {
    match (instr, st.stack) {
      case (PushConst(n), tl) =>
        st.(stack := Cons(n, tl))
      case (PushVar(v), tl) =>
        var val := if v in st.regs.Keys then st.regs[v] else 0;
        st.(stack := Cons(val, tl))
      case (PopAdd, Cons(n2, Cons(n1, tl))) =>
        st.(stack := Cons(n1 + n2, tl))
      case (PopSub, Cons(n2, Cons(n1, tl))) =>
        st.(stack := Cons(n1 - n2, tl))
      case (PopPrint, Cons(n, tl)) =>
        st.(stack := tl, output := st.output + [n])
      case (PopVar(v), Cons(n, tl)) =>
        st.(stack := tl, regs := st.regs[v := n])
      // Error cases
      case (PopAdd, _) => st
      case (PopSub, _) => st
      case (PopPrint, _) => st
      case (PopVar, _) => st
    }
  }

  function method interpProg'(p: Prog, st: State) : State {
    match p {
      case Nil => st
      case Cons(instr, p) => interpInstr(instr, interpProg'(p, st))
    }
  }

  const EmptyState := State(Nil, map[], []);

  function method interpProg(p: Prog, input: RegisterFile) : seq<int> {
    interpProg'(p, EmptyState.(regs := input)).output
  }
}

module Compiler {
  import opened LinkedList

  import DafnyAST
  import opened StackMachine

  function method compileExpr(e: DafnyAST.Expr): Prog {
    match e {
      case Const(n) => Cons(PushConst(n), Nil)
      case Var(v) => Cons(PushVar(v), Nil)
      case Op(Add, e1, e2) => Cons(PopAdd, Concat(compileExpr(e2), compileExpr(e1)))
      case Op(Sub, e1, e2) => Cons(PopSub, Concat(compileExpr(e2), compileExpr(e1)))
    }
  }

  function method compileStmt(s: DafnyAST.Stmt): Prog {
    match s {
      case Skip => Nil
      case Assign(v, e) => Cons(PopVar(v), compileExpr(e))
      case Print(e) => Cons(PopPrint, compileExpr(e))
      case Seq(s1, s2) => Concat(compileStmt(s2), compileStmt(s1))
    }
  }

  lemma interpProg'_Concat(p1: Prog, p2: Prog, st: State)
    ensures interpProg'(LinkedList.Concat(p1, p2), st) ==
            interpProg'(p1, interpProg'(p2, st))
  {}

  lemma {:induction false} compileExprCorrect'(e: DafnyAST.Expr, st: State) // FIXME default induction on e, st breaks things
    ensures interpProg'(compileExpr(e), st) ==
            st.(stack := Cons(DafnyAST.interpExpr(e, st.regs), st.stack))
  {
    match e {
      case Const(n) =>
      case Var(v) =>
      case Op(op, e1, e2) => // Here's the expanded version of the same proof
        interpProg'_Concat(compileExpr(e2), compileExpr(e1), st);
        compileExprCorrect'(e1, st);
        var st' := st.(stack := Cons(DafnyAST.interpExpr(e1, st.regs), st.stack));
        compileExprCorrect'(e2, st');
        // var st'' := st'.(stack := Cons(DafnyAST.interpExpr(e2, st'.regs), st'.stack));
    }
  }

  lemma {:induction false} compileStmtCorrect'(s: DafnyAST.Stmt, st: State)
    ensures interpProg'(compileStmt(s), st) ==
            var InterpResult(ctx', output) := DafnyAST.interpStmt'(s, st.regs);
            st.(regs := ctx', output := st.output + output)
  {
    match s {
      case Skip =>
      case Assign(v, e) =>
        compileExprCorrect'(e, st);
      case Print(e) =>
        compileExprCorrect'(e, st);
      case Seq(s1, s2) =>
        interpProg'_Concat(compileStmt(s2), compileStmt(s1), st);
        compileStmtCorrect'(s1, st);
        compileStmtCorrect'(s2, interpProg'(compileStmt(s1), st));
        // calc { // Here is the original proof
        //   interpProg'(compileStmt(s), st);
        //   interpProg'(compileStmt(DafnyAST.Seq(s1, s2)), st);
        //   interpProg'(Concat(compileStmt(s2), compileStmt(s1)), st);
        //   { interpProg'_Concat(compileStmt(s2), compileStmt(s1), st); }
        //   interpProg'(compileStmt(s2), interpProg'(compileStmt(s1), st));
        //   { compileStmtCorrect'(s1, st); }
        //   var (ctx1, o1) := DafnyAST.interpStmt'(s1, st.regs);
        //   interpProg'(compileStmt(s2), st.(regs := ctx1, output := st.output + o1));
        //   == { compileStmtCorrect'(s2,
        //          var (ctx', output) := DafnyAST.interpStmt'(s1, st.regs);
        //          st.(regs := ctx', output := st.output + output)); }
        //   var (ctx1, o1) := DafnyAST.interpStmt'(s1, st.regs);
        //   var (ctx2, o2) := DafnyAST.interpStmt'(s2, ctx1);
        //   st.(regs := ctx2, output := st.output + o1 + o2);
        // }
    }
  }

  lemma compileStmtCorrect(s: DafnyAST.Stmt)
    ensures forall input: DafnyAST.Context ::
              interpProg'(compileStmt(s), EmptyState.(regs := input)).output ==
              DafnyAST.interpStmt'(s, input).output
  {
    forall input: DafnyAST.Context {
      compileStmtCorrect'(s, EmptyState.(regs := input));
    }
  }

  lemma compileCorrect(s: DafnyAST.Stmt)
    ensures forall input: DafnyAST.Context ::
              interpProg(compileStmt(s), input) == DafnyAST.interpStmt(s, input)
  {
    compileStmtCorrect(s);
  }
}

/// Connecting C# to Dafny
/// ----------------------
///
/// Our Dafny ASTs are defined in terms of Dafny ``datatype`` values, but the AST produced using ANTLR on the C# side is defined through a class hierarchy (see ``Main.cs``).  To connect the two we need a translation function.  Here we write it in Dafny, which allows us to depend on fewer specifics of the compilation process.
///
/// To write the translator in Dafny we need a Dafny model of the C# class hierarchy.  Below we use traits, annotated with `{:extern}` to specify their C# names and `{:compile false}` to indicate that they should not be compiled: they act as shims that we can write the Dafny code against.  Then, once compiled to C#, our Dafny code can be linked against the real C# class hierarchy.
///
/// The model does not have to be completely faithful: for example we won't capture the fact that Expression and Statement share a base class on the C# side.
///
/// There are a few interesting difficulties: native C# types, nested types, enums:
///
/// 1. The C# side uses native C# types like `List<T>` (we'll model them using Dafny traits).
/// 2. C# types are commonly `nested <https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/nested-types>`__, but Dafny doesn't support nesting types (we'll use `:extern` annotations on the traits to map Dafny non-nested traits to C# nested types, e.g. in `Op__BinOp` below).
/// 3. Dafny datatypes don't compile to C# enums, so we cannot use them to model those (instead we'll model enum members as `static const` fields of a Dafny class, as in `Add: Op__BinOp` below).

/// Modeling C# types in Dafny
/// ~~~~~~~~~~~~~~~~~~~~~~~~~~
///
/// Native types
/// ^^^^^^^^^^^^
///
/// In this example the native C# types that we use are `int`, `string`, and `List<T>`.
///
/// For `int`, we use Dafny's support for subset types:

module NativeTypes {
  newtype nativeint = n: int | -0x8000_0000 <= n < 0x8000_0000
}

/// For `string` we add a model of the native string type, and below (in `CSharpUtils`) we declare an additional method `StringAsDafnyString`.

module {:extern "System"} {:compile false} System {
  class {:extern} {:compile false} String {}
}

/// We add a model of the no-arguments `List<T>` constructor and of the `Add` method to build new lists.  For iterating over existing lists (passed from C#) using the native ``Aggregate`` method provided by Linq would be enough, but to make this example more informative we define a custom method `FoldR` instead, model it in Dafny as an `:extern` method, and implement in ``Main.cs``.

module {:extern "System.Collections.Generic"} {:compile false} System.Collections.Generic {
  class {:extern} {:compile false} List<T> {
    constructor {:extern} ()

    method {:extern} Add(t: T)
      modifies this
  }
}

module {:extern "SimpleCompiler.CSharpUtils"} CSharpUtils {
  import LinkedList
  import opened System
  import opened System.Collections.Generic

  class StringUtils {
    // It's OK to model this as a function method because Dafny's `string` is a
    // value type (otherwise it would be invalid in general to assume that
    // calling the method twice produces equal results).
    static function method {:extern}
      StringAsDafnyString(s: String): string
  }

  class ListUtils {
    static function method {:extern}
      FoldR<A, B>(f: (A, B) -> B, b0: B, l: List<A>) : B

    static method LinkedListToCList<T>(ll: LinkedList.List<T>) returns (l: List<T>) {
      l := new List<T>();
      var it := ll;
      while it.Cons?
        decreases it
      {
        l.Add(it.hd);
        it := it.tl;
      }
    }
  }
}

/// User-defined types
/// ^^^^^^^^^^^^^^^^^^
///
/// This part follows the structure of the ASTs defined in ``Main.cs``.  Note, however, that:
///
/// 1. ``Op.BinOp`` has been renamed to `Op__BinOp`; this is because Dafny does not support nested types.  We ensure that things line back up when we compile to C# by adding an `extern` name to `Op__BinOp`.
///
/// 2. Fields of the ``Op.Binop`` enum are now `static const` fields of the `Op__BinOp` class.
///
/// 3. The AST types do not line up exactly: C# has ``Prog`` as a separate type of AST node, whereas the Dafny datatype AST `Stmt` has a `Seq` constructor instead.

module {:extern "SimpleCompiler.CSharpAST"} CSharpAST {
  import System
  import opened NativeTypes

  class {:extern "Op.BinOp"} Op__BinOp {
    static const {:extern} Add: Op__BinOp
    static const {:extern} Sub: Op__BinOp
    function method {:extern} Equals(other: Op__BinOp): bool
  }

  trait {:compile false} {:extern} Expr {}

  trait {:compile false} {:extern} Const extends Expr {
    var n: nativeint
  }

  trait {:compile false} {:extern} Var extends Expr {
    var v: System.String
  }

  trait {:compile false} {:extern} Op extends Expr {
    var op: Op__BinOp
    var e1: Expr
    var e2: Expr
  }

  trait {:compile false} {:extern} Stmt {}

  trait {:compile false} {:extern} Print extends Stmt {
    var e: Expr
  }

  trait {:compile false} {:extern} Assign extends Stmt {
    var v: System.String;
    var e: Expr
  }

  trait {:compile false} {:extern} Prog {
    var s: System.Collections.Generic.List<Stmt>
  }
}

/// Translating C# inputs
/// ~~~~~~~~~~~~~~~~~~~~~
///
/// With these definitions in place the translation is straightforward.  The translation functions are marked `{:verify false}` because:
///
/// 1. We cannot statically guarantee that the C# types that get passed in are not cyclic, so they could in fact loop forever; and
/// 2. The C# class hierarchy is not sealed, so we cannot statically guarantee that we cover all cases (for simplicity the functions below just enter an infinite loop when they encounter an unexpected case).

module Translator {
  import CSharpAST
  import DafnyAST
  import opened CSharpUtils
  import opened LinkedList

  function method {:verify false} translateOp(op: CSharpAST.Op__BinOp)
    : DafnyAST.BinOp
  {
    if op.Equals(CSharpAST.Op__BinOp.Add) then DafnyAST.Add
    else if op.Equals(CSharpAST.Op__BinOp.Sub) then DafnyAST.Sub
    else translateOp(op)
  }

  function method {:verify false} translateExpr(c: CSharpAST.Expr)
    : DafnyAST.Expr
    reads *
  {
    if c is CSharpAST.Const then
      var c := c as CSharpAST.Const;
      DafnyAST.Const(c.n as int)
    else if c is CSharpAST.Var then
      var c := c as CSharpAST.Var;
      DafnyAST.Var(StringUtils.StringAsDafnyString(c.v))
    else if c is CSharpAST.Op then
      var c := c as CSharpAST.Op;
      DafnyAST.Op(translateOp(c.op), translateExpr(c.e1), translateExpr(c.e2))
    else
      assert false;
      translateExpr(c)
  }

  function method {:verify false} translateStmt(c: CSharpAST.Stmt)
    : DafnyAST.Stmt
    reads *
  {
    if c is CSharpAST.Print then
      var c := c as CSharpAST.Print;
      DafnyAST.Print(translateExpr(c.e))
    else if c is CSharpAST.Assign then
      var c := c as CSharpAST.Assign;
      DafnyAST.Assign(StringUtils.StringAsDafnyString(c.v), translateExpr(c.e))
    else
      translateStmt(c)
  }

  function method {:verify false} translateProg(c: CSharpAST.Prog)
    : DafnyAST.Stmt
    reads *
  {
    ListUtils.FoldR(
      (c, ds) => DafnyAST.Seq(translateStmt(c), ds),
      DafnyAST.Skip,
      c.s
    )
  }
}

/// Pretty-printing Dafny's outputs
/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
///
/// In general passing complex types between Dafny and C# requires some efforts (e.g. the modeling above), so for the final part of this pipeline (returning the data to C#) we'll use a very simple type: a list of strings (one per instruction).  Conveniently, the `ToString` method of Dafny strings provides a trivial conversion to C# strings, so we leave that conversion to the C# side.
///
/// Alternatively, we could have chosen to expose the stack machine types to C# directly and do the pretty-printing from there.

module PrettyPrint {
  import opened LinkedList
  import opened StackMachine

  function method prettyPrintNum(n: int, zero: string) : string
    decreases n < 0, if n < 0 then -n else n
  {
    if n == 0 then zero
    else if n < 0 then prettyPrintNum(-n, zero)
    else if n < 10 then ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"][n]
    else if n >= 10 then prettyPrintNum(n / 10, "") + prettyPrintNum(n % 10, "0")
    else assert false; prettyPrintNum(n, zero)
  }

  function method prettyPrintInstr(instr: Instr) : string {
    match instr {
      case PushConst(n) => "PushConst(" + prettyPrintNum(n, "0") + ")"
      case PushVar(v) => "PushVar(" + v + ")"
      case PopAdd => "PopAdd"
      case PopSub => "PopSub"
      case PopPrint => "PopPrint"
      case PopVar(v) => "PopVar(" + v + ")"
    }
  }

  function method prettyPrint(p: Prog) : List<string> {
    match p {
      case Nil => Nil
      case Cons(instr, p) => Cons(prettyPrintInstr(instr), prettyPrint(p))
    }
  }
}

/// Exposing Dafny to C#
/// ~~~~~~~~~~~~~~~~~~~~
///
/// Finally, we define a single Dafny function that serves as our interface to C#.  It takes a C# AST, translates it to Dafny, runs rewriting passes, compiles it to a stack machine program, pretty-prints it to a string, and returns it to C#.
///
/// Note that the `Interop` module below has an `extern` annotation but no `{:compile false}` annotation: this is because we want to implement it in Dafny, but give it a predictable name in the generated C# code.

module {:extern "SimpleCompiler"} Interop {
  import LinkedList
  import CSharpAST
  import DafnyAST
  import Rewriter
  import StackMachine
  import Translator
  import Compiler
  import PrettyPrint
  import CSharpUtils
  import Generics = System.Collections.Generic

  class DafnyCompiler {
    static method Compile(dAST: DafnyAST.Stmt) returns (dSM: StackMachine.Prog)
      ensures forall input: DafnyAST.Context ::
                DafnyAST.interpStmt(dAST, input) == StackMachine.interpProg(dSM, input)
    {
      print "translated = \n  "; print dAST; print "\n";
      print "interp(translated) = \n  "; print DafnyAST.interpStmt(dAST, map[]); print "\n";
      print "\n";

      var optimized: DafnyAST.Stmt := Rewriter.simplifyStmt(dAST);
      print "optimized = \n  "; print optimized; print "\n";
      print "interp(optimized) = \n  "; print DafnyAST.interpStmt(optimized, map[]); print "\n";
      print "\n";

      dSM := Compiler.compileStmt(optimized);
      print "compiled = \n  "; print dSM; print "\n";
      print "interp(compiled) = \n  "; print StackMachine.interpProg(dSM, map[]); print "\n";
      print "\n";

      // Proof:
      Compiler.compileStmtCorrect(optimized);
    }

    static method CompileAndExport(cAST: CSharpAST.Prog)
      returns (output: Generics.List<string>)
    {
      var translated: DafnyAST.Stmt := Translator.translateProg(cAST);
      var compiled: StackMachine.Prog := Compile(translated);
      var prettyPrinted: LinkedList.List<string> := PrettyPrint.prettyPrint(compiled);
      output := CSharpUtils.ListUtils.LinkedListToCList(prettyPrinted);
    }
  }
}

/// With all this, we're ready to connect to C# and run our compiler.  Read through ``Main.cs`` to see how this file is called.
