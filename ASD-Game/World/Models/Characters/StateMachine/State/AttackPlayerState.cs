using System;
using ASD_Game.World.Models.Characters.StateMachine.Data;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public class AttackPlayerState : CharacterState
    {
        public AttackPlayerState(ICharacterData characterData) : base(characterData)
        {
            _characterData = characterData;
        }

        public override void Entry()
        {
            Console.WriteLine("Attack player state Entry");
        }

        public override void Do()
        {
            throw new NotImplementedException();
        }

        public override void Do(ICharacterData characterData)
        {
            //throw new NotImplementedException();
        }

        public override void Exit()
        {
            Console.WriteLine("Attack player state Exit");
        }
    }
}