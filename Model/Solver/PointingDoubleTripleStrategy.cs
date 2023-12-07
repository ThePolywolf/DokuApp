using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DokuApp.Model.Solver
{
    class PointingDoubleTripleStrategy : Strategy
    {
        public PointingDoubleTripleStrategy() 
        {
            return;
        }

        protected override string GetName()
        {
            return "Pointing Double/Triple";
        }

        public override bool Solve(SudokuMatrix gameboard)
        {
            for (int target = 0; target < 9; target++)
            {
                LogicMatrix board = gameboard.Options[target];

                for (int box = 0; box < 9; box++)
                {
                    // find row / column doubles in box
                    bool[] boxValues = Extractor.LogicalBox(board, box);

                    HashSet<int> activeRows = new();
                    HashSet<int> activeCols = new();

                    for (int cell = 0; cell < 9; cell++)
                    {
                        if (boxValues[cell])
                        {
                            (int col, int row) = CellPosition.BoxCell(box, cell);

                            activeRows.Add(row);
                            activeCols.Add(col);
                        }
                    }

                    // check for single rows / columns
                    if (activeRows.Count == 1)
                    {
                        // only double if more than one active column
                        if (activeCols.Count <= 1)
                        {
                            continue;
                        }

                        int row = activeRows.First();
                        bool[] rowSet = Extractor.LogicalRow(board, row);
                        int boxFactor = box % 3;

                        if (SetClearable(rowSet, boxFactor))
                        {
                            LogicMatrix removal = LogicBuilder.Row(row).Subtract(LogicBuilder.Box(box));

                            board.Subtract(removal);

                            gameboard.SetOption(board, target);

                            List<Tuple<int, int>> cells = new();
                            foreach (int col in activeCols)
                            {
                                cells.Add(Tuple.Create(col, row));
                            }
                            _lastChangedCells = LogicBuilder.Cells(cells.ToArray());
                            string type;
                            if (activeCols.Count == 2)
                            {
                                type = "Pair";
                            }
                            else
                            {
                                type = "Triple";
                            }
                            _lastSolutionText = $"{target + 1} Pointing-{type} in Box #{box + 1}, Row #{row + 1}";
                            return true;
                        }
                    }

                    if (activeCols.Count == 1)
                    {
                        // only double if more than one active row
                        if (activeRows.Count <= 1)
                        {
                            continue;
                        }

                        int col = activeCols.First();
                        bool[] colSet = Extractor.LogicalColumn(board, col);
                        int boxFactor = (box - (box % 3)) / 3;

                        if (SetClearable(colSet, boxFactor))
                        {
                            // remove column except box
                            LogicMatrix removal = LogicBuilder.Column(col).Subtract(LogicBuilder.Box(box));

                            board.Subtract(removal);

                            gameboard.SetOption(board, target);

                            List<Tuple<int, int>> cells = new();
                            foreach (int row in activeRows)
                            {
                                cells.Add(Tuple.Create(col, row));
                            }
                            _lastChangedCells = LogicBuilder.Cells(cells.ToArray());
                            string type;
                            if (activeRows.Count == 2)
                            {
                                type = "Pair";
                            }
                            else
                            {
                                type = "Triple";
                            }
                            _lastSolutionText = $"{target + 1} Pointing-{type} in Box #{box + 1}, Column #{col + 1}";
                            return true;
                        }
                    }
                }
            }

            _lastChangedCells = new();
            _lastSolutionText = $"No solutions found (Pointing Pairs)";
            return false;
        }

        /// <summary>
        /// Checks if there are any values to be cleared outside of the box.
        /// </summary>
        /// <param name="set">The set to check for the possibility of clearing.</param>
        /// <param name="boxFactor">The group to ignore. 0 <= BoxFactor < 3.</param>
        /// <returns>Returns true if there are true values outside of the boxFactors range.</returns>
        private static bool SetClearable(bool[] set, int boxFactor)
        {
            for (int cell = 0; cell < 9; cell++)
            {
                // box factor - which set of 3 it's part of
                int cellBoxFactor = (cell - (cell % 3)) / 3;

                if (cellBoxFactor == boxFactor)
                {
                    continue;
                }

                if (set[cell])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
