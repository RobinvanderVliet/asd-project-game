using Creature.Creature.StateMachine.Data;
using Creature.World;
using System;
using System.Numerics;

namespace Creature
{
    class CreaturePrototype
    {
        static void Main(string[] args)
        {
            IWorld world = new DefaultWorld(25);

            PlayerData playerData = new PlayerData(true, new Vector2(5, 5), 20, 5, 10, world);
            MonsterData monsterData = new MonsterData(true, new Vector2(10, 10), 20, 5, 15, world, false);
            MonsterData monsterData2 = new MonsterData(true, new Vector2(20, 20), 20, 5, 15, world, false);

            ICreature player = new Player(playerData);
            ICreature creature = new Monster(monsterData);
            ICreature creature2 = new Monster(monsterData2);

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
