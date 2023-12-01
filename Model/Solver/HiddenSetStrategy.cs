using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DokuApp.Model.Solver
{
    abstract class HiddenSetStrategy : Strategy
    {
        /// <summary>
        /// Solves the gameboard for a specified Hidden multi-sets (pair, triple, etc.).
        /// </summary>
        /// <param name="multi">Multi match to target (pair, triple, etc.)</param>
        /// <param name="gameboard">Gameboard to ater.</param>
        /// <returns>Only true if a multi-match was found (gameboard was edited).</returns>
        protected bool HiddenMultiSolution(int multi, SudokuMatrix gameboard)
        {
            List<int[]> AllPairingOptions = AllMultiSets(multi);

            foreach (int[] pairingOptions in AllPairingOptions)
            {
                LogicMatrix overlappedLogic = gameboard.OverlapOptions(pairingOptions);

                for (int target = 0; target < 9; target++)
                {
                    // rows
                    bool[] row = Extractor.LogicalRow(overlappedLogic, target);
                    if (SetHasMulti(row, multi, out int[] rowCells))
                    {
                        Tuple<int, int>[] targetCells = new Tuple<int, int>[multi];

                        for (int i = 0; i < multi; i++)
                        {
                            targetCells[i] = Tuple.Create(rowCells[i], target);
                        }

                        ClearCellsForMulti(pairingOptions, targetCells, gameboard);
                        return true;
                    }

                    // columns
                    bool[] col = Extractor.LogicalColumn(overlappedLogic, target);
                    if (SetHasMulti(col, multi, out int[] colCells))
                    {
                        Tuple<int, int>[] targetCells = new Tuple<int, int>[multi];

                        for (int i = 0; i < multi; i++)
                        {
                            targetCells[i] = Tuple.Create(target, colCells[i]);
                        }

                        ClearCellsForMulti(pairingOptions, targetCells, gameboard);
                        return true;
                    }

                    // boxes
                    bool[] box = Extractor.LogicalBox(overlappedLogic, target);
                    if (SetHasMulti(box, multi, out int[] boxCells))
                    {
                        Tuple<int, int>[] targetCells = new Tuple<int, int>[multi];

                        for (int i = 0; i < multi; i++)
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
        /// Returns all unordered uniqued sets of length (multi) from the set 0 to 8.
        /// </summary>
        /// <param name="multi">Unique set length.</param>
        /// <returns>Hash set of arrays of all unique unordered sets from the set [0, 1, 2, 3, 4, 5, 6, 7, 8].</returns>
        private List<int[]> AllMultiSets(int multi)
        {
            List<int[]> sets = new();

            if (multi > 9 || multi < 1)
            {
                return sets;
            }

            // create probes
            int[] probes = new int[multi];

            for (int i = 0; i < multi; i++)
            {
                probes[i] = i;
            }

            // initialize first Hash
            sets.Add(probes);
            // select last probe first
            int probeTarget = multi - 1;

            while (true)
            {
                // break if out-of-range
                if (probeTarget < 0)
                {
                    break;
                }

                // increment probe
                probes[probeTarget] += 1;

                // ascend probe ladder if current probe is out-of-range
                int maxProbePosition = 8 - ((multi - 1) - probeTarget);
                if (probes[probeTarget] > maxProbePosition)
                {
                    probeTarget -= 1;
                    continue;
                }

                // cascade probes forward from current probeTarget
                for(int target = probeTarget + 1; target < multi; target++)
                {
                    probes[target] = probes[probeTarget] + (target - probeTarget);
                }

                // add probe values
                sets.Add(probes);

                // re-target last probe
                probeTarget = multi - 1;
            }

            return sets;
        }

        /// <summary>
        /// Checks for multis (pairs, triples, etc) Inside the set.
        /// </summary>
        /// <param name="set">Set to check (length must be 9).</param>
        /// <param name="multi">Match type (pair, triple, etc.).</param>
        /// <param name="cells">All cells in the set that are members of the match.</param>
        /// <returns>True only if set contains multi-match.</returns>
        private bool SetHasMulti(bool[] set, int multi, out int[] cells)
        {
            List<int> rawCells = new();

            for (int i = 0; i < 9; i++)
            {
                if (set[i])
                {
                    rawCells.Add(i);
                }
            }

            if (rawCells.Count == multi)
            {
                cells = rawCells.ToArray();
                return true;
            }

            cells = Array.Empty<int>();
            return false;
        }

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
