using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_project.Agent.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class SyntaxErrorException : Exception
    {
        public SyntaxErrorException(string message) : base(message)
        {

        }
    }
}