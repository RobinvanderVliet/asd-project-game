using LiteDB.Async;

namespace DatabaseHandler
{
    public interface IDbConnection
    {
        ILiteDatabaseAsync GetConnectionAsync();
    }
}