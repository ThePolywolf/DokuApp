using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DokuApp.View
{
    public partial class EntrySlider : UserControl
    {
        public event EventHandler<bool>? NewEntryMode;

        public EntrySlider()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(
                "IsActive",
                typeof(bool),
                typeof(EntrySlider),
                new PropertyMetadata(false, OnActivationChanged));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        private static void OnActivationChanged( DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntrySlider slider = (EntrySlider)d;
            bool isActive = (bool)e.NewValue;

            if (isActive)
            {
                slider.Box.Fill = Brushes.Green;
                slider.Box.VerticalAlignment = VerticalAlignment.Top;
            }
            else
            {
                slider.Box.Fill = Brushes.Red;
                slider.Box.VerticalAlignment = VerticalAlignment.Bottom;
            }
        }

        private void ChangeActivation(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsActive = !IsActive;

            NewEntryMode?.Invoke(this, IsActive);
        }
    }
}
