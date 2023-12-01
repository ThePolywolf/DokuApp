using DokuApp.Model.Data;

namespace DokuApp.Model.Solver
{
    internal class NakedHiddenPairStrategy : NakedHiddenSetStrategy
    {
        protected override string GetName()
        {
            return "Pairs";
        }

        public override bool Solve(SudokuMatrix gameboard)
        {
            return MultiSolution(2, gameboard);
        }
    }
}
