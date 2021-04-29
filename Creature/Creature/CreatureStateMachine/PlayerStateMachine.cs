using Appccelerate.StateMachine;
using Creature.Creature.StateMachine.CreatureData;
using System;

namespace Creature.Creature.CreatureStateMachine
{
    class PlayerStateMachine : ICreatureStateMachine
    {
        private RuleSet _ruleset;
        private PassiveStateMachine<CreatureState, int> passiveStateMachine;

        public ICreatureData CreatureData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void FireEvent(Enum creatureEvent, object argument)
        {
            throw new NotImplementedException();
        }

        public void FireEvent(Enum creatureEvent)
        {
            throw new NotImplementedException();
        }

        public void StartStateMachine(RuleSet ruleSet)
        {
            throw new NotImplementedException();
        }
    }
}
