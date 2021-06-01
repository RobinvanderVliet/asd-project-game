using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class ItemPOCO
    {
        public string ItemName { get; set; }
        public int ItemType { get; set; }

    }
}
