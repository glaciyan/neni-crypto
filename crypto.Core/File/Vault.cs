using System.Collections.Generic;

namespace crypto.Core.File
{
    /// <summary>
    ///     Class for managing multiple CryptoFiles that pair with their respective config
    /// </summary>
    public class Vault
    {
        public string Name { get; set; }
        public List<VaultFile> VaultItems { get; set; } = new List<VaultFile>();
        
        public Vault(string name)
        {
            Name = name;
        }

        public void AddFile(string path)
        {
            VaultItems.Add();
        }

        public static Vault Create(string path, string name, byte[] key)
        {
            throw new System.NotImplementedException();
        }
    }
}