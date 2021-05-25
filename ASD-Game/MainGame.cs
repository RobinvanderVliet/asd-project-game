using Microsoft.Extensions.Logging;
using System;
using DatabaseHandler.Poco;
using DatabaseHandler.Repository;
using InputCommandHandler;
using Player.Model;
using Player.Services;
using System.Collections.Generic;
using WorldGeneration;
using Chat;
using DataTransfer.DTO.Character;
using Network;
using Session;
using Player.ActionHandlers;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private readonly ILogger<MainGame> _log;
            private readonly IInventory _inventory;
            private readonly IChatHandler _chatHandler;
            private readonly ISessionHandler _sessionHandler;
            private readonly IMoveHandler _moveHandler;
            private readonly IGameSessionHandler _gameSessionHandler;
            private readonly IRepository<PlayerPoco> _playerRepository;
        //    private readonly IRepository<MainGamePoco> _mainGameRepository;
            private bool GameStarted = true;
            private List<MapCharacterDTO> playerPositions;
            private readonly IClientController _clientController;
            private readonly IWorldService _worldService;


            public MainGame(ILogger<MainGame> log, IInventory inventory, IChatHandler chatHandler,
                ISessionHandler sessionHandler, IMoveHandler moveHandler, IGameSessionHandler gameSessionHandler, IRepository<PlayerPoco> playerRepository, IClientController clientController, IWorldService worldService)
            {
                _log = log;
                _inventory = inventory;
                _chatHandler = chatHandler;
                _sessionHandler = sessionHandler;
                _moveHandler = moveHandler;
                _playerRepository = playerRepository;
                _gameSessionHandler = gameSessionHandler;
                _clientController = clientController;
                _worldService = worldService;
                //  _mainGameRepository = mainGameRepository;
            }

        //needs to be in GameStartupClass? From Session to Game
           //  public void SetupDataBaseForGame1()
           //  {
            //     var tmpServicePlayerPoco = new Services<PlayerPoco>(_playerRepository);
           //      var tmpServiceMainGamePoco = new Services<MainGamePoco>(_mainGameRepository);
           //      
           //      var tmpGuidGame = Guid.NewGuid();
           //      var tmpObject = new MainGamePoco {MainGameGuid = tmpGuidGame, GameName = "Game1"};
           //      var tmpPlayer = new PlayerPoco {PlayerGuid = Guid.NewGuid(), GameGuid = tmpObject, PlayerName = "Player1"};
           //      var tmpPlayer2 = new PlayerPoco {PlayerGuid = Guid.NewGuid(), GameGuid = tmpObject, PlayerName = "Player2"};
           //      
           //      tmpServiceMainGamePoco.CreateAsync(tmpObject);
           //      tmpServicePlayerPoco.CreateAsync(tmpPlayer);
           //      tmpServicePlayerPoco.CreateAsync(tmpPlayer2);
           // //     var result1 = tmpServicePlayerPoco.GetAllAsync();
           //    //  Game1Started = true; 
           //
           //  }

            public void Run()
            {
                Console.WriteLine("Game is gestart");
                InputCommandHandlerComponent inputHandler = new InputCommandHandlerComponent();

                // AgentConfigurationService agentConfigurationService = new AgentConfigurationService(new List<Configuration>(), new FileToDictionaryMapper(), inputHandler);
                // agentConfigurationService.Configure();

                //moet later vervangen worden
                IPlayerModel playerModel = new PlayerModel("Gerard", _inventory, new Bitcoin(20), new RadiationLevel(1));
                IPlayerService playerService = new PlayerService(playerModel, _chatHandler, _sessionHandler, _moveHandler, _clientController, _worldService);

                ISessionService sessionService = new SessionService(_sessionHandler, _gameSessionHandler);
                
                
                // inputHandler.HandleSession(sessionService);
              
                 //OF
                 //Menu
                 //Create or select player/Game
                 //Handlecommands
                 //Sessions
                 
                 //GameSessionHandler
                 //Wordt gestart vanaf startSesion();
                     //Maakt databases aan
                 //Zet spelers op locatie.
                 //Geeft aan dat game gestart is: while boolean op true ?. 
                // inputHandler.HandlePlayer(sessionService);
                
                
               
                while (true) 
                {
                    // Console.WriteLine("create player");
                        // String playername = Console.ReadLine();
                        // if (playername.Length != 0)
                        // {
                        // IPlayerService player = createPlayer(playername);
                        Console.WriteLine("Type input messages below");
                        inputHandler.HandleCommands(playerService, sessionService);
                  
                }
            }

            // private IPlayerService createPlayer(String name, WorldService world)
            // {
            //     IPlayerModel playerModel = new PlayerModel(name, _inventory, new Bitcoin(20), new RadiationLevel(1));
            //     
            //     
            //     playerPositions.Add(new MapCharacterDTO(3, 0, playerModel.Name));
            //     IPlayerService playerService = new PlayerService(playerModel, _chatHandler, _sessionHandler,
            //         playerPositions, _moveHandler);
            //     return playerService;
            // }
        }
    }
}