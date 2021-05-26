using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Agent.Mapper;
using NUnit.Framework;

namespace Agent.Tests.Mapper
{
    [ExcludeFromCodeCoverage]
    public class FileToDictionaryMapperTest
    {
        private FileToDictionaryMapper _sut;
        private FileHandler handler;

        [SetUp]
        public void Setup()
        {
            _sut = new FileToDictionaryMapper();
            handler = new FileHandler();

        }

        [Test]
        public void Test_MapFileToConfiguration_Successful()
        {
            //Arrange
            Dictionary<string, string> expectedDictionary = new Dictionary<string, string>();
            expectedDictionary.Add("aggressiveness", "high");
            expectedDictionary.Add("explore", "random");
            expectedDictionary.Add("combat", "offensive");
            var filepath = handler.GetBaseDirectory() + "/Resource/npcFileTest.txt";
            
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
            var filepath = handler.GetBaseDirectory() + "/Resource/npcFileTest_2.txt";
            
            //Act & Assert
            Assert.Throws<SyntaxErrorException>(() => _sut.MapFileToConfiguration(filepath));

        }
    }
}