using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.Poco
{
    [ExcludeFromCodeCoverage]
    public class MainGamePoco
    {
        [BsonId] public Guid MainGameGuid { get; set; }
    }
}