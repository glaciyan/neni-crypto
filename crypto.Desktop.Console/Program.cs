using System;

namespace crypto.Desktop.Cnsl
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgumentParser.ParseConfig(args).Run();

            // parse arguments
            //try
            //{
            //    CommandLineArgumentParser.ParseConfig(args).Run();
            //}
            //catch (ArgumentException e)
            //{
            //    Notifier.Error($"Something went wrong: {e.Message}");
            //}
            // file not found execp

            // new argument
            // open argument

            // app config
            // all classes should be extensible

            // encrypt/decrypt
        }
    }
}
