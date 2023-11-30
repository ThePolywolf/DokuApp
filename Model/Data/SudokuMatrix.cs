using DokuApp.Model.Builder;
using DokuApp.Model.UI;
using System;
using System.Collections.Generic;

namespace DokuApp.Model.Data
{
    public class SudokuMatrix
    {
        private NumericalMatrix _values;
        private readonly LogicMatrix[] _options;

        public NumericalMatrix Values {  get { return _values; } }
        public LogicMatrix[] Options { get { return _options; } }

        // constructors
        public SudokuMatrix()
        {
            _values = new NumericalMatrix();
            _options = new LogicMatrix[9];

            for (int i = 0; i < 9; i++)
            {
                _options[i] = new LogicMatrix();
            }
        }

        public SudokuMatrix(NumericalMatrix values)
        {
            _values = values;
            _options = new LogicMatrix[9];

            for (int i = 0; i < 9; i++)
            {
                _options[i] = new LogicMatrix();
            }
        }
        
        public void AddCorner(Tuple<int, int> position, int corner)
        {
            _options[corner - 1].Flip(LogicBuilder.Cell(position));
        }

        public CellData[][] CellData(LogicMatrix? permenance = null)
        {
            CellData[][] data = new CellData[9][];

            for (int row = 0; row < 9; row++)
            {
                CellData[] rowData = new CellData[9];

                for (int col = 0; col < 9; col++)
                {
                    List<int> cornerData = new();

                    for (int corner = 0; corner < 9; corner++)
                    {
                        if (_options[corner].Truths[col, row])
                        {
                            cornerData.Add(corner + 1);
                        }
                    }

                    bool permenant = false;

                    if (permenance != null)
                    {
                        permenant = permenance.Truths[col, row];
                    }

                    rowData[col] = new CellData(_values.Matrix[col, row], cornerData.ToArray(), permenant);
                }

                data[row] = rowData;
            }

            return data;
        }

        // Setters
        public void SetOption(LogicMatrix newOption, int target)
        {
            if (target < 0 || target >= 9 || newOption == null)
            {
                return;
            }

            _options[target] = newOption;
        }

        public void SetValues(NumericalMatrix newValues)
        {
            if (newValues == null)
            {
                return;
            }

            _values = newValues;
        }
    }
}
