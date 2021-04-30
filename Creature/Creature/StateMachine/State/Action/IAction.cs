namespace Creature.Creature.StateMachine.State.Action
{
    public interface IAction
    {
        public void Do();
        public void Do(object argument);
    }
}
