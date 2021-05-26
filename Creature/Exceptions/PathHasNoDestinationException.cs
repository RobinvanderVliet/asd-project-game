using System;
using System.Diagnostics.CodeAnalysis;

namespace Creature.Exceptions
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
