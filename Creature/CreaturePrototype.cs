using Agent.Mapper;
using Agent.Services;
using Agent.Models;
using Creature.Creature.StateMachine.Data;
using Creature.World;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

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

            PlayerData playerData = new PlayerData(new Vector2(5, 5), 20, 5, 10, world, npcConfigurationService.GetConfigurations()[0].Settings);
            //AgentData agentData = new AgentData(new Vector2(10, 10), 20, 5, 50, world, false, npcConfigurationService.GetConfigurations()[1].Settings);
            MonsterData monsterData = new MonsterData(new Vector2(10, 15), 20, 5, 50, world, npcConfigurationService.GetConfigurations()[2].Settings, false);

            ICreature player = new Player(playerData);
            //ICreature agent = new Agent(agentData);
            ICreature monster = new Monster(monsterData);

            world.GenerateWorldNodes();
            world.SpawnPlayer(player);
            //world.SpawnAgent(agent);
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
