using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ASD_Game.Creature.Creature;
using ASD_Game.Creature.Creature.StateMachine.Event;
using ASD_Game.Creature.Pathfinder;

namespace ASD_Game.Creature.World
{
    [ExcludeFromCodeCoverage]
    public class DefaultWorld : IWorld
    {
        private List<ICreature> _creatures;
        private List<ICreature> _players;
        private List<List<Node>> _nodes;
        private int _size;

        public List<ICreature> Creatures => _creatures;

        public List<ICreature> Players => _players;

        public List<List<Node>> Nodes => _nodes;
        public int Size => _size;

        public DefaultWorld(int initialSize)
        {
            _size = initialSize;
            _nodes = new List<List<Node>>();
            _creatures = new List<ICreature>();
            _players = new List<ICreature>();
        }

        public void GenerateWorldNodes()
        {
            for (int row = 0; row < _size; row++)
            {
                List<Node> nodePoints = new List<Node>();
                for (int col = 0; col < _size; col++)
                {
                    Vector2 nodeLocation = new Vector2(row, col);
                    Node node = new Node(nodeLocation, true);
                    nodePoints.Add(node);
                }
                _nodes.Add(nodePoints);
            }
        }

        public void SpawnCreature(ICreature creature)
        {
            _creatures.Add(creature);
        }

        public void SpawnPlayer(ICreature player)
        {
            _players.Add(player);
        }

        public void Render()
        {
            foreach (ICreature creature in _creatures)
            {
                ICreature player = _players[0];
                if (Vector2.DistanceSquared(creature.CreatureStateMachine.CreatureData.Position, player.CreatureStateMachine.CreatureData.Position) < creature.CreatureStateMachine.CreatureData.VisionRange)
                {
                    creature.CreatureStateMachine.FireEvent(CreatureEvent.Event.SpottedPlayer, player.CreatureStateMachine.CreatureData);

                    Vector2 monsterPosition = creature.CreatureStateMachine.CreatureData.Position;
                    Vector2 playerPosition = player.CreatureStateMachine.CreatureData.Position;

                    if ((monsterPosition.X + 1f) == playerPosition.X && (monsterPosition.Y == playerPosition.Y)
                        || (monsterPosition.X - 1f) == playerPosition.X && (monsterPosition.Y == playerPosition.Y)
                        || (monsterPosition.Y + 1f) == playerPosition.Y && (monsterPosition.X == playerPosition.X)
                        || (monsterPosition.Y - 1f) == playerPosition.Y && (monsterPosition.X == playerPosition.X))
                    {
                        creature.CreatureStateMachine.FireEvent(CreatureEvent.Event.PlayerInRange, player.CreatureStateMachine.CreatureData);
                    }
                    else
                    {
                        creature.CreatureStateMachine.FireEvent(CreatureEvent.Event.PlayerOutOfRange, player.CreatureStateMachine.CreatureData);
                    }
                }
                else
                {
                    creature.CreatureStateMachine.FireEvent(CreatureEvent.Event.LostPlayer, player.CreatureStateMachine.CreatureData);
                }
            }

            for (int y = _size; y > 0; y--)
            {
                string line = null;

                for (int x = 0; x < _size; x++)
                {
                    bool addedLine = false;
                    ICreature player = _players[0];

                    if (player.CreatureStateMachine.CreatureData.Position.X == x && player.CreatureStateMachine.CreatureData.Position.Y == y)
                    {
                        line += "+";
                        addedLine = true;
                    }
                    foreach (ICreature creature in _creatures)
                    {
                        if (creature.CreatureStateMachine.CreatureData.Position.X == x && creature.CreatureStateMachine.CreatureData.Position.Y == y)
                        {
                            line += "|";
                            addedLine = true;
                        }
                    }
                    if (!addedLine) line += "-";
                }
                Console.WriteLine(line);
            }
        }
    }
}
