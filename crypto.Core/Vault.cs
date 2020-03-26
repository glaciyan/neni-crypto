﻿using System.Collections.Generic;

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

        public Dictionary<CipherFile, VaultItemInfo> CryptoFiles { get; } =
            new Dictionary<CipherFile, VaultItemInfo>();

        public string Name { get; set; }

        public void Add(CipherFile file, VaultItemInfo config)
        {
            CryptoFiles.Add(file, config);
        }
    }
}