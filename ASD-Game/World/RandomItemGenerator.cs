using Items;
using System;
using System.Collections.Generic;

namespace WorldGeneration
{
    public class RandomItemGenerator
    {

        public Item GetRandomItem(float noise)
        {
            return (noise * 100) switch
            {
                (< 92 and >= 88) => ItemFactory.GetAK47(),
                (< 88 and >= 77) => ItemFactory.GetBandage(),
                (< 77 and >= 70) => ItemFactory.GetMorphine(),
                (< 66 and >= 55) => ItemFactory.GetBaseballBat(),
                (< 55 and >= 44) => ItemFactory.GetBigMac(),
                (< 44 and >= 37) => ItemFactory.GetFlakVest(),
                (< 33 and >= 22) => ItemFactory.GetGasMask(),
                (< 22 and >= 11) => ItemFactory.GetGlock(),
                (< 11 and >= 0) => ItemFactory.GetHardHat(),
                (< 0 and >= -11) => ItemFactory.GetHazmatSuit(),
                (< -11 and >= -22) => ItemFactory.GetIodineTablets(),
                (< -22 and >= -33) => ItemFactory.GetJacket(),
                (< -33 and >= -38) => ItemFactory.GetKatana(),
                (< -44 and >= -49) => ItemFactory.GetTacticalVest(),
                (< -55 and >= -60) => ItemFactory.GetMedkit(),
                (< -66 and >= -71) => ItemFactory.GetMilitaryHelmet(),
                (< -77 and >= -84) => ItemFactory.GetMonsterEnergy(),
                (< -88 and >= -92) => ItemFactory.GetSuspiciousWhitePowder(),
                (< -92 and >= -99) => ItemFactory.GetP90(),
                _ => null
            };
        }
    }
}
