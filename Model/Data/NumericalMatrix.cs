using DokuApp.Model.Builder;
using System;

namespace DokuApp.Model.Data
{
    public class NumericalMatrix
    {
        private int[,] _matrix;
        public int[,] Matrix { get { return _matrix; } }
        
        private LogicMatrix _numberPermenance;
        public LogicMatrix Permenance { get { return _numberPermenance; } }

        public NumericalMatrix()
        {
            _matrix = new int[9, 9];
            _numberPermenance = new LogicMatrix();
        }

        public NumericalMatrix(int[,] matrix)
        {
            _matrix = new int[9, 9];
            _numberPermenance = new LogicMatrix();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    try
                    {
                        _matrix[i, j] = matrix[i, j];
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Set the specified cell to the specified value.
        /// </summary>
        /// <param name="cell">Cell to set.</param>
        /// <param name="value">Must be between 1 and 9. Use Delete instead of setting 0 here.</param>
        /// <param name="permenance">The permenance factor of the cell.</param>
        /// <returns>Returns false if value or cell was out of range.</returns>
        public bool SetCell(Tuple<int, int> cell, int value, bool permenance)
        {
            (int col, int row) = cell;

            if (row < 0 || row > 8 || col < 0 || col > 8|| value < 1 || value > 9)
            {
                return false;
            }

            _matrix[col, row] = value;
            _numberPermenance.SetCell(cell, permenance);
            return true;
        }

        /// <summary>
        /// Sets specified cell to 0.
        /// </summary>
        /// <param name="cell">Cell to delete.</param>
        /// <returns>Returns false if cell is out of range.</returns>
        public bool DeleteCell(Tuple<int, int> cell)
        {
            (int col, int row) = cell;

            if (row < 0 || row > 8 || col < 0 || col > 8)
            {
                return false;
            }

            if (_matrix[col, row] == 0)
            {
                return false;
            }

            _matrix[col, row] = 0;
            _numberPermenance.SetCell(cell, false);
            return true;
        }

        /// <summary>
        /// Returns the matrix as a LogicMatrix
        /// </summary>
        /// <returns>If a cell holds a value, the logic for that cell is true. LogicMatrix[cell] = (values[cell] != 0)</returns>
        public LogicMatrix AsLogic()
        {
            bool[,] truths = new bool[9, 9];

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    truths[col, row] = (_matrix[col, row] != 0);
                }
            }

            return new LogicMatrix(truths);
        }

        /// <summary>
        /// All impermenant numbers inside the matrix are deleted (set to 0);
        /// </summary>
        public void ClearImpermenantNumbers()
        {
            for (int i = 0; i < 81; i++)
            {
                (int col, int row) = CellPosition.Index(i);

                if (!_numberPermenance.Truths[col, row])
                {
                    _matrix[col, row] = 0;
                }
            }
        }

        /// <summary>
        /// Resets all values in the NumericalMatrix: Values all to 0 and Permenance all to false.
        /// </summary>
        public void Reset()
        {
            _matrix = new int[9, 9];
            _numberPermenance = new LogicMatrix();
        }

        /// <summary>
        /// Returns permenance of the cell.
        /// </summary>
        /// <param name="cell">Cell to check.</param>
        /// <returns>Returns true if cell is permenant.</returns>
        public bool CellIsPermenant(Tuple<int, int> cell)
        {
            return _numberPermenance.Truths[cell.Item1, cell.Item2];
        }
    }
}
