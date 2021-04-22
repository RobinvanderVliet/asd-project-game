using NUnit.Framework;
using System;
using System.Collections.Generic;
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
            //sut = new FileHandler();
        }

        [Test]
        public void ImportFileTest()
        {
            var expected = "combat when player nearby player then attack combat";

            //var result = sut.ImportFile(@"resource/import test file 1.txt");

            //Assert.AreEqual(expected, result);
        }
        [Test]
        public void ImportWrongFileTest()
        {
            //var result = sut.ImportFile(@"resource/import test file 1.php");

            //Assert.Throws<Exception>(ImportFile);
        }

        [Test]
        public void ExportFileTest()
        {
            var expected = "combat when player nearby player then attack combat";

            //sut.exportFile("combat when player nearby player then attack combat");
            //TODO get actual from file

            //Assert.Equals(expected, actual);
        }
    }
}
