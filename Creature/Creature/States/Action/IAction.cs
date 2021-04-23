namespace Creature.Creature.States.Action
{
    interface IAction
    {
        public void Do();
        public void Do(object argument);
    }
}
