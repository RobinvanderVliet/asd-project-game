using Creature.Creature.StateMachine.Data;
using System;
using System.Numerics;

namespace Creature.Creature.StateMachine.State
{
    public class FleeFromCreatureState : CreatureState
    {
        private ICreatureData _target;
        public override void Do()
        {
            Vector2 newPosition = _creatureData.Position;
            if (Vector2.DistanceSquared(_creatureData.Position, _target.Position) <= Vector2.DistanceSquared(new Vector2(_creatureData.Position.X + 1, _creatureData.Position.Y + 1), _target.Position))
            {
                newPosition = new Vector2(_creatureData.Position.X + 1, _creatureData.Position.Y + 1);
            }
            else if (Vector2.DistanceSquared(_creatureData.Position, _target.Position) <= Vector2.DistanceSquared(new Vector2(_creatureData.Position.X - 1, _creatureData.Position.Y - 1), _target.Position))
            {
                newPosition = new Vector2(_creatureData.Position.X - 1, _creatureData.Position.Y - 1);
            }

            _creatureData.Position = newPosition; //TODO alter this to work with the actionhandler like in the WanderState
        }

        public void setTarget(ICreatureData target)
        {
            _target = target;
        }
       
    }
}