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
    
    public class VaultFileReaderWriter
    {
        private readonly VaultFile _underlying;
        private readonly byte[] _key;
        
        public VaultFileReaderWriter(VaultFile underlying, byte[] key)
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

        public static VaultFile ReadFrom(Stream source)
        {
            using var binReader = new BinaryReader(source, Encoding.Unicode, true);
            var result = new VaultFile();
            
            // info from plaintext
            var plainIV = binReader.ReadBytes(AesSizes.IV);
            var plainNameLength = binReader.ReadInt32();
            var encryptedPlainName = binReader.ReadBytes(plainNameLength);
            
            result.SecuredPlainName = new SecretFileName(encryptedPlainName, plainIV);

            result.TargetCipherIV = binReader.ReadBytes(AesSizes.IV);
            result.TargetAuthentication = binReader.ReadBytes(AesSizes.Auth);
            result.TargetPath = binReader.ReadString();
            
            var isUnlocked = binReader.ReadBoolean();

            if (isUnlocked)
            {
                var unlockedFilePathIV = binReader.ReadBytes(AesSizes.IV);
                var length = binReader.ReadInt32();
                var secretUnlockedFilePath = binReader.ReadBytes(length);
                
                result.UnlockedFilePath = new SecretFileName(secretUnlockedFilePath, unlockedFilePathIV);
            }

            return result;
        }
    }
}