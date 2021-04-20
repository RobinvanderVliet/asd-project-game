/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: prototyping the seed-based procedural world generation.
     
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldGeneration
{

    public class Class1
    {

        private FastNoiseLite noise;

        public Class1(int seed)
        {
            // Create and configure FastNoise object
            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetSeed(seed);
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
            
            // Gather noise data
            float[] noiseData = new float[10 * 10];
            int index = 0;

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    noiseData[index++] = noise.GetNoise(x, y);
                }
            }

            for (int x = 0; x < 100; x++)
            {
                // Use the noise data to generate some random tiles.
                result.Add(generateTileName(noiseData[x]));
            }
            return result;
        }

        private String generateTileName (float randomNumber)
        {
            // Noise is a float, generate an int between 0 and 10.
            int roundedNumber = (int) (((randomNumber +1)*10)-10);
            if (roundedNumber < 0)
            {
                roundedNumber = roundedNumber * -1;
            }
            switch (roundedNumber)
            {
                case 0: return "Nul, ";
                case 1: return "Een, ";
                case 2: return "Twee, ";
                case 3: return "Drie, ";
                case 4: return "Vier, ";
                case 5: return "Vijf, ";
                case 6: return "Zes, ";
                case 7: return "Zeven, ";
                case 8: return "Acht, ";
                case 9: return "Negen, ";
                default: return "No valid tile detected, number was " + roundedNumber;
            }

        }
    }
}