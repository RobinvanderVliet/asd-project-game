using Agent.Mapper;
using Agent.Services;
using Agent.Models;
using Creature.Creature.StateMachine.Data;
using Creature.World;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Creature.Creature.StateMachine;

namespace Creature
{
    [ExcludeFromCodeCoverage]
    class CreaturePrototype
    {
        static void Main(string[] args)
        {
            IWorld world = new DefaultWorld(25);

            NpcConfigurationService npcConfigurationService = new NpcConfigurationService(new List<NpcConfiguration>(), new FileToDictionaryMapper());
            npcConfigurationService.CreateNpcConfiguration("zombie", SuperUgly.MONSTER_PATH);
            npcConfigurationService.CreateNpcConfiguration("zombie", SuperUgly.MONSTER_PATH);
            npcConfigurationService.CreateNpcConfiguration("zombie", SuperUgly.MONSTER_PATH);

            PlayerData playerData = new PlayerData(new Vector2(5, 5), 20, 5, 10, world, new List<Setting>());
            AgentData agentData = new AgentData(new Vector2(10, 10), 20, 5, 50, world, new List<Setting>(), false);


            var settings = new List<Setting>();
            settings.Add(new Setting("combat_engage_inventory_comparable", "inventory"));
            settings.Add(new Setting("combat_engage_inventory_treshold", "knife"));
            settings.Add(new Setting("combat_engage_inventory_comparison", "contains"));
            settings.Add(new Setting("combat_engage_inventory_comparison_true", "use knife"));
            settings.Add(new Setting("explore_engage_inventory_comparison_true", "use knife"));

            MonsterData monsterData = new MonsterData(new Vector2(10, 15), 20, 5, 50, world, settings, false);

            ICreature player = new Player(playerData);
            ICreature agent = new Agent(agentData);
            ICreature monster = new Monster(monsterData);

            world.GenerateWorldNodes();
            world.SpawnPlayer(player);
            
            world.SpawnAgent(agent);

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
