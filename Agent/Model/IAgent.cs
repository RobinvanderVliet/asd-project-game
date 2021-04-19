namespace Agent.Model
{
    public interface IAgent
    {
        public void SwitchAgent();

        public void LoadConfiguration(AgentConfiguration agentConfiguration);

        public void ExecuteAttackAction();

        public void ExecuteCollectAction();

        public void ExecuteCheckAction();
        
        public void ExecuteCommand();
    }
}