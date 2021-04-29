using System;

namespace InputCommandHandler.Exceptions
{
    public class MoveException : Exception
    {
        public MoveException(string message) : base(message)
        {
        }
    }
}