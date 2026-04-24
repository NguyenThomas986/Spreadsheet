// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SpreadsheetEngine;

/// <summary>
/// evaluates the addition of two child nodes.
/// </summary>
internal class AdditionOperatorNode : OperatorNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdditionOperatorNode"/> class.
    /// initializes a new instance of the <see cref="AdditionOperatorNode"/> class.
    /// </summary>
    public AdditionOperatorNode()
        : base('+')
    {
    }

    /// <summary>
    /// gets the operator symbol used by the factory to identify this node.
    /// </summary>
    public static char Operation => '+';

    /// <summary>
    /// gets the precedence of this operator.
    /// </summary>
    public override int Precedence { get; } = 1;

    /// <summary>
    /// evaluates the addition of the left and right child nodes.
    /// </summary>
    /// <returns>the sum of the left and right child nodes.</returns>
    public override double Evaluate()
    {
        return this.Left.Evaluate() + this.Right.Evaluate();
    }
}
