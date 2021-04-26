using System;

namespace Chat.exception
{
    public class CommandSyntaxException : Exception
    {
        public CommandSyntaxException(string message) : base(message)
        {
        }
    }
}