using System;
using System.Linq;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;

namespace Session
{
    public class GamesSessionService : IGamesSessionService
    {
        private readonly IGameSessionHandler _gameSessionHandler;

        public GamesSessionService(IGameSessionHandler gameSessionHandler)
        {
            _gameSessionHandler = gameSessionHandler;
        }

        public void RequestSavedGames()
        {
            // return list met alle games waar ik host ben
            var tmp = new DbConnection();
                    
            var clientHistoryRepository = new Repository<ClientHistoryPoco>(tmp);
            var clientHistory = new ServicesDb<ClientHistoryPoco>(clientHistoryRepository);
            var gameRepository = new Repository<GamePOCO>(tmp);
            var gameService = new ServicesDb<GamePOCO>(gameRepository);

            var allHistory = clientHistory.GetAllAsync();
            var allGames = gameService.GetAllAsync();
            allHistory.Wait();
            allGames.Wait();

            // join 2 tabels on eachother
            var joinedTables = from p in allGames.Result
                               join pi in allHistory.Result
                               on p.PlayerGUIDHost equals pi.PlayerId
                               select new
                               {
                                   p.PlayerGUIDHost,
                                   p.GameGuid
                               };

            foreach(var element in joinedTables)
            {
                Console.WriteLine(element);
            }
        }

        public void LoadGame(string value)
        {
            Console.WriteLine("load game " + value);
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }
    }
}