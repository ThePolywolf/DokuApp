using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DokuApp.Model.Solver
{
    abstract class HiddenSetStrategy : SetStrategy
    {
        /// <summary>
        /// Solves the gameboard for a specified Hidden multi-sets (pair, triple, etc.).
        /// </summary>
        /// <param name="gameboard">Gameboard to ater.</param>
        /// <returns>Only true if a Hidden set was found (gameboard was changed)</returns>
        public override bool Solve(SudokuMatrix gameboard)
        {
            List<int[]> allPairingOptions = AllMultiSets(_multi);

            foreach (int[] pairingOptions in allPairingOptions)
            {
                LogicMatrix overlappedLogic = Extractor.OverlapOptions(gameboard, pairingOptions);

                for (int target = 0; target < 9; target++)
                {
                    // rows
                    bool[] row = Extractor.LogicalRow(overlappedLogic, target);
                    if (SetHasMulti(row, out int[] rowCells))
                    {
                        Tuple<int, int>[] targetCells = new Tuple<int, int>[_multi];

                        for (int i = 0; i < _multi; i++)
                        {
                            targetCells[i] = Tuple.Create(rowCells[i], target);
                        }

                        ClearCellsForMulti(pairingOptions, targetCells, gameboard);
                        return true;
                    }

                    // columns
                    bool[] col = Extractor.LogicalColumn(overlappedLogic, target);
                    if (SetHasMulti(col, out int[] colCells))
                    {
                        Tuple<int, int>[] targetCells = new Tuple<int, int>[_multi];

                        for (int i = 0; i < _multi; i++)
                        {
                            targetCells[i] = Tuple.Create(target, colCells[i]);
                        }

                        ClearCellsForMulti(pairingOptions, targetCells, gameboard);
                        return true;
                    }

                    // boxes
                    bool[] box = Extractor.LogicalBox(overlappedLogic, target);
                    if (SetHasMulti(box, out int[] boxCells))
                    {
                        Tuple<int, int>[] targetCells = new Tuple<int, int>[_multi];

                        for (int i = 0; i < _multi; i++)
                        {
                            targetCells[i] = CellPosition.BoxCell(target, boxCells[i]);
                        }

                        ClearCellsForMulti(pairingOptions, targetCells, gameboard);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Removes all non-set values from the cells.
        /// </summary>
        /// <param name="multiSet">Numbers to keep in the cells.</param>
        /// <param name="cells">Cells to target for removal.</param>
        /// <param name="gameboard">Sudoku matrix getting changed.</param>
        private void ClearCellsForMulti(int[] multiSet, Tuple<int, int>[] cells, SudokuMatrix gameboard)
        {
            for (int number = 0; number < 9; number++)
            {
                // skip number clearing if part of the multi set
                if (multiSet.Contains(number))
                {
                    continue;
                }

                // disable the possibility in the cell
                foreach (Tuple<int, int> cell in cells)
                {
                    gameboard.Options[number].SetCell(cell, false);
                }
            }
        }
    }
}
