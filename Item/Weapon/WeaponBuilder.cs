using Item.Weapon.WeaponStats;
using Player.Model;
using Player.Model.ItemStats;

namespace Weapon
{
    public class WeaponBuilder : IBuilder
    {
        private Item.Weapon.Weapon _weapon;

        private WeaponBuilder()
        {
            _weapon = new Item.Weapon.Weapon();
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
            _weapon = new Item.Weapon.Weapon();
        }
        
        public Item.Weapon.Weapon GetItem()
        {
            return _weapon;
        }
    }
}