using DokuApp.Model.Data;
using System.Windows.Controls;

namespace DokuApp.View
{
    public partial class ErrorGrid : UserControl
    {
        public ErrorGrid()
        {
            InitializeComponent();
        }

        public void SetErrorCells(LogicMatrix? cells)
        {
            // clear ErrorGrid of errorBox children
            EGrid.Children.Clear();

            if (cells == null)
            {
                return;
            }

            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 9; row++)
                {
                    if (cells.Truths[col, row])
                    {
                        ErrorBox errorBox = new ErrorBox();

                        // set grid position -- Skips Spacer columns with the + (logic)
                        Grid.SetRow(errorBox, row + (row - (row % 3)) / 3);
                        Grid.SetColumn(errorBox, col + (col - (col % 3)) / 3);

                        // add to grid item
                        EGrid.Children.Add(errorBox);
                    }
                }
            }
        }
    }
}
