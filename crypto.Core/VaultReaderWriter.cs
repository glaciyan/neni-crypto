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

        public static Vault ReadFromConfig(VaultPaths paths, byte[] key)
        {
            var result = new Vault(paths.Name, key) {VaultPath = paths.FullVaultFolderPath};

            using var vaultFile = new FileStream(paths.VaultFilePath, FileMode.Open, FileAccess.Read);

            result.Header = VaultHeaderReader.ReadFrom(vaultFile);

            var (keyWasCorrect, password) = result.Header.MasterPassword.GetDecryptedPassword(key);

            if (!keyWasCorrect) throw new CryptographicException("Password wasn't able to be verified");

            while (vaultFile.Position < vaultFile.Length)
                result.UserDataFiles.Add(new UserDataFile(UserDataHeaderReader.ReadFrom(vaultFile, password)));

            result.CheckAndCorrectAllItemHeaders();
            // TODO: check if cipher files exists, if not remove that file

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

            foreach (var dataFile in underlying.UserDataFiles) WriteItemHeader(fileStream, underlying, dataFile.Header);

            fileStream.SetLength(fileStream.Position);
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