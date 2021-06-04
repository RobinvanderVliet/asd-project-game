using System;
using Creature.Creature.StateMachine.Data;
using Creature.Pathfinder;
using System.Collections.Generic;
using System.Numerics;
using ASD_project.Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Builder;

namespace Creature.Creature.StateMachine.State
{
    public class FollowCreatureState : CreatureState
    {
        public FollowCreatureState(ICreatureData creatureData, ICreatureStateMachine stateMachine, List<BuilderInfo> builderInfoList, BuilderConfiguration builderConfiguration) : base(creatureData, stateMachine, builderInfoList, builderConfiguration)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
            _builderConfiguration = builderConfiguration;
            _builderInfoList = builderInfoList;
        }

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