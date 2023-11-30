using System;

namespace DokuApp.Model.Builder
{
    internal class CellPosition
    {
        public static Tuple<int, int> BoxCell(int box, int cell)
        {
            box = Math.Clamp(box, 0, 8);
            cell = Math.Clamp(cell, 0, 8);

            //      BOX      Col    Row         CELL     Col    Row
            //      0 1 2    0 3 6  0 0 0       0 1 2    0 1 2  0 0 0
            //      3 4 5 -> 0 3 6  3 3 3       3 4 5 -> 0 1 2  1 1 1
            //      6 7 8    0 3 6  6 6 6       6 7 8    0 1 2  2 2 2

            int col = 3 * (box % 3) + (cell % 3);
            int row = (box - (box % 3)) + ((cell - (cell % 3)) / 3);
            return Tuple.Create(col, row);
        }

        public static Tuple<int, int> InverseBoxCell(int col, int row)
        {
            col = Math.Clamp(col, 0, 8);
            row = Math.Clamp(row, 0, 8);

            int box = ((col - (col % 3)) / 3) + (row - (row % 3));
            int cell = (col % 3) + (3 * (row % 3));
            return Tuple.Create(box, cell);
        }
    }
}
