namespace Creature.Creature.StateMachine
{
    public class Setting
    {
        public string Property { get; }
        public string Value { get; }

        public Setting(string property, string value)
        {
            Property = property;
            Value = value;
        }
    }
}