using LiteDB;

namespace WorldGeneration
{
    public class Chunk
    {
        public Chunk(int x, int y, Tile[] map, int rowSize)
        {
            this.x = x;
            this.y = y;
            this.map = map;
            this.rowSize = rowSize;
        }
        
        public Chunk()
        {
        }

        public int x { get; set; }
        public int y { get; set; }
        public Tile[] map { get; set; }
        
        public int rowSize { get; set; }
    }
}