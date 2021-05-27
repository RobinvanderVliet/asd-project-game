using System;
using Item.Consumable.ConsumableStats;
using Player.Model;
using Player.Model.Consumable.ConsumableStats;
using Player.Model.Consumable.HazardProtectedConsumable;
using Player.Model.Consumable.HealthConsumable;
using Player.Model.Consumable.StaminaConsumable;
using Player.Model.ItemStats;

namespace Item.Consumable
{
    public class ConsumableBuilder : IBuilder
    {
        private Consumable _consumable;

        private ConsumableBuilder(ConsumableType type)
        {
            _consumable = InitializeConsumable(type);
        }

        private Consumable InitializeConsumable(ConsumableType type)
        {
            return type switch
            {
                ConsumableType.HazardProtected => new HazardProtectedConsumable(),
                ConsumableType.Health => new HealthConsumable(),
                ConsumableType.Stamina => new StaminaConsumable(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void Reset()
        {
            if (_consumable is HazardProtectedConsumable)
            {
                _consumable = InitializeConsumable(ConsumableType.HazardProtected);
            }

            if (_consumable is HealthConsumable)
            {
                _consumable = InitializeConsumable(ConsumableType.Health);
            }

            if (_consumable is StaminaConsumable)
            {
                _consumable = InitializeConsumable(ConsumableType.Stamina);
            }
        }

        public void SetName(string name)
        {
            _consumable.ItemName = name;
        }

        public void SetDescription(string description)
        {
            _consumable.Description = description;
        }

        public void SetRarity(Rarity rarity)
        {
            _consumable.Rarity = rarity;
        }

        public void SetStamina(Stamina stamina)
        {
            if (_consumable is StaminaConsumable consumable)
            {
                consumable.Stamina = stamina;
            }
        }

        public void SetHealth(Health health)
        {
            if (_consumable is HealthConsumable consumable)
            {
                consumable.Health = health;
            }
        }

        public void SetRpp(int Rpp)
        {
            if (_consumable is HazardProtectedConsumable consumable)
            {
                consumable.RPP = Rpp;
            }
        }

        public Consumable GetItem()
        {
            return _consumable;
        }
    }
}