using Creature.Creature.StateMachine.Data;
using System;
using System.Numerics;

namespace Creature.Creature.StateMachine.State
{
    public class FleeFromCreatureState : CreatureState
    {
        public FleeFromCreatureState(ICreatureData creatureData) : base(creatureData)
        {
            _creatureData = creatureData;
        }

        public override void Do()
        {
            throw new NotImplementedException();
        }

        public override void Do(ICreatureData creatureData)
        {
            ICreatureData playerData = creatureData;

            Vector2 newPosition = _creatureData.Position;
            if (Vector2.DistanceSquared(_creatureData.Position, playerData.Position) <= Vector2.DistanceSquared(new Vector2(_creatureData.Position.X + 1, _creatureData.Position.Y + 1), playerData.Position))
            {
                newPosition = new Vector2(_creatureData.Position.X + 1, _creatureData.Position.Y + 1);
            }
            else if (Vector2.DistanceSquared(_creatureData.Position, playerData.Position) <= Vector2.DistanceSquared(new Vector2(_creatureData.Position.X - 1, _creatureData.Position.Y - 1), playerData.Position))
            {
                newPosition = new Vector2(_creatureData.Position.X - 1, _creatureData.Position.Y - 1);
            }

            _creatureData.Position = newPosition;
        }
    }
}