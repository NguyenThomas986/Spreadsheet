// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    /// <summary>
    /// represents a command that sets the text of a cell.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CellSetTextCommand"/> class.
    /// </remarks>
    /// <param name="cell">the cell to modify.</param>
    /// <param name="text">the text to set.</param>
#pragma warning disable SA1024 // Colons Should Be Spaced Correctly
    public class CellSetTextCommand(Cell cell, string text): ICommand
#pragma warning restore SA1024 // Colons Should Be Spaced Correctly
    {
        /// <summary>
        /// the cell to modify.
        /// </summary>
        private Cell cell = cell;

        /// <summary>
        /// the text to set.
        /// </summary>
        private string text = text;

        /// <summary>
        /// Gets the title of the command.
        /// </summary>
        public string Title => "text change";

        /// <inheritdoc/>
        public void Execute()
        {
            this.cell.Text = this.text;
        }

        /// <inheritdoc/>
        public ICommand GetReverse()
        {
            return new CellSetTextCommand(this.cell, this.cell.Text);
        }
    }
}
