using Creature.Creature.StateMachine.Data;
using Creature.Pathfinder;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Creature.Creature.StateMachine.State
{
    public class FollowPlayerState : CreatureState
    {
        public override void Do(ICreatureData creatureData)
        {
            PathFinder pathFinder = new PathFinder(creatureData.World.Nodes);
            ICreatureData playerData = creatureData.World.Players[0].CreatureStateMachine.CreatureData;

            Stack<Node> newPath = pathFinder.FindPath(creatureData.Position, playerData.Position);

            if (!(newPath.Peek().position.X == playerData.Position.X && newPath.Peek().position.Y == playerData.Position.Y))
            {
                float newPositionX = newPath.Peek().position.X;
                float newPositionY = newPath.Peek().position.Y;
                creatureData.Position = new Vector2(newPositionX, newPositionY);
            }
        }
    }
}