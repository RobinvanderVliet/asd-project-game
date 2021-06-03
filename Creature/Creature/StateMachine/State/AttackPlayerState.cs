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

        public override void Entry()
        {
            Console.WriteLine("Attack player state Entry");
        }

        public override void Do()
        {
            throw new NotImplementedException();
        }

        public override void Do(ICreatureData creatureData)
        {
            //throw new NotImplementedException();
        }

        public override void Exit()
        {
            Console.WriteLine("Attack player state Exit");
        }

    }
}