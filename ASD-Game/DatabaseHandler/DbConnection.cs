using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using LiteDB.Async;

namespace ASD_Game.DatabaseHandler
{
    [ExcludeFromCodeCoverage]
    public class DbConnection : IDbConnection
    {
        private static readonly char Separator = Path.DirectorySeparatorChar;
        
        public ILiteDatabaseAsync GetConnectionAsync()
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var connection =
                    new LiteDatabaseAsync($"Filename={currentDirectory}{Separator}ASD-Game.db;connection=shared;");
                return connection;
            }
            catch (LiteAsyncException ex)
            {
                Console.WriteLine("[{0}][{1}] Source: {2}, Message: {3}\r\n[StackTrace] {4}", DateTime.Now.ToString(new CultureInfo("nl-NL")), GetType().Name, ex.Source, ex.Message, ex.StackTrace);
                throw new LiteAsyncException("Exception thrown in DBConnection.", ex);
            }
        }
    }
}