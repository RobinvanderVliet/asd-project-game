using System;

namespace Player.Model
{
    public interface IPlayerModel
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
        
        public string Symbol { get; set; }
        public ConsoleColor Color { get; set; }
        public int Team { get; set; }
    }
}