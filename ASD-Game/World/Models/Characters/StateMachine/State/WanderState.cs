using System;
using System.Numerics;
using ASD_Game.World.Models.Characters.StateMachine.Data;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public class WanderState : CharacterState
    {
        private const int MAX_MOVEMENT_SPEED = 3;

        public WanderState(ICharacterData characterData, ICharacterStateMachine characterStateMachine) : base(
            characterData, characterStateMachine)
        {
        }

        public override void Do()
        {
            DoWorldCheck();

            if (_characterData is AgentData)
            {
                if (_characterData.WorldService.GetPlayer(_characterData.CharacterId).Stamina >= 20)
                {
                    MoveRandomDirection();
                }
            }
            else
            {
                MoveRandomDirection();
            }
        }

        private void MoveRandomDirection()
        {
            var x = 0;
            var y = 0;

            if (new Random().Next(10) >= 5)
            {
                x += new Random().Next(MAX_MOVEMENT_SPEED);
            }
            else
            {
                y += new Random().Next(MAX_MOVEMENT_SPEED);
            }

            if (_characterData is MonsterData)
            {
                _characterData.Position = new Vector2(
                    _characterData.Position.X + x,
                    _characterData.Position.Y + y);

                _characterData.MoveType = "Move";
                _characterData.Destination = new Vector2(
                    _characterData.Position.X + x,
                    _characterData.Position.Y + y
                );
            }
            else
            {
                _characterData.MoveHandler.SendAIMove(_characterData.CharacterId,
                    Convert.ToInt32(_characterData.Position.X + x),
                    Convert.ToInt32(_characterData.Position.Y + y)
                );
            }
        }
    }
}