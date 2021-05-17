using Microsoft.Extensions.Logging;
using System;
using DatabaseHandler;
using DatabaseHandler.Poco;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using InputCommandHandler;
using Player.Model;
using Player.Services;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private readonly ILogger<MainGame> _log;
            private readonly IRepository<PlayerPoco> _repository;
            private readonly IRepository<MainGamePoco> _repositoryMainGame;
            

            public MainGame(ILogger<MainGame> log, IRepository<PlayerPoco> repository, IRepository<MainGamePoco> repositoryMainGame)
            {
                this._log = log;
                _repository = repository;
                _repositoryMainGame = repositoryMainGame;
            }

            public void Run()
            {
                Console.WriteLine("Game is gestart");

                // TODO: Remove from this method, team 2 will provide a command for it
                // AgentConfigurationService agentConfigurationService = new AgentConfigurationService();
                // agentConfigurationService.StartConfiguration();

                // new WorldGeneration.Program();

                // //moet later vervangen worden
                // InputCommandHandlerComponent inputHandler = new InputCommandHandlerComponent();
                // PlayerModel playerModel = new PlayerModel("Name", new Inventory(), new Bitcoin(20), new RadiationLevel(1));
                // IPlayerService playerService = new PlayerService(playerModel); 
                // Console.WriteLine("Type input messages below");
                // while (true) // moet vervangen worden met variabele: isQuit 
                // {
                //     inputHandler.HandleCommands(playerService);
                // }

                var tmp = new DbConnection();
              
                var tmpServicePlayerPoco = new Services<PlayerPoco>(_repository);
                var tmpServiceMainGamePoco = new Services<MainGamePoco>(_repositoryMainGame);

                var tmpGuidGame = Guid.NewGuid();

                var tmpObject = new MainGamePoco {MainGameGuid = Guid.NewGuid()};
                var tmpPlayer = new PlayerPoco {PlayerGuid = Guid.NewGuid(), GameGuid = tmpObject};



                tmp.SetConnectionString("C:\\Temp\\ChunkDatabase.db");
                tmp.SetForeignKeys();

               tmpServiceMainGamePoco.InsertAsync(tmpObject);
               tmpServicePlayerPoco.InsertAsync(tmpPlayer);

               var result1 = tmpServicePlayerPoco.GetAllAsync();
               result1.Wait(); 

               var result2 = tmpServiceMainGamePoco.GetAllAsync();
               result2.Wait();

               
               
               
               
               Console.WriteLine(result1);
               Console.WriteLine(result2);

            }
        }
    }
}