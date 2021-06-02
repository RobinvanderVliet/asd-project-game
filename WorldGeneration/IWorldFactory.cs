using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration
{
    public interface IWorldFactory
    {
        IWorld GenerateWorldWithSeed(int seed, int chunkSize = 6);
    }
}
