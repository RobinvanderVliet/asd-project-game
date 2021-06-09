using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ASD_Game.DatabaseHandler.POCO;
using LiteDB;
using LiteDB.Async;

namespace ASD_Game.DatabaseHandler
{
    [ExcludeFromCodeCoverage]
    public class DBConnection : IDBConnection
    {
        private static readonly char _separator = Path.DirectorySeparatorChar;

        [ExcludeFromCodeCoverage]
        public ILiteDatabaseAsync GetConnectionAsync()
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var connection =
                    new LiteDatabaseAsync($"Filename={currentDirectory}{_separator}ASD-Game.db;connection=shared;");
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