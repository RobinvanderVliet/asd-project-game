using System;

namespace ASD_Game.InputHandling.Exceptions
{
    public class MoveException : Exception
    {
        public MoveException(string message) : base(message)
        {
        }
    }
}