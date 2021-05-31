using System.Diagnostics.CodeAnalysis;

namespace WorldGeneration
{
    [ExcludeFromCodeCoverage]
    public class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public string Symbol { get; set; }

        public Character(string name, int xPosition, int yPosition, string symbol)
        {
            Name = name;
            Health = 100;
            XPosition = xPosition;
            YPosition = yPosition;
            Symbol = symbol;
        }
    }
}