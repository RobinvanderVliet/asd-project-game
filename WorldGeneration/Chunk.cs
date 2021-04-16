namespace WorldGeneration
{
    public class Chunk
    {
        public Chunk(int x, int y, Tile[,] map)
        {
            this.x = x;
            this.y = y;
            this.map = map;
        }

        public int x { get; set; }
        public int y { get; set; }
        public Tile[,] map { get; set; }
    }
}