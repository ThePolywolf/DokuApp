using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;
using System.Collections.Generic;

namespace DokuApp.Model.Solver
{
    class SudokuStrategy : Strategy
    {
        public SudokuStrategy() { }

        protected override string GetName()
        {
            return "Sudoku";
        }

        /// <summary>
        /// Applies sudoku rules to narrow possibilities down.
        /// </summary>
        /// <param name="gameboard">Sudoku matrix. Gets editied during the Solve.</param>
        /// <returns>Always returns false</returns>
        public override bool Solve(SudokuMatrix gameboard)
        {
            LogicMatrix valueLogic = gameboard.Values.AsLogic();
            NumericalMatrix values = gameboard.Values;

            // loop through each number for sudoku
            for (int target = 0; target < 9; target++)
            {
                // Matrix Setup
                LogicMatrix board = gameboard.Options[target];
                board.Subtract(valueLogic);

                List<Tuple<int, int>> exclusionCells = new();

                for (int col = 0; col < 9; col++)
                {
                    for (int row = 0; row < 9; row++)
                    {
                        if (values.Matrix[col, row] == target + 1)
                        {
                            exclusionCells.Add(Tuple.Create(col, row));
                        }
                    }
                }

                LogicMatrix numberExclusions = LogicBuilder.CellExclusions(exclusionCells.ToArray());
                board.Subtract(numberExclusions);

                // resave gameboard
                gameboard.SetOption(board, target);
            }

            return false;
        }
    }
}
