using System;

namespace Agent.exceptions
{
    [Serializable]
    public class SyntaxErrorException : Exception
    {
        public SyntaxErrorException(string message) : base(message)
        {
    
        }
    }
}