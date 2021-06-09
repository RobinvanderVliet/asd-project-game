using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActionHandling;
using Agent.Services;
using ASD_Game.ActionHandling;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Network;
using ASD_Game.World;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Characters.Algorithms.Creator;
using ASD_Game.World.Services;
using Moq;
using NUnit.Framework;
using WorldGeneration.StateMachine;
using Agent = World.Models.Characters.Agent;
using World = ASD_Game.World.World;

namespace Creature.Tests.Handlers
{
    public class AgentHandlerTest
    {
        private AgentHandler sut;

        private Mock<IWorldService> _mockedWorldService;
        private Mock<IClientController> _mockedClientController;
        private Mock<IDatabaseService<AgentPOCO>> _mockedAgentDatabaseService;
        private Mock<IConfigurationService> _mockedConfigurationService;
        private Mock<IAgentCreator> _mockedAgentCreator;

        [SetUp]
        public void Setup()
        {
            _mockedWorldService = new Mock<IWorldService>();
            _mockedClientController = new Mock<IClientController>();
            _mockedAgentDatabaseService = new Mock<IDatabaseService<AgentPOCO>>();
            _mockedConfigurationService = new Mock<IConfigurationService>();
            _mockedAgentCreator = new Mock<IAgentCreator>();

            sut = new AgentHandler(
                _mockedWorldService.Object,
                _mockedClientController.Object,
                _mockedAgentDatabaseService.Object,
                _mockedConfigurationService.Object,
                _mockedAgentCreator.Object
            );
        }

        [Test]
        public void Test_Replace_ReplacesStateMachine()
        {
            // Arrange
            var mockedWorld = new Mock<IWorld>();
            var player = new Player("Gerrit", 10, 10, "#", "random-player-id");
            var agentPoco = new AgentPOCO
            {
                AgentConfiguration = new List<KeyValuePair<string, string>>(), PlayerGUID = "random-player-id"
            };
            List<AgentPOCO> playerItemPOCOs = new();
            playerItemPOCOs.Add(agentPoco);
            IEnumerable<AgentPOCO> enumerable = playerItemPOCOs;
            var agent = new global::World.Models.Characters.Agent(player.Name, player.XPosition, player.YPosition,
                player.Symbol, player.Id);
            agent.AgentStateMachine = new Mock<ICharacterStateMachine>().Object;

            _mockedWorldService.Setup(world => world.GetWorld()).Returns(mockedWorld.Object);
            _mockedWorldService.Setup(world => world.GetPlayer("random-player-id")).Returns(player);
            _mockedAgentDatabaseService.Setup(mock => mock.GetAllAsync()).Returns(Task.FromResult(enumerable));
            _mockedAgentCreator.Setup(creator => creator.CreateAgent(player, agentPoco.AgentConfiguration))
                .Returns(agent);

            // Act
            sut.Replace("random-player-id");
            
            // Assert
            _mockedAgentDatabaseService.Verify(db => db.UpdateAsync(agentPoco));
            Assert.That(agentPoco.Activated);
        }
    }
}