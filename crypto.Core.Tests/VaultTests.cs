using System.IO;
using crypto.Core.Cryptography;
using crypto.Core.Header;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class VaultTests
    {
        private const string TestFolderPath = "../testData/";
        private const string FilePath = "../test/mock/testFile.txt";
        private const string TestFile = "../../../testdata/data.dat";

        [OneTimeSetUp]
        public void Set_Up()
        {
            Directory.CreateDirectory(TestFolderPath);
        }

        // [Test]
        // public void Writing_Reading_Vault_File_Info()
        // {
        //     const string password = "passphrase";
        //     var key = password.ToByteArraySHA256();
        //     
        //     var vaultFileInfo = new VaultItemInfo(new PlainTextFile(FilePath));
        //     var writer = new VaultItemInfoIO(new VaultItemInfoParser(vaultFileInfo));
        //     
        //     writer.Write(testFolderPath, key);
        //
        //     Console.WriteLine($"Wrote to: {testFolderPath + vaultFileInfo.CipherTextFileName}");
        //     
        //     var cipherVaultItemInfo = new VaultItemInfo(new CipherTextFile(testFolderPath + vaultFileInfo.CipherTextFileName));
        //     var newVaultFileInfoReader = new VaultItemInfoIO(new VaultItemInfoParser(cipherVaultItemInfo));
        //     newVaultFileInfoReader.Read(key);
        //
        //     Assert.AreEqual(vaultFileInfo.PlainTextName, cipherVaultItemInfo.PlainTextName);
        //     Assert.AreEqual(vaultFileInfo.CipherIv, cipherVaultItemInfo.CipherIv);
        //     Assert.AreEqual(vaultFileInfo.IsDecryptedInVault, cipherVaultItemInfo.IsDecryptedInVault);
        //
        //     Console.WriteLine("Plain name: " + cipherVaultItemInfo.PlainTextName);
        // }

        // [Test]
        // public void Adding_Files_To_New_Vault()
        // {
        //     var key = "passphrase".StretchKey();
        //     var testingVault = Vault.Create("../testData/", "TestVault", key);
        //     
        //     testingVault.AddFile(TODO, TODO);
        // }

        [Test]
        public void CreateCryptoConfigNoPrefixPath()
        {
            const string targetFile = TestFolderPath + "CreateCryptoConfigNoPrefixPath.td";
            const string mockFileName = "importantData.txt";

            var key = CryptoRNG.GetRandomBytes(AesSizes.Key);

            var vaultFile = ItemHeader.Create(mockFileName);
            var writer = new ItemHeaderWriter(vaultFile);

            using (var stream = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                writer.WriteTo(stream, key);
            }

            ItemHeader readItemHeader;

            using (var stream = new FileStream(targetFile, FileMode.Open, FileAccess.Read))
            {
                readItemHeader = ItemHeaderReader.ReadFrom(stream, key);
            }

            Assert.AreEqual(vaultFile.TargetPath, readItemHeader.TargetPath);
            Assert.AreEqual(vaultFile.TargetCipherIV, readItemHeader.TargetCipherIV);
            Assert.AreEqual(vaultFile.TargetAuthentication, readItemHeader.TargetAuthentication);

            Assert.AreEqual(vaultFile.IsUnlocked, readItemHeader.IsUnlocked);
            Assert.AreEqual(vaultFile.SecuredPlainName.PlainName, readItemHeader.SecuredPlainName.PlainName);
        }

        [Test]
        public void WriterReaderVaultHeader()
        {
            const string targetPath = TestFolderPath + "WriterReaderVaultHeader.td";
            var key = CryptoRNG.GetRandomBytes(AesSizes.Key);

            var header = VaultHeader.Create();

            using (var stream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
            {
                var writer = new VaultHeaderWriter(header);
                writer.WriteTo(stream, key);
            }

            VaultHeader readHeader;
            using (var stream = new FileStream(targetPath, FileMode.Open, FileAccess.Read))
            {
                readHeader = VaultHeaderReader.ReadFrom(stream);
            }

            Assert.IsTrue(readHeader.MasterPassword.GetDecryptedPassword(key).Item1);
        }

        [Test]
        public void VaultItemHeadersFileWriteRead()
        {
            var key = "passphrase".ApplySHA256();
            using var file = VaultItemHeadersFile.Create("TestVault", key);
            
            file.AddFile(TestFile);
            
            file.WriteTo(TestFolderPath);

            using var readFile = VaultItemHeadersFile.ReadFrom(TestFolderPath + "TestVault.vlt", key);
        }
    }
}