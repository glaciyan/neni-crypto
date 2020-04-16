using System.IO;

namespace crypto.Core
{
    public class VaultPaths
    {
        public VaultPaths(string folderPath)
        {
            FullVaultFolderPath = Path.GetFullPath(folderPath);
            var fullPathDirInfo = new DirectoryInfo(FullVaultFolderPath);

            Name = fullPathDirInfo.Name;

            VaultFilePath = Vault.GetVaultFilePath(FullVaultFolderPath, Name);

            if (!File.Exists(VaultFilePath))
                throw new FileNotFoundException("Couldn't find vault file for path " + VaultFilePath);
        }

        public string Name { get; set; }
        public string FullVaultFolderPath { get; set; }
        public string VaultFilePath { get; set; }
    }
}