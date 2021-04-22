using System;

namespace Agent.exceptions
{
    [Serializable]
    public class SemanticErrorException : Exception
    {
        public SemanticErrorException(string message) : base(message)
        {
    
        }
    }
}