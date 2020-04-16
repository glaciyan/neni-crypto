using System;

namespace crypto.Core.Exceptions
{
    public class NotANameException : Exception
    {
        public NotANameException(string message) : base(message)
        {
        }
    }
}