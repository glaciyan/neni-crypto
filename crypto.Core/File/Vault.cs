using System;
using System.Collections.Generic;
using System.IO;

namespace crypto.Core.File
{
    public class Vault
    {
        public Vault(string fullPath, MasterPassword masterPassword)
        {
            FullPath = fullPath;
            MasterPassword = masterPassword;
        }
        
        public static Vault Create(string path, string name, byte[] stretchedKey)
        {
            return new Vault(Path.GetFullPath(path + name), new MasterPassword());
        }

        // file and parent folder name
        public string FullPath { get; }

        public MasterPassword MasterPassword { get; set; }
        
        public List<ItemHeader> VaultItems { get; } = new List<ItemHeader>();

        public void AddHeader(string name, string parentFolderPath)
        {
            VaultItems.Add(ItemHeader.Create(name, parentFolderPath));
        }
    }
}