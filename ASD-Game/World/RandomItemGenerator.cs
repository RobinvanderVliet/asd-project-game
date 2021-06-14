using ASD_Game.Items;

namespace ASD_Game.World
{
    public class RandomItemGenerator : IRandomItemGenerator
    {

        public Item GetRandomItem(float noise, int chanceThereIsAItem)
        {
            var amountOfItems = 19;
            var chancePerItem = (float)chanceThereIsAItem / amountOfItems;
            for (int i = 0; i < amountOfItems; i++)
            {
                if (noise * 100 > 100 - i * chancePerItem)
                {
                    return GetItemForIndex(i);
                }
            }
            return null;
        }

        private Item GetItemForIndex(int index)
        {
            return (index) switch
            {
                (< 1) => ItemFactory.GetAK47(),
                (< 2) => ItemFactory.GetBandage(),
                (< 3) => ItemFactory.GetMorphine(),
                (< 4) => ItemFactory.GetBaseballBat(),
                (< 5) => ItemFactory.GetBigMac(),
                (< 6) => ItemFactory.GetFlakVest(),
                (< 7) => ItemFactory.GetGasMask(),
                (< 8) => ItemFactory.GetGlock(),
                (< 9) => ItemFactory.GetHardHat(),
                (< 10) => ItemFactory.GetHazmatSuit(),
                (< 11) => ItemFactory.GetIodineTablets(),
                (< 12) => ItemFactory.GetJacket(),
                (< 13) => ItemFactory.GetKatana(),
                (< 14) => ItemFactory.GetTacticalVest(),
                (< 15) => ItemFactory.GetMedkit(),
                (< 16) => ItemFactory.GetMilitaryHelmet(),
                (< 17) => ItemFactory.GetMonsterEnergy(),
                (< 18) => ItemFactory.GetSuspiciousWhitePowder(),
                (< 19) => ItemFactory.GetP90(),
                _ => null
            };
        }
    }
}
