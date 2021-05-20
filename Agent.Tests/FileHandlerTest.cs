using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Agent.Exceptions;

namespace Agent.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class FileHandlerTest
    {
        private static FileHandler _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new FileHandler();

        }

        [Test]
        public void Test_Import_CorrectFile()
        {
            var expected = "combat when player nearby player then attack";
            var fileLocation = string.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\import_test_file_1.txt";
            var result = _sut.ImportFile(fileLocation);

            Assert.AreEqual(expected, result);
        }
        [Test]
        public void Test_Import_WrongFile()
        { 
            //Method to 
            var fileLocation = string.Format(Path.GetFullPath(Path.Combine
                        (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\import_test_file_1.php";

            var exception = Assert.Throws<FileException>(() =>
                _sut.ImportFile(fileLocation));

            Assert.AreEqual("File given is not of the correct file type", exception.Message);
        }

        [Test]
        public void Test_ExportFile()
        {
            var expected = "combat when player nearby player then attack combat";

            _sut.ExportFile("combat when player nearby player then attack combat");

            var fileLocation = String.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\agentFile.cfg";

            var actual = File.ReadAllText(fileLocation);

            Assert.AreEqual(expected, actual);
        }
    }
}
