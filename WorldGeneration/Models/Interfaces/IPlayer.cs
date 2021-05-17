using System.Reflection;

namespace WorldGeneration.Models.Interfaces
{
    public interface IPlayer : ICharacter
    {
        string Name { get; set; }
        int Health { get; set;}
        int RadiationLevel { get; set; }
        // inventory met items
        int Armor { get; set; }
        int Bitcoins { get; set; }
    }
}