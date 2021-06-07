using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.World.Models
{
    [ExcludeFromCodeCoverage]
    public static class TileSymbol 
    {
        public const string DOOR = "/";
        public const string HOUSE = "+";
        public const string WALL = "\u25A0";
        public const string GAS = "&";
        public const string SPIKE = "\u25B2";
        public const string CHEST = "n";
        public const string DIRT = ".";
        public const string GRASS = ",";
        public const string STREET = "\u2591";
        public const string WATER = "~";
    }
}