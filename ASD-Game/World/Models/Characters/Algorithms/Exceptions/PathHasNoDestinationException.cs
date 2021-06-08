using System;
using System.Diagnostics.CodeAnalysis;

namespace Character.Exceptions
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
