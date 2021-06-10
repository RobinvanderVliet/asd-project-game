using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ASD_Game.InputHandling.Models
{
    public class BaseVariables
    {
        public readonly List<string> comparables = new() 
        {
            "player", 
            "agent",
            "health", 
            "inventory", 
            "item"
        };

        public readonly List<string> comparison = new() 
        { 
            "greater than", 
            "less than", 
            "nearby", 
            "contains", 
            "finds", 
            "does not contain" 
        };
        public  readonly List<string> consumables = new()
        {
            "bandage",
            "morphine",
            "medkit",
            "big-mac",
            "monster-energy",
            "suspicious-white-powder",
            "lodine-tablets"
        };

        public readonly List<string> armor = new()
        {
            "jacket",
            "flak-vest",
            "tactical-vest",
            "bandana",
            "hard-hat",
            "military-helmet",
            "gas-mask",
            "hazmat-suit"
        };

        public readonly List<string> actionReferences = new()
        {
            "walk",
            "attack",
            "grab",
            "drop",
            "flee",
            "use",
            "replace"
        };

        public readonly List<string> tiles = new()
        {
            "street",
            "grass",
            "dirt",
            "water",
            "office-space",
            "airplane",
            "house",
            "gas",
            "spikes",
            "fire",
            "health-boost",
            "stamina-boost",
            "bitcoin-mining-farm",
            "chest",
            "door",
            "wall"
        };

         public readonly List<string> actions = new()
        {
            "engage",
            "collect"
        };

        public readonly List<string> weapons = new()
        {
            "knife",
            "baseball-bat",
            "katana",
            "glock",
            "p90",
            "ak-47"
        };

        public  readonly List<string> bitcoinItems = new()
        {
            "scrap-electronics",
            "gpu-upgrade",
            "usb-stick",
            "bitcoin-wallet"
        };

        [ExcludeFromCodeCoverage]
        public List<string> ReturnAllItems()
        {
            return new List<string>().Concat(weapons).Concat(armor).Concat(consumables).Concat(bitcoinItems).Concat(tiles).ToList();
        }
    }
}
