using System.Collections.Generic;
using System.Numerics;
using ActionHandling;
using World.Models.Characters.StateMachine.Builder;
using WorldGeneration;

namespace World.Models.Characters.StateMachine.Data
{
    public interface ICharacterData
    {
        public bool IsAlive { get; }
        public Vector2 Position { get; set; }
        public int VisionRange { get; set; }
        public double Health { get; set; }

        public List<KeyValuePair<string, string>> RuleSet { get; set; }
        public Inventory Inventory { get; set; }
        public int Team { get; set; }
        public int RadiationLevel { get; set; }
        public IMoveHandler MoveHandler { get; set; }
        public IWorldService WorldService { get; set; }
        public BuilderConfigurator BuilderConfigurator { get; set; }
        public IAttackHandler AttackHandler { get; set; }
    }
}