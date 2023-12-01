using System;
using System.Collections.Generic;

namespace DokuApp.Model.Solver
{
    abstract class SetStrategy : Strategy
    {
        protected int _multi;

        /// <summary>
        /// Returns all unordered uniqued sets of length (multi) from the set 0 to 8.
        /// </summary>
        /// <param name="multi">Unique set length.</param>
        /// <returns>Hash set of arrays of all unique unordered sets from the set [0, 1, 2, 3, 4, 5, 6, 7, 8].</returns>
        protected List<int[]> AllMultiSets(int multi)
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
                for (int target = probeTarget + 1; target < multi; target++)
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
        /// <param name="cells">All cells in the set that are members of the match.</param>
        /// <returns>True only if set contains multi-match.</returns>
        protected bool SetHasMulti(bool[] set, out int[] cells)
        {
            List<int> rawCells = new();

            for (int i = 0; i < 9; i++)
            {
                if (set[i])
                {
                    rawCells.Add(i);
                }
            }

            if (rawCells.Count == _multi)
            {
                cells = rawCells.ToArray();
                return true;
            }

            cells = Array.Empty<int>();
            return false;
        }
    }
}
