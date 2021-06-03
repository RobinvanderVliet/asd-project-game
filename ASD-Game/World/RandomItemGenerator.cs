using ASD_project.World.Models;
using Items;

namespace ASD_project.World
{
    public class RandomItemGenerator
    {

        public static Item GetRandomItem(float noise)
        {
            return (noise * 100) switch
            {
                (> 99.5f) => ItemFactory.GetAK47(),
                (> 98.5f) => ItemFactory.GetBandage(),
                (> 97.5f) => ItemFactory.GetMorphine(),
                (> 96.5f) => ItemFactory.GetBaseballBat(),
                (> 95.5f) => ItemFactory.GetBigMac(),
                (> 94.5f) => ItemFactory.GetFlakVest(),
                (> 93.5f) => ItemFactory.GetGasMask(),
                (> 92.5f) => ItemFactory.GetGlock(),
                (> 91.5f) => ItemFactory.GetHardHat(),
                (> 90.5f) => ItemFactory.GetHazmatSuit(),
                (> 89.5f) => ItemFactory.GetIodineTablets(),
                (> 88.5f) => ItemFactory.GetJacket(),
                (> 87.5f) => ItemFactory.GetKatana(),
                (> 86.5f) => ItemFactory.GetTacticalVest(),
                (> 85.5f) => ItemFactory.GetMedkit(),
                (> 84.5f) => ItemFactory.GetMilitaryHelmet(),
                (> 83.5f) => ItemFactory.GetMonsterEnergy(),
                (> 82.5f) => ItemFactory.GetSuspiciousWhitePowder(),
                (> 81.5f) => ItemFactory.GetP90(),
                _ => null
            };
        }
        
        public Chunk Spawn(Chunk createdChunk, float noiseresult)
        {
            var randomTile = (int) ((createdChunk.RowSize * createdChunk.RowSize - 1) * noiseresult);
            if (randomTile < 0)
            {
                randomTile *= -1;
            }
            createdChunk.Map[randomTile].ItemsOnTile.Add(GetRandomItem(noiseresult));
            return createdChunk;
        }
    }
}
