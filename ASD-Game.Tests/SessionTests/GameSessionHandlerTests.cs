using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.ActionHandling;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.Session;
using ASD_Game.Session.DTO;
using ASD_Game.Session.GameConfiguration;
using ASD_Game.UserInterface;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Services;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario;

namespace Session.Tests
{
    [ExcludeFromCodeCoverage]
    public class GameSessionHandlerTests
    {
        private GameSessionHandler _sut;

        private PacketDTO _packetDTO;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;

        private Mock<IWorldService> _mockedWorldService;
        private Mock<ISessionHandler> _mockedsessionHandler;
        private Mock<IDatabaseService<GamePOCO>> _mockedGamePOCOServices;
        private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerPOCOServices;
        private Mock<IScreenHandler> _mockedScreenHandler;
        private Mock<IRelativeStatHandler> _mockedRelativeStatHandler;
        private Mock<IMessageService> _mockedMessageService;
        private Mock<IDatabaseService<PlayerItemPOCO>> _mockedPlayerItemService;
        private Mock<IGameConfigurationHandler> _mockedGameConfiguration;
        private Mock<IDatabaseService<GameConfigurationPOCO>> _mockedGameConfigurationPoco;
        private Mock<ITerrainTile> _mockedTile;

        private char[,] _map = { { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
                                 { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','} };

        [SetUp]
        public void Setup()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;

            Console.SetOut(standardOutput);

            _mockedClientController = new Mock<IClientController>();
            _mockedPlayerPOCOServices = new Mock<IDatabaseService<PlayerPOCO>>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedsessionHandler = new Mock<ISessionHandler>();
            _mockedGamePOCOServices = new Mock<IDatabaseService<GamePOCO>>();
            _mockedScreenHandler = new Mock<IScreenHandler>();
            _mockedRelativeStatHandler = new Mock<IRelativeStatHandler>();
            _mockedMessageService = new Mock<IMessageService>();
            _mockedPlayerItemService = new Mock<IDatabaseService<PlayerItemPOCO>>();
            _mockedGameConfiguration = new Mock<IGameConfigurationHandler>();
            _mockedGameConfigurationPoco = new Mock<IDatabaseService<GameConfigurationPOCO>>();
            _mockedTile = new Mock<ITerrainTile>();

            _mockedWorldService.Setup(mock => mock.LoadArea(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));

            _mockedWorldService.Setup(mock => mock.GetTile(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(_mockedTile.Object);

            _mockedTile.Setup(mock => mock.IsAccessible).Returns(true);

            _mockedWorldService.Setup(mock => mock.CheckIfCharacterOnTile(It.IsAny<ITile>())).Returns(false);

            _sut = new GameSessionHandler(_mockedClientController.Object, _mockedsessionHandler.Object, _mockedRelativeStatHandler.Object, _mockedGameConfiguration.Object, _mockedScreenHandler.Object, _mockedPlayerPOCOServices.Object, _mockedGamePOCOServices.Object, _mockedGameConfigurationPoco.Object, _mockedPlayerItemService.Object, _mockedWorldService.Object, _mockedMessageService.Object);
            _packetDTO = new PacketDTO();
        }

        [Test]
        public void Test_SendGameSession_NewGameSendsEmptyGameDTO()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();

            //Act
            _sut.SendGameSession();
            var payload = JsonConvert.SerializeObject(startGameDto);

            //Assert
            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedClientController.Verify(x => x.SendPayload(payload, PacketType.GameSession), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_NewGameTransitionsToNewGameScreen()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();

            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();

            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;
            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });

            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedScreenHandler.Verify(x => x.TransitionTo(It.IsAny<GameScreen>()), Times.Once());
        }

        [Test]
        public void Test_GenerateWorld_IfStartedNewGame()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();

            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedWorldService.Verify(x => x.GenerateWorld(1), Times.Once);
        }

        [Test]
        public void Test_NewGame_IfStartedNewGameInsertNewGameInDatabase()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();
            startGameDto.GameGuid = null;

            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;
            Player player = new Player("name1", 0, 0, "@", "1");

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            List<Player> savedPlayers = new List<Player>();
            savedPlayers.Add(player);

