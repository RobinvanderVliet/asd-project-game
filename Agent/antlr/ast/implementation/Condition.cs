using System.Collections;

namespace Agent.antlr.ast
{
    /*
     * 
     * @author Abdul     
    */
    public class Condition : Node, ICondition
    {
        private When _whenClause;
        private Otherwise _otherwiseClause;
        private ArrayList _body;

        public Condition()
        {
            // _whenClause = new When();
            // _otherwiseClause = new Otherwise();
            _body = new ArrayList();
        }

        public new string GetNodeType()
        {
            return "Condition";
        }

        public new ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(_whenClause);
            children.AddRange(_body);
            if (_otherwiseClause != null)
                children.Add(_otherwiseClause);
            
            return _body;
        }

        public new Node AddChild(INode node)
        {
            switch (node)
            {
                case When whenClause:
                    _whenClause = whenClause;
                    break;
                case Otherwise otherwiseClause:
                    _otherwiseClause = otherwiseClause;
                    break;
                default:
                    _body.Add(node);
                    break;
            }

            return this;
        }

        public When GetWhenClause()
        {
            return _whenClause;
        }

        public Otherwise GetOtherWiseClause()
        {
            return _otherwiseClause;
        }
    }
}