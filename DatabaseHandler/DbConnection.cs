using System;
using LiteDB;

namespace DatabaseHandler
{
    public class DbConnection : IDbConnection
    {
        public DbConnection()
        {
        }
        
        public ILiteDatabase GetConnection()
        {
            try
            {
                using var connection = new LiteDatabase("Filename=.\\chunks.db;Mode=Exclusive;");
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