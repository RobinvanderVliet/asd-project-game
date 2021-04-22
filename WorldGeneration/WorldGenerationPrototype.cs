using System;
using System.Collections.Generic;

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
        
        public void generateThreeWorldsWithDifferentFrequenceButTheSameSeed (int seed){
            noise.SetSeed(seed);
            SetNoiseFrequency (0.9f);
            Console.WriteLine(PrototypeWorldGeneration());
            SetNoiseFrequency (0.1f);
            Console.WriteLine(PrototypeWorldGeneration());
            SetNoiseFrequency (0.01f);
            Console.WriteLine(PrototypeWorldGeneration());
        }

        public String PrototypeWorldGeneration()
        {
            String result = "";
            List<String> datamap = GenerateChunk();
            for (int x = 0; x < datamap.Count; x++)
            {
                result += (datamap[x]);
            }
            return result;
        }

        private List<String> GenerateChunk()
        {
            // Create an empty result array.
            List<String> result = new List<string>();
            
            // Set number of rows and columns to generate. In order to test the prototype, this is simplified to a single number.
            int  number = 10;

            // Gather noise data
            float[,] noiseData = new float[number,number];

            for (int y = 0; y < number; y++)
            {
                for (int x = 0; x < number; x++)
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