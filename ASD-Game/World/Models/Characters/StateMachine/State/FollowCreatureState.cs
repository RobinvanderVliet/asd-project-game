using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using WorldGeneration.StateMachine;

namespace Creature.Creature.StateMachine.State
{
    public class FollowCreatureState : CharacterState
    {
        public FollowCreatureState(ICharacterData characterData, ICharacterStateMachine characterStateMachine) : base(characterData, characterStateMachine)
        {
            
        }

        public override void Do()
        {
            var _builderInfoList = _characterData.BuilderConfigurator.GetBuilderInfoList();
            var _builderConfiguration = _characterData.BuilderConfigurator;
            
            foreach (var builderInfo in _builderInfoList)
            {
                if (builderInfo.Action == "follow")
                {
                    if (_builderConfiguration.GetGuard(_characterData, _target, builderInfo))
                    {
                        //TODO implement Attack logic + gather targetData
                        // PathFinder pathFinder = new PathFinder(_creatureData.World.Nodes);
                        // ICreatureData playerData = creatureData;
                        //
                        // Stack<Node> newPath = pathFinder.FindPath(_creatureData.Position, playerData.Position);
                        //
                        // if (!(newPath.Peek().Position.X == playerData.Position.X && newPath.Peek().Position.Y == playerData.Position.Y))
                        // {
                        //     float newPositionX = newPath.Peek().Position.X;
                        //     float newPositionY = newPath.Peek().Position.Y;
                        //     _creatureData.Position = new Vector2(newPositionX, newPositionY);
                        //}
                    }
                }
            }
        }
    }
}