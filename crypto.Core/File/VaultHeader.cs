namespace crypto.Core.File
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

        public static VaultHeader Create()
        {
            return new VaultHeader(new MasterPassword());
        }

        public MasterPassword MasterPassword { get; set; }
    }
}