using Microsoft.Extensions.Logging;
using System;
using DatabaseHandler;
using DatabaseHandler.Poco;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using InputCommandHandler;
using Microsoft.Extensions.Logging.Abstractions;
using Player.Model;
using Player.Services;
using System.Collections.Generic;
using WorldGeneration;
using Agent.Services;
using Chat;
using Session;
using Player.ActionHandlers;
using Player.DTO;

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
            private readonly IRepository<PlayerPoco> _playerRepository;
        //    private readonly IRepository<MainGamePoco> _mainGameRepository;
            private Boolean GameStarted = true;
            private List<PlayerDTO> playerPositions;



            public MainGame(ILogger<MainGame> log, IInventory inventory, IChatHandler chatHandler,
                ISessionHandler sessionHandler, IMoveHandler moveHandler, IRepository<PlayerPoco> playerRepository)
            {
                this._log = log;
                _inventory = inventory;
                _chatHandler = chatHandler;
                _sessionHandler = sessionHandler;
                _moveHandler = moveHandler;
                _playerRepository = playerRepository;
              //  _mainGameRepository = mainGameRepository;
            }

        //needs to be in GameStartupClass? From Session to Game
           //  public void SetupDataBaseForGame1()
           //  {
           //      var tmpServicePlayerPoco = new Services<PlayerPoco>(_playerRepository);
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

                // TODO: Remove from this method, team 2 will provide a command for it
                // AgentConfigurationService agentConfigurationService = new AgentConfigurationService();
                // agentConfigurationService.StartConfiguration();

             //   new WorldGeneration.Program();

                //moet later vervangen worden
                InputCommandHandlerComponent inputHandler = new InputCommandHandlerComponent();
                IPlayerModel playerModel = new PlayerModel("Name", _inventory, new Bitcoin(20), new RadiationLevel(1));
                //lobby start
                //networkcomponent heeft lijst van players -> Session heeft lijst van spelers toch?
                //die players moeten toegevoegd worden aan playerPositions -> Moet de host doen en dit sturen naar de clients wanneer session_start gerund wordt. Kan een list sturen naar alle
                // clients
                playerPositions = new List<PlayerDTO>
                {
                    new PlayerDTO("Joe", 10, 10),
                    new PlayerDTO("Mama", 40, 40)
                };
                IPlayerService playerService = new PlayerService(playerModel, _chatHandler, _sessionHandler,
                    playerPositions, _moveHandler);
                Console.WriteLine("Type input messages below");

                ISessionService sessionService = new SessionService(_sessionHandler);
                
                
                inputHandler.HandleSession(sessionService);
                //
                // if (Game1Started)
                // {
                //     SetupDataBaseForGame1();
                // }
                // else
                // {
                //     //GetGameData
                // } 
                //Buiten commands om Of extra inputHandler die geen Player Nodig heeft. //_SessionHandler gelijk meegeven aan maingame -> Superclasse maybe niet mooi wel makkelijk voor nu
                //Menu
                //Sessions
                    //Create or select player
                        //If select player: Show only the game where the player is part of dan door naar handlecommands met player. 
                        //If create player: Insert name create new player enz dan door naar handlecommands met player
                    //HandleCommands
                    
                    
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
                    inputHandler.HandleSession(sessionService);

                    if (sessionService.inSession)
                    {                        
                        Console.WriteLine("create player");
                        String playername = Console.ReadLine();
                        if (playername.Length != 0)
                        {
                            IPlayerService player = createPlayer(playername);
                            inputHandler.HandleCommands(player);
                        }
                    }
                    
                    //if sessionService.GameStarted 
                    // inputHandler.HandleCommands(playerService);

                    

                }

            }

            private IPlayerService createPlayer(String name)
            {
                IPlayerModel playerModel = new PlayerModel(name, _inventory, new Bitcoin(20), new RadiationLevel(1));
                IPlayerService playerService = new PlayerService(playerModel, _chatHandler, _sessionHandler,
                    playerPositions, _moveHandler);
                return playerService;
            }
        }
    }
}