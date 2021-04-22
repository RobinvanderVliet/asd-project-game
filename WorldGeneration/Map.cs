using System.Collections.Generic;
using WorldGeneration.Models;

namespace WorldGeneration
{
    public class Map
    {
        private List<Chunk> _chunks;
        private int chunkSize;

        public Map(int chunkSize = 16)
        {
            this.chunkSize = chunkSize;
        }

        public void loadArea(int[] playerLocation, int viewDistance)
        {
            var maxX = (playerLocation[0] + viewDistance + chunkSize) / chunkSize;
            var minX = (playerLocation[0] - viewDistance - chunkSize * 2) / chunkSize; // chunks beginnen links bovenin, dus daarom *2
            var maxY = (playerLocation[1] + viewDistance + chunkSize * 2) / chunkSize;
            var minY = (playerLocation[1] - viewDistance - chunkSize) / chunkSize;
            var loadingChunks = new List<int[,]>();
            var chunksToLoad = new List<Chunk>();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    loadingChunks.Add(new int[x,y]);
                }
            }
            
        }
    }
}       