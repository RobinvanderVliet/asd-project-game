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
                (< 66 and >= 55) => ItemFactory.GetBaseballBat(),
                (< 44 and >= 37) => ItemFactory.GetFlakVest(),
                (< 22 and >= 11) => ItemFactory.GetGlock(),
                (< 11 and >= 0) => ItemFactory.GetHardHat(),
                (< -22 and >= -33) => ItemFactory.GetJacket(),
                (< -33 and >= -38) => ItemFactory.GetKatana(),
                (< -66 and >= -71) => ItemFactory.GetMilitaryHelmet(),
                (< -92 and >= -99) => ItemFactory.GetP90(),
                _ => null
            };
        }
    }
}