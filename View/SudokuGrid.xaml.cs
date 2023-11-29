using DokuApp.Model.UI;
using System;
using System.Windows.Controls;

namespace DokuApp.View
{
    public partial class SudokuGrid : UserControl
    {
        public SudokuGrid()
        {
            InitializeComponent();
        }

        public void SetGrid(CellData[][] cellData)
        {
            SudokuRow[] row = new SudokuRow[] { r1, r2, r3, r4, r5, r6, r7, r8, r9 };

            for (int i = 0; i < 9; i++)
            {
                if (i >= cellData.Length)
                {
                    row[i].SetRow(Array.Empty<CellData>());
                    continue;
                }

                row[i].SetRow(cellData[i]);
            }
        }
    }
}
