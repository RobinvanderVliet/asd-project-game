using System.Collections.Generic;
using Agent.Models;
using Agent.Services;
using Creature.Services;
using Moq;
using NUnit.Framework;
using Player.Model;

namespace Creature.Tests
{
    [TestFixture]
    public class AgentServiceTest
    {
        private AgentService _sut;

        [SetUp]
        public void Setup()
        {
            var agentConfigurationServiceMock = new Mock<BaseConfigurationService>();
            var playerModelMock = new Mock<IPlayerModel>();
            var configurations = new List<Configuration>();
            var config = new AgentConfiguration();
            config.Settings = new Dictionary<string, string>();
            configurations.Add(config);

            agentConfigurationServiceMock.Setup(config => config.GetConfigurations())
                .Returns(configurations);
            playerModelMock.Setup(model => model.XPosition).Returns(1);
            playerModelMock.Setup(model => model.YPosition).Returns(1);
            playerModelMock.Setup(model => model.Health).Returns(1);
            playerModelMock.Setup(model => model.GetAttackDamage()).Returns(1);
            
            _sut = new AgentService(agentConfigurationServiceMock.Object, playerModelMock.Object);
        }

        [Test]
        public void Test_Activate_ActivatesAgent()
        {
            // Act -------------
            _sut.Activate();

            // Assert ----------
            Assert.True(_sut.IsActivated());
        }
        
        [Test]
        public void Test_DeActivate_DeActivatesAgent()
        {
            // Act -------------
            _sut.DeActivate();

            // Assert ----------
            Assert.False(_sut.IsActivated());
        }
    }
}