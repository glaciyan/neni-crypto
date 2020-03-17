using crypto.Core;

namespace crypto.Desktop.Cnsl
{
    // TODO: remove
    public class TestExec : IExecutionConfig
    {
        public TestExec(string source, string destination)
        {
            Source = new CryptoFile(source);
            Destination = destination;
        }

        public CryptoFile Source { get; }
        public string Destination { get; }

        public void Run()
        {
            var pw = PasswordPromt.PromtPassword();

            var keyring = KeyIVPair.FromPasswordString(pw);

            var cFConfig = new CryptoFileConfig(Source.FileName, Source.FileInfo, keyring.IV);

            CryptoFileIO.Write(Source, keyring, Destination);
            //CryptoFileConfigIO.Write(cFConfig, Destination + ".ncg");

            CryptoFileIO.WriteDecrypted(Destination, keyring, Destination + "_dencrypted");
        }
    }
}