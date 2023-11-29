using DokuApp.Model.Data;
using DokuApp.Model.UI;
using System.Windows;

namespace DokuApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SudokuMatrix sudokuMatrix = new SudokuMatrix();

            SetGrid(sudokuMatrix.CellData());

            // TODO: Add UserSelection class object
            // TODO: Add arrow-key controls to move UserSelection
            // TODO: Add Mouse clicking to change selection
            // TODO: Add support to set cell values from key-presses and del/backspace
        }

        public void SetGrid(CellData[][] cellData)
        {
            FullGrid.Values.SetGrid(cellData);
        }
    }
}
