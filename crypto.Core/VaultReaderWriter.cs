using System.IO;
using System.Security.Cryptography;
using crypto.Core.Header;

namespace crypto.Core
{
    public static class VaultReaderWriter
    {
        //++++++++++++++++++++++++++++
        //        Reading
        //++++++++++++++++++++++++++++

        public static Vault ReadFromConfig(VaultReadingPaths readingPaths, byte[] key)
        {
            var result = new Vault(readingPaths.Name, key) {VaultPath = readingPaths.FullVaultFolderPath};

            using var vaultFile = new FileStream(readingPaths.VaultFilePath, FileMode.Open, FileAccess.Read);

            result.Header = VaultHeaderReader.ReadFrom(vaultFile);

            var (keyWasCorrect, password) = result.Header.MasterPassword.GetDecryptedPassword(key);

            if (!keyWasCorrect) throw new CryptographicException("Password wasn't able to be verified");

            while (vaultFile.Position < vaultFile.Length)
                result.DataFiles.Add(new UserDataFile(UserDataHeaderReader.ReadFrom(vaultFile, password)));

            result.CheckAndCorrectAllItemHeaders();

            return result;
        }

        //++++++++++++++++++++++++++++
        //        Writing
        //++++++++++++++++++++++++++++

        public static void WriteConfig(Vault underlying, byte[] key)
        {
            using var fileStream = new FileStream(underlying.VaultFilePath, FileMode.Open);

            WriteHeader(fileStream, underlying, key);

            using var binWriter = new BinaryWriter(fileStream);

            foreach (var dataFile in underlying.DataFiles) WriteItemHeader(fileStream, underlying, dataFile.Header);

            underlying.Written = true;
        }

        private static void WriteHeader(Stream fileStream, Vault underlying, byte[] key)
        {
            var headerWriter = new VaultHeaderWriter(underlying.Header);
            headerWriter.WriteTo(fileStream, key);
        }

        private static void WriteItemHeader(Stream fileStream, Vault underlying, UserDataHeader userDataHeader)
        {
            var itemHeaderWriter = new UserDataHeaderWriter(userDataHeader);
            itemHeaderWriter.WriteTo(fileStream, underlying.Header.MasterPassword.Password);
        }
    }
}