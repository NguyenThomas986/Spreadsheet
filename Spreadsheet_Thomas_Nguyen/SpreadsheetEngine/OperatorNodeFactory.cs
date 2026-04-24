// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// responsible for creating an operator node of the correct type.
    /// </summary>
    internal class OperatorNodeFactory
    {
        /// <summary>
        /// the singleton instance of the factory.
        /// </summary>
        private static OperatorNodeFactory? factory;

        /// <summary>
        /// maps operator characters to their corresponding node types.
        /// </summary>
        private Dictionary<char, Type> operators = new Dictionary<char, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
        /// </summary>
        private OperatorNodeFactory()
        {
            Type operatorNodeType = typeof(OperatorNode);

            // search the same assembly that OperatorNode is defined in
            IEnumerable<Type> operatorTypes = operatorNodeType.Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(operatorNodeType));

            foreach (Type type in operatorTypes)
            {
                PropertyInfo? prop = type.GetProperty("Operation");
                if (prop != null)
                {
                    object? value = prop.GetValue(null);
                    if (value is char op)
                    {
                        this.operators.Add(op, type);
                    }
                }
            }
        }

        /// <summary>
        /// returns the single instance of the factory, creating it if it does not exist.
        /// </summary>
        /// <returns>the only instance of this class.</returns>
        public static OperatorNodeFactory GetInstance()
        {
            if (factory == null)
            {
                factory = new OperatorNodeFactory();
            }

            return factory;
        }

        /// <summary>
        /// creates and returns the correct operator node for the given character.
        /// </summary>
        /// <param name="op">the operator character to look up.</param>
        /// <returns>an operator node that represents the operation.</returns>
        /// <exception cref="ArgumentException">thrown when no matching operator is found.</exception>
        public static OperatorNode CreateOperatorNode(char op)
        {
            OperatorNodeFactory instance = GetInstance();
            if (instance.operators.TryGetValue(op, out Type? type))
            {
                object? node = Activator.CreateInstance(type);
                if (node is OperatorNode operatorNode)
                {
                    return operatorNode;
                }
            }

            throw new ArgumentException($"Invalid operator: {op}");
        }

        /// <summary>
        /// returns the precedence of the given operator.
        /// </summary>
        /// <param name="op">the operator character to look up.</param>
        /// <returns>the precedence of the operator, or -1 if not found.</returns>
        public int CheckPrecedence(char op)
        {
            if (this.operators.TryGetValue(op, out Type? type))
            {
                object? instance = Activator.CreateInstance(type);
                PropertyInfo? prop = type.GetProperty("Precedence");
                if (prop?.GetValue(instance) is int precedence)
                {
                    return precedence;
                }
            }

            return -1;
        }

        /// <summary>
        /// determines if a character represents a known operator.
        /// </summary>
        /// <param name="op">the character to check.</param>
        /// <returns>true if the character is a registered operator; false otherwise.</returns>
        public bool IsOperator(char op)
        {
            return this.operators.ContainsKey(op);
        }

        /// <summary>
        /// determines if a string represents a known operator.
        /// </summary>
        /// <param name="op">the string to check.</param>
        /// <returns>true if the string is a registered operator; false otherwise.</returns>
        public bool IsOperator(string op)
        {
            return op.Length == 1 && this.operators.ContainsKey(op[0]);
        }
    }
}
