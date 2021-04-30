using System;

namespace Creature.Creature.StateMachine.Event
{
    public abstract class CreatureEvent : IComparable
    {
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
