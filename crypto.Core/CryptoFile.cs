using System.IO;

namespace crypto.Core
{
    public class CryptoFile
    {
        public string FileName { get; }

        private FileInfo _fi;
        public FileInfo FileInfo
        {
            get => _fi ?? (_fi = new FileInfo(FileName));
            set => _fi = value;
        }

        public bool Exists => FileInfo.Exists;

        /// <summary>
        /// Constructs a new <see cref="CryptoFile"/>
        /// </summary>
        /// <param name="fileName">Path of the file</param>
        /// <param name="fileInfo">Optional FileInfo</param>
        /// <exception cref="IOException">Thrown when the file does not exists</exception>
        public CryptoFile(string fileName, FileInfo fileInfo = null)
        {
            FileName = fileName;
            _fi = fileInfo;
        }
    }
}