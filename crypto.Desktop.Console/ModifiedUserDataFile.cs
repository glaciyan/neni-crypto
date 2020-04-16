using crypto.Core;

namespace crypto.Desktop.Cnsl
{
    public class ModifiedUserDataFile
    {
        public UserDataFile UserDataFile { get; }
        public string UnlockedFilePath { get; }
        public string EncryptedFilePath { get; }

        public ModifiedUserDataFile(UserDataFile userDataFile, string unlockedFilePath, string encryptedFilePath)
        {
            UserDataFile = userDataFile;
            UnlockedFilePath = unlockedFilePath;
            EncryptedFilePath = encryptedFilePath;
        }
    }
}