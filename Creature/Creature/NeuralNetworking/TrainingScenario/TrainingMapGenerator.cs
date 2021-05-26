using Agent.Mapper;
using Agent.Services;
using Agent.Models;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.CustomRuleSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Creature.NeuralNetworking.TrainingScenario
{
    public class TrainingMapGenerator
    {
        public List<List<int>> trainingmap = new List<List<int>>();
        public List<ICreature> players = new List<ICreature>();
        public List<ICreature> monsters = new List<ICreature>();

        private int _worldSize = 30;
        private Random _random = new Random();
        private int maxPlayerCount = 5;
        private int maxMonsterCount = 5;


        public List<List<int>> GenerateWorld()
        {
            for(int i = 0; i < _worldSize; i++)
            {
                for (int j = 0; j < _worldSize; j++)
                {
                    trainingmap.Add(RandomTile(i,j));

                }
            }
            return null;
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
                    NewMonster();
                    break;

                case 10:
                    NewPlayer();
                    break;

                default:
                    break;
            }
        }

        private void NewMonster()
        {
            Vector2 location = new(_random.Next(0, 30), _random.Next(0, 30);
            StateMachine.Data.MonsterData monsterData =
            new StateMachine.Data.MonsterData(
                location,
                _random.NextDouble() * (5 - 20) + 5,
                _random.Next(1, 5),
                20,
                null,
                false
                );
            monsters.Add(new Monster(monsterData, monsterRuleSet));
        }

        private void NewPlayer()
        {

        }
    }
}
