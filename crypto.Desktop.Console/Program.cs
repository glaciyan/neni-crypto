﻿using System;
using Serilog;

namespace crypto.Desktop.Cnsl
{
    class Program
    {
        static void Main(string[] args)
        {
            // logger setup
#if DEBUG
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
#endif

            //parse arguments
            try
            {
                CommandLineArgumentParser.ParseConfig(args).Run();
            }
            catch (Exception e)
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
