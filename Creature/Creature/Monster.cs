using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Consumable;
using Creature.Creature;
using Creature.Pathfinder;
using Creature.World;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Creature
{
    public class Monster : ICreature
    {
        private bool _following;
        private readonly double _maxHealth;
        private double _health = 0;
        private readonly double _damage;
        private readonly IWorld _world;
        public bool IsFollowing
        {
            get => _following;
            set => _following = value;
        }

        private bool _alive;
        public bool IsAlive
        {
            get => _alive;
            set => _alive = value;
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        private int _visionRange;
        public int VisionRange {
            get => _visionRange; 
            set => _visionRange = value; 
        }

        /// <summary>
        /// Player that is being interacted with.
        /// Monsters can follow, attack, spot a player, etc.
        /// </summary>
        private ICreature _player;

        /// <summary>
        /// All events that this creature is capable of responding to.
        /// Every creature can respond to its own events.
        /// </summary>
        public enum Event
        {
            LOST_PLAYER,
            SPOTTED_PLAYER,
            ALMOST_DEAD,
            REGAINED_HEALTH_PLAYER_OUT_OF_RANGE,
            REGAINED_HEALTH_PLAYER_IN_RANGE,
            PLAYER_OUT_OF_RANGE,
            PLAYER_IN_RANGE
        };

        /// <summary>
        /// Passive NOT ASYNC statemachine.
        /// A statemachine will decide how a creature responds to specific events.
        /// Statemachines will decide how a creature behaves in certain events.
        /// </summary>
        private PassiveStateMachine<CreatureState, Event> _stateMachine;

        public Monster(IWorld world, Vector2 position, double damage, double initialHealth, int visionRange)
        {
            _position = position;
            _damage = damage;
            _visionRange = visionRange;
            _maxHealth = initialHealth;
            _alive = true;
            _world = world;
            _following = false;

            _health = _maxHealth;

            StartStateMachine();
        }

        public void FireEvent(Enum creatureEvent, object argument)
        {
            if (creatureEvent.GetType() == typeof(Event))
            {
                _stateMachine.Fire((Event)creatureEvent, argument);
            }
        }

        public void FireEvent(Enum creatureEvent)
        {
            if (creatureEvent.GetType() == typeof(Event))
            {
                _stateMachine.Fire((Event)creatureEvent);
            }
        }

        public void Do(Stack<Node> path)
        {
            if (_following)
            {
                if (!(path.Peek().position.X == _player.Position.X && path.Peek().position.Y == _player.Position.Y))
                {
                    _position.X = path.Peek().position.X;
                    _position.Y = path.Peek().position.Y;
                }
            }

            if ((_position.X + 1f) == _player.Position.X && (_position.Y == _player.Position.Y)
                || (_position.X - 1f) == _player.Position.X && (_position.Y == _player.Position.Y)
                || (_position.Y + 1f) == _player.Position.Y && (_position.X == _player.Position.X)
                || (_position.Y - 1f) == _player.Position.Y && (_position.X == _player.Position.X))
            {
                FireEvent(Monster.Event.PLAYER_IN_RANGE, _player);
            }
        }

        private void OnFollowPlayer(ICreature player)
        {
            _following = true;
            _player = player;
        }

        private void OnUseConsumable(IConsumable consumable)
        {
            consumable.Use();
            _health += consumable.Amount;
        }

        private void OnAttackPlayer(ICreature player)
        {
            player.ApplyDamage(_damage);
        }

        public void ApplyDamage(double amount)
        {
            _health -= amount;
            if (_health < 0)
                _alive = false;
        }

        public void HealAmount(double amount)
        {
            if (_health < _maxHealth)
                _health += amount;

            if (_health >= _maxHealth)
                _health = _maxHealth;
        }

        public void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CreatureState, Event>();

            CreatureState followPlayer = new FollowPlayerState();
            CreatureState wanderState = new WanderState();
            CreatureState useConsumable = new UseConsumableState();
            CreatureState attackPlayerState = new AttackPlayerState();

            // Wandering
            builder.In(followPlayer).On(Event.LOST_PLAYER).Goto(wanderState);

            // Follow player
            builder.In(wanderState).On(Event.SPOTTED_PLAYER).Goto(followPlayer).Execute<ICreature>(OnFollowPlayer);
            builder.In(useConsumable).On(Event.REGAINED_HEALTH_PLAYER_OUT_OF_RANGE).Goto(followPlayer).Execute<ICreature>(OnFollowPlayer);
            builder.In(attackPlayerState).On(Event.PLAYER_OUT_OF_RANGE).Goto(followPlayer).Execute<ICreature>(OnFollowPlayer);

            // Attack player
            builder.In(followPlayer).On(Event.PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICreature>(OnAttackPlayer);
            builder.In(attackPlayerState).On(Event.PLAYER_IN_RANGE).Execute<ICreature>(OnAttackPlayer);
            builder.In(useConsumable).On(Event.REGAINED_HEALTH_PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICreature>(OnAttackPlayer);

            // Use potion
            builder.In(attackPlayerState).On(Event.ALMOST_DEAD).Goto(useConsumable).Execute<IConsumable>(OnUseConsumable);
            builder.In(followPlayer).On(Event.ALMOST_DEAD).Goto(useConsumable).Execute<IConsumable>(OnUseConsumable);

            builder.WithInitialState(wanderState);

            _stateMachine = builder.Build().CreatePassiveStateMachine();
            _stateMachine.Start();
        }

        public void StartStateMachine(RuleSet ruleSet)
        {
            throw new NotImplementedException();
        }
    }
}
