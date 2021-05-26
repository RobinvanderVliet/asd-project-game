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
            throw new NotImplementedException();
        }

        public override void Do(ICreatureData creatureData)
        {
            ICreatureData playerData = creatureData;
            playerData.Health -= playerData.Damage;

            Console.WriteLine("Player health: " + playerData.Health);

           // throw new NotImplementedException();
        }
    }
}