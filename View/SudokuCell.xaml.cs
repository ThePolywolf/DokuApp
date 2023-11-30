using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using DokuApp.Model.UI;

namespace DokuApp.View
{
    public partial class SudokuCell : UserControl
    {
        public SudokuCell()
        {
            InitializeComponent();
        }

        public void SetCell(CellData data)
        {
            if (data.Permenant)
            {
                CellValue.Foreground = Brushes.Black;
            }
            else
            {
                CellValue.Foreground = Brushes.DarkBlue;
            }

            TextBlock[] Corners = new TextBlock[] {b1, b2, b3, b4, b5, b6, b7, b8, b9};

            foreach (TextBlock corner in Corners)
            {
                corner.Text = "";
            }

            // Set Cell value or corner values, not both
            if (data.Value > 0 && data.Value <= 9)
            {
                CellValue.Text = data.Value.ToString();

                return;
            }

            CellValue.Text = "";

            for (int i = 0; i < 9; i++)
            {
                if (data.Corners.Contains(i + 1))
                {
                    Corners[i].Text = (i + 1).ToString();
                }
            }
        }
    }
}
