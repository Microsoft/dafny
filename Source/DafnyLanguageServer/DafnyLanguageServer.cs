﻿using System.Diagnostics;
using Microsoft.Dafny.LanguageServer.Handlers;
using Microsoft.Dafny.LanguageServer.Language;
using Microsoft.Dafny.LanguageServer.Workspace;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Server;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Dafny.LanguageServer {
  public static class DafnyLanguageServer {
    private static string DafnyVersion {
      get {
        var version = typeof(DafnyLanguageServer).Assembly.GetName().Version!;
        return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
      }
    }

    public static LanguageServerOptions WithDafnyLanguageServer(this LanguageServerOptions options, 
      IConfiguration configuration, CancellationTokenSource cancelLanguageServer) {
      return options
        .WithDafnyLanguage(configuration)
        .WithDafnyWorkspace(configuration)
        .WithDafnyHandlers()
        .OnInitialize((server, @params, token) => InitializeAsync(server, @params, token, cancelLanguageServer))
        .OnStarted(StartedAsync);
    }

    private static Task InitializeAsync(ILanguageServer server, InitializeParams request, CancellationToken cancelRequestToken, 
        CancellationTokenSource cancelLanguageServer) {
      var logger = server.GetRequiredService<ILogger<Program>>();
      logger.LogTrace("initializing service");
    
      // https://github.com/microsoft/language-server-protocol/blob/gh-pages/_specifications/specification-3-16.md?plain=1#L1713
      if (request.ProcessId >= 0)
      {
        try
        {
          var hostProcess = Process.GetProcessById((int)request.ProcessId)!;
          hostProcess.EnableRaisingEvents = true;
          hostProcess.Exited += (_, _) => Process.GetCurrentProcess().Kill(); //cancelLanguageServer.Cancel());
        }
        catch
        {
          // If the process dies before we get here then request shutdown immediately
          Process.GetCurrentProcess().Kill(); //;cancelLanguageServer.Cancel();
        }
      }
      
      return Task.CompletedTask;
    }

    private static Task StartedAsync(ILanguageServer server, CancellationToken cancellationToken) {
      // TODO this currently only sent to get rid of the "Server answer pending" of the VSCode plugin.
      server.SendNotification("serverStarted", DafnyVersion);
      server.SendNotification("dafnyLanguageServerVersionReceived", DafnyVersion);
      return Task.CompletedTask;
    }
  }
}
