using System;

namespace Chat.exception
{
    public class MoveException : Exception
    {
        public MoveException(string message) : base(message)
        {
        }
    }
}