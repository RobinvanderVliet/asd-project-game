﻿using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;
using Creature.Pathfinder;
using System;
using System.Collections.Generic;
using System.Numerics;


namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public class TrainingMapGenerator
    {
        public List<List<Node>> trainingmap = new List<List<Node>>();
        public List<TrainerAI> players = new List<TrainerAI>();
        public List<TrainerAI> monsters = new List<TrainerAI>();

        private int _worldSize = 30;
        private Random _random = new Random();

        public char symbolWall = '|';
        public char[,] board = new char[30, 30];

        public TrainingMapGenerator()
        {
            GenerateWorld();
        }

        private List<List<Node>> GenerateWorld()
        {
            GenerateNodes();
            GenerateCreatures();
            return trainingmap;
        }

        private void GenerateBoard()
        {
            for (int i = 0; i < _worldSize; i++)
            {
                for (int j = 0; j < _worldSize; j++)
                {
                    board[i, j] = '◙';
                }
            }
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
                if(i < 5)
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

        bool isPassable(char symbol)
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
    }
}