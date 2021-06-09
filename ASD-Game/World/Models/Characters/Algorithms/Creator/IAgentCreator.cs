using System.Collections.Generic;

namespace ASD_Game.World.Models.Characters.Algorithms.Creator
{
    public interface IAgentCreator
    {
        public global::World.Models.Characters.Agent CreateAgent(Player player, List<KeyValuePair<string, string>> agentConfiguration);
    }
}