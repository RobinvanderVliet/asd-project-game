using System;

namespace InputCommandHandler.exception
{
    public class CommandSyntaxException : Exception
    {
        public CommandSyntaxException(string message) : base(message)
        {
        }
    }
}