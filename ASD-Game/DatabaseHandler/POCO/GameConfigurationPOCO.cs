using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace ASD_Game.DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class GameConfigurationPOCO
    {
        [BsonId]
        public string GameGUID { get; set; }

        public int NPCDifficultyCurrent { get; set; }
        public int NPCDifficultyNew { get; set; }
        public int ItemSpawnRate { get; set; }
    }
}