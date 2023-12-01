using DokuApp.Model.Data;

namespace DokuApp.Model.Solver
{
    internal class HiddenPairStrategy : HiddenSetStrategy
    {
        protected override string GetName()
        {
            return "Hidden Pairs";
        }

        public override bool Solve(SudokuMatrix gameboard)
        {
            return HiddenMultiSolution(2, gameboard);
        }
    }
}
