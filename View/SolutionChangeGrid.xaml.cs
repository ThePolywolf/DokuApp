using System.Windows.Controls;

namespace DokuApp.View
{
    public partial class SolutionChangeGrid : UserControl
    {
        private readonly SolutionChangeRow[] _rows;

        public SolutionChangeGrid()
        {
            InitializeComponent();

            _rows = new SolutionChangeRow[] { Row1, Row2, Row3, Row4, Row5, Row6, Row7, Row8, Row9 };
        }

        public void SetChangesGrid(bool[,] changes)
        {
            for (int rowNum = 0; rowNum < _rows.Length; rowNum++)
            {
                bool[] values = new bool[9];

                for (int colNum = 0; colNum < 9; colNum++)
                {
                    try
                    {
                        values[colNum] = changes[colNum, rowNum];
                    }
                    catch
                    {
                        values[colNum] = false;
                    }
                }

                _rows[rowNum].SetCellVisibility(values);
            }
        }
    }
}
