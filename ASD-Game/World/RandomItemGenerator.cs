using ASD_Game.Items;

namespace ASD_Game.World
{
    public class RandomItemGenerator : IRandomItemGenerator
    {

        public Item GetRandomItem(float noise)
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
                (> 94.5f) => ItemFactory.GetIodineTablets(),
                (> 94) => ItemFactory.GetJacket(),
                (> 93.5f) => ItemFactory.GetKatana(),
                (> 93) => ItemFactory.GetTacticalVest(),
                (> 92.5f) => ItemFactory.GetMedkit(),
                (> 92) => ItemFactory.GetMilitaryHelmet(),
                (> 91.5f) => ItemFactory.GetMonsterEnergy(),
                (> 91) => ItemFactory.GetSuspiciousWhitePowder(),
                (> 90.5f) => ItemFactory.GetP90(),
                _ => null
            };
        }
    }
}
