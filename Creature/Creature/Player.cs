﻿using Creature.Creature;
using Creature.Pathfinder;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Creature
{
    public class Player : ICreature
    {
        private double _health;
        private readonly double _maxHealth;
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
        public int VisionRange { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Player(double initialHealth)
        {
            _health = initialHealth;
            _maxHealth = initialHealth;
            _alive = true;
        }

        public Player(double initialHealth, Vector2 position)
        {
            _health = initialHealth;
            _maxHealth = initialHealth;
            _alive = true;
            _position = position;
        }

        public void ApplyDamage(double amount)
        {
            _health -= amount;
            Console.WriteLine("You suffered: " + amount + "damage. Remaining health: " + _health + ".");
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
            throw new NotImplementedException();
        }

        public void StartStateMachine(RuleSet ruleSet)
        {
            throw new NotImplementedException();
        }

        public void FireEvent(Enum creatureEvent, object argument)
        {
            throw new NotImplementedException();
        }

        public void FireEvent(Enum creatureEvent)
        {
            throw new NotImplementedException();
        }

        public void Do(Stack<Node> path)
        {
            throw new NotImplementedException();
        }
    }
}