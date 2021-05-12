using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Exceptions
{
    [Serializable]
    public class FileException : IOException
    {
        public FileException() { }
        public FileException(String message) : base(String.Format(message)) { }
    }
}
