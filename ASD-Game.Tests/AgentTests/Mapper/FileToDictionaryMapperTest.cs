using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ASD_Game.Agent;
using ASD_Game.Agent.Mapper;
using NUnit.Framework;

namespace ASD_Game.Tests.AgentTests.Mapper
{
    [ExcludeFromCodeCoverage]
    public class FileToDictionaryMapperTest
    {
        private FileToDictionaryMapper _sut;
        private FileHandler _handler;
        private static readonly char _separator = Path.DirectorySeparatorChar;

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
            List<Setting> expectedDictionary = new();
            expectedDictionary.Add(new Setting("explore", "random"));
            expectedDictionary.Add(new Setting("combat", "offensive"));
            var filepath = _handler.GetBaseDirectory() + $"{_separator}Resource{_separator}npcFileTest.txt";
            
            //Act
            var actualDictionary = _sut.MapFileToConfiguration(filepath);

            //Assert
            Assert.AreEqual(expectedDictionary, actualDictionary);
        }

        [Test]
        public void Test_MapFileToConfiguration_Unsuccessful()
        {
            //Arrange
            var filepath = _handler.GetBaseDirectory() + $"{_separator}Resource{_separator}npcFileTest_2.txt";
            
            //Act & Assert
            Assert.Throws<SyntaxErrorException>(() => _sut.MapFileToConfiguration(filepath));

        }
    }
}