using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent
{
    public class FileHandler
    {
        public string ImportFile(string filepath)
        {
            try
            {
                if (Path.GetExtension(filepath) != ".txt" || Path.GetExtension(filepath) != ".cfg")
                {
                    throw new FileException("File given is not of the correct file type");
                }

                using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string fileData = reader.ReadToEnd();
                        return fileData;
                    };
                }
            }
            catch (FileException)
            { 
                throw;
            }
            
        }

        public void ExportFile(string content)
        {
            //TODO change file name to selected by user
            string tmpFileName = "agentFile.cfg";
            string safeFileLocation = Path.Combine(Environment.CurrentDirectory, @"resource\", tmpFileName);

            try
            {
                CreateDirectory(safeFileLocation);

                using (FileStream fileStream = new FileStream(safeFileLocation, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(content);
                    };   
                };
            }
            //TODO change to work with exception handler
            catch (FileException)
            {
                throw;
            }
        }
        public void CreateDirectory(string filepath)
        {
            string directoryName = Path.GetDirectoryName(filepath);

            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(directoryName);
            }

            if (!File.Exists(filepath))
            {
                File.Create(filepath);
            }
        }
    }
}

[Serializable]
public class FileException : IOException
{
    public FileException() { }
    public FileException(String massage) : base(String.Format(massage)) { }
}
