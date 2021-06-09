using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.Agent.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public sealed class SemanticErrorException : Exception
    {
        public SemanticErrorException(string message) : base(message)
        {

        }
    }
}