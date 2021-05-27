using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;

namespace Creature.Creature.StateMachine
{
    class MonsterStateMachine : ICreatureStateMachine
    {
        private PassiveStateMachine<CreatureState, CreatureEvent.Event> _passiveStateMachine;
        private MonsterData _monsterData;

        public MonsterStateMachine(MonsterData monsterData)
        {
            _monsterData = monsterData;
        }

        public ICreatureData CreatureData
        {
            get => _monsterData;
            set => _monsterData = (MonsterData)value;
        }

        public void FireEvent(CreatureEvent.Event creatureEvent, object argument)
        {
             _passiveStateMachine.Fire(creatureEvent, argument);
        }

        public void FireEvent(CreatureEvent.Event creatureEvent)
        {
            _passiveStateMachine.Fire(creatureEvent);
        }

        public void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CreatureState, CreatureEvent.Event>();

            CreatureState followPlayerState = new FollowCreatureState(CreatureData);
            CreatureState wanderState = new WanderState(CreatureData);
            CreatureState useConsumableState = new UseConsumableState(CreatureData);
            CreatureState attackPlayerState = new AttackState(CreatureData);
            CreatureState fleeFromCreatureState = new FleeFromCreatureState(CreatureData);

            // Wandering
            builder.In(followPlayerState).On(CreatureEvent.Event.LOST_PLAYER).Goto(wanderState);

            // TODO: implement this using Setting
            // foreach (var setting in CreatureData.RuleSet)
            // {
            //     if (setting.Property == "combat_default_monster_threshold" && setting.Value == "player")
            //     {
            //         if (setting.ContainsKey("combat_default_monster_comparison") && setting["combat_default_monster_comparison"] == "nearby")
            //         {
            //             if (setting.ContainsKey("combat_default_monster_comparison_true") && setting["combat_default_monster_comparison_true"] == "attack")
            //             {
            //                 builder.In(followPlayerState).On(CreatureEvent.Event.PLAYER_IN_RANGE).If<ICreatureData>((c) => typeof(PlayerData) == c.GetType()).Goto(attackPlayerState).Execute<ICreatureData>(new AttackState(CreatureData).Do);
            //                 builder.In(attackPlayerState).On(CreatureEvent.Event.PLAYER_IN_RANGE).If<ICreatureData>((c) => typeof(PlayerData) == c.GetType()).Execute<ICreatureData>(new AttackState(CreatureData).Do);
            //             }
            //         }
            //         else if (setting.ContainsKey("combat_default_monster_comparison") && setting["combat_default_monster_comparison"] == "sees")
            //         {
            //             if (setting.ContainsKey("combat_default_monster_comparison_true") && setting["combat_default_monster_comparison_true"] == "follow")
            //             {
            //                 builder.In(wanderState).On(CreatureEvent.Event.SPOTTED_PLAYER).Goto(followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            //                 builder.In(followPlayerState).On(CreatureEvent.Event.SPOTTED_PLAYER).Goto(followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            //                 
            //                 builder.In(useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_PLAYER_OUT_OF_RANGE).Goto(followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            //                 builder.In(attackPlayerState).On(CreatureEvent.Event.PLAYER_OUT_OF_RANGE).Goto(followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            //             }
            //             else if (setting.ContainsKey("combat_default_monster_comparison_true") && setting["combat_default_monster_comparison_true"] == "flee")
            //             {
            //                 builder.In(wanderState).On(CreatureEvent.Event.SPOTTED_PLAYER).Goto(fleeFromCreatureState).Execute<ICreatureData>(new FleeFromCreatureState(CreatureData).Do);
            //                 builder.In(fleeFromCreatureState).On(CreatureEvent.Event.SPOTTED_PLAYER).Goto(fleeFromCreatureState).Execute<ICreatureData>(new FleeFromCreatureState(CreatureData).Do);
            //             }
            //         }
            //     }
            // }

            builder.In(useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICreatureData>(new AttackState(CreatureData).Do);

            // Use potion
            builder.In(attackPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);
            builder.In(followPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
    }
}
