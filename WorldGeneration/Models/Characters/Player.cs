﻿using System.Diagnostics.CodeAnalysis;

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
        public const int HEALTHCAP = 100;
        public const int STAMINACAP = 100;
        public const int RADIATIONLEVELCAP = 100;

        public Player(string name, int xPosition, int yPosition, string symbol, string id) : base(name, xPosition, yPosition, symbol)
        {
            Id = id;
            Stamina = STAMINACAP;
            Health = HEALTHCAP;
            Inventory = new();
            RadiationLevel = 10;
            Team = 0;
        }
        
        public void AddStamina(int amount)
        {
            Stamina += amount;
            if (Stamina > STAMINACAP)
            {
                Stamina = STAMINACAP;
            }
        }
        
        public void AddRadiationLevel(int amount)
        {
            RadiationLevel += amount;
            if (RadiationLevel > RADIATIONLEVELCAP)
            {
                RadiationLevel = RADIATIONLEVELCAP;
            }
        }
        
        public void AddHealth(int amount)
        {
            Health += amount;
            if (Health > HEALTHCAP)
            {
                Health = HEALTHCAP;
            }
        }
    }
}
