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

        }
        public void damageEnemie(String enemie); // Must be a object type from the attacking entity instead of String.
        public void healPlayer(String playername); // Must be a Object of type Player instead of String.
        

        

    }
}
