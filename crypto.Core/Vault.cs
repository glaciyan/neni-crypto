using System.Collections.Generic;

namespace crypto.Core
{
    /// <summary>
    /// Class for managing multiple CryptoFiles that pair with their respective config
    /// </summary>
    public class Vault
    {
        public Dictionary<CryptoFile, CryptoFileConfig> CryptoFiles { get; } = new Dictionary<CryptoFile, CryptoFileConfig>();
        public string Name { get; set; }

        public Vault(string name)
        {
            Name = name;
        }

        public void Add(CryptoFile file, CryptoFileConfig config)
        {
            CryptoFiles.Add(file, config);
        }
    }
}