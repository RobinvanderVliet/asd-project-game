using LiteDB.Async;

namespace DatabaseHandler
{
    public interface IDbConnection
    {
        void SetConnectionString(string connectionString);
        ILiteDatabaseAsync GetConnectionAsync();
    }
}