using System;
using System.Collections.Generic;

namespace WorldGeneration
{
    public class WorldGenerationPrototype
    {
        private readonly FastNoiseLite _noise;

        public WorldGenerationPrototype(int seed)
        {
            // Create and configure FastNoise object
            _noise = new FastNoiseLite();
            _noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            _noise.SetSeed(seed);
        }

        public void SetNoiseFrequency(float frequency)
        {
            _noise.SetFrequency(frequency);
        }

        public void GenerateThreeWorldsWithDifferentFrequencyButTheSameSeed(int seed)
        {
            _noise.SetSeed(seed);
        }

        public string PrototypeWorldGeneration()
        {
            var result = "";
            var datamap = GenerateChunk();
            foreach (var chunk in datamap) result += chunk;
            return result;
        }

        private IEnumerable<string> GenerateChunk()
        {
            // Create an empty result array.
            IEnumerable<string> result = new List<string>();

            // Set number of rows and columns to generate. In order to test the prototype, this is simplified to a single number.
            const int NUMBER = 10;

            // Gather noise data
            var noiseData = new float[NUMBER, NUMBER];

            for (var y = 0; y < NUMBER; y++)
            {
                for (var x = 0; x < NUMBER; x++)
                {
                    noiseData[x, y] = _noise.GetNoise(x, y);
                    if ((int)Math.Floor(noiseData[x, y] * 10) > -1)
                        Console.Write("_" + (int)Math.Floor(noiseData[x, y] * 10));
                    else
                        Console.Write((int)Math.Floor(noiseData[x, y] * 10));
                }

                Console.WriteLine("");
            }

            return result;
        }
    }
}