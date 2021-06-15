using System;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;

namespace ASD_Game.World
{
    public class EnemySpawner : IEnemySpawner
    {
        public Monster spawnMonster(int x, int y, string id, int smartBrainedChance)
        {
            if (new Random().Next(0, 100) < smartBrainedChance)
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