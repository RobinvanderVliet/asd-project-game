using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.Agent.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public sealed class SyntaxErrorException : Exception
    {
        public SyntaxErrorException(string message) : base(message)
        {

        }
    }
}