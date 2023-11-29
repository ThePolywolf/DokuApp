using DokuApp.Model.UI;
using System.Collections.Generic;

namespace DokuApp.Model.Data
{
    class SudokuMatrix
    {
        private NumericalMatrix _values;
        private LogicMatrix[] _options;

        public NumericalMatrix Values {  get { return _values; } }
        public LogicMatrix[] Options { get { return _options; } }

        public SudokuMatrix()
        {
            _values = new NumericalMatrix();
            _options = new LogicMatrix[9];

            for (int i = 0; i < 9; i++)
            {
                _options[i] = new LogicMatrix();
            }
        }

        public CellData[][] CellData()
        {
            CellData[][] data = new CellData[9][];

            for (int row = 0; row < 9; row++)
            {
                CellData[] rowData = new CellData[9];

                for (int col = 0; col < 9; col++)
                {
                    List<int> cornerData = new List<int>();

                    for (int corner = 0; corner < 9; corner++)
                    {
                        if (_options[corner].Truths[row, col])
                        {
                            cornerData.Add(corner + 1);
                        }
                    }

                    rowData[col] = new CellData(_values.Matrix[row, col], cornerData.ToArray());
                }

                data[row] = rowData;
            }

            return data;
        }
    }
}
