using System;
using System.Text;
using crypto.Core.Cryptography;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class DataGetsEncryptedDecryptedCorrectly
    {
        [Test]
        public void LongString()
        {
            // ReSharper disable StringLiteralTypo
            const string data =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent in mauris justo. In ut lacinia enim. Phasellus eu porta nunc. Mauris finibus dui at nulla mattis, vel consequat odio posuere. Proin pretium urna et orci vulputate, ac rutrum urna iaculis. Vestibulum sodales lobortis mollis. Sed fringilla mauris sed nisi imperdiet iaculis.";
            Data_Gets_Encrypted_Decrypted_Correctly(data);
        }

        [Test]
        public void ShortString()
        {
            const string data = "Mock";
            Data_Gets_Encrypted_Decrypted_Correctly(data);
        }

        [Test]
        public void TwoBlockString()
        {
            const string data = "12345678901234567890123123123123";
            Data_Gets_Encrypted_Decrypted_Correctly(data);
        }

        private void Data_Gets_Encrypted_Decrypted_Correctly(string data)
        {
            var byteData = Encoding.ASCII.GetBytes(data);

            var kiPair = new KeyIVPair();

            using var crypto = new AesQuickCrypto(kiPair);

            var encryptedData = crypto.EncryptBytes(byteData);

            Console.WriteLine($"Original data: {ByteRepres(byteData)}\n" +
                              $"Encrypted Data: {ByteRepres(encryptedData)}");

            Assert.AreNotEqual(encryptedData, byteData);

            var decryptedData = crypto.DecryptBytes(encryptedData);

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