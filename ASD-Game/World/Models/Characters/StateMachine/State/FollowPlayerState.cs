using System;
using ASD_Game.World.Models.Characters.StateMachine.Data;

namespace ASD_Game.World.Models.Characters.StateMachine.State
{
    public class FollowPlayerState : CharacterState
    {
        public FollowPlayerState(ICharacterData characterData) : base(characterData)
        {
            _characterData = characterData;
        }

        public override void Entry()
        {
            Console.WriteLine("Follow player state Entry");
        }

        public override void Do()
        {
            Console.WriteLine("Ik doe iets");
        }

        public override void Do(ICharacterData characterData)
        {
            // TODO:: fix pathfinding with new world setup.
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
            // }
        }

        public override void Exit()
        {
            Console.WriteLine("Follow player state exit");
        }
    }
}