using LiteDB.Async;

namespace DatabaseHandler
{
    public interface IDbConnection
    {
        public ILiteDatabaseAsync GetConnectionAsync();
    }
}