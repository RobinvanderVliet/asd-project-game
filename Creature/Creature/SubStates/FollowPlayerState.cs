using Creature.Creature;

namespace Creature
{
    public class FollowPlayerState : ICreatureStateInterface
    {
        private readonly Monster.Event _stateName;
        public Monster.Event StateName { get => _stateName; }
        public override void Do()
        {

        }
    }
}