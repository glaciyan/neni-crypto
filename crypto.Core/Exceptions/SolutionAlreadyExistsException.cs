using System;

namespace crypto.Core.Exceptions
{
    public class SolutionAlreadyExistsException : Exception
    {
        public SolutionAlreadyExistsException(string message) : base(message)
        {
        }
    }
}