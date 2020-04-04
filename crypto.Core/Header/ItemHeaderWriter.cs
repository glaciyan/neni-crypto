using System.IO;
using System.Text;

namespace crypto.Core.File
{
    public class ItemHeaderWriter
    {
        private readonly ItemHeader _underlying;

        public ItemHeaderWriter(ItemHeader underlying)
        {
            _underlying = underlying;
        }

        public void WriteTo(Stream destination, byte[] key)
        {
            using var binWriter = new BinaryWriter(destination, Encoding.Unicode, true);

            // info from plaintext
            binWriter.Write(_underlying.SecuredPlainName.IV);
            // secret decrypted name
            var encryptedName = _underlying.SecuredPlainName.GetEncryptedName(key);
            binWriter.Write(encryptedName.Length);
            binWriter.Write(encryptedName);

            binWriter.Write(_underlying.TargetCipherIV);
            binWriter.Write(_underlying.TargetAuthentication);
            binWriter.Write(_underlying.TargetPath);

            binWriter.Write(_underlying.IsUnlocked);

            // write the path when unlocked
            if (_underlying.IsUnlocked)
            {
                binWriter.Write(_underlying.UnlockedFilePath.IV);

                var secretUnlockedFilePath = _underlying.UnlockedFilePath.GetEncryptedName(key);
                binWriter.Write(secretUnlockedFilePath.Length);
                binWriter.Write(secretUnlockedFilePath);
            }
        }
    }
}