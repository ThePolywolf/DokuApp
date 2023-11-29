using DokuApp.Model.UI;
using System.Windows.Controls;

namespace DokuApp.View
{
    public partial class SudokuRow : UserControl
    {
        public SudokuRow()
        {
            InitializeComponent();
        }

        public void SetRow(CellData[] cellData)
        {
            SudokuCell[] cell = new SudokuCell[] { c1, c2, c3, c4, c5, c6, c7, c8, c9 };

            for (int i = 0; i < 9; i++)
            {
                // fill in missing data with blank data
                if (cellData.Length <= i)
                {
                    cell[i].SetCell(new CellData(0, new int[] {}));
                    continue;
                }

                cell[i].SetCell(cellData[i]);
            }
        }
    }
}
