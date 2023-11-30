using DokuApp.Model.Data;
using DokuApp.Model.Builder;

namespace DokuApp.Model.Solver
{
    abstract class Strategy
    {
        abstract public bool Solve(SudokuMatrix gameboard);
    }
}
