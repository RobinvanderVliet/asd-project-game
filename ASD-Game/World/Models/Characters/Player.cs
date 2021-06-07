using System.Diagnostics.CodeAnalysis;

namespace ASD_project.World.Models.Characters
{
    [ExcludeFromCodeCoverage]
    public class Player : Character
    {
        public int Stamina { get; set; }
        public Inventory Inventory { get; set; }
        public int RadiationLevel { get; set; }
        public int Team { get; set; }

        //random default values for health&stamina for now
        private const int HEALTHCAP = 100;
        private const int STAMINACAP = 10;

        public Player(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol, id)
        {
            Stamina = STAMINACAP;
            Health = HEALTHCAP;
            Inventory = new();
            RadiationLevel = 0;
            Team = 0;
        }
    }
}
