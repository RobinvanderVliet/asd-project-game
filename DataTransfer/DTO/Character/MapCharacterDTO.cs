using System;
using LiteDB;

namespace DataTransfer.DTO.Character
{
    public class MapCharacterDTO
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public string PlayerGUID { get; set; }

        public string Symbol { get; set; }

        public string GameGuid { get; set; }

        [BsonIgnore]
        public ConsoleColor Color;
        public ConsoleColor BackgroundColor;
        public int Team;

        public MapCharacterDTO(int xPosition
            , int yPosition
            , string playerGuid
            , string symbol = null
            , ConsoleColor color = ConsoleColor.White
            , ConsoleColor backgroundColor = ConsoleColor.Black
            , int team = 0)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            PlayerGUID = playerGuid;
            Symbol = symbol;
            Color = color;
            BackgroundColor = backgroundColor;
            Team = team;
        }

        public MapCharacterDTO()
        {
            
        }
    }
}