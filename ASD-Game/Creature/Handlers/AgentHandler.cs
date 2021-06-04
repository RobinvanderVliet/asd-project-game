using System;
using System.Collections.Generic;
using System.Numerics;
using ActionHandling;
using Agent.Mapper;
using Agent.Models;
using Agent.Services;
using Creature.Creature;
using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;
using InputHandling;
using WorldGeneration;

namespace Creature
{
    public class AgentHandler : IAgentHandler
    {
        private bool replaced;
        private ICreature _creature;
        private IMoveHandler _moveHandler;
        private IWorldService _worldService;
        private AgentConfigurationService _agentConfigurationService;

        // TODO: add attack handler when that is on develop
        public AgentHandler(IWorldService worldService, IMoveHandler moveHandler)
        {
            _worldService = worldService;
            _agentConfigurationService = new AgentConfigurationService(new List<Configuration>(), new FileToDictionaryMapper(), new InputHandler());
            _moveHandler = moveHandler;
            // TODO: create agent and use correct configuration service
            //_agentConfigurationService.CreateConfiguration("piet", Directory.GetCurrentDirectory() + "\\Resource\\agentfile.cfg");
        }
        
        public void Replace()
        {
            replaced = !replaced;
            if (replaced)
            {
                var player = _worldService.getCurrentPlayer();
                
                // TODO: damage and world should not be in there
                // TODO: after roy merge into develop 
                
                // var playerData = new PlayerData(new Vector2(player.XPosition, player.YPosition), player.Health, 10, 6, null, _moveHandler);
                //
                // var playerStateMachine = new PlayerStateMachine(playerData);
                // _creature = new Creature.Player(playerStateMachine);
                // _creature.CreatureStateMachine.StartStateMachine();
            }
            else
            {
                _creature.CreatureStateMachine.StopStateMachine();
            }
        }
    }
}