using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Moq;
using NUnit.Framework;

namespace Session.Tests
{
    public class GameSessionServiceTest
    {
        private GamesSessionService sut;
        private Mock<IDatabaseService<GamePOCO>> _mockedDatabaseGameService;
        private Mock<IDatabaseService<ClientHistoryPOCO>> _mockedDatabaseClientHistory;
        private Mock<ISessionHandler> _mockedSessionHandler;

        [SetUp]
        public void GameSessionServiceSetup()
        {
            _mockedSessionHandler = new Mock<ISessionHandler>();
            _mockedDatabaseClientHistory = new Mock<IDatabaseService<ClientHistoryPOCO>>();
            _mockedDatabaseGameService = new Mock<IDatabaseService<GamePOCO>>();

            sut = new GamesSessionService(_mockedSessionHandler.Object, _mockedDatabaseClientHistory.Object,
                _mockedDatabaseGameService.Object);
        }

        [Test]
        public void TestIfRequestSavedGamesGivesBackAListWhereIAmHost()
        {
            // Arrange
            List<ClientHistoryPOCO> clientList = new List<ClientHistoryPOCO>();
            List<GamePOCO> gameList = new List<GamePOCO>();

            ClientHistoryPOCO clientHistoryPoco = new ClientHistoryPOCO {GameId = "game1", PlayerId = "player1"};
            GamePOCO gamePoco = new GamePOCO {GameGuid = "game1", PlayerGUIDHost = "player1", GameName = "gameName1"};

            clientList.Add(clientHistoryPoco);
            gameList.Add(gamePoco);

            _mockedDatabaseClientHistory.Setup(x => x.GetAllAsync()).ReturnsAsync(clientList);
            _mockedDatabaseGameService.Setup(x => x.GetAllAsync()).ReturnsAsync(gameList);

            // Lijst wordt gevuld
            var joinedTables = from p in gameList
                join pi in clientList
                    on p.PlayerGUIDHost equals pi.PlayerId
                select new
                {
                    p.PlayerGUIDHost,
                    p.GameGuid,
                    p.GameName
                };

            // check e WriteLine overeenkomen met de gevulde tabel
            using (StringWriter sw = new StringWriter())
            {
                // Act
                Console.SetOut(sw);
                sut.RequestSavedGames();
                var variable = joinedTables.Select(x => new {x.GameGuid, x.GameName}).First();
                string expected = string.Format(variable.GameGuid + " " + variable.GameName + Environment.NewLine);

                // Assert
                Assert.AreEqual(expected, sw.ToString());
            }
        }

        [Test]
        public void TestIfRequestSavedGamesGivesBackAnEmptyList()
        {
            // Arrange
            List<ClientHistoryPOCO> clientList = new List<ClientHistoryPOCO>();
            List<GamePOCO> gameList = new List<GamePOCO>(); // Niet gebruiken want er zijn geen oude spellen
            ClientHistoryPOCO clientHistoryPoco = new ClientHistoryPOCO {GameId = "game1", PlayerId = "player1"};
            clientList.Add(clientHistoryPoco);

            _mockedDatabaseClientHistory.Setup(x => x.GetAllAsync()).ReturnsAsync(clientList);

            using (StringWriter sw = new StringWriter())
            {
                // Act
                Console.SetOut(sw);
                sut.RequestSavedGames();
                string expected = string.Format("There are no saved games" + Environment.NewLine);

                // Assert
                Assert.AreEqual(expected, sw.ToString());
            }
        }

        [Test]
        public void TestLoadGameWhenGameDoesNotExist()
        {
            // Arrange
            List<GamePOCO> gameList = new List<GamePOCO>();
            gameList.Clear();
            
            _mockedDatabaseGameService.Setup(x => x.GetAllAsync()).ReturnsAsync(gameList);
            
            using (StringWriter sw = new StringWriter())
            {
                // Act
                Console.SetOut(sw);
                sut.LoadGame("non-existing-game");
                string expected = string.Format("Game cannot be loaded as it does not exist." + Environment.NewLine);

                // Assert
                Assert.AreEqual(expected, sw.ToString());
            }
        }
        
        [Test]
        public void TestIfCreateSessionIsBeingCalledWhenLoadGameSuccess()
        {
            // Arrange
            List<GamePOCO> gameList = new List<GamePOCO>();

            GamePOCO gamePoco = new GamePOCO {GameGuid = "game1", PlayerGUIDHost = "player1", GameName = "gameName1", Seed = 1};

            gameList.Add(gamePoco);

            _mockedDatabaseGameService.Setup(x => x.GetAllAsync()).ReturnsAsync(gameList);
           
            sut.LoadGame("game1");

            _mockedSessionHandler.Verify(x => x.CreateSession(gamePoco.GameName, true, gamePoco.GameGuid, gamePoco.Seed));
        }
        
    }
}