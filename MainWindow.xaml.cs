using DokuApp.Model.Data;
using DokuApp.Model.Solver;
using DokuApp.Model.Builder;
using System;
using System.Windows;
using System.Windows.Input;

namespace DokuApp
{
    public partial class MainWindow : Window
    {
        private SudokuMatrix _sudokuMatrix;
        private LogicMatrix _numberPermenance;
        private UserSelection _selection;
        private NumericErrors _numericErrors;

        private bool _permenantEntry;

        public MainWindow()
        {
            InitializeComponent();

            FullGrid.MouseSelection += GridClicked;

            _sudokuMatrix = new SudokuMatrix();
            _numberPermenance = new LogicMatrix();
            _numericErrors = new NumericErrors();
            SetGrid();

            _selection = new UserSelection();
            SetSelection();

            Entries.NumberRecieved += ManualNumberInput;
            
            _permenantEntry = true;
            Entries.NewEntryMode += ChangeEntryMode;
        }

        public void SetGrid()
        {
            FullGrid.Values.SetGrid(_sudokuMatrix.CellData());

            LogicMatrix errors = _numericErrors.FindErrors(_sudokuMatrix.Values);
            FullGrid.Errors.SetErrorCells(errors);
        }

        public void SetSelection()
        {
            FullGrid.Selection.SelectSquare(_selection.SingleSelection);
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            // cascade SHIFT changes
            Entries.Shift(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift));

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
                if (!_permenantEntry)
                {
                    if (_numberPermenance.Truths[position.Item1, position.Item2])
                    {
                        return;
                    }
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
            if (!_permenantEntry)
            {
                if (_numberPermenance.Truths[position.Item1, position.Item2])
                {
                    return;
                }
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

            _sudokuMatrix.Values.SetCell(position, number);
            if (_permenantEntry) _numberPermenance.Add(LogicBuilder.Cell(position));
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

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            // cascade SHIFT changes
            Entries.Shift(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift));
        }
    }
}
