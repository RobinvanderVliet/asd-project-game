using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ASD_Game.Agent.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class FileException : IOException
    {
        public FileException() { }
        public FileException(string message) : base(string.Format(message)) { }
    }
}
