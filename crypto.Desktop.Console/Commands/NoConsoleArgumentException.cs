using System;

namespace crypto.Desktop.Cnsl.Commands
{
    public class NoConsoleArgumentException : Exception
    {
        public NoConsoleArgumentException(string? message) : base(message)
        {
        }
    }
}