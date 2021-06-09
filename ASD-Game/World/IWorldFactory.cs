namespace WorldGeneration
{
    public interface IWorldFactory
    {
        IWorld GenerateWorldWithSeed(int seed, int chunkSize = 6);
    }
}