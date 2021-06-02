using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration
{
    interface IRandomItemGenerator
    {
        Item GetRandomItem(float noise);
    }
}
