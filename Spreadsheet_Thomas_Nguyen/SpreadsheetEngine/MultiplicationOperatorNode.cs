// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// evaluates the multiplication of two child nodes.
    /// </summary>
    internal class MultiplicationOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplicationOperatorNode"/> class.
        /// </summary>
        public MultiplicationOperatorNode()
            : base('*')
        {
        }

        /// <summary>
        /// gets the operator symbol used by the factory to identify this node.
        /// </summary>
        public static char Operation => '*';

        /// <summary>
        /// gets the precedence of this operator.
        /// </summary>
        public override int Precedence { get; } = 2;

        /// <summary>
        /// evaluates the multiplication of the left and right child nodes.
        /// </summary>
        /// <returns>the product of the left and right child nodes.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() * this.Right.Evaluate();
        }
    }
}
