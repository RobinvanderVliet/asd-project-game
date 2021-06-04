using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_project.Agent.Exceptions
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