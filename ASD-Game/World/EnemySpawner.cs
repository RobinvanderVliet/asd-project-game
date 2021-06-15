using System;
using System.Buffers;
using ASD_Game.Session;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Services;

namespace ASD_Game.World
{
    public class EnemySpawner : IEnemySpawner
    {
        public Monster spawnMonster(int x, int y, string id, int smartBrainedChance)
        {
            if (new Random().Next() < smartBrainedChance)
            {
                SmartMonster monster = new SmartMonster("Gerard Gerardsen", x, y, CharacterSymbol.TERMINATOR, id);
                return monster;
            }
            else
            {
                Monster monster = new Monster("George Clooney", x, y, CharacterSymbol.ZOMBIE, id);
                monster.MonsterData.CharacterId = id;
                return monster;
            }
        }
    }
}