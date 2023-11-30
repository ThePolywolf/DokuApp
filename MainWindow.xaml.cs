using DokuApp.Model.Data;
using DokuApp.Model.Solver;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DokuApp
{
    public partial class MainWindow : Window
    {
        private SudokuMatrix _sudokuMatrix;
        private UserSelection _selection;
        private NumericErrors _numericErrors;


        public MainWindow()
        {
            InitializeComponent();

            _sudokuMatrix = new SudokuMatrix();
            _numericErrors = new NumericErrors();
            SetGrid();

            _selection = new UserSelection();
            SetSelection();
            
            // TODO: Add Mouse clicking to change selection
            // TODO: Add support to set cell values from key-presses and del/backspace
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
                Tuple<int, int> position = _selection.SingleSelection;
                
                // targets corner possibilities if shifted
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    // only set corner values if cell is empty
                    if (_sudokuMatrix.Values.Matrix[position.Item1, position.Item2] != 0)
                    {
                        return;
                    }

                    _sudokuMatrix.AddCorner(position, numberPressed);
                    SetGrid();

                    return;
                }
                
                _sudokuMatrix.Values.SetCell(position, numberPressed);
                SetGrid();

                return;
            }

            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                Tuple<int, int> position = _selection.SingleSelection;

                _sudokuMatrix.Values.DeleteCell(position);
                SetGrid();
            }
        }

        private void GridClicked(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(FullGrid);

            double percentageX = (mousePosition.X / FullGrid.Width);
            double percentageY = (mousePosition.Y / FullGrid.Height);

            Debug.WriteLine($"X: {percentageX} Y: {percentageY}");

            int row = (int)Math.Floor(percentageY * 9);
            int column = (int)Math.Floor(percentageX * 9);

            if (Math.Clamp(row, 0, 8) != row || Math.Clamp(column, 0, 8) != column)
            {
                return;
            }

            _selection.SingleSelect(Tuple.Create(column, row));
            SetSelection();
        }
    }
}
