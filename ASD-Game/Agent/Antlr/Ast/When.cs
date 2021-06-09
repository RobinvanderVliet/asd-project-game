using System.Collections.Generic;

namespace ASD_Game.Agent.Antlr.Ast
{
    public class When : Node
    {
        public Comparable ComparableL;
        public Comparable ComparableR;
        public Comparison Comparison;
        public ActionReference Then;

        public override string GetNodeType()
        {
            return "When";
        }

        public override List<Node> GetChildren()
        {
            var children = new List<Node>();

            if (ComparableL != null)
                children.Add(ComparableL);
            if (Comparison != null)
                children.Add(Comparison);
            if (ComparableR != null)
                children.Add(ComparableR);
            if (Then != null)
                children.Add(Then);

            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node)
            {
                case Comparable comparable:
                    if (ComparableL == null) 
                    {
                        ComparableL = comparable;
                    }
                    else if (ComparableR == null) 
                    {
                        ComparableR = comparable;
                    }
                    else
                    {
                        body.Add(comparable);
                    }
                    break;
                case ActionReference action:
                    Then = action;
                    break;
                case Comparison comparison:
                    Comparison = comparison;
                    break;
                default:
                    body.Add(node);
                    break;
            }

            return this;
        }
    }
}