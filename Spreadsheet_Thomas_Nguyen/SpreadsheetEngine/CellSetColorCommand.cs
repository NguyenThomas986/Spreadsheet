// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System.Collections.Generic;

    /// <summary>
    /// represents a command that sets the background color of a collection of cells.
    /// </summary>
    public class CellSetColorCommand : ICommand
    {
        /// <summary>
        /// the cells and their target colors.
        /// </summary>
        private readonly List<(Cell Cell, uint Color)> cells;

        /// <summary>
        /// Gets the title of the command.
        /// </summary>
        public string Title => "color change";

        /// <summary>
        /// Initializes a new instance of the <see cref="CellSetColorCommand"/> class.
        /// </summary>
        /// <param name="cells">the cells and their target colors.</param>
        public CellSetColorCommand(List<(Cell Cell, uint Color)> cells)
        {
            this.cells = cells;
        }

        /// <inheritdoc/>
        public void Execute()
        {
            foreach ((Cell cell, uint color) in this.cells)
            {
                cell.BGColor = color;
            }
        }

        /// <inheritdoc/>
        public ICommand GetReverse()
        {
            List<(Cell, uint)> previous = [];
            foreach ((Cell cell, uint _) in this.cells)
            {
                previous.Add((cell, cell.BGColor));
            }

            return new CellSetColorCommand(previous);
        }
    }
}
