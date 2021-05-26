using System;
using System.Numerics;
using Agent.Services;
using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using InputCommandHandler;
using Player.Model;

namespace Creature.Services
{
    // TODO: integrate with world generation for creature creation
    // TODO: integrate with group 1 for difficulty
    public class AgentService : IAgentService
    {
        private ICreature _agent;
        private bool _isActivated;

        public AgentService(AgentConfigurationService agentConfigurationService, IPlayerModel playerModel)
        {
            var agentData = new PlayerData(new Vector2(playerModel.XPosition, playerModel.YPosition),
                playerModel.Health, playerModel.GetAttackDamage(), 10, null);

            var agentStateMachine = new PlayerStateMachine(agentData,
                new RuleSet(agentConfigurationService.GetConfigurations()[0].Settings));

            _agent = new Player(agentStateMachine);
        }

        public void Activate()
        {
            _isActivated = true;
            _agent.CreatureStateMachine.StartStateMachine();
        }

        public void DeActivate()
        {
            _isActivated = false;
            _agent.CreatureStateMachine.StopStateMachine();
        }

        public bool IsActivated()
        {
            return _isActivated;
        }
    }
}