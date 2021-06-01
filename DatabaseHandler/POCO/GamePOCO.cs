using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class GamePOCO
    {
        public string GameGuid { get; set; }
        public string PlayerGUIDHost { get; set; }
        public int Seed { get; set; }
    }
}