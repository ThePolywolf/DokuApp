using System;

namespace DokuApp.Model.Builder
{
    internal class CellPosition
    {
        /// <summary>
        /// Takes in a box and cell and returns their column and row.
        /// </summary>
        /// <param name="box">Box number (clamped 0 - 8)</param>
        /// <param name="cell">Cell number (clamped 0 - 8)</param>
        /// <returns>(Column, Row) as Tuple.</returns>
        public static Tuple<int, int> BoxCell(int box, int cell)
        {
            box = Math.Clamp(box, 0, 8);
            cell = Math.Clamp(cell, 0, 8);

            int col = 3 * (box % 3) + (cell % 3);
            int row = (box - (box % 3)) + ((cell - (cell % 3)) / 3);
            return Tuple.Create(col, row);
        }

        /// <summary>
        /// Takes a column / row coordinate and returns their box / cell coordinate.
        /// </summary>
        /// <param name="col">Column (clamped 0 - 8)</param>
        /// <param name="row">Row (clamped 0 - 8)</param>
        /// <returns>(Box, Cell) as Tuple.</returns>
        public static Tuple<int, int> InverseBoxCell(int col, int row)
        {
            col = Math.Clamp(col, 0, 8);
            row = Math.Clamp(row, 0, 8);

            int box = ((col - (col % 3)) / 3) + (row - (row % 3));
            int cell = (col % 3) + (3 * (row % 3));
            return Tuple.Create(box, cell);
        }

        /// <summary>
        /// Takes index from 0 - 80 and returns the column / row cordinate.
        /// </summary>
        /// <param name="index">Index (clamped 0 - 80)</param>
        /// <returns>(Column, Row) as Tuple.</returns>
        public static Tuple<int, int> Index(int index)
        {
            index = Math.Clamp(index, 0, 80);

            int col = (index % 9);
            int row = (index - col) / 9;

            return Tuple.Create(col, row);
        }
    }
}
