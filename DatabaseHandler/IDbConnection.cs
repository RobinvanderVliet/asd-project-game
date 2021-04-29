using System;
using LiteDB;
using LiteDB.Async;

namespace DatabaseHandler
{
    public interface IDbConnection
    {
        ILiteDatabase GetConnection();
    }
}