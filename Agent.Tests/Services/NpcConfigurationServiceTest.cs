using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Agent.Mapper;
using Agent.Models;
using Agent.Services;
using NUnit.Framework;

namespace Agent.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class NpcConfigurationServiceTest
    {
        private FileToDictionaryMapper _mapper;
        private NpcConfigurationService _npcConfigurationService;
        private List<NpcConfiguration> _npcConfigurations;

        [SetUp]
        public void Setup()
        {
            _mapper = new FileToDictionaryMapper();
            _npcConfigurations = new List<NpcConfiguration>();
            _npcConfigurationService = new NpcConfigurationService(_npcConfigurations, _mapper);
        }

        [Test]
        public void Test_CreateNewNpcConfiguration_WithNewNpc()
        {
            //Arrange
            var filepath = String.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) + "resource\\npcFileTest.txt";
            
            //Act
            _npcConfigurationService.CreateNpcConfiguration("TestNPCName", filepath);
            
            //Assert
            Assert.True(_npcConfigurationService.GetConfigurations().Count > 0);
            Assert.AreEqual(_npcConfigurationService.GetConfigurations()[0].GetSetting("aggressiveness"), "high");
        }
    }
}