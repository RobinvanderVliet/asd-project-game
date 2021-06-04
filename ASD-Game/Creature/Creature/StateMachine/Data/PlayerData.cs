using System;
using Creature.World;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Timers;
using ActionHandling;

namespace Creature.Creature.StateMachine.Data
{
    [ExcludeFromCodeCoverage]
    public class PlayerData : ICreatureData
    {
        private Vector2 _position;
        private double _health;
        private int _damage;
        private int _visionRange;
        private IWorld _world;
        private IMoveHandler _moveHandler;

        public bool IsAlive { get => _health > 0; }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public double Health
        {
            get => _health;
            set => _health = value;
        }

        public int Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public int VisionRange
        {
            get => _visionRange;
            set => _visionRange = value;
        }

        public IWorld World
        {
            get => _world;
            set => _world = value;
        }
        
        public IMoveHandler MoveHandler
        {
            get => _moveHandler;
            set => _moveHandler = value;
        }

        public PlayerData(Vector2 position, double health, int damage, int visionRange, IWorld world, IMoveHandler moveHandler)
        {
            _position = position;
            _health = health;
            _damage = damage;
            _visionRange = visionRange;
            _world = world;
            _moveHandler = moveHandler;
            
            var aTimer = new Timer();
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Interval = 1000;
            aTimer.Enabled = true;
            
            while(Console.Read() != 'q');
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private  void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _moveHandler.SendMove("up", 1);
        }

    }
}
