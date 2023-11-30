using System;

namespace DokuApp.Model.Data
{
    public class NumericalMatrix
    {
        private readonly int[,] _matrix;
        public int[,] Matrix { get { return _matrix; } }

        public NumericalMatrix()
        {
            _matrix = new int[9, 9];
        }

        public NumericalMatrix(int[,] matrix)
        {
            _matrix = new int[9, 9];

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

        public bool SetCell(Tuple<int, int> cell, int value)
        {
            (int col, int row) = cell;

            if (Math.Clamp(row, 0, 9) != row || Math.Clamp(col, 0, 8) != col || Math.Clamp(value, 1, 9) != value)
            {
                return false;
            }

            _matrix[col, row] = value;
            return true;
        }

        public bool DeleteCell(Tuple<int, int> cell)
        {
            int col = cell.Item1;
            int row = cell.Item2;

            if (Math.Clamp(row, 0, 8) != row || Math.Clamp(col, 0, 8) != col)
            {
                return false;
            }

            _matrix[col, row] = 0;
            return true;
        }

        public LogicMatrix AsLogic()
        {
            bool[,] truths = new bool[9, 9];

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (_matrix[col, row] != 0)
                    {
                        truths[col, row] = true;
                    }
                }
            }

            return new LogicMatrix(truths);
        }
    }
}
