using System.Collections.Generic;

namespace crypto.Core
{
    /// <summary>
    ///     Class for managing multiple CryptoFiles that pair with their respective config
    /// </summary>
    public class Vault
    {
        public string Name { get; set; }
        public List<VaultItemInfo> VaultItems { get; set; } = new List<VaultItemInfo>();
        
        public Vault(string name)
        {
            Name = name;
        }

        public void AddFile(string path)
        {
            VaultItems.Add(new VaultItemInfo(new PlainTextFile(path)));
        }
    }
}