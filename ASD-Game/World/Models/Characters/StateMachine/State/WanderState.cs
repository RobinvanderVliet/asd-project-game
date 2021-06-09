using System;
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
            
            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "wander")
                {
                    if (_builderConfiguration.GetGuard(_characterData, _target, builderInfo))
                    {
                        int steps = new Random().Next(10);
                        _characterData.MoveHandler.SendMove(PickRandomDirection(), steps);
                    }
                }
            }
        }

        private string PickRandomDirection()
        {
            var _direction = "";
            var CaseSwitch = new Random().Next(1, 4);
            switch (CaseSwitch)
            {
                case 1:
                    _direction += "up";
                    break;
                case 2:
                    _direction += "right";
                    break;
                case 3:
                    _direction += "down";
                    break;
                case 4:
                    _direction += "left";
                    break;
            }
            return _direction;
        }
    }
}