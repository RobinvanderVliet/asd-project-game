using System;
using System.Collections.Generic;
using System.Numerics;
using Creature.Pathfinder;

namespace Creature.Creature.NeuralNetworking
{
    public class SmartCreatureActions
    {
        private Random _random = new Random();
        private PathFinder _pathfinder;

        public Stack<Node> path = new Stack<Node>();

        public void Wander(Vector2 loc) 
        {
            int newXLoc = _random.Next(0, 30);
            int newYLoc = _random.Next(0, 30);

            Vector2 destination = new Vector2(newXLoc, newYLoc);

            path = _pathfinder.FindPath(loc, destination);
        }

        public void Attack(ICreature player, SmartMonster smartmonster)
        {
            if(IsAdjacent(player.CreatureStateMachine.CreatureData.Position,smartmonster.creatureData.Position))
            {
                player.CreatureStateMachine.CreatureData.Health = player.CreatureStateMachine.CreatureData.Health - smartmonster.creatureData.Damage;
                smartmonster.DamageDealt = smartmonster.DamageDealt + smartmonster.creatureData.Damage;
                if(player.CreatureStateMachine.CreatureData.Health < smartmonster.creatureData.Damage)
                {
                    smartmonster.EnemysKilled++;
                }
            }
        }

        public void Flee(ICreature player, SmartMonster smartmonster) 
        {
            if(player.CreatureStateMachine.CreatureData.Position.X == smartmonster.creatureData.Position.X)
            {
                Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.X, smartmonster.creatureData.Position.Y + 10);
                path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
            }
            else if (player.CreatureStateMachine.CreatureData.Position.Y == smartmonster.creatureData.Position.Y)
            {
                Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.Y, smartmonster.creatureData.Position.X + 10);
                path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
            }
            else
            {
                Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.Y+10, smartmonster.creatureData.Position.X + 10);
                path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
            }
        }

        public void UseItem() 
        {
            //To be implemented
        }

        public void RunToMonster(ICreature monster, SmartMonster smartmonster) 
        {
            path = _pathfinder.FindPath(monster.CreatureStateMachine.CreatureData.Position, smartmonster.creatureData.Position);
        }

        public void GrabItem(Vector2 loc) 
        {
            //To be implemented
        }

        public void TakeDamage(ICreature player, SmartMonster smartMonster)
        {
            smartMonster.DamageTaken = player.CreatureStateMachine.CreatureData.Damage;

            if(smartMonster.creatureData.Health <= 0)
            {
                smartMonster.dead = true;
            }
        }

        private bool IsAdjacent(Vector2 loc1, Vector2 loc2)
        {
            float distance = Vector2.Distance(loc1, loc2);
            return (distance < 2);
        }

    }
}
