using Agent.Model;

namespace Agent.Services
{
    public class AgentService : IAgentService
    {
        private readonly IAgent _agent;

        public AgentService(IAgent agent)
        {
            _agent = agent;
        }

        
        public void EnableAgent()
        {
            _agent.EnableAgent(AgentStatus.AgentOn);
        }

        public void DisableAgent()
        {
            _agent.EnableAgent(AgentStatus.AgentOff);
        }

        public void ConfigureAgent()
        {
            // start configuration terminal
        }
    }
}