using System;
using Creature.Creature.StateMachine.Data;

namespace Creature.Creature.StateMachine.State
{
    public class WanderState : CreatureState
    {
        public WanderState(ICreatureData creatureData) : base(creatureData)
        {
            _creatureData = creatureData;
        }

        public override void Entry()
        {
            Console.WriteLine("Wander state Entry");
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
            Console.WriteLine("Wander state Exit");
        }
    }
}