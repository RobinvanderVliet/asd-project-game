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
            Console.WriteLine("In wander");

            var _builderInfoList = _characterData.BuilderConfigurator.GetBuilderInfoList();
            var _builderConfiguration = _characterData.BuilderConfigurator;

            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "wander")
                {
                    if (_builderConfiguration.GetGuard(_characterData, _target, builderInfo))
                    {
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