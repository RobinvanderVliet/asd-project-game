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
            _agent.EnableAgent();
        }

        public void DisableAgent()
        {
            
        }

        public void ConfigureAgent()
        {
            // start configuration terminal
        }
    }
}