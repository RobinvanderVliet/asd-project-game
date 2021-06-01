using Items.Consumables;
using Items.WeaponStats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Items
{
    public static class ItemFactory
    {
        public static Weapon GetKnife()
        {
            var weaponbuilder = new WeaponBuilder();
            weaponbuilder.SetName("Knife");
            weaponbuilder.SetDescription("That ain't a knoife, this is a knoife");
            weaponbuilder.SetWeaponType(WeaponType.Melee);
            weaponbuilder.SetRarity(Items.ItemStats.Rarity.Common);
            weaponbuilder.SetWeaponSpeed(WeaponSpeed.Slow);
            weaponbuilder.SetWeaponDistance(WeaponDistance.Close);
            weaponbuilder.SetDamage(WeaponDamage.Low);
            return weaponbuilder.GetItem();
        }

        public static Armor GetBandana()
        {
            ArmorBuilder armorBuilder = new(ArmorStats.ArmorType.DefaultArmor);
            armorBuilder.SetName("Bandana");
            armorBuilder.SetDescription("Default headwear, plain but good looking");
            armorBuilder.SetArmorPartType(ArmorStats.ArmorPartType.Helmet);
            armorBuilder.SetRarity(ItemStats.Rarity.Common);
            armorBuilder.SetArmorProtectionPoints(1);
            return armorBuilder.GetItem();
        }

        public static Consumable GetBandage()
        {
            ConsumableBuilder builder = new(Consumables.ConsumableStats.ConsumableType.Health);
            builder.SetName("Bandage");
            builder.SetDescription("Let me patch you together");
            builder.SetHealth(Consumables.ConsumableStats.Health.Low);
            builder.SetRarity(ItemStats.Rarity.Common);
            return builder.GetItem();
        }
    }
}
