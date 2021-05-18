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

                var tmp = new DbConnection();
                tmp.SetForeignKeys();
                
                var repository = new Repository<PlayerPoco>();
                var repositoryMainGame = new Repository<MainGamePoco>();
                var tmpServicePlayerPoco = new Services<PlayerPoco>(repository, new NullLogger<Services<PlayerPoco>>());
                var tmpServiceMainGamePoco = new Services<MainGamePoco>(repositoryMainGame, new NullLogger<Services<MainGamePoco>>());
                var tmpGuidGame = Guid.NewGuid();
                var tmpObject = new MainGamePoco {MainGameGuid = Guid.NewGuid()};
                var tmpPlayer = new PlayerPoco {PlayerGuid = Guid.NewGuid(), GameGuid = tmpObject};


                tmpServiceMainGamePoco.CreateAsync(tmpObject);
                tmpServicePlayerPoco.CreateAsync(tmpPlayer);
                var result1 = tmpServicePlayerPoco.GetAllAsync();
                result1.Wait();
                var result2 = tmpServiceMainGamePoco.GetAllAsync();
                result2.Wait();
            }
        }
    }
}
