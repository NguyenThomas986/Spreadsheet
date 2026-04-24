// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;

    /// <summary>
    /// exception thrown when a formula contains a variable that is not a valid cell name.
    /// </summary>
    public class InvalidVariableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidVariableException"/> class.
        /// initializes a new instance of the <see cref="InvalidVariableException"/> class.
        /// </summary>
        public InvalidVariableException()
            : base("invalid variable.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidVariableException"/> class.
        /// initializes a new instance of the <see cref="InvalidVariableException"/> class.
        /// </summary>
        /// <param name="message">the message that describes the error.</param>
        public InvalidVariableException(string message)
            : base(message)
        {
        }
    }
}
