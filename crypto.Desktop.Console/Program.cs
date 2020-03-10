using System;

namespace crypto.Desktop.Cnsl
{
    class Program
    {
        static void Main(string[] args)
        {
            // parse arguments
            try
            {
                CommandLineArgumentParser.ParseConfig(args).Run();
            }
            catch (ArgumentException e)
            {
                Notifier.Error($"Something went wrong: {e.Message}");
            }

            // new argument
            // open argument

            // app config
            // all classes should be extensible

            // encrypt/decrypt
        }
    }
}
