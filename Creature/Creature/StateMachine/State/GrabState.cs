using Creature.Creature.StateMachine.Data;
using System;

namespace Creature.Creature.StateMachine.State
{
    public class GrabState : CreatureState
    {
        public GrabState(ICreatureData creatureData) : base(creatureData)
        {
            _creatureData = creatureData;
        }

        public override void Do()
        {
            throw new NotImplementedException();
        }

        public override void Do(ICreatureData creatureData)
        {
            // throw new NotImplementedException();
        }
    }
}
