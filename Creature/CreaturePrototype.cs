using Creature.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Creature
{
    class CreaturePrototype
    {
        static void Main(string[] args)
        {
            IWorld world = new DefaultWorld(25);
            IPlayer player = new Player(10, new Vector2(3, 2));
            ICreature creature = new Monster(world, new Vector2(10, 10), 2, 6, 50);
            ICreature creature2 = new Monster(world, new Vector2(20, 20), 2, 6, 50);

            world.SpawnPlayer(player);
            world.SpawnCreature(creature);
            world.SpawnCreature(creature2);

            world.Render();

            while (true)
            {
                string input = Console.ReadLine();

                MovePlayer(player, input);
                world.Render();
            }
        }

        private static void MovePlayer(IPlayer player, string input)
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
