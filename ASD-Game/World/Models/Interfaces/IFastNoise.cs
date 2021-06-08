namespace ASD_Game.World.Models.Interfaces
{
    public interface IFastNoise
    {
        public float GetNoise(float x, float y);
        void SetNoiseType(FastNoiseLite.NoiseType cellular);
        void SetSeed(int seed);
        void SetFrequency(float f);
        void SetCellularReturnType(FastNoiseLite.CellularReturnType cellValue);
    }
}