using System;

namespace ASD_Game.World.Models.Characters.Exceptions
{
    public class InventoryFullException : Exception
    {
        public InventoryFullException(string message) : base(message)
        {
        }
    }
}