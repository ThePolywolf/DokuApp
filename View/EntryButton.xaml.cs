using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DokuApp.View
{
    public partial class EntryButton : UserControl
    {
        public EntryButton()
        {
            InitializeComponent();
        }

        public void SetActivity(bool isActive)
        {
            if (isActive)
            {
                FillBox.Fill = Brushes.White;
            }

            else
            {
                FillBox.Fill = Brushes.DarkGray;
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
    }
}
