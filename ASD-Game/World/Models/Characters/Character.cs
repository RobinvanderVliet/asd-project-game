using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ASD_Game.World.Models.Characters

{
    [ExcludeFromCodeCoverage]
    public class Character
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public string Symbol { get; set; }
        public Vector2 Destination { get; set; }

        public Character(string name, int xPosition, int yPosition, string symbol, string id)
        {
            Name = name;
            Health = 100;
            XPosition = xPosition;
            YPosition = yPosition;
            Symbol = symbol;
            Id = id;
        }

        public string GetSymbol()
        {
            if (Health <= 0)
            {
                return "X";
            }

            return Symbol;
        }
    }
}