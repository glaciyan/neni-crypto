﻿using System.IO;
using System.Text;

namespace crypto.Core
{
    public static class CryptoFileConfigWriter
    {
        public static void WriteCryptoFileConfig(CryptoFileConfig cfg, string destination)
        {
            using var configStream = new FileStream(destination, FileMode.CreateNew);

            configStream.Write(cfg.IV);

            // add encryption
            var ranLFileContent = new RandomLengthFileContent(Encoding.Unicode.GetBytes(cfg.FileName));
            ranLFileContent.WriteTo(configStream);
        }
    }
}