using Agent.antlr.ast.interfaces;
using System.Collections;

namespace Agent.antlr.ast.implementation
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    
    public class When : Node, IWhen
    {
        private IComparable _comparableL;
        private IComparable _comparableR;
        private IComparison _comparison;
        private IActionReference _then;
        
        public new string GetNodeType()
        {
            return "When";
        }

        public new ArrayList GetChildren()
        {
            var children = new ArrayList() 
            {
                this._comparableL,
                this._comparison,
                this._comparableR,
                this._then
            };
            children.AddRange(body);
            return children;
        }

        public new INode AddChild(INode node)
        {
            switch (node) {
                case IComparable comparable:
                    if (this._comparableL == null) {
                        this._comparableL = comparable;
                    }
                    else if (this._comparableR == null) {
                        this._comparableR = comparable;
                    }
                    else {
                        this.body.Add(comparable);
                    }
                    break;
                case IActionReference action:
                    this._then = action;
                    break;
                case IComparison comparison:
                    this._comparison = comparison;
                    break;
                default:
                    this.body.Add(node);
                    break;
            }

            return this;
        }
        

        public IComparable GetComparableL()
        {
            return _comparableL;
        }

        public void SetComparableL(IComparable comparable)
        {
            _comparableL = comparable;
        }
        
        
        public IComparable GetComparableR()
        {
            return _comparableR;
        }

        public void SetComparableR(IComparable comparable)
        {
            _comparableR = comparable;
        }

        public IComparison GetComparison()
        {
            return _comparison;
        }

        public void SetComparison(IComparison comparison)
        {
            _comparison = comparison;
        }
        
        public IActionReference GetThen()
        {
            return _then;
        }

        public void SetThen(IActionReference then)
        {
            _then = then;
        }

    }
}