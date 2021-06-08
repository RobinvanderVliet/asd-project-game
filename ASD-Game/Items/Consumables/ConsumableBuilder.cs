using System;
using ASD_Game.Items.Consumables.ConsumableStats;
using ASD_Game.Items.ItemStats;

namespace ASD_Game.Items.Consumables
{
    public class ConsumableBuilder : IBuilder
    {
        private Consumable _consumable;

        public ConsumableBuilder(ConsumableType type)
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

        public void SetRpp(int rpp)
        {
            if (_consumable is HazardProtectedConsumable consumable)
            {
                consumable.RPP = rpp;
            }
        }

        public Consumable GetItem()
        {
            return _consumable;
        }
    }
}