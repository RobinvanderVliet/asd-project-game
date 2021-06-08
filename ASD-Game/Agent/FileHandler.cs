using Agent.Exceptions;
using System;
using System.IO;
using System.Linq;

namespace Agent
{
    public class FileHandler
    {
        private readonly string[] _allowedTypes = new[] { ".txt", ".cfg" };
        private static readonly char _separator = Path.DirectorySeparatorChar;

        public virtual string ImportFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileException("File not found!");
            }

            if (!_allowedTypes.Contains(Path.GetExtension(filepath)))
            {
                throw new FileException("The provided file is of an incorrect extension");
            }

            using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string fileData = reader.ReadToEnd();

                    reader.Close();

                    return fileData;
                }
            }
        }

        public virtual void ExportFile(string content, string fileName)
        {
            string safeFileLocation = GetBaseDirectory() + $"{_separator}Resource{_separator}{fileName}";

            CreateDirectory(safeFileLocation);

            FileStream stream = null;
            try
            {
                stream = new FileStream(safeFileLocation, FileMode.OpenOrCreate);
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    stream = null;
                    streamWriter.Write(content);
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        public void CreateDirectory(string filepath)
        {
            string directoryName = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        public string GetBaseDirectory()
        {
            string currentDirectory = string.Format(Path.GetFullPath(Path.Combine(GoBackToRoot())));
            string childDirectory = Directory.GetDirectories(currentDirectory, "*Agent*")[0].ToString();

            return childDirectory;
        }

        private string GoBackToRoot()
        {
            return Directory.GetParent
                (Directory.GetParent
                    (Directory.GetParent
                        (Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString()).ToString()).ToString()).ToString();

        }
    }
}