using System.Collections.Generic;

namespace Creature.Services
{
    public class AgentService : IAgentService
    {
        private Dictionary<string, ICreature> agents;

        public AgentService(string playerId)
        {
            
        }
        
        public void Activate()
        {
            throw new System.NotImplementedException();
        }

        public void DeActivate()
        {
            throw new System.NotImplementedException();
        }

        public void IsActivated()
        {
            throw new System.NotImplementedException();
        }
    }
}