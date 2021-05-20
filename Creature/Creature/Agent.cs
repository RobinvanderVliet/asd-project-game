using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;

namespace Creature
{
    public class Agent : ICreature
    {
        private AgentStateMachine _agentStateMachine;

        public ICreatureStateMachine CreatureStateMachine
        {
            get => _agentStateMachine;
        }

        public Agent(AgentData agentData, RuleSet ruleSet)
        {
            _agentStateMachine = new(agentData, ruleSet);
            _agentStateMachine.StartStateMachine();
        }

        public void ApplyDamage(double amount)
        {
            _agentStateMachine.CreatureData.Health -= amount;
        }

        public void HealAmount(double amount)
        {
            _agentStateMachine.CreatureData.Health += amount;
        }
    }
}