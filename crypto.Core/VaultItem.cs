using System.IO;

namespace crypto.Core
{
    public class VaultItem
    {
        public FileInfo FileInfo { get; }

        public VaultItem(string name)
        {
            FileInfo = new FileInfo(name);
        }

        public string Name => FileInfo.Name;
        public string Path => FileInfo.FullName;
    }
}