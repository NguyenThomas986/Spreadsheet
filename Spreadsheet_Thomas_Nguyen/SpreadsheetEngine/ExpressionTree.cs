// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SpreadsheetEngine
{
    /// <summary>
    /// represents an expression tree that can evaluate mathematical expressions.
    /// </summary>
    public class ExpressionTree
    {
        /// <summary>
        /// the root node of the expression tree.
        /// </summary>
        private readonly Node root;

        /// <summary>
        /// maps variable names to their corresponding variable nodes.
        /// </summary>
        private readonly Dictionary<string, VariableNode> variableNodes = new Dictionary<string, VariableNode>();

        /// <summary>
        /// the singleton factory used to create operator nodes.
        /// </summary>
        private readonly OperatorNodeFactory factory = OperatorNodeFactory.GetInstance();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">the expression to parse and build into a tree.</param>
        public ExpressionTree(string expression)
        {
            this.root = this.BuildTree(expression);
        }

        /// <summary>
        /// sets the value of a variable in the expression tree.
        /// </summary>
        /// <param name="variableName">the name of the variable to set.</param>
        /// <param name="variableValue">the value to assign to the variable.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            if (this.variableNodes.ContainsKey(variableName))
            {
                this.variableNodes[variableName].Value = variableValue;
            }
        }

        /// <summary>
        /// evaluates the expression tree and returns the result.
        /// </summary>
        /// <returns>the result of the expression.</returns>
        public double Evaluate()
        {
            return this.root.Evaluate();
        }

        /// <summary>
        /// returns the names of all variables in the expression tree.
        /// </summary>
        /// <returns>an enumerable of variable name strings.</returns>
        public IEnumerable<string> GetVariableNames()
        {
            return this.variableNodes.Keys;
        }

        /// <summary>
        /// determines if a character is a parenthesis.
        /// </summary>
        /// <param name="c">the character to check.</param>
        /// <returns>true if the character is a parenthesis; false otherwise.</returns>
        private static bool IsParenthesis(char c)
        {
            return c == '(' || c == ')';
        }

        /// <summary>
        /// converts an infix expression string to a postfix token list.
        /// </summary>
        /// <param name="expression">the infix expression to convert.</param>
        /// <returns>a list of tokens in postfix order.</returns>
        private List<string> ToPostfix(string expression)
        {
            List<string> postfixList = new List<string>();
            Stack<char> operatorStack = new Stack<char>();
            int i = 0;

            while (i < expression.Length)
            {
                char currentChar = expression[i];

                if (char.IsWhiteSpace(currentChar))
                {
                    // skip whitespace
                    i++;
                }
                else if (char.IsDigit(currentChar) || currentChar == '.')
                {
                    // parse a numeric token including decimals
                    string number = string.Empty;
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        number += expression[i++];
                    }

                    postfixList.Add(number);
                }
                else if (char.IsLetter(currentChar))
                {
                    // parse a variable or cell reference like A1, B12
                    string name = string.Empty;
                    while (i < expression.Length && char.IsLetterOrDigit(expression[i]))
                    {
                        name += expression[i++];
                    }

                    postfixList.Add(name);
                }
                else if (currentChar == '(')
                {
                    operatorStack.Push(currentChar);
                    i++;
                }
                else if (currentChar == ')')
                {
                    // pop until matching opening parenthesis
                    while (operatorStack.Count > 0 && operatorStack.Peek() != '(')
                    {
                        postfixList.Add(operatorStack.Pop().ToString());
                    }

                    if (operatorStack.Count > 0)
                    {
                        operatorStack.Pop();
                    }

                    i++;
                }
                else if (this.factory.IsOperator(currentChar))
                {
                    // pop higher or equal precedence operators before pushing
                    while (operatorStack.Count > 0 && !IsParenthesis(operatorStack.Peek()) &&
                           this.factory.CheckPrecedence(currentChar) <= this.factory.CheckPrecedence(operatorStack.Peek()))
                    {
                        postfixList.Add(operatorStack.Pop().ToString());
                    }

                    operatorStack.Push(currentChar);
                    i++;
                }
                else
                {
                    // throw for any unrecognized character
                    throw new NotSupportedException($"unsupported operator: {currentChar}");
                }
            }

            // pop any remaining operators
            while (operatorStack.Count > 0)
            {
                postfixList.Add(operatorStack.Pop().ToString());
            }

            return postfixList;
        }

        /// <summary>
        /// builds the expression tree from an infix expression string.
        /// </summary>
        /// <param name="expression">the infix expression to build the tree from.</param>
        /// <returns>the root node of the expression tree.</returns>
        private Node BuildTree(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return new ConstantNode(0);
            }

            Stack<Node> nodeStack = new Stack<Node>();

            foreach (string token in this.ToPostfix(expression))
            {
                if (double.TryParse(token, out double number))
                {
                    nodeStack.Push(new ConstantNode(number));
                }
                else if (this.factory.IsOperator(token))
                {
                    // use the factory to create the correct operator node
                    OperatorNode operatorNode = OperatorNodeFactory.CreateOperatorNode(token[0]);
                    operatorNode.Right = nodeStack.Pop();
                    operatorNode.Left = nodeStack.Pop();
                    nodeStack.Push(operatorNode);
                }
                else
                {
                    VariableNode variableNode = new VariableNode(token);
                    this.variableNodes[token] = variableNode;
                    nodeStack.Push(variableNode);
                }
            }

            return nodeStack.Pop();
        }
    }
}
