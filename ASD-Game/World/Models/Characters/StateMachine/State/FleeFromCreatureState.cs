using System;
using System.Numerics;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public class FleeFromCreatureState : CharacterState
    {
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
                        String direction = "";
                        if (Vector2.DistanceSquared(_characterData.Position, _target.Position) <=
                            Vector2.DistanceSquared(
                                new Vector2(_characterData.Position.X + 1, _characterData.Position.Y + 1),
                                _target.Position))
                        {
                            direction = "up";
                        }
                        else if (Vector2.DistanceSquared(_characterData.Position, _target.Position) <=
                                 Vector2.DistanceSquared(
                                     new Vector2(_characterData.Position.X - 1, _characterData.Position.Y - 1),
                                     _target.Position))
                        {
                            direction = "down";
                        }

                        _characterData.MoveHandler.SendMove(direction, 1);
                    }
                }
            }
        }
    }
}