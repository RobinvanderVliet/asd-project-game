using System;

namespace InputHandling.Exceptions
{
    public class SlotException : Exception
    {
        public SlotException(string message) : base(message)
        {
        }
    }
}