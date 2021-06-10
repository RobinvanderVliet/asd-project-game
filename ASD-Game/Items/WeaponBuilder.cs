using ASD_Game.Items.ItemStats;
using ASD_Game.Items.WeaponStats;

namespace ASD_Game.Items
{
    public class WeaponBuilder : IBuilder
    {
        private Weapon _weapon;

        public WeaponBuilder()
        {
            Reset();
        }

        public void SetName(string name)
        {
            _weapon.ItemName = name;
        }

        public void SetDescription(string description)
        {
            _weapon.Description = description;
        }

        public void SetRarity(Rarity rarity)
        {
            _weapon.Rarity = rarity;
        }

        public void SetDamage(WeaponDamage damage)
        {
            _weapon.Damage = damage;
        }

        public void SetWeaponDistance(WeaponDistance weaponDistance)
        {
            _weapon.Distance = weaponDistance;
        }

        public void SetWeaponSpeed(WeaponSpeed speed)
        {
            _weapon.Speed = speed;
        }
        
        public void SetWeaponType(WeaponType type)
        {
            _weapon.Type = type;
        }
        
        public void Reset()
        {
            _weapon = new Weapon();
        }
        
        public Weapon GetItem()
        {
            return _weapon;
        }
    }
}