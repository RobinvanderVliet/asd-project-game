using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Network;
using UserInterface;

namespace Session
{
    public class GamesSessionService : IGamesSessionService
    {
        private readonly ISessionHandler _sessionHandler;
        private readonly IDatabaseService<ClientHistoryPOCO> _clientHistoryService;
        private readonly IDatabaseService<GamePOCO> _gamePocoService;
        private readonly IScreenHandler _screenHandler;
        private readonly IClientController _clientController; 

        public GamesSessionService(ISessionHandler sessionHandler,
            IDatabaseService<ClientHistoryPOCO> clientHistoryService, IDatabaseService<GamePOCO> gamePocoService, IScreenHandler screenHandler, IClientController clientController)
        {
            _sessionHandler = sessionHandler;
            _clientHistoryService = clientHistoryService;
            _gamePocoService = gamePocoService;
            _screenHandler = screenHandler;
            _clientController = clientController;
        }

        public void RequestSavedGames()
        {
            var allGames = _gamePocoService.GetAllAsync();
            allGames.Wait();
            var result = allGames.Result.Where(x => x.PlayerGUIDHost.Equals(_clientController.GetOriginId()));
       

            if (result.IsNullOrEmpty())
            {
                _screenHandler.UpdateInputMessage("No saved sessions found, type 'return' to go back to main menu!");
            }
            else
            {
                var sessions = result.Select(x => new string[] {x.GameGuid, x.GameName}).ToList();

                _screenHandler.UpdateSavedSessionsList(sessions);
            }
        }

        public void LoadGame(string value)
        {
            var allGames = _gamePocoService.GetAllAsync();

            if (allGames.Result.Where(x => x.GameGuid == value).IsNullOrEmpty())
            {
                Console.WriteLine("Game cannot be loaded as it does not exist.");
            }
            else
            {
                var gameName = allGames.Result.Where(x => x.GameGuid == value).Select(x => x.GameName).First()
                    .ToString();
                var seed = allGames.Result.Where(x => x.GameGuid == value).Select(x => x.Seed).FirstOrDefault();
                //get host userName from somewhere.
                _sessionHandler.CreateSession(gameName, "gerrie", true, value, seed);
            }
        }
    }

    
}