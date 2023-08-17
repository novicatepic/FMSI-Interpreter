using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class BinaryOperation : INode
    {
        public INode Left { get; }
        public string Operation { get; }
        public INode Right { get;}
        public BinaryOperation(INode left, INode right, string Operation)
        {
            this.Left = left;
            this.Right = right;
            this.Operation = Operation;
            if (Operation == "+")
            {
                this.Value = Left.Value + Right.Value;
            }
            if (Operation == "*")
            {
                this.Value = Left.Value * Right.Value;
            }
            if(Operation == "-")
            {
                 this.Value = Left.Value - Right.Value;
            }
            if(Operation == "/")
            {
                this.Value = Left.Value / Right.Value;
            }
        }
        /*public int Value { get
            {
                if(Operation == "+")
                {
                    return Left.Value + Right.Value;
                }
                if (Operation == "*")
                {
                    return Left.Value * Right.Value;
                }
                if(Operation == "-")
                {
                    return Left.Value - Right.Value;
                }
                if(Operation == "/")
                {
                    return Left.Value / Right.Value;
                }
                throw new Exception();
            } 
        }*/
    }
}
