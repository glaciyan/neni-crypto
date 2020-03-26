using System.IO;

namespace crypto.Core
{
    public class CipherFile
    {
        private FileInfo _fi;
        private string _name;

        public CipherFile(string fileName, FileInfo fileInfo = null)
        {
            FileName = fileName;
            _fi = fileInfo;
        }

        public string FileName { get; }

        public FileInfo FileInfo
        {
            get => _fi ??= new FileInfo(FileName);
            set => _fi = value;
        }

        public bool Exists => FileInfo.Exists;

        public string Name
        {
            get
            {
                _name ??= Exists ? Path.GetFileName(FileName) : throw new FileNotFoundException();
                return _name;
            }
            private set => _name = value;
        }
    }
}