using System;
using System.Collections.Generic;
using System.Text;

namespace LogicEngine
{
    internal class OperatorNode : Node
    {
        private readonly char op;
        private readonly Node left;
        private readonly Node right;

        public OperatorNode(char op, Node left, Node right)
        {
            this.op = op;
            this.left = left;
            this.right = right;
        }

        public override double Evaluate(Dictionary<string, double> variables)
        {
            double l = left.Evaluate(variables);
            double r = right.Evaluate(variables);

            switch (op)
            {
                case '+': return l + r;
                case '-': return l - r;
                case '*': return l * r;
                case '/': return l / r;
            }

            // no operations supported
            // todo: throw exception??
            return 0;
        }
    }
}
