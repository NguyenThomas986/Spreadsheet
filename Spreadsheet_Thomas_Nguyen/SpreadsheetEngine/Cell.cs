// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SpreadsheetEngine
{
    using System.ComponentModel;

    /// <summary>
    /// the base class for all cells within the spreadsheet.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        private List<Cell> referencedCells = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="row">the row index.</param>
        /// <param name="col">the column index.</param>
        protected Cell(int row, int col)
        {
            this.MyRow = row;
            this.MyColumn = col;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// gets or sets the row index of this cell.
        /// </summary>
        public int MyRow { get; set; }

        /// <summary>
        /// gets or sets the column index of this cell.
        /// </summary>
        public int MyColumn { get; set; }

        /// <summary>
        /// gets or sets the background color of the cell.
        /// </summary>
        public uint BGColor
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.BGColor)));
                }
            }
        }

= 0xFFFFFFFF;

        /// <summary>
        /// gets or sets the text of this cell.
        /// </summary>
        public string Text
        {
            get => field ?? string.Empty;
            set
            {
                field = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Text)));
            }
        }

= string.Empty;

        /// <summary>
        /// gets the evaluated value of this cell.
        /// </summary>
        public string? Value { get; private set; }

        /// <summary>
        /// gets the default color for this cell.
        /// </summary>
        public uint DefaultColor { get; } = 0xFFFFFFFF;

        /// <summary>
        /// gets the default text for this cell.
        /// </summary>
        public string DefaultText { get; } = string.Empty;

        /// <summary>
        /// sets the value of this cell and notifies listeners.
        /// </summary>
        /// <param name="v">the value to set.</param>
        public void SetValue(string v)
        {
            this.Value = v;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
        }

        /// <summary>
        /// updates which cells this cell depends on, checking for circular references.
        /// </summary>
        /// <param name="cells">the cells this cell now references.</param>
        /// <exception cref="InvalidOperationException">thrown if a circular reference is detected.</exception>
        public void SetReferencedCells(List<Cell> cells)
        {
            // unsubscribe from old dependencies first.
            foreach (Cell referenced in this.referencedCells)
            {
                referenced.PropertyChanged -= this.OnReferencedCellChanged;
            }

            this.referencedCells = cells;

            // check for circular references before subscribing.
            foreach (Cell referenced in this.referencedCells)
            {
                // catch self-reference before checking for cycles.
                if (referenced == this)
                {
                    this.referencedCells = new ();
                    throw new SelfReferenceException();
                }

                if (this.CreatesCircularReference(referenced))
                {
                    // roll back to empty so the cell is left in a clean state.
                    this.referencedCells = new ();
                    throw new CircularReferenceException();
                }
            }

            // no cycle found — safe to subscribe.
            foreach (Cell referenced in this.referencedCells)
            {
                referenced.PropertyChanged += this.OnReferencedCellChanged;
            }
        }

        // walks the dependency graph from 'candidate' downward to see if
        // we'd end up back at 'this', which would mean a cycle exists.
        private bool CreatesCircularReference(Cell candidate)
        {
            if (candidate == this)
            {
                return true;
            }

            foreach (Cell dep in candidate.referencedCells)
            {
                if (this.CreatesCircularReference(dep))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnReferencedCellChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.Value))
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Text)));
            }
        }
    }
}
