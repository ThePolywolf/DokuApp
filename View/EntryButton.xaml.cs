using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DokuApp.View
{
    public partial class EntryButton : UserControl
    {
        public event EventHandler<int>? NumberButtonPressed;

        public EntryButton()
        {
            InitializeComponent();
        }

        public void SetActivity(bool isActive)
        {
            if (isActive)
            {
                NumberBox.Background = Brushes.White;
            }

            else
            {
                NumberBox.Background = Brushes.DarkGray;
            }
        }

        public static readonly DependencyProperty NumberTextProperty =
            DependencyProperty.Register(
                "NumberText",
                typeof(string),
                typeof(EntryButton),
                new PropertyMetadata(string.Empty, OnNumberTextChanged));

        public string NumberText
        {
            get { return (string)GetValue(NumberTextProperty); }
            set { SetValue(NumberTextProperty, value); }
        }

        private static void OnNumberTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryButton numberBox = (EntryButton)d;
            numberBox.textBox.Text = (string)e.NewValue;
        }

        private void ButtonPressed(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(NumberText, out int number))
            {
                number = 0;
            }

            NumberButtonPressed?.Invoke(this, number);
        }

        public void Shift(bool doShift)
        {
            if (doShift)
            {
                textBox.FontSize = 25;
            }
            else
            {
                textBox.FontSize = 60;
            }
        }
    }
}
