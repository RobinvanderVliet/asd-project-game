namespace WorldGeneration.Models.Interfaces
{
    public interface IHazardousTile : ITile
    {
        int GetDamage(int time);
    }
}