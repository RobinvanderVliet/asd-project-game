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
            //Arrange
            var expected = "combat when player nearby player then attack";
            var fileLocation = String.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\import_test_file_1.txt";
            
            //Act
            var result = _sut.ImportFile(fileLocation);

            //Assert
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void Test_ImportFile_ThrowsFileException1()
        { 
            //Arrange
            var fileLocation = String.Format(Path.GetFullPath(Path.Combine
                        (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\ThisFileDoesNotExist.txt";

            //Act
            var exception = Assert.Throws<FileException>(() =>
                _sut.ImportFile(fileLocation));

            //Assert
            Assert.AreEqual("File not found!", exception.Message);
        }
        
        [Test]
        public void Test_ImportFile_ThrowsFileException2()
        { 
            //Arrange
            var fileLocation = String.Format(Path.GetFullPath(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\AgentTestFileWrongExtension.xml";

            //Act
            var exception = Assert.Throws<FileException>(() =>
                _sut.ImportFile(fileLocation));

            //Assert
            Assert.AreEqual("The provided file is of an incorrect extension", exception.Message);
        }

        [Test]
        public void Test_ExportFile_FileGetsExported()
        {
            //Arrange
            var expected = "combat=defensive\r\nexplore=random";
            var fileLocation = String.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\";
            var fileName = "AgentExportFile.cfg";
            
            //Act
            _sut.ExportFile(expected, fileName);

            //Assert
            Assert.AreEqual(expected, _sut.ImportFile(fileLocation + fileName));
        }

        [Test]
        public void Test_CreateDirectory_DirectoryCreated()
        {
            //Arrange
            var directory = String.Format(Path.GetFullPath(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "Resources\\Agent\\";
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory);
            }
            
            //Act
            _sut.CreateDirectory(directory + "TestFile.txt");
            
            //Assert
            Assert.True(Directory.Exists(directory));
        }
        
    }
}
