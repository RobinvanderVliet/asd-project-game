namespace ASD_Game.Agent.Mapper
{
    public class Setting
    {
        private string _property;
        public string Property { get => _property; }
        private string _value;
        public string Value { get => _value; }

        public Setting(string property, string value)
        {
            _property = property;
            _value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj is Setting)
            {
                return ((Setting) obj)._property.Equals(_property) && ((Setting) obj)._value.Equals(_value);
            }
            return false;
        }
    }
}