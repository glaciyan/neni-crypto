using System.IO;
using System.Threading.Tasks;
using crypto.Core.Cryptography;
using crypto.Core.File;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class VaultTests
    {
        private const string testFolderPath = "../testData/";
        private const string FilePath = "../test/mock/testFile.txt";

        [OneTimeSetUp]
        public void Set_Up()
        {
            
            Directory.CreateDirectory(testFolderPath);
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
            const string targetFile = testFolderPath + "CreateCryptoConfigNoPrefixPath.td";
            const string mockFileName = "importantData.txt";

            var key = CryptoRNG.GetRandomBytes(AesSizes.Key);

            var vaultFile = VaultFile.Create(mockFileName);
            var writer = new VaultFileWriter(vaultFile, key);

            using (var stream = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                writer.WriteTo(stream);
            }

            VaultFile readVaultFile;

            using (var stream = new FileStream(targetFile, FileMode.Open, FileAccess.Read))
            {
                readVaultFile = VaultFileReader.ReadFrom(stream);
            }
            
            Assert.AreEqual(vaultFile.TargetPath, readVaultFile.TargetPath);
            Assert.AreEqual(vaultFile.TargetCipherIV, readVaultFile.TargetCipherIV);
            Assert.AreEqual(vaultFile.TargetAuthentication, readVaultFile.TargetAuthentication);
            
            Assert.AreEqual(vaultFile.IsUnlocked, readVaultFile.IsUnlocked);
            Assert.AreEqual(vaultFile.SecuredPlainName.GetName(), readVaultFile.SecuredPlainName.GetName(key));
        }
    }
}