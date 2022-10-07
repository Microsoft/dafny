using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Microsoft.Dafny;

internal interface ParseArgumentResult {
}

record ParseArgumentSuccess(DafnyOptions DafnyOptions) : ParseArgumentResult;
record ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult ExitResult) : ParseArgumentResult;

static class CommandRegistry {
  private static readonly Dictionary<string, ICommandSpec> Commands = new();

  public static IReadOnlyList<IOptionSpec> CommonOptions = new List<IOptionSpec>(new IOptionSpec[] {
    CoresOption.Instance,
    VerificationTimeLimitOption.Instance,
    LibraryOption.Instance,
    ShowSnippetsOption.Instance,
    PluginOption.Instance,
    BoogieOption.Instance,
    PreludeOption.Instance,
    UseBaseFileNameOption.Instance,
    PrintOption.Instance,
    ResolvedPrintOption.Instance,
    BoogiePrintOption.Instance,
    UnicodeCharactersOption.Instance, 
  });

  static void AddCommand(ICommandSpec command) {
    Commands.Add(command.Name, command);
  }

  static CommandRegistry() {
    AddCommand(new VerifyCommand());
    AddCommand(new RunCommand());
    AddCommand(new BuildCommand());
    AddCommand(new TranslateCommand());
  }

  [CanBeNull]
  public static ParseArgumentResult Create(string[] arguments) {

    bool allowHidden = true;
    if (arguments.Length != 0) {
      var first = arguments[0];
      if (first != "--dev" && first != "--version" && first != "-h" && first != "--help" && !Commands.ContainsKey(first)) {
        var oldOptions = new DafnyOptions();
        if (oldOptions.Parse(arguments)) {
          return new ParseArgumentSuccess(oldOptions);
        }

        return new ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult.PREPROCESSING_ERROR);
      }

      if (first == "--dev") {
        allowHidden = false;
        arguments = arguments.Skip(1).ToArray();
      }
    }

    var wasInvoked = false;
    string optionFailure = null;
    var dafnyOptions = new DafnyOptions();
    var optionValues = new Dictionary<IOptionSpec, object>();
    var options = new Options(optionValues);
    dafnyOptions.Options = options;
    var filesArgument = new Argument<IEnumerable<FileInfo>>("file", "input files");
    filesArgument.AddValidator(r => {
      var value = r.Tokens[0].Value;
      if (value.StartsWith("--")) {
        r.ErrorMessage = $"{value} is not a valid argument";
      }
    });

    var optionToSpec = Commands.Values.SelectMany(c => c.Options).Distinct().ToDictionary(o => {
      var result = o.ToOption;
      if (!allowHidden) {
        result.IsHidden = false;
      }
      return result;
    }, o => o);
    var specToOption = optionToSpec.ToDictionary(o => o.Value, o => o.Key);
    var commandToSpec = Commands.Values.ToDictionary(c => {
      var result = new Command(c.Name, c.Description);
      foreach (var option in c.Options) {
        result.AddOption(specToOption[option]);
      }
      result.AddArgument(filesArgument);
      return result;
    }, c => c);
    foreach (var command in commandToSpec.Keys) {
      command.SetHandler(CommandHandler);
    }

    var rootCommand = new RootCommand("The Dafny CLI enables working with Dafny, a verification-aware programming language. Use 'dafny /help' to see help for a previous CLI format.");
    foreach (var command in commandToSpec.Keys) {
      rootCommand.AddCommand(command);
    }

    void CommandHandler(InvocationContext context) {
      wasInvoked = true;
      var command = context.ParseResult.CommandResult.Command;
      var commandSpec = commandToSpec[command];
      foreach (var option in command.Options) {
        if (!context.ParseResult.HasOption(option)) {
          continue;
        }

        var optionSpec = optionToSpec[option];
        var value = context.ParseResult.GetValueForOption(option);
        options.OptionArguments[optionSpec] = value;
        optionFailure ??= optionSpec.PostProcess(dafnyOptions);
        if (optionFailure != null) {
          break;
        }
      }

      foreach (var file in context.ParseResult.GetValueForArgument(filesArgument)) {
        dafnyOptions.AddFile(file.FullName);
      }

      dafnyOptions.ApplyDefaultOptionsWithoutSettingsDefault();
      commandSpec.PostProcess(dafnyOptions, options);

    }

    var exitCode = rootCommand.InvokeAsync(arguments).Result;
    if (optionFailure != null) {
      Console.WriteLine(optionFailure);
      return new ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult.PREPROCESSING_ERROR);
    }
    if (!wasInvoked) {
      return new ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult.OK_EXIT_EARLY);
    }
    if (exitCode == 0) {
      return new ParseArgumentSuccess(dafnyOptions);
    }

    return new ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult.PREPROCESSING_ERROR);
  }
}
