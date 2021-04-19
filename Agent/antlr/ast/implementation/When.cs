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
                this._comparableR
            };
            if (this._then != null) {
                children.Add(this._then);
                
            }
            children.AddRange(body);
            return children;
        }

        public new INode AddChild(INode node)
        {
            switch (node) {
                case IComparable comparable:
                    if (this._comparableL != null) {
                        this._comparableL = comparable;
                    }
                    else if (this._comparableR != null) {
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
    }
}