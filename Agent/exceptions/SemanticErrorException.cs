using System;
using System.Diagnostics.CodeAnalysis;

namespace Agent.exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class SemanticErrorException : Exception
    {
        public SemanticErrorException(string message) : base(message)
        {
    
        }
    }
}