using DokuApp.Model.Data;
using System;

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
        public static LogicMatrix All(bool value)
        {
            bool[,] truths = new bool[9, 9];

            for (int cell = 0; cell < 81; cell++)
            {
                int col = cell % 9;
                int row = (cell - col) / 9;
                truths[col, row] = value;
            }

            return new LogicMatrix(truths);
        }

        public static LogicMatrix Row(int row)
        {
            bool[,] truths = new bool[9, 9];

            row = Math.Clamp(row, 0, 8);

            for (int col = 0; col < 9; col++)
            {
                truths[col, row] = true;
            }

            return new LogicMatrix(truths);
        }

        public static LogicMatrix Column(int col)
        {
            bool[,] truths = new bool[9, 9];

            col = Math.Clamp(col, 0, 8);

            for (int row = 0; row < 9; row++)
            {
                truths[col, row] = true;
            }

            return new LogicMatrix(truths);
        }

        public static LogicMatrix Box(int box)
        {
            bool[,] truths = new bool[9, 9];

            box = Math.Clamp(box, 0, 8);

            for (int cell = 0; cell < 9; cell++)
            {
                (int col, int row) = CellPosition.BoxCell(box, cell);

                truths[col, row] = true;
            }

            return new LogicMatrix(truths);
        }

        public static LogicMatrix Cell(Tuple<int, int> cell)
        {
            bool[,] truths = new bool[9, 9];

            if (cell.Item1 < 0 || cell.Item1 >= 9 || cell.Item2 < 0 || cell.Item2 >= 9)
            {
                return new LogicMatrix();
            }

            truths[cell.Item1, cell.Item2] = true;

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
            int col = cell.Item1;
            int row = cell.Item2;

            if (position == Position.Row)
            {
                return row;
            }

            if (position == Position.Column)
            {
                return col;
            }

            // otherwise Position.Box
            (int box, int nCell) = CellPosition.InverseBoxCell(col, row);
            return box;
        }

        public static LogicMatrix CellExclusion(Tuple<int, int> cell)
        {
            LogicMatrix matrix = All(false);

            int row =  ExtractPosition(Position.Row, cell);
            int column = ExtractPosition(Position.Column, cell);
            int box = ExtractPosition(Position.Box, cell);

            matrix.Add(Row(row));
            matrix.Add(Column(column));
            matrix.Add(Box(box));

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
