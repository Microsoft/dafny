using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;

namespace Microsoft.Dafny;

public interface ICommandSpec {

  public static Argument<FileInfo> FileArgument { get; }

  private static ValidateSymbolResult<ArgumentResult> ValidateFileArgument() {
    return r => {
      var value = r.Tokens[0].Value;
      if (value.StartsWith("--")) {
        r.ErrorMessage = $"{value} is not a valid argument";
      }
    };
  }

  static ICommandSpec() {
    FilesArgument = new Argument<IEnumerable<FileInfo>>("file", "input files");
    FilesArgument.AddValidator(ValidateFileArgument());
  }
  public static Argument<IEnumerable<FileInfo>> FilesArgument { get; }

  public static IReadOnlyList<Option> VerificationOptions = new Option[] {
    BoogieOptionBag.NoVerify,
    BoogieOptionBag.VerificationTimeLimit,
    MiscOptionBag.VerifyIncludedFiles
  }.ToList();

  public static IReadOnlyList<Option> ExecutionOptions = new Option[] {
    MiscOptionBag.Target,
    MiscOptionBag.EnforceDeterminism
  }.ToList();

  public static IReadOnlyList<Option> ConsoleOutputOptions = new List<Option>(new Option[] {
    DafnyConsolePrinter.ShowSnippets,
    DeveloperOptionBag.UseBaseFileName,
    DeveloperOptionBag.Print,
    DeveloperOptionBag.ResolvedPrint,
    DeveloperOptionBag.BoogiePrint,
    MiscOptionBag.WarningAsErrors,
  });
  
  public static IReadOnlyList<Option> CommonOptions = new List<Option>(new Option[] {
    BoogieOptionBag.Cores,
    MiscOptionBag.Libraries,
    MiscOptionBag.Plugin,
    BoogieOptionBag.BoogieArguments,
    MiscOptionBag.Prelude,
    MiscOptionBag.RelaxDefiniteAssignment,
    Function.FunctionSyntaxOption,
    MiscOptionBag.QuantifierSyntax,
    MiscOptionBag.WarnShadowing,
    MiscOptionBag.WarnMissingConstructorParenthesis,
    PrintStmt.TrackPrintEffectsOption,
    MiscOptionBag.DisableNonLinearArithmetic,
    MiscOptionBag.UnicodeCharacters,
  });

  IEnumerable<Option> Options { get; }

  Command Create();

  void PostProcess(DafnyOptions dafnyOptions, Options options, InvocationContext context);
}
