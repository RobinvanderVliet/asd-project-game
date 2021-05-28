using System;

namespace InputCommandHandler.Exceptions
{
    public class SlotException : Exception
    {
        public SlotException(string message) : base(message)
        {
        }
    }
}