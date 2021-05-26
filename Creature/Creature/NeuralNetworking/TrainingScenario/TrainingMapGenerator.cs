using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;
using System;
using System.Collections.Generic;
using System.Numerics;


namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public class TrainingMapGenerator
    {
        public List<List<int>> trainingmap = new List<List<int>>();
        public List<ICreature> players = new List<ICreature>();
        public List<ICreature> monsters = new List<ICreature>();

        private int _worldSize = 30;
        private Random _random = new Random();

        private int playerCount = 5;
        private int monsterCount = 5;

        private int maxPlayerCount = 5;
        private int maxMonsterCount = 5;

        public TrainingMapGenerator()
        {
            GenerateWorld();
        }

        private List<List<int>> GenerateWorld()
        {
            for(int i = 0; i < _worldSize; i++)
            {
                for (int j = 0; j < _worldSize; j++)
                {
                    trainingmap.Add(RandomTile(i,j));
                }
            }
            GenerateCreatures();
            return trainingmap;
        }

        private List<int> RandomTile(int x, int y)
        {
            List<int> tile = new List<int>();
            tile.Add(x);
            tile.Add(y);
            tile.Add(_random.Next(0,5));
            return tile;
        }

        private void GenerateCreatures()
        {
            switch (_random.Next(0, 10))
            {
                case 5:
                    if(monsterCount < maxMonsterCount)
                    NewMonster();
                    break;

                case 10:
                    if (playerCount < maxPlayerCount)
                    NewPlayer();
                    break;

                default:
                    break;
            }
        }

        private void NewMonster()
        {
            Vector2 location = new(_random.Next(0, 30), _random.Next(0, 30));
            
            MonsterData monsterData =
            new MonsterData
            (
                location,
                _random.NextDouble() * (5 - 20) + 5,
                _random.Next(1, 5),
                20,
                null,
                false
            );

            ICreatureStateMachine monsterStateMachine = new MonsterStateMachine(monsterData,null);

            monsters.Add(new Monster(monsterStateMachine));
            monsterCount++;
        }

        private void NewPlayer()
        {
            Vector2 location = new(_random.Next(0, 30), _random.Next(0, 30));

            MonsterData monsterData =
            new MonsterData
            (
                location,
                _random.NextDouble() * (5 - 20) + 5,
                _random.Next(1, 5),
                20,
                null,
                false
            );

            ICreatureStateMachine monsterStateMachine = new MonsterStateMachine(monsterData, null);

            monsters.Add(new Monster(monsterStateMachine));

            playerCount++;
        }

        private void AddSmartMonster()
        {
            
        }
    }
}
