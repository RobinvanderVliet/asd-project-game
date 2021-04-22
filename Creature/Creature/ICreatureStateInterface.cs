using System;

namespace Creature.Creature
{
    public abstract class ICreatureStateInterface : IComparable
    {
        Monster.Event StateName { get; }

        public virtual void Do()
        {

        }
        public int CompareTo(object? obj)
        {
            throw new InvalidOperationException("State machine should not be compared to");
        }
    }
}