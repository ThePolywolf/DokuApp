using DokuApp.Model.Solver;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DokuApp.View
{
    public partial class SolverDock : UserControl
    {
        private readonly List<Strategy> _strategies;
        public List<Strategy> Strategies { get { return _strategies; } }

        public SolverDock()
        {
            InitializeComponent();

            _strategies = new List<Strategy>();

            DataContext = this;
        }
    }
}
