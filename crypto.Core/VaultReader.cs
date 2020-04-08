using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using crypto.Core.ExtensionUtilities;
using crypto.Core.Header;

namespace crypto.Core
{
    public static class VaultReader
    {
        public static Vault ReadFromConfig(string folderPath, byte[] key)
        {
            Debug.Assert(folderPath != null, nameof(folderPath) + " != null");

            var (fullFolderPath, folderName, vaultFilePath) = VerifyPathAndGetNames(folderPath);
            var result = new Vault(folderName) {VaultPath = fullFolderPath};

            using var vaultFile = new FileStream(vaultFilePath, FileMode.Open, FileAccess.Read);

            result.Header = VaultHeaderReader.ReadFrom(vaultFile);

            var (keyWasCorrect, password) = result.Header.MasterPassword.GetDecryptedPassword(key);
            key.Zeros();

            if (!keyWasCorrect) throw new CryptographicException("Password wasn't able to be verified");

            while (vaultFile.Position < vaultFile.Length)
                result.ItemHeaders.Add(ItemHeaderReader.ReadFrom(vaultFile, password));

            return result;
        }

        private static (string, string, string) VerifyPathAndGetNames(string path)
        {
            var fullPath = Path.GetFullPath(path);
            var folderName = Path.GetFileNameWithoutExtension(fullPath);
            var vaultFilePath = Vault.GetVaultFilePath(fullPath, folderName);

            if (File.Exists(vaultFilePath)) return (fullPath, folderName, vaultFilePath);

            throw new FileNotFoundException("Couldn't find vault file for path: " + path);
        }
    }
}