namespace DokuApp.Model.UI
{
    public class CellData
    {
        private int _value;
        public int Value { get { return _value; } }

        private int[] _corners;
        public int[] Corners { get { return _corners; } }

        public CellData (int value, int[] corners)
        {
            _value = value;
            _corners = corners;
        }
    }
}
