using LiteDB.Async;

namespace ASD_project.DatabaseHandler
{
    public interface IDBConnection
    {
        public ILiteDatabaseAsync GetConnectionAsync();
    }
}