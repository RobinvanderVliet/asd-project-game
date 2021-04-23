using Creature.Creature;

namespace Creature
{
    public class FollowPlayerState : ICreatureState
    {
        private readonly Monster.Event _stateName;
        public Monster.Event StateName { get => _stateName; }
        public override void Do()
        {

        }
    }
}