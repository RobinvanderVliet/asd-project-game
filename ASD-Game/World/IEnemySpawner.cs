using ASD_Game.World.Models.Characters;

namespace ASD_Game.World
{
    public interface IEnemySpawner
    {
        Monster spawnMonster(int x, int y, string id, int smartBrainedChance);
    }
}