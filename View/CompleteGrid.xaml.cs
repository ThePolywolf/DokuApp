using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DokuApp.View
{
    public partial class CompleteGrid : UserControl
    {
        public event EventHandler<Tuple<int, int>>? MouseSelection;

        public CompleteGrid()
        {
            InitializeComponent();
        }

        private void GridClicked(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);

            double percentageX = (mousePosition.X / ActualWidth);
            double percentageY = (mousePosition.Y / ActualHeight);

            Debug.WriteLine($"X: {percentageX} Y: {percentageY}");

            int row = (int)Math.Floor(percentageY * 9);
            int column = (int)Math.Floor(percentageX * 9);

            if (row < 0 || row > 8 || column < 0 || column > 8)
            {
                return;
            }

            Tuple<int, int> selection = Tuple.Create(column, row);
            MouseSelection?.Invoke(this, selection);
        }
    }
}
