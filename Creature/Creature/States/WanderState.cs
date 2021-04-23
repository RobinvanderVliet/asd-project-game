using Creature.Creature;

namespace Creature
{
    public class WanderState : ICreatureStateInterface
    {
        private Monster.Event _stateName;
        public Monster.Event StateName { get => _stateName; }
        public override void Do()
        {

        }
    }
}