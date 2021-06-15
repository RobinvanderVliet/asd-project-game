using ASD_Game.World.Models.Characters.StateMachine.Data;
using System;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public class AttackState : CharacterState
    {
        public AttackState(ICharacterData characterData, ICharacterStateMachine characterStateMachine) : base(
            characterData, characterStateMachine)
        {
        }

        public override void Do()
        {
            DoWorldCheck();
            //TODO implement Attack logic +gather targetData

            // Console.WriteLine("Player health: " + _characterData.WorldService.GetCharacter(_characterData.CharacterId).Health);
            //
            // _target.AttackHandler.SendAttack("");
            // _target.Health -= 5;
            // Console.WriteLine("Enemy health: " + _target.Health);
        }
    }
}