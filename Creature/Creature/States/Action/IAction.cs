using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Creature.States.Action
{
    interface IAction
    {
        public void Do();
        public void Do(object argument);
    }
}
