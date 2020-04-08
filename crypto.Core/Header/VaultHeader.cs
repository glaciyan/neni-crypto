using crypto.Core.Cryptography;

namespace crypto.Core.Header
{
    public class VaultHeader
    {
        public static readonly byte[] MagicNumber = {0x6e, 0x76, 0x66};
        public static readonly int MagicNumberLength = MagicNumber.Length;

        private VaultHeader(MasterPassword masterPassword)
        {
            MasterPassword = masterPassword;
        }

        public VaultHeader()
        {
        }

        public MasterPassword MasterPassword { get; set; }

        public static VaultHeader Create()
        {
            return new VaultHeader(new MasterPassword());
        }
    }
}