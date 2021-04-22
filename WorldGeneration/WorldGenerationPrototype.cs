using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldGeneration
{

    public class WorldGenerationPrototype
    {

        private FastNoiseLite noise;

        public WorldGenerationPrototype(int seed)
        {
            // Create and configure FastNoise object
            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            noise.SetSeed(seed);
        }

        public void SetNoiseFrequency(float frequency)
        {
            noise.SetFrequency(frequency);
        }
        
        public void GenerateThreeWorldsWithDifferentFrequencyButTheSameSeed (int seed){
            noise.SetSeed(seed);
        }

        public string PrototypeWorldGeneration()
        {
            var result = "";
            var datamap = GenerateChunk();
            foreach (var chunk in datamap)
            {
                result += chunk;
            }
            return result;
        }

        private IEnumerable<string> GenerateChunk()
        {
            // Create an empty result array.
            IEnumerable<string> result = new List<string>();
            
            // Set number of rows and columns to generate. In order to test the prototype, this is simplified to a single number.
            const int number = 10;

            // Gather noise data
            var noiseData = new float[number,number];

            for (var y = 0; y < number; y++)
            {
                for (var x = 0; x < number; x++)
                {
                    noiseData[x,y] = noise.GetNoise(x, y);
                    if ((int) Math.Floor(noiseData[x, y] * 10) > -1)
                    {
                        Console.Write("_" + (int)Math.Floor(noiseData[x,y] * 10));
                    }
                    else
                    {
                        Console.Write((int)Math.Floor(noiseData[x,y] * 10));
                    }
                    
                }
                Console.WriteLine("");
            }
            return result;
        }
    }
}