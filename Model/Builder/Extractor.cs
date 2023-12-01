using DokuApp.Model.Data;
using System;
using System.Linq;

namespace DokuApp.Model.Builder
{
    internal class Extractor
    {
        /// <summary>
        /// Used to retrieve a row from a NumericalMatrix.
        /// </summary>
        /// <param name="matrix">Matrix to pull from.</param>
        /// <param name="row">Row to retrieve.</param>
        /// <returns>Returns an array. To access: results[col].</returns>
        public static int[] NumericRow(NumericalMatrix matrix, int row)
        {
            int[] results = new int[9];

            if (Math.Clamp(row, 0, 8) != row)
            {
                return results;
            }

            for (int col = 0; col < 9; col++)
            {
                results[col] = matrix.Matrix[col, row];
            }

            return results;
        }

        /// <summary>
        /// Used to retrieve a column from a NumericalMatrix.
        /// </summary>
        /// <param name="matrix">Matrix to pull from.</param>
        /// <param name="col">Column to retrieve.</param>
        /// <returns>Returns column data. To access: results[row]</returns>
        public static int[] NumericColumn(NumericalMatrix matrix, int col)
        {
            int[] results = new int[9];

            if (Math.Clamp(col, 0, 8) != col)
            {
                return results;
            }

            for (int row = 0; row < 9; row++)
            {
                results[row] = matrix.Matrix[col, row];
            }

            return results;
        }

        /// <summary>
        /// Used to retrieve a box from NumericalData
        /// </summary>
        /// <param name="matrix">Matrix to pull from.</param>
        /// <param name="box">Box to retrieve.</param>
        /// <returns>Returns box data. To access: results[cell]</returns>
        public static int[] NumericBox(NumericalMatrix matrix, int box)
        {
            int[] results = new int[9];

            if (Math.Clamp(box, 0, 8) != box)
            {
                return results;
            }

            for (int cell = 0; cell < 9; cell++)
            {
                (int col, int row) = CellPosition.BoxCell(box, cell);

                results[cell] = matrix.Matrix[col, row];
            }

            return results;
        }

        /// <summary>
        /// Used to retreive a logical row.
        /// </summary>
        /// <param name="matrix">Matrix to pull from.</param>
        /// <param name="row">Row to retreive.</param>
        /// <returns>To access: results[col] => matrix[col, row]</returns>
        public static bool[] LogicalRow(LogicMatrix matrix, int row)
        {
            bool[] results = new bool[9];

            if (Math.Clamp(row, 0, 8) != row)
            {
                return results;
            }

            for (int col = 0; col < 9; col++)
            {
                results[col] = matrix.Truths[col, row];
            }

            return results;
        }

        /// <summary>
        /// Used to retrieve a logical column.
        /// </summary>
        /// <param name="matrix">Matrix to pull from.</param>
        /// <param name="col">Column to retrieve.</param>
        /// <returns>To access: results[row] => matrix[col, row]</returns>
        public static bool[] LogicalColumn(LogicMatrix matrix, int col)
        {
            bool[] results = new bool[9];

            if (Math.Clamp(col, 0, 8) != col)
            {
                return results;
            }

            for (int row = 0; row < 9; row++)
            {
                results[row] = matrix.Truths[col, row];
            }

            return results;
        }

        /// <summary>
        /// Used to retrieve a logical box.
        /// </summary>
        /// <param name="matrix">Matrix to pull from.</param>
        /// <param name="box">Box to retrieve.</param>
        /// <returns>results[cell] => [box, cell]. Use CellPosition.BoxCell to convert to [col, row]</returns>
        public static bool[] LogicalBox(LogicMatrix matrix, int box)
        {
            bool[] results = new bool[9];

            if (Math.Clamp(box, 0, 8) != box)
            {
                return results;
            }

            for (int cell = 0; cell < 9; cell++)
            {
                (int col, int row) = CellPosition.BoxCell(box, cell);

                results[cell] = matrix.Truths[col, row];
            }

            return results;
        }

        /// <summary>
        /// Takes in possibilities matrix and returns array of truths at specified index
        /// </summary>
        /// <param name="matricies">Target matricies.</param>
        /// <param name="index">Target index.</param>
        /// <returns>result[i] = matrix[i][@index]</returns>
        public static bool[] LogicalPossibilities(LogicMatrix[] matricies, int index)
        {
            (int col, int row) = CellPosition.Index(index);

            bool[] results = new bool[9];

            for (int i = 0; i < 9; i++)
            {
                results[i] = matricies[i].Truths[col, row];
            }

            return results;
        }

        /// <summary>
        /// Overlaps all selected option matricies with LogicMatrix addition.
        /// </summary>
        /// <param name="gameboard">Target gameboard for the options.</param>
        /// <param name="selections">Selected options to overlap. Make sure all are between 0 and 8.</param>
        /// <returns>Returns logic matrix of added matricies.</returns>
        public static LogicMatrix OverlapOptions(SudokuMatrix gameboard, int[] selections)
        {
            if (selections.Length <= 0)
            {
                return new LogicMatrix();
            }

            LogicMatrix primary = new();

            for (int i = 0; i < selections.Length; i++)
            {
                primary.Add(gameboard.Options[selections[i]]);
            }

            return primary;
        }

        /// <summary>
        /// Overlaps all options except the selections' option matricies with LogicMatrix addition.
        /// </summary>
        /// <param name="gameboard">Target gameboard for the options.</param>
        /// <param name="selections">Selected options to NOT overlap. Should be between 0 and 8.</param>
        /// <returns>Returns logic matrix of added excepted matricies.</returns>
        public static LogicMatrix ExceptOverlapOptions(SudokuMatrix gameboard, int[] selections)
        {
            LogicMatrix primary = new();

            for (int i = 0; i < 9; i++)
            {
                // skip selection option matricies
                if (selections.Contains(i))
                {
                    continue;
                }

                primary.Add(gameboard.Options[i]);
            }

            return primary;
        }
    }
}
