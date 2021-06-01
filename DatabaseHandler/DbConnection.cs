using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using DatabaseHandler.POCO;
using LiteDB;
using LiteDB.Async;

namespace DatabaseHandler
{
    public class DbConnection : IDbConnection
    {


        [ExcludeFromCodeCoverage]
        public void SetForeignKeys()
        {
            BsonMapper.Global.Entity<PlayerPOCO>()
                .DbRef(x => x.GameGuid, nameof(GamePOCO));
        }
        [ExcludeFromCodeCoverage]
        public ILiteDatabaseAsync GetConnectionAsync()
        {
            
            SetForeignKeys();
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var connection = new LiteDatabaseAsync(@"Filename="  + currentDirectory + "\\ASD-Game.db;connection=shared;");
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