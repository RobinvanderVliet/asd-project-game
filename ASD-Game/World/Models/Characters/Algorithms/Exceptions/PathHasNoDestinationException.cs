using System;
using System.Diagnostics.CodeAnalysis;

namespace Character.Algorithms.Exceptions
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