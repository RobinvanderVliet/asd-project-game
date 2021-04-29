using System;
using LiteDB;
using LiteDB.Async;
using Microsoft.Extensions.Logging;

namespace DatabaseHandler
{
    public class DbConnection : IDbConnection
    {
        private readonly ILogger<DbConnection> _log;
        private readonly string _connectionString;

        public DbConnection(ILogger<DbConnection> log, string connectionString)
        {
            _log = log;
            _connectionString = connectionString;
        }
        
        public ILiteDatabaseAsync getConnectionASync()
        {
            try
            {
                var connection = new LiteDatabaseAsync(_connectionString);
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