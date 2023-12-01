using DokuApp.Model.Builder;
using System;

namespace DokuApp.Model.Data
{
    public class LogicMatrix
    {
        private readonly bool[,] _truths;

        public bool[,] Truths { get { return  _truths; } }

        public LogicMatrix()
        {
            _truths = new bool[9, 9];
        }

        public LogicMatrix(bool[,] truths)
        {
            _truths = new bool[9, 9];

            SetLogic(truths);

        }

        /// <summary>
        /// Set the logic to a new value.
        /// </summary>
        /// <param name="truths">New logic. Automatically fills in gaps if not a 9x9 matrix.</param>
        public void SetLogic(bool[,] truths)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    try
                    {
                        _truths[i, j] = truths[i, j];
                    }
                    catch
                    {
                        _truths[i, j] = false;
                    }
                }
            }
        }

        /// <summary>
        /// Adds another matrix to it. value = Current || New. Combination of all truths.
        /// </summary>
        /// <param name="matrix">Matrix to add.</param>
        /// <param name="changed">Out: if there was any change as a result of the addition.</param>
        /// <returns>Returns self to allow method chaining.</returns>
        public LogicMatrix Add(LogicMatrix matrix, out bool changed)
        {
            changed = false;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    bool newTruth = _truths[i, j] || matrix.Truths[i, j];

                    if (!changed && _truths[i, j] != newTruth)
                    {
                        changed = true;
                    }

                    _truths[i, j] = newTruth;
                }
            }

            return this;
        }

        /// <summary>
        /// Adds another matrix to it. value = Current || New. Combination of all truths. There is also a variation with an out bool changed.
        /// </summary>
        /// <param name="matrix">Matrix to add.</param>
        /// <returns>Returns self to allow method chaining.</returns>
        public LogicMatrix Add(LogicMatrix matrix)
        {
            return Add(matrix, out bool _1);
        }

        /// <summary>
        /// Subtracts by given matrix. If matrix[cell] = true, self[cell] = false.
        /// </summary>
        /// <param name="matrix">Matrix to subtract by.</param>
        /// <returns>Returns self for method chaining.</returns>
        public LogicMatrix Subtract(LogicMatrix matrix)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    bool newTruth = _truths[i, j] && !matrix.Truths[i, j];

                    _truths[i, j] = newTruth;
                }
            }

            return this;
        }

        /// <summary>
        /// Flip all cells corresponding to the matrix. Tries to add matrix, and if there is no change, subtracts the matrix.
        /// </summary>
        /// <param name="matrix">Matrix to flip by (add or subtract)</param>
        public void Flip(LogicMatrix matrix)
        {
            Add(matrix, out bool changed);
            if (changed)
            {
                return;
            }

            Subtract(matrix);
            return;
        }

        /// <summary>
        /// Figures out if the whole matrix is true.
        /// </summary>
        /// <returns>Returns false if any value in the matrix is false.</returns>
        public bool IsTrue()
        {
            for (int index = 0; index < 9; index++)
            {
                (int col, int row) = CellPosition.Index(index);

                if (!_truths[col, row])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Set the cell to a new value.
        /// </summary>
        /// <param name="position">Cell to change.</param>
        /// <param name="value">New value to set to.</param>
        /// <returns>True if new cell value is different from its old value.</returns>
        public bool SetCell(Tuple<int, int> position, bool value)
        {
            (int col, int row) = position;
            if (_truths[col, row] == value)
            {
                return false;
            }

            _truths[col, row] = value;
            return true;
        }

        /// <summary>
        /// Checks is a cell is true
        /// </summary>
        /// <param name="position">Cell to check.</param>
        /// <returns>Returns the value of the cell.</returns>
        public bool IsCellTrue(Tuple<int, int> position)
        {
            return _truths[position.Item1, position.Item2];
        }
    }
}
