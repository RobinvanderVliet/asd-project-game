using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ActionHandling;
using ASD_Game.ActionHandling;
using ASD_Game.World.Services;

namespace ASD_Game.World.Models.Characters.Algorithms.Creator
{
    [ExcludeFromCodeCoverage]
    public class AgentCreator : IAgentCreator
    {
        private readonly IMoveHandler _moveHandler;
        private readonly IWorldService _worldService;
        private readonly IAttackHandler _attackHandler;

        public AgentCreator(IMoveHandler moveHandler, IWorldService worldService, IAttackHandler attackHandler)
        {
            _moveHandler = moveHandler;
            _worldService = worldService;
            _attackHandler = attackHandler;
        }
        
        public global::World.Models.Characters.AgentAI CreateAgent(Player player, List<KeyValuePair<string, string>> agentConfiguration)
        {
            return new(player.Name, player.XPosition, player.YPosition, player.Symbol, player.Id)
            {
                AgentData =
                {
                    MoveHandler = _moveHandler, WorldService = _worldService, Health = player.Health,
                    Inventory = player.Inventory, Stamina = player.Stamina, Team = player.Team,
                    RadiationLevel = player.RadiationLevel, VisionRange = 6, RuleSet = agentConfiguration,
                    AttackHandler = _attackHandler, CharacterId = player.Id
                }
            };
        }
    }
}