// using System;
// using System.Collections.Generic;
// using System.Diagnostics.CodeAnalysis;
// using System.IO;
// using Moq;
// using Network;
// using Newtonsoft.Json;
// using NUnit.Framework;
// using Session.DTO;
// using Session.GameConfiguration;
// using System;
// using System.Diagnostics.CodeAnalysis;
// using System.IO;
// using ActionHandling;
// using DatabaseHandler.POCO;
// using DatabaseHandler.Services;
// using DatabaseHandler.POCO;
// using DatabaseHandler.Services;
// using Session.DTO;
// using WorldGeneration;
// using DatabaseHandler.Services;
// using DatabaseHandler.POCO;
//
// namespace Session.Tests
// {
//     [ExcludeFromCodeCoverage]
//     public class GameSessionHandlerTests
//     {
//         private GameSessionHandler _sut;
//
//         private PacketDTO _packetDTO;
//
//         //Declaration of mocks
//         private Mock<IClientController> _mockedClientController;
//         private Mock<IWorldService> _mockedWorldService;
//         private Mock<ISessionHandler> _mockedsessionHandler;
//         private Mock<IDatabaseService<GamePOCO>> _mockedGamePOCOServices;
//         private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerPOCOServices;
//         private IDatabaseService<PlayerPOCO> _services;
//         private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerDatabaseService;
//         private Mock<IDatabaseService<GamePOCO>> _mockedGameDatabaseService;
//         private Mock<IDatabaseService<PlayerItemPOCO>> _mockedPlayerItemDatabaseService;
//         private Mock<IRelativeStatHandler> _mockedRelativeStatHandler;
//
//         private Mock<IGameConfigurationHandler> _mockedGameConfigurationHandler;
//         private Mock<IDatabaseService<GameConfigurationPOCO>> _mockedGameConfigDatabaseService;
//
//
//         [SetUp]
//         public void Setup()
//         {
//             var standardOutput = new StreamWriter(Console.OpenStandardOutput());
//             standardOutput.AutoFlush = true;
//             Console.SetOut(standardOutput);
//             _mockedClientController = new Mock<IClientController>();
//             _mockedPlayerPOCOServices = new Mock<IDatabaseService<PlayerPOCO>>();
//             _mockedWorldService = new Mock<IWorldService>();
//             _mockedsessionHandler = new Mock<ISessionHandler>();
//             _mockedGamePOCOServices = new Mock<IDatabaseService<GamePOCO>>();
//
//             _sut = new GameSessionHandler(_mockedClientController.Object, _mockedWorldService.Object,
//                 _mockedsessionHandler.Object, _mockedGamePOCOServices.Object, _mockedPlayerPOCOServices.Object);
//             _mockedGameConfigurationHandler = new Mock<IGameConfigurationHandler>();
//             _mockedGameConfigDatabaseService = new Mock<IDatabaseService<GameConfigurationPOCO>>();
//             _mockedPlayerItemDatabaseService = new Mock<IDatabaseService<PlayerItemPOCO>>();
//             _mockedRelativeStatHandler = new Mock<IRelativeStatHandler>();
//             _mockedPlayerDatabaseService = new Mock<IDatabaseService<PlayerPOCO>>();
//             _mockedGameDatabaseService = new Mock<IDatabaseService<GamePOCO>>();
//            // _sut = new GameSessionHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedsessionHandler.Object, _mockedPlayerServiceDb.Object, _mockedgameDatabaseService.Object, _mockedGameConfigDatabaseService.Object, _mockedGameConfigurationHandler.Object);
//             _packetDTO = new PacketDTO();
//         }
//
//         /// <summary>
//         ///  [SendGameSession()]
//         ///
//         /// [Description of the test]
//         /// Als game is saved dan moet StartGameDTO gevuld worden met bestaande items
//         /// </summary>
//         [Test]
//         public void Test_SendGameSession_FillsStartGameDTO()
//         {
//             _mockedsessionHandler.Setup(x => x.GetSavedGame()).Returns(true);
//             StartGameDTO startGameDto = new StartGameDTO();
//             List<PlayerPOCO> savedPlayers = new List<PlayerPOCO>();
//             PlayerPOCO player = new PlayerPOCO
//             {
//                 GameGuid = "GameGuid1", Health = 1, Stamina = 1, PlayerGuid = "GameGuid1Player1",
//                 GameGUIDAndPlayerGuid = "GameGuid1Player1", PlayerName = "Player1", TypePlayer = 1, XPosition = 0,
//                 YPosition = 0
//             };
//
//             Dictionary<string, int[]> dictPlayer = new Dictionary<string, int[]>();
//             int[] playerPosition = new int[2];
//             playerPosition[0] = 0;
//             playerPosition[1] = 0;
//             dictPlayer.Add("GameGuid1Player1", playerPosition);
//
//             savedPlayers.Add(player);
//
//             startGameDto.Seed = 0;
//             startGameDto.SavedPlayers = savedPlayers;
//             startGameDto.GameGuid = "GameGuid1";
//             startGameDto.PlayerLocations = dictPlayer;
//
//             _mockedClientController.Setup(x => x.SessionId).Returns("GameGuid1");
//
//
//             StartGameDTO sendGameDTO = new StartGameDTO
//             {
//                 Seed = startGameDto.Seed,
//                 SavedPlayers = startGameDto.SavedPlayers,
//                 PlayerLocations = dictPlayer,
//                 ExistingPlayer = null
//             };
//
//             _mockedPlayerPOCOServices.Setup(x => x.GetAllAsync()).ReturnsAsync(savedPlayers);
//             _mockedsessionHandler.Setup(x => x.GetSessionSeed()).Returns(1);
//             _sut.SendGameSession();
//             var payload = JsonConvert.SerializeObject(sendGameDTO);
//             _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
//             _mockedClientController.Verify(x => x.SendPayload(payload, PacketType.GameSession), Times.Once);
//         }
//
//         /// <summary>
//         ///  [SendGameSession()]
//         ///
//         /// [Description of the test]
//         /// Test If New Game Is Started Setups New Game State With New Location For Player
//         /// </summary>
//         [Test]
//         public void Test_SendGameSession_SetupGameStateWithNewLocations()
//         {
//             _mockedsessionHandler.Setup(x => x.GetSavedGame()).Returns(false);
//             StartGameDTO startGameDto = new StartGameDTO();
//
//
//             Dictionary<string, int[]> dictPlayer = new Dictionary<string, int[]>();
//             int[] playerPosition = new int[2];
//             playerPosition[0] = 26;
//             playerPosition[1] = 11;
//             dictPlayer.Add("GameGuid1Player1", playerPosition);
//
//
//             startGameDto.Seed = 0;
//             startGameDto.GameGuid = "GameGuid1";
//             startGameDto.PlayerLocations = dictPlayer;
//
//             _mockedClientController.Setup(x => x.SessionId).Returns("GameGuid1");
//
//             List<string> allClients = new List<string>();
//             allClients.Add("GameGuid1Player1");
//
//
//             StartGameDTO sendGameDTO = new StartGameDTO
//             {
//                 Seed = startGameDto.Seed,
//                 PlayerLocations = dictPlayer,
//                 ExistingPlayer = null,
//                 GameGuid = startGameDto.GameGuid
//             };
//             _mockedsessionHandler.Setup(x => x.GetAllClients()).Returns(allClients);
//             _mockedsessionHandler.Setup(x => x.GetSessionSeed()).Returns(1);
//             _sut.SendGameSession();
//             var payload = JsonConvert.SerializeObject(sendGameDTO);
//             _mockedClientController.Setup(x => x.SendPayload(payload, PacketType.GameSession));
//             _mockedClientController.Verify(x => x.SendPayload(payload, PacketType.GameSession), Times.Once);
//         }
//
//         /// <summary>
//         ///  [HandleStartGameSession()]
//         ///
//         /// [Description of the test]
//         /// Players toevoegen aan de game met de seed
//         /// </summary>
//         // [Test]
//         // public void Test_HandleStartGameSession()
//         // {
//         //     // Arrange
//         //     StartGameDTO startGameDto = new StartGameDTO();
//         //     startGameDto.Seed = 0;
//         //     startGameDto.GameGuid = "GameGuid1";
//         //     PlayerPOCO player = new PlayerPOCO
//         //     {
//         //         GameGuid = "GameGuid1", Health = 1, Stamina = 1, PlayerGuid = "GameGuid1Player1",
//         //         GameGUIDAndPlayerGuid = "GameGuid1Player1", PlayerName = "Player1", TypePlayer = 1, XPosition = 0,
//         //         YPosition = 0
//         //     };
//         //     startGameDto.ExistingPlayer = player;
//         //
//         //     _mockedClientController.Setup(x => x.GetOriginId()).Returns(startGameDto.ExistingPlayer.GameGuid);
//         //
//         //     // Act
//         //
//         //     // Assert
//         //     _sut.HandleStartGameSEssion(startGameDto);
//         // }
//
//
//     }
// }