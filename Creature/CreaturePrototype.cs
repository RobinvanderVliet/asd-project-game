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
using Creature.Creature.StateMachine.CustomRuleSet;

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
            
            PlayerData playerData = new PlayerData(new Vector2(5, 5), 20, 5, 10, world);
            MonsterData monsterData = new MonsterData(new Vector2(10, 10), 20, 5, 50, world, false);
            MonsterData monsterData2 = new MonsterData(new Vector2(20, 20), 20, 5, 50, world, false);

            RuleSet monsterRuleSet = new RuleSet(npcConfigurationService.GetConfigurations()[0].Settings);
            RuleSet playerRuleSet = new RuleSet(npcConfigurationService.GetConfigurations()[1].Settings);

            ICreature player = new Player(new PlayerStateMachine(playerData, playerRuleSet));
            ICreature creature = new Monster(monsterData, monsterRuleSet);
            ICreature creature2 = new Monster(monsterData2, monsterRuleSet);

            world.GenerateWorldNodes();
            world.SpawnPlayer(player);
            world.SpawnCreature(creature);
            world.SpawnCreature(creature2);

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
