using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_project.Creature.Exceptions
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
