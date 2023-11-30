using DokuApp.Model.Data;
using System;

namespace DokuApp.Model.Builder
{
    internal class Extractor
    {
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

        public static int[] NumericBox(NumericalMatrix matrix, int box)
        {
            int[] results = new int[9];

            if (Math.Clamp(box, 0, 8) != box)
            {
                return results;
            }

            for (int cell = 0; cell < 9; cell++)
            {
                Tuple<int, int> pos = CellPosition.BoxCell(box, cell);

                results[cell] = matrix.Matrix[pos.Item1, pos.Item2];
            }

            return results;
        }
    }
}
