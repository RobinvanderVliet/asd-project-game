using System;
using System.Diagnostics.CodeAnalysis;
using DatabaseHandler.Poco;
using LiteDB;
using LiteDB.Async;
using Microsoft.Extensions.Logging;

namespace DatabaseHandler
{
    [ExcludeFromCodeCoverage]
    public class DbConnection : IDbConnection
    {
        private readonly ILogger<DbConnection> _log;
        private string _connectionString;

        public DbConnection()
        {
            //moet nog toegevoegd worden als wij dit willen?
        //    _log = log;
        }

        [ExcludeFromCodeCoverage]
        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SetForeignKeys()
        {
            using (var db = new LiteDatabase(@"C:\\Temp\\MyData.db"))
            {
                var col = db.GetCollection<PlayerPoco>(GetDbName<PlayerPoco>());
                var game = db.GetCollection<MainGamePoco>(GetDbName<MainGamePoco>());

                LiteDB.BsonMapper.Global.Entity<PlayerPoco>()
                    .DbRef(x => x.GameGuid, GetDbName<MainGamePoco>());
            }
        }

        [ExcludeFromCodeCoverage]
        public ILiteDatabaseAsync GetConnectionAsync()
        {
            try
            {
                if (_connectionString == null) throw new ArgumentNullException("Connection string is not declared?");

                var connection = new LiteDatabaseAsync(_connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {Message}", ex.Message);
                throw;
            }
        }
        
        private string GetDbName<T>()
         {
             var name = typeof(T).Name + "s";
             return name;
         }
    }
}