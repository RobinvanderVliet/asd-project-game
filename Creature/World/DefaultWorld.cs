using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Creature.World
{
    class DefaultWorld : IWorld
    {
        private List<ICreature> _creatures;
        private List<IPlayer> _players;
        private int _size;

        public List<ICreature> creatures => creatures;

        public List<IPlayer> players => players;

        public int Size => _size;

        public DefaultWorld(int initialSize)
        {
            _size = initialSize;
            _creatures = new List<ICreature>();
            _players = new List<IPlayer>();
        }

        public void SpawnCreature(ICreature creature)
        {
            _creatures.Add(creature);
        }

        public void SpawnPlayer(IPlayer player)
        {
            _players.Add(player);
        }

        public void Render()
        {
            for (int y = _size; y > 0; y--) {
                string line = null;

                for (int x = 0; x < _size; x++)
                {
                    bool addedLine = false;

                    foreach (ICreature creature in _creatures)
                    {
                        if (creature.Position.X == x && creature.Position.Y == y)
                        {
                            line += "|||";
                            addedLine = true;
                        }
                    }

                    foreach (IPlayer player in _players)
                    {
                        if (player.Position.X == x && player.Position.Y == y)
                        {
                            line += "+++";
                            addedLine = true;
                        }
                    }
                    if (!addedLine) line += "---";
                }
                Console.WriteLine(line);
            }
        }
    }
}
