using System;
using Creature.Creature.StateMachine.Data;
using Creature.Pathfinder;
using System.Collections.Generic;
using System.Numerics;

namespace Creature.Creature.StateMachine.State
{
    public class FollowCreatureState : CreatureState
    {
        
        public override void Do()
        {
            //TODO implement State functions
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