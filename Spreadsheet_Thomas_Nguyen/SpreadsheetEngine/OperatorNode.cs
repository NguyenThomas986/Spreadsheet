// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// the base class for all operator nodes in the expression tree.
    /// </summary>
    internal abstract class OperatorNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNode"/> class.
        /// </summary>
        /// <param name="operatorchar">the operator character this node represents.</param>
        public OperatorNode(char operatorchar)
        {
            this.Operatorvalue = operatorchar;
            this.Left = null!;
            this.Right = null!;
        }

        /// <summary>
        /// gets or sets the left child node.
        /// </summary>
        public Node Left { get; set; }

        /// <summary>
        /// gets or sets the right child node.
        /// </summary>
        public Node Right { get; set; }

        /// <summary>
        /// gets or sets the operator character of this node.
        /// </summary>
        public char Operatorvalue { get; set; }

        /// <summary>
        /// gets the precedence of this operator.
        /// </summary>
        public abstract int Precedence { get; }

        /// <summary>
        /// evaluates and returns the result of applying this operator to its child nodes.
        /// </summary>
        /// <returns>the evaluated result of this operator node.</returns>
        public abstract override double Evaluate();
    }
}
