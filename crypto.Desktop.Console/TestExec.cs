using System;
using crypto.Core;

namespace crypto.Desktop.Cnsl
{
    // TODO: remove
    public class TestExec : IExecutionConfig
    {
        public CryptoFile Source { get; }
        public string Destination { get; }

        public TestExec(string source, string destination)
        {
            Source = new CryptoFile(source);
            Destination = destination;
        }

        public void Run()
        {
            Console.Write("Enter Password: ");
            var pw = Console.ReadLine();

            var keyring = KeyIVPair.FromPasswordString(pw);

            var cFConfig = new CryptoFileConfig(Source.FileName, Source.FileInfo, keyring.IV);

            CryptoFileWriter.WriteCryptoFile(Source, keyring, Destination);
        }
    }
}