using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Dafny.LanguageServer.IntegrationTest.Util; 

public class FileTestExtensions {
  public static async Task<FileStream> WaitForFileToUnlock(string fullPath, FileMode mode, FileAccess access, FileShare share) {
    for (int numTries = 0; numTries < 10; numTries++) {
      FileStream fs = null;
      try {
        fs = new FileStream(fullPath, mode, access, share);
        return fs;
      } catch (IOException) {
        if (fs != null) {
          await fs.DisposeAsync();
        }
        await Task.Delay(50);
      }
    }

    return null;
  }
}