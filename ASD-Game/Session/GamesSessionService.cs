using System;
using System.Linq;
using Castle.Core.Internal;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;

namespace Session
{
    public class GamesSessionService : IGamesSessionService
    {
        private readonly ISessionHandler _sessionHandler;
        private readonly IDatabaseService<ClientHistoryPoco> _clientHistoryService;
        private readonly IDatabaseService<GamePOCO> _gamePocoService;

        public GamesSessionService(ISessionHandler sessionHandler,
            IDatabaseService<ClientHistoryPoco> clientHistoryService, IDatabaseService<GamePOCO> gamePocoService)
        {
            _sessionHandler = sessionHandler;
            _clientHistoryService = clientHistoryService;
            _gamePocoService = gamePocoService;
        }

        public void RequestSavedGames()
        {
            var allHistory = _clientHistoryService.GetAllAsync();
            var allGames = _gamePocoService.GetAllAsync();
            allHistory.Wait();
            allGames.Wait();

            var joinedTables = from p in allGames.Result
                join pi in allHistory.Result
                    on p.PlayerGUIDHost equals pi.PlayerId
                select new
                {
                    p.PlayerGUIDHost,
                    p.GameGuid,
                };

            if (joinedTables.IsNullOrEmpty())
            {
                Console.WriteLine("There are no saved games");
            }
            else
            {
                foreach (var element in joinedTables.Select(x => x.GameGuid))
                {
                    Console.WriteLine(element);
                }
            }
        }

        private DatabaseService<GamePOCO> GetGameService()
        {
            var gameService = new DatabaseService<GamePOCO>();

            return gameService;
        }

        public void LoadGame(string value)
        {
            var gameService = new DatabaseService<GamePOCO>();

            var allGames = gameService.GetAllAsync();
            allGames.Wait();

            Console.WriteLine("load game with name: ");
            var gameName = Console.ReadLine();
            var seed = allGames.Result.Where(x => x.GameGuid == value).Select(x => x.Seed).FirstOrDefault();
            _sessionHandler.CreateSession(gameName, true, value, seed);
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }
    }
}