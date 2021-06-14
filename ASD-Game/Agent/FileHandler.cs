using System;
using System.IO;
using System.Linq;
using ASD_Game.Agent.Exceptions;

namespace ASD_Game.Agent
{
    public class FileHandler
    {
        private string[] _allowedTypes = new[] { ".txt", ".cfg" };
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

            if (File.Exists(safeFileLocation))
            {
                File.Delete(safeFileLocation);
            }
            
            CreateDirectory(safeFileLocation);

            using (FileStream fileStream = File.Open(safeFileLocation, FileMode.OpenOrCreate))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(content);
                }
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
            string currentDirectory = string.Format(Path.GetFullPath(Path.Combine(GoBackToRoot(AppDomain.CurrentDomain.BaseDirectory))));
            string childDirectory = Directory.GetDirectories(currentDirectory, "*Agent*")[0].ToString();

            return childDirectory;
        }

        private string GoBackToRoot(string path)
        {
            return Directory.GetParent
                (Directory.GetParent
                    (Directory.GetParent
                        (Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString()).ToString()).ToString()).ToString();

        }
        
    }
}