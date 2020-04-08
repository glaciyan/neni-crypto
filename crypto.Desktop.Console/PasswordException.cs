using System;

namespace crypto.Desktop.Cnsl
{
    public class PasswordException : Exception
    {
        public PasswordException(string? message) : base(message)
        {
        }
    }
}