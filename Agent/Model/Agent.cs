namespace Agent.Model
{
    public class Agent : IAgent
    {
        private AgentConfiguration _agentConfiguration;
        
        
        public void LoadConfiguration(AgentConfiguration agentConfiguration)
        {
            _agentConfiguration = agentConfiguration;
        }

        public void EnableAgent()
        {
            executeLogic();
        }

        private void executeLogic()
        {
            
        }
    }
}