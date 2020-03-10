using System;
using crypto.Core;

namespace crypto.Desktop.Cnsl
{
    public class TestExec : IExecutionConfig
    {
        public const string Keyword = "test";

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

            CryptoFileWriter.WriteCryptoFile(Source, keyring, Destination);
        }
    }
}