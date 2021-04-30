using System;

namespace Creature.Creature.StateMachine.State
{
    public abstract class CreatureState : IComparable
    {
        public virtual void Do() {
            throw new NotImplementedException();
        }

        public virtual void Do(object argument) {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new InvalidOperationException("State machine should not be compared to");
        }
    }
}