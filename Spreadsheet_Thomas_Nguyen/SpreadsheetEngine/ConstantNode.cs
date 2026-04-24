// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// represents a constant numeric value node in the expression tree.
    /// </summary>
    internal class ConstantNode : Node
    {
        /// <summary>
        /// the constant value of this node.
        /// </summary>
        private readonly double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// </summary>
        /// <param name="value">the constant value to store.</param>
        public ConstantNode(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// evaluates and returns the constant value.
        /// </summary>
        /// <returns>the constant value of this node.</returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
