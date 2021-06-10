using System.Collections.Generic;
using System.Threading.Tasks;
using Agent.Services;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Session;
using ASD_Game.World;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Characters.Algorithms.Creator;
using ASD_Game.World.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Session.DTO;
using WorldGeneration.StateMachine;

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
        [TestCase(false)]
        [TestCase(true)]
        public void Test_Replace_ReplacesStateMachine(bool started)
        {
            // Arrange
            var mockedWorld = new Mock<IWorld>();
            var player = new Player("Gerrit", 10, 10, "#", "random-player-id");
            var agentPoco = new AgentPOCO
            {
                AgentConfiguration = new List<KeyValuePair<string, string>>(), PlayerGUID = "random-player-id",
            };
            List<AgentPOCO> playerItemPOCOs = new() {agentPoco};
            IEnumerable<AgentPOCO> enumerable = playerItemPOCOs;
            var agent = new global::World.Models.Characters.Agent(player.Name, player.XPosition, player.YPosition,
                player.Symbol, player.Id);
            var mockedAgentStateMachine = new Mock<ICharacterStateMachine>();
            agent.AgentStateMachine = mockedAgentStateMachine.Object;

            _mockedWorldService.Setup(world => world.GetWorld()).Returns(mockedWorld.Object);
            _mockedWorldService.Setup(world => world.GetPlayer("random-player-id")).Returns(player);
            _mockedAgentDatabaseService.Setup(mock => mock.GetAllAsync()).Returns(Task.FromResult(enumerable));
            _mockedAgentCreator.Setup(creator => creator.CreateAgent(player, agentPoco.AgentConfiguration))
                .Returns(agent);
            mockedAgentStateMachine.Setup(machine => machine.WasStarted()).Returns(started);

            // Act
            sut.Replace("random-player-id");
            sut.Replace("random-player-id");

            // Assert
            if (!started)
            {
                mockedAgentStateMachine.Verify(machine => machine.StartStateMachine(), Times.Exactly(2));
            }
            else
            {
                mockedAgentStateMachine.Verify(machine => machine.StartStateMachine(), Times.Once);
                mockedAgentStateMachine.Verify(machine => machine.StopStateMachine(), Times.Once);
            }
        }

        [Test]
        [TestCase("random-player-id")]
        [TestCase("invalid-player-id")]
        public void Test_HandlePacket_InsertsOrUpdatesAgentConfiguration(string playerId)
        {
            // Arrange
            var agentConfigurationDto = new AgentConfigurationDTO(SessionType.SendAgentConfiguration)
            {
                AgentConfiguration = new List<KeyValuePair<string, string>>(),
                PlayerId = playerId
            };
            var packetDto = new PacketDTO();
            var header = new PacketHeaderDTO {Target = "host"};
            packetDto.Header = header;
            packetDto.Payload = JsonConvert.SerializeObject(agentConfigurationDto);
            var agentPoco = new AgentPOCO
            {
                AgentConfiguration = agentConfigurationDto.AgentConfiguration,
                PlayerGUID = "random-player-id"
            };
            List<AgentPOCO> playerItemPOCOs = new() {agentPoco};
            IEnumerable<AgentPOCO> enumerable = playerItemPOCOs;


            _mockedAgentDatabaseService.Setup(mock => mock.GetAllAsync()).Returns(Task.FromResult(enumerable));

            // Act
            sut.HandlePacket(packetDto);

            // Assert
            if (playerId == agentPoco.PlayerGUID)
            {
                _mockedAgentDatabaseService.Verify(db => db.UpdateAsync(agentPoco), Times.Exactly(1));
            }
            else
            {
                _mockedAgentDatabaseService.Verify(db => db.UpdateAsync(agentPoco), Times.Exactly(0));
            }
        }
    }
}