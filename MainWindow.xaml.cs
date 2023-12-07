using DokuApp.Model.UI;
using System.Windows;
using System.Windows.Input;
using System;

namespace DokuApp
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowMVVM vm;

        public event EventHandler<KeyEventArgs>? WindowKeyDown;
        public event EventHandler<KeyEventArgs>? WindowKeyUp;

        public event EventHandler<RoutedEventArgs>? WindowSolveGrid;
        public event EventHandler<RoutedEventArgs>? WindowMarkCorners;
        public event EventHandler<RoutedEventArgs>? WindowStepSolution;

        public event EventHandler<RoutedEventArgs>? WindowClearGrid;
        public event EventHandler<RoutedEventArgs>? WindowTotalClearGrid;
        public event EventHandler<RoutedEventArgs>? WindowClearPossibilitiesGrid;
        public event EventHandler<RoutedEventArgs>? WindowClearNumberGrid;

        public MainWindow()
        {
            InitializeComponent();

            vm = new MainWindowMVVM(this);
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            WindowKeyDown?.Invoke(sender, e);
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            WindowKeyUp?.Invoke(sender, e);
        }

        private void SolveGrid(object sender, RoutedEventArgs e)
        {
            WindowSolveGrid?.Invoke(sender, e);
        }

        private void ClearGrid(object sender, RoutedEventArgs e)
        {
            WindowClearGrid?.Invoke(sender, e);
        }

        private void FullClearGrid(object sender, RoutedEventArgs e)
        {
            WindowTotalClearGrid?.Invoke(sender, e);
        }

        private void ClearNumbers(object sender, RoutedEventArgs e)
        {
            WindowClearNumberGrid?.Invoke(sender, e);
        }

        private void ClearPossibilities(object sender, RoutedEventArgs e)
        {
            WindowClearPossibilitiesGrid?.Invoke(sender, e);
        }

        private void MarkCorners(object sender, RoutedEventArgs e)
        {
            WindowMarkCorners?.Invoke(sender, e);
        }

        private void SolveStep(object sender, RoutedEventArgs e)
        {
            WindowStepSolution?.Invoke(sender, e);
        }
    }
}
