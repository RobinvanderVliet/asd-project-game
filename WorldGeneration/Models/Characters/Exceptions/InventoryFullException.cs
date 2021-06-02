using System;

namespace WorldGeneration.Exceptions
{
    public class InventoryFullException : Exception
    {
        public InventoryFullException(string message) : base(message)
        {
        }
    }
}