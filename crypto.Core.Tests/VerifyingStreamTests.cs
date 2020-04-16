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
        public async Task WriteFittingData()
        {
            await TestFile("gfx.cfg");
        }

        [Test]
        public async Task WriteBitData()
        {
            await TestFile("server.jar");
        }
        
        [Test]
        public async Task WriteOtherData()
        {
            await TestFile("test.cfg");
        }

        private static async Task TestFile(string fileName)
        {
            var serverFile = Path.Combine(Preparations.TestDataPath, fileName);
            await using var sourceFs = new FileStream(serverFile, FileMode.Open, FileAccess.Read);

            var dest = Path.Combine(Preparations.TestFolderPath, fileName);
            await using var destFs = new FileStream(dest, FileMode.Create, FileAccess.Write);

            await sourceFs.CopyToCreateHashAsync(destFs);
        }
    }
}