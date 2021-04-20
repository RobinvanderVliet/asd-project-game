using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Consumable
{
   public interface IConsumable
    {
        public String type { get; set; }
        public int amount { get; set; }
        
        public void applyEffect(String recipient)
        {
            //implement logic to translate the effect of the consumable to actions.
        }
        public void damageEnemie(String recipient)// Must be a object type from the attacking entity instead of String.
            //implement logic to damage an player.
        {
        }
        public void healPlayer(String recipient)// Must be a Object of type Player instead of String.
        {
            // Implement logic to heal an player.
        }
        public void lowerAmount()
        {
            this.amount -= 1;
        }
        

    }
}
