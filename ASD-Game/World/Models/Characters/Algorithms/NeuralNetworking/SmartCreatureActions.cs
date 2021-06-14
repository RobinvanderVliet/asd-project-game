using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ASD_Game.World.Models.Characters.Algorithms.Pathfinder;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking
{
    //Functionality already covered in the SmartCreatureTrainingActions
    [ExcludeFromCodeCoverage]
    public class SmartCreatureActions
    {
        private readonly Random _random = new Random();
        private readonly PathFinder _pathfinder;
        private DataGatheringService _dataGatheringService;

        public Stack<Node> Path = new Stack<Node>();

        private Vector2 _startPos = new Vector2(6, 6);
        private Vector2 _pathingOffset;

        public SmartCreatureActions(SmartMonster smartMonster)
        {
            _dataGatheringService = smartMonster._dataGatheringService;
            _pathfinder = new PathFinder(_dataGatheringService.TranslateCharacterMap(smartMonster));
        }

        public void Wander(SmartMonster smartMonster)
        {
            switch (_random.Next(0, 3))
            {
                case 0:
                    WalkUp(smartMonster);
                    break;

                case 1:
                    WalkDown(smartMonster);
                    break;

                case 2:
                    WalkLeft(smartMonster);
                    break;

                case 3:
                    WalkRight(smartMonster);
                    break;
            }
        }

        public void WalkUp(SmartMonster smartMonster)
        {
            ViewPointCalculator(smartMonster.CreatureData.Position);
            Vector2 destination = new Vector2(_startPos.X, _startPos.Y + 1);
            if (IsValidMove(destination))
            {
                Path = _pathfinder.FindPath(_startPos, destination);
                if (Path != null)
                {
                    if (Path.Count != 0)
                    {
                        smartMonster.MoveType = "Move";
                        smartMonster.Destination = TransformPath(Path.Peek().Position);
                    }
                }
            }
        }

        public void WalkDown(SmartMonster smartMonster)
        {
            ViewPointCalculator(smartMonster.CreatureData.Position);
            Vector2 destination = new Vector2(_startPos.X, _startPos.Y - 1);
            if (IsValidMove(destination))
            {
                Path = _pathfinder.FindPath(_startPos, destination);
                if (Path != null)
                {
                    if (Path.Count != 0)
                    {
                        smartMonster.MoveType = "Move";
                        smartMonster.Destination = TransformPath(Path.Peek().Position);
                    }
                }
            }
        }

        public void WalkLeft(SmartMonster smartMonster)
        {
            ViewPointCalculator(smartMonster.CreatureData.Position);
            Vector2 destination = new Vector2(_startPos.X - 1, _startPos.Y);
            if (IsValidMove(destination))
            {
                Path = _pathfinder.FindPath(_startPos, destination);
                if (Path != null)
                {
                    if (Path.Count != 0)
                    {
                        smartMonster.MoveType = "Move";
                        smartMonster.Destination = TransformPath(Path.Peek().Position);
                    }
                }
            }
        }

        public void WalkRight(SmartMonster smartMonster)
        {
            ViewPointCalculator(smartMonster.CreatureData.Position);
            Vector2 destination = new Vector2(_startPos.X + 1, _startPos.Y);
            if (IsValidMove(destination))
            {
                Path = _pathfinder.FindPath(_startPos, destination);
                if (Path != null)
                {
                    if (Path.Count != 0)
                    {
                        smartMonster.MoveType = "Move";
                        smartMonster.Destination = TransformPath(Path.Peek().Position);
                    }
                }
            }
        }

        public void Attack(Character player, SmartMonster smartMonster)
        {
            if (player != null)
            {
                Vector2 PPos = new Vector2(player.XPosition, player.YPosition);
                Vector2 SMPos = new Vector2(smartMonster.XPosition, smartMonster.YPosition);
                if (IsAdjacent(PPos, SMPos))
                {
                    smartMonster.MoveType = "Attack";
                    smartMonster.Destination = PPos;
                }
            }
        }

        private void LevelUp(SmartMonster smartMonster)
        {
            int oldHP = (int)smartMonster.CreatureData.Health;
            int oldDMG = smartMonster.CreatureData.Damage;
            int newHP = (int)(oldHP * 1.5);
            int newDMG = (int)(oldDMG * 1.5);

            smartMonster.HealthHealed = newHP - oldHP;
            smartMonster.StatsGained = newDMG - oldDMG;

            smartMonster.CreatureData.Damage = newDMG;
            smartMonster.CreatureData.Health = newHP;
        }

        public void Flee(Character player, SmartMonster smartMonster)
        {
            if (player != null)
            {
                smartMonster.MoveType = "Move";
                Wander(smartMonster);
            }
        }

        public void RunToMonster(Character monster, SmartMonster smartMonster)
        {
            if (monster != null)
            {
                ViewPointCalculator(smartMonster.CreatureData.Position);
                Vector2 MPos = new Vector2(monster.XPosition - _pathingOffset.X, monster.YPosition - _pathingOffset.Y);
                Path = _pathfinder.FindPath(_startPos, MPos);
                if (Path != null && Path.Count != 0)
                {
                    smartMonster.MoveType = "Move";
                    smartMonster.Destination = TransformPath(Path.Pop().Position);
                }
            }
        }

        public void RunToPlayer(Player player, SmartMonster smartMonster)
        {
            if (player != null)
            {
                Vector2 playerPos = new Vector2(player.XPosition, player.YPosition);
                Path = _pathfinder.FindPath(smartMonster.CreatureData.Position, playerPos);
                smartMonster.CreatureData.Position = Path.Pop().Position;
            }
        }

        public void TakeDamage(int damage, SmartMonster smartMonster)
        {
            smartMonster.DamageTaken = damage;
            smartMonster.CreatureData.Health -= damage;
            if (smartMonster.CreatureData.Health <= 0)
            {
                smartMonster.MoveType = "Move";
                smartMonster.Dead = true;
            }
        }

        private static bool IsAdjacent(Vector2 loc1, Vector2 loc2)
        {
            float distance = Vector2.Distance(loc1, loc2);
            return (distance == 1);
        }

        private static bool IsValidMove(Vector2 destination)
        {
            return true;
        }

        private Vector2 TransformPath(Vector2 nextpos)
        {
            return nextpos + _pathingOffset;
        }

        private void ViewPointCalculator(Vector2 pos)
        {
            _pathingOffset = new Vector2(pos.X - 6, pos.Y - 6);
        }
    }
}