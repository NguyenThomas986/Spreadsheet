// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    /// <summary>
    /// Represents a command that can be executed and reversed.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the title of the command.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        void Execute();

        /// <summary>
        /// Returns a command that reverses this one.
        /// </summary>
        /// <returns>a command representing the reverse of this command.</returns>
        ICommand GetReverse();
    }
}
