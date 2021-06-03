using Items.Consumables;
using Items.Consumables.ConsumableStats;
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

        public static Weapon GetBaseballBat()
        {
            var weaponbuilder = new WeaponBuilder();
            weaponbuilder.SetName("Baseball Bat");
            weaponbuilder.SetDescription("I will hit a home run.");
            weaponbuilder.SetWeaponType(WeaponType.Melee);
            weaponbuilder.SetRarity(Items.ItemStats.Rarity.Uncommon);
            weaponbuilder.SetWeaponSpeed(WeaponSpeed.Slow);
            weaponbuilder.SetWeaponDistance(WeaponDistance.Close);
            weaponbuilder.SetDamage(WeaponDamage.High);
            return weaponbuilder.GetItem();
        }

        public static Weapon GetKatana()
        {
            var weaponbuilder = new WeaponBuilder();
            weaponbuilder.SetName("Katana");
            weaponbuilder.SetDescription("Cutting edge technology.");
            weaponbuilder.SetWeaponType(WeaponType.Melee);
            weaponbuilder.SetRarity(Items.ItemStats.Rarity.Rare);
            weaponbuilder.SetWeaponSpeed(WeaponSpeed.Fast);
            weaponbuilder.SetWeaponDistance(WeaponDistance.Close);
            weaponbuilder.SetDamage(WeaponDamage.Medium);
            return weaponbuilder.GetItem();
        }

        public static Weapon GetGlock()
        {
            var weaponbuilder = new WeaponBuilder();
            weaponbuilder.SetName("Glock");
            weaponbuilder.SetDescription("Ate a glock.");
            weaponbuilder.SetWeaponType(WeaponType.Range);
            weaponbuilder.SetRarity(Items.ItemStats.Rarity.Uncommon);
            weaponbuilder.SetWeaponSpeed(WeaponSpeed.Slow);
            weaponbuilder.SetWeaponDistance(WeaponDistance.Medium);
            weaponbuilder.SetDamage(WeaponDamage.Low);
            return weaponbuilder.GetItem();
        }

        public static Weapon GetP90()
        {
            var weaponbuilder = new WeaponBuilder();
            weaponbuilder.SetName("P90");
            weaponbuilder.SetDescription("CS:GO players hate him.");
            weaponbuilder.SetWeaponType(WeaponType.Melee);
            weaponbuilder.SetRarity(Items.ItemStats.Rarity.Common);
            weaponbuilder.SetWeaponSpeed(WeaponSpeed.Slow);
            weaponbuilder.SetWeaponDistance(WeaponDistance.Close);
            weaponbuilder.SetDamage(WeaponDamage.Low);
            return weaponbuilder.GetItem();
        }

        public static Weapon GetAK47()
        {
            var weaponbuilder = new WeaponBuilder();
            weaponbuilder.SetName("AK-47");
            weaponbuilder.SetDescription("She was a veiled threat.");
            weaponbuilder.SetWeaponType(WeaponType.Range);
            weaponbuilder.SetRarity(Items.ItemStats.Rarity.Rare);
            weaponbuilder.SetWeaponSpeed(WeaponSpeed.Average);
            weaponbuilder.SetWeaponDistance(WeaponDistance.Far);
            weaponbuilder.SetDamage(WeaponDamage.Low);
            return weaponbuilder.GetItem();
        }

        public static Armor GetFlakVest()
        {
            ArmorBuilder armorBuilder = new(ArmorStats.ArmorType.DefaultArmor);
            armorBuilder.SetName("Flak vest");
            armorBuilder.SetDescription("BOOM, wait i'm fiiiiiiine");
            armorBuilder.SetArmorPartType(ArmorStats.ArmorPartType.Body);
            armorBuilder.SetRarity(ItemStats.Rarity.Uncommon);
            armorBuilder.SetArmorProtectionPoints(20);
            return armorBuilder.GetItem();
        }

        public static Armor GetHardHat()
        {
            ArmorBuilder armorBuilder = new(ArmorStats.ArmorType.DefaultArmor);
            armorBuilder.SetName("Hard Hat");
            armorBuilder.SetDescription("Bob the builder, can we fix it.");
            armorBuilder.SetArmorPartType(ArmorStats.ArmorPartType.Helmet);
            armorBuilder.SetRarity(ItemStats.Rarity.Uncommon);
            armorBuilder.SetArmorProtectionPoints(10);
            return armorBuilder.GetItem();
        }

        public static Armor GetJacket()
        {
            ArmorBuilder armorBuilder = new(ArmorStats.ArmorType.DefaultArmor);
            armorBuilder.SetName("Jacket");
            armorBuilder.SetDescription("My new jacket is reversible, as it turns out.");
            armorBuilder.SetArmorPartType(ArmorStats.ArmorPartType.Body);
            armorBuilder.SetRarity(ItemStats.Rarity.Common);
            armorBuilder.SetArmorProtectionPoints(10);
            return armorBuilder.GetItem();
        }

        public static Armor GetMilitaryHelmet()
        {
            ArmorBuilder armorBuilder = new(ArmorStats.ArmorType.DefaultArmor);
            armorBuilder.SetName("Military Helmet");
            armorBuilder.SetDescription("A shell-met!");
            armorBuilder.SetArmorPartType(ArmorStats.ArmorPartType.Helmet);
            armorBuilder.SetRarity(ItemStats.Rarity.Rare);
            armorBuilder.SetArmorProtectionPoints(20);
            return armorBuilder.GetItem();
        }

        public static Armor GetTacticalVest()
        {
            ArmorBuilder armorBuilder = new(ArmorStats.ArmorType.DefaultArmor);
            armorBuilder.SetName("Tactical Vest");
            armorBuilder.SetDescription("Bullets got nothing on this!");
            armorBuilder.SetArmorPartType(ArmorStats.ArmorPartType.Body);
            armorBuilder.SetRarity(ItemStats.Rarity.Rare);
            armorBuilder.SetArmorProtectionPoints(40);
            return armorBuilder.GetItem();
        }

        public static Armor GetHazmatSuit()
        {
            ArmorBuilder armorBuilder = new(ArmorStats.ArmorType.HazardProtectedArmor);
            armorBuilder.SetName("Hazmat Suit");
            armorBuilder.SetDescription("I look like an employee at a nuclear power plant!");
            armorBuilder.SetArmorPartType(ArmorStats.ArmorPartType.Body);
            armorBuilder.SetRarity(ItemStats.Rarity.Rare);
            armorBuilder.SetArmorProtectionPoints(20);
            armorBuilder.SetRadiationProtectionPoints(80);
            armorBuilder.SetStaminaPoints(-20);
            return armorBuilder.GetItem();
        }

        public static Armor GetGasMask()
        {
            ArmorBuilder armorBuilder = new(ArmorStats.ArmorType.HazardProtectedArmor);
            armorBuilder.SetName("Gas Mask");
            armorBuilder.SetDescription("I can breath now.");
            armorBuilder.SetArmorPartType(ArmorStats.ArmorPartType.Helmet);
            armorBuilder.SetRarity(ItemStats.Rarity.Uncommon);
            armorBuilder.SetArmorProtectionPoints(20);
            armorBuilder.SetRadiationProtectionPoints(40);
            armorBuilder.SetStaminaPoints(-20);
            return armorBuilder.GetItem();
        }

        public static Consumable GetIodineTablets()
        {
            ConsumableBuilder consumableBuilder = new(ConsumableType.HazardProtected);
            consumableBuilder.SetName("Iodine tablets");
            consumableBuilder.SetDescription("What do you call a child with Iodine deficiency? Chld.");
            consumableBuilder.SetRarity(ItemStats.Rarity.Common);
            consumableBuilder.SetRpp(20);
            return consumableBuilder.GetItem();
        }

        public static Consumable GetBandage()
        {
            ConsumableBuilder consumableBuilder = new(ConsumableType.Health);
            consumableBuilder.SetName("Bandage");
            consumableBuilder.SetDescription("Let me patch you together.");
            consumableBuilder.SetRarity(ItemStats.Rarity.Common);
            consumableBuilder.SetHealth(Health.Low);
            return consumableBuilder.GetItem();
        }

        public static Consumable GetMedkit()
        {
            ConsumableBuilder consumableBuilder = new(ConsumableType.Health);
            consumableBuilder.SetName("Medkit");
            consumableBuilder.SetDescription("Good as new.");
            consumableBuilder.SetRarity(ItemStats.Rarity.Rare);
            consumableBuilder.SetHealth(Health.High);
            return consumableBuilder.GetItem();
        }

        public static Consumable GetMorphine()
        {
            ConsumableBuilder consumableBuilder = new(ConsumableType.Health);
            consumableBuilder.SetName("Morphine");
            consumableBuilder.SetDescription("Comfortably numb!");
            consumableBuilder.SetRarity(ItemStats.Rarity.Uncommon);
            consumableBuilder.SetHealth(Health.Medium);
            return consumableBuilder.GetItem();
        }

        public static Consumable GetBigMac()
        {
            ConsumableBuilder consumableBuilder = new(ConsumableType.Stamina);
            consumableBuilder.SetName("Big Mac");
            consumableBuilder.SetDescription("What type of computer does Ronald McDonald use?");
            consumableBuilder.SetRarity(ItemStats.Rarity.Common);
            consumableBuilder.SetStamina(Stamina.Low);
            return consumableBuilder.GetItem();
        }

        public static Consumable GetMonsterEnergy()
        {
            ConsumableBuilder consumableBuilder = new(ConsumableType.Stamina);
            consumableBuilder.SetName("Monster energy");
            consumableBuilder.SetDescription("WARNING: contains real monsters!");
            consumableBuilder.SetRarity(ItemStats.Rarity.Uncommon);
            consumableBuilder.SetStamina(Stamina.Medium);
            return consumableBuilder.GetItem();
        }

        public static Consumable GetSuspiciousWhitePowder()
        {
            ConsumableBuilder consumableBuilder = new(ConsumableType.Stamina);
            consumableBuilder.SetName("Suspicious white powder");
            consumableBuilder.SetDescription("Pink fluffy unicorns dancing on rainbows..");
            consumableBuilder.SetRarity(ItemStats.Rarity.Rare);
            consumableBuilder.SetStamina(Stamina.High);
            return consumableBuilder.GetItem();
        }
    }
}
