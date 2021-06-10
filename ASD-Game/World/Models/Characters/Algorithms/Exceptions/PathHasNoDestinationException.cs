using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.World.Models.Characters.Algorithms.Exceptions
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