using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.ActionHandling;
using ASD_Game.Agent.Antlr.Ast.Comparables;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Session;
using ASD_Game.Session.GameConfiguration;
using ASD_Game.UserInterface;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Services;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.SessionTests.GameConfiguration
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class PlayerSpawnTests
    {
        //Declaration and initialisation of constant variables
        private GameSessionHandler _sut;
        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;
        private Mock<ISessionHandler> _mockedSessionHandler;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<IRelativeStatHandler> _mockedRelativeStatHandler;
        private Mock<IMessageService> _mockedMessageService;
        private Mock<IGameConfigurationHandler> _mockedGameConfigurationHandler;
        private Mock<IScreenHandler> _mockedScreenHandler;
        private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerDatabaseService;
        private Mock<IDatabaseService<GamePOCO>> _mockedGameDatabaseService;
        private Mock<IDatabaseService<GameConfigurationPOCO>> _mockedGameConfigDatabaseService;
        private Mock<IDatabaseService<PlayerItemPOCO>> _mockedPlayerItemDatabaseService;
        private Mock<ITerrainTile> _mockedTile;
        
        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedSessionHandler = new Mock<ISessionHandler>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedRelativeStatHandler = new Mock<IRelativeStatHandler>();
            _mockedMessageService = new Mock<IMessageService>();
            _mockedGameConfigurationHandler = new Mock<IGameConfigurationHandler>();
            _mockedScreenHandler = new Mock<IScreenHandler>();
            _mockedPlayerDatabaseService = new Mock<IDatabaseService<PlayerPOCO>>();
            _mockedGameDatabaseService = new Mock<IDatabaseService<GamePOCO>>();
            _mockedGameConfigDatabaseService = new Mock<IDatabaseService<GameConfigurationPOCO>>();
            _mockedPlayerItemDatabaseService = new Mock<IDatabaseService<PlayerItemPOCO>>();
            _mockedTile = new Mock<ITerrainTile>();
            
            int sessionSeed = 12;
            List<String[]> allClients = new List<string[]>();
            allClients.Add(new string[]{"1", "Player 1"});
            allClients.Add(new string[]{"2", "Player 2"});
            _mockedSessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(sessionSeed);
            _mockedSessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedWorldService.Setup(mock => mock.LoadArea(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            _mockedWorldService.Setup(mock => mock.GetTile(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(_mockedTile.Object);
            _mockedTile.Setup(mock => mock.IsAccessible).Returns(true);
            _mockedWorldService.Setup(mock => mock.CheckIfCharacterOnTile(It.IsAny<ITile>())).Returns(false);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");

            _mockedWorldService.Setup(mock => mock.AddPlayerToWorld(It.IsAny<Player>(), true)).Verifiable();
            _mockedWorldService.Setup(mock => mock.AddPlayerToWorld(It.IsAny<Player>(), false)).Verifiable();

            _sut = new GameSessionHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedSessionHandler.Object, _mockedRelativeStatHandler.Object, _mockedPlayerDatabaseService.Object,
                _mockedGameDatabaseService.Object, _mockedGameConfigDatabaseService.Object,  _mockedGameConfigurationHandler.Object, _mockedScreenHandler.Object, _mockedPlayerItemDatabaseService.Object, _mockedMessageService.Object);

        }

        [Test]
        public void AddPlayersToWorldTest()
        {
            // Arrange
            Player player1 = new Player("Player 1", 13, 15, CharacterSymbol.CURRENT_PLAYER, "1");
            Player player2 = new Player("Player 2", 13, 16, CharacterSymbol.ENEMY_PLAYER, "2");

            // Act
            _sut.AddPlayersToWorld();

            // Assert
            _mockedWorldService.Verify();
        }
    }
}