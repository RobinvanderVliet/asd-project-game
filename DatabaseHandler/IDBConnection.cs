using LiteDB.Async;

namespace DatabaseHandler
{
    public interface IDBConnection
    {
        public ILiteDatabaseAsync GetConnectionAsync();
        public void SetForeignKeys();
    }
}