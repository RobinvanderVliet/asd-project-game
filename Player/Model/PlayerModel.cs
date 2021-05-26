using System;
using WorldGeneration.Models;

namespace Player.Model
{
    public class PlayerModel : IPlayerModel
    {
        public string Name { get; set; }
        public string PlayerGuid { get; set; }
        public int Health { get; set; }
        public int Stamina { get; set; }
        public IInventory Inventory { get; set; }
        public IBitcoin Bitcoins { get; set; }
        public IRadiationLevel RadiationLevel { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public string Symbol { get; set; } = CharacterSymbol.CURRENT_PLAYER;
        public ConsoleColor Color { get; set; }
        public int Team { get; set; }

        //random default values for health&stamina for now
        private const int HEALTHCAP = 100;
        private const int STAMINACAP = 10;
        
        public PlayerModel(string name, IInventory inventory, IBitcoin bitcoins, IRadiationLevel radiationLevel)
        {
            Name = name;
            Health = HEALTHCAP;
            Stamina = STAMINACAP;
            Inventory = inventory;
            Bitcoins = bitcoins;
            RadiationLevel = radiationLevel;
        }
    }
}