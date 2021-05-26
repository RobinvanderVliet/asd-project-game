using System;
using Player.Exceptions;
using WorldGeneration.Models;

namespace Player.Model
{
    public class PlayerModel : IPlayerModel
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }

        private string _PlayerGuid;
        public string PlayerGuid { get => _PlayerGuid; set => _PlayerGuid = value; }
        
        private int _health;
        public int Health { get => _health; set => _health = value; }
        private int _stamina;
        public int Stamina { get => _stamina; set => _stamina = value; }
        
        private IInventory _inventory;
        public IInventory Inventory { get => _inventory; set => _inventory = value; }
        private IBitcoin _bitcoins;
        public IBitcoin Bitcoins { get => _bitcoins; set => _bitcoins = value; }
        private IRadiationLevel _radiationLevel;
        public IRadiationLevel RadiationLevel { get => _radiationLevel; set => _radiationLevel = value; }

        private int _xPosition;
        private int _yPosition;
        public int XPosition { get => _xPosition; set => _xPosition = value; }
        public int YPosition { get => _yPosition; set => _yPosition = value; }
        
        private string _symbol = CharacterSymbol.CURRENT_PLAYER;
        public string Symbol { get => _symbol; set => _symbol = value; }
        public ConsoleColor Color { get; set; }
        public int Team { get; set; }

        //random default values for health&stamina for now
        private const int HEALTHCAP = 100;
        private const int STAMINACAP = 10;
        
        public PlayerModel(string name, IInventory inventory, IBitcoin bitcoins, IRadiationLevel radiationLevel)
        {
            _name = name;
            _health = HEALTHCAP;
            _stamina = STAMINACAP;
            _inventory = inventory;
            _bitcoins = bitcoins;
            _radiationLevel = radiationLevel;
        }
    }
}