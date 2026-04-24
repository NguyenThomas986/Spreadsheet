using System;
using System.Collections.Generic;
using System.Text;

namespace LogicEngine
{
    internal abstract class Node
    {
        public abstract double Evaluate(Dictionary<string, double> variables);
    }
}
