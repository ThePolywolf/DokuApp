using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DokuApp.Model.Solver
{
    abstract class NakedSetStrategy : SetStrategy
    {
        /// <summary>
        /// Solves the gameboard for Naked multi-sets (Pair, Triple, etc.)
        /// </summary>
        /// <param name="gameboard">Gameboard to solve.</param>
        /// <returns>Returns true if is finds a Naked set (changed gameboard).</returns>
        public override bool Solve(SudokuMatrix gameboard)
        {
            List<int[]> allPairingOptions = AllMultiSets(_multi);

            foreach (int[] pairingOption in allPairingOptions)
            {
                // Get summed set logic
                LogicMatrix activeCells = Extractor.OverlapOptions(gameboard, pairingOption);
                LogicMatrix exceptActiveCells = Extractor.ExceptOverlapOptions(gameboard, pairingOption);

                // remaingin logic matrix is true cells that only have (pairingOption) options in them
                activeCells.Subtract(exceptActiveCells);

                for (int target = 0; target < 9; target++)
                {
                    // row <= target
                    bool[] row = Extractor.LogicalRow(activeCells, target);
                    if (SetHasMulti(row, out int[] safeCols))
                    {
                        for (int iCol = 0; iCol < 9; iCol++)
                        {
                            // skip safe columns
                            if (safeCols.Contains(iCol))
                            {
                                continue;
                            }

                            // remove each set match number possibility from the other cells
                            foreach (int numberTarget in pairingOption)
                            {
                                Tuple<int, int> position = Tuple.Create(iCol, target);
                                gameboard.Options[numberTarget].SetCell(position, false);
                            }
                        }

                        return true;
                    }

                    // column <= target
                    bool[] col = Extractor.LogicalColumn(activeCells, target);
                    if (SetHasMulti(col, out int[] safeRows))
                    {
                        for (int iRow = 0; iRow < 9; iRow++)
                        {
                            // skip safe rows
                            if (safeRows.Contains(iRow))
                            {
                                continue;
                            }

                            // remove each set match number possibility from the other cells
                            foreach (int numberTarget in pairingOption)
                            {
                                Tuple<int, int> position = Tuple.Create(target, iRow);
                                gameboard.Options[numberTarget].SetCell(position, false);
                            }
                        }

                        return true;
                    }

                    // box <= target
                    bool[] box = Extractor.LogicalBox(activeCells, target);
                    if (SetHasMulti(box, out int[] safeCells))
                    {
                        for (int iCell = 0; iCell < 9; iCell++)
                        {
                            // skip safe cells
                            if (safeCells.Contains(iCell))
                            {
                                continue;
                            }

                            // remove each set match number possibility from the other cells
                            foreach (int numberTarget in pairingOption)
                            {
                                Tuple<int, int> position = CellPosition.BoxCell(target, iCell);
                                gameboard.Options[numberTarget].SetCell(position, false);
                            }
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
