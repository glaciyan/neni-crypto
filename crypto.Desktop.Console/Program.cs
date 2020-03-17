using Serilog;

namespace crypto.Desktop.Cnsl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // logger setup
#if DEBUG
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
#endif

            //parse arguments
            //try
            //{
            CommandLineArgumentParser.ParseConfig(args).Run();
            //}
            //catch (Exception e)
            //{
            //    Log.Error(e.ToString());
            //    Notifier.Error($"Something went wrong: {e.Message}");
            //}

            // new argument
            // open argument

            // app config
            // all classes should be extensible

            // encrypt/decrypt
        }
    }
}