using System;
using System.Collections.Generic;
using System.IO;

namespace crypto.Core.File
{
    public class VaultHeader
    {
        public static readonly byte[] MagicNumber = {0x6e, 0x76, 0x66};
        public static readonly int MagicNumberLength = MagicNumber.Length;
        
        public VaultHeader(string fullPath, MasterPassword masterPassword)
        {
            FullPath = fullPath;
            MasterPassword = masterPassword;
        }
        
        public static VaultHeader Create(string path, string name, byte[] stretchedKey)
        {
            return new VaultHeader(Path.GetFullPath(path + name), new MasterPassword());
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