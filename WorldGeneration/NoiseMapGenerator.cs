using System;

namespace WorldGeneration
{
    public abstract class NoiseMapGenerator
    {
        public int[,] GenerateAreaMap(int size, int seed)
        {
            var noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            noise.SetSeed(seed);
            noise.SetFrequency(0.015f);
            noise.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
            int[,] noiseData = new int[size,size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    noiseData[x,y] = (int)noise.GetNoise(x, y);
                }
                Console.WriteLine("");
            }
            return noiseData;
        }
    }
}