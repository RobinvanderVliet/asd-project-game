// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using ASD_Game.DatabaseHandler.POCO;
// using ASD_Game.DatabaseHandler.Services;
// using ASD_Game.Network;
// using ASD_Game.Session;
// using ASD_Game.UserInterface;
// using Moq;
// using NUnit.Framework;
//
// namespace Session.Tests
// {
//     public class GameSessionServiceTest
//     {
//         private GamesSessionService sut;
//         private Mock<IDatabaseService<GamePOCO>> _mockedDatabaseGameService;
//         private Mock<ISessionHandler> _mockedSessionHandler;
//         private Mock<IScreenHandler> _mockedScreenHandler;
//         private Mock<IClientController> _mockedClientController;
//
//         [SetUp]
//         public void GameSessionServiceSetup()
//         {
//             _mockedSessionHandler = new Mock<ISessionHandler>();
//             _mockedDatabaseGameService = new Mock<IDatabaseService<GamePOCO>>();
//             _mockedScreenHandler = new Mock<IScreenHandler>();
//             _mockedClientController = new Mock<IClientController>();
//
//
//             sut = new GamesSessionService(_mockedSessionHandler.Object, _mockedDatabaseGameService.Object,
//                 _mockedScreenHandler.Object, _mockedClientController.Object);
//         }
//
//         /// <summary>
//         ///  [RequestSavedGames()]
//         ///
//         /// [Description of the test]
//         /// If Request Saved Games Gives Back A List Where I Am Host
//         /// </summary>
//         [Test]
//         public void Test_RequestSavedGames_IfRequestSavedGamesGivesBackAListWhereIAmHost()
//         {
//             // Arrange
//             List<GamePOCO> gameList = new List<GamePOCO>();
//
//             GamePOCO gamePoco = new GamePOCO {GameGUID = "game1", PlayerGUIDHost = "player1", GameName = "gameName1"};
//
//             gameList.Add(gamePoco);
//
//             _mockedClientController.Setup(x => x.GetOriginId()).Returns("player1");
//
//             _mockedDatabaseGameService.Setup(x => x.GetAllAsync()).ReturnsAsync(gameList);
//             sut.RequestSavedGames();
//             
//             
//             _mockedScreenHandler.Verify(x => x.UpdateSavedSessionsList(It.IsAny<List<string[]>>()),Times.Once);
//         }
//
//         /// <summary>
//         ///  [RequestSavedGames()]
//         ///
//         /// [Description of the test]
//         /// If Request Saved Games Gives Back An Empty List
//         /// </summary> 
//         [Test]
//         public void Test_RequestSavedGames_Updates_Input_Message_When_No_Saved_Games()
//         {
//             // Arrange
//             List<GamePOCO> gameList = new List<GamePOCO>(); 
//             
//             _mockedClientController.Setup(x => x.GetOriginId()).Returns("player1");
//
//             _mockedDatabaseGameService.Setup(x => x.GetAllAsync()).ReturnsAsync(gameList);
//             
//             sut.RequestSavedGames();
//
//             _mockedScreenHandler.Verify(x => x.UpdateInputMessage("No saved sessions found, type 'return' to go back to main menu!"), Times.Once);
//             
//         
//         }
//
//         /// <summary>
//         ///  [LoadGame()]
//         ///
//         /// [Description of the test]
//         /// Load Game When Game Does Not Exist
//         /// </summary> 
//         [Test]
//         public void Test_LoadGame_LoadGameWhenGameDoesNotExist()
//         {
//             // Arrange
//             List<GamePOCO> gameList = new List<GamePOCO>();
//             gameList.Clear();
//
//             _mockedDatabaseGameService.Setup(x => x.GetAllAsync()).ReturnsAsync(gameList);
//
//             using (StringWriter sw = new StringWriter())
//             {
//                 // Act
//                 Console.SetOut(sw);
//                 sut.LoadGame("non-existing-game");
//                 
//                 // Assert
//                 _mockedScreenHandler.Verify(x => x.UpdateInputMessage("Game cannot be loaded as it does not exist."),
//                     Times.Once());
//             }
//         }
//
//         /// <summary>
//         ///  [LoadGame()]
//         ///
//         /// [Description of the test]
//         /// If Create Session Is Being Called When Load Game Success
//         /// </summary> 
//         [Test]
//         public void Test_LoadGame_Creates_Session()
//         {
//             // Arrange
//             List<GamePOCO> gameList = new List<GamePOCO>();
//
//             GamePOCO gamePoco = new GamePOCO
//                 {GameGUID = "game1", PlayerGUIDHost = "player1", GameName = "gameName1", Seed = 1};
//
//             gameList.Add(gamePoco);
//
//             _mockedDatabaseGameService.Setup(x => x.GetAllAsync()).ReturnsAsync(gameList);
//
//             sut.LoadGame("game1");
//
//             _mockedSessionHandler.Verify(
//                 x => x.CreateSession(gamePoco.GameName, "gerrie", true, gamePoco.GameGUID, gamePoco.Seed));
//         }
//     }
// }