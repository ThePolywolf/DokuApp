using DokuApp.Model.Builder;
using System;

namespace DokuApp.Model.Data
{
    internal class UserSelection
    {
        private int[] _singleSelection;
        private LogicMatrix _selected;

        public Tuple<int, int> SingleSelection { get { return Tuple.Create(_singleSelection[0], _singleSelection[1]); } }
        public LogicMatrix Selected { get { return _selected; } }

        public UserSelection()
        {
            _singleSelection = new int[2] { 4, 4 };
            _selected = LogicBuilder.Cell(SingleSelection);
        }

        public void SingleSelect(Tuple<int, int> selection)
        {
            _singleSelection = new int[2] { selection.Item1, selection.Item2 };
            _selected = LogicBuilder.Cell(selection);
        }

        public void Left()
        {
            _singleSelection[0] = Math.Clamp(_singleSelection[0] - 1, 0, 8);
        }

        public void Right()
        {
            _singleSelection[0] = Math.Clamp(_singleSelection[0] + 1, 0, 8);
        }

        public void Up()
        {
            _singleSelection[1] = Math.Clamp(_singleSelection[1] - 1, 0, 8);
        }

        public void Down()
        {
            _singleSelection[1] = Math.Clamp(_singleSelection[1] + 1, 0, 8);
        }

        // can add multi-select option in here
    }
}
