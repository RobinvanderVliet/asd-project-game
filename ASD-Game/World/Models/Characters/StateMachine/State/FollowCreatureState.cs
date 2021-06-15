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
            DataGatheringService dataGatheringService = new(_characterData.WorldService);
            Character character = _characterData.WorldService.GetCharacter(_characterData.CharacterId);
            PathFinder pathFinder = new PathFinder(dataGatheringService.TranslateCharacterMap(character));
            ICharacterData targetData = _target;
            if (character != null)
            {
                dataGatheringService.ViewPointCalculator(new Vector2(character.XPosition, character.YPosition));

                Vector2 MPos = new Vector2(targetData.Position.X - dataGatheringService._pathingOffset.X,
                    targetData.Position.Y - dataGatheringService._pathingOffset.Y);

                Stack<Node> newPath = pathFinder.FindPath(new Vector2(6, 6), MPos);

                if (newPath == null) return;

                if (!(newPath.Peek().Position.X == _characterData.Position.X &&
                      newPath.Peek().Position.Y == _characterData.Position.Y))
                {
                    Vector2 destination = dataGatheringService.TransformPath(newPath.Pop().Position);
                    float newPositionX = destination.X;
                    float newPositionY = destination.Y;

                    if (_characterData is MonsterData)
                    {
                        _characterData.Position = new Vector2(
                            newPositionX,
                            newPositionY);

                        _characterData.MoveType = "Move";
                        _characterData.Destination = new Vector2(
                            newPositionX,
                            newPositionY
                        );
                    }
                    else
                    {
                        Character cha = _characterData.WorldService.GetCharacter(_target.CharacterId);
                        Character pl = _characterData.WorldService.GetCharacter(_characterData.CharacterId);
                        if (cha != null)
                        {
                            Vector2 plPos = new Vector2(pl.XPosition, pl.YPosition);
                            Vector2 chaPos = new Vector2(cha.XPosition, cha.YPosition);

                            if (Vector2.Distance(plPos, chaPos) > 1)
                            {
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
}