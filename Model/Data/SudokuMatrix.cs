﻿using DokuApp.Model.Builder;
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
        
        /// <summary>
        /// Sets the corner possibility to true.
        /// </summary>
        /// <param name="position">Target cell.</param>
        /// <param name="corner">Target number.</param>
        public void AddCorner(Tuple<int, int> position, int corner)
        {
            _options[corner - 1].Flip(LogicBuilder.Cell(position));
        }

        public CellData[][] CellData()
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

                    bool permenant = _values.Permenance.Truths[col, row];

                    rowData[col] = new CellData(_values.Matrix[col, row], cornerData.ToArray(), permenant);
                }

                data[row] = rowData;
            }

            return data;
        }

        /// <summary>
        /// Set a whole option board to a new LogicMatrix.
        /// </summary>
        /// <param name="newOption">New board.</param>
        /// <param name="target">Targeted number. Must be between 0 and 8 to apply.</param>
        public void SetOption(LogicMatrix newOption, int target)
        {
            if (target < 0 || target > 8 || newOption == null)
            {
                return;
            }

            _options[target] = newOption;
        }

        /// <summary>
        /// Set the ValueMatrix to a new ValueMatrix.
        /// </summary>
        /// <param name="newValues">New matrix.</param>
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
