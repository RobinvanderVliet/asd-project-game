using System;

namespace InputCommandHandler.exception
{
    public class MoveException : Exception
    {
        public MoveException(string message) : base(message)
        {
        }
    }
}