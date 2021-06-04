using System;

namespace ASD_project.InputHandling.Exceptions
{
    public class CommandSyntaxException : Exception
    {
        public CommandSyntaxException(string message) : base(message)
        {
        }
    }
}