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
        private IComparable comparableL;
        private IComparable comparableR;
        private IComparison comparison;
        private IActionReference then;

        public new string GetNodeType()
        {
            return "When";
        }

        public new ArrayList GetChildren()
        {
            // TODO: Er worden nu drie waardes in de array gereseveerd voor null Als je door de three loopt, zijn dit alweer drie extra loops


            // var children = new ArrayList()
            // {
            //     this._comparableL,
            //     this._comparison,
            //     this._comparableR
            // };

            var children = new ArrayList();

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

        public new INode AddChild(INode node)
        {
            switch (node)
            {
                case IComparable comparable:
                    // TODO: Wanneer komt die hier in? Dit is altijd null, hij komt altijd in de else
                    
                    if (this.comparableL != null)
                    {
                        this.comparableL = comparable;
                    }
                    else if (this.comparableR != null)
                    {
                        this.comparableR = comparable;
                    }
                    else
                    {
                        this.body.Add(comparable);
                    }

                    break;
                case IActionReference action:
                    this.then = action;
                    break;
                case IComparison comparison:
                    this.comparison = comparison;
                    break;
                default:
                    this.body.Add(node);
                    break;
            }

            return this;
        }
    }
}