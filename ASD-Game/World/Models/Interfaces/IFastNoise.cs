namespace WorldGeneration.Models.Interfaces
{
    public interface IFastNoise
    {
        public float GetNoise(float x, float y);
    }
}