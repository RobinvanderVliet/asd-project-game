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
                .DbRef(x => x.GameGUID, nameof(GamePOCO));

            //FK Game -> Player
            BsonMapper.Global.Entity<GamePOCO>()
                .DbRef(x => x.PlayerGUIDHost, nameof(PlayerPOCO));

            //FK Agent -> Game
            BsonMapper.Global.Entity<AgentPoco>()
                .DbRef(x => x.GameGUID, nameof(GamePOCO));

            //FK Agent -> Player
            BsonMapper.Global.Entity<AgentPoco>()
                .DbRef(x => x.PlayerGUID, nameof(PlayerPOCO));

            //FK PlayerItem -> Player
            BsonMapper.Global.Entity<PlayerItemPoco>()
                .DbRef(x => x.PlayerGUID, nameof(PlayerPOCO));

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