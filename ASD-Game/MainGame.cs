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
            private readonly IDbConnection _connection;
            private readonly IRepository<PlayerPoco> _playerRepository;
            private readonly IRepository<MainGamePoco> _mainGameRepository;

            public MainGame(ILogger<MainGame> log, IDbConnection connection, IRepository<PlayerPoco> playerRepository, IRepository<MainGamePoco> mainGameRepository)
            {
                _log = log;
                _connection = connection;
                _playerRepository = playerRepository;
                _mainGameRepository = mainGameRepository;
            }

            public void Run()
            {
                Console.WriteLine("Game is gestart");
                _connection.SetForeignKeys();
                var tmpServicePlayerPoco = new Services<PlayerPoco>(_playerRepository);
                var tmpServiceMainGamePoco = new Services<MainGamePoco>(_mainGameRepository);
                
                var tmpGuidGame = Guid.NewGuid();
                var tmpObject = new MainGamePoco {MainGameGuid = tmpGuidGame};
                var tmpPlayer = new PlayerPoco {PlayerGuid = Guid.NewGuid(), GameGuid = tmpObject};


                tmpServiceMainGamePoco.CreateAsync(tmpObject);
                tmpServicePlayerPoco.CreateAsync(tmpPlayer);
                var result1 = tmpServicePlayerPoco.GetAllAsync();
                result1.Wait();
                var result2 = tmpServiceMainGamePoco.GetAllAsync();
                result2.Wait();
                Console.WriteLine("lol");
            }
        }
    }
}
