using Creature.Creature.StateMachine.Data;
using Creature.World;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Creature.Creature.StateMachine;
using System.Linq;

namespace Creature
{
    [ExcludeFromCodeCoverage]
    class CreaturePrototype
    {
        static void Main(string[] args)
        {
            IWorld world = new DefaultWorld(25);

            // NpcConfigurationService npcConfigurationService = new NpcConfigurationService(new List<NpcConfiguration>(), new FileToDictionaryMapper()); //broke
            // npcConfigurationService.CreateNpcConfiguration("zombie", SuperUgly.MONSTER_PATH);
            // npcConfigurationService.CreateNpcConfiguration("zombie", SuperUgly.MONSTER_PATH);
            // npcConfigurationService.CreateNpcConfiguration("zombie", SuperUgly.MONSTER_PATH);

            List<ValueTuple<string, string>> listOfDictionaries = new List<ValueTuple<string, string>>()
            {
                new ValueTuple<string, string>("combat_default_monster_comparable", "monster"),
                new ValueTuple<string, string>("combat_default_monster_threshold", "player"),
                new ValueTuple<string, string>("combat_default_monster_comparison", "sees"),
                new ValueTuple<string, string>("combat_default_monster_comparison_true", "follow"),
                new ValueTuple<string, string>("combat_default_monster_comparison_false", "flee"),
                new ValueTuple<string, string>("combat_default_monster_comparable", "monster"),
                new ValueTuple<string, string>("combat_default_monster_threshold", "player"),
                new ValueTuple<string, string>("combat_default_monster_comparison", "nearby"),
                new ValueTuple<string, string>("combat_default_monster_comparison_true", "attack")
            };

            //PlayerData playerData = new PlayerData(new Vector2(5, 5), 20, 5, 10, world, npcConfigurationService.GetConfigurations()[0].Settings);
            ICreatureData playerData = new PlayerData(new Vector2(5, 5), 100, 5, 10, listOfDictionaries);
            ICreatureData agentData = new AgentData(new Vector2(10, 10), 100, 100, 5, 50, listOfDictionaries, false);
            ICreatureData monsterData = new MonsterData(new Vector2(10, 15), 20, 5, 50,listOfDictionaries, false);

            ICreatureStateMachine monsterStateMachine = new MonsterStateMachine(monsterData);
            ICreature monster = new Monster(monsterStateMachine);

            ICreatureStateMachine playerStateMachine = new PlayerStateMachine(playerData);
            ICreature player = new Player(playerStateMachine);
            
            ICreatureStateMachine agentStateMachine = new AgentStateMachine(monsterData);
            ICreature agent = new Monster(agentStateMachine);

            world.GenerateWorldNodes();
            world.SpawnPlayer(player);
            //world.SpawnAgent(agent);
            // TODO: fix monster statemachine to get this working
            world.SpawnCreature(monster);

            world.Render();

            while (true)
            {
                string input = Console.ReadLine();

                MovePlayer(playerData, input);
                world.Render();
            }
        }

        private static void MovePlayer(ICreatureData player, string input)
        {
            switch (input)
            {
                case "w":
                    player.Position = new Vector2(player.Position.X, player.Position.Y + 1);
                    break;
                case "a":
                    player.Position = new Vector2(player.Position.X - 1, player.Position.Y);
                    break;
                case "s":
                    player.Position = new Vector2(player.Position.X, player.Position.Y - 1);
                    break;
                case "d":
                    player.Position = new Vector2(player.Position.X + 1, player.Position.Y);
                    break;
                default:
                    break;
            }
        }
    }
}
