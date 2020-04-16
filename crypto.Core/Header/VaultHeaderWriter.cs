using System.IO;
using System.Text;

namespace crypto.Core.Header
{
    public class VaultHeaderWriter
    {
        // magic number
        // PasswordIV
        // Password authentication
        // Encrypted master password

        private readonly VaultHeader _underlying;

        public VaultHeaderWriter(VaultHeader underlying)
        {
            _underlying = underlying;
        }

        public void WriteTo(Stream destination, byte[] key)
        {
            using var binWriter = new BinaryWriter(destination, Encoding.Unicode, true);

            binWriter.Write(VaultHeader.MagicNumber);

            binWriter.Write(_underlying.MasterPassword.AuthenticationHash);
            binWriter.Write(_underlying.MasterPassword.GetEncryptedPassword(key));
        }
    }
}