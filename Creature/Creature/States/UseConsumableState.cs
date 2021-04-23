using Creature.Creature;

namespace Creature
{
    public class UseConsumableState : ICreatureState
    {
        private Monster.Event _stateName;
        public Monster.Event StateName { get => _stateName; }
        public override void Do()
        {

        }
    }
}