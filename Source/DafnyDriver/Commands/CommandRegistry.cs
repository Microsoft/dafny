using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Boogie;
using Microsoft.Dafny.LanguageServer;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.Dafny;

public interface ParseArgumentResult {
}

public record ParseArgumentSuccess(DafnyOptions DafnyOptions) : ParseArgumentResult;
record ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult ExitResult) : ParseArgumentResult;

public static class CommandRegistry {
  private const string ToolchainDebuggingHelpName = "--help-internal";
  private static readonly HashSet<ICommandSpec> Commands = new();

  static void AddCommand(ICommandSpec command) {
    Commands.Add(command);
  }

  static CommandRegistry() {
    AddCommand(new ResolveCommand());
    AddCommand(new VerifyCommand());
    AddCommand(new BuildCommand());
    AddCommand(new RunCommand());
    AddCommand(new TranslateCommand());
    AddCommand(new FormatCommand());
    AddCommand(new MeasureComplexityCommand());
    AddCommand(new ServerCommand());
    AddCommand(new TestCommand());
    AddCommand(new GenerateTestsCommand());
    AddCommand(new DeadCodeCommand());
    AddCommand(new AuditCommand());

    FileArgument = new Argument<FileInfo>("file", "input file");
  }

  public static Argument<FileInfo> FileArgument { get; }

  class WritersConsole : IConsole {
    private readonly TextWriter errWriter;
    private readonly TextWriter outWriter;

    public WritersConsole(TextWriter outWriter, TextWriter errWriter) {
      this.errWriter = errWriter;
      this.outWriter = outWriter;
    }

    public IStandardStreamWriter Out => StandardStreamWriter.Create(outWriter ?? TextWriter.Null);

    public bool IsOutputRedirected => outWriter != null;
    public IStandardStreamWriter Error => StandardStreamWriter.Create(errWriter ?? TextWriter.Null);
    public bool IsErrorRedirected => errWriter != null;
    public bool IsInputRedirected => false;
  }

  [CanBeNull]
  public static ParseArgumentResult Create(TextWriter outputWriter, TextWriter errorWriter, TextReader inputReader, string[] arguments) {
    bool allowHidden = arguments.All(a => a != ToolchainDebuggingHelpName);
    var console = new WritersConsole(outputWriter, errorWriter);
    var wasInvoked = false;
    var dafnyOptions = new DafnyOptions(outputWriter, inputReader);
    var optionValues = new Dictionary<Option, object>();
    var options = new Options(optionValues);
    dafnyOptions.ShowEnv = ExecutionEngineOptions.ShowEnvironment.Never;
    dafnyOptions.Environment = "Command-line arguments: " + string.Join(" ", arguments);
    dafnyOptions.Options = options;

    foreach (var option in Commands.SelectMany(c => c.Options)) {
      if (!allowHidden) {
        option.IsHidden = false;
      }
      if (!option.Arity.Equals(ArgumentArity.ZeroOrMore) && !option.Arity.Equals(ArgumentArity.OneOrMore)) {
        option.AllowMultipleArgumentsPerToken = true;
      }
    }

    var commandToSpec = Commands.ToDictionary(c => {
      var result = c.Create();
      foreach (var option in c.Options) {
        result.AddOption(option);
      }
      return result;
    }, c => c);
    foreach (var command in commandToSpec.Keys) {
      command.SetHandler(CommandHandler);
    }

    if (arguments.Length != 0) {
      var first = arguments[0];
      var keywordForNewMode = commandToSpec.Keys.Select(c => c.Name).
        Union(new[] { "--version", "-h", ToolchainDebuggingHelpName, "--help", "[parse]", "[suggest]" });
      if (!keywordForNewMode.Contains(first)) {
        if (first.Length > 0 && first[0] != '/' && first[0] != '-' && !System.IO.File.Exists(first) && first.IndexOf('.') == -1) {
          dafnyOptions.Printer.ErrorWriteLine(dafnyOptions.Writer,
            "*** Error: '{0}': The first input must be a command or a legacy option or file with supported extension", first);
          return new ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult.PREPROCESSING_ERROR);
        }
        var oldOptions = new DafnyOptions(outputWriter, inputReader);
        if (oldOptions.Parse(arguments)) {
          return new ParseArgumentSuccess(oldOptions);
        }

        return new ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult.PREPROCESSING_ERROR);
      }
    }
    dafnyOptions.UsingNewCli = true;

    var rootCommand = new RootCommand("The Dafny CLI enables working with Dafny, a verification-aware programming language. Use 'dafny /help' to see help for a previous CLI format.");
    foreach (var command in commandToSpec.Keys) {
      rootCommand.AddCommand(command);
    }

    void CommandHandler(InvocationContext context) {
      wasInvoked = true;
      var command = context.ParseResult.CommandResult.Command;
      var commandSpec = commandToSpec[command];

      var singleFile = context.ParseResult.GetValueForArgument(FileArgument);
      if (singleFile != null) {
        dafnyOptions.AddFile(singleFile.FullName);
      }
      var files = context.ParseResult.GetValueForArgument(ICommandSpec.FilesArgument);
      if (files != null) {
        foreach (var file in files) {
          dafnyOptions.AddFile(file.FullName);
        }
      }

      foreach (var option in command.Options) {
        var value = context.ParseResult.GetValueForOption(option);
        options.OptionArguments[option] = value;
        dafnyOptions.ApplyBinding(option);
      }

      dafnyOptions.ApplyDefaultOptionsWithoutSettingsDefault();
      commandSpec.PostProcess(dafnyOptions, options, context);
    }

    var builder = new CommandLineBuilder(rootCommand).UseDefaults();
    builder = AddDeveloperHelp(rootCommand, builder);

#pragma warning disable VSTHRD002
    var exitCode = builder.Build().InvokeAsync(arguments, console).Result;
#pragma warning restore VSTHRD002
    if (!wasInvoked) {
      if (exitCode == 0) {
        return new ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult.OK_EXIT_EARLY);
      }

      return new ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult.PREPROCESSING_ERROR);
    }
    if (exitCode == 0) {
      return new ParseArgumentSuccess(dafnyOptions);
    }

    return new ParseArgumentFailure(DafnyDriver.CommandLineArgumentsResult.PREPROCESSING_ERROR);
  }

  private static CommandLineBuilder AddDeveloperHelp(RootCommand rootCommand, CommandLineBuilder builder) {
    var languageDeveloperHelp = new Option<bool>(ToolchainDebuggingHelpName,
      "Show help and usage information, including options designed for developing the Dafny language and toolchain.");
    rootCommand.AddGlobalOption(languageDeveloperHelp);
    builder = builder.AddMiddleware(async (context, next) => {
      if (context.ParseResult.FindResultFor(languageDeveloperHelp) is { }) {
        context.InvocationResult = new HelpResult();
      } else {
        await next(context);
      }
    }, MiddlewareOrder.Configuration - 101);
    return builder;
  }
}

/// <summary>
/// The class HelpResult is internal to System.CommandLine so we have to include it as source.
/// It seems System.CommandLine didn't consider having more than one help option as a use-case.
/// </summary>
internal class HelpResult : IInvocationResult {
  public void Apply(InvocationContext context) {
    var output = context.Console.Out.CreateTextWriter();
    var helpBuilder = ((HelpBuilder)context.BindingContext.GetService(typeof(HelpBuilder)))!;
    var helpContext = new HelpContext(helpBuilder,
      context.ParseResult.CommandResult.Command,
      output,
      context.ParseResult);

    helpBuilder.Write(helpContext);
  }
}
