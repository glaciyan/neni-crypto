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

        public Dictionary<CryptoFile, VaultFile> CryptoFiles { get; } =
            new Dictionary<CryptoFile, VaultFile>();

        public string Name { get; set; }

        public void Add(CryptoFile file, VaultFile config)
        {
            CryptoFiles.Add(file, config);
        }
    }
}