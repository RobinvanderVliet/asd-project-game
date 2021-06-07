using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_project.Creature.Consumable
{
    [ExcludeFromCodeCoverage]
    public class Consumable : IConsumable
    {
        public string Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Amount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
