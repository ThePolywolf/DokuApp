using System;

namespace DokuApp.Model.Data
{
    class NumericalMatrix
    {
        private int[,] _matrix;
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
            int row = cell.Item1;
            int col = cell.Item2;

            if (Math.Clamp(row, 0, 9) != row || Math.Clamp(col, 0, 8) != col || Math.Clamp(value, 1, 9) != value)
            {
                return false;
            }

            _matrix[row, col] = value;
            return true;
        }

        public bool DeleteCell(Tuple<int, int> cell)
        {
            int row = cell.Item1;
            int col = cell.Item2;

            if (Math.Clamp(row, 0, 9) != row || Math.Clamp(col, 0, 8) != col)
            {
                return false;
            }

            _matrix[row, col] = 0;
            return true;
        }
    }
}
