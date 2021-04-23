using Microsoft.Extensions.Logging;
using System;
using Player;
using Agent.Services;
using Chat;

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
                //AgentConfigurationService agentConfigurationService = new AgentConfigurationService();
                //agentConfigurationService.StartConfiguration();
                
                //moet later vervangen worden
                ChatComponent chat = new ChatComponent();
                PlayerModel playerModel = new PlayerModel();
                do
                {
                    chat.HandleCommands(playerModel);
                } while (true); // moet vervangen worden met variabele: isQuit 
                
            }
        }
    }
}
