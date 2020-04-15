using System.IO;
using System.Text;

namespace crypto.Core.Header
{
    public class UserDataHeaderWriter
    {
        private readonly UserDataHeader _underlying;

        public UserDataHeaderWriter(UserDataHeader underlying)
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
        }
    }
}