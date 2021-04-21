namespace Agent.Model
{
    public interface IAgent
    {
        void LoadConfiguration(AgentConfiguration agentConfiguration);
        void SwitchAgent(AgentStatus status);
    }
}