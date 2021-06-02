using System;

namespace Creature.Consumable
{
   public interface IConsumable
    {
        public String Type { get; set; }// Uses text like: "Healing for 20 points." or "Damage oppponent for 20 points.".
        public int Amount { get; set; }
        
        public void Use()
        {
            Console.WriteLine("Consumed consumable!");
        }

    }
}
