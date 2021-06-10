using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ASD_Game.World.Models.Characters.Algorithms.Pathfinder;

namespace ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario
{
    public class TrainingMapGenerator
    {
        public List<List<Node>> trainingmap = new List<List<Node>>();
        public List<TrainerAI> players = new List<TrainerAI>();
        public List<TrainerAI> monsters = new List<TrainerAI>();

        private readonly int _worldSize = 30;
        private readonly Random _random = new Random();

        public char symbolWall = '|';
        public char[,] board = new char[30, 30];

        [ExcludeFromCodeCoverage]
        public TrainingMapGenerator()
        {
            GenerateWorld();
        }

        private void GenerateWorld()
        {
            GenerateNodes();
            GenerateCreatures();
        }

        private void GenerateNodes()
        {
            for (int row = 0; row < _worldSize; row++)
            {
                List<Node> nodePoints = new List<Node>();
                for (int col = 0; col < _worldSize; col++)
                {
                    Vector2 nodeLocation = new Vector2(row, col);
                    Node node = new Node(nodeLocation, isPassable(board[row, col]));
                    nodePoints.Add(node);
                }
                trainingmap.Add(nodePoints);
            }
        }

        private void GenerateCreatures()
        {
            for (int i = 0; i <= 20; i++)
            {
                if (i < 5)
                {
                    NewPlayer();
                }
                else
                {
                    NewMonster();
                }
            }
        }

        private void NewMonster()
        {
            Vector2 location = new(_random.Next(0, 29), _random.Next(0, 29));

            TrainerAI trainerAI = new TrainerAI(location, "monster");
            monsters.Add(trainerAI);
        }

        private void NewPlayer()
        {
            Vector2 location = new(_random.Next(0, 29), _random.Next(0, 29));

            TrainerAI trainerAI = new TrainerAI(location, "player");
            players.Add(trainerAI);
        }

        [ExcludeFromCodeCoverage]
        private bool isPassable(char symbol)
        {
            if (symbol == symbolWall)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool AllPlayersDead()
        {
            if ((players).Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}