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
            if (Path.GetExtension(filepath) != ".txt" || Path.GetExtension(filepath) != ".cfg") 
            {
                //TODO throw exception
            }


            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fileStream);

            try
            {
                string fileData = reader.ReadToEnd();

                reader.Close();

                return fileData;
            }
            //TODO add version to work with exception mapper
            catch (Exception)
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

                FileStream fileStream = new FileStream(safeFileLocation, FileMode.Create, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(fileStream);

                //TODO change to write with \n
                streamWriter.Write(content);

                streamWriter.Close();
            }
            //TODO change to work with exception handler
            catch (Exception)
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
