using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Agent.Mapper;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Agent.Tests.Mapper
{
    [ExcludeFromCodeCoverage]
    public class FileToDictionaryMapperTest
    {
        private FileToDictionaryMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new FileToDictionaryMapper();

        }

        [Test]
        public void Test_MapFileToConfiguration_Successful()
        {
            //Arrange
            Dictionary<string, string> expectedDictionary = new Dictionary<string, string>();
            expectedDictionary.Add("aggressiveness", "high");
            expectedDictionary.Add("explore", "random");
            expectedDictionary.Add("combat", "offensive");
            var filepath = string.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\npcFileTest.txt";
            
            //Act
            var actualDictionary = _mapper.MapFileToConfiguration(filepath);

            //Assert
            Assert.AreEqual(expectedDictionary, actualDictionary);
            Assert.AreEqual(expectedDictionary["explore"], actualDictionary["explore"]);


        }
        
        [Test]
        public void Test_MapFileToConfiguration_Unsuccessful()
        {
            //Arrange
            var filepath = string.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\npcFileTest_2.txt";
            
            //Act
            //Assert
            Assert.Throws<SyntaxErrorException>(() => _mapper.MapFileToConfiguration(filepath));

        }
    }
}