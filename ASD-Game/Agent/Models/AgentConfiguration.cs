namespace ASD_project.Agent.Models
{
    public class AgentConfiguration : Configuration
    {
        private string _agentName;

        public string AgentName
        {
            get => _agentName;
            set => _agentName = value;
        }
    }
}