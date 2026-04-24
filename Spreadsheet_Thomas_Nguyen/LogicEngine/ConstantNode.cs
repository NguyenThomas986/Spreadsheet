using System;
using System.Collections.Generic;
using System.Text;

namespace LogicEngine
{
    internal class ConstantNode : Node
    {
        private readonly double value;
        public ConstantNode(double value)
        {
            this.value = value;
        }
        public override double Evaluate(Dictionary<string, double> variables)
        {
            return value;
        }
    }
}
