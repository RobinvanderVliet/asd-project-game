namespace ASD_project.World.Models.Interfaces
{
    public interface IHazardousTile : ITile
    {
        int Damage { get; set; }
        int GetDamage(int time);
    }
}