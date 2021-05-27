using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using LiteDB.Async;

namespace DatabaseHandler
{
    public class DbConnection : IDbConnection
    {
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