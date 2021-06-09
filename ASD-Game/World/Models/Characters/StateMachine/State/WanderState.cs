using System;
using System.Numerics;
using World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine.State;

namespace Creature.Creature.StateMachine.State
{
    public class WanderState : CharacterState
    {
        public WanderState(ICharacterData characterData) : base(characterData)
        {
        }

        public override void Do()
        {
            var _builderInfoList = _characterData.BuilderConfigurator.GetBuilderInfoList();
            var _builderConfiguration = _characterData.BuilderConfigurator;

            var steps = new Random().Next(10);
            //_characterData.MoveHandler.SendMove(PickRandomDirection(), steps);
            
            _characterData.Position = new Vector2(
                _characterData.Position.X + 2,
                _characterData.Position.Y
            );

            _characterData.MoveHandler.SendAIMove(_characterData.CharacterId,
                Convert.ToInt32(_characterData.Position.X),
                Convert.ToInt32(_characterData.Position.Y)
            );

            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "wander")
                {
                    if (_builderConfiguration.GetGuard(_characterData, _target, builderInfo))
                    {
                        //int steps = new Random().Next(10);
                        //_characterData.MoveHandler.SendMove(PickRandomDirection(), steps);
                    }
                }
            }
        }

        private string PickRandomDirection()
        {
            var _direction = "";
            var CaseSwitch = new Random().Next(1, 2);
            switch (CaseSwitch)
            {
                case 1:
                    _direction += "x";
                    break;
                case 2:
                    _direction += "y";
                    break;
            }

            return _direction;
        }
    }
}