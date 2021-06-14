namespace ASD_Game.Agent.Models
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

