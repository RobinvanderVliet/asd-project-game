using Microsoft.Extensions.Logging;
using System;
using InputCommandHandler;
using Player.Model;
using Player.Services;
using System.Collections.Generic;
using WorldGeneration;
using WorldGeneration.Models.Interfaces;
using Chat;
using DataTransfer.DTO.Character;
using DataTransfer.DTO.Player;
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

            public MainGame(ILogger<MainGame> log, IInventory inventory, IChatHandler chatHandler,
                ISessionHandler sessionHandler, IMoveHandler moveHandler)
            {
                this._log = log;
                _inventory = inventory;
                _chatHandler = chatHandler;
                _sessionHandler = sessionHandler;
                _moveHandler = moveHandler;
            }

            public void Run()
            {
                Console.WriteLine("Game is gestart");

                // TODO: Remove from this method, team 2 will provide a command for it
                // AgentConfigurationService agentConfigurationService = new AgentConfigurationService();
                // agentConfigurationService.StartConfiguration();

                //moet later vervangen worden
                InputCommandHandlerComponent inputHandler = new InputCommandHandlerComponent();
                List<MapCharacterDTO> players = new List<MapCharacterDTO>();
                players.Add(new MapCharacterDTO(3, 0, "henk"));
                players.Add(new MapCharacterDTO(5, 4, "pietje"));
                IList<ICharacter> characters = new List<ICharacter>();

                
                
                
                
                
                IPlayerModel playerModel = new PlayerModel("Gerard", _inventory, new Bitcoin(20), new RadiationLevel(1));
                MapCharacterDTO playerDTO = new MapCharacterDTO(playerModel.XPosition, playerModel.YPosition,
                    playerModel.Name, playerModel.Symbol);
                var worldService = new WorldService(new World(666, 5, playerDTO));
                worldService.displayWorld();
                //lobby start
                //networkcomponent heeft lijst van players
                //die players moeten toegevoegd worden aan playerPositions
// <<<<<<< HEAD
                List<PlayerDTO> playerPositions = new List<PlayerDTO>
                {
                    new PlayerDTO("Joe", 10, 10),
                    new PlayerDTO("Mama", 40, 40)
                };
                IPlayerService playerService = new PlayerService(playerModel, _chatHandler, _sessionHandler, playerPositions, _moveHandler);
// =======
                // IPlayerService playerService = new PlayerService(playerModel, _chatHandler, _sessionHandler, players, _moveHandler, worldService);
// >>>>>>> sub-task/VDFZEH-376-speler-locatie-aanpassen-bij-beweging
                Console.WriteLine("Type input messages below");
                while (true) // moet vervangen worden met variabele: isQuit 
                {    
                    inputHandler.HandleCommands(playerService);
                }
            }
        }
    }
}