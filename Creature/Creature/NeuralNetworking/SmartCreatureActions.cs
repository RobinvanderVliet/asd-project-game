﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Creature.Pathfinder;

namespace Creature.Creature.NeuralNetworking
{
    public class SmartCreatureActions
    {
        private readonly Random _random = new Random();
        private readonly PathFinder _pathfinder;

        public Stack<Node> path = new Stack<Node>();

        public SmartCreatureActions(World.IWorld world)
        {
            //_pathfinder = new PathFinder(world);
        }

        public void Wander(SmartMonster smartMonster)
        {
            if (path == null || path.Count == 0)
            {
                int newXLoc = _random.Next((int)smartMonster.creatureData.Position.X, smartMonster.creatureData.VisionRange);
                int newYLoc = _random.Next((int)smartMonster.creatureData.Position.Y, smartMonster.creatureData.VisionRange);

                Vector2 destination = new Vector2(newXLoc, newYLoc);

                path = _pathfinder.FindPath(smartMonster.creatureData.Position, destination);
                CheckPath(smartMonster);
            }
            smartMonster.creatureData.Position = path.Pop().Position;
        }

        public void WalkUp(SmartMonster smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.creatureData.Position.X, smartMonster.creatureData.Position.Y + 1);
            if (IsValidMove(destination))
            {
                path = _pathfinder.FindPath(smartMonster.creatureData.Position, destination);
                CheckPath(smartMonster);
                smartMonster.creatureData.Position = path.Pop().Position;
            }
        }

        public void WalkDown(SmartMonster smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.creatureData.Position.X, smartMonster.creatureData.Position.Y - 1);
            if (IsValidMove(destination))
            {
                path = _pathfinder.FindPath(smartMonster.creatureData.Position, destination);
                CheckPath(smartMonster);
                smartMonster.creatureData.Position = path.Pop().Position;
            }
        }

        public void WalkLeft(SmartMonster smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.creatureData.Position.X - 1, smartMonster.creatureData.Position.Y);
            if (IsValidMove(destination))
            {
                path = _pathfinder.FindPath(smartMonster.creatureData.Position, destination);
                CheckPath(smartMonster);
                smartMonster.creatureData.Position = path.Pop().Position;
            }
        }

        public void WalkRight(SmartMonster smartMonster)
        {
            Vector2 destination = new Vector2(smartMonster.creatureData.Position.X + 1, smartMonster.creatureData.Position.Y);
            if (IsValidMove(destination))
            {
                path = _pathfinder.FindPath(smartMonster.creatureData.Position, destination);
                CheckPath(smartMonster);
                smartMonster.creatureData.Position = path.Pop().Position;
            }
        }

        public void Attack(ICreature player, SmartMonster smartmonster)
        {
            if (player != null && IsAdjacent(player.CreatureStateMachine.CreatureData.Position, smartmonster.creatureData.Position))
            {
                player.CreatureStateMachine.CreatureData.Health = player.CreatureStateMachine.CreatureData.Health - smartmonster.creatureData.Damage;
            }
        }

        public void Flee(ICreature player, SmartMonster smartmonster)
        {
            if (player != null)
            {
                Wander(smartmonster);
            }
        }

        public void UseItem()
        {
            //To be implemented
        }

        public void RunToMonster(ICreature monster, SmartMonster smartMonster)
        {
            if (monster != null)
            {
                path = _pathfinder.FindPath(smartMonster.creatureData.Position, monster.CreatureStateMachine.CreatureData.Position);
                CheckPath(smartMonster);
                smartMonster.creatureData.Position = path.Pop().Position;
            }
        }

        public void GrabItem(Vector2 loc)
        {
            //To be implemented
        }

        public void TakeDamage(int damage, SmartMonster smartMonster)
        {
            smartMonster.DamageTaken = damage;
            smartMonster.creatureData.Health -= damage;
            if (smartMonster.creatureData.Health <= 0)
            {
                smartMonster.dead = true;
            }
        }

        private static bool IsAdjacent(Vector2 loc1, Vector2 loc2)
        {
            float distance = Vector2.Distance(loc1, loc2);
            return (distance < 2);
        }

        private void CheckPath(SmartMonster smartMonster)
        {
            if (path == null)
            {
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