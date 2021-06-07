using System;
using ASD_Game.Items.ArmorStats;
using ASD_Game.Items.ItemStats;

namespace ASD_Game.Items
{
    public class ArmorBuilder : IBuilder
    {
        private Armor _armor;

        public ArmorBuilder(ArmorType armorType)
        {
            _armor = InitializeArmor(armorType);
        }

        private Armor InitializeArmor(ArmorType type)
        {
            return type switch
            {
                ArmorType.HazardProtectedArmor => new HazardProtectedArmor(),
                ArmorType.DefaultArmor => new Armor(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void Reset()
        {
            if (_armor is HazardProtectedArmor)
            {
                _armor = InitializeArmor(ArmorType.HazardProtectedArmor);
            }
            else
            {
                _armor = InitializeArmor(ArmorType.DefaultArmor);
            }
        }

        public void SetName(string name)
        {
            _armor.ItemName = name;
        }

        public void SetDescription(string description)
        {
            _armor.Description = description;
        }

        public void SetRarity(Rarity rarity)
        {
            _armor.Rarity = rarity;
        }

        public void SetArmorPartType(ArmorPartType type)
        {
            _armor.ArmorPartType = type;
        }

        public void SetArmorProtectionPoints(int armorProtectionPoints)
        {
            _armor.ArmorProtectionPoints = armorProtectionPoints;
        }

        public void SetRadiationProtectionPoints(int radiationProtectionPoints)
        {
            if (_armor is HazardProtectedArmor armor)
            {
                armor.RadiationProtectionPoints = radiationProtectionPoints;
            }
        }
        
        public void SetStaminaPoints(int staminaPoints)
        {
            if (_armor is HazardProtectedArmor armor)
            {
                armor.StaminaPoints = staminaPoints;
            }
        }
        
        public Armor GetItem()
        {
            return _armor;
        }
    }
}