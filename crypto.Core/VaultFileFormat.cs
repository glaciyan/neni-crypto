using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core
{
    public static class VaultFileFormat
    {
        public static void Write(VaultFile cfg, string destination)
        {
            using var configStream = new FileStream(destination, FileMode.Create);


            // save last time edited

            // add encryption
            var byteCryptoKeyRing = new KeyIVPair();

            using var encrypt = new AesByteCryptography(byteCryptoKeyRing);
            
            var encryptedFileArray = encrypt.EncryptBytes(Encoding.Unicode.GetBytes(cfg.FileName));

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