﻿using DokuApp.Model.Data;
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

        private int _solutionStep;
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
            _window.WindowStepSolution += StepSolution;
            _window.WindowTotalClearGrid += TotalClearGrid;
            _window.WindowClearNumberGrid += ClearNumberGrid;
            _window.WindowClearPossibilitiesGrid += ClearPossibilitiesGrid;
            _window.WindowMarkCorners += MarkCorners;

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
                new NakedPairStrategy(),
                new HiddenPairStrategy(),
                new NakedTripleStrategy(),
                new HiddenTripleStrategy(),
                new NakedQuadStrategy(),
                new HiddenQuadStrategy(),
            };

            // set current step to -1
            _solutionStep = -1;
            SetRecentStrategyText("", "");
        }

        public void SetGrid(LogicMatrix? solutionChanges = null)
        {
            _window.FullGrid.Values.SetGrid(_sudokuMatrix.CellData());

            LogicMatrix errors = NumericErrors.FindErrors(_sudokuMatrix.Values);
            _window.FullGrid.Errors.SetErrorCells(errors);

            if (solutionChanges == null)
            {
                solutionChanges = new();
            }
            _window.FullGrid.SolutionChanges.SetChangesGrid(solutionChanges.Truths);
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
                _solutionStep = -1;

                Tuple<int, int> position = _selection.SingleSelection;

                // if not in permenance mode, can't delete a permenant cell
                if (!_permenantEntry && _sudokuMatrix.Values.CellIsPermenant(position))
                {
                    return;
                }

                bool cellCleared = _sudokuMatrix.Values.DeleteCell(position);
                
                if (!cellCleared)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        _sudokuMatrix.Options[i].SetCell(position, false);
                    }
                }
                
                SetGrid();

                return;
            }
        }

        private void SetCellValue(int number)
        {
            _solutionStep = -1;

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
            _solutionStep = -1;

            SolverSetup.StartingSudokuLogic(_sudokuMatrix);

            // Loops through each strategy, and on a succesful attempt restarts from the beginning.
            int i = 0;
            int count = 0;
            while (i < _strategies.Length)
            {
                count++;

                if (count > 10000)
                {
                    break;
                }

                bool result = _strategies[i].Solve(_sudokuMatrix);
                if (result)
                {
                    SetRecentStrategyText($"Solve - {_strategies[i].Name} @count-{count}");

                    i = 0;
                    continue;
                }

                i++;
            }

            SetGrid();
        }

        private void StepSolution(object? sender, RoutedEventArgs e)
        {
            LogicMatrix changedCells = new();

            if (_solutionStep < 0)
            {
                SolverSetup.StartingSudokuLogic(_sudokuMatrix);
                _solutionStep = 0;
            }

            // Step to next strategy solve (if any)
            for (int i = 0; i < _strategies.Length + 1; i++)
            {
                bool result = _strategies[_solutionStep].Solve(_sudokuMatrix);

                if (result)
                {
                    SetRecentStrategyText($"Solve Found - {_strategies[_solutionStep].LastSolutionText}");
                    changedCells = _strategies[_solutionStep].LastChangedCells;

                    _solutionStep = 0;
                    break;
                }

                _solutionStep++;

                if (_solutionStep >= _strategies.Length)
                {
                    // next step resets from beginning
                    _solutionStep = -1;

                    break;
                }
            }

            // re-sudoku board for accurate numbers
            _strategies[0].Solve(_sudokuMatrix);

            SetGrid(changedCells);
        }

        private void ClearNumberGrid(object? sender, RoutedEventArgs e)
        {
            _solutionStep = -1;

            _sudokuMatrix.Values.ClearImpermenantNumbers();

            SetGrid();
        }

        private void ClearPossibilitiesGrid(object? sender, RoutedEventArgs e)
        {
            _solutionStep = -1;

            for (int i = 0; i < 9; i++)
            {
                _sudokuMatrix.Options[i] = new LogicMatrix();
            }

            SetGrid();
        }

        private void ClearGrid(object? sender, RoutedEventArgs e)
        {
            _solutionStep = -1;

            ClearNumberGrid(sender, e);
            ClearPossibilitiesGrid(sender, e);

            SetGrid();
        }

        private void TotalClearGrid(object? sender, RoutedEventArgs e)
        {
            _solutionStep = -1;

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

        private void SetRecentStrategyText(string newText, string prefix = "> ")
        {
            _window.LastSolutionText.Text = prefix + newText;
        }
    }
}
