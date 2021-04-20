namespace Agent.Model
{
    public class Agent
    {
        private AgentConfiguration _agentConfiguration;
        
        
        public void LoadConfiguration(AgentConfiguration agentConfiguration)
        {
            _agentConfiguration = agentConfiguration;
            
            
        }
    }
}