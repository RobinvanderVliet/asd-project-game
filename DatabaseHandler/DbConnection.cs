using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB.Async;

namespace DatabaseHandler
{
    [ExcludeFromCodeCoverage]
    public class DbConnection : IDbConnection
    {
        private string _connectionString;

        public string GetConnectionString()
        {
            return _connectionString;
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        [ExcludeFromCodeCoverage]
        public ILiteDatabaseAsync GetConnectionAsync()
        {
        }
        
        public ILiteDatabase GetConnection()
        {
            try
            {
                if (_connectionString == null)
                {
                    throw new ArgumentNullException($"Connection string is not declared?");
                }
                
                var connection = new LiteDatabaseAsync(_connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}