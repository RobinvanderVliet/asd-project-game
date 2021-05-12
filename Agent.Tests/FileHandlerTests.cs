using Agent.Exceptions;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Agent.Tests
{
    [ExcludeFromCodeCoverage]
    public class FileHandlerTests
    {
        private FileHandler _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new FileHandler();

        }

        [Test]
        public void Test_ImportFile_FileIsImported()
        {
            var expected = "combat when player nearby player then attack";
            var fileLocation = String.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\import_test_file_1.txt";
            
            var result = _sut.ImportFile(fileLocation);

            Assert.AreEqual(expected, result);
        }
        [Test]
        public void Test_ImportFile_ThrowsFileException1()
        { 
            //Method to 
            var fileLocation = String.Format(Path.GetFullPath(Path.Combine
                        (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\ThisFileDoesNotExist.txt";

            var exception = Assert.Throws<FileException>(() =>
                _sut.ImportFile(fileLocation));

            Assert.AreEqual("File not found!", exception.Message);
        }
        
        [Test]
        public void Test_ImportFile_ThrowsFileException2()
        { 
            //Method to 
            var fileLocation = String.Format(Path.GetFullPath(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\AgentTestFileWrongExtension.xml";

            var exception = Assert.Throws<FileException>(() =>
                _sut.ImportFile(fileLocation));

            Assert.AreEqual("The provided file is of an incorrect extension", exception.Message);
        }

        [Test]
        public void Test_ExportFile_FileGetsExported()
        {
            var expected = "combat=defensive\r\nexplore=random";
            var fileLocation = String.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\";
            var fileName = "AgentExportFile.cfg";
            _sut.ExportFile(expected, fileName);

            Assert.AreEqual(expected, _sut.ImportFile(fileLocation + fileName));
        }

        [Test]
        public void Test_CreateDirectory_DirectoryCreated()
        {
            var directory = String.Format(Path.GetFullPath(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\Agent\\";
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory);
            }
            
            _sut.CreateDirectory(directory + "TestFile.txt");
            Assert.True(Directory.Exists(directory));
        }
        
    }
}
