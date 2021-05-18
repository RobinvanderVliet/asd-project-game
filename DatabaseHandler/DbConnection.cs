using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using DatabaseHandler.Poco;
using LiteDB;
using LiteDB.Async;

namespace DatabaseHandler
{
    [ExcludeFromCodeCoverage]
    public class DbConnection : IDbConnection
    {

        [ExcludeFromCodeCoverage]
        public void SetForeignKeys()
        {
            BsonMapper.Global.Entity<PlayerPoco>()
                .DbRef(x => x.GameGuid, nameof(MainGamePoco));
        }

        [ExcludeFromCodeCoverage]
        public ILiteDatabaseAsync GetConnectionAsync()
        {
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