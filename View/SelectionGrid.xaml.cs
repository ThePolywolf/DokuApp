using System;
using System.Windows.Controls;

namespace DokuApp.View
{
    public partial class SelectionGrid : UserControl
    {
        public SelectionGrid()
        {
            InitializeComponent();
        }

        public void SelectSquare(int column, int row)
        {
            // re-show
            SingleBox.Visibility = System.Windows.Visibility.Visible;

            column = Math.Clamp(column, 0, 8);
            row = Math.Clamp(row, 0, 8);

            // set viewColumn to skip spacer columns
            int viewColumn = column;
            if (column >= 3)
            {
                viewColumn++;
            }
            if (column >= 6)
            {
                viewColumn++;
            }

            int viewRow = row;
            if (row >= 3)
            {
                viewRow++;
            }
            if (row >= 6)
            {
                viewRow++;
            }

            Grid.SetRow(SingleBox, viewRow);
            Grid.SetColumn(SingleBox, viewColumn);
        }

        public void HideSelection()
        {
            SingleBox.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
