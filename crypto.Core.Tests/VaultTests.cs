using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class VaultTests
    {
        private string testFolderPath;
        private const string FilePath = "../test/mock/testFile.txt";

        [OneTimeSetUp]
        public void Set_Up()
        {
            testFolderPath = "../testData/";
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
        public async Task Create_CryptoConfig()
        {
            _
        }
    }
}