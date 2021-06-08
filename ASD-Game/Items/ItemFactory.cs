using Items.Consumables;
using Items.WeaponStats;

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