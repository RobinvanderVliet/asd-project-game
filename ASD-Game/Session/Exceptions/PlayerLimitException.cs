using System;

namespace ASD_Game.Session.Exceptions
{
    public class PlayerLimitException : Exception
    {
        public PlayerLimitException(string message) : base(message)
        {
        }
    }
}