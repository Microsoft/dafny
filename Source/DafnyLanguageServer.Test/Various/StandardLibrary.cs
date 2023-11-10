using System.IO;
using System.Threading.Tasks;
using Microsoft.Dafny.LanguageServer.IntegrationTest.Util;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Dafny.LanguageServer.IntegrationTest.Various;

public class StandardLibrary : ClientBasedLanguageServerTest {
  [Fact]
  public async Task CanUseWrappers() {
    var source = @"
import opened DafnyStdLibs.Wrappers;

method Foo() returns (s: Option<int>) { 
  return Some(3); 
}".TrimStart();

    var projectSource = @"
[options]
standard-libraries = true";
    var directory = Path.GetTempFileName();
    CreateAndOpenTestDocument(projectSource, Path.Combine(directory, DafnyProject.FileName));
    CreateAndOpenTestDocument(source, Path.Combine(directory, "document"));
    await AssertNoDiagnosticsAreComing(CancellationToken);
  }

  public StandardLibrary(ITestOutputHelper output, LogLevel dafnyLogLevel = LogLevel.Information) : base(output, dafnyLogLevel) {
  }
}