using System;
using Creature.Creature;
using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;

namespace Creature
{
    public class Agent : ICreature
    {
        private AgentStateMachine _agentStateMachine;

        public ICreatureStateMachine CreatureStateMachine
        {
            get => _agentStateMachine;
        }

        public Agent(AgentData agentData)
        {
            _agentStateMachine = new(agentData);
            _agentStateMachine.StartStateMachine();
        }
        
        public void ApplyDamage(double amount)
        {
            _agentStateMachine.CreatureData.Health -= amount;
            Console.WriteLine( amount + " damage is applied to Agent remaining health = " + _agentStateMachine.CreatureData.Health );
            if (_agentStateMachine.CreatureData.Health <= 30)
            {
                _agentStateMachine.FireEvent(CreatureEvent.Event.ALMOST_DEAD, _agentStateMachine.CreatureData);
            }
        }

        public void HealAmount(double amount)
        {
            _agentStateMachine.CreatureData.Health += amount;
        }
    }
}