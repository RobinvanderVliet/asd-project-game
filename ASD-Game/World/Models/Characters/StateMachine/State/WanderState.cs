using Creature.Creature.StateMachine.Data;
using System;

namespace WorldGeneration.StateMachine.State
{
    public class WanderState : CharacterState
    {
        public WanderState(ICharacterData characterData) : base(characterData)
        {
            _characterData = characterData;
        }

        public override void Entry()
        {
            Console.WriteLine("Wander state Entry");
        }

        public override void Do()
        {
            Console.WriteLine("Ik doe iets");
        }
        
        public override void Do(ICharacterData characterData)
        {
            throw new NotImplementedException();
        }

        public override void Exit()
        {
            Console.WriteLine("Wander state Exit");
        }
    }
}