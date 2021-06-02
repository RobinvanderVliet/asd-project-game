using System;
using Creature.Creature.StateMachine.Data;

namespace Creature.Creature.StateMachine.State
{
    public class IdleState : CreatureState
    {
        public IdleState(ICreatureData creatureData)
        {
            _creatureData = creatureData;
        }

        public void Do()
        {
            //TODO implement State functions
            // Do Nothing.
        }
    }
}