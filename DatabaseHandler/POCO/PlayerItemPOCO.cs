using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class PlayerItemPOCO
    {
        public string GameGuid { get; set; }
        public string PlayerGUID { get; set; }
        public string ItemName { get; set; }
    }
}
