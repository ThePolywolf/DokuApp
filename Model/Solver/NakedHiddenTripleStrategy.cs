using DokuApp.Model.Data;

namespace DokuApp.Model.Solver
{
    internal class NakedHiddenTripleStrategy : NakedHiddenSetStrategy
    {
        protected override string GetName()
        {
            return "Triples";
        }

        public override bool Solve(SudokuMatrix gameboard)
        {
            return MultiSolution(3, gameboard);
        }
    }
}