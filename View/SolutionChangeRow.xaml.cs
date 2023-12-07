using System.Windows.Controls;

namespace DokuApp.View
{
    public partial class SolutionChangeRow : UserControl
    {
        private readonly SolutionChangeCell[] _cells;

        public SolutionChangeRow()
        {
            InitializeComponent();

            _cells = new SolutionChangeCell[] {  Box1, Box2, Box3, Box4, Box5, Box6, Box7, Box8, Box9 };
        }

        public void SetCellVisibility(bool[] isVisible)
        {
            for (int i = 0; i < 9; i++)
            {
                if (i >= isVisible.Length)
                {
                    _cells[i].Visibility = System.Windows.Visibility.Hidden;
                    continue;
                }

                if (isVisible[i])
                {
                    _cells[i].Visibility = System.Windows.Visibility.Visible;
                    continue;
                }

                _cells[i].Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
