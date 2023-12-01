using DokuApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DokuApp.Model.Solver
{
    internal class HiddenQuadStrategy : HiddenSetStrategy
    {
        protected override string GetName()
        {
            return "Hidden Quads";
        }

        public override bool Solve(SudokuMatrix gameboard)
        {
            return HiddenMultiSolution(4, gameboard);
        }
    }
}
