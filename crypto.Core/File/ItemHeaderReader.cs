using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core.File
{
    public class ItemHeaderReader
    {
        public static ItemHeader ReadFrom(Stream source)
        {
            using var binReader = new BinaryReader(source, Encoding.Unicode, true);
            var result = new ItemHeader();
            
            // info from plaintext
            var plainIV = binReader.ReadBytes(AesSizes.IV);
            var plainNameLength = binReader.ReadInt32();
            var encryptedPlainName = binReader.ReadBytes(plainNameLength);
            
            result.SecuredPlainName = new SecretFileName(encryptedPlainName, plainIV);

            result.TargetCipherIV = binReader.ReadBytes(AesSizes.IV);
            result.TargetAuthentication = binReader.ReadBytes(AesSizes.Auth);
            result.TargetPath = binReader.ReadString();
            
            var isUnlocked = binReader.ReadBoolean();

            if (isUnlocked)
            {
                var unlockedFilePathIV = binReader.ReadBytes(AesSizes.IV);
                var length = binReader.ReadInt32();
                var secretUnlockedFilePath = binReader.ReadBytes(length);
                
                result.UnlockedFilePath = new SecretFileName(secretUnlockedFilePath, unlockedFilePathIV);
            }

            return result;
        }
    }
}