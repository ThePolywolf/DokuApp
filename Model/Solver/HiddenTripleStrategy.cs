using DokuApp.Model.Data;

namespace DokuApp.Model.Solver
{
    internal class HiddenTripleStrategy : HiddenSetStrategy
    {
        protected override string GetName()
        {
            return "Hidden Triples";
        }

        public override bool Solve(SudokuMatrix gameboard)
        {
            return HiddenMultiSolution(3, gameboard);
        }
    }
}