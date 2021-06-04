using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ASD_project.Agent;
using ASD_project.Agent.Mapper;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Mapper
{
    [ExcludeFromCodeCoverage]
    public class FileToDictionaryMapperTest
    {
        private FileToDictionaryMapper _sut;
        private FileHandler _handler;

        [SetUp]
        public void Setup()
        {
            _sut = new FileToDictionaryMapper();
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
            var filepath = _handler.GetBaseDirectory() + "/AgentTests/Resource/npcFileTest.txt";
            
            //Act
            var actualDictionary = _sut.MapFileToConfiguration(filepath);

            //Assert
            Assert.AreEqual(expectedDictionary, actualDictionary);
            Assert.AreEqual(expectedDictionary["explore"], actualDictionary["explore"]);


        }

        [Test]
        public void Test_MapFileToConfiguration_Unsuccessful()
        {
            //Arrange
            var filepath = _handler.GetBaseDirectory() + "/AgentTests/Resource/npcFileTest_2.txt";
            
            //Act & Assert
            Assert.Throws<SyntaxErrorException>(() => _sut.MapFileToConfiguration(filepath));

        }
    }
}