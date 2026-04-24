// Copyright (c) Thomas Nguyen 11888002. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Spreadsheet_Thomas_Nguyen
{
    using SpreadsheetEngine;
    using System.ComponentModel;
    using System.Xml;

    /// <summary>
    /// Represents the main spreadsheet user interface.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly Spreadsheet spreadsheet = new(50, 26);

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.Load += this.Form1_Load;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.spreadsheet.CellPropertyChanged += this.CellAttribute;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.dataGridView1.CellEndEdit += this.CellEndEdit;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.dataGridView1.CellBeginEdit += this.CellBeginEdit;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnCount = 26;

            int myColumn = 0;
            for (char myChar = 'A'; myChar <= 'Z'; myChar++)
            {
                this.dataGridView1.Columns[myColumn].Name = myChar.ToString();
                myColumn++;
            }

            for (int myRow = 1; myRow <= 50; myRow++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[myRow - 1].HeaderCell.Value = myRow.ToString();
            }

            this.dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        /// <summary>
        /// updates the undo and redo menu items based on the current stack state.
        /// </summary>
        private void UpdateUndoRedoMenuItems()
        {
            this.undoChangingCellBackgroundColorToolStripMenuItem.Enabled = this.spreadsheet.CanUndo;
            this.redoCellTextChangeToolStripMenuItem.Enabled = this.spreadsheet.CanRedo;
            this.undoChangingCellBackgroundColorToolStripMenuItem.Text = this.spreadsheet.CanUndo ? $"Undo {this.spreadsheet.UndoTitle}" : "Undo";
            this.redoCellTextChangeToolStripMenuItem.Text = this.spreadsheet.CanRedo ? $"Redo {this.spreadsheet.RedoTitle}" : "Redo";
        }

        // updates the DataGridView cell whenever the spreadsheet Cell's Value changes.
        private void CellAttribute(object sender, PropertyChangedEventArgs e)
        {
            if (sender is not Cell myCell)
            {
                return;
            }

            if (e.PropertyName == nameof(Cell.Value))
            {
                this.dataGridView1[myCell.MyColumn, myCell.MyRow].Value = myCell.Value;
            }

            if (e.PropertyName == "BGColor")
            {
                System.Drawing.Color color = System.Drawing.Color.FromArgb((int)myCell.BGColor);
                this.dataGridView1[myCell.MyColumn, myCell.MyRow].Style.BackColor = color;
            }
        }

        private void CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // get the current displayed value of the cell being edited
            var myVal = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value;
            if (myVal != null)
            {
                // fetch the corresponding spreadsheet cell
                Cell cell = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex);

                // swap the display to show the raw text instead of the evaluated value
                this.dataGridView1[e.ColumnIndex, e.RowIndex].Value = cell.Text;
            }
        }

        // handles when the user finishes editing a DataGridView cell and updates the spreadsheet cell text.
        private void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var myVal = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value;
            if (myVal != null)
            {
                Cell cell = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex);

                this.spreadsheet.AddUndo(new CellSetTextCommand(cell, cell.Text));

                cell.Text = myVal.ToString();

                // update the grid to show Value instead of raw Text
                this.dataGridView1[e.ColumnIndex, e.RowIndex].Value = cell.Value;
                this.UpdateUndoRedoMenuItems();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void DemoButton_Click(object sender, EventArgs e)
        {
            Random myRnd = new();

            for (int myNumber = 0; myNumber < 49; myNumber++)
            {
                this.spreadsheet.GetCell(myRnd.Next(0, 49), myRnd.Next(0, 25)).Text = "I love C#!";
            }

            for (int myNumber = 0; myNumber <= 49; myNumber++)
            {
                this.spreadsheet.GetCell(myNumber, 1).Text = $"This is cell B{myNumber + 1}";
            }

            for (int myNumber = 0; myNumber <= 49; myNumber++)
            {
                this.spreadsheet.GetCell(myNumber, 0).Text = $"=B{myNumber + 1}";
            }
        }

        private void cellToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// opens a color dialog and sets the background color of all selected cells.
        /// </summary>
        private void SelectColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using ColorDialog colorPicker = new ColorDialog();
            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                List<(Cell, uint)> previousColors = new List<(Cell, uint)>();
                foreach (DataGridViewCell selectedCell in this.dataGridView1.SelectedCells)
                {
                    Cell cell = this.spreadsheet.GetCell(selectedCell.RowIndex, selectedCell.ColumnIndex);
                    previousColors.Add((cell, cell.BGColor)); // save previous color
                }

                this.spreadsheet.AddUndo(new CellSetColorCommand(previousColors));

                foreach ((Cell cell, uint _) in previousColors)
                {
                    cell.BGColor = (uint)colorPicker.Color.ToArgb();
                }
            }

            this.dataGridView1.ClearSelection();
            this.UpdateUndoRedoMenuItems();
        }

        private void undoChangingCellBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.spreadsheet.Undo();
            this.UpdateUndoRedoMenuItems();
        }

        private void redoCellTextChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.spreadsheet.Redo();
            this.UpdateUndoRedoMenuItems();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new();

            // only lets user save it as xml files
            saveFile.Filter = "xml files (*.xml)|*.xml";
            saveFile.FilterIndex = 2;
            saveFile.RestoreDirectory = true;

            // Check if the file was chosen or created successfully
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                // Creates a new StreamWriter object to write to the newly opened file
                using (StreamWriter outStream = new(saveFile.FileName))
                {
                    this.spreadsheet.Save(outStream);
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new();
            // only allow xml files
            openFile.Filter = "xml files (*.xml)|*.xml";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;

            // check if a file was chosen successfully
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using StreamReader inStream = new(openFile.FileName);
                    this.spreadsheet.Load(inStream);
                }
                catch (XmlException ex)
                {
                    MessageBox.Show("invalid xml file: " + ex.Message, "load error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("malformed cell data: " + ex.Message, "load error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("could not read file: " + ex.Message, "load error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.spreadsheet.Clear();
        }
    }
}
