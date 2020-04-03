using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace crypto.Core.File
{
    public enum ReaderWriter
    {
        Reader,
        Writer
    }
    
    public class VaultFileReaderWriter
    {
        private VaultFile _underlying;
        private ReaderWriter _mode;
        private byte[] _key;

        public VaultFileReaderWriter(VaultFile underlying, ReaderWriter mode, byte[] key)
        {
            _underlying = underlying;
            _mode = mode;
            _key = key;
        }

        public void WriteTo(Stream destination)
        {
            if (_mode != ReaderWriter.Writer) throw new InvalidOperationException("Not set to write");
            
            using var binWriter = new BinaryWriter(destination, Encoding.Unicode, true);
            
            binWriter.Write(_underlying.IsUnlocked);
            
            // info from plaintext
            binWriter.Write(_underlying.SecuredPlainName.IV);
            binWriter.Write(_underlying.SecuredPlainName.GetEncryptedName(_key));
            
            // when file is unlocked
            binWriter.Write(_underlying.IsUnlocked);
            
            // decrypted file
            binWriter.Write(_underlying.TargetCipherIV);
            binWriter.Write(_underlying.TargetAuthentication);
            binWriter.Write(_underlying.TargetPath);
            
            // write the path when unlocked
            if (_underlying.IsUnlocked)
            {
                binWriter.Write(_underlying.UnlockedFilePath);
            }
        }

        public void ReadFrom(Stream source)
        {
            if (_mode != ReaderWriter.Reader) throw new InvalidOperationException("Not set to read");
            
            using var binReader = new BinaryReader(source, Encoding.Unicode, true);
        }
    }
}