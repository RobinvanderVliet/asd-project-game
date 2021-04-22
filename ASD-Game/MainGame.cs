using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Agent.Services;

namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private readonly ILogger<MainGame> log;

            public MainGame(ILogger<MainGame> log)
            {
                this.log = log;
            }

            public void Run()
            {
                Console.WriteLine("Game is gestart");
                
                // TODO: Remove from this method, team 2 will provide a command for it
                AgentConfigurationService agentConfigurationService = new AgentConfigurationService();
                agentConfigurationService.StartConfiguration();
            }
        }
    }
}
