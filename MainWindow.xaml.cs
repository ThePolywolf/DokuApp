using DokuApp.Model.Data;
using DokuApp.Model.UI;
using System;
using System.Windows;
using System.Windows.Input;

namespace DokuApp
{
    public partial class MainWindow : Window
    {
        private SudokuMatrix _sudokuMatrix;
        private UserSelection _selection;


        public MainWindow()
        {
            InitializeComponent();

            _sudokuMatrix = new SudokuMatrix();
            SetGrid();

            _selection = new UserSelection();
            SetSelection();
            
            // TODO: Add Mouse clicking to change selection
            // TODO: Add support to set cell values from key-presses and del/backspace
        }

        public void SetGrid()
        {
            FullGrid.Values.SetGrid(_sudokuMatrix.CellData());
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
    }
}
