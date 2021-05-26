using System;
using System.Diagnostics.CodeAnalysis;

namespace DataTransfer.DTO.Character
{
    [ExcludeFromCodeCoverage]
    public class CharacterDTO : MapCharacterDTO
    {
        public int Health;
        public int Stamina;
        public int RadiationLevel;

        public InventoryDTO Inventory;
        public BitcoinDTO Bitcoins;

        public CharacterDTO(
            int xPosition,
            int yPosition,
            string playerGuid,
            string gameGuid,
            string symbol,
            ConsoleColor color = ConsoleColor.White,
            ConsoleColor backgroundColor = ConsoleColor.Black,
            int team = 0,
            int health = default,
            int stamina = default,
            int radiationLevel = default,
            InventoryDTO inventory = null,
            BitcoinDTO bitcoins = null)
            : base(
                xPosition,
                yPosition,
                playerGuid,
                gameGuid,
                symbol,
                color,
                backgroundColor,
                team)
        {
            Health = health;
            Stamina = stamina;
            RadiationLevel = radiationLevel;
            Inventory = inventory;
            Bitcoins = bitcoins;
        }
    }
}