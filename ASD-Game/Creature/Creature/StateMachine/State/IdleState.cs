using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;

namespace Creature.Creature.StateMachine.State
{
    public class IdleState : CreatureState
    {
        public IdleState(ICreatureData creatureData, ICreatureStateMachine stateMachine) : base(creatureData, stateMachine)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
        }

        public void Do()
        {
            string mapResult = _ruleSetCoreFunctions.AnalyzeMap();
            if (mapResult == null)
            {
                //Do Nothing
            } else if (mapResult != null )
            {
                switch (mapResult)
                { //TODO make sure this acquires a target
                    case "player":
                    case "monster":
                        _stateMachine.FireEvent(CreatureEvent.Event.SPOTTED_CREATURE);
                        break;
                    case "item":
                        _stateMachine.FireEvent(CreatureEvent.Event.FOUND_ITEM);
                        break;
                }
            } 
        }
    }
}