using LiteDB.Async;

namespace ASD_Game.DatabaseHandler
{
    public interface IDbConnection
    {
        public ILiteDatabaseAsync GetConnectionAsync();
    }
}