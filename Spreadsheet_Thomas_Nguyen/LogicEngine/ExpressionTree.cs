namespace LogicEngine
{
    public class ExpressionTree
    {
        private readonly Node root;
        private readonly Dictionary<string, double> variables = new ();
        public ExpressionTree(string expression)
        {
            char op = FindOperator(expression);
            root = Compile(expression, op);
        }

        public void SetVariable(string variableName, double variableValue)
        {
            variables[variableName] = variableValue;
        }

        public double Evaluate()
        {
            return root.Evaluate(variables);
        }

        private static Node Compile(string expression, char op)
        {
            // check if expression is null first
            if(string.IsNullOrEmpty(expression))
            {
                return new ConstantNode(0);
            }
            // iterate from back to front
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                // if we found the operator we're looking for, split here
                if (op == expression[i])
                {
                    // build an operator node with left and right sub-expressions
                    Node left = Compile(expression.Substring(0, i), op);
                    Node right = Compile(expression.Substring(i + 1), op);
                    return new OperatorNode(expression[i], left, right);
                }
            }

            // no operator found — it's a leaf node (variable or constant)
            return CreateExpressionTreeNode(expression);
        }

        private static char FindOperator(string expression)
        {
            // loops through expression to find what kind of operator it is
            foreach (char op in expression)
            {
                if (op == '+' || op == '-' || op == '*' || op == '/')
                    return op;
            }
            // operator not supported
            // todo: throw exception
            return '\0';
        }

        private static Node CreateExpressionTreeNode(string expression)
        {
            // if the expression starts with a letter it's a variable 
            // otherwise it must be a number
            if (char.IsLetter(expression[0]))
                return new VariableNode(expression);

            return new ConstantNode(double.Parse(expression));
        }
    }
}
