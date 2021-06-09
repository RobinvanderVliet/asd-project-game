using System.Collections.Generic;

namespace ASD_Game.Agent.Antlr.Ast
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
            if (_then != null)
                children.Add(_then);

            children.AddRange(body);
            return children;
        }

        public override Node AddChild(Node node)
        {
            switch (node)
            {
                case Comparable comparable:
                    if (_comparableL == null)
                    {
                        _comparableL = comparable;
                    }
                    else if (_comparableR == null)
                    {
                        _comparableR = comparable;
                    }
                    else
                    {
                        body.Add(comparable);
                    }
                    break;
                case ActionReference action:
                    _then = action;
                    break;
                case Comparison comparison:
                    _comparison = comparison;
                    break;
                default:
                    body.Add(node);
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
            _comparison = comparison;
        }

        public ActionReference GetThen()
        {
            return _then;
        }

        public void SetThen(ActionReference then)
        {
            _then = then;
        }

    }
}