using System;
using System.Diagnostics.CodeAnalysis;

namespace World.Models.Character.Algorithms.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class PathHasNoDestinationException : Exception
    {
        public PathHasNoDestinationException() : base()
        {
        }
    }
}