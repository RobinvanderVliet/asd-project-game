using ActionHandling.DTO;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using System;
using System.Numerics;

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

            if (_characterData is AgentData)
            {
                AgentAttack();
            }
            else
            {
                AIAttack();
            }
        }

        private void AIAttack()
        {
            _characterData.Destination = _target.Position;
            _characterData.MoveType = "Attack";
        }

        private void AgentAttack()
        {
            Character cha = _characterData.WorldService.GetCharacter(_target.CharacterId);
            Character pl = _characterData.WorldService.GetCharacter(_characterData.CharacterId);
            if (cha != null)
            {
                Vector2 plPos = new Vector2(pl.XPosition, pl.YPosition);
                Vector2 chaPos = new Vector2(cha.XPosition, cha.YPosition);

                if (Vector2.Distance(plPos, chaPos) == 1)
                {
                    AttackDTO attackDTO = new();
                    attackDTO.XPosition = cha.XPosition;
                    attackDTO.YPosition = cha.YPosition;
                    attackDTO.Stamina = 100;
                    attackDTO.Damage = 50;
                    attackDTO.PlayerGuid = pl.Id;

                    _characterData.AttackHandler.SendAttackDTO(attackDTO);
                }
            }
        }
    }
}