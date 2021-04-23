using System;

namespace Creature.Creature
{
    public abstract class CreatureState : IComparable
    {
        Monster.Event StateName { get; }

        public virtual void Do() {}

        public virtual void Do(object argument) {}

        public int CompareTo(object? obj)
        {
            throw new InvalidOperationException("State machine should not be compared to");
        }
    }
}