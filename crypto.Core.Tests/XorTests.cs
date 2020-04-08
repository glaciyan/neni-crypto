using crypto.Core.Cryptography;
using crypto.Core.ExtensionUtilities;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class XorTests
    {
        [Test]
        public void TestWithKey()
        {
            var key = CryptoRNG.GetRandomBytes(32);
            var data = CryptoRNG.GetRandomBytes(32);

            var encryptedData = key.Xor(data);
            var decryptedData = key.Xor(encryptedData);

            Assert.AreEqual(data, decryptedData);
        }
    }
}