using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DokuApp.Model.Solver
{
    internal class NumericErrors
    {
        public NumericErrors()
        {
        }

        public LogicMatrix FindErrors(NumericalMatrix values)
        {
            LogicMatrix errors = new LogicMatrix();

            // rows
            for (int row = 0; row < 9; row++)
            {
                int[] rowData = Extractor.NumericRow(values, row);
                errors.Add(CheckOptions(rowData, row, LogicBuilder.Position.Row));
            }

            // columns
            for (int col = 0; col < 9; col++)
            {
                int[] colData = Extractor.NumericColumn(values, col);
                errors.Add(CheckOptions(colData, col, LogicBuilder.Position.Column));
            }

            // boxes
            for (int box = 0; box < 9; box++)
            {
                int[] boxData = Extractor.NumericBox(values, box);
                errors.Add(CheckOptions(boxData, box, LogicBuilder.Position.Box));
            }

            return errors;
        }

        private LogicMatrix CheckOptions(int[] data, int index, LogicBuilder.Position type)
        {
            List<Tuple<int, int>> cells = new List<Tuple<int, int>>();

            for (int i = 1; i < 9; i++)
            {
                if (data[i] == 0)
                {
                    continue;
                }

                for (int j = 0; j < i; j++)
                {
                    if (data[j] == 0)
                    {
                        continue;
                    }

                    if (data[i] == data[j])
                    {
                        if (type == LogicBuilder.Position.Row)
                        {
                            cells.Add(Tuple.Create(i, index));
                            cells.Add(Tuple.Create(j, index));
                            continue;
                        }

                        if (type == LogicBuilder.Position.Column)
                        {
                            cells.Add(Tuple.Create(index, i));
                            cells.Add(Tuple.Create(index, j));
                            continue;
                        }

                        // else type == LogicBuilder.Position.Box
                        cells.Add(CellPosition.BoxCell(index, i));
                        cells.Add(CellPosition.BoxCell(index, j));
                        continue;
                    }
                }
            }

            return LogicBuilder.Cells(cells.ToArray());
        }
    }
}
