using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Network;
using ASD_Game.Session;
using ASD_Game.Session.Helpers;
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
        //Declaration and initialisation of variables
        private List<string[]> _allClients;
        private int _sessionSeed;
        
        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;
        private Mock<ISessionHandler> _mockedSessionHandler;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<ITerrainTile> _mockedTile;
        
        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedSessionHandler = new Mock<ISessionHandler>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedTile = new Mock<ITerrainTile>();
            _sessionSeed = 12;
            _allClients = new List<string[]> {new[] {"1", "Player 1"}, new[] {"2", "Player 2"}};
            _mockedSessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(_sessionSeed);
            _mockedSessionHandler.Setup(mock => mock.GetAllClients()).Returns(_allClients);
            _mockedWorldService.Setup(mock => mock.LoadArea(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            _mockedWorldService.Setup(mock => mock.GetTile(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(_mockedTile.Object);
            _mockedTile.Setup(mock => mock.IsAccessible).Returns(true);
            _mockedWorldService.Setup(mock => mock.CheckIfCharacterOnTile(It.IsAny<ITile>())).Returns(false);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");

            _mockedWorldService.Setup(mock => mock.AddPlayerToWorld(It.IsAny<Player>(), true)).Verifiable();
            _mockedWorldService.Setup(mock => mock.AddPlayerToWorld(It.IsAny<Player>(), false)).Verifiable();
        }

        [Test]
        public void AddPlayersToWorldTest()
        {
            // Arrange

            // Act
            PlayerSpawner.SpawnPlayers(_allClients, _sessionSeed, _mockedWorldService.Object, _mockedClientController.Object);

            // Assert
            _mockedWorldService.Verify();
        }
    }
}