﻿using System;

namespace crypto.Desktop.Cnsl
{
    public static class CommandLineArgumentParser
    {
        public static Command ParseConfig(string[] args)
        {
            if (args.Length == 0)
                throw new NoConsoleArgumentException();

            var arguments = new ArrayEnumerator<string>(args);

            return arguments.NextOrNull() switch
            {
                // create new project
                "new" => new NewCommand(arguments.NextOrNull(), arguments.NextOrNull()),

                _ => throw new ArgumentException("Argument was not recognized")
            };
        }
    }
}