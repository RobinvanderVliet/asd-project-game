using System;
using System.Collections.Generic;
using System.Numerics;
using Creature.Creature.NeuralNetworking.TrainingScenario;
using Creature.Pathfinder;

namespace Creature.Creature.NeuralNetworking
{
    public class SmartCreatureActions
    {
        private Random _random = new Random();
        private PathFinder _pathfinder;

        public Stack<Node> path = new Stack<Node>();

        public SmartCreatureActions(List<List<Node>> map)
        {
            _pathfinder = new PathFinder(map);
        }

        public void Wander(SmartMonster smartmonster, Vector2 loc) 
        {
            if (path == null || path.Count == 0)
            {
                int newXLoc = _random.Next(0, 29);
                int newYLoc = _random.Next(0, 29);

                Vector2 destination = new Vector2(newXLoc, newYLoc);

                path = _pathfinder.FindPath(loc, destination);
            }
            smartmonster.creatureData.Position = path.Pop().Position;

        }

        public void Attack(TrainerAI player, SmartMonster smartmonster)
        {
            if(player != null &&IsAdjacent(player.location,smartmonster.creatureData.Position))
            {
                player.health = player.health - smartmonster.creatureData.Damage;
                smartmonster.DamageDealt = smartmonster.DamageDealt + smartmonster.creatureData.Damage;
                if(player.health < smartmonster.creatureData.Damage)
                {
                    smartmonster.EnemysKilled++;
                }
            }
            else
            {
                smartmonster.score =- 25;
            }
        }

        public void Flee(TrainerAI player, SmartMonster smartmonster) 
        {
            if (player != null)
            {
                if (player.location.X == smartmonster.creatureData.Position.X)
                {
                    if (smartmonster.creatureData.Position.Y <= 19)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.X, smartmonster.creatureData.Position.Y + 10);
                        path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                    }
                    else if(smartmonster.creatureData.Position.Y >= 10)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.X, smartmonster.creatureData.Position.Y - 10);
                        path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                    }
                }
                else if (player.location.Y == smartmonster.creatureData.Position.Y)
                {
                    if (smartmonster.creatureData.Position.X <= 19)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.Y, smartmonster.creatureData.Position.X + 10);
                        path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                    }
                    else if(smartmonster.creatureData.Position.X >= 10)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.Y, smartmonster.creatureData.Position.X - 10);
                        path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                    }
                }
                else
                {
                    if (smartmonster.creatureData.Position.X <= 19 && smartmonster.creatureData.Position.Y <= 19)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.Y + 10, smartmonster.creatureData.Position.X + 10);
                        path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                    }
                    else if(smartmonster.creatureData.Position.X >= 10 && smartmonster.creatureData.Position.Y >= 10)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.Y - 10, smartmonster.creatureData.Position.X - 10);
                        path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                    }
                }
            }
        }

        public void UseItem() 
        {
            //To be implemented
        }

        public void RunToMonster(TrainerAI monster, SmartMonster smartmonster) 
        {
            if (monster != null)
            {
                path = _pathfinder.FindPath(monster.location, smartmonster.creatureData.Position);
            }
        }

        public void GrabItem(Vector2 loc) 
        {
            //To be implemented
        }

        public void TakeDamage(int damage, SmartMonster smartMonster)
        {
            smartMonster.DamageTaken = damage;
            Console.WriteLine();
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
