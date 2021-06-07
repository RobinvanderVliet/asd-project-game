using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.Creature.Exceptions
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
