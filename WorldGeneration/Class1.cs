/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: prototyping the seed-based procedural world generation.
     
*/
using System;
using System.Collections.Generic;

namespace WorldGeneration
{

    public class Class1
    {
            public String prototype(String seed)
            {
                String result = "";
                List<String> datamap = generateChunk(seed);
                for (int x = 0; x < datamap.Count; x++)
                {
                    result += datamap[x];
                }
            return result;
        }

        private List<String> generateChunk(String seed)
        {
            // Create an empty result array.
            List<String> result = new List<string>();

            // Create and configure FastNoise object
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

            // Gather noise data
            float[] noiseData = new float[128 * 128];
            int index = 0;

            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    noiseData[index++] = noise.GetNoise(x, y);
                }
            }

            for (int x = 0; x < noiseData.Length; x++)
            {
                result.Add(generateTileName(noiseData[x]));
            }

            return result;
        }

        private String generateTileName (float noise)
        {
            int roundedNoise = (int) (((noise +1)*10)-10);
            if (roundedNoise < 0)
            {
                roundedNoise = roundedNoise * -1;
            }
            switch (roundedNoise)
            {
                case 0: return "Nul" + roundedNoise;
                case 1: return "Een" + roundedNoise;
                case 2: return "Twee" + roundedNoise;
                case 3: return "Drie" + roundedNoise;
                case 4: return "Vier" + roundedNoise;
                case 5: return "Vijf" + roundedNoise;
                case 6: return "Zes" + roundedNoise;
                case 7: return "Zeven" + roundedNoise;
                case 8: return "Acht" + roundedNoise;
                case 9: return "Negen" + roundedNoise;
                default: return "No valid tile detected, number was " + roundedNoise;
            }

        }
    }
}
