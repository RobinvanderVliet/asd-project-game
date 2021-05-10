using Microsoft.Extensions.Logging;
using System;
using InputCommandHandler;
using Player.Model;
using Player.Services;
using Microsoft.Extensions.Logging;
using System;
using WorldGeneration;
using Player;
using Agent.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private List<IEvent> ListEvent = new List<IEvent>();
            private readonly IEvent _gameEvents;
            private readonly ILogger<MainGame> _log;

            public MainGame(ILogger<MainGame> log, IEvent gameEvents)
            {
                this._log = log;
                _gameEvents = gameEvents;
            }



            public void Run()
            {
                Console.WriteLine("Game is gestart");

                // TODO: Remove from this method, team 2 will provide a command for it
                // AgentConfigurationService agentConfigurationService = new AgentConfigurationService();
                // agentConfigurationService.StartConfiguration();
                
                //new WorldGeneration.Program();
                
                //moet later vervangen worden
                InputCommandHandlerComponent inputHandler = new InputCommandHandlerComponent();
                PlayerModel playerModel = new PlayerModel("Name", new Inventory(), new Bitcoin(20), new RadiationLevel(1));
                IPlayerService playerService = new PlayerService(playerModel, _gameEvents); 
                Console.WriteLine("Type input messages below");
                while (true) // moet vervangen worden met variabele: isQuit 
                {
                    var temp = ListEvent;

                   
                    inputHandler.HandleCommands(playerService);
                    if (playerService.GetEvents().Count > 0)
                    {
                        addEvent(playerService.GetEvents());
                    }
                }
            }

            public void addEvent(List<IEvent> gameEvent)
            {
                ListEvent.Add(gameEvent.Last());
            }

         
        }
    }
}
