using System.Numerics;

namespace Creature
{
    public interface IPlayer
    {
        bool IsAlive { get; set; }
        Vector2 Position { get; set; }
        public void ApplyDamage(double amount);

        public void HealAmount(double amount);
    }
}