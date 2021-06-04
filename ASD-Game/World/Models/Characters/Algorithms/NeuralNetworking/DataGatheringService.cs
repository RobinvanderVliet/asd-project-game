using Creature.Pathfinder;
using System;
using System.Collections.Generic;
using System.Numerics;
using WorldGeneration;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public class DataGatheringService
    {
        private IWorldService WorldService;

        private int _colCount = 12;
        private int _rowCount = 12;

        private List<Player> _players = new List<Player>();

        public List<List<Node>> mapNodes = new List<List<Node>>();
        public Player closestPlayer { get; set; }
        public Single distanceToClosestPlayer { get; set; } = 9999999999999999999;
        public Character closestMonster { get; set; }
        public Single distanceToClosestMonster { get; set; } = 9999999999999999999;

        public DataGatheringService(IWorldService worldService)
        {
            WorldService = worldService;
        }

        public void ScanMap(SmartMonster smartMonster, int visionRange)
        {
            SetClosestMonster(smartMonster, visionRange);
            SetClosestPlayer(smartMonster, visionRange);
        }

        private void SetClosestMonster(SmartMonster smartMonster, int visionRange)
        {
            List<Character> monsters = WorldService.GetMonsters();
            foreach (Character monster in monsters)
            {
                Vector2 pPos = new Vector2(monster.XPosition, monster.YPosition);
                Vector2 cPos = new Vector2(smartMonster.XPosition, smartMonster.YPosition);
                Single distance = Vector2.Distance(pPos, cPos);
                if (distance < distanceToClosestPlayer)
                {
                    closestMonster = monster;
                    distanceToClosestPlayer = distance;
                }
            }
        }

        private void SetClosestPlayer(SmartMonster smartMonster, int visionRange)
        {
            List<Player> players = WorldService.GetPlayers();
            foreach (Player player in players)
            {
                Vector2 pPos = new Vector2(player.XPosition, player.YPosition);
                Vector2 cPos = new Vector2(smartMonster.XPosition, smartMonster.YPosition);
                Single distance = Vector2.Distance(pPos, cPos);
                if (distance < distanceToClosestPlayer)
                {
                    closestPlayer = player;
                    distanceToClosestPlayer = distance;
                }
            }
        }

        public void CheckNewPosition(SmartMonster smartMonster)
        {
            if (distanceToClosestPlayer < smartMonster.currDistanceToPlayer)
            {
                smartMonster.currDistanceToPlayer = distanceToClosestPlayer;
            }
            else if (distanceToClosestPlayer > smartMonster.currDistanceToPlayer)
            {
                smartMonster.currDistanceToPlayer = distanceToClosestPlayer;
            }
            if (distanceToClosestMonster < smartMonster.currDistanceToMonster)
            {
                smartMonster.currDistanceToMonster = distanceToClosestMonster;
            }
            else if (distanceToClosestMonster > smartMonster.currDistanceToMonster)
            {
                smartMonster.currDistanceToMonster = distanceToClosestMonster;
            }
        }

        public List<List<Node>> TranslateCharacterMap(Character c)
        {
            List<List<Node>> translatedMap = new List<List<Node>>();
            char[,] map = WorldService.GetMapAroundCharacter(c);

            for (int row = 0; row < _colCount; row++)
            {
                List<Node> nodePoints = new List<Node>();
                for (int col = 0; col < _rowCount; col++)
                {
                    Vector2 nodeLocation = new Vector2(row, col);
                    Node node = TranslateCharToNode(nodeLocation, map[_colCount, _rowCount]);
                    nodePoints.Add(node);
                }
                translatedMap.Add(nodePoints);
            }
            return translatedMap;
        }

        private Node TranslateCharToNode(Vector2 pos, char c)
        {
            Node node = new Node(pos, IsWalkable(c));
            return node;
        }

        private bool IsWalkable(char c)
        {
            switch (c)
            {
                case '~':
                    return false;
                    break;

                case 'E':
                    return false;
                    break;

                case '#':
                    return false;
                    break;

                case 'T':
                    return false;
                    break;

                case '\u25B2':
                    return false;
                    break;
            }
            return true;
        }

        private bool IsPlayerInSight(Character c)
        {
            char[,] map = WorldService.GetMapAroundCharacter(c);
            for (int row = 0; row < _colCount; row++)
            {
                for (int col = 0; col < _rowCount; col++)
                {
                    if (map[row, col].Equals('#'))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}