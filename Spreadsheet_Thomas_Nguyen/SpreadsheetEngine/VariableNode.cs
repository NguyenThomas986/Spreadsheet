// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// represents a variable node in the expression tree.
    /// </summary>
    internal class VariableNode : Node
    {
        /// <summary>
        /// the name of the variable.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// the value of the variable.
        /// </summary>
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="name">the name of the variable.</param>
        /// <param name="value">the initial value of the variable.</param>
        public VariableNode(string name, double value = 0)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// sets the value of this variable.
        /// </summary>
        public double Value
        {
            set { this.value = value; }
        }

        /// <summary>
        /// evaluates and returns the value of this variable.
        /// </summary>
        /// <returns>the current value of this variable.</returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
