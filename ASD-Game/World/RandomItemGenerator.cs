using Items;
using System;
using System.Collections.Generic;

namespace WorldGeneration
{
    public class RandomItemGenerator : IRandomItemGenerator
    {
        private static Item GetRandomItem()
        {
            var itemNumber = GetRandomItemNumber();
            return (itemNumber) switch
            {
                (1) => ItemFactory.GetAK47(),
                (2) => ItemFactory.GetBandage(),
                (3) => ItemFactory.GetBandana(),
                (4) => ItemFactory.GetBaseballBat(),
                (5) => ItemFactory.GetBigMac(),
                (6) => ItemFactory.GetFlakVest(),
                (7) => ItemFactory.GetGasMask(),
                (8) => ItemFactory.GetGlock(),
                (9) => ItemFactory.GetHardHat(),
                (10) => ItemFactory.GetHazmatSuit(),
                (11) => ItemFactory.GetIodineTablets(),
                (12) => ItemFactory.GetJacket(),
                (13) => ItemFactory.GetKatana(),
                (14) => ItemFactory.GetKnife(),
                (15) => ItemFactory.GetMedkit(),
                (16) => ItemFactory.GetMilitaryHelmet(),
                (17) => ItemFactory.GetMonsterEnergy(),
                (18) => ItemFactory.GetMorphine(),
                (19) => ItemFactory.GetP90(),
                (20) => ItemFactory.GetSuspiciousWhitePowder(),
                (21) => ItemFactory.GetTacticalVest(),
                _ => ItemFactory.GetKatana()
            };

        }

        public List<Item> GetRandomItems()
        {
            List<Item> items = new List<Item>();
            int amount = GetItemAmount();
            for(int i = 0; i < amount; i++ )
            {
                var randomItem = GetRandomItem();
                items.Add(randomItem);
            }
            return items;
        }

        private static int GetRandomItemNumber()
        {
            Random random = new Random();
            return random.Next(1, 22);
        }

        private static int GetItemAmount()
        {
            Random random = new Random();
            return random.Next(1, 3);
        }

    }
}
