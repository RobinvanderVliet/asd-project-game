using Creature.Creature.StateMachine.Data;
using System;

namespace Creature.Creature.StateMachine.State
{
    public abstract class CreatureState : IComparable
    {
        protected ICreatureData _creatureData;

        public CreatureState(ICreatureData creatureData)
        {
            _creatureData = creatureData;
        }

        public virtual void Do() 
        {
            
        }

        public virtual void Do(ICreatureData creatureData) 
        {
            
        }

        public int CompareTo(object obj)
        {
            throw new InvalidOperationException("State machine should not be compared to");
        }
    }
}