using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ASD_Game.DatabaseHandler.POCO;
using LiteDB;
using LiteDB.Async;

namespace ASD_Game.DatabaseHandler
{
    [ExcludeFromCodeCoverage]
    public class DBConnection : IDBConnection
    {
        private static readonly char _separator = Path.DirectorySeparatorChar;

        public void SetForeignKeys()
        {
            //FK Player -> Game
            BsonMapper.Global.Entity<PlayerPOCO>()
                .DbRef(x => x.GameGUID, nameof(GamePOCO));

            //FK Game -> Player
            BsonMapper.Global.Entity<GamePOCO>()
                .DbRef(x => x.PlayerGUIDHost, nameof(PlayerPOCO));

            //FK Agent -> Game
            BsonMapper.Global.Entity<AgentPOCO>()
                .DbRef(x => x.GameGUID, nameof(GamePOCO));

            //FK Agent -> Player
            BsonMapper.Global.Entity<AgentPOCO>()
                .DbRef(x => x.PlayerGUID, nameof(PlayerPOCO));

            //FK PlayerItem -> Player
            BsonMapper.Global.Entity<PlayerItemPOCO>()
                .DbRef(x => x.PlayerGUID, nameof(PlayerPOCO));

            //FK PlayerItem -> Item
            BsonMapper.Global.Entity<PlayerItemPOCO>()
                .DbRef(x => x.ItemName, nameof(PlayerItemPOCO));

            //FK Game -> WorldItem
            BsonMapper.Global.Entity<GamePOCO>()
                .DbRef(x => x.GameGUID, nameof(WorldItemPOCO));

            //FK WorldItem -> Item
            BsonMapper.Global.Entity<WorldItemPOCO>()
                .DbRef(x => x.ItemName, nameof(PlayerItemPOCO));
        }

        [ExcludeFromCodeCoverage]
        public ILiteDatabaseAsync GetConnectionAsync()
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var connection =
                    new LiteDatabaseAsync($"Filename={currentDirectory}{_separator}ASD-Game.db;connection=shared;");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {Message}", ex.Message);
                throw;
            }
        }
    }
}