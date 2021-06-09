using World.Models.Characters.StateMachine;
using World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine;

namespace WorldGeneration
{
    public class Agent : Character
    {
        public ICharacterStateMachine AgentStateMachine { get; set; }
        public AgentData AgentData { get; set; }

        public Agent(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol, id)
        {
            SetStats(0);
            AgentStateMachine = new AgentStateMachine(AgentData);
        }

        private void SetStats(int difficulty)
        {
            CreateAgentData(difficulty);
        }

        private void CreateAgentData(int difficulty)
        {
            AgentData = new AgentData(XPosition, YPosition, difficulty);
        }
    }
}