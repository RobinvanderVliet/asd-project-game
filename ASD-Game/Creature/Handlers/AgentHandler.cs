using System;
using System.Collections.Generic;
using Agent.Mapper;
using Agent.Models;
using Agent.Services;
using Creature.Creature;
using InputHandling;
using WorldGeneration;

namespace Creature
{
    public class AgentHandler : IAgentHandler
    {
        private bool replaced;
        private ICreature _creature;
        private IWorldService _worldService;
        private AgentConfigurationService _agentConfigurationService;

        public AgentHandler(IWorldService worldService)
        {
            _worldService = worldService;
            _agentConfigurationService = new AgentConfigurationService(new List<Configuration>(), new FileToDictionaryMapper(), new InputHandler());
            // TODO: create agent and use correct configuration service
            //_agentConfigurationService.CreateConfiguration("piet", Directory.GetCurrentDirectory() + "\\Resource\\agentfile.cfg");
        }
        
        public void Replace()
        {
            replaced = !replaced;
            
            // TODO: replace this with actually activating or de-activating agent using statemachine
            Console.WriteLine(replaced ? "De-activating your agent." : "Activating your agent.");
        }
    }
}