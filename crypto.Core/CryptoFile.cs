using System.IO;

namespace crypto.Core
{
    public class CryptoFile
    {
        public string FileName { get; }

        private FileInfo _fi;
        public FileInfo FileInfo
        {
            get => _fi ??= new FileInfo(FileName);
            set => _fi = value;
        }

        public bool Exists => FileInfo.Exists;

        public CryptoFile(string fileName, FileInfo fileInfo = null)
        {
            FileName = fileName;
            _fi = fileInfo;
        }
    }
}