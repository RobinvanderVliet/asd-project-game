using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Moq;
using NUnit.Framework;

namespace Session.Tests
{
    public class GameSessionServiceTest
    {
        private GamesSessionService sut;
        private Mock<IDatabaseService<GamePOCO>> _mockedDatabaseGameService;
        private Mock<IDatabaseService<ClientHistoryPoco>> _mockedDatabaseClientHistory;
        private Mock<ISessionHandler> _mockedSessionHandler;

        [SetUp]
        public void GameSessionServiceSetup()
        {
            _mockedSessionHandler = new Mock<ISessionHandler>();
            _mockedDatabaseClientHistory = new Mock<IDatabaseService<ClientHistoryPoco>>();
            _mockedDatabaseGameService = new Mock<IDatabaseService<GamePOCO>>();

            sut = new GamesSessionService(_mockedSessionHandler.Object, _mockedDatabaseClientHistory.Object,
                _mockedDatabaseGameService.Object);
        }

        [Test]
        public void TestIfRequestSavedGamesGivesBackAListWhereIAmHost()
        {
            // Arrange
            List<ClientHistoryPoco> clientList = new List<ClientHistoryPoco>();
            List<GamePOCO> gameList = new List<GamePOCO>();

            ClientHistoryPoco clientHistoryPoco = new ClientHistoryPoco {GameId = "game1", PlayerId = "player1"};
            GamePOCO gamePoco = new GamePOCO {GameGuid = "game1", PlayerGUIDHost = "player1"};

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
                };
            
            // check e WriteLine overeenkomen met de gevulde tabel
            using (StringWriter sw = new StringWriter())
            {
                // Act
                Console.SetOut(sw);
                sut.RequestSavedGames();
                string stom = joinedTables.Select(x => x.GameGuid).First().ToString();
                string expected = string.Format(stom + Environment.NewLine);

                // Assert
                Assert.AreEqual(expected, sw.ToString());
            }
        }
        
        [Test]
        public void TestIfRequestSavedGamesGivesBackAnEmptyList()
        {
            // Arrange
            List<ClientHistoryPoco> clientList = new List<ClientHistoryPoco>();
            List<GamePOCO> gameList = new List<GamePOCO>(); // niet gebruiken want er zijn geen oude spellen
            ClientHistoryPoco clientHistoryPoco = new ClientHistoryPoco {GameId = "game1", PlayerId = "player1"};
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
    }
}