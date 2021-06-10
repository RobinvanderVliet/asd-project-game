using LiteDB.Async;

namespace ASD_Game.DatabaseHandler
{
    public interface IDBConnection
    {
        public ILiteDatabaseAsync GetConnectionAsync();
    }
}