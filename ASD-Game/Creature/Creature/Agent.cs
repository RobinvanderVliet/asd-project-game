using System;
using System.Numerics;
using Creature.Creature;
using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using WorldGeneration;

namespace Creature
{
    public class Agent : Player, ICreature
    {
        private AgentStateMachine _agentStateMachine;
       
        public ICreatureStateMachine CreatureStateMachine
        {
            get => _agentStateMachine;
        }

        public Agent(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol, id)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
            XPosition = xPosition;
            YPosition = yPosition;
            _agentStateMachine = new AgentStateMachine(CreateAgentData());
            _agentStateMachine.StartStateMachine();
        }

        private AgentData CreateAgentData()
        {
            Vector2 position = new Vector2(XPosition, YPosition);
            return new AgentData(position, Health, Stamina, 6, null, false);
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