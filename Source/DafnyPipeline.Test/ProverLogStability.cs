using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DafnyCore.Test;
using DafnyTestGeneration;
using Microsoft.Boogie;
using Microsoft.Dafny;
using Xunit;
using Xunit.Abstractions;
using BoogieProgram = Microsoft.Boogie.Program;

namespace DafnyPipeline.Test {
  // Main.Resolve has static shared state (TypeConstraint.ErrorsToBeReported for example)
  // so we can't execute tests that use it in parallel.
  [Collection("Singleton Test Collection - Resolution")]
  public class ProverLogStabilityTest {

    private readonly ITestOutputHelper testOutputHelper;

    // All types of top level declarations.
    readonly string originalProgram = @"
module SomeModule {

  module NestedModule {
    class C {
      var f: int
      constructor ()
    }
  }

  method m() {
    var x: NestedModule.C;
    x := new NestedModule.C();
    x.f := 4;
  }
}

import opened SomeModule

type FooSynonym<T> = FooClass

class FooClass {
  var f: int
  var c: NestedModule.C
 
  constructor ()
}

datatype Friends = Agnes | Agatha | Jermaine

function SomeFunc(funcFormal: int, foo: FooClass): nat 
  reads foo, foo.c 
  ensures SomeFunc(funcFormal, foo) == foo.c.f
{ 
  3
}

method SomeMethod(methodFormal: int) returns (result: bool)
  requires methodFormal == 2
  ensures result == (methodFormal == 2) 
  // ensures forall x :: x == methodFormal
{
  m();
  var lambdaExpr := x => x + 1;
  var c := new FooClass();
  result := methodFormal == SomeFunc(42, c);
}
";

    public ProverLogStabilityTest(ITestOutputHelper testOutputHelper) {
      this.testOutputHelper = testOutputHelper;
    }

    /// <summary>
    /// This test is meant to detect _any_ changes in Dafny's verification behavior.
    /// Dafny's verification is powered by an SMT solver. For difficult inputs, such solvers may change their behavior,
    /// both in performance and outcome, even when tiny non-semantic changes such as changes in variable names,
    /// are made in the input.
    ///
    /// To detect whether it is possible that the verification of Dafny proofs has changed,
    /// this test compares the input Dafny sends to the SMT solver against what it was sending previously.
    ///
    /// If this test fails, that means a change was made to Dafny that changes the SMT input it sends.
    /// If this was intentional, you should update this test's expect file with the new SMT input.
    /// The git history of updates to this test allows us to easily see when Dafny's verification has changed.
    ///
    /// Note that this test does not detect changes in DafnyPrelude.bplf
    /// 
    /// </summary>
    [Fact]
    public async Task ProverLogRegression() {
      var options = DafnyOptions.Create((TextWriter)new WriterFromOutputHelper(testOutputHelper));
      options.ProcsToCheck.Add("SomeMethod*");

      var filePath = Path.Combine(Directory.GetCurrentDirectory(), "expectedProverLog.smt2");
      var expectation = await File.ReadAllTextAsync(filePath);
      var regularProverLog = await GetProverLogForProgramAsync(options, GetBoogie(options, originalProgram));
      Assert.Equal(expectation, regularProverLog.Replace("\r", ""));
    }

    private async Task<string> GetProverLogForProgramAsync(DafnyOptions options, IEnumerable<Microsoft.Boogie.Program> boogiePrograms) {
      var logs = await GetProverLogsForProgramAsync(options, boogiePrograms).ToListAsync();
      Assert.Single(logs);
      return logs[0];
    }

    private async IAsyncEnumerable<string> GetProverLogsForProgramAsync(DafnyOptions options,
      IEnumerable<BoogieProgram> boogiePrograms) {
      string directory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      Directory.CreateDirectory(directory);
      var temp1 = directory + "/proverLog";
      options.ProverLogFilePath = temp1;
      using (var engine = ExecutionEngine.CreateWithoutSharedCache(options)) {
        foreach (var boogieProgram in boogiePrograms) {
          var (outcome, _) = await DafnyMain.BoogieOnce(options, options.OutputWriter, engine, "", "", boogieProgram, "programId");
        }
      }
      foreach (var proverFile in Directory.GetFiles(directory)) {
        yield return await File.ReadAllTextAsync(proverFile);
      }
    }

    IEnumerable<BoogieProgram> GetBoogie(DafnyOptions options, string dafnyProgramText) {
      var reporter = new BatchErrorReporter(options);
      var dafnyProgram = Utils.Parse(reporter, dafnyProgramText, false);
      Assert.NotNull(dafnyProgram);
      DafnyMain.Resolve(dafnyProgram);
      Assert.Equal(0, reporter.ErrorCount);
      return Translator.Translate(dafnyProgram, reporter).Select(t => t.Item2).ToList();
    }
  }
}
