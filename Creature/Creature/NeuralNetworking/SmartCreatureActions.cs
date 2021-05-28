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
                CheckPath(smartmonster);
            }
            smartmonster.creatureData.Position = path.Pop().Position;
        }

        public void WalkUp(SmartMonster smartMonster) 
        {
            Vector2 destination = new Vector2(smartMonster.creatureData.Position.X, smartMonster.creatureData.Position.Y+1);
            if (IsValidMove(destination))
            {
                path = _pathfinder.FindPath(smartMonster.creatureData.Position, destination);
                CheckPath(smartMonster);
            }
        }

        public void WalkDown(SmartMonster smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.creatureData.Position.X, smartMonster.creatureData.Position.Y - 1);
            if (IsValidMove(destination))
            {
                path = _pathfinder.FindPath(smartMonster.creatureData.Position, destination);
                CheckPath(smartMonster);
            }
        }

        public void WalkLeft(SmartMonster smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.creatureData.Position.X -1, smartMonster.creatureData.Position.Y);
            if (IsValidMove(destination))
            {
                path = _pathfinder.FindPath(smartMonster.creatureData.Position, destination);
                CheckPath(smartMonster);
            }
        }

        public void WalkRight(SmartMonster smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.creatureData.Position.X +1, smartMonster.creatureData.Position.Y);
            if (IsValidMove(destination))
            {
                path = _pathfinder.FindPath(smartMonster.creatureData.Position, destination);
                CheckPath(smartMonster);
            }
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
                smartmonster.score -= 25;
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
                        if (IsValidMove(newDestination))
                        {
                            path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                            CheckPath(smartmonster);
                        }
                    }
                    else if(smartmonster.creatureData.Position.Y >= 10)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.X, smartmonster.creatureData.Position.Y - 10);
                        if (IsValidMove(newDestination))
                        {
                            path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                            CheckPath(smartmonster);
                        }
                    }
                }
                else if (player.location.Y == smartmonster.creatureData.Position.Y)
                {
                    if (smartmonster.creatureData.Position.X <= 19)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.Y, smartmonster.creatureData.Position.X + 10);
                        if (IsValidMove(newDestination))
                        {
                            path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                            CheckPath(smartmonster);
                        }
                    }
                    else if(smartmonster.creatureData.Position.X >= 10)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.Y, smartmonster.creatureData.Position.X - 10);
                        if (IsValidMove(newDestination))
                        {
                            path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                            CheckPath(smartmonster);
                        }
                    }
                }
                else
                {
                    if (smartmonster.creatureData.Position.X <= 19 && smartmonster.creatureData.Position.Y <= 19)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.Y + 10, smartmonster.creatureData.Position.X + 10);
                        if (IsValidMove(newDestination))
                        {
                            path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                            CheckPath(smartmonster);
                        }
                    }
                    else if(smartmonster.creatureData.Position.X >= 10 && smartmonster.creatureData.Position.Y >= 10)
                    {
                        Vector2 newDestination = new Vector2(smartmonster.creatureData.Position.Y - 10, smartmonster.creatureData.Position.X - 10);
                        if (IsValidMove(newDestination))
                        {
                            path = _pathfinder.FindPath(smartmonster.creatureData.Position, newDestination);
                            CheckPath(smartmonster);
                        }
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
                CheckPath(smartmonster);
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

        private void CheckPath(SmartMonster smartMonster)
        {
            if (path == null)
            {
                smartMonster.score--;
            }
        }

        private bool IsValidMove(Vector2 destination)
        {
            int topOfMap = 0;
            int botOfMap = 29;
            int leftOfMap = 0;
            int rightOfMap = 29;

            if(destination.X > leftOfMap && destination.X < rightOfMap && destination.Y > topOfMap && destination.Y < botOfMap)
            {
                return true;
            }

            return false;
        }
    }
}
