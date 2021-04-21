using System.Collections.Generic;

namespace Agent.antlr.ast
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */

    public class When : Node
    {
        private Comparable comparableL;
        private Comparable comparableR;
        private Comparison comparison;
        private ActionReference then;

        public new string GetNodeType()
        {
            return "When";
        }

        public new List<Node> GetChildren()
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

        public new Node AddChild(Node node)
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