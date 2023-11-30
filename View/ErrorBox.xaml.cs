using System.Windows.Controls;

namespace DokuApp.View
{
    public partial class ErrorBox : UserControl
    {
        public ErrorBox()
        {
            InitializeComponent();
        }

        public void SetVisibility(bool isVisible)
        {
            if (isVisible)
            {
                Box.Visibility = System.Windows.Visibility.Visible;
            }

            else
            {
                Box.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
