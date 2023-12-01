using DokuApp.Model.Data;
using DokuApp.Model.Solver;
using System.Windows.Input;
using System.Windows;
using System;

namespace DokuApp.Model.UI
{
    class MainWindowMVVM
    {
        private readonly MainWindow _window;

        private readonly SudokuMatrix _sudokuMatrix;
        private readonly UserSelection _selection;

        private readonly Strategy[] _strategies;

        private bool _permenantEntry;

        public MainWindowMVVM(MainWindow mainWindow)
        {
            // set reference
            _window = mainWindow;

            // attach events
            _window.WindowKeyDown += KeyDown;
            _window.WindowKeyUp += KeyUp;
            _window.WindowSolveGrid += SolveGrid;
            _window.WindowClearGrid += ClearGrid;
            _window.WindowTotalClearGrid += TotalClearGrid;
            _window.WindowClearNumberGrid += ClearNumberGrid;
            _window.WindowClearPossibilitiesGrid += ClearPossibilitiesGrid;
            _window.WindowsMarkCorners += MarkCorners;

            // attach main window events
            _window.FullGrid.MouseSelection += GridClicked;
            _window.Entries.NumberRecieved += ManualNumberInput;
            _permenantEntry = true;
            _window.Entries.NewEntryMode += ChangeEntryMode;

            // setup sudoku grid
            _sudokuMatrix = new SudokuMatrix();
            SetGrid();

            // set up selection
            _selection = new UserSelection();
            SetSelection();

            // set up strategies
            _strategies = new Strategy[] {
                new SudokuStrategy(),
                new NakedSinglesStrategy(),
                new SinglesStrategy(),
                new PointingDoubleTripleStrategy(),
            };
        }

        public void SetGrid()
        {
            _window.FullGrid.Values.SetGrid(_sudokuMatrix.CellData());

            LogicMatrix errors = NumericErrors.FindErrors(_sudokuMatrix.Values);
            _window.FullGrid.Errors.SetErrorCells(errors);
        }

        public void SetSelection()
        {
            _window.FullGrid.Selection.SelectSquare(_selection.SingleSelection);
        }

        private void KeyDown(object? sender, KeyEventArgs e)
        {
            // cascade SHIFT changes
            _window.Entries.Shift(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift));

            if (e.Key == Key.Down)
            {
                _selection.Down();
                SetSelection();
                return;
            }

            if (e.Key == Key.Up)
            {
                _selection.Up();
                SetSelection();
                return;
            }

            if (e.Key == Key.Right)
            {
                _selection.Right();
                SetSelection();
                return;
            }

            if (e.Key == Key.Left)
            {
                _selection.Left();
                SetSelection();
                return;
            }

            if (e.Key >= Key.D1 && e.Key <= Key.D9)
            {
                int numberPressed = e.Key - Key.D0;
                SetCellValue(numberPressed);

                return;
            }

            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                Tuple<int, int> position = _selection.SingleSelection;

                // if not in permenance mode, can't delete a permenant cell
                if (!_permenantEntry && _sudokuMatrix.Values.CellIsPermenant(position))
                {
                    return;
                }

                _sudokuMatrix.Values.DeleteCell(position);
                SetGrid();

                return;
            }
        }

        private void SetCellValue(int number)
        {
            if (Math.Clamp(number, 1, 9) != number)
            {
                return;
            }

            Tuple<int, int> position = _selection.SingleSelection;

            // can only change non-permenant cells if not in permenance mode
            if (!_permenantEntry && _sudokuMatrix.Values.CellIsPermenant(position))
            {
                return;
            }

            // targets corner possibilities if shifted
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                // only set corner values if cell is empty
                if (_sudokuMatrix.Values.Matrix[position.Item1, position.Item2] != 0)
                {
                    return;
                }

                _sudokuMatrix.AddCorner(position, number);
                SetGrid();

                return;
            }

            _sudokuMatrix.Values.SetCell(position, number, _permenantEntry);

            SetGrid();

            return;
        }

        private void GridClicked(object? sender, Tuple<int, int> selectedCell)
        {
            _selection.SingleSelect(selectedCell);
            SetSelection();
        }

        private void ManualNumberInput(object? sender, int number)
        {
            SetCellValue(number);
        }

        private void ChangeEntryMode(object? sender, bool newMode)
        {
            _permenantEntry = newMode;
        }

        private void KeyUp(object? sender, KeyEventArgs e)
        {
            // cascade SHIFT changes
            _window.Entries.Shift(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift));
        }

        private void SolveGrid(object? sender, RoutedEventArgs e)
        {
            SolverSetup.StartingSudokuLogic(_sudokuMatrix);

            // Loops through each strategy, and on a succesful attempt restarts from the beginning.
            int i = 0;
            while (i < _strategies.Length)
            {
                bool result = _strategies[i].Solve(_sudokuMatrix);
                if (result)
                {
                    SetGrid();
                    i = 0;
                    continue;
                }

                i++;
            }
        }

        private void ClearNumberGrid(object? sender, RoutedEventArgs e)
        {
            _sudokuMatrix.Values.ClearImpermenantNumbers();

            SetGrid();
        }

        private void ClearPossibilitiesGrid(object? sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                _sudokuMatrix.Options[i] = new LogicMatrix();
            }

            SetGrid();
        }

        private void ClearGrid(object? sender, RoutedEventArgs e)
        {
            ClearNumberGrid(sender, e);
            ClearPossibilitiesGrid(sender, e);

            SetGrid();
        }

        private void TotalClearGrid(object? sender, RoutedEventArgs e)
        {
            _sudokuMatrix.Values.Reset();
            ClearPossibilitiesGrid(sender, e);

            SetGrid();
        }

        private void MarkCorners(object? sender, RoutedEventArgs e)
        {
            SolverSetup.StartingSudokuLogic(_sudokuMatrix);
            Strategy strategy = new SudokuStrategy();
            strategy.Solve(_sudokuMatrix);

            SetGrid();
        }
    }
}
