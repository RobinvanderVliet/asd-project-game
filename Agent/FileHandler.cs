using Agent.Exceptions;
using Serilog;
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
        public virtual string ImportFile(string filepath)
        {
            try
            {
                if (Path.GetExtension(filepath).Equals(".txt") || Path.GetExtension(filepath).Equals(".cfg"))
                {
                    using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            string fileData = reader.ReadToEnd();

                            reader.Close();

                            return fileData;
                        };
                    }
                }
                else 
                {
                    throw new FileException("File given is not of the correct file type");
                }
            }
            catch (FileException e)
            {
                Log.Logger.Information(e.Message);
                return null;
            }
            
        }

        public void ExportFile(string content, string fileName)
        {
            string safeFileLocation = String.Format(Path.GetFullPath(Path.Combine
                        (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\" + fileName;

            CreateDirectory(safeFileLocation);

            using (FileStream fileStream = new FileStream(safeFileLocation, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(content);
                };   
            };
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


