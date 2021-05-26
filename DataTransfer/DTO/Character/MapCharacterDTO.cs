using System;
using System.Diagnostics.CodeAnalysis;

namespace DataTransfer.DTO.Character
{
    [ExcludeFromCodeCoverage]
    public class MapCharacterDTO
    {
        public int XPosition;
        public int YPosition;
        public int Steps;
        public int Stamina;
        public int RadiationLevel;
        public string PlayerGuid;
        public string GameGuid;
        public string Symbol;
        public ConsoleColor Color;
        public ConsoleColor BackgroundColor;
        public int Team;

        public MapCharacterDTO(
            int xPosition, 
            int yPosition,
            int steps,
            int stamina,
            int radiationLevel,
            string playerGuid,
            string gameGuid, 
            string symbol = null, 
            ConsoleColor color = ConsoleColor.White, 
            ConsoleColor backgroundColor = ConsoleColor.Black, 
            int team = 0)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            Steps = steps;
            Stamina = stamina;
            RadiationLevel = radiationLevel;
            PlayerGuid = playerGuid;
            GameGuid = gameGuid;
            Symbol = symbol;
            Color = color;
            BackgroundColor = backgroundColor;
            Team = team;
        }
    }
}