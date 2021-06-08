using Agent.Mapper;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests.Mapper
{
    [ExcludeFromCodeCoverage]
    public class FileToDictionaryMapperTest
    {
        private FileToConfigurationMapper _sut;
        private FileHandler _handler;

        [SetUp]
        public void Setup()
        {
            _sut = new FileToConfigurationMapper();
            _handler = new FileHandler();

        }

        [Test]
        public void Test_MapFileToConfiguration_Successful()
        {
            //Arrange
            Dictionary<string, string> expectedDictionary = new Dictionary<string, string>();
            expectedDictionary.Add("aggressiveness", "high");
            expectedDictionary.Add("explore", "random");
            expectedDictionary.Add("combat", "offensive");
            var filepath = _handler.GetBaseDirectory() + "/Resource/npcFileTest.txt";
            
            //Act
            var actualDictionary = _sut.MapFileToConfiguration(filepath);

            //Assert
            Assert.AreEqual(expectedDictionary, actualDictionary);
            //Assert.AreEqual(expectedDictionary["explore"], actualDictionary["explore"]); //TODO commeted our by team 4 Due to unmade dictionary changes


        }

        [Test]
        public void Test_MapFileToConfiguration_Unsuccessful()
        {
            //Arrange
            var filepath = _handler.GetBaseDirectory() + "/Resource/npcFileTest_2.txt";
            
            //Act & Assert
            Assert.Throws<SyntaxErrorException>(() => _sut.MapFileToConfiguration(filepath));

        }
    }
}