namespace DokuApp.Model.UI
{
    public class CellData
    {
        private int _value;
        public int Value { get { return _value; } }

        private int[] _corners;
        public int[] Corners { get { return _corners; } }

        private bool _permenant;
        public bool Permenant { get { return _permenant; } }

        public CellData (int value, int[] corners, bool permenant)
        {
            _value = value;
            _corners = corners;
            _permenant = permenant;
        }
    }
}
