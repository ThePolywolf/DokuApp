using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DokuApp.Model.Solver
{
    abstract class HiddenSetStrategy : SetStrategy
    {
        /// <summary>
        /// Solves the gameboard for a specified Hidden multi-sets (pair, triple, etc.).
        /// </summary>
        /// <param name="gameboard">Gameboard to alter.</param>
        /// <returns>Only true if a Hidden set was found (gameboard was changed).</returns>
        public override bool Solve(SudokuMatrix gameboard)
        {
            List<int[]> allPairingOptions = AllMultiSets(_multi);

            foreach (int[] pairingOptions in allPairingOptions)
            {
                LogicMatrix overlappedLogic = Extractor.OverlapOptions(gameboard, pairingOptions);

                // loop through [row, column, box] 1-9 (0-8)
                for (int target = 0; target < 9; target++)
                {
                    // row <-- target
                    bool[] row = Extractor.LogicalRow(overlappedLogic, target);
                    if (SetHasMulti(row, out int[] rowCells))
                    {
                        Tuple<int, int>[] targetCells = new Tuple<int, int>[_multi];

                        for (int i = 0; i < _multi; i++)
                        {
                            targetCells[i] = Tuple.Create(rowCells[i], target);
                        }

                        bool changed = ClearCellsForMulti(pairingOptions, targetCells, gameboard);
                        
                        if (changed)
                        {
                            return true;
                        }
                    }

                    // column <-- target
                    bool[] col = Extractor.LogicalColumn(overlappedLogic, target);
                    if (SetHasMulti(col, out int[] colCells))
                    {
                        Tuple<int, int>[] targetCells = new Tuple<int, int>[_multi];

                        for (int i = 0; i < _multi; i++)
                        {
                            targetCells[i] = Tuple.Create(target, colCells[i]);
                        }

                        bool changed = ClearCellsForMulti(pairingOptions, targetCells, gameboard);
                        
                        if (changed)
                        {
                            return true;
                        }
                    }

                    // boxes <-- target
                    bool[] box = Extractor.LogicalBox(overlappedLogic, target);
                    if (SetHasMulti(box, out int[] boxCells))
                    {
                        Tuple<int, int>[] targetCells = new Tuple<int, int>[_multi];

                        for (int i = 0; i < _multi; i++)
                        {
                            targetCells[i] = CellPosition.BoxCell(target, boxCells[i]);
                        }

                        bool changed = ClearCellsForMulti(pairingOptions, targetCells, gameboard);
                        
                        if (changed)
                        {
                            return true;
                        }
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
        /// <returns>Boolean: true if any changes were made to existing logic.</returns>
        private bool ClearCellsForMulti(int[] multiSet, Tuple<int, int>[] cells, SudokuMatrix gameboard)
        {
            bool changed = false;

            Debug.WriteLine($"\nHidden {_multi} Targets:");
            Debug.WriteLine($" - set [{string.Join(",", multiSet)}]");

            foreach (Tuple<int, int> cell in cells)
            {
                Debug.WriteLine($" - col {cell.Item1}, row {cell.Item2}");

                changed |= gameboard.SetCellExclusive(cell, multiSet);
            }

            return changed;
        }
    }
}
