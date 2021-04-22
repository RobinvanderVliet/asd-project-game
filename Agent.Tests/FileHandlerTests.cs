using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Tests
{
    public class FileHandlerTests
    {
        static FileHandler sut;

        [SetUp]
        public void Setup()
        {
            sut = new FileHandler();

        }

        [Test]
        public void ImportFileTest()
        {
            var expected = "combat when player nearby player then attack";
            var fileLocation = String.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\import_test_file_1.txt";

            var result = sut.ImportFile(fileLocation);

            Assert.AreEqual(expected, result);
        }
        [Test]
        public void ImportWrongFileTest()
        { 
            //Method to 
            var fileLocation = String.Format(Path.GetFullPath(Path.Combine
                        (AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\import_test_file_1.php";

            var exception = Assert.Throws<FileException>(() =>
                sut.ImportFile(fileLocation));

            Assert.AreEqual("File given is not of the correct file type", exception.Message);
        }

        [Test]
        public void ExportFileTest()
        {
            //var expected = "combat when player nearby player then attack combat";

            //sut.ExportFile("combat when player nearby player then attack combat");

            //Assert.Equals(expected, actual);
        }
    }
}
