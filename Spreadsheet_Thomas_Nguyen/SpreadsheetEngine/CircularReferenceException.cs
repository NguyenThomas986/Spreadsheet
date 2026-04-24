// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;

    /// <summary>
    /// Exception thrown when a cell formula creates a circular reference chain.
    /// </summary>
    public class CircularReferenceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircularReferenceException"/> class.
        /// </summary>
        public CircularReferenceException()
            : base("Circular Reference")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularReferenceException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CircularReferenceException(string message)
            : base(message)
        {
        }
    }
}
