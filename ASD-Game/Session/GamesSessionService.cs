using System.Linq;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Network;
using ASD_Game.Session;
using ASD_Game.UserInterface;
using Castle.Core.Internal;

namespace Session
{
    public class GamesSessionService : IGamesSessionService
    {
        private readonly ISessionHandler _sessionHandler;
        private readonly IDatabaseService<GamePOCO> _gamePocoService;
        private readonly IDatabaseService<PlayerPOCO> _playerPocoService;
        private readonly IScreenHandler _screenHandler;
        private readonly IClientController _clientController; 

        public GamesSessionService(ISessionHandler sessionHandler, IDatabaseService<GamePOCO> gamePocoService,
            IScreenHandler screenHandler, IClientController clientController, IDatabaseService<PlayerPOCO> playerPocoService)
        {
            _sessionHandler = sessionHandler;
            _gamePocoService = gamePocoService;
            _screenHandler = screenHandler;
            _clientController = clientController;
            _playerPocoService = playerPocoService;
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
                var sessions = result.Select(x => new string[] {x.GameGUID, x.GameName}).ToList();

                _screenHandler.UpdateSavedSessionsList(sessions);
            }
        }

        public void LoadGame(string value)
        {
            var allGames = _gamePocoService.GetAllAsync();
            var allPlayers = _playerPocoService.GetAllAsync();
            allPlayers.Wait();
            
            if (allGames.Result.Where(x => x.GameGUID == value).IsNullOrEmpty())
            {
                _screenHandler.UpdateInputMessage("Game cannot be loaded as it does not exist.");
            }
            else
            {
                var gameName = allGames.Result.Where(x => x.GameGUID == value).Select(x => x.GameName).First()
                    .ToString();
                var seed = allGames.Result.Where(x => x.GameGUID == value).Select(x => x.Seed).FirstOrDefault();

                var playerName = allPlayers.Result
                    .FirstOrDefault(x => x.GameGUID == value && x.PlayerGUID == _clientController.GetOriginId())
                    ?.PlayerName;

                _sessionHandler.CreateSession(gameName, playerName, true, value, seed);
            }
        }
    }
}