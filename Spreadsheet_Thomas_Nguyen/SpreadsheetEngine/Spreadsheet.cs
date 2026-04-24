// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml;

    /// <summary>
    /// represents the spreadsheet and manages all cells.
    /// </summary>
    public class Spreadsheet
    {
        /// <summary>
        /// the 2d array of cells in the spreadsheet.
        /// </summary>
        private readonly Cell[,] cells = null!;

        /// <summary>
        /// the stack in which undo commands are stored.
        /// </summary>
        private Stack<ICommand> undoStack;

        /// <summary>
        /// the stack in which redo commands are stored.
        /// </summary>
        private Stack<ICommand> redoStack;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="myRow">the number of rows.</param>
        /// <param name="myCol">the number of columns.</param>
        public Spreadsheet(int myRow, int myCol)
        {
            this.undoStack = new Stack<ICommand>();
            this.redoStack = new Stack<ICommand>();
            this.RowCount = myRow;
            this.ColumnCount = myCol;
            this.cells = new SpreadSheetCell[myRow, myCol];

            for (int row = 0; row < myRow; row++)
            {
                for (int column = 0; column < myCol; column++)
                {
                    this.cells[row, column] = new SpreadSheetCell(row, column);

#pragma warning disable CS8622
                    this.cells[row, column].PropertyChanged += this.OnCellPropertyChanged;
#pragma warning restore CS8622
                }
            }
        }

        /// <summary>
        /// event fired when any cell property changes.
        /// </summary>
        public event PropertyChangedEventHandler? CellPropertyChanged;

        /// <summary>
        /// gets a value indicating whether there are any undo commands.
        /// </summary>
        public bool CanUndo => this.undoStack.Count > 0;

        /// <summary>
        /// gets a value indicating whether there are any redo commands.
        /// </summary>
        public bool CanRedo => this.redoStack.Count > 0;

        /// <summary>
        /// gets the title of the top undo command.
        /// </summary>
        public string UndoTitle => this.CanUndo ? this.undoStack.Peek().Title : string.Empty;

        /// <summary>
        /// gets the title of the top redo command.
        /// </summary>
        public string RedoTitle => this.CanRedo ? this.redoStack.Peek().Title : string.Empty;

        /// <summary>
        /// gets the number of rows in the spreadsheet.
        /// </summary>
        private int RowCount { get; }

        /// <summary>
        /// gets the number of columns in the spreadsheet.
        /// </summary>
        private int ColumnCount { get; }

        /// <summary>
        /// gets the cell at the given row and column.
        /// </summary>
        /// <param name="row">the row index.</param>
        /// <param name="column">the column index.</param>
        /// <returns>the cell at the given position.</returns>
        public Cell GetCell(int row, int column)
        {
            if (row < 0 || row >= this.RowCount || column < 0 || column >= this.ColumnCount)
            {
                throw new IndexOutOfRangeException("cell index is out of bounds.");
            }

            return this.cells[row, column];
        }

        /// <summary>
        /// pushes a command to the undo stack.
        /// </summary>
        /// <param name="command">the command to push.</param>
        public void AddUndo(ICommand command)
        {
            this.undoStack.Push(command);
            this.redoStack.Clear();
        }

        /// <summary>
        /// executes the top undo command and pushes the prior state to the redo stack.
        /// </summary>
        public void Undo()
        {
            if (this.undoStack.Count > 0)
            {
                ICommand command = this.undoStack.Pop();
                this.redoStack.Push(command.GetReverse());
                command.Execute();
            }
        }

        /// <summary>
        /// executes the top redo command and pushes the prior state to the undo stack.
        /// </summary>
        public void Redo()
        {
            if (this.redoStack.Count > 0)
            {
                ICommand command = this.redoStack.Pop();
                this.undoStack.Push(command.GetReverse());
                command.Execute();
            }
        }

        /// <summary>
        /// clears all cells in the spreadsheet back to their default values.
        /// </summary>
        public void Clear()
        {
            foreach (Cell cell in this.cells)
            {
                cell.Text = cell.DefaultText;
                cell.BGColor = cell.DefaultColor;
            }

            this.undoStack.Clear();
            this.redoStack.Clear();
        }

        /// <summary>
        /// saves all relevant information about the spreadsheet to an xml doc.
        /// </summary>
        /// <param name="fileStream">a pointer to the file to write to.</param>
        public void Save(StreamWriter fileStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "    ",
                NewLineOnAttributes = true,
            };

            XmlWriter writer = XmlWriter.Create(fileStream, settings);

            writer.WriteStartElement("spreadsheet"); // start spreadsheet element

            foreach (Cell cell in this.cells)
            {
                // only write cells with changed values
                if (cell.Text != cell.DefaultText || cell.BGColor != cell.DefaultColor)
                {
                    writer.WriteStartElement("cell"); // start cell element

                    char letter = (char)(cell.MyColumn + 65);
                    string number = (cell.MyRow + 1).ToString();

                    writer.WriteStartElement("name"); // start name element
                    writer.WriteString(letter + number);
                    writer.WriteEndElement(); // end name element

                    writer.WriteStartElement("text"); // start text element
                    writer.WriteString(cell.Text);
                    writer.WriteEndElement(); // end text element

                    writer.WriteStartElement("color"); // start color element
                    writer.WriteString(cell.BGColor.ToString("X8"));
                    writer.WriteEndElement(); // end color element

                    writer.WriteEndElement(); // end cell element
                }
            }

            writer.WriteEndElement(); // end spreadsheet element
            writer.Close(); // flush and close the writer
        }

        /// <summary>
        /// loads spreadsheet data from an xml file, replacing the current state.
        /// </summary>
        /// <param name="fileStream">a pointer to the file to read from.</param>
        public void Load(StreamReader fileStream)
        {
            this.Clear(); // clear current state before loading new data

            XmlReader reader = XmlReader.Create(fileStream, new XmlReaderSettings());

            // track which element we're currently inside and the current cell name
            string cellName = string.Empty;
            string currentElement = string.Empty;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    // update the current element tag as we move through the xml
                    currentElement = reader.Name;
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    if (currentElement == "name")
                    {
                        // store the cell name (e.g. "A1") to use when reading sibling elements
                        cellName = reader.Value;
                    }
                    else if (currentElement == "text" && cellName != string.Empty)
                    {
                        // parse column and row from the cell name and set the text
                        int col = cellName[0] - 65;
                        int row = int.Parse(cellName[1..]) - 1;
                        this.cells[row, col].Text = reader.Value;
                    }
                    else if (currentElement == "color" && cellName != string.Empty)
                    {
                        // parse the hex color string back into a uint and apply it
                        int col = cellName[0] - 65;
                        int row = int.Parse(cellName[1..]) - 1;
                        this.cells[row, col].BGColor = uint.Parse(reader.Value, System.Globalization.NumberStyles.HexNumber);
                    }
                }
            }
        }

        /// <summary>
        /// called when any cell's property changes.
        /// </summary>
        /// <param name="sender">the cell that changed.</param>
        /// <param name="e">information about the change.</param>
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is Cell cell && e.PropertyName == nameof(Cell.Text))
            {
                if (cell.Text.StartsWith("="))
                {
                    // remove '=' to get the expression
                    string expression = cell.Text.Substring(1);

                    try
                    {
                        // build expression tree from formula
                        ExpressionTree tree = new ExpressionTree(expression);

                        // collect referenced cells and set variables
                        List<Cell> referenced = new List<Cell>();
                        foreach (string varName in tree.GetVariableNames())
                        {
                            // throws InvalidVariableException if format is bad, returns false if out of bounds.
                            if (!this.IsValidCellName(varName))
                            {
                                throw new InvalidVariableException();
                            }

                            int col = varName[0] - 'A';
                            int row = int.Parse(varName.Substring(1)) - 1;

                            Cell referencedCell = this.GetCell(row, col);
                            referenced.Add(referencedCell);

                            if (!double.TryParse(referencedCell.Value, out double value))
                            {
                                value = 0; // default to 0 if not a number
                            }

                            tree.SetVariable(varName, value);
                        }

                        // subscribe to referenced cells so this cell updates when they change
                        cell.SetReferencedCells(referenced);

                        // evaluate and set result
                        double result = tree.Evaluate();
                        cell.SetValue(result.ToString());
                    }
                    catch (InvalidVariableException)
                    {
                        cell.SetValue("invalid variable");
                    }
                    catch (SelfReferenceException)
                    {
                        cell.SetValue("self reference");
                    }
                    catch (CircularReferenceException)
                    {
                        cell.SetValue("circular reference");
                    }
                    catch (NotSupportedException)
                    {
                        cell.SetValue("unsupported operator");
                    }
                    catch (Exception)
                    {
                        cell.SetValue("unknown error");
                    }
                }
                else
                {
                    // if not a formula, clear references and set value equal to text
                    cell.SetReferencedCells(new List<Cell>());
                    cell.SetValue(cell.Text);
                }
            }

            this.CellPropertyChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// determines if a variable name represents a valid cell reference.
        /// </summary>
        /// <param name="name">the variable name to validate.</param>
        /// <returns>true if the name maps to a cell within the grid; false otherwise.</returns>
        /// <exception cref="InvalidVariableException">thrown if the name is not a valid cell name format.</exception>
        private bool IsValidCellName(string name)
        {
            char colChar = name[0];
            string rowStr = name.Substring(1);

            int rowIndex = -1;
            try
            {
                rowIndex = int.Parse(rowStr) - 1;
            }
            catch
            {
                throw new InvalidVariableException();
            }

            return (colChar >= 'A' && colChar < 'A' + this.ColumnCount) && (rowIndex >= 0 && rowIndex < this.RowCount);
        }

        /// <summary>
        /// the private concrete implementation of cell.
        /// </summary>
#pragma warning disable SA1024 // Colons Should Be Spaced Correctly
        private class SpreadSheetCell(int row, int col): Cell(row, col)
#pragma warning restore SA1024 // Colons Should Be Spaced Correctly
        {
        }
    }
}
