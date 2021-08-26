using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Dafny;
using Program = Microsoft.Dafny.Program;

namespace DafnyTestGeneration {

  public static class Main {

    /// <summary>
    /// This method returns each capturedState that is unreachable, one by one,
    /// and then a line with the summary of how many such states there are, etc.
    /// Note that loop unrolling may cause false positives and the absence of
    /// loop unrolling may cause false negatives.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<string> GetDeadCodeStatistics(Program program) {

      DafnyOptions.O.TestGenOptions.Mode = TestGenerationOptions.Modes.DeadCode;
      var modifications = GetModifications(program);
      var blocksReached = modifications.Count;
      HashSet<string> allStates = new();
      HashSet<string> allDeadStates = new();

      // Generate tests based on counterexamples produced from modifications
      for (var i = modifications.Count - 1; i >= 0; i--) {
        modifications[i].GetCounterExampleLog();
        var deadStates = ((BlockBasedModification)modifications[i])
          .GetKnownDeadStates();
        if (deadStates.Count != 0) {
          foreach (var capturedState in deadStates) {
            yield return $"Code at {capturedState} is potentially unreachable.";
          }
          blocksReached--;
          allDeadStates.UnionWith(deadStates);
        }
        allStates.UnionWith(((BlockBasedModification)modifications[i])
          .GetAllStates());
      }

      yield return $"Out of {modifications.Count} basic blocks " +
                   $"({allStates.Count} capturedStates), {blocksReached} " +
                   $"({allStates.Count - allDeadStates.Count}) are reachable. " +
                   $"There might be false negatives if you are not unrolling " +
                   $"loops. False positives are always possible.";
    }

    public static IEnumerable<string> GetDeadCodeStatistics(string sourceFile) {
      var source = new StreamReader(sourceFile).ReadToEnd();
      var program = Utils.Parse(source, sourceFile);
      if (program == null) {
        yield return "Cannot parse program";
        yield break;
      }
      foreach (var line in GetDeadCodeStatistics(program)) {
        yield return line;
      }
    }

    private static List<ProgramModification> GetModifications(Program program) {
      // Translate the Program to Boogie:
      var oldPrintInstrumented = DafnyOptions.O.PrintInstrumented;
      DafnyOptions.O.PrintInstrumented = true;
      var boogiePrograms = Translator
        .Translate(program, program.reporter)
        .ToList().ConvertAll(tuple => tuple.Item2);
      DafnyOptions.O.PrintInstrumented = oldPrintInstrumented;

      // Create modifications of the program with assertions for each block\path
      ProgramModifier programModifier =
        DafnyOptions.O.TestGenOptions.Mode == TestGenerationOptions.Modes.Path
          ? new PathBasedModifier()
          : new BlockBasedModifier();
      return programModifier.Modify(boogiePrograms);
    }

    /// <summary>
    /// Generate test methods for a certain Dafny program.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<TestMethod> GetTestMethodsForProgram(
      Program program, DafnyInfo? dafnyInfo = null) {

      dafnyInfo ??= new DafnyInfo(program);
      var modifications = GetModifications(program);

      // Generate tests based on counterexamples produced from modifications
      var testMethods = new ConcurrentBag<TestMethod>();
      for (var i = modifications.Count - 1; i >= 0; i--) {
        var log = modifications[i].GetCounterExampleLog();
        if (log == null) {
          continue;
        }
        var testMethod = new TestMethod(dafnyInfo, log);
        if (testMethods.Contains(testMethod)) {
          continue;
        }
        testMethods.Add(testMethod);
        yield return testMethod;
      }
    }

    /// <summary>
    /// Return a Dafny class (as a string) with tests for the given Dafny file
    /// </summary>
    public static string GetTestClassForProgram(string sourceFile) {

      var result = "";
      var source = new StreamReader(sourceFile).ReadToEnd();
      var program = Utils.Parse(source, sourceFile);
      if (program == null) {
        return result;
      }
      var dafnyInfo = new DafnyInfo(program);
      var rawName = sourceFile.Split("/").Last().Split(".").First();

      result += $"include \"{rawName}.dfy\"\n";
      result += $"module {rawName}UnitTests {{\n";
      result += string.Join("\n", dafnyInfo.ToImport
        .Select(module => $"import {module}")) + "\n";

      result = GetTestMethodsForProgram(program, dafnyInfo)
        .Aggregate(result, (current, method) => current + method + "\n");

      return result + "}";
    }
  }
}