using Creature.Creature.StateMachine.Data;
using System;

namespace Creature.Creature.StateMachine.State
{
    public class UseConsumableState : CreatureState
    {
        public UseConsumableState(ICreatureData creatureData, ICreatureStateMachine stateMachine) : base(creatureData, stateMachine)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
        }

        public override void Do()
        {
            //TODO implement State functions
            throw new NotImplementedException();
        }
       
    }
}