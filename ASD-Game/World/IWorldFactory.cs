namespace ASD_Game.World
{
    public interface IWorldFactory
    {
        IWorld GenerateWorldWithSeed(int seed, int chunkSize = 6);
    }
}
