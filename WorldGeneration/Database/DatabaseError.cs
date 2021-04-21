#nullable enable
using System;

namespace WorldGeneration.Database
{
    public class DatabaseError : Exception
    {
        public DatabaseError(string? message) : base(message)
        {
        }
    }
}