            GamePOCO gamePOCO = new GamePOCO();
            gamePOCO.Seed = 1;
            gamePOCO.GameGUID = "1";
            gamePOCO.PlayerGUIDHost = "1";

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.SessionId).Returns("1");
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.IsHost()).Returns(true);
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedGamePOCOServices.Verify(mock => mock.CreateAsync(It.IsAny<GamePOCO>()), Times.Once());
        }

        [Test]
        public void Test_NewGame_IfStartedNewGameInsertNewPlayerInDatabase()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();
            startGameDto.GameGuid = null;

            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;
            Player player = new Player("name1", 0, 0, "@", "1");

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            List<Player> savedPlayers = new List<Player>();
            savedPlayers.Add(player);

            GamePOCO gamePOCO = new GamePOCO();
            gamePOCO.Seed = 1;
            gamePOCO.GameGUID = "1";
            gamePOCO.PlayerGUIDHost = "1";

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.SessionId).Returns("1");
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.IsHost()).Returns(true);
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedPlayerPOCOServices.Verify(mock => mock.CreateAsync(It.IsAny<PlayerPOCO>()), Times.Once());
        }

        [Test]
        public void Test_InsertItem_IfStartedNewGameInsertNewPlayerItemsInDatabase()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();
            startGameDto.GameGuid = null;

            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;
            Player player = new Player("name1", 0, 0, "@", "1");

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            List<Player> savedPlayers = new List<Player>();
            savedPlayers.Add(player);

            GamePOCO gamePOCO = new GamePOCO();
            gamePOCO.Seed = 1;
            gamePOCO.GameGUID = "1";
            gamePOCO.PlayerGUIDHost = "1";

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.SessionId).Returns("1");
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.IsHost()).Returns(true);
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedPlayerItemService.Verify(mock => mock.CreateAsync(It.IsAny<PlayerItemPOCO>()), Times.AtLeast(1));
        }

        [Test]
        public void Test_NewConfig_IfStartedNewGameInsertNewGameConfigurationInDatabase()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();
            startGameDto.GameGuid = null;

            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;
            Player player = new Player("name1", 0, 0, "@", "1");

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            List<Player> savedPlayers = new List<Player>();
            savedPlayers.Add(player);

            GamePOCO gamePOCO = new GamePOCO();
            gamePOCO.Seed = 1;
            gamePOCO.GameGUID = "1";
            gamePOCO.PlayerGUIDHost = "1";

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.SessionId).Returns("1");
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.IsHost()).Returns(true);
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedGameConfigurationPoco.Verify(mock => mock.CreateAsync(It.IsAny<GameConfigurationPOCO>()), Times.AtLeast(1));
        }

        [Test]
        public void Test_NewGameConfig_IfStartedNewGameClientDontInsertNewGameConfigurationInDatabase()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();
            startGameDto.GameGuid = null;

            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;
            Player player = new Player("name1", 0, 0, "@", "1");

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            List<Player> savedPlayers = new List<Player>();
            savedPlayers.Add(player);

            GamePOCO gamePOCO = new GamePOCO();
            gamePOCO.Seed = 1;
            gamePOCO.GameGUID = "1";
            gamePOCO.PlayerGUIDHost = "1";

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.SessionId).Returns("1");
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.IsHost()).Returns(false);
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedGameConfigurationPoco.Verify(mock => mock.CreateAsync(It.IsAny<GameConfigurationPOCO>()), Times.Never);
        }

        [Test]
        public void Test_StartSavedGame_IfStartedSavedGameGeneratesWorldWithOldSessionSeed()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();
            startGameDto.GameGuid = "1";

            PlayerPOCO player = new PlayerPOCO();
            player.PlayerGUID = "1";
            player.GameGUID = "1";
            player.XPosition = 10;
            player.YPosition = 11;

            List<PlayerPOCO> savedPlayers = new List<PlayerPOCO>();
            savedPlayers.Add(player);
            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });

            Monster newMonster = new Monster("Zombie", 12, 15,
                CharacterSymbol.TERMINATOR, "monst");

            Player playerinWorld = new Player("name1", 20, 20, "@", "1");

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedWorldService.Verify(x => x.GenerateWorld(1), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_StatHandlerCurrentPlayerGetSets()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();

            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();

            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;
            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            Player playerinWorld = new Player("name1", 0, 0, "@", "1");

            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");

            _mockedWorldService.Setup(x => x.GetCurrentPlayer()).Returns(playerinWorld);
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedRelativeStatHandler.Verify(x => x.SetCurrentPlayer(playerinWorld), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_StatHandlerChecksStaminaTimerGetsCalled()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();

            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();

            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;
            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            Player playerinWorld = new Player("name1", 0, 0, "@", "1");

            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");

            _mockedWorldService.Setup(x => x.GetCurrentPlayer()).Returns(playerinWorld);
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedRelativeStatHandler.Verify(x => x.CheckStaminaTimer(), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_StatHandlerChecksRadiationTimerGetsCalled()
        {
            //Arrange
            StartGameDTO startGameDto = new StartGameDTO();

            var payload = JsonConvert.SerializeObject(startGameDto);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();

            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;
            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            Player playerinWorld = new Player("name1", 0, 0, "@", "1");

            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(_map);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedWorldService.Setup(x => x.GetCurrentPlayer()).Returns(playerinWorld);

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            //Act
            _sut.HandlePacket(_packetDTO);

            //Assert
            _mockedRelativeStatHandler.Verify(x => x.CheckRadiationTimer(), Times.Once);
        }
    }
}