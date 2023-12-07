using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace DokuApp.Model.Solver
{
    abstract class NakedSetStrategy : SetStrategy
    {
        /// <summary>
        /// Solves the gameboard for Naked multi-sets (Pair, Triple, etc.).
        /// </summary>
        /// <param name="gameboard">Gameboard to solve.</param>
        /// <returns>Returns true if is finds a Naked set (changed gameboard).</returns>
        public override bool Solve(SudokuMatrix gameboard)
        {
            List<int[]> allPairingOptions = AllMultiSets(_multi);

            foreach (int[] pairingOption in allPairingOptions)
            {
                // Get summed set logic sets
                LogicMatrix activeCells = Extractor.OverlapOptions(gameboard, pairingOption);
                LogicMatrix exceptActiveCells = Extractor.ExceptOverlapOptions(gameboard, pairingOption);

                // remaining logic matrix is true cells that only have (pairingOption) options in them
                activeCells.Subtract(exceptActiveCells);

                for (int target = 0; target < 9; target++)
                {
                    // row <= target
                    bool[] row = Extractor.LogicalRow(activeCells, target);
                    if (SetHasMulti(row, out int[] safeCols))
                    {
                        // remove each *triple* number possibility from the non-*triple* cells
                        List<Tuple<int, int>> targets = new();

                        for (int iCol = 0; iCol < 9; iCol++)
                        {
                            // skip safe columns i.e. cells containing the double/triple/etc.
                            if (safeCols.Contains(iCol))
                            {
                                continue;
                            }

                            Tuple<int, int> position = Tuple.Create(iCol, target);
                            targets.Add(position);
                        }

                        bool changeMade = ChangeCells(targets.ToArray(), pairingOption, gameboard);

                        if (changeMade)
                        {
                            return true;
                        }
                    }

                    // column <= target
                    bool[] col = Extractor.LogicalColumn(activeCells, target);
                    if (SetHasMulti(col, out int[] safeRows))
                    {
                        // remove each *triple* number possibility from the non-*triple* cells
                        List<Tuple<int, int>> targets = new();

                        for (int iRow = 0; iRow < 9; iRow++)
                        {
                            // skip safe rows
                            if (safeRows.Contains(iRow))
                            {
                                continue;
                            }

                            Tuple<int, int> position = Tuple.Create(target, iRow);
                            targets.Add(position);
                        }

                        bool changeMade = ChangeCells(targets.ToArray(), pairingOption, gameboard);

                        if (changeMade)
                        {
                            return true;
                        }
                    }

                    // box <= target
                    bool[] box = Extractor.LogicalBox(activeCells, target);
                    if (SetHasMulti(box, out int[] safeCells))
                    {
                        // remove each *triple* number possibility from the non-*triple* cells
                        List<Tuple<int, int>> targets = new();

                        for (int iCell = 0; iCell < 9; iCell++)
                        {
                            // skip safe cells
                            if (safeCells.Contains(iCell))
                            {
                                continue;
                            }

                            Tuple<int, int> position = CellPosition.BoxCell(target, iCell);
                            targets.Add(position);
                        }

                        bool changeMade = ChangeCells(targets.ToArray(), pairingOption, gameboard);

                        if (changeMade)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // change back to static after debugging
        private bool ChangeCells(Tuple<int, int>[] cells, int[] matchSet, SudokuMatrix gameboard)
        {
            bool change = false;

            Debug.WriteLine($"\nNaked {_multi} Targets:");

            foreach (Tuple<int, int> cell in cells)
            {
                Debug.WriteLine($" - col {cell.Item1}, row {cell.Item2}");

                // remove all the numbers in the *triple* from non-*triple* cells
                foreach (int matchNumber in matchSet)
                {
                    change |= gameboard.Options[matchNumber].SetCell(cell, false);
                }
            }

            return change;
        }
    }
}
