using Creature.Creature.StateMachine.Data;
using System;

namespace Creature.Creature.StateMachine.State
{
    public class UseConsumableState : CreatureState
    {
        public UseConsumableState(ICreatureData creatureData) : base(creatureData)
        {
            _creatureData = creatureData;
        }

        public override void Entry()
        {
            Console.WriteLine("UseConsumable state Entry");
        }

        public override void Do()
        {
            throw new NotImplementedException();
        }
        public override void Do(ICreatureData creatureData)
        {
            throw new NotImplementedException();
        }

        public override void Exit()
        {
            Console.WriteLine("UseConsumable state Exit");
        }
    }
}