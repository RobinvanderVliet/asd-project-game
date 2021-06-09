namespace WorldGeneration.StateMachine.CustomRuleSet
{
    public class RuleSet
    {
        private string _setting;
        private string _action;
        private string _comparable;
        private string _threshold;
        private string _comparison;
        private string _comparisonTrue;
        private string _comparisonFalse;

        public string Setting
        {
            get => _setting;
            set => _setting = value;
        }

        public string Action
        {
            get => _action;
            set => _action = value;
        }

        public string Comparable
        {
            get => _comparable;
            set => _comparable = value;
        }

        public string Threshold
        {
            get => _threshold;
            set => _threshold = value;
        }

        public string Comparison
        {
            get => _comparison;
            set => _comparison = value;
        }

        public string ComparisonTrue
        {
            get => _comparisonTrue;
            set => _comparisonTrue = value;
        }

        public string ComparisonFalse
        {
            get => _comparisonFalse;
            set => _comparisonFalse = value;
        }

        public RuleSet()
        {
            
        }
    }
}