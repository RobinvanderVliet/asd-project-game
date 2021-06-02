using System.Diagnostics.CodeAnalysis;

namespace WorldGeneration
{
    [ExcludeFromCodeCoverage]
    public class Player : Character
    {
        public string Id { get; set; }
        public int Stamina { get; set; }
        public Inventory Inventory { get; set; }
        public int RadiationLevel { get; set; }
        public int Team { get; set; }

        //random default values for health&stamina for now
        public const int HEALTH_MIN = 0;
        public const int HEALTH_MAX = 100;
        public const int STAMINA_MIN = 0;
        public const int STAMINA_MAX = 100;
        public const int RADIATION_LEVEL_MIN = 0;
        public const int RADIATION_LEVEL_MAX = 100;

        public Player(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol)
        {
            Id = id;
            Stamina = STAMINA_MAX;
            Health = HEALTH_MAX;
            Inventory = new();
            RadiationLevel = RADIATION_LEVEL_MIN;
            Team = 0;
        }
        
        public void AddStamina(int amount)
        {
            Stamina += amount;

            switch (Stamina)
            {
                case < STAMINA_MIN:
                    Stamina = STAMINA_MIN;
                    break;
                case > STAMINA_MAX:
                    Stamina = STAMINA_MAX;
                    break;
            }
        }

        public void AddRadiationLevel(int amount)
        {
            RadiationLevel += amount;

            switch (RadiationLevel)
            {
                case < RADIATION_LEVEL_MIN:
                    RadiationLevel = RADIATION_LEVEL_MIN;
                    break;
                case > RADIATION_LEVEL_MAX:
                    RadiationLevel = RADIATION_LEVEL_MAX;
                    break;
            }
        }

        public void AddHealth(int amount)
        {
            Health += amount;

            switch (Health)
            {
                case < HEALTH_MIN:
                    Health = HEALTH_MIN;
                    break;
                case > HEALTH_MAX:
                    Health = HEALTH_MAX;
                    break;
            }
        }
    }
}
