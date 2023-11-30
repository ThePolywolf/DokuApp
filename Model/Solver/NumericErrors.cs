using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;
using System.Collections.Generic;

namespace DokuApp.Model.Solver
{
    internal class NumericErrors
    {
        public NumericErrors()
        {
        }

        public static LogicMatrix FindErrors(NumericalMatrix values)
        {
            LogicMatrix errors = new();

            for (int i = 0; i < 9; i++)
            {
                // rows <= i
                int[] rowData = Extractor.NumericRow(values, i);
                errors.Add(CheckOptions(rowData, i, LogicBuilder.Position.Row));

                // columns <= i
                int[] colData = Extractor.NumericColumn(values, i);
                errors.Add(CheckOptions(colData, i, LogicBuilder.Position.Column));

                // boxes <= i
                int[] boxData = Extractor.NumericBox(values, i);
                errors.Add(CheckOptions(boxData, i, LogicBuilder.Position.Box));
            }

            return errors;
        }

        private static LogicMatrix CheckOptions(int[] data, int index, LogicBuilder.Position type)
        {
            List<Tuple<int, int>> cells = new();

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
