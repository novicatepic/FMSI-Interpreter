using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class IntNode : INode
    {
        //public int Value { get; set; }

        public IntNode(int value)
        {
            Value = value;
        }

    }
}
