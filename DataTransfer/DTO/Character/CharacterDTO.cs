using System;

namespace DataTransfer.DTO.Character
{
    public class CharacterDTO : MapCharacterDTO
    {
        public int Health;
        public int Stamina;
        public int RadiationLevel;

        public InventoryDTO Inventory;
        public BitcoinDTO Bitcoins;
        
        public CharacterDTO(int xPosition
            , int yPosition
            , string symbol
            , string name
            , ConsoleColor color = ConsoleColor.White
            , ConsoleColor backgroundColor = ConsoleColor.Black
            , int team = 0
            , int health = default
            , int stamina = default
            , int radiationLevel = default
            , InventoryDTO inventory = null
            , BitcoinDTO bitcoins = null) : base(xPosition
            , yPosition
            , symbol
            , name
            , color
            , backgroundColor
            , team)
        {
            Name = name;
            Health = health;
            Stamina = stamina;
            RadiationLevel = radiationLevel;
            Inventory = inventory;
            Bitcoins = bitcoins;
        }
    }
}