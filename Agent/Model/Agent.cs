namespace Agent.Model
{
    public class Agent : IAgent
    {
        private AgentConfiguration _agentConfiguration;
        
        
        public void LoadConfiguration(AgentConfiguration agentConfiguration)
        {
            _agentConfiguration = agentConfiguration;
        }

        public void SwitchAgent(AgentStatus status)
        {
            if (status.Equals(AgentStatus.AgentOn))
            {
                // TODO: Activate agent logic and mark it somewhere as activated
                executeLogic();
            }
            else
            {
                // TODO: Deactivate agent logic and mark it somewhere as deactivated
            }
        }

        private void executeLogic()
        {
            
        }
    }
}