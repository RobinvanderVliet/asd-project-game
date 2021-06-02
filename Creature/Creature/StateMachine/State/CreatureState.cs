using Creature.Creature.StateMachine.Data;
using System;

namespace Creature.Creature.StateMachine.State
{
    public abstract class CreatureState : IComparable
    {
        protected ICreatureData _creatureData;
        
        public virtual void Do() 
        {
            throw new NotImplementedException();
        }

        public virtual void SetCreatureData(ICreatureData creatureData)
        {
            _creatureData = creatureData;
        }
        
        public int CompareTo(object obj)
        {
            throw new InvalidOperationException("State machine is not a comparable object.");
        }
    }
}