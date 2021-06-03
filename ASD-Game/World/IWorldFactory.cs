namespace ASD_project.World
{
    public interface IWorldFactory
    {
        IWorld GenerateWorldWithSeed(int seed, int chunkSize = 6);
    }
}
