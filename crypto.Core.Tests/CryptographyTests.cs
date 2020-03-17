using System;
using System.Text;
using NUnit.Framework;
using crypto.Core;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class CryptographyTests
    {
        [Test]
        public void DecryptedDataGetsReturnedCorrectly()
        {
            var data = "Hello this is a test string";
            var byteData = Encoding.ASCII.GetBytes(data);

            var kiPair = new KeyIVPair();

            var encryptedData = SimpleCryptography.EncryptBytes(byteData, kiPair.Key, kiPair.IV);

            Console.WriteLine($"Original data: {BitConverter.ToString(byteData)}\n" +
                              $"Encrypted Data: {BitConverter.ToString(encryptedData)}");

            Assert.AreNotEqual(encryptedData, byteData);

            var decryptedData = SimpleCryptography.DecryptBytes(encryptedData, kiPair.Key, kiPair.IV);

            Assert.AreEqual(decryptedData, byteData);
        }
    }
}