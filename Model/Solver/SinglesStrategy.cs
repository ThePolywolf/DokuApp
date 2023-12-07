using DokuApp.Model.Builder;
using DokuApp.Model.Data;
using System;

namespace DokuApp.Model.Solver
{
    class SinglesStrategy : Strategy
    {
        public SinglesStrategy()
        {
            return;
        }

        public override bool Solve(SudokuMatrix gameboard)
        {
            for (int target = 0; target < 9; target++)
            {
                LogicMatrix board = gameboard.Options[target];

                // loop through boxes
                for (int i = 0; i < 9; i++)
                {
                    // box <= i
                    bool[] boxValues = Extractor.LogicalBox(board, i);

                    if (SetContainsSingle(boxValues, out int lastCell))
                    {
                        Tuple<int, int> position = CellPosition.BoxCell(i, lastCell);

                        // set number on NumericalMatrix
                        gameboard.Values.SetCell(position, target + 1, false);

                        // exclude that cell from possiblities
                        board.Subtract(LogicBuilder.CellExclusion(position));
                        gameboard.SetOption(board, target);

                        _lastChangedCells = LogicBuilder.Cell(position);
                        _lastSolutionText = $"{target + 1} Hidden Single in Box #{i + 1}";
                        return true;
                    }

                    // row <= i
                    bool[] rowValues = Extractor.LogicalRow(board, i);

                    if (SetContainsSingle(rowValues, out int lastCol))
                    {
                        Tuple<int, int> position = Tuple.Create(lastCol, i);

                        // set number on NumericalMatrix
                        gameboard.Values.SetCell(position, target + 1, false);

                        // exclude that cell from possiblities
                        board.Subtract(LogicBuilder.CellExclusion(position));
                        gameboard.SetOption(board, target);

                        _lastChangedCells = LogicBuilder.Cell(position);
                        _lastSolutionText = $"{target + 1} Hidden Single in Row #{i + 1}";
                        return true;
                    }
                    
                    // column <= i
                    bool[] possibilities = Extractor.LogicalColumn(board, i);

                    if (SetContainsSingle(possibilities, out int lastRow))
                    {
                        Tuple<int, int> position = Tuple.Create(i, lastRow);

                        // set number on NumericalMatrix
                        gameboard.Values.SetCell(position, target + 1, false);

                        // exclude that cell from possiblities
                        board.Subtract(LogicBuilder.CellExclusion(position));
                        gameboard.SetOption(board, target);

                        _lastChangedCells = LogicBuilder.Cell(position);
                        _lastSolutionText = $"{target + 1} Hidden Single in Column #{i + 1}";
                        return true;
                    }
                }
            }

            _lastChangedCells = new();
            _lastSolutionText = $"No solutions found (Hidden Single)";
            return false;
        }

        protected override string GetName()
        {
            return "Singles";
        }

        private static bool SetContainsSingle(bool[] set, out int lastTruth)
        {
            lastTruth = 0;
            int countedOptions = 0;

            for (int cell = 0; cell < 9; cell++)
            {
                if (set[cell])
                {
                    countedOptions++;
                    lastTruth = cell;
                }
            }

            if (countedOptions == 1)
            {
                return true;
            }

            return false;
        }
    }
}
