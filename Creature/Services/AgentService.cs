using InputCommandHandler;

namespace Creature.Services
{
    // TODO: integrate with world generation for creature creation
    // TODO: integrate with group 1 for difficulty
    public class AgentService : IAgentService
    {
        private ICreature _agent;
        private bool _isActivated;

        public AgentService(ICreature agent)
        {
            _agent = agent;
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