// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;

    /// <summary>
    /// Exception thrown when a cell formula references itself.
    /// </summary>
    public class SelfReferenceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelfReferenceException"/> class.
        /// </summary>
        public SelfReferenceException()
            : base("Cell contains a self-reference.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfReferenceException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SelfReferenceException(string message)
            : base(message)
        {
        }
    }
}
