using System;
using System.Collections.Generic;
using System.Numerics;
using ASD_Game.World.Models.Characters.Algorithms.Pathfinder;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario
{
    public class SmartCreatureTrainingActions
    {
        private readonly Random _random = new Random();
        private readonly PathFinder _pathfinder;

        public Stack<Node> Path = new Stack<Node>();

        public SmartCreatureTrainingActions(List<List<Node>> map)
        {
            _pathfinder = new PathFinder(map);
        }

        public void Wander(SmartMonsterForTraining smartMonster)
        {
            if (Path == null || Path.Count == 0)
            {
                int maxMapSize = 29;
                int newXLoc = _random.Next(0, maxMapSize);
                int newYLoc = _random.Next(0, maxMapSize);

                Vector2 destination = new Vector2(newXLoc, newYLoc);

                Path = _pathfinder.FindPath(smartMonster.CreatureData.Position, destination);
                CheckPath(smartMonster);
            }
            smartMonster.CreatureData.Position = Path.Pop().Position;
        }

        public void WalkUp(SmartMonsterForTraining smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.CreatureData.Position.X, smartMonster.CreatureData.Position.Y + 1);
            if (IsValidMove(destination))
            {
                Path = _pathfinder.FindPath(smartMonster.CreatureData.Position, destination);
                CheckPath(smartMonster);
                smartMonster.CreatureData.Position = Path.Pop().Position;
            }
        }

        public void WalkDown(SmartMonsterForTraining smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.CreatureData.Position.X, smartMonster.CreatureData.Position.Y - 1);
            if (IsValidMove(destination))
            {
                Path = _pathfinder.FindPath(smartMonster.CreatureData.Position, destination);
                CheckPath(smartMonster);
                smartMonster.CreatureData.Position = Path.Pop().Position;
            }
        }

        public void WalkLeft(SmartMonsterForTraining smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.CreatureData.Position.X - 1, smartMonster.CreatureData.Position.Y);
            if (IsValidMove(destination))
            {
                Path = _pathfinder.FindPath(smartMonster.CreatureData.Position, destination);
                CheckPath(smartMonster);
                smartMonster.CreatureData.Position = Path.Pop().Position;
            }
        }

        public void WalkRight(SmartMonsterForTraining smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.CreatureData.Position.X + 1, smartMonster.CreatureData.Position.Y);
            if (IsValidMove(destination))
            {
                Path = _pathfinder.FindPath(smartMonster.CreatureData.Position, destination);
                CheckPath(smartMonster);
                smartMonster.CreatureData.Position = Path.Pop().Position;
            }
        }

        public void Attack(TrainerAI player, SmartMonsterForTraining smartmonster)
        {
            if (player != null && IsAdjacent(player.Location, smartmonster.CreatureData.Position))
            {
                if (player.Health < smartmonster.CreatureData.Damage)
                {
                    smartmonster.DamageDealt = smartmonster.DamageDealt + smartmonster.CreatureData.Damage;
                    smartmonster.EnemysKilled++;
                }
                else
                {
                    player.Health = player.Health - smartmonster.CreatureData.Damage;
                    smartmonster.DamageDealt = smartmonster.DamageDealt + smartmonster.CreatureData.Damage;
                }
            }
        }

        public void Flee(TrainerAI player, SmartMonsterForTraining smartmonster)
        {
            if (player != null)
            {
                Wander(smartmonster);
            }
        }

        public void RunToMonster(TrainerAI monster, SmartMonsterForTraining smartMonster)
        {
            if (monster != null)
            {
                Path = _pathfinder.FindPath(smartMonster.CreatureData.Position, monster.Location);
                CheckPath(smartMonster);
                smartMonster.CreatureData.Position = Path.Pop().Position;
            }
        }

        public void TakeDamage(int damage, SmartMonsterForTraining smartMonster)
        {
            smartMonster.DamageTaken = damage;
            smartMonster.CreatureData.Health -= damage;
            if (smartMonster.CreatureData.Health <= 0)
            {
                smartMonster.Dead = true;
            }
        }

        private static bool IsAdjacent(Vector2 loc1, Vector2 loc2)
        {
            float distance = Vector2.Distance(loc1, loc2);
            return (distance < 2);
        }

        private void CheckPath(SmartMonsterForTraining smartMonster)
        {
            if (Path == null)
            {
                smartMonster.Score--;
            }
        }

        private static bool IsValidMove(Vector2 destination)
        {
            int topOfMap = 0;
            int botOfMap = 29;
            int leftOfMap = 0;
            int rightOfMap = 29;

            if (destination.X > leftOfMap && destination.X < rightOfMap && destination.Y > topOfMap && destination.Y < botOfMap)
            {
                return true;
            }

            return false;
        }
    }
}