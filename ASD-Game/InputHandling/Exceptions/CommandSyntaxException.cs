using System;

namespace InputHandling.Exceptions
{
    public class CommandSyntaxException : Exception
    {
        public CommandSyntaxException(string message) : base(message)
        {
        }
    }
}