using Creature.Creature.StateMachine.Data;
using System;
using System.Collections.Generic;
using ASD_project.Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.Builder;

namespace Creature.Creature.StateMachine.State
{
    public abstract class CreatureState : IComparable
    {
        protected ICreatureData _creatureData;
        protected ICreatureData _target;
        protected ICreatureStateMachine _stateMachine;
        protected BuilderConfiguration _builderConfiguration;
        protected List<BuilderInfo> _builderInfoList;


        public CreatureState(ICreatureData creatureData, ICreatureStateMachine stateMachine, List<BuilderInfo> builderInfoList, BuilderConfiguration builderConfiguration)
        {
            _creatureData = creatureData;
            _stateMachine = stateMachine;
            _builderConfiguration = builderConfiguration;
            _builderInfoList = builderInfoList;
        }
        public virtual void Do()
        {
            throw new NotImplementedException();
        }

        public virtual void Do(ICreatureData creatureData)
        {
            throw new NotImplementedException();
        }
        
        public virtual void SetTargetData(ICreatureData data)
        {
            _target = data;
        }
        
        public int CompareTo(object obj)
        {
            throw new InvalidOperationException("State machine is not a comparable object.");
        }
    }
}