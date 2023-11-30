namespace DokuApp.Model.UI
{
    public class CellData
    {
        private readonly int _value;
        public int Value { get { return _value; } }

        private readonly int[] _corners;
        public int[] Corners { get { return _corners; } }

        private readonly bool _permenant;
        public bool Permenant { get { return _permenant; } }

        public CellData (int value, int[] corners, bool permenant)
        {
            _value = value;
            _corners = corners;
            _permenant = permenant;
        }
    }
}
