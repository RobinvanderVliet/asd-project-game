using System.Collections.Generic;

namespace Agent.Antlr.Ast
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

            if (this._comparableL != null)
                children.Add(this._comparableL);
            if (this._comparison != null)
                children.Add(this._comparison);
            if (this._comparableR != null)
                children.Add(this._comparableR);
            if (this._then != null)
                children.Add(this._then);

            children.AddRange(this.body);
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
            return this._comparableL;
        }

        public void SetComparableL(Comparable comparable)
        {
            this._comparableL = comparable;
        }
        
        
        public Comparable GetComparableR()
        {
            return this._comparableR;
        }

        public void SetComparableR(Comparable comparable)
        {
            this._comparableR = comparable;
        }

        public Comparison GetComparison()
        {
            return this._comparison;
        }

        public void SetComparison(Comparison comparison)
        {
            comparison = comparison;
        }
        
        public ActionReference GetThen()
        {
            return this._then;
        }

        public void SetThen(ActionReference then)
        {
            then = then;
        }

    }
}