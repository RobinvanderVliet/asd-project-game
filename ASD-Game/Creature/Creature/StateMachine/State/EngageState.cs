using Creature.Creature.StateMachine.Data;
using System;

namespace Creature.Creature.StateMachine.State
{
    public class EngageState : CreatureState
    {
        public EngageState(ICreatureData creatureData, ICreatureStateMachine stateMachine) : base(creatureData, stateMachine)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
        }

        public override void Do()
        {
            //TODO implement logic
            throw new NotImplementedException();
        }
    }
}