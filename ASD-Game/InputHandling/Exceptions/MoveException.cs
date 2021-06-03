using System;

namespace InputHandling.Exceptions
{
    public class MoveException : Exception
    {
        public MoveException(string message) : base(message)
        {
        }
    }
}