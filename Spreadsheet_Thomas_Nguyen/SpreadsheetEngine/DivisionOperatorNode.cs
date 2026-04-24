// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// evaluates the division of two child nodes.
    /// </summary>
    internal class DivisionOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionOperatorNode"/> class.
        /// initializes a new instance of the <see cref="DivisionOperatorNode"/> class.
        /// </summary>
        public DivisionOperatorNode()
            : base('/')
        {
        }

        /// <summary>
        /// gets the operator symbol used by the factory to identify this node.
        /// </summary>
        public static char Operation => '/';

        /// <summary>
        /// gets the precedence of this operator.
        /// </summary>
        public override int Precedence { get; } = 2;

        /// <summary>
        /// evaluates the division of the left and right child nodes.
        /// </summary>
        /// <returns>the quotient of the left and right child nodes.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() / this.Right.Evaluate();
        }
    }
}
