namespace Creature.Creature.States.Action
{
    public interface IAction
    {
        public void Do();
        public void Do(object argument);
    }
}
