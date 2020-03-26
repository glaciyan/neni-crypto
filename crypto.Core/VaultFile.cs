using System.IO;

namespace crypto.Core
{
    public class VaultFile
    {
        public VaultFile(string fileName, FileInfo partnerFileInfo, byte[] iv)
        {
            FileName = fileName;
            PartnerFileInfo = partnerFileInfo;
            IV = iv;
        }

        public string FileName { get; set; }
        public string DecryptedFileName { get; set; }
        public FileInfo PartnerFileInfo { get; set; }
        public byte[] IV { get; }
    }
}