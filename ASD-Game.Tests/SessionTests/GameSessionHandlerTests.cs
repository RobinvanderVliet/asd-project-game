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
            StartGameDTO startGameDto = new StartGameDTO();

            _sut.SendGameSession();
            var payload = JsonConvert.SerializeObject(startGameDto);

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedClientController.Verify(x => x.SendPayload(payload, PacketType.GameSession), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_NewGameTransitionsToNewGameScreen()
        {
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            _sut.HandlePacket(_packetDTO);

            _mockedScreenHandler.Verify(x => x.TransitionTo(It.IsAny<GameScreen>()), Times.Once());
        }

        [Test]
        public void Test_If_Started_New_Game_Generates_World()
        {
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            _sut.HandlePacket(_packetDTO);
            _mockedWorldService.Verify(x => x.GenerateWorld(1), Times.Once);
        }

        [Test]
        public void Test_If_Started_New_Game_Insert_New_Game_In_Database()
        {
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            _sut.HandlePacket(_packetDTO);

            _mockedGamePOCOServices.Verify(mock => mock.CreateAsync(It.IsAny<GamePOCO>()), Times.Once());
        }

        [Test]
        public void Test_If_Started_New_Game_Insert_New_Player_In_Database()
        {
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            _sut.HandlePacket(_packetDTO);

            _mockedPlayerPOCOServices.Verify(mock => mock.CreateAsync(It.IsAny<PlayerPOCO>()), Times.Once());
        }

        [Test]
        public void Test_If_Started_New_Game_Insert_New_PlayerItems_In_Database()
        {
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            _sut.HandlePacket(_packetDTO);

            _mockedPlayerItemService.Verify(mock => mock.CreateAsync(It.IsAny<PlayerItemPOCO>()), Times.AtLeast(1));
        }

        [Test]
        public void Test_If_Started_New_Game_Insert_New_GameConfiguration_In_Database()
        {
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            _sut.HandlePacket(_packetDTO);

            _mockedGameConfigurationPoco.Verify(mock => mock.CreateAsync(It.IsAny<GameConfigurationPOCO>()), Times.AtLeast(1));
        }

        [Test]
        public void Test_If_Started_New_Game_Client_Dont_Insert_New_GameConfiguration_In_Database()
        {
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            _sut.HandlePacket(_packetDTO);

            _mockedGameConfigurationPoco.Verify(mock => mock.CreateAsync(It.IsAny<GameConfigurationPOCO>()), Times.Never);
        }

        //ToDo
        //Out of bounds error because of new method to create new monsters.
        [Test]
        public void Test_If_Started_Saved_Game_Generates_World_WithOldSessionSeed()
        {
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
            char[,] monsterList = { { ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ',', ','},
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

            Player playerinWorld = new Player("name1", 20, 20, "@", "1");

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedWorldService.Setup(x => x.GetMapAroundCharacter(It.IsAny<Character>())).Returns(monsterList);
            _mockedsessionHandler.Setup(x => x.TrainingScenario).Returns(new TrainingScenario());

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            _sut.HandlePacket(_packetDTO);
            _mockedWorldService.Verify(x => x.GenerateWorld(1), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_StatHandler_CurrentPlayer_Get_Sets()
        {
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            _sut.HandlePacket(_packetDTO);

            _mockedRelativeStatHandler.Verify(x => x.SetCurrentPlayer(playerinWorld), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_StatHandler_Checks_Stamina_timer_Gets_Called()
        {
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            _sut.HandlePacket(_packetDTO);

            _mockedRelativeStatHandler.Verify(x => x.CheckStaminaTimer(), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_StatHandler_Checks_Radiation_timer_Gets_Called()
        {
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            _sut.HandlePacket(_packetDTO);

            _mockedRelativeStatHandler.Verify(x => x.CheckRadiationTimer(), Times.Once);
        }
    }
}