using System;
using System.Text;
using NUnit.Framework;

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

            Console.WriteLine($"Original data: {ByteRepres(byteData)}\n" +
                              $"Encrypted Data: {ByteRepres(encryptedData)}");

            Assert.AreNotEqual(encryptedData, byteData);

            var decryptedData = SimpleCryptography.DecryptBytes(encryptedData, kiPair.Key, kiPair.IV);

            Assert.AreEqual(decryptedData, byteData);

            Console.WriteLine($"Original data: {ByteRepres(byteData)}\n" +
                              $"Decrypted Data: {ByteRepres(decryptedData)}");
        }

        private string ByteRepres(byte[] b)
        {
            return $"({b.Length}){BitConverter.ToString(b)}";
        }
    }
}