using System;

namespace WorldGeneration
{
    public class DatabaseError : Exception
    {
        public DatabaseError(string? message) : base(message)
        {
        }
    }
}