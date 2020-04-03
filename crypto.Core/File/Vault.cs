using System;
using System.Collections.Generic;

namespace crypto.Core.File
{
    /// <summary>
    ///     Class for managing multiple CryptoFiles that pair with their respective config
    /// </summary>
    public class Vault
    {
        public Vault(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public List<VaultFile> VaultItems { get; set; } = new List<VaultFile>();

        public void AddFile(string name, string parentFolderPath)
        {
            VaultItems.Add(VaultFile.Create(name, parentFolderPath));
        }

        public static Vault Create(string path, string name, byte[] key)
        {
            throw new NotImplementedException();
        }
    }
}