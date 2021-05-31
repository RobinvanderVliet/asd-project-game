using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using DatabaseHandler.POCO;
using LiteDB;
using LiteDB.Async;

namespace DatabaseHandler
{
    [ExcludeFromCodeCoverage]
    public class DBConnection : IDBConnection
    {
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
                .DbRef(x => x.GameGUID, nameof(GamePoco));

            //FK Agent -> Player
            BsonMapper.Global.Entity<AgentPOCO>()
                .DbRef(x => x.PlayerGUID, nameof(PlayerPoco));

            //FK PlayerItem -> Player
            BsonMapper.Global.Entity<PlayerItemPoco>()
                .DbRef(x => x.PlayerGUID, nameof(PlayerPoco));

            //FK PlayerItem -> Item
            BsonMapper.Global.Entity<PlayerItemPoco>()
                .DbRef(x => x.ItemName, nameof(ItemPoco));

            //FK Game -> WorldItem
            BsonMapper.Global.Entity<GamePOCO>()
                .DbRef(x => x.GameGUID, nameof(WorldItemPoco));

            //FK WorldItem -> Item
            BsonMapper.Global.Entity<WorldItemPoco>()
                .DbRef(x => x.ItemName, nameof(ItemPoco));
        }
        [ExcludeFromCodeCoverage]
        public ILiteDatabaseAsync GetConnectionAsync()
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var connection =
                    new LiteDatabaseAsync(@"Filename=" + currentDirectory + "\\ASD-Game.db;connection=shared;");
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