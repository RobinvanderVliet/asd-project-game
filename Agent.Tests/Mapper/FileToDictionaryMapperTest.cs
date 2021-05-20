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

        [SetUp]
        public void Setup()
        {
            _sut = new FileToDictionaryMapper();

        }

        [Test]
        public void Test_MapFileToConfiguration_Successful()
        {
            //Arrange
            Dictionary<String, String> expectedDictionary = new Dictionary<string, string>();
            expectedDictionary.Add("aggressiveness", "high");
            expectedDictionary.Add("explore", "random");
            expectedDictionary.Add("combat", "offensive");
            var filepath = string.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\npcFileTest.txt";
            
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
            var filepath = string.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\npcFileTest_2.txt";
            
            //Act & Assert
            Assert.Throws<SyntaxErrorException>(() => _sut.MapFileToConfiguration(filepath));

        }
    }
}