using System;
using System.Numerics;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public class FleeFromCreatureState : CharacterState
    {
        private const int MAX_MOVEMENT_SPEED = 3;

        public FleeFromCreatureState(ICharacterData characterData, ICharacterStateMachine characterStateMachine) : base(characterData, characterStateMachine)
        {
        }

        public override void Do()
        {
            DoWorldCheck();

            var _builderInfoList = _characterData.BuilderConfigurator.GetBuilderInfoList();
            var _builderConfiguration = _characterData.BuilderConfigurator;

            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "flee")
                {
                    if (_builderConfiguration.GetGuard(_characterData, _target, builderInfo))
                    {
                        MoveRandomDirection();
                    }
                }
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

            _characterData.Position = new Vector2(
                _characterData.Position.X + x,
                _characterData.Position.Y + y
            );

            _characterData.MoveHandler.SendAIMove(_characterData.CharacterId,
                Convert.ToInt32(_characterData.Position.X),
                Convert.ToInt32(_characterData.Position.Y)
            );
        }
    }
}