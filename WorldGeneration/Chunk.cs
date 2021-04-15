namespace WorldGeneration
{
    public class Chunk
    {
        public int x { get; set; }
        public int y { get; set; }
        public Tile[,] map { get; set; }
    }
}