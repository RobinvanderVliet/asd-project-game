using Creature.Creature.StateMachine.Data;
using System;

namespace Creature.Creature.StateMachine.State
{
    public class AttackState : CreatureState
    {
        public AttackState(ICreatureData creatureData) : base(creatureData)
        {
            _creatureData = creatureData;
        }

        public override void Do()
        {
            //TODO implement logic
            throw new NotImplementedException();
        }
    }
}