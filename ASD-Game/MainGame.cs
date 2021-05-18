using Microsoft.Extensions.Logging;
using System;
using InputCommandHandler;
using Player.Model;
using Player.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration;
using Player;
using Agent.Services;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;
using Player = WorldGeneration.Player;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private readonly ILogger<MainGame> _log;

            public MainGame(ILogger<MainGame> log)
            {
                this._log = log;
            }

            public void Run()
            {
                Console.WriteLine("Game is gestart");

                // TODO: Remove from this method, team 2 will provide a command for it
                // AgentConfigurationService agentConfigurationService = new AgentConfigurationService();
                // agentConfigurationService.StartConfiguration();

                //moet later vervangen worden
                InputCommandHandlerComponent inputHandler = new InputCommandHandlerComponent();
                IList<IPlayer> players = new List<IPlayer>();
                players.Add(new WorldGeneration.Player("henk", 3,0));
                players.Add(new WorldGeneration.Player("pietje", 2, 1));
                    
               
                World world = new World(players, 66666666);
                world.DisplayWorld(8, players.First());
                
                Console.WriteLine("Type input messages below");
            }
        }
    }
}
