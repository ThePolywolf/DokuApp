using DokuApp.Model.Data;
using System;
using System.Linq;

namespace DokuApp.Model.Builder
{
    class LogicBuilder
    {
        public enum Position
        {
            Row,
            Column,
            Box
        }

        public static LogicMatrix? Matrix(Position position, int[] targets)
        {
            if (position == Position.Row)
            {
                return Rows(targets);
            }
            
            if (position == Position.Column)
            {
                return Columns(targets);
            }

            if (position == Position.Box)
            {
                return Boxes(targets);
            }

            return null;
        }

        public static LogicMatrix All(bool value)
        {
            bool[,] truths = new bool[9, 9];

            for (int cell = 0; cell < 81; cell++)
            {
                int column = cell % 9;
                truths[(cell - column) / 9, column] = value;
            }

            return new LogicMatrix(truths);
        }

        public static LogicMatrix Rows(int[] rows)
        {
            bool[,] truths = new bool[9, 9];

            for (int row = 0; row < 9; row++)
            {
                bool value = rows.Contains(row);

                for (int column = 0; column < 9; column++)
                {
                    truths[row, column] = value;
                }
            }

            return new LogicMatrix(truths);
        }

        public static LogicMatrix Columns(int[] columns)
        {
            bool[,] truths = new bool[9, 9];

            for (int column = 0; column < 9; column++)
            {
                bool value = columns.Contains(column);

                for (int row = 0; row < 9; row++)
                {
                    truths[row, column] = value;
                }
            }

            return new LogicMatrix(truths);
        }

        public static LogicMatrix Boxes(int[] boxes)
        {
            bool[,] truths = new bool[9, 9];

            for (int box = 0; box < 9; box++)
            {
                bool value = boxes.Contains(box);

                for (int cell = 0; cell < 9; cell++)
                {
                    int row = (box - (box % 3)) + ((cell - (cell % 3)) / 3);
                    int column = 3 * (box % 3) + (cell % 3);

                    truths[row, column] = value;
                }
            }

            return new LogicMatrix(truths);
        }

        public static LogicMatrix Cells(Tuple<int, int>[] cells)
        {
            bool[,] truths = new bool[9, 9];

            foreach (Tuple<int, int> cell in cells)
            {
                if (cell.Item1 < 0 || cell.Item1 >= 9 || cell.Item2 < 0 || cell.Item2 >= 9)
                {
                    continue;
                }

                truths[cell.Item1, cell.Item2] = true;
            }

            return new LogicMatrix(truths);
        }

        public static int ExtractPosition(Position position, Tuple<int, int> cell)
        {
            int row = cell.Item1;
            int col = cell.Item2;

            if (position == Position.Row)
            {
                return row;
            }

            if (position == Position.Column)
            {
                return col;
            }

            // otherwise Position.Box
            int box = ((row - (row % 3)) / 3) + (col - (col % 3));
            return box;
        }

        public static LogicMatrix CellExclusion(Tuple<int, int> cell)
        {
            LogicMatrix matrix = All(false);

            int[] row = new int[] { ExtractPosition(Position.Row, cell) };
            int[] column = new int[] { ExtractPosition(Position.Column, cell) };
            int[] box = new int[] { ExtractPosition(Position.Box, cell) };

            matrix.Add(Rows(row));
            matrix.Add(Columns(column));
            matrix.Add(Boxes(box));

            return matrix;
        }

        public static LogicMatrix CellExclusions(Tuple<int, int>[] cells)
        {
            LogicMatrix[] matricies = new LogicMatrix[cells.Length];

            for (int i = 0;  i < cells.Length; i++)
            {
                matricies[i] = CellExclusion(cells[i]);
            }

            LogicMatrix matrix = All(false);

            foreach(LogicMatrix addition in matricies)
            {
                matrix.Add(addition);
            }

            return matrix;
        }
    }
}
