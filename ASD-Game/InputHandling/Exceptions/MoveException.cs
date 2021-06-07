using System;

namespace ASD_project.InputHandling.Exceptions
{
    public class MoveException : Exception
    {
        public MoveException(string message) : base(message)
        {
        }
    }
}