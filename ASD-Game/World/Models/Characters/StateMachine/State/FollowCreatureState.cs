using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Characters.Algorithms.Pathfinder;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public class FollowCreatureState : CharacterState
    {
        public FollowCreatureState(ICharacterData characterData, ICharacterStateMachine characterStateMachine) : base(
            characterData, characterStateMachine)
        {
        }

        public override void Do()
        {
            DoWorldCheck();

            var _builderInfoList = _characterData.BuilderConfigurator.GetBuilderInfoList();
            var _builderConfiguration = _characterData.BuilderConfigurator;

            foreach (var builderInfo in _builderInfoList)
            {
                {
                    if (_builderConfiguration.GetGuard(_characterData, _target, builderInfo))
                    {
                        Console.WriteLine("In follow");

                        DataGatheringService dataGatheringService = new(_characterData.WorldService);
                        Character character = _characterData.WorldService.GetCharacter(_characterData.CharacterId);
                        PathFinder pathFinder = new PathFinder(dataGatheringService.TranslateCharacterMap(character));
                        ICharacterData targetData = _target;

                        dataGatheringService.ViewPointCalculator(new Vector2(character.XPosition, character.YPosition));

                        Vector2 MPos = new Vector2(targetData.Position.X - dataGatheringService._pathingOffset.X,
                            targetData.Position.Y - dataGatheringService._pathingOffset.Y);

                        Stack<Node> newPath = pathFinder.FindPath(new Vector2(6, 6), MPos);

                        if (!(newPath.Peek().Position.X == _characterData.Position.X &&
                              newPath.Peek().Position.Y == _characterData.Position.Y))
                        {
                            Vector2 destination = dataGatheringService.TransformPath(newPath.Pop().Position);
                            float newPositionX = destination.X;
                            float newPositionY = destination.Y;
                            _characterData.MoveHandler.SendAIMove(_characterData.CharacterId,
                                Convert.ToInt32(newPositionX),
                                Convert.ToInt32(newPositionY)
                            );
                        }
                    }
                }
            }
        }
    }
}