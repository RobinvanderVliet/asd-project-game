using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
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
using ASD_Game.World.Models.Characters.Algorithms.Pathfinder;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Services;
using Castle.Core.Logging;
using DatabaseHandler.POCO;
using LiteDB;
using WorldGeneration;

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

        // public GameSessionHandler(IClientController clientController, IWorldService worldService,
        //     ISessionHandler sessionHandler, IDatabaseService<GamePOCO> gamePocoService,
        //     IDatabaseService<PlayerPOCO> playerService,
        //     IDatabaseService<ClientHistoryPOCO> clientHistoryService, IScreenHandler screenHandler,
        //     IRelativeStatHandler relativeStatHandler, IMessageService messageService,
        //     IDatabaseService<PlayerItemPOCO> playerItemDatabaseService,
        //     IGameConfigurationHandler gameConfigurationHandler,
        //     IDatabaseService<GameConfigurationPOCO> gameConfigDatabaseService)

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

            _sut = new GameSessionHandler(_mockedClientController.Object, _mockedWorldService.Object,
                _mockedsessionHandler.Object, _mockedGamePOCOServices.Object, _mockedPlayerPOCOServices.Object,
              _mockedScreenHandler.Object, _mockedRelativeStatHandler.Object,
                _mockedMessageService.Object, _mockedPlayerItemService.Object, _mockedGameConfiguration.Object,
                _mockedGameConfigurationPoco.Object);
            _packetDTO = new PacketDTO();
        }

        [Test]
        public void Test_SendGameSession_FillsStartGameDTOWithExistingGame()
        {
            _mockedsessionHandler.Setup(x => x.GetSavedGame()).Returns(true);
            StartGameDTO startGameDto = new StartGameDTO();
            PlayerPOCO player = new PlayerPOCO
            {
                GameGUID = "GameGuid1",
                Health = 1,
                Stamina = 1,
                PlayerGUID = "GameGuid1Player1",
                GameGUIDAndPlayerGuid = "GameGuid1Player1",
                PlayerName = "Player1",
                TypePlayer = 1,
                XPosition = 0,
                YPosition = 0
            };

            List<PlayerPOCO> savedPlayers = new List<PlayerPOCO>();

            savedPlayers.Add(player);
            _mockedClientController.Setup(x => x.SessionId).Returns("GameGuid1");
            _mockedPlayerPOCOServices.Setup(x => x.GetAllAsync()).ReturnsAsync(savedPlayers);

            startGameDto.GameGuid = "GameGuid1";
            startGameDto.Seed = 0;
            startGameDto.SavedPlayers = savedPlayers;

            _sut.SendGameSession();
            var payload = JsonConvert.SerializeObject(startGameDto);

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedClientController.Verify(x => x.SendPayload(payload, PacketType.GameSession), Times.Once);
        }

        [Test]
        public void Test_SendGameSession_NewGameSendsEmptyGameDTO()
        {
            _mockedsessionHandler.Setup(x => x.GetSavedGame()).Returns(false);
            StartGameDTO startGameDto = new StartGameDTO();

            _sut.SendGameSession();
            var payload = JsonConvert.SerializeObject(startGameDto);

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedClientController.Verify(x => x.SendPayload(payload, PacketType.GameSession), Times.Once);
        }

        [Test]
        public void Test_SendGameSession_SendGameSessionSetsGameStartedOnTrue()
        {
            _mockedsessionHandler.Setup(x => x.GetSavedGame()).Returns(false);

            _sut.SendGameSession();
            _mockedsessionHandler.Verify(x => x.SetGameStarted(true), Times.Once);
        }

        [Test]
        public void Test_HandlePacket_NewGameTransitionsToNewGameScreen()
        {
            _mockedsessionHandler.Setup(x => x.GetSavedGame()).Returns(false);
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

            _mockedsessionHandler.Setup(x => x.GameStarted()).Returns(false);

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
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;
            Player player = new Player("name1", 0, 0, "@", "1");

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            List<Player> savedPlayers = new List<Player>();
            savedPlayers.Add(player);

            GamePOCO gamePOCO = new GamePOCO();
            gamePOCO.Seed = 1;
            gamePOCO.GameName = "New game";
            gamePOCO.GameGUID = "1";
            gamePOCO.PlayerGUIDHost = "1";

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.SessionId).Returns("1");
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");
            _mockedsessionHandler.Setup(x => x.GameName).Returns("New game");
            _mockedClientController.Setup(x => x.IsHost()).Returns(true);

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            _mockedsessionHandler.Setup(x => x.GameStarted()).Returns(false);

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
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;
            Player player = new Player("name1", 0, 0, "@", "1");

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            List<Player> savedPlayers = new List<Player>();
            savedPlayers.Add(player);

            GamePOCO gamePOCO = new GamePOCO();
            gamePOCO.Seed = 1;
            gamePOCO.GameName = "New game";
            gamePOCO.GameGUID = "1";
            gamePOCO.PlayerGUIDHost = "1";

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.SessionId).Returns("1");
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");
            _mockedsessionHandler.Setup(x => x.GameName).Returns("New game");
            _mockedClientController.Setup(x => x.IsHost()).Returns(true);

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            _mockedsessionHandler.Setup(x => x.GameStarted()).Returns(false);

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
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;
            Player player = new Player("name1", 0, 0, "@", "1");

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            List<Player> savedPlayers = new List<Player>();
            savedPlayers.Add(player);

            GamePOCO gamePOCO = new GamePOCO();
            gamePOCO.Seed = 1;
            gamePOCO.GameName = "New game";
            gamePOCO.GameGUID = "1";
            gamePOCO.PlayerGUIDHost = "1";

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.SessionId).Returns("1");
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");
            _mockedsessionHandler.Setup(x => x.GameName).Returns("New game");
            _mockedClientController.Setup(x => x.IsHost()).Returns(true);

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            _mockedsessionHandler.Setup(x => x.GameStarted()).Returns(false);

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
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;
            Player player = new Player("name1", 0, 0, "@", "1");

            List<string[]> allClients = new List<string[]>();
            allClients.Add(new string[] { "1234", "swankie" });
            List<Player> savedPlayers = new List<Player>();
            savedPlayers.Add(player);

            GamePOCO gamePOCO = new GamePOCO();
            gamePOCO.Seed = 1;
            gamePOCO.GameName = "New game";
            gamePOCO.GameGUID = "1";
            gamePOCO.PlayerGUIDHost = "1";

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.SessionId).Returns("1");
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");
            _mockedsessionHandler.Setup(x => x.GameName).Returns("New game");
            _mockedClientController.Setup(x => x.IsHost()).Returns(true);

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            _mockedsessionHandler.Setup(x => x.GameStarted()).Returns(false);

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
            gamePOCO.GameName = "New game";
            gamePOCO.GameGUID = "1";
            gamePOCO.PlayerGUIDHost = "1";

            _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(allClients);
            _mockedsessionHandler.Setup(mock => mock.GetSessionSeed()).Returns(1);
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _mockedClientController.Setup(x => x.SessionId).Returns("1");
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");
            _mockedsessionHandler.Setup(x => x.GameName).Returns("New game");
            _mockedClientController.Setup(x => x.IsHost()).Returns(false);

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
            _mockedWorldService.Setup(x => x.GetAllPlayers()).Returns(savedPlayers);

            _mockedsessionHandler.Setup(x => x.GameStarted()).Returns(false);

            _sut.HandlePacket(_packetDTO);

            _mockedGameConfigurationPoco.Verify(mock => mock.CreateAsync(It.IsAny<GameConfigurationPOCO>()), Times.Never);
        }

        [Test]
        public void Test_If_Started_Saved_Game_Generates_World_WithOldSessionSeed()
        {
            StartGameDTO startGameDto = new StartGameDTO();
            startGameDto.Seed = 1;
            startGameDto.GameGuid = "1";

            PlayerPOCO player = new PlayerPOCO();
            player.PlayerGUID = "1";
            player.GameGUID = "1";
            player.XPosition = 10;
            player.YPosition = 11;

            List<PlayerPOCO> savedPlayers = new List<PlayerPOCO>();
            savedPlayers.Add(player);
            startGameDto.SavedPlayers = savedPlayers;
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

            _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));

            _mockedsessionHandler.Setup(x => x.GameStarted()).Returns(true);

            _sut.HandlePacket(_packetDTO);
            _mockedWorldService.Verify(x => x.GenerateWorld(1), Times.Once);
        }

        [Test]
        public void Test_StartGameDTO_is_null_returns_sendaction_ignore()
        {
            var payload = JsonConvert.SerializeObject(null);

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            _packetDTO.Payload = payload;
            packetHeaderDTO.OriginID = "hostOriginId";
            packetHeaderDTO.SessionID = "1";
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "hostOriginId";
            _packetDTO.Header = packetHeaderDTO;

            HandlerResponseDTO expected = new HandlerResponseDTO(SendAction.Ignore, null);
            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------;
            Assert.AreEqual(expected.Action, actualResult.Action);
            Assert.AreEqual(expected.ResultMessage, actualResult.ResultMessage);
        }

        [Test]
        public void Test_HandlePacket_StatHandler_CurrentPlayer_Get_Sets()
        {
            _mockedsessionHandler.Setup(x => x.GetSavedGame()).Returns(false);
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
            _mockedsessionHandler.Setup(x => x.GetSavedGame()).Returns(false);
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
            _mockedsessionHandler.Setup(x => x.GetSavedGame()).Returns(false);
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