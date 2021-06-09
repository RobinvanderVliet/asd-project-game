using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Characters.Algorithms.Pathfinder;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using System.Collections.Generic;
using System.Numerics;
using WorldGeneration.StateMachine;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public class FollowCreatureState : CharacterState
    {
        public FollowCreatureState(ICharacterData characterData, ICharacterStateMachine characterStateMachine) : base(characterData, characterStateMachine)
        {
            
        }

        public override void Do()
        {
            DoWorldCheck();

            var _builderInfoList = _characterData.BuilderConfigurator.GetBuilderInfoList();
            var _builderConfiguration = _characterData.BuilderConfigurator;
            
            //foreach (var builderInfo in _builderInfoList)
            //{
            //    if (builderInfo.Action == "follow")
            //    {
            //        if (_builderConfiguration.GetGuard(_characterData, _target, builderInfo))
            //        {
                        DataGatheringService dataGatheringService = new(_characterData.WorldService);
                        PathFinder pathFinder = new PathFinder(dataGatheringService.TranslateCharacterMap(_characterData.WorldService.GetAI(_characterData.CharacterId)));
                        ICharacterData targetData = _target;

                        dataGatheringService.ViewPointCalculator(_target.Position);

                        Stack<Node> newPath = pathFinder.FindPath(new Vector2(6,6), new Vector2(targetData.Position.X, targetData.Position.Y));

                        if (!(newPath.Peek().Position.X == _characterData.Position.X && newPath.Peek().Position.Y == _characterData.Position.Y))
                        {
                            Vector2 destination = dataGatheringService.TransformPath(newPath.Pop().Position);
                            float newPositionX = destination.X;
                            float newPositionY = destination.Y;
                            _characterData.Position = new Vector2(newPositionX, newPositionY);
                        }
                //    }
                //}
            //}
        }
    }
}