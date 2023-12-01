using DokuApp.Model.Builder;
using DokuApp.Model.Data;

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
                    gameboard.Values.SetCell(CellPosition.Index(i), nakedNumber + 1, false);
                }
            }

            // always returns false since it applies all single-candidates at once
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
