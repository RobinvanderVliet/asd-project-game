using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Creature.Pathfinder;

namespace Creature.Creature.NeuralNetworking
{
    public class SmartCreatureActions
    {
        private Random _random = new Random();
        private PathFinder _pathfinder;

        private Stack<Node> path = new Stack<Node>();

        public void Wander(Vector2 loc) 
        {
            int newXLoc = _random.Next(0, 30);
            int newYLoc = _random.Next(0, 30);

            Vector2 destination = new Vector2(newXLoc, newYLoc);

            path = _pathfinder.FindPath(loc, destination);
        }

        public void Attack(ICreature player, ICreature smartmonster)
        {
            if(IsAdjacent(player.CreatureStateMachine.CreatureData.Position,smartmonster.CreatureStateMachine.CreatureData.Position))
            {
                smartmonster.ApplyDamage(smartmonster.CreatureStateMachine.CreatureData.Damage);
            }
        }

        public void Flee(ICreature player, ICreature smartmonster) 
        {
            if(player.CreatureStateMachine.CreatureData.Position.X == smartmonster.CreatureStateMachine.CreatureData.Position.X)
            {
                Vector2 newDestination = new Vector2(smartmonster.CreatureStateMachine.CreatureData.Position.X, smartmonster.CreatureStateMachine.CreatureData.Position.Y + 10);
                path = _pathfinder.FindPath(smartmonster.CreatureStateMachine.CreatureData.Position, newDestination);
            }
            else if (player.CreatureStateMachine.CreatureData.Position.Y == smartmonster.CreatureStateMachine.CreatureData.Position.Y)
            {
                Vector2 newDestination = new Vector2(smartmonster.CreatureStateMachine.CreatureData.Position.Y, smartmonster.CreatureStateMachine.CreatureData.Position.X + 10);
                path = _pathfinder.FindPath(smartmonster.CreatureStateMachine.CreatureData.Position, newDestination);
            }
            else
            {
                Vector2 newDestination = new Vector2(smartmonster.CreatureStateMachine.CreatureData.Position.Y+10, smartmonster.CreatureStateMachine.CreatureData.Position.X + 10);
                path = _pathfinder.FindPath(smartmonster.CreatureStateMachine.CreatureData.Position, newDestination);
            }
        }

        public void UseItem() 
        {
            //To be implemented
        }

        public void RunToMonster(ICreature monster, ICreature smartmonster) 
        {
            path = _pathfinder.FindPath(monster.CreatureStateMachine.CreatureData.Position, smartmonster.CreatureStateMachine.CreatureData.Position);
        }

        public void GrabItem(Vector2 loc) 
        {
            //To be implemented
        }

        private bool IsAdjacent(Vector2 loc1, Vector2 loc2)
        {
            float distance = Vector2.Distance(loc1, loc2);
            return (distance < 2);

        }

    }
}
