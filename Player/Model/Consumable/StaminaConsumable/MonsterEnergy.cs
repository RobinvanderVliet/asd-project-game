﻿using Player.Model.Consumable.ConsumableStats;
using Player.Model.ItemStats;

namespace Player.Model.Consumable.StaminaConsumable
{
    public class MonsterEnergy : StaminaConsumable
    {
        public MonsterEnergy()
        {
            Name = "Monster energy";
            Stamina = Stamina.Medium;
            Rarity = Rarity.Uncommon;
        }
    }
}