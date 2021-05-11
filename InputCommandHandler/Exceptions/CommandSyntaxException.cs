using System;

namespace InputCommandHandler.Exceptions
{
    public class CommandSyntaxException : Exception
    {
        public CommandSyntaxException(string message) : base(message)
        {
        }
    }
}