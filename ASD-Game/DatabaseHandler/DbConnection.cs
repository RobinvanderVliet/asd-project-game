using DatabaseHandler.POCO;
using LiteDB;
using LiteDB.Async;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace DatabaseHandler
{
    public class DbConnection : IDbConnection
    {
       public void SetForeignKeys()
        {
            //FK Player -> Game
            BsonMapper.Global.Entity<PlayerPOCO>()
                .DbRef(x => x.GameGuid, nameof(GamePOCO));

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
                .DbRef(x => x.ItemName, nameof(ItemPOCO));

            //FK Game -> WorldItem
            BsonMapper.Global.Entity<GamePOCO>()
                .DbRef(x => x.GameGuid, nameof(WorldItemPOCO));

            //FK WorldItem -> Item
            BsonMapper.Global.Entity<WorldItemPOCO>()
                .DbRef(x => x.ItemName, nameof(ItemPOCO));
        }
        [ExcludeFromCodeCoverage]
        public ILiteDatabaseAsync GetConnectionAsync()
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var connection = new LiteDatabaseAsync(@"Filename=" + currentDirectory + "\\ASD-Game.db;connection=shared;");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                throw;
            }
        }
    }
}