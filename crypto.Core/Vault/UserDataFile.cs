using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using crypto.Core.Cryptography;

namespace crypto.Core.Vault
{
    public class UserDataFile
    {
        public static async Task<byte[]> ExtractUserDataFile(string sourcePath, string destinationPath, byte[] key,
            byte[] iv)
        {
            await using var src = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            await using var dest = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);

            using var decryptor = QuickAesTransform.CreateDecryptor(key, iv);
            await using var srcCrypto = new CryptoStream(src, decryptor, CryptoStreamMode.Read);

            var hash = await srcCrypto.CopyToCreateHashAsync(dest);
            return hash;
        }

        public static async Task<byte[]> WriteUserDataFile(string sourcePath, string destinationPath, byte[] key,
            byte[] iv)
        {
            await using var src = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            await using var dest = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);

            using var encryptor = QuickAesTransform.CreateEncryptor(key, iv);
            await using var destCrypto = new CryptoStream(dest, encryptor, CryptoStreamMode.Write);

            var hash = await src.CopyToCreateHashAsync(destCrypto);
            return hash;
        }
    }
}