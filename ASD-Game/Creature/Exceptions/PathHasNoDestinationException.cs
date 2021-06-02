using System;
using System.Diagnostics.CodeAnalysis;

namespace Creature.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class PathHasNoDestinationException : System.Exception
    {
        public PathHasNoDestinationException() : base()
        {

        }
    }
}
