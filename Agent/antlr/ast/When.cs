using System.Collections.Generic;

namespace Agent.antlr.ast
{
    public class When : Node
    {
        private Comparable comparableL;
        private Comparable comparableR;
        private Comparison comparison;
        private ActionReference then;

        public override string GetNodeType()
        {
            return "When";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();

            if (comparableL != null)
                children.Add(comparableL);
            if (comparison != null)
                children.Add(comparison);
            if (comparableR != null)
                children.Add(comparableR);
            if (this.then != null)
                children.Add(this.then);

            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node)
            {
                case Comparable comparable:
                    if (this.comparableL == null) 
                    {
                        this.comparableL = comparable;
                    }
                    else if (this.comparableR == null) 
                    {
                        this.comparableR = comparable;
                    }
                    else
                    {
                        this.body.Add(comparable);
                    }
                    break;
                case ActionReference action:
                    this.then = action;
                    break;
                case Comparison comparison:
                    this.comparison = comparison;
                    break;
                default:
                    this.body.Add(node);
                    break;
            }

            return this;
        }
        

        public Comparable GetComparableL()
        {
            return comparableL;
        }

        public void SetComparableL(Comparable comparable)
        {
            comparableL = comparable;
        }
        
        
        public Comparable GetComparableR()
        {
            return comparableR;
        }

        public void SetComparableR(Comparable comparable)
        {
            comparableR = comparable;
        }

        public Comparison GetComparison()
        {
            return comparison;
        }

        public void SetComparison(Comparison comparison)
        {
            comparison = comparison;
        }
        
        public ActionReference GetThen()
        {
            return then;
        }

        public void SetThen(ActionReference then)
        {
            then = then;
        }

    }
}