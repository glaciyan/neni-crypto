using System.IO;

namespace crypto.Core
{
    public class VaultItem
    {
        public FileInfo FileInfo { get; }

        public VaultItem(string name)
        {
            Name = name;
            FileInfo = new FileInfo(name);
        }

        public string Name { get; }
        public string FileName => FileInfo.Name;
        public string Path => FileInfo.FullName;
    }
}