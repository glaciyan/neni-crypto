using System;

namespace crypto.Desktop.Cnsl
{
    public static class CommandLineArgumentParser
    {
        public static IExecutionConfig ParseConfig(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("No argument given");

            return args[0] switch
            {
                NewProject.ArgumentKeyword =>
                new NewProject(args.TryGetString(1), args.TryGetString(2)),
                _ => throw new ArgumentException("Argument was not recognized")
            };
        }
    }
}