using System;
using System.Collections.Generic;
using System.Linq;
using DafnyDriver.Test.XUnitExtensions;

namespace DafnyDriver.Test {
  
  /**
   * Specialization of CLITestCase that mainly exists to support a much more
   * consice definition of ToString().
   */
  public class DafnyTestCase : CLITestCase {

    private static readonly Dictionary<string, object> defaultDafnyOptions = new() {
      ["countVerificationErrors"] = "0",

      // We do not want absolute or relative paths in error messages, just the basename of the file
      ["useBaseNameForFileName"] = "yes",

      // We do not want output such as "Compiled program written to Foo.cs"
      // from the compilers, since that changes with the target language
      ["compileVerbose"] = "0"
    };

    private static IEnumerable<string> OptionsToFullArguments(string sourcePath,
      Dictionary<string, object> dafnyOptions, List<string> otherFiles) 
    {
      Dictionary<string, object> optionsWithDefaults = new(defaultDafnyOptions);
      foreach (var (key, value) in dafnyOptions) {
        optionsWithDefaults[key] = value;
      }

      return new[] {
        "run --no-build --project ",
        DafnyTestSpec.DAFNY_PROJ,
        " --"
      }.Concat(OptionsToArguments(sourcePath, optionsWithDefaults, otherFiles));
    }

    private static IEnumerable<string> OptionsToArguments(string sourcePath, Dictionary<string, object> dafnyOptions, List<string> otherFiles) {
      return new []{ sourcePath }
        .Concat(otherFiles)
        .Concat(dafnyOptions.Select(ConfigPairToArgument));
    }
    
    private static string ConfigPairToArgument(KeyValuePair<string, object> pair) {
      if (pair.Value.Equals("yes")) {
        return $"/{pair.Key}";
      }
      return $"/{pair.Key}:{pair.Value}";
    }

    public readonly string SourcePath;
    
    public Dictionary<string, object> DafnyOptions = new();
    public List<string> OtherFiles = new();

    public DafnyTestCase(string sourcePath, Dictionary<string, object> dafnyOptions, List<string> otherFiles, Expectation expected)
      : base("dotnet", OptionsToFullArguments(sourcePath, dafnyOptions, otherFiles), expected)
    {
      SourcePath = sourcePath;
      DafnyOptions = dafnyOptions;
      OtherFiles = otherFiles;
      Expected = expected;
    }
    
    public DafnyTestCase() {
      
    }
    
    public override string ToString() {
      return String.Join(" ", OptionsToArguments(SourcePath, DafnyOptions, OtherFiles))  + " => " + Expected;
    }
  }
}