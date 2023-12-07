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
        /// Solves the gameboard for Naked multi-sets (Pair, Triple, etc.).
        /// </summary>
        /// <param name="gameboard">Gameboard to solve.</param>
        /// <returns>Returns true if is finds a Naked set (changed gameboard).</returns>
        public override bool Solve(SudokuMatrix gameboard)
        {
            List<int[]> allPairingOptions = AllMultiSets(_multi);

            foreach (int[] pairingOption in allPairingOptions)
            {
                List<int> falsePairingSet = new();
                foreach (int number in pairingOption)
                {
                    falsePairingSet.Add(number + 1);
                }
                string pairingSetString = $"[{string.Join(", ", falsePairingSet)}]";

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
                        List<Tuple<int, int>> resultCells = new();

                        for (int iCol = 0; iCol < 9; iCol++)
                        {
                            Tuple<int, int> position = Tuple.Create(iCol, target);
                            
                            // skip safe columns i.e. cells containing the double/triple/etc.
                            if (safeCols.Contains(iCol))
                            {
                                resultCells.Add(position);
                                continue;
                            }

                            targets.Add(position);
                        }

                        bool changeMade = ChangeCells(targets.ToArray(), pairingOption, gameboard);

                        if (changeMade)
                        {
                            _lastChangedCells = LogicBuilder.Cells(resultCells.ToArray());
                            _lastSolutionText = $"{pairingSetString} Naked {_multi}-Set on Row #{target + 1}";
                            return true;
                        }
                    }

                    // column <= target
                    bool[] col = Extractor.LogicalColumn(activeCells, target);
                    if (SetHasMulti(col, out int[] safeRows))
                    {
                        // remove each *triple* number possibility from the non-*triple* cells
                        List<Tuple<int, int>> targets = new();
                        List<Tuple<int, int>> resultCells = new();

                        for (int iRow = 0; iRow < 9; iRow++)
                        {
                            Tuple<int, int> position = Tuple.Create(target, iRow);

                            // skip safe rows
                            if (safeRows.Contains(iRow))
                            {
                                resultCells.Add(position);
                                continue;
                            }

                            targets.Add(position);
                        }

                        bool changeMade = ChangeCells(targets.ToArray(), pairingOption, gameboard);

                        if (changeMade)
                        {
                            _lastChangedCells = LogicBuilder.Cells(resultCells.ToArray());
                            _lastSolutionText = $"{pairingSetString} Naked {_multi}-Set on Col #{target + 1}";
                            return true;
                        }
                    }

                    // box <= target
                    bool[] box = Extractor.LogicalBox(activeCells, target);
                    if (SetHasMulti(box, out int[] safeCells))
                    {
                        // remove each *triple* number possibility from the non-*triple* cells
                        List<Tuple<int, int>> targets = new();
                        List<Tuple<int, int>> resultCells = new();

                        for (int iCell = 0; iCell < 9; iCell++)
                        {
                            Tuple<int, int> position = CellPosition.BoxCell(target, iCell);

                            // skip safe cells
                            if (safeCells.Contains(iCell))
                            {
                                resultCells.Add(position);
                                continue;
                            }

                            targets.Add(position);
                        }

                        bool changeMade = ChangeCells(targets.ToArray(), pairingOption, gameboard);

                        if (changeMade)
                        {
                            _lastChangedCells = LogicBuilder.Cells(resultCells.ToArray());
                            _lastSolutionText = $"{pairingSetString} Naked {_multi}-Set in Box #{target + 1}";
                            return true;
                        }
                    }
                }
            }

            _lastSolutionText = $"No solutions found (Naked {_multi}-Set)";
            return false;
        }

        // change back to static after debugging
        private static bool ChangeCells(Tuple<int, int>[] cells, int[] matchSet, SudokuMatrix gameboard)
        {
            bool change = false;

            foreach (Tuple<int, int> cell in cells)
            {
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
