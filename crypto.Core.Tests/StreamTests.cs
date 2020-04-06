using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using crypto.Core.Cryptography;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class StreamTests
    {
        private const string TestFile = "../../../../testdata/data2.dat";
        
        [Test]
        public async Task HashIsCorrect()
        {
            var timer = Stopwatch.StartNew();

            await using var sourceStream = new FileStream(TestFile, FileMode.Open, FileAccess.Read);
            await using var outputStream = new FileStream("HashIsCorrect.td", FileMode.Create, FileAccess.Write);
            
            using var sha = SHA256.Create();

            var hashFromCompute = sha.ComputeHash(sourceStream);
            sourceStream.Position = 0;
            var hashFromCopy = await sourceStream.CopyToCreateHashAsync(outputStream);

            Console.WriteLine(timer.ElapsedMilliseconds + "ms");
            
            Assert.AreEqual(hashFromCompute, hashFromCopy);
        }
    }
}