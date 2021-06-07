using System.Collections.Generic;
using System.Numerics;
using ActionHandling;
using WorldGeneration;

namespace Creature.Creature.StateMachine.Data
{
    public interface ICreatureData
    {
        bool IsAlive { get; }
        Vector2 Position { get; set; }
        int VisionRange { get; set; }
        double Health { get; set; }
        List<KeyValuePair<string, string>> RuleSet { get; }
        public Inventory Inventory { get; set; }
        public int Team { get; set; }
        public int RadiationLevel { get; set; }
        IMoveHandler MoveHandler { get; set; }
        IWorldService WorldService { get; set; }
    }
}