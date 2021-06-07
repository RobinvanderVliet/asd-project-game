namespace ASD_project.Agent.Models
{
    public class NpcConfiguration : Configuration
    {
        private string _npcName;

        public string NpcName
        {
            get => _npcName;
            set => _npcName = value;
        }
    }
}

