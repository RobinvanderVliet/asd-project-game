namespace Agent.antlr.ast
{
    public class Rule : Node, IRule
    {
        public string SettingName { get; set; }
        public string Value { get; set; }
    }
}