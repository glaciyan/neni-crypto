using System.Collections.Generic;
using crypto.Core.Deprecated;

namespace crypto.Core.File
{
    /// <summary>
    ///     Class for managing multiple CryptoFiles that pair with their respective config
    /// </summary>
    public class Vault
    {
        public string Name { get; set; }
        public List<?> VaultItems { get; set; } = new List<?>();
        
        public Vault(string name)
        {
            Name = name;
        }

        public void AddFile(string path)
        {
            VaultItems.Add(new VaultItemInfo(new PlainTextFile(path)));
        }

        public static Vault Create(string path, string name, byte[] key)
        {
            throw new System.NotImplementedException();
        }
    }
}