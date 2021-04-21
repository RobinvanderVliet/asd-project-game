using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Consumable;
using Creature.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Creature
{
    public class Monster : ICreature
    {
        private bool _following;
        private readonly double _maxHealth;
        private double _health = 0;
        private double _damage;
        private readonly IWorld _world;

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
        private IPlayer _player;

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
        /// All states that this creature is capable of activating.
        /// Every creature has its own states that it can be in.
        /// </summary>
        public enum State
        {
            WANDERING,
            FOLLOW_PLAYER,
            ATTACK_PLAYER,
            USE_CONSUMABLE
        };

        /// <summary>
        /// Passive NOT ASYNC statemachine.
        /// A statemachine will decide how a creature responds to specific events.
        /// Statemachines will decide how a creature behaves in certain events.
        /// </summary>
        private PassiveStateMachine<State, Event> stateMachine;

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
                stateMachine.Fire((Event)creatureEvent, argument);
            }
        }

        public void FireEvent(Enum creatureEvent)
        {
            if (creatureEvent.GetType() == typeof(Event))
            {
                stateMachine.Fire((Event)creatureEvent);
            }
        }

        private void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<State, Event>();

            // Wandering
            builder.In(State.FOLLOW_PLAYER).On(Event.LOST_PLAYER).Goto(State.WANDERING);

            // Follow player
            builder.In(State.WANDERING).On(Event.SPOTTED_PLAYER).Goto(State.FOLLOW_PLAYER).Execute<IPlayer>(OnFollowPlayer);
            builder.In(State.USE_CONSUMABLE).On(Event.REGAINED_HEALTH_PLAYER_OUT_OF_RANGE).Goto(State.FOLLOW_PLAYER).Execute<IPlayer>(OnFollowPlayer);
            builder.In(State.ATTACK_PLAYER).On(Event.PLAYER_OUT_OF_RANGE).Goto(State.FOLLOW_PLAYER).Execute<IPlayer>(OnFollowPlayer);

            // Attack player
            builder.In(State.FOLLOW_PLAYER).On(Event.PLAYER_IN_RANGE).Goto(State.ATTACK_PLAYER).Execute<IPlayer>(OnAttackPlayer);
            builder.In(State.USE_CONSUMABLE).On(Event.REGAINED_HEALTH_PLAYER_IN_RANGE).Goto(State.ATTACK_PLAYER).Execute<IPlayer>(OnAttackPlayer);

            // Use potion
            builder.In(State.ATTACK_PLAYER).On(Event.ALMOST_DEAD).Goto(State.USE_CONSUMABLE).Execute<IConsumable>(OnUseConsumable);
            builder.In(State.FOLLOW_PLAYER).On(Event.ALMOST_DEAD).Goto(State.USE_CONSUMABLE).Execute<IConsumable>(OnUseConsumable);

            builder.WithInitialState(State.WANDERING);

            stateMachine = builder.Build().CreatePassiveStateMachine();
            stateMachine.Start();
        }

        public void Do()
        {
            if (_following)
            {
                if (_position.X < _player.Position.X) _position.X += 1;
                else if (_position.X > _player.Position.X) _position.X -= 1;
                else if (_position.Y < _player.Position.Y) _position.Y += 1;
                else if (_position.Y > _player.Position.Y) _position.Y -= 1;
            }
        }

        private void OnFollowPlayer(IPlayer player)
        {
            _following = true;
            _player = player;
        }

        private void OnUseConsumable(IConsumable consumable)
        {
            _health += consumable.Amount;
        }

        private void OnAttackPlayer(IPlayer player)
        {
            player.ApplyDamage(_damage);
        }

        public void ApplyDamage(double amount)
        {
            _health -= amount;
            Console.WriteLine("Damaged enemie for: " + amount + ". Remaining enemie health: "+ _health + ".");
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
    }
}
