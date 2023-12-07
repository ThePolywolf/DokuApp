using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;

namespace DokuApp.Model.Solver
{
    class NakedSinglesStrategy : Strategy
    {
        public NakedSinglesStrategy()
        {
            return;
        }

        protected override string GetName()
        {
            return "Naked Singles";
        }
        
        public override bool Solve(SudokuMatrix gameboard)
        {
            for (int i = 0; i < 81; i++)
            {
                bool[] possibilities = Extractor.LogicalPossibilities(gameboard.Options, i);

                if (CellHasSingle(possibilities, out int nakedNumber))
                {
                    Tuple<int, int> position = CellPosition.Index(i);
                    gameboard.Values.SetCell(position, nakedNumber + 1, false);

                    _lastChangedCells = LogicBuilder.Cell(position);
                    _lastSolutionText = $"{nakedNumber + 1} Naked Single";
                    return true;
                }
            }

            _lastChangedCells = new();
            _lastSolutionText = $"No solutions found (Naked Singles)";
            return false;
        }

        private static bool CellHasSingle(bool[] values, out int nakedNumber)
        {
            nakedNumber = 0;
            int truthCount = 0;

            for (int i = 0; i < 9; i++)
            {
                if (values[i])
                {
                    nakedNumber = i;
                    truthCount++;
                }
            }

            if (truthCount == 1)
            {
                return true;
            }

            return false;
        }
    }
}
