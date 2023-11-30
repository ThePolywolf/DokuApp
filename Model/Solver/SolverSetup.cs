using DokuApp.Model.Builder;
using DokuApp.Model.Data;

namespace DokuApp.Model.Solver
{
    class SolverSetup
    {
        public static void StartingSudokuLogic(SudokuMatrix gameboard)
        {
            for (int i = 0; i < 9; i++)
            {
                gameboard.Options[i] = LogicBuilder.All(true);
            }

            SudokuStrategy sudokuStrategy = new();
            sudokuStrategy.Solve(gameboard);
        }
    }
}
