using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Items;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World.Models.HazardousTiles
{
    [ExcludeFromCodeCoverage]
    public class SpikeTile : IHazardousTile
    {
        public bool IsAccessible { get; set; }
        public string Symbol { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int StaminaCost { get; set; }
        public List<Item> ItemsOnTile { get; set; }

        public SpikeTile(int x, int y)
        {
            ItemsOnTile = new();
            Symbol = TileSymbol.SPIKE;
            IsAccessible = true;
            XPosition = x;
            YPosition = y;
            StaminaCost = 5;
            Damage = new Random().Next(2, 11);
            ItemsOnTile = new List<Item>();
        }

        public int Damage { get; set; }
        

        public int GetDamage(int time)
        {
            return Damage;
        }
    }
}