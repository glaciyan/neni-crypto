using System.IO;
using System.Text;

namespace crypto.Core
{
    public static class CryptoFileConfigIO
    {
        public static void Write(CryptoFileConfig cfg, string destination)
        {
            using var configStream = new FileStream(destination, FileMode.Create);

            configStream.Write(cfg.IV);

            // add encryption
            var ranLFileContent = new RandomLengthFileContent(Encoding.Unicode.GetBytes(cfg.FileName));
            ranLFileContent.WriteTo(configStream);
        }
    }
}