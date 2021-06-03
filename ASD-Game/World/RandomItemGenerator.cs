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
                (> 99) => ItemFactory.GetBandage(),
                (> 98.5f) => ItemFactory.GetMorphine(),
                (> 98) => ItemFactory.GetBaseballBat(),
                (> 97.5f) => ItemFactory.GetBigMac(),
                (> 97) => ItemFactory.GetFlakVest(),
                (> 96.5f) => ItemFactory.GetGasMask(),
                (> 96) => ItemFactory.GetGlock(),
                (> 95.5f) => ItemFactory.GetHardHat(),
                (> 95) => ItemFactory.GetHazmatSuit(),
                (> 89) => ItemFactory.GetIodineTablets(),
                (> 88.5f) => ItemFactory.GetJacket(),
                (> 88) => ItemFactory.GetKatana(),
                (> 87.5f) => ItemFactory.GetTacticalVest(),
                (> 87) => ItemFactory.GetMedkit(),
                (> 86.5f) => ItemFactory.GetMilitaryHelmet(),
                (> 86) => ItemFactory.GetMonsterEnergy(),
                (> 85.5f) => ItemFactory.GetSuspiciousWhitePowder(),
                (> 85) => ItemFactory.GetP90(),
                _ => null
            };
        }
    }
}
