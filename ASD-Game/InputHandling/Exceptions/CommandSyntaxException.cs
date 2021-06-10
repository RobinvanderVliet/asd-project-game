using System;

namespace ASD_Game.InputHandling.Exceptions
{
    public class CommandSyntaxException : Exception
    {
        public CommandSyntaxException(string message) : base(message)
        {
        }
    }
}