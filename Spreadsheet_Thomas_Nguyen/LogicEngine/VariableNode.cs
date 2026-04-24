using System;
using System.Collections.Generic;
using System.Text;

namespace LogicEngine
{
    internal class VariableNode : Node
    {
        private readonly string name;
        public VariableNode(string name)
        {
            this.name = name;
        }

        public override double Evaluate(Dictionary<string, double> variables)
        {
            if (variables.ContainsKey(name))
                return variables[name];

            return 0;
        }
    }
}
