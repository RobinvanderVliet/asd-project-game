using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Agent.Services;
using NUnit.Framework;

namespace Agent.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class AgentConfigurationServiceTest
    {
        private AgentConfigurationService _sut;
        
        [SetUp]
        public void Setup()
        {
            _sut = new AgentConfigurationService();
        }
        
        [Test]
        public void Test_CreateNewAgentConfiguration_WithNewAgent()
        {
            //Arrange
            var filepath =
                String.Format(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\..\\"))) +
                "resource\\agent_test.cfg";

            //Act
            _sut.CreateConfiguration("Agent", filepath);

            //Assert
            Assert.True(_sut.GetConfigurations().Count > 0);
            Assert.AreEqual(_sut.GetConfigurations()[0].GetSetting("explore"), "random");
        }
    }
}