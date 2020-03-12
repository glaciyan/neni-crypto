using System.IO;

namespace crypto.Core
{
    public class CryptoFileConfig
    {
        public string FileName { get; set; }
        public FileInfo PartnerFileInfo { get; set; }

        private readonly byte[] _iv;

        public CryptoFileConfig(string fileName, FileInfo partnerFileInfo, byte[] iv)
        {
            FileName = fileName;
            PartnerFileInfo = partnerFileInfo;
            _iv = iv;
        }
    }
}