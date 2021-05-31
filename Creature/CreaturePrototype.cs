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
using System.Linq;

namespace Creature
{
    [ExcludeFromCodeCoverage]
    class CreaturePrototype
    {

        private static ICreature testAgent;
        static void Main(string[] args)
        {
            IWorld world = new DefaultWorld(25);

            NpcConfigurationService npcConfigurationService = new NpcConfigurationService(new List<NpcConfiguration>(), new FileToDictionaryMapper());
            npcConfigurationService.CreateNpcConfiguration("zombie", SuperUgly.MONSTER_PATH);
            npcConfigurationService.CreateNpcConfiguration("zombie", SuperUgly.MONSTER_PATH);
            npcConfigurationService.CreateNpcConfiguration("zombie", SuperUgly.MONSTER_PATH);

            List<Dictionary<string, string>> listOfDictionaries = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>()
                {
                    {"combat_default_monster_comparable","monster"},
                    {"combat_default_monster_threshold","player"},
                    {"combat_default_monster_comparison","sees"},
                    {"combat_default_monster_comparison_true","follow"},
                    {"combat_default_monster_comparison_false","flee"}
                },
                new Dictionary<string,string>()
                {
                    {"combat_default_monster_comparable","monster"},
                    {"combat_default_monster_threshold","player"},
                    {"combat_default_monster_comparison","nearby"},
                    {"combat_default_monster_comparison_true","attack"}
                }
            };

            var lines = listOfDictionaries[0].Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            System.Diagnostics.Debug.WriteLine(string.Join(Environment.NewLine, lines));

            //PlayerData playerData = new PlayerData(new Vector2(5, 5), 20, 5, 10, world, npcConfigurationService.GetConfigurations()[0].Settings);
            PlayerData playerData = new PlayerData(new Vector2(5, 5), 100, 5, 10, world, listOfDictionaries);
            AgentData agentData = new AgentData(new Vector2(10, 10), 100, 100, 5, 50, world, listOfDictionaries, false);
            MonsterData monsterData = new MonsterData(new Vector2(10, 15), 20, 5, 50, world, listOfDictionaries, false);

            ICreature player = new Player(playerData);
            testAgent = new Agent(agentData);
            ICreature agent = new Agent(agentData);
            ICreature monster = new Monster(monsterData);

            world.GenerateWorldNodes();
            world.SpawnPlayer(player);
            world.SpawnAgent(testAgent);
            //world.SpawnAgent(agent);

            // TODO: fix monster statemachine to get this working
            //world.SpawnCreature(monster);

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
                case "attack":
                    testAgent.ApplyDamage(player.Damage);
                    break;
                default:
                    break;
            }
        }

        private void setTestAgent(ICreature agent)
        {
            testAgent = agent;
        }
        
    }
}
