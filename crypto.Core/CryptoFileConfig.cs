using System.IO;

namespace crypto.Core
{
    public class CryptoFileConfig
    {
        public CryptoFileConfig(string fileName, FileInfo partnerFileInfo, byte[] iv)
        {
            FileName = fileName;
            PartnerFileInfo = partnerFileInfo;
            IV = iv;
        }

        public string FileName { get; set; }
        public FileInfo PartnerFileInfo { get; set; }
        public byte[] IV { get; }
    }
}