using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using System;

namespace Creature.Creature.StateMachine.State
{
    public class AttackPlayerState : CreatureState
    {
        public AttackPlayerState(ICreatureData creatureData) : base(creatureData)
        {
            _creatureData = creatureData;
        }

        public override void Do()
        {
            throw new NotImplementedException();
        }

        public override void Do(ICreatureData creatureData)
        {
            ICreatureData playerData = creatureData;
            playerData.Health -= playerData.Damage;

            throw new NotImplementedException();
        }
    }
}