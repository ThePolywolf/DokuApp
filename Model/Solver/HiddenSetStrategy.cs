using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;
using System.Collections.Generic;

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
                List<int> falsePairingSet = new();
                foreach (int number in pairingOptions)
                {
                    falsePairingSet.Add(number + 1);
                }
                string pairingSetString = $"[{string.Join(", ", falsePairingSet)}]";

                // pull out summed matrix --> box true if it contains any of the numbers part of the *triple*
                LogicMatrix overlappedLogic = Extractor.OverlapOptions(gameboard, pairingOptions);

                // loop through [row, column, box] 1-9 (0-8)
                for (int target = 0; target < 9; target++)
                {
                    // row <-- target
                    bool[] row = Extractor.LogicalRow(overlappedLogic, target);
                    if (SetHasMulti(row, out int[] rowCells))
                    {
                        List<Tuple<int, int>> targetCells = new();

                        foreach (int rowTarget in rowCells)
                        {
                            Tuple<int, int> position = Tuple.Create(rowTarget, target);
                            targetCells.Add(position);
                        }

                        bool changed = ClearCellsForMulti(pairingOptions, targetCells.ToArray(), gameboard);
                        
                        if (changed)
                        {
                            _lastChangedCells = LogicBuilder.Cells(targetCells.ToArray());
                            _lastSolutionText = $"{pairingSetString} Hidden {_multi}-Set on Row #{target + 1}";
                            return true;
                        }
                    }

                    // column <-- target
                    bool[] col = Extractor.LogicalColumn(overlappedLogic, target);
                    if (SetHasMulti(col, out int[] colCells))
                    {
                        List<Tuple<int, int>> targetCells = new();

                        foreach (int colTarget in colCells)
                        {
                            Tuple<int, int> position = Tuple.Create(target, colTarget);
                            targetCells.Add(position);
                        }

                        bool changed = ClearCellsForMulti(pairingOptions, targetCells.ToArray(), gameboard);
                        
                        if (changed)
                        {
                            _lastChangedCells = LogicBuilder.Cells(targetCells.ToArray());
                            _lastSolutionText = $"{pairingSetString} Hidden {_multi}-Set on Column #{target + 1}";
                            return true;
                        }
                    }

                    // boxes <-- target
                    bool[] box = Extractor.LogicalBox(overlappedLogic, target);
                    if (SetHasMulti(box, out int[] boxCells))
                    {
                        List<Tuple<int, int>> targetCells = new();

                        foreach (int cellTarget in boxCells)
                        {
                            Tuple<int, int> position = CellPosition.BoxCell(target, cellTarget);
                            targetCells.Add(position);
                        }

                        bool changed = ClearCellsForMulti(pairingOptions, targetCells.ToArray(), gameboard);
                        
                        if (changed)
                        {
                            _lastChangedCells = LogicBuilder.Cells(targetCells.ToArray());
                            _lastSolutionText = $"{pairingSetString} Hidden {_multi}-Set in Box #{target + 1}";
                            return true;
                        }
                    }
                }
            }

            _lastSolutionText = $"No solutions found (Hidden {_multi}-Set)";
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

            foreach (Tuple<int, int> cell in cells)
            {
                changed |= gameboard.SetCellExclusive(cell, multiSet);
            }

            return changed;
        }
    }
}
