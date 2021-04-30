using System;

namespace Player.Exceptions
{
    public class ItemException : Exception
    {
        public ItemException(string message) : base(message)
        {
        }
    }
}