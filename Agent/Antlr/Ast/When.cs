using System.Collections.Generic;

namespace Agent.antlr.ast
{
    public class When : Node
    {
        private Comparable _comparableL;
        private Comparable _comparableR;
        private Comparison _comparison;
        private ActionReference _then;

        public override string GetNodeType()
        {
            return "When";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();

            if (_comparableL != null)
                children.Add(_comparableL);
            if (_comparison != null)
                children.Add(_comparison);
            if (_comparableR != null)
                children.Add(_comparableR);
            if (this._then != null)
                children.Add(this._then);

            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node)
            {
                case Comparable comparable:
                    if (this._comparableL == null) 
                    {
                        this._comparableL = comparable;
                    }
                    else if (this._comparableR == null) 
                    {
                        this._comparableR = comparable;
                    }
                    else
                    {
                        this.body.Add(comparable);
                    }
                    break;
                case ActionReference action:
                    this._then = action;
                    break;
                case Comparison comparison:
                    this._comparison = comparison;
                    break;
                default:
                    this.body.Add(node);
                    break;
            }

            return this;
        }
        

        public Comparable GetComparableL()
        {
            return _comparableL;
        }

        public void SetComparableL(Comparable comparable)
        {
            _comparableL = comparable;
        }
        
        
        public Comparable GetComparableR()
        {
            return _comparableR;
        }

        public void SetComparableR(Comparable comparable)
        {
            _comparableR = comparable;
        }

        public Comparison GetComparison()
        {
            return _comparison;
        }

        public void SetComparison(Comparison comparison)
        {
            comparison = comparison;
        }
        
        public ActionReference GetThen()
        {
            return _then;
        }

        public void SetThen(ActionReference then)
        {
            then = then;
        }

    }
}