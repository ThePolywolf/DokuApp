﻿using DokuApp.Model.Data;

namespace DokuApp.Model.Solver
{
    public abstract class Strategy
    {
        public string Name { get { return $"{GetName()} Strategy"; } }

        protected LogicMatrix _lastChangedCells;
        public LogicMatrix LastChangedCells { get { return _lastChangedCells; } }

        protected string _lastSolutionText;
        public string LastSolutionText { get { return _lastSolutionText; } }

        /// <summary>
        /// Applies a strategy to the gameboard to narrow down possibilities.
        /// </summary>
        /// <param name="gameboard">Gameboard to reference and edit.</param>
        /// <returns>True: A change was made, False: No changes were made</returns>
        public abstract bool Solve(SudokuMatrix gameboard);

        protected abstract string GetName();
    }
}
