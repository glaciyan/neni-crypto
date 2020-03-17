using System.IO;
using System.Text;

namespace crypto.Core
{
    public static class CryptoFileConfigIO
    {
        public static void Write(CryptoFileConfig cfg, string destination)
        {
            using var configStream = new FileStream(destination, FileMode.Create);


            // save last time edited

            // add encryption
            var byteCryptoKeyRing = new KeyIVPair();

            var encryptedFileArray = SimpleCryptography.EncryptBytes(
                Encoding.Unicode.GetBytes(cfg.FileName), byteCryptoKeyRing.Key, byteCryptoKeyRing.IV);

            var ranLFileContent = new RandomLengthFileContent(encryptedFileArray);

            // 16 bytes
            configStream.Write(cfg.IV);

            // 32 bytes
            configStream.Write(byteCryptoKeyRing.Key);
            // 16 bytes
            configStream.Write(byteCryptoKeyRing.IV);

            // random length
            ranLFileContent.WriteTo(configStream);
        }
    }
}