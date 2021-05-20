using System;
using Creature.Creature.StateMachine.Data;
using Creature.Pathfinder;
using System.Collections.Generic;
using System.Numerics;
using Creature.Creature.StateMachine.CustomRuleSet;

namespace Creature.Creature.StateMachine.State
{
    public class FollowPlayerState : CreatureState
    {
        public FollowPlayerState(ICreatureData creatureData) : base(creatureData)
        {
            _creatureData = creatureData;
        }

        public override void Do()
        {
            throw new NotImplementedException();
        }

        public override void Do(ICreatureData creatureData)
        {
            PathFinder pathFinder = new PathFinder(_creatureData.World.Nodes);
            ICreatureData playerData = creatureData;

            Stack<Node> newPath = pathFinder.FindPath(_creatureData.Position, playerData.Position);

            if (!(newPath.Peek().position.X == playerData.Position.X && newPath.Peek().position.Y == playerData.Position.Y))
            {
                float newPositionX = newPath.Peek().position.X;
                float newPositionY = newPath.Peek().position.Y;
                _creatureData.Position = new Vector2(newPositionX, newPositionY);
            }
        }
    }
}