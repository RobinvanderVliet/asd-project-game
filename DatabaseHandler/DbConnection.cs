using System;
using System.Diagnostics.CodeAnalysis;
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

        public DbConnection(ILogger<DbConnection> log)
        {
            _log = log;
        }

        [ExcludeFromCodeCoverage]
        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        [ExcludeFromCodeCoverage]
        public ILiteDatabaseAsync GetConnectionAsync()
        {
            try
            {
                if (_connectionString == null)
                {
                    throw new ArgumentNullException($"Connection string is not declared. Please check the connection string.");
                }

                var connection = new LiteDatabaseAsync(_connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {Message}", ex.Message);
                throw;
            }
        }
    }
}