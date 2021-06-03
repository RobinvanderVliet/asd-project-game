#nullable enable
using System;

namespace WorldGeneration.DatabaseFunctions
{
    public class DatabaseException : Exception
    {
        public DatabaseException()
        {
        }

        public DatabaseException(string? message) : base(message)
        {
        }
    }
}