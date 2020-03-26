namespace crypto.Core
{
    public class CipherTextFile : VaultItem
    {
        public const int NameLength = 16;
        
        public CipherTextFile(string name) : base(System.IO.Path.GetFullPath(name))
        {
        }
    }
}