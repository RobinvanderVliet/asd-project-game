using System;
using Creature.Creature.StateMachine.Data;

namespace Creature.Creature.StateMachine.State
{
    public class UseConsumableState : CreatureState
    {
        public UseConsumableState(ICreatureData creatureData)
        {
            _creatureData = creatureData;
        }

        public override void Do()
        {
            //TODO implement State functions
            throw new NotImplementedException();
        }
       
    }
}