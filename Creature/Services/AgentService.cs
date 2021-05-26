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
    // TODO: if host use playerIds to replace player with agent
    public class AgentService : IAgentService
    {
        private ICreature _agent;
        private bool _isActivated = false;

        public AgentService(BaseConfigurationService agentConfigurationService, IPlayerModel playerModel)
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
            if (_isActivated) _agent.CreatureStateMachine.StopStateMachine();
            _isActivated = false;
        }

        public bool IsActivated()
        {
            return _isActivated;
        }
    }
}