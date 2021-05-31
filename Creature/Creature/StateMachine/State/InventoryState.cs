

using System;
using Creature.Creature.StateMachine.Data;

namespace Creature.Creature.StateMachine.State
{
    public class InventoryState : CreatureState
    {
        public InventoryState(ICreatureData creatureData) : base(creatureData)
        {
            _creatureData = creatureData;
        }
        
        public override void Do(ICreatureData creatureData)
        {
            throw new NotImplementedException();
        }
    }
}