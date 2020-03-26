using System.Collections.Generic;

namespace crypto.Core
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

        public Dictionary<CipherFile, VaultFile> CryptoFiles { get; } =
            new Dictionary<CipherFile, VaultFile>();

        public string Name { get; set; }

        public void Add(CipherFile file, VaultFile config)
        {
            CryptoFiles.Add(file, config);
        }
    }
}