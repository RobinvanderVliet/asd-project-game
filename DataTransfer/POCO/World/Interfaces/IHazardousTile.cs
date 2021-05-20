namespace DataTransfer.POCO.World.Interfaces
{
    public interface IHazardousTile : ITile
    {
        int Damage { get; set; }
        int GetDamage(int time);
    }
}