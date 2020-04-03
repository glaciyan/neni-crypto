using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core.File
{
    public enum ReaderWriter
    {
        Reader,
        Writer
    }
    
    public class VaultFileWriter
    {
        private readonly VaultFile _underlying;
        private readonly byte[] _key;
        
        public VaultFileWriter(VaultFile underlying, byte[] key)
        {
            _underlying = underlying;
            _key = key;
        }
        
        public void WriteTo(Stream destination)
        {
            using var binWriter = new BinaryWriter(destination, Encoding.Unicode, true);
            
            // info from plaintext
            binWriter.Write(_underlying.SecuredPlainName.IV);
            // secret decrypted name
            var encryptedName = _underlying.SecuredPlainName.GetEncryptedName(_key);
            binWriter.Write(encryptedName.Length);
            binWriter.Write(encryptedName);
            
            binWriter.Write(_underlying.TargetCipherIV);
            binWriter.Write(_underlying.TargetAuthentication);
            binWriter.Write(_underlying.TargetPath);
            
            binWriter.Write(_underlying.IsUnlocked);

            // write the path when unlocked
            if (_underlying.IsUnlocked)
            {
                binWriter.Write(_underlying.UnlockedFilePath.IV);

                var secretUnlockedFilePath = _underlying.UnlockedFilePath.GetEncryptedName(_key);
                binWriter.Write(secretUnlockedFilePath.Length);
                binWriter.Write(secretUnlockedFilePath);
            }
        }
    }
}