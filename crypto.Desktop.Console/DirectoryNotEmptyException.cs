using System;

namespace crypto.Desktop.Cnsl
{
    public class DirectoryNotEmptyException : Exception
    {
        public DirectoryNotEmptyException(string? message) : base(message)
        {
        }
    }
}