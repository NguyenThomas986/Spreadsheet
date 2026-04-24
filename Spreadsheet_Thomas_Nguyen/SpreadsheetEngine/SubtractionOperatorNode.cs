// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// evaluates the subtraction of two child nodes.
    /// </summary>
    internal class SubtractionOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractionOperatorNode"/> class.
        /// </summary>
        public SubtractionOperatorNode()
            : base('-')
        {
        }

        /// <summary>
        /// gets the operator symbol used by the factory to identify this node.
        /// </summary>
        public static char Operation => '-';

        /// <summary>
        /// gets the precedence of this operator.
        /// </summary>
        public override int Precedence { get; } = 1;

        /// <summary>
        /// evaluates the subtraction of the left and right child nodes.
        /// </summary>
        /// <returns>the difference of the left and right child nodes.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() - this.Right.Evaluate();
        }
    }
}
