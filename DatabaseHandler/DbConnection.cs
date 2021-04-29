using System;
using LiteDB.Async;
using Microsoft.Extensions.Logging;

namespace DatabaseHandler
{
    public class DbConnection : IDbConnection
    {
        private readonly ILogger<DbConnection> _log;

        public DbConnection(ILogger<DbConnection> log)
        {
            _log = log;
        }
        
        public ILiteDatabaseAsync getConnectionASync()
        {
            try
            {
                using var connection = new LiteDatabaseAsync("Filename=.\\chunks.db;Mode=Exclusive;Async=true;");
                return connection;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }
    }
}