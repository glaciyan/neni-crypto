using System;
using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core.Header
{
    public static class VaultHeaderReader
    {
        public static VaultHeader ReadFrom(Stream source)
        {
            using var binReader = new BinaryReader(source, Encoding.Unicode, true);

            var magicNumber = binReader.ReadBytes(VaultHeader.MagicNumberLength);

            if (!magicNumber.ContentEqualTo(VaultHeader.MagicNumber))
                throw new FormatException("Magic number doesn't match");

            var result = new VaultHeader();

            var mpIV = binReader.ReadBytes(AesSizes.IV);
            var mpAuthentication = binReader.ReadBytes(AesSizes.Auth);
            var encryptedMp = binReader.ReadBytes(AesSizes.PaddedKey);

            result.MasterPassword = new MasterPassword(mpIV, mpAuthentication, encryptedMp);

            return result;
        }
    }
}