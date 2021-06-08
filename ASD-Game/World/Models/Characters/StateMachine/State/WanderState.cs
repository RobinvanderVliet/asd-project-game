using System;
using ASD_Game.World.Models.Characters.StateMachine.Data;

namespace ASD_Game.World.Models.Characters.StateMachine.State
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
            Console.WriteLine("Ik doe iets");
        }

        public override void Exit()
        {
            Console.WriteLine("Wander state Exit");
        }
    }
}