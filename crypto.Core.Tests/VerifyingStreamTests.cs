using System.IO;
using System.Threading.Tasks;
using crypto.Core.Cryptography;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class VerifyingStreamTests
    {
        [Test]
        public async Task WriteBigData()
        {
            // set own file of 30MB+
            var serverFile = Path.Combine(Preparations.TestDataPath, "server.jar");
            await using var sourceFs = new FileStream(serverFile, FileMode.Open, FileAccess.Read);

            var dest = Path.Combine(Preparations.TestFolderPath, "server.jar");
            await using var destFs = new FileStream(dest, FileMode.Create, FileAccess.Write);

            await sourceFs.CopyToCreateHashAsync(destFs);
        }
    }
}