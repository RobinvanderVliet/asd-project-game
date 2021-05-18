using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using DatabaseHandler.Poco;
using LiteDB;
using LiteDB.Async;
using Microsoft.Extensions.Logging;

namespace DatabaseHandler
{
    [ExcludeFromCodeCoverage]
    public class DbConnection : IDbConnection
    {
        private string _connectionString;

        public DbConnection()
        {
        }

        [ExcludeFromCodeCoverage]
        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public void SetForeignKeys()
        {
            var col = GetConnectionAsync().GetCollection<PlayerPoco>(GetDbName<PlayerPoco>());
            var game = GetConnectionAsync().GetCollection<MainGamePoco>(GetDbName<MainGamePoco>());
            BsonMapper.Global.Entity<PlayerPoco>()
                .DbRef(x => x.GameGuid, GetDbName<MainGamePoco>());
        }
        
        private string GetDbName<T>()
        {
            var name = typeof(T).Name + "s";
            return name;
        }

        [ExcludeFromCodeCoverage]
        public ILiteDatabaseAsync GetConnectionAsync()
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var connection = new LiteDatabaseAsync(currentDirectory + "\\Mama.db");
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