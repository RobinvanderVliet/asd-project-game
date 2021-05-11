using System;
using LiteDB;
using LiteDB.Async;

namespace DatabaseHandler
{
    public interface IDbConnection
    {
        string GetConnectionString();
        void SetConnectionString(string connectionString);
        ILiteDatabaseAsync GetConnectionAsync();
    }
}