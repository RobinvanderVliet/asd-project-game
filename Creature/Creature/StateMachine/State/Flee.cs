using System;
using System.Numerics;
using Creature.Creature.StateMachine.Data;

namespace Creature.Creature.StateMachine.State
{
    public class Flee : CreatureState
    {
        public Flee(ICreatureData creatureData, ICreatureData enemie) : base(creatureData)
        {
            _creatureData = creatureData;
        }

        public override void Do(ICreatureData creatureData)
        {
            ICreatureData playerData = creatureData;

            Vector2 newMonsterPosition = _creatureData.Position;
            if (Vector2.DistanceSquared(_creatureData.Position, playerData.Position) <= Vector2.DistanceSquared(new Vector2(_creatureData.Position.X + 1, _creatureData.Position.Y + 1), playerData.Position))
            {
                newMonsterPosition = new Vector2(_creatureData.Position.X + 1, _creatureData.Position.Y + 1);
            }
            else if (Vector2.DistanceSquared(_creatureData.Position, playerData.Position) <= Vector2.DistanceSquared(new Vector2(_creatureData.Position.X - 1, _creatureData.Position.Y - 1), playerData.Position))
            {
                newMonsterPosition = new Vector2(_creatureData.Position.X - 1, _creatureData.Position.Y - 1);
            }

            _creatureData.Position = newMonsterPosition;
        }
    }
}