namespace ASD_Game.World.Models.Interfaces
{
    public interface IHazardousTile : ITile
    {
        int Damage { get; set; }
        int GetDamage(int time);
    }
}