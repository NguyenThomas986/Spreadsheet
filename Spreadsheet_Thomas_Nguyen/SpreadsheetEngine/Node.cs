// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// the base class for all nodes in the expression tree.
    /// </summary>
    internal abstract class Node
    {
        /// <summary>
        /// evaluates and returns the value of this node.
        /// </summary>
        /// <returns>the evaluated value of this node.</returns>
        public abstract double Evaluate();
    }
}
