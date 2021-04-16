using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.antlr.ast
{
    interface IComparison : INode
    {
        public string ComparisonType { get; set; }
    }
}
