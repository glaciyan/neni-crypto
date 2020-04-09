using System;

namespace crypto.Core.Exceptions
{
    public class FileAlreadyExistsException : Exception
    {
        public FileAlreadyExistsException(string message) : base(message)
        {
        }
    }
}