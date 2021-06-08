using ASD_Game.Creature.Creature;

namespace ASD_project.World
{
    public static class RandomEnemyGenerator
    {

        public static Monster GetRandomEnemy(float noise, int coordinateX, int coordinateY)
        {
            switch (noise %2)
            {
                case 0:
                    return null;
                    // return new Monster("Zombie", coordinateX, coordinateY, CharacterSymbol.TERMINATOR, "monst");
                case 1:
                    // return new new SmartMonster("Zombie", coordinateX, coordinateY, CharacterSymbol.TERMINATOR, "monst", new DataGatheringService(_worldService));
                    // Let op! Dit zorgt mogelijk voor een circular dependency!
                    return null;
                default:
                    return null;
            }
        }
    }
}
