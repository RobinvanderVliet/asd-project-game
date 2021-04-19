using LiteDB;

namespace WorldGeneration
{
    public class Chunk
    {
        [BsonCtor]
        public Chunk(int x, int y, Tile[,] map)
        {
            this.x = x;
            this.y = y;
            this.map = map;
        }
        public Chunk(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Chunk()
        {
        }

        public int x { get; set; }
        public int y { get; set; }
        public Tile[,] map { get; set; }
    }
